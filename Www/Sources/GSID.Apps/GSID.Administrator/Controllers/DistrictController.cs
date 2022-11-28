using AutoMapper;
using GSID.Model.Enums;
using GSID.Model.MongodbModels;
using GSID.Service;
using GSID.Setting;
using GSID.Setting.FormKeys;
using GSID.Web.Core.Authentication;
using GSID.Web.Core.Extensions;
using GSID.Admin.Helpers;
using GSID.Admin.ViewModels.MongoModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GSID.Service.MongoRepositories.Service;
using System.Threading.Tasks;

namespace GSID.Admin.Controllers
{
    public class DistrictController : BaseAuthenticationController
    {
        private readonly IDistrictService districtService;
        private readonly ICountryService countryService;
        private readonly IProvinceService provinService;
        // GET: District
        public DistrictController(IDistrictService _districtService,
                                    ICountryService _countryService,
                                    IProvinceService _provinService)
        {
            districtService = _districtService;
            countryService  = _countryService;
            provinService   = _provinService;
        }

        [RBAC]
        public async Task<ActionResult> Index(string p)
        {
            DistrictFilterViewModel model = new DistrictFilterViewModel();
            model.Countries = countryService.GetAll(false);
            model.Provinces = new List<Province>();
            ViewBag.ActiveMenu = RBACUser.RootPermissionId(Request);

            return View(model);
        }

        public async Task<ActionResult> PartialIndex(string p, string Keyword, string BeginAddDateString, string EndAddDateString, string[] CountryId, string[] ProvinceId)
        {
            var _beginAddDate = ConvertDateTimeIsNull(BeginAddDateString);
            var _endAdddDate = ConvertDateTimeIsNull(EndAddDateString);
            var model = districtService.GetAllBySearch(Keyword, _beginAddDate, _endAdddDate, CountryId, ProvinceId);

            PageIndex = p.ConvertIntPaging();
            ViewBag.TotalPage = (Math.Ceiling((double)model.Count / PageSize));
            ViewBag.CurrentPage = PageIndex;
            ViewBag.PageVisit = PageVisit;
            ViewBag.PageSize = PageSize;
            ViewBag.CountTotal = model.Count();
            model = model.Skip(PageSize * (PageIndex - 1))
                                    .Take(PageSize)
                                        .OrderBy(c => c.NameVn)
                                            .ToList();

            return PartialView(model);
        }
        public ActionResult FormFilterProvinceByCountryType(string Country)
        {
            DistrictFormFilterProvinceViewModel model = new DistrictFormFilterProvinceViewModel();
            model.Provinces = provinService.GetAllByCountryIsDeleted(Country, false);
            return PartialView(model);
        }

        [RBAC]
        public ActionResult Create()
        {
            DistrictCreateViewModel model = new DistrictCreateViewModel();
            model.Countries = countryService.GetAll(false);
            model.Provinces = new List<Province>();
            model.IsDeleted = true;
            ViewBag.ActiveMenu = RBACUser.RootPermissionId(Request);
            return View(model);
        }

        [HttpPost]
        [RBAC]
        public ActionResult Create([Bind(Include = "NameVn, NameEn, CountryId, ProvinceId, IsDeleted")]DistrictCreateViewModel obj)
        {
            string title = Message.TITLE_ERROR;
            string message = Message.CONTENT_POSTDATA_DEFAUT_ERROR;
            string status = Default.Status_Error;
            string id = string.Empty;
            try
            {
                if (!RBACUser.HasPermission("RecycleBin", "District"))
                    ModelState.Remove("IsDeleted");
                if (ModelState.IsValid)
                {
                    District model = Mapper.Map<DistrictCreateViewModel, District>(obj);
                    model.IsDeleted = !RBACUser.HasPermission("RecycleBin", "District") ? false : !obj.IsDeleted;
                    model.AddedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
                    id = districtService.Create(model);
                    title   = Message.TITLE_REPORT;
                    message = Message.CONTENT_POSTDATA_CREATE_SUCCESSFULL;
                    status  = Default.Status_Sucessfull;
                }
                else
                {
                    var messageError = string.Join(" | ", ModelState.Values
                                                  .SelectMany(v => v.Errors)
                                                  .Select(e => e.ErrorMessage));

                    //Log This exception to ELMAH:
                    //Exception exception = new Exception(message.ToString());
                    ////Return Status Code:
                    //return new HttpStatusCodeResult(HttpStatusCode.BadRequest, message);
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                message = Message.CONTENT_CATCH_RetryLimitExceededException;
            }

            return Json(new
            {
                Id = id,
                Title = title,
                Message = message,
                Status = status
            }, JsonRequestBehavior.AllowGet);
        }

        [RBAC]
        public ActionResult Update(string gsid)
        {
            var model = Mapper.Map<District, DistrictEditViewModel>(districtService.GetBy(gsid));
            if (model == null)
                Response.Redirect(string.Format("/Error/NotFound?url={0}", Request.Url), true);

            model.Countries = countryService.GetAll(false);
            model.Provinces = !string.IsNullOrEmpty(model.CountryId) ? provinService.GetAllByCountryIsDeleted(model.CountryId, false) : new List<Province>();
            model.IsDeleted = !model.IsDeleted;
            ViewBag.ActiveMenu = RBACUser.RootPermissionId(Request);

            return View(model);
        }

        [HttpPost]
        [RBAC]
        public ActionResult Update([Bind(Include = "Id, NameVn, NameEn, CountryId, ProvinceId, IsDeleted")]DistrictEditViewModel obj)
        {
            string title = Message.TITLE_ERROR;
            string message = Message.CONTENT_POSTDATA_DEFAUT_ERROR;
            string status = Default.Status_Error;
            try
            {
                if (!RBACUser.HasPermission("RecycleBin", "District"))
                    ModelState.Remove("IsDeleted");
                if (ModelState.IsValid)
                {
                    var model = districtService.GetBy(obj.Id);
                    if (model != null)
                    {
                        model.NameVn = obj.NameVn;
                        model.NameEn = obj.NameEn;
                        model.CountryId     = obj.CountryId;
                        model.ProvinceId    = obj.ProvinceId;
                        if (RBACUser.HasPermission("RecycleBin", "District"))
                            model.IsDeleted = !obj.IsDeleted;
                        model.EditedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
                        districtService.Update(model);

                        title = Message.TITLE_REPORT;
                        message = Message.CONTENT_POSTDATA_UPDATE_SUCCESSFULL;
                        status = Default.Status_Sucessfull;
                    }
                }
                else
                {
                    var messageError = string.Join(" | ", ModelState.Values
                                                  .SelectMany(v => v.Errors)
                                                  .Select(e => e.ErrorMessage));

                    //Log This exception to ELMAH:
                    //Exception exception = new Exception(message.ToString());
                    ////Return Status Code:
                    //return new HttpStatusCodeResult(HttpStatusCode.BadRequest, message);
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                message = Message.CONTENT_CATCH_RetryLimitExceededException;
            }

            return Json(new
            {
                Title = title,
                Message = message,
                Status = status
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [RBAC]
        public ActionResult Delete(string id)
        {
            string status = ((int)StatusDelete.Error).ToString();
            string message = Message.CONTENT_POSTDATA_DELETE_ERROR_OR_EMPTY;
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var _hasDelete = districtService.Delete(id);
            if (_hasDelete.HasValue && _hasDelete.Value)
                status = ((int)StatusDelete.Deleted).ToString();

            return Json(new
            {
                Status = status,
                Message = message
            });
        }

        public ActionResult FormFilterDistrictByProvinceType1(string Province)
        {
            DistrictFilterModel model = new DistrictFilterModel();
            model.Districts = districtService.GetAllByProvinceIsDeleted(Province, false);
            return PartialView(model);
        }

        [HttpPost]
        [RBAC]
        public ActionResult RecycleBin(string id, bool isDeleted)
        {
            string title = Message.TITLE_ERROR;
            string status = ((int)StatusDelete.Error).ToString();
            string message = Message.CONTENT_POSTDATA_DELETE_ERROR_OR_EMPTY;
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var _hasRecycleBin = districtService.GetBy(id);
            _hasRecycleBin.DeletedByDate = DateTime.Now;
            _hasRecycleBin.DeletedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
            _hasRecycleBin.IsDeleted = !isDeleted;
            districtService.Update(_hasRecycleBin);

            title = Message.TITLE_REPORT;
            message = Message.CONTENT_POSTDATA_UPDATE_SUCCESSFULL;
            status = Default.Status_Sucessfull;

            return Json(new
            {
                Title = title,
                Message = message,
                Status = status
            }, JsonRequestBehavior.AllowGet);
        }
    }
}