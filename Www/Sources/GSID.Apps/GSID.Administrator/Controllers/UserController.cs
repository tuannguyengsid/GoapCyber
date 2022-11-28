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
using static GSID.Model.MongodbModels.User;
using System.Globalization;
using GSID.Model.ExtraEntities;
using Newtonsoft.Json;
using GSID.Service.MongoRepositories.Service;
using System.Threading.Tasks;

namespace GSID.Admin.Controllers
{
    public class UserController : BaseAuthenticationController
    {
        private readonly IUserService uService;
        private readonly IRoleService roleService;
        private readonly IUserToRoleService userToRoleService;
        private readonly IDepartmentService departmentService;
        private readonly IPositionService positionService;
        private readonly ICountryService countryService;
        private readonly IProvinceService provinceService;
        private readonly IDistrictService districtService;
        // GET: User
        public UserController(IUserService _uService,
                                IRoleService _roleService,
                                IUserToRoleService _userToRoleService,
                                IDepartmentService _departmentService,
                                IPositionService _positionService,
                                ICountryService _countryService,
                                IProvinceService _provinceService,
                                IDistrictService _districtService)
        {
            uService = _uService;
            departmentService = _departmentService;
            positionService = _positionService;
            countryService = _countryService;
            provinceService = _provinceService;
            districtService = _districtService;
            roleService = _roleService;
            userToRoleService = _userToRoleService;
        }

        // GET: ControlPanel/User]
        [RBAC]
        public ActionResult Index(string p)
        {
            var model = uService.GetAllByType(UserIsType.UserinSystem);

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
            UserCreateViewModel model = new UserCreateViewModel();
            model.Roles = roleService.GetAll();
            model.IsDeleted = true;
            ViewBag.ActiveMenu = RBACUser.RootPermissionId(Request);

            return View(model);
        }

        [HttpPost]
        [RBAC]
        public ActionResult Create([Bind(Include = "FirstName, LastName, Email, Password, RoleId, IsDeleted")] UserCreateViewModel obj)
        {
            string title = Message.TITLE_ERROR;
            string message = Message.CONTENT_POSTDATA_DEFAUT_ERROR;
            string status = Default.Status_Error;
            string id = string.Empty;
            try
            {
                if (!RBACUser.HasPermission("RecycleBin", "User"))
                    ModelState.Remove("IsDeleted");
                if (ModelState.IsValid)
                {
                    User model = Mapper.Map<UserCreateViewModel, User>(obj);

                    model.Email = obj.Email.ToLower();
                    model.PasswordSalt = RandomStringGenerator.RandomString();
                    model.Password = PasswordHelper.GenerateHashedPassword(obj.Password.Trim().ToLower(), model.PasswordSalt);
                    model.IsType = UserIsType.UserinSystem;
                    model.IsAdministrator = true;
                    model.IsDeleted = !RBACUser.HasPermission("RecycleBin", "User") ? false : !obj.IsDeleted;
                    model.AddedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
                    id = uService.Create(model);

                    //var role = roleService.GetByAdmin();
                    //if (role == null)
                    //{
                    //    role = new Role();
                    //    role.Name = "Quản trị viên";
                    //    role.IsSysAdmin = true;
                    //    role.IsDeleted = false;
                    //    roleService.Create(role);
                    //}
                    if (model.Id != null && obj.RoleId.Count() >= 0)
                    {
                        foreach (var item in obj.RoleId)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                RoleToUser u2role = new RoleToUser();
                                u2role.UserId = model.Id;
                                u2role.RoleId = item;

                                userToRoleService.Create(u2role);
                            }
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
            var model = Mapper.Map<User, UserEditViewModel>(uService.GetBy(gsid));
            if (model == null)
                Response.Redirect(string.Format("/Error/NotFound?url={0}", Request.Url), true);

            var u2roles = userToRoleService.GetAllByUser(gsid);
            model.RoleId = u2roles != null ? u2roles.Select(c => c.RoleId).ToList() : new List<string>();
            model.Roles = roleService.GetAll();
            model.IsDeleted = !model.IsDeleted;
            ViewBag.ActiveMenu = RBACUser.RootPermissionId(Request);
            return View(model);
        }

        [HttpPost]
        [RBAC]
        public ActionResult Update([Bind(Include = "Id, FirstName, LastName, Email, Password, RoleId, IsDeleted")] UserEditViewModel obj)
        {
            string title = Message.TITLE_ERROR;
            string message = Message.CONTENT_POSTDATA_DEFAUT_ERROR;
            string status = Default.Status_Error;
            try
            {
                if (!RBACUser.HasPermission("RecycleBin", "User"))
                    ModelState.Remove("IsDeleted");
                if (ModelState.IsValid)
                {
                    var model = uService.GetBy(obj.Id);
                    if (model != null)
                    {

                        if (!string.IsNullOrEmpty(obj.Password))
                        {
                            model.PasswordSalt = RandomStringGenerator.RandomString();
                            model.Password = PasswordHelper.GenerateHashedPassword(obj.Password.Trim().ToLower(), model.PasswordSalt);
                        }
                        model.FirstName = obj.FirstName;
                        model.LastName = obj.LastName;
                        model.EditedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
                        if (RBACUser.HasPermission("RecycleBin", "User"))
                            model.IsDeleted = !obj.IsDeleted;
                        uService.Update(model);

                        userToRoleService.DeleteByUser(model.Id);
                        if (model.Id != null && obj.RoleId.Count() >= 0)
                        {
                            foreach (var item in obj.RoleId)
                            {
                                if (!string.IsNullOrEmpty(item))
                                {
                                    RoleToUser u2role = new RoleToUser();
                                    u2role.UserId = model.Id;
                                    u2role.RoleId = item;

                                    userToRoleService.Create(u2role);
                                }
                            }
                        }

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

            var _hasDelete = uService.Delete(id);
            userToRoleService.DeleteByUser(id);
            if (_hasDelete)
                status = ((int)StatusDelete.Deleted).ToString();

            return Json(new
            {
                Status = status,
                Message = message
            });
        }

        [RBAC]
        [HttpPost]
        public ActionResult CheckExistingShortName(string Email)
        {
            try
            {
                return Json(!(uService.GetByEmail(Email.ToLower().Trim()) != null));
            }
            catch (Exception ex)
            {
                return Json(false);
            }
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

            var _hasRecycleBin = uService.GetBy(id);
            _hasRecycleBin.DeletedByDate = DateTime.Now;
            _hasRecycleBin.DeletedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
            _hasRecycleBin.IsDeleted = !isDeleted;
            _hasRecycleBin.DeletedByDate = DateTime.Now;
            //_hasRecycleBin.DeletedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
            uService.Update(_hasRecycleBin);

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