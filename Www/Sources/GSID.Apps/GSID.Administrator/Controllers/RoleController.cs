using AutoMapper;
using GSID.Model.MongodbModels;
using GSID.Service;
using GSID.Setting;
using GSID.Setting.FormKeys;
using GSID.Web.Core.Extensions;
using GSID.Admin.Helpers;
using System;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using GSID.Admin.ViewModels.MongoModels;
using GSID.Service.MongoRepositories.Service;
using System.Collections.Generic;

namespace GSID.Admin.Controllers
{
    public class RoleController : BaseAuthenticationController
    {
        private readonly IRoleService roleService;
        private readonly IPermissionService permissionService;
        private readonly IUserService uService;
        private readonly IUserToRoleService userToRoleService;
        private readonly IRoleToPermisionService roleToPermisionService;
        public RoleController(IRoleService _roleService,
                                IUserService _uService,
                                IUserToRoleService _userToRoleService,
                                IPermissionService _permissionService,
                                IRoleToPermisionService _roleToPermisionService)
        {
            roleService = _roleService;
            uService = _uService;
            userToRoleService = _userToRoleService;
            permissionService = _permissionService;
            roleToPermisionService = _roleToPermisionService;
        }

        [RBAC]
        public ActionResult Index(string p)
        {
            List<string> NoActionIds = new List<string>();
            var u = uService.VerifiedAccount("support@g-sid.com"); //no delete role master
            var roleNoDelete = u != null ? userToRoleService.GetAllByUser(u.Id) : null;
            if (roleNoDelete != null)
                NoActionIds = roleNoDelete.Select(s => s.RoleId).ToList();
            ViewBag.NoActionIds = NoActionIds;

            var model = roleService.GetAll();
            PageIndex = p.ConvertIntPaging();
            ViewBag.TotalPage = (Math.Ceiling((double)model.Count / PageSize));
            ViewBag.CurrentPage = PageIndex;
            ViewBag.PageVisit = PageVisit;
            ViewBag.PageSize = PageSize;
            ViewBag.CountTotal = model.Count;
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
            RoleCreateViewModel model = new RoleCreateViewModel();
            model.IsDeleted = true;
            ViewBag.ActiveMenu = RBACUser.RootPermissionId(Request);
            return View(model);
        }

        [HttpPost]
        [RBAC]
        public ActionResult Create([Bind(Include = "Name, Description, IsSysAdmin, ListPermisison, Listundetermined, IsDeleted")] RoleCreateViewModel obj)
        {
            string title = Message.TITLE_ERROR;
            string message = Message.CONTENT_POSTDATA_DEFAUT_ERROR;
            string status = Default.Status_Error;
            string id = string.Empty;
            try
            {
                if (!RBACUser.HasPermission("RecycleBin", "Role"))
                    ModelState.Remove("IsDeleted");
                if (ModelState.IsValid)
                {
                    Role model = Mapper.Map<RoleCreateViewModel, Role>(obj);
                    model.AddedByDate = DateTime.Now;
                    model.IsDeleted = !RBACUser.HasPermission("RecycleBin", "Role") ? false : !obj.IsDeleted;
                    model.AddedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
                    id = roleService.Create(model);

                    if (!string.IsNullOrEmpty(model.Id))
                    {
                        if (!string.IsNullOrEmpty(obj.Listundetermined))
                        {
                            var sa = obj.Listundetermined.Split(',');
                            roleToPermisionService.Create(model.Id, obj.Listundetermined.Split(','), (int)PermissionState.Undetermined);
                        }

                        if (!string.IsNullOrEmpty(obj.ListPermisison))
                        {
                            roleToPermisionService.Create(model.Id, obj.ListPermisison.Split(','), (int)PermissionState.Selected);
                        }
                    }


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
            var model = Mapper.Map<Role, RoleEditViewModel>(roleService.GetBy(gsid));
            if (model == null)
                Response.Redirect(string.Format("/Error/NotFound?url={0}", Request.Url), true);

            var permission = roleToPermisionService.GetAllBy(model.Id, (int)PermissionState.Selected).Select(p => p.Permission.Id).ToList();
            model.ListPermisison = String.Join(",", permission);
            model.IsDeleted = !model.IsDeleted;
            ViewBag.ActiveMenu = RBACUser.RootPermissionId(Request);
            return View(model);
        }

        [HttpPost]
        [RBAC]
        public ActionResult Update([Bind(Include = "Id, Name, Description, IsSysAdmin, ListPermisison, Listundetermined, IsDeleted")] RoleEditViewModel obj)
        {
            string title = Message.TITLE_ERROR;
            string message = Message.CONTENT_POSTDATA_DEFAUT_ERROR;
            string status = Default.Status_Error;
            try
            {
                if (!RBACUser.HasPermission("RecycleBin", "Role"))
                    ModelState.Remove("IsDeleted");
                if (ModelState.IsValid)
                {
                    var model = roleService.GetBy(obj.Id);
                    if (model != null)
                    {
                        model.Name = obj.Name;
                        if (RBACUser.HasPermission("RecycleBin", "Role"))
                            model.IsDeleted = !obj.IsDeleted;
                        model.EditedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
                        model.EditedByDate = DateTime.Now;
                        roleService.Update(model);

                        roleToPermisionService.DeleteByRole(model.Id);

                        if (!string.IsNullOrEmpty(obj.Listundetermined))
                            roleToPermisionService.Create(model.Id, obj.Listundetermined.Split(','), (int)PermissionState.Undetermined);

                        if (!string.IsNullOrEmpty(obj.ListPermisison))
                            roleToPermisionService.Create(model.Id, obj.ListPermisison.Split(','), (int)PermissionState.Selected);

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
            if (string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var u = uService.VerifiedAccount("support@g-sid.com"); //no delete role master
            var roleNoDelete = u != null ? userToRoleService.GetAllByUser(u.Id) : null;
            if (roleNoDelete != null)
            {
                var roleNoDeleteIds = roleNoDelete.Select(s => s.RoleId).ToList();
                if (roleNoDeleteIds.Contains(id))
                    return Json(new {
                        Status = status,
                        Message = message
                    });
            }

            var roles = userToRoleService.GetAllByRole(id);
            if (roles.Count <= 0)
            {
                var _hasDelete = roleService.Delete(id);
                if (_hasDelete)
                {
                    roleToPermisionService.DeleteByRole(id);
                    status = ((int)StatusDelete.Deleted).ToString();
                }
            }

            return Json(new
            {
                Status = status,
                Message = message
            });
        }

        [HttpGet]
        public ActionResult GetListBySortLevel(string id)
        {
            ViewBag.SelectNode = string.IsNullOrEmpty(id) ? id : "";
            return PartialView();
        }

        [ChildActionOnly]
        public PartialViewResult Children(string ParentID)
        {
            var menu = permissionService.GetAllByChild(ParentID, false);
            return PartialView(menu);
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
            
            var u = uService.VerifiedAccount("support@g-sid.com"); //no delete role master
            var roleNoDelete = u != null ? userToRoleService.GetAllByUser(u.Id) : null;
            if (roleNoDelete != null)
            {
                var roleNoDeleteIds = roleNoDelete.Select(s => s.RoleId).ToList();
                if (roleNoDeleteIds.Contains(id))
                    return Json(new
                    {
                        Title = title,
                        Message = message,
                        Status = status
                    }, JsonRequestBehavior.AllowGet);
            }

            var _hasRecycleBin = roleService.GetBy(id);
            _hasRecycleBin.DeletedByDate = DateTime.Now;
            _hasRecycleBin.DeletedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
            _hasRecycleBin.IsDeleted = !isDeleted;
            roleService.Update(_hasRecycleBin);

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