using AutoMapper;
using System.Collections.Generic;
using GSID.Model.MongodbModels;
using GSID.Service;
using GSID.Setting;
using GSID.Setting.FormKeys;
using GSID.Web.Core.Extensions;
using GSID.Admin.Helpers;
using System;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Web.Mvc;
using System.Linq;
using System.Reflection;
using GSID.Admin.ViewModels.MongoModels;
using GSID.Service.MongoRepositories.Service;

namespace GSID.Admin.Controllers
{
    public class FunctionController : BaseController
    {
        private readonly IPermissionService permissionService;
        public FunctionController(IPermissionService _permissionService)
        {
            permissionService = _permissionService;
        }
        #region Permission
        // GET: Permission
        //[RBAC]
        public ActionResult Index(string p)
        {
            var model = permissionService.GetAllByChild("");
            return View(model);
        }

        [ChildActionOnly]
        public PartialViewResult PartialIndexChildren(string ParentId)
        {
            var model = permissionService.GetAllByChild(ParentId);
            return PartialView(model);
        }

        //[RBAC]
        public ActionResult Create()
        {
            PermissionCreateViewModel model = new PermissionCreateViewModel();
            model.IsDeleted = true;
            return View(model);
        }

        //[HttpPost, RBAC]
        [HttpPost]
        public ActionResult Create([Bind(Include = "ParentId, Name, Description, Url, IsMenu, Sort, IsDeleted, Icon")] PermissionCreateViewModel obj)
        {
            string title = Message.TITLE_ERROR;
            string message = Message.CONTENT_POSTDATA_DEFAUT_ERROR;
            Permission model = null;
            try
            {
                if (ModelState.IsValid)
                {
                    model = Mapper.Map<PermissionCreateViewModel, Permission>(obj);
                    model.ParentId = !string.IsNullOrEmpty(obj.ParentId) ? obj.ParentId.Replace("____", "") : "";
                    model.Description = !string.IsNullOrEmpty(obj.Description) ? obj.Description.Trim().ToLower() : "";
                    //model.AddedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
                    model.IsDeleted = !obj.IsDeleted;
                    permissionService.Create(model);
                    title = Message.TITLE_REPORT;
                    message = Message.CONTENT_POSTDATA_CREATE_SUCCESSFULL;
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
                Id = model != null ? model.Id.ToString() : "",
                Title = title,
                Message = message
            }, JsonRequestBehavior.AllowGet);
        }

        //[RBAC]
        public ActionResult Update(string gsid)
        {
            var model = Mapper.Map<Permission, PermissionEditViewModel>(permissionService.GetBy(gsid));
            if (model == null)
                Response.Redirect(string.Format("/Error/NotFound?url={0}", Request.Url), true);

            var parent = !string.IsNullOrEmpty(model.ParentId) ? permissionService.GetBy(model.ParentId) : null;
            ViewBag.ParentName = parent != null ? parent.Name : "";
            model.IsDeleted = !model.IsDeleted;
            return View(model);
        }

        [HttpPost]
        //[RBAC]
        public ActionResult Update([Bind(Include = "Id, ParentId, Name, Description, Url, IsMenu, Sort, IsDeleted, Icon")] PermissionEditViewModel obj)
        {
            string title = Message.TITLE_ERROR;
            string message = Message.CONTENT_POSTDATA_DEFAUT_ERROR;
            try
            {
                if (ModelState.IsValid)
                {
                    var model = permissionService.GetBy(obj.Id);
                    if (model != null)
                    {
                        model.ParentId = !string.IsNullOrEmpty(obj.ParentId) ? obj.ParentId.Replace("____", "") : "";
                        model.Name = obj.Name;
                        model.Description = !string.IsNullOrEmpty(obj.Description) ? obj.Description.Trim().ToLower() : "";
                        model.IsMenu = obj.IsMenu;
                        model.Sort = obj.Sort;
                        model.Icon = obj.Icon;
                        model.Url = obj.Url;
                        model.IsDeleted = !obj.IsDeleted;
                        //model.EditedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
                        permissionService.Update(model);
                        title = Message.TITLE_REPORT;
                        message = Message.CONTENT_POSTDATA_UPDATE_SUCCESSFULL;
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
                Message = message
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        //[RBAC]
        public ActionResult Delete(string id)
        {
            string status = ((int)StatusDelete.Error).ToString();
            string message = Message.CONTENT_POSTDATA_DELETE_ERROR_OR_EMPTY;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var _hasDelete = permissionService.Delete(id);
            if (_hasDelete)
                status = ((int)StatusDelete.Deleted).ToString();

            return Json(new
            {
                Status = status,
                Message = message
            });
        }

        [HttpPost]
        //[RBAC]
        public ActionResult Import()
        {
            var _controllerTypes = AppDomain.CurrentDomain.GetAssemblies()
                   .SelectMany(a => a.GetTypes())
                   .Where(t => t != null
                       && t.IsPublic
                       && t.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)
                       && !t.IsAbstract
                       && typeof(IController).IsAssignableFrom(t));

            var _controllerMethods = _controllerTypes.ToDictionary(controllerType => controllerType,
                    controllerType => controllerType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .Where(m => typeof(ActionResult).IsAssignableFrom(m.ReturnType)));

            foreach (var _controller in _controllerMethods)
            {
                string _controllerName = _controller.Key.Name;
                foreach (var _controllerAction in _controller.Value)
                {
                    string _controllerActionName = _controllerAction.Name;
                    if (_controllerName.EndsWith("Controller"))
                    {
                        _controllerName = _controllerName.Substring(0, _controllerName.LastIndexOf("Controller"));
                    }

                    string _permissionDescription = string.Format("{0}-{1}", _controllerName.ToLower(), _controllerActionName.ToLower());
                    Permission _permission = permissionService.GetByDescription(_permissionDescription);
                    if (_permission == null)
                    {
                        if (ModelState.IsValid)
                        {
                            Permission _perm = new Permission();
                            _perm.ParentId = "";
                            _perm.IsMenu = false;
                            _perm.Description = _permissionDescription;

                            permissionService.Create(_perm);
                        }
                    }
                }
            }
            return Json(new
            {
                Message = "Import is successfull"
            });
        }
        #endregion

        [HttpPost]
        //[RBAC]
        public ActionResult RecycleBin(string id, bool isDeleted)
        {
            string title = Message.TITLE_ERROR;
            string status = ((int)StatusDelete.Error).ToString();
            string message = Message.CONTENT_POSTDATA_DELETE_ERROR_OR_EMPTY;
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var _hasRecycleBin = permissionService.GetBy(id);
            _hasRecycleBin.DeletedByDate = DateTime.Now;
            _hasRecycleBin.DeletedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
            _hasRecycleBin.IsDeleted = !isDeleted;
            permissionService.Update(_hasRecycleBin);

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

        [HttpGet]
        public ActionResult GetListBySortLevel(string id)
        {
            ViewBag.SelectNode = !string.IsNullOrEmpty(id) ? id : "";
            return PartialView();
        }

        [ChildActionOnly]
        public PartialViewResult Children(string ParentID)
        {
            var menu = permissionService.GetAllByChild(ParentID);
            return PartialView(menu);
        }
    }
}