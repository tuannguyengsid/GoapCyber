using GSID.Service;
using GSID.Setting;
using GSID.Web.Core.Authentication;
using GSID.Admin.Helpers;
using GSID.Admin.ViewModels.MongoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using GSID.Service.MongoRepositories.Service;

namespace GSID.Admin.Controllers
{
    public class MembershipController : BaseController
    {
        private readonly IUserService userService;
        public IUserFormsAuthenticationService FormsService { get; set; }
        public HttpContextBase context { get; set; }

        // GET: ControlPanel/Membership
        public MembershipController(IUserService _userService)
        {
            userService = _userService;
            context = HttpContext;
            if (context == null)
                context = new HttpContextWrapper(System.Web.HttpContext.Current);
        }

        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null) { FormsService = new UserFormsAuthenticationService(); }

            base.Initialize(requestContext);
        }

        public ActionResult Login(string returnUrl)
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                LoginViewModel model = new LoginViewModel();
                model.returnUrl = returnUrl;
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult Login([Bind(Include = "Email, Password, returnUrl")]LoginViewModel model)
        {
            string title = Message.TITLE_ERROR;
            string message = Message.CONTENT_POSTDATA_DEFAUT_ERROR;
            string status = Default.Status_Error;
            string url = string.Empty;

            if (!Request.IsAuthenticated)
            {
                var _has = userService.VerifiedAccount(model.Email);
                if (ModelState.IsValid && _has != null)
                {
                    if (!_has.IsDeleted)
                    {
                        if (PasswordHelper.GenerateHashedPassword(model.Password, _has.PasswordSalt).Equals(_has.Password))
                        {
                            FormsService.SignIn(_has, true, context);
                            _has.LastLogon = DateTime.Now;
                            userService.Update(_has);
                            url = (Url.IsLocalUrl(model.returnUrl) && model.returnUrl.Length > 1 && model.returnUrl.StartsWith("/")
                                                && !model.returnUrl.StartsWith("//") && !model.returnUrl.StartsWith("/\\"))
                                                    ? model.returnUrl : "/";
                            title = Message.TITLE_REPORT;
                            message = Message.LOGIN_SUCCESSFULL;
                            status = Default.Status_Sucessfull;
                        }
                        else
                            message = Message.LOGIN_FAIL;
                    }
                    else
                        message = Message.LOGIN_LOCKED;
                }
            }
            else
                message = Message.LOGIN_LOGON;

            return Json(new
            {
                Url = url,
                Status = status,
                Title = title,
                Message = message
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ChangePassword()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            string status = "1";
            string message = "";
            if (Request.IsAuthenticated)
            {
                var _hasUser = userService.VerifiedAccount(GSIDSessionFacade.GSIDSessionUserLogon.Email);
                if (ModelState.IsValid && _hasUser != null)
                {
                    if (PasswordHelper.GenerateHashedPassword(model.PasswordOld, _hasUser.PasswordSalt).Equals(_hasUser.Password))
                    {
                        _hasUser.Password = PasswordHelper.GenerateHashedPassword(model.NewPassword.Trim().ToLower(), _hasUser.PasswordSalt);
                        userService.Update(_hasUser);
                        message = "Thay đổi mật khẩu mới thành công!";
                    }
                    else
                    {
                        message = "Mật khẩu cũ nhập sai!";
                    }
                }
            }

            return Json(new
            {
                Status = status,
                Message = message
            }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult LogOff()
        {
            FormsService.SignOut();
            return RedirectToLocal(SestionName.ReturnLogin);
        }
    }
}