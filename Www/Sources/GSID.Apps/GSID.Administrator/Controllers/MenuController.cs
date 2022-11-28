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
using System.Web.Mvc;
using GSID.Service.MongoRepositories.Service;
using System.Threading.Tasks;

namespace GSID.Admin.Controllers
{
    public partial class MenuController : BaseAuthenticationController
    {
        private readonly IParameterService paraService;
        private readonly IMenuNodeService menuNodeService;
        private readonly IProductService prdService;
        // GET: MenuNode
        public MenuController(IMenuNodeService _menuNodeService,
                                    IProductService _prdService,
                                    IParameterService _paraService)
        {
            menuNodeService = _menuNodeService;
            prdService      = _prdService;
            paraService     = _paraService;
        }

        [RBAC]
        public ActionResult Index(string p)
        {
            var model = menuNodeService.GetAllParent("");
            ViewBag.ActiveMenu = RBACUser.RootPermissionId(Request);
            return View(model);
        }

        [ChildActionOnly]
        public PartialViewResult PartialIndexChildren(string ParentId)
        {
            var model = menuNodeService.GetAllParent(ParentId);
            return PartialView(model);
        }

        [RBAC]
        public ActionResult Create()
        {
            MenuNodeCreateViewModel model = new MenuNodeCreateViewModel();
            model.SlugVn = model.SlugEn = "#";
            model.Sort = 0;
            model.IsDeleted = true;

            model.MenuNodes = new List<MenuNode>();

            var xxx = menuNodeService.GetAllParent("");
            foreach (var x in xxx)
            {
                var xx = new MenuNode();
                xx.ParentId = x.ParentId;
                xx.Id = x.Id;
                xx.NameVn = string.Format("{0}", x.NameVn);
                xx.NameEn = string.Format("{0}", x.NameEn);
                model.MenuNodes.Add(xx);
                var yyy = menuNodeService.GetAllParent(x.Id);
                foreach (var y in yyy)
                {
                    var yy = new MenuNode();
                    yy.Id = y.Id;
                    yy.ParentId = y.ParentId;
                    yy.NameVn = string.Format("__{0}", y.NameVn);
                    yy.NameEn = string.Format("__{0}", y.NameEn);
                    model.MenuNodes.Add(yy);
                }
            }
            ViewBag.ActiveMenu = RBACUser.RootPermissionId(Request);
            return View(model);
        }

        [HttpPost]
        [RBAC]
        public ActionResult Create([Bind(Include = "ParentId, NameVn, NameEn, SlugVn, SlugEn, Sort, IsDeleted")]MenuNodeCreateViewModel obj)
        {
            string title = Message.TITLE_ERROR;
            string message = Message.CONTENT_POSTDATA_DEFAUT_ERROR;
            string status = Default.Status_Error;
            string id = string.Empty;
            try
            {
                if (!RBACUser.HasPermission("RecycleBin", "MenuNode"))
                    ModelState.Remove("IsDeleted");
                if (ModelState.IsValid)
                {
                    MenuNode model = Mapper.Map<MenuNodeCreateViewModel, MenuNode>(obj);

                    if (string.IsNullOrEmpty(obj.ParentId))
                    {
                        model.ParentId = "";
                        model.Level = 1;
                    }
                    else if (!string.IsNullOrEmpty(obj.ParentId))
                    {
                        var objParrent = menuNodeService.GetBy(obj.ParentId);
                        model.ParentId = obj.ParentId;
                        model.Level = objParrent != null ? (!string.IsNullOrEmpty(objParrent.ParentId) ? 3 : 2) : 1;

                        objParrent.HasChild = true;
                        menuNodeService.Update(objParrent);
                    }

                    model.NameVn = !string.IsNullOrEmpty(obj.NameVn) ? obj.NameVn.Trim() : "";
                    model.NameEn = !string.IsNullOrEmpty(obj.NameEn) ? obj.NameEn.Trim() : "";
                    model.SlugVn = !string.IsNullOrEmpty(obj.SlugVn) ? obj.SlugVn.ToLower().Trim() : "";
                    model.SlugEn = !string.IsNullOrEmpty(obj.SlugEn) ? obj.SlugEn.ToLower().Trim() : "";
                    model.IsDeleted = !RBACUser.HasPermission("RecycleBin", "MenuNode") ? false : !obj.IsDeleted;
                    model.AddedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
                    id = menuNodeService.Create(model);

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
            var model = Mapper.Map<MenuNode, MenuNodeEditViewModel>(menuNodeService.GetBy(gsid));
            if (model == null)
                Response.Redirect(string.Format("/Error/NotFound?url={0}", Request.Url), true);

            model.IsDeleted = !model.IsDeleted;

            model.MenuNodes = new List<MenuNode>();
            var xxx = menuNodeService.GetAllParent("", gsid);
            foreach (var x in xxx)
            {
                var xx = new MenuNode();
                xx.ParentId = x.ParentId;
                xx.Id = x.Id;
                xx.NameVn = string.Format("{0}", x.NameVn);
                xx.NameEn = string.Format("{0}", x.NameEn);
                model.MenuNodes.Add(xx);
                var yyy = menuNodeService.GetAllParent(x.Id, gsid);
                foreach (var y in yyy)
                {
                    var yy = new MenuNode();
                    yy.Id = y.Id;
                    yy.ParentId = y.ParentId;
                    yy.NameVn = string.Format("__{0}", y.NameVn);
                    yy.NameEn = string.Format("__{0}", y.NameEn);
                    model.MenuNodes.Add(yy);
                }
            }
            ViewBag.ActiveMenu = RBACUser.RootPermissionId(Request);
            return View(model);
        }

        [HttpPost]
        [RBAC]
        public ActionResult Update([Bind(Include = "Id, ParentId, NameVn, NameEn, SlugVn, SlugEn, Sort, IsDeleted")]MenuNodeEditViewModel obj)
        {
            string title = Message.TITLE_ERROR;
            string message = Message.CONTENT_POSTDATA_DEFAUT_ERROR;
            string status = Default.Status_Error;
            try
            {
                if (!RBACUser.HasPermission("RecycleBin", "MenuNode"))
                    ModelState.Remove("IsDeleted");
                if (ModelState.IsValid)
                {
                    var model = menuNodeService.GetBy(obj.Id);
                    if (model != null)
                    {
                        if (string.IsNullOrEmpty(obj.ParentId))
                        {
                            model.ParentId = "";
                            model.Level = 1;
                        }
                        else if (!string.IsNullOrEmpty(obj.ParentId))
                        {
                            var objNewParrent = menuNodeService.GetBy(obj.ParentId);
                            if (model.ParentId != obj.ParentId)
                            {
                                if (!string.IsNullOrEmpty(model.ParentId))
                                {
                                    var objOldParrent = menuNodeService.GetBy(model.ParentId);
                                    var objParrents = menuNodeService.GetAllParent(model.ParentId).Where(c => c.Id != model.Id).ToList();
                                    objOldParrent.HasChild = objParrents.Count > 0 ? true : false;
                                    menuNodeService.Update(objOldParrent);
                                }

                                objNewParrent.HasChild = true;
                                menuNodeService.Update(objNewParrent);
                            }
                            model.ParentId = obj.ParentId;
                            model.Level = objNewParrent != null ? (!string.IsNullOrEmpty(objNewParrent.ParentId) ? 3 : 2) : 1;
                        }

                        var childs = menuNodeService.GetAllParent(model.Id);
                        model.HasChild = childs.Count > 0 ? true : false;

                        model.NameVn = !string.IsNullOrEmpty(obj.NameVn) ? obj.NameVn.Trim() : "";
                        model.NameEn = !string.IsNullOrEmpty(obj.NameEn) ? obj.NameEn.Trim() : "";
                        model.SlugVn = !string.IsNullOrEmpty(obj.SlugVn) ? obj.SlugVn.ToLower().Trim() : "";
                        model.SlugEn = !string.IsNullOrEmpty(obj.SlugEn) ? obj.SlugEn.ToLower().Trim() : "";
                        model.Sort      = obj.Sort;
                        if (RBACUser.HasPermission("RecycleBin", "MenuNode"))
                            model.IsDeleted = !obj.IsDeleted;
                        model.EditedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
                        menuNodeService.Update(model);

                        title   = Message.TITLE_REPORT;
                        message = Message.CONTENT_POSTDATA_UPDATE_SUCCESSFULL;
                        status  = Default.Status_Sucessfull;
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

            var _hasDelete = menuNodeService.Delete(id);
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

            var _hasRecycleBin = menuNodeService.GetBy(id);
            _hasRecycleBin.DeletedByDate = DateTime.Now;
            _hasRecycleBin.DeletedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
            _hasRecycleBin.IsDeleted = !isDeleted;
            menuNodeService.Update(_hasRecycleBin);

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
            return Json(menuNodeService.IsNameVnAvailable(NameVn) != null ? false : true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult IsNameVnIdAvailable(string NameVn, string Id)
        {
            // Check if the NameVn already exists
            return Json(menuNodeService.IsNameVnAvailable(NameVn, Id) != null ? false : true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult IsNameEnAvailable(string NameEn)
        {
            // Check if the NameEn already exists
            return Json(menuNodeService.IsNameEnAvailable(NameEn) != null ? false : true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult IsNameEnIdAvailable(string NameEn, string Id)
        {
            // Check if the NameEn already exists
            return Json(menuNodeService.IsNameEnAvailable(NameEn, Id) != null ? false : true, JsonRequestBehavior.AllowGet);
        }
    }
}