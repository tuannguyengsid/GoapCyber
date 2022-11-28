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
    public class PositionController : BaseAuthenticationController
    {
        private readonly IPositionService positionService;
        private readonly IDepartmentService departmentService;
        private readonly ISiteService siteService;
        // GET: District
        public PositionController(IPositionService _positionService,
                                        IDepartmentService _departmentService,
                                        ISiteService _siteService)
        {
            positionService = _positionService;
            departmentService = _departmentService;
            siteService = _siteService;
        }

        [RBAC]
        public async Task<ActionResult> Index(string p)
        {
            PositionFilterViewModel model = new PositionFilterViewModel();
            ViewBag.ActiveMenu = RBACUser.RootPermissionId(Request);

            return View(model);
        }

        public async Task<ActionResult> PartialIndex(string p, string Keyword, string BeginAddDateString, string EndAddDateString)
        {
            var _beginAddDate = ConvertDateTimeIsNull(BeginAddDateString);
            var _endAdddDate = ConvertDateTimeIsNull(EndAddDateString);
            var model = positionService.GetAllBySearch(Keyword, _beginAddDate, _endAdddDate);

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
            ViewBag.ActiveMenu = RBACUser.RootPermissionId(Request);

            return PartialView(model);
        }

        [RBAC]
        public ActionResult Create()
        {
            PositionCreateViewModel model = new PositionCreateViewModel();
            //model.Sites = siteService.GetAll(false);
            model.Departments = departmentService.GetAll();
            model.IsDeleted = true;
            return View(model);
        }

        [HttpPost]
        [RBAC]
        public ActionResult Create([Bind(Include = "NameVn, NameEn, Description, DepartmentId, IsDeleted")]PositionCreateViewModel obj)
        {
            string title = Message.TITLE_ERROR;
            string message = Message.CONTENT_POSTDATA_DEFAUT_ERROR;
            string status = Default.Status_Error;
            string id = string.Empty;
            try
            {
                if (!RBACUser.HasPermission("RecycleBin", "Position"))
                    ModelState.Remove("IsDeleted");
                if (ModelState.IsValid)
                {
                    Position model = Mapper.Map<PositionCreateViewModel, Position>(obj);
                    model.IsDeleted = !RBACUser.HasPermission("RecycleBin", "Position") ? false : !obj.IsDeleted;
                    model.AddedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
                    id = positionService.Create(model);
                    title = Message.TITLE_REPORT;
                    message = Message.CONTENT_POSTDATA_CREATE_SUCCESSFULL;
                    status = Default.Status_Sucessfull;
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
            var model = Mapper.Map<Position, PositionEditViewModel>(positionService.GetBy(gsid));
            if (model == null)
                Response.Redirect(string.Format("/Error/NotFound?url={0}", Request.Url), true);

            model.Departments = departmentService.GetAll(false);

            model.IsDeleted = !model.IsDeleted;
            ViewBag.ActiveMenu = RBACUser.RootPermissionId(Request);
            return View(model);
        }

        [HttpPost]
        [RBAC]
        public ActionResult Update([Bind(Include = "Id, NameVn, NameEn, DepartmentId, IsDeleted")]PositionEditViewModel obj)
        {
            string title = Message.TITLE_ERROR;
            string message = Message.CONTENT_POSTDATA_DEFAUT_ERROR;
            string status = Default.Status_Error;
            try
            {
                if (!RBACUser.HasPermission("RecycleBin", "Position"))
                    ModelState.Remove("IsDeleted");
                if (ModelState.IsValid)
                {
                    var model = positionService.GetBy(obj.Id);
                    if (model != null)
                    {
                        model.NameVn = obj.NameVn;
                        model.NameEn = obj.NameEn;
                        model.DepartmentId = obj.DepartmentId;
                        if (RBACUser.HasPermission("RecycleBin", "Position"))
                            model.IsDeleted = !obj.IsDeleted;
                        model.EditedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
                        positionService.Update(model);

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

            var _hasDelete = positionService.Delete(id);
            if (_hasDelete)
                status = ((int)StatusDelete.Deleted).ToString();

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

            var _hasRecycleBin = positionService.GetBy(id);
            _hasRecycleBin.DeletedByDate = DateTime.Now;
            _hasRecycleBin.DeletedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
            _hasRecycleBin.IsDeleted = !isDeleted;
            positionService.Update(_hasRecycleBin);

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

        [HttpPost]
        public JsonResult IsNameVnAvailable(string NameVn)
        {
            // Check if the NameVn already exists
            return Json(positionService.IsNameVnAvailable(NameVn) != null ? false : true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult IsNameVnIdAvailable(string NameVn, string Id)
        {
            // Check if the NameVn already exists
            return Json(positionService.IsNameVnAvailable(NameVn, Id) != null ? false : true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult IsNameEnAvailable(string NameEn)
        {
            // Check if the NameEn already exists
            return Json(positionService.IsNameEnAvailable(NameEn) != null ? false : true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult IsNameEnIdAvailable(string NameEn, string Id)
        {
            // Check if the NameEn already exists
            return Json(positionService.IsNameEnAvailable(NameEn, Id) != null ? false : true, JsonRequestBehavior.AllowGet);
        }
    }
}