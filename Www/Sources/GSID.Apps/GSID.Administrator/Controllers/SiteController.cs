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
using GSID.Model.ExtraEntities;
using Newtonsoft.Json;
using GSID.Service.MongoRepositories.Service;
using System.Threading.Tasks;

namespace GSID.Admin.Controllers
{
    public class SiteController : BaseAuthenticationController
    {
        private readonly ISiteService siteService;
        private readonly ICountryService countryService;
        private readonly IProvinceService provinceService;
        private readonly IDistrictService districtService;
        private readonly IParameterService paraService;
        private readonly IRecruitmentService recruitmentService;
        SiteInformationConfig paraSite;
        // GET: District
        public SiteController(ISiteService _siteService,
                                ICountryService _countryService,
                                IProvinceService _provinceService,
                                IDistrictService _districtService,
                                IParameterService _paraService,
             IRecruitmentService _recruitmentService)
        {
            siteService = _siteService;
            countryService = _countryService;
            provinceService = _provinceService;
            districtService = _districtService;
            paraService = _paraService;
            recruitmentService = _recruitmentService;
        }

        [RBAC]
        public async Task<ActionResult> Index(string p)
        {
            SiteFilterViewModel model = new SiteFilterViewModel();
            ViewBag.ActiveMenu = RBACUser.RootPermissionId(Request);

            return View(model);
        }

        public async Task<ActionResult> PartialIndex(string p, string Keyword, string BeginAddDateString, string EndAddDateString)
        {
            var _beginAddDate = ConvertDateTimeIsNull(BeginAddDateString);
            var _endAdddDate = ConvertDateTimeIsNull(EndAddDateString);
            var model = siteService.GetAllBySearch(Keyword, _beginAddDate, _endAdddDate);

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

        [RBAC]
        public ActionResult Create()
        {
            SiteCreateViewModel model = new SiteCreateViewModel();

            model.Countries = countryService.GetAll(false);
            model.Provinces = new List<Province>();
            model.Districts = new List<District>();
            model.IsDeleted = true;
            ViewBag.ActiveMenu = RBACUser.RootPermissionId(Request);
            return View(model);
        }

        [HttpPost]
        [RBAC]
        public ActionResult Create([Bind(Include = " NameVn, NameEn, DescriptionVn, DescriptionEn, AddressVn, AddressEn, CountryId, ProvinceId, DistrictId, PhoneNumber, Email, IsDeleted")]SiteCreateViewModel obj)
        {
            string title = Message.TITLE_ERROR;
            string message = Message.CONTENT_POSTDATA_DEFAUT_ERROR;
            string status = Default.Status_Error;
            string id = string.Empty;
            try
            {
                if (!RBACUser.HasPermission("RecycleBin", "Site"))
                    ModelState.Remove("IsDeleted");
                if (ModelState.IsValid)
                {
                    Site model = Mapper.Map<SiteCreateViewModel, Site>(obj);
                    model.IsDeleted = !RBACUser.HasPermission("RecycleBin", "Site") ? false : !obj.IsDeleted;
                    model.AddedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
                    id      = siteService.Create(model);
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
            var model = Mapper.Map<Site, SiteEditViewModel>(siteService.GetBy(gsid));
            if (model == null)
                Response.Redirect(string.Format("/Error/NotFound?url={0}", Request.Url), true);

            model.Countries = countryService.GetAll(false);
            model.Provinces = !string.IsNullOrEmpty(model.CountryId) ? provinceService.GetAllByCountryIsDeleted(model.CountryId, false) : new List<Province>();
            model.Districts = !string.IsNullOrEmpty(model.ProvinceId) ? districtService.GetAllByProvinceIsDeleted(model.ProvinceId, false) : new List<District>();
            model.IsDeleted = !model.IsDeleted;
            ViewBag.ActiveMenu = RBACUser.RootPermissionId(Request);
            return View(model);
        }

        [HttpPost]
        [RBAC]
        public ActionResult Update([Bind(Include = "Id, NameVn, NameEn, DescriptionVn, DescriptionEn, AddressVn, AddressEn, CountryId, ProvinceId, DistrictId, PhoneNumber, Email, IsDeleted")]SiteEditViewModel obj)
        {
            string title = Message.TITLE_ERROR;
            string message = Message.CONTENT_POSTDATA_DEFAUT_ERROR;
            string status = Default.Status_Error;
            try
            {
                if (!RBACUser.HasPermission("RecycleBin", "Site"))
                    ModelState.Remove("IsDeleted");
                if (ModelState.IsValid)
                {
                    var model = siteService.GetBy(obj.Id);
                    if (model != null)
                    {
                        model.NameVn = obj.NameVn;
                        model.NameEn = obj.NameEn;
                        model.DescriptionVn = obj.DescriptionVn;
                        model.DescriptionEn = obj.DescriptionEn;
                        model.AddressVn = obj.AddressVn;
                        model.AddressEn = obj.AddressEn;
                        model.CountryId = obj.CountryId;
                        model.ProvinceId = obj.ProvinceId;
                        model.DistrictId = obj.DistrictId;
                        model.PhoneNumber = obj.PhoneNumber;
                        model.Email = obj.Email;
                        if (RBACUser.HasPermission("RecycleBin", "Site"))
                            model.IsDeleted = !obj.IsDeleted;
                        model.EditedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
                        siteService.Update(model);

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

            var recrs = recruitmentService.GetAllByRecruitmentSite(id);
            if (recrs.Count <= 0)
            {
                var _hasDelete = siteService.Delete(id);
                status = ((int)StatusDelete.Deleted).ToString();
            }
            else
            {
                message = string.Format(Message.CONTENT_POSTDATA_DELETE_ERROR_OR_ERROR_EXIST_DATA, "Văn phòng", "tin tuyển dụng");
            }

            return Json(new
            {
                Status = status,
                Message = message
            });
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

            var _hasRecycleBin = siteService.GetBy(id);
            _hasRecycleBin.DeletedByDate = DateTime.Now;
            _hasRecycleBin.DeletedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
            _hasRecycleBin.IsDeleted = !isDeleted;
            siteService.Update(_hasRecycleBin);

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