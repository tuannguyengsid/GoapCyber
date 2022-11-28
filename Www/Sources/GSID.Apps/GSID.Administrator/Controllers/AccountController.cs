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
    [Authorize]
    public class AccountController : BaseController
    {
        private readonly IUserService userService;
        public IUserFormsAuthenticationService FormsService { get; set; }
        public HttpContextBase context { get; set; }
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController(IUserService _userService)
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

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            string title = "Thông báo",
                message = "Tài khoản không tồn tại hoặc mật khẩu sai. Xin vui lòng thử lại.",
                status = "1",
                url = "";

            if (ModelState.IsValid)
            {
                if (!Request.IsAuthenticated)
                {
                    var _hasUser = userService.VerifiedAccount(model.Email);
                    if (ModelState.IsValid && _hasUser != null)
                    {
                        FormsService.SignIn(_hasUser, model.RememberMe, context);
                        _hasUser.LastLogon = DateTime.Now;
                        userService.Update(_hasUser);
                        if (Url.IsLocalUrl(model.returnUrl)
                                && model.returnUrl.Length > 1
                                && model.returnUrl.StartsWith("/")
                                && !model.returnUrl.StartsWith("//")
                                && !model.returnUrl.StartsWith("/\\"))
                        {
                            url = model.returnUrl;
                        }
                        else
                        {
                            var _menuItems = RBACUser.GetStaticListMenu();
                            var _menuItem = _menuItems.Where(w => !string.IsNullOrEmpty(w.Url)).FirstOrDefault();

                            url = _menuItem != null ? _menuItem.Url : "/";
                        }
                        status = "2";
                        message = "Đăng nhập thành công!";
                    }
                }
                else
                {
                    if ((Url.IsLocalUrl(model.returnUrl) && model.returnUrl.Length > 1 && model.returnUrl.StartsWith("/")
                                               && !model.returnUrl.StartsWith("//") && !model.returnUrl.StartsWith("/\\")))
                    {
                        url = model.returnUrl;
                    }
                    else
                    {
                        var _menuItems = RBACUser.GetStaticListMenu();
                        var _menuItem = _menuItems.Where(w => !string.IsNullOrEmpty(w.Url)).FirstOrDefault();

                        url = _menuItem != null ? _menuItem.Url : "/";
                    }
                    status = "2";
                    message = "Bạn đã đăng nhập vào hệ thống. Xin vui lòng bấm Ok để sử dụng.";
                }
            }
            else
            {
                message = "Xin vui lòng nhập đầy đủ thông tin trên màn hình.";
            }

            return Json(new
            {
                Title = title,
                Message = message,
                Status = status,
                Url = url
            }, JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /Account/LogOff
        public ActionResult LogOff()
        {
            FormsService.SignOut();
            return RedirectToLocal(SestionName.ReturnLogin);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}