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
using GSID.Model.ExtraEntities;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace GSID.Admin.Controllers
{
    public class QuickLinkController : BaseAuthenticationController
    {
        private readonly IQuickLinkService quickLinkService;
        private readonly IParameterService paraService;
        // GET: Customer
        public QuickLinkController(IQuickLinkService _quickLinkService,
                                IParameterService _paraService)
        {
            quickLinkService = _quickLinkService;
            paraService = _paraService;
        }

        [RBAC]
        public ActionResult Index(string p)
        {
            var model = quickLinkService.GetAll();

            PageIndex = p.ConvertIntPaging();
            ViewBag.TotalPage = (Math.Ceiling((double)model.Count / PageSize));
            ViewBag.CurrentPage = PageIndex;
            ViewBag.PageVisit = PageVisit;
            ViewBag.PageSize = PageSize;
            ViewBag.CountTotal = model.Count();
            model = model.Skip(PageSize * (PageIndex - 1))
                                    .Take(PageSize)
                                        .OrderByDescending(c => c.EditedByDate ?? c.AddedByDate ?? DateTime.MaxValue)
                                            .ToList();
            ViewBag.ActiveMenu = RBACUser.RootPermissionId(Request);

            return View(model);
        }

        [RBAC]
        public ActionResult Create()
        {
            QuickLinkCreateViewModel model = new QuickLinkCreateViewModel();
            model.Sort          = 1;
            model.IsDeleted     = true;
            model.QuickLinks = quickLinkService.GetAllByParent("");
            ViewBag.ActiveMenu = RBACUser.RootPermissionId(Request);
            return View(model);
        }

        [HttpPost]
        [RBAC]
        public ActionResult Create([Bind(Include = "ParentId, NameVn, NameEn, SlugVn, SlugEn, DescriptionVn, DescriptionEn, Sort, IsDeleted")]QuickLinkCreateViewModel obj)
        {
            string title = Message.TITLE_ERROR;
            string message = Message.CONTENT_POSTDATA_DEFAUT_ERROR;
            string status = Default.Status_Error;
            string id = string.Empty;
            try
            {
                if (!RBACUser.HasPermission("RecycleBin", "QuickLink"))
                    ModelState.Remove("IsDeleted");
                if (ModelState.IsValid)
                {
                    QuickLink model = Mapper.Map<QuickLinkCreateViewModel, QuickLink>(obj);
                    model.ParentId  = string.IsNullOrEmpty(obj.ParentId) ? "" : obj.ParentId;
                    model.IsDeleted = !RBACUser.HasPermission("RecycleBin", "QuickLink") ? false : !obj.IsDeleted;
                    model.AddedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
                    model.AddedByDate = DateTime.Now;
                    id = quickLinkService.Create(model);
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
            var model = Mapper.Map<QuickLink, QuickLinkEditViewModel>(quickLinkService.GetBy(gsid));
            if (model == null)
                Response.Redirect(string.Format("/Error/NotFound?url={0}", Request.Url), true);
            model.IsDeleted = !model.IsDeleted;
            model.QuickLinks = quickLinkService.GetAllByParent("");
            ViewBag.ActiveMenu = RBACUser.RootPermissionId(Request);
            return View(model);
        }

        [HttpPost]
        [RBAC]
        public ActionResult Update( [Bind(Include = "Id, ParentId, NameVn, NameEn, SlugVn, SlugEn, DescriptionVn, DescriptionEn, Sort, IsDeleted")]QuickLinkEditViewModel obj)
        {
            string title = Message.TITLE_ERROR;
            string message = Message.CONTENT_POSTDATA_DEFAUT_ERROR;
            string status = Default.Status_Error;
            try
            {
                if (!RBACUser.HasPermission("RecycleBin", "QuickLink"))
                    ModelState.Remove("IsDeleted");
                if (ModelState.IsValid)
                {
                    var model = quickLinkService.GetBy(obj.Id);
                    if (model != null)
                    {
                        model.ParentId      = string.IsNullOrEmpty(obj.ParentId) ? "" : obj.ParentId;
                        model.NameVn        = obj.NameVn;
                        model.NameEn        = obj.NameEn;
                        model.SlugVn        = obj.SlugVn;
                        model.SlugEn        = obj.SlugEn;
                        model.DescriptionVn = obj.DescriptionVn;
                        model.DescriptionEn = obj.DescriptionEn;
                        model.Sort          = obj.Sort;
                        if (RBACUser.HasPermission("RecycleBin", "QuickLink"))
                            model.IsDeleted = !obj.IsDeleted;
                        model.EditedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
                        quickLinkService.Update(model);

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

            var _hasDelete = quickLinkService.Delete(id);
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

            var _hasRecycleBin = quickLinkService.GetBy(id);
            _hasRecycleBin.DeletedByDate = DateTime.Now;
            _hasRecycleBin.DeletedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
            _hasRecycleBin.IsDeleted = !isDeleted;
            quickLinkService.Update(_hasRecycleBin);

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
            return Json(quickLinkService.IsNameVnAvailable(NameVn) != null ? false : true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult IsNameVnIdAvailable(string NameVn, string Id)
        {
            // Check if the NameVn already exists
            return Json(quickLinkService.IsNameVnAvailable(NameVn, Id) != null ? false : true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult IsNameEnAvailable(string NameEn)
        {
            // Check if the NameEn already exists
            return Json(quickLinkService.IsNameEnAvailable(NameEn) != null ? false : true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult IsNameEnIdAvailable(string NameEn, string Id)
        {
            // Check if the NameEn already exists
            return Json(quickLinkService.IsNameEnAvailable(NameEn, Id) != null ? false : true, JsonRequestBehavior.AllowGet);
        }
    }
}