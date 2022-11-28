using GSID.Service;
using GSID.Setting;
using GSID.Web.Core.Authentication;
using GSID.FrontEnd.Helpers;
using GSID.FrontEnd.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using GSID.Service.MongoRepositories.Service;
using GSID.Model.ExtraEntities;
using Newtonsoft.Json;
using GSID.FrontEnd.Models;
using System.Data.Entity.Infrastructure;
using GSID.Model.MongodbModels;
using GSID.Web.Core.Extensions;
using GSID.Web.Core.Helpers;
using System.Globalization;

namespace GSID.FrontEnd.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private readonly ICustomerService customerService;
        private readonly IUserService userService;
        private readonly IParameterService paraService;
        private readonly IMailTemplateService mailTemplateService;
        public IUserFormsAuthenticationService FormsService { get; set; }
        public HttpContextBase context { get; set; }
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController(IUserService _userService,
                                    IMailTemplateService _mailTemplateService,
                                    ICustomerService _customerService,
                                    IParameterService _paraService)
        {
            userService = _userService;
            paraService = _paraService;
            customerService = _customerService;
            mailTemplateService = _mailTemplateService;
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
        public ActionResult Login(string language, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            HomePageManagementAdminConfig homePageConfig = new HomePageManagementAdminConfig();
            var paraHomePageConfig = paraService.GetByCode(new HomePageManagementAdminConfig().Code);
            if (paraHomePageConfig != null)
                homePageConfig = JsonConvert.DeserializeObject<HomePageManagementAdminConfig>(paraHomePageConfig.Content.ToString());
            ViewBag.HomePageConfig = homePageConfig;

            LoginViewModel model = new LoginViewModel();
            model.Language = language;

            DefineRouterValueLanguages(language, new { returnUrl = returnUrl }, new { returnUrl = returnUrl });
            return View(model);
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "Email, Password, IsRememberMe, Language")] LoginViewModel obj, string returnUrl)
        {
            string title = "";
            string message = "";
            string status = Default.Status_Error;
            string url = (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                                                && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                                        ? returnUrl : string.Format("/{0}", obj.Language);

            var languages = (List<Language>)System.Web.HttpContext.Current.Application[SestionName.Languages];
            Language curentLan = (languages != null && languages.Count > 0) ? languages.Where(l => l.Parent == obj.Language).FirstOrDefault() : null;

            if (curentLan != null)
            {
                title = (curentLan.Country == Language.LanguagueCountry.Vietmamese ? "Lỗi" : (curentLan.Country == Language.LanguagueCountry.English ? "Error" : ""));
                message = (curentLan.Country == Language.LanguagueCountry.Vietmamese ? "Dữ liệu gặp lỗi. Xin vui lòng kiểm tra lại." : (curentLan.Country == Language.LanguagueCountry.English ? "Data error. Please check again." : ""));
                try
                {
                    if (ModelState.IsValid)
                    {
                        if (!Request.IsAuthenticated && !string.IsNullOrEmpty(obj.Email))
                        {
                            var username = obj.Email.Trim().ToLower();
                            var _hasUser = customerService.VerifiedAccountEmailOrPhone(username);

                            if (_hasUser != null && !_hasUser.IsDeleted)
                            {
                                if (_hasUser.AccountVerified)
                                {
                                    if (PasswordHelper.GenerateHashedPassword(obj.Password, _hasUser.PasswordSalt).Equals(_hasUser.Password))
                                    {
                                        FormsService.SignIn(_hasUser, obj.IsRememberMe, context);
                                        _hasUser.LastLogon = DateTime.Now;
                                        customerService.Update(_hasUser);

                                        title = (curentLan.Country == Language.LanguagueCountry.Vietmamese ? "Thông báo" : (curentLan.Country == Language.LanguagueCountry.English ? "Notification" : ""));
                                        message = (curentLan.Country == Language.LanguagueCountry.Vietmamese ? "Đăng nhập thành công!." : (curentLan.Country == Language.LanguagueCountry.English ? "Logged in successfully." : ""));
                                        status = Default.Status_Sucessfull;
                                    }
                                }
                                else
                                {
                                    title = (curentLan.Country == Language.LanguagueCountry.Vietmamese ? "Thông báo" : (curentLan.Country == Language.LanguagueCountry.English ? "Notification" : ""));
                                    message = (curentLan.Country == Language.LanguagueCountry.Vietmamese ? "Xin vui lòng xác nhận tài khoản qua email!." : (curentLan.Country == Language.LanguagueCountry.English ? "Please confirm your account via email." : ""));
                                }
                            }
                            else
                            {
                                title = (curentLan.Country == Language.LanguagueCountry.Vietmamese ? "Lỗi" : (curentLan.Country == Language.LanguagueCountry.English ? "Error" : ""));
                                message = (curentLan.Country == Language.LanguagueCountry.Vietmamese ? "Tài khoản của bạn đã bị khóa hoặc không tồn tại. Xin vui lòng liên hệ hotline để được hỗ trợ." : (curentLan.Country == Language.LanguagueCountry.English ? "Your account has been locked or does not exist. Please contact the hotline for support." : ""));
                            }
                        }
                        else
                        {
                            title = (curentLan.Country == Language.LanguagueCountry.Vietmamese ? "Thông báo" : (curentLan.Country == Language.LanguagueCountry.English ? "Notification" : ""));
                            message = (curentLan.Country == Language.LanguagueCountry.Vietmamese ? "Tài khoản đã đăng nhập. Xin vui lòng đăng xuất trước khi đăng nhập tài khoản khác." : (curentLan.Country == Language.LanguagueCountry.English ? "Account is logged in. Please log out before logging into another account." : ""));
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
            }

            return Json(new
            {
                Title = title,
                Message = message,
                Status = status,
                Url = url
            }, JsonRequestBehavior.AllowGet);
        }

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Register(string language, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            HomePageManagementAdminConfig homePageConfig = new HomePageManagementAdminConfig();
            var paraHomePageConfig = paraService.GetByCode(new HomePageManagementAdminConfig().Code);
            if (paraHomePageConfig != null)
                homePageConfig = JsonConvert.DeserializeObject<HomePageManagementAdminConfig>(paraHomePageConfig.Content.ToString());
            ViewBag.HomePageConfig = homePageConfig;

            RegisterViewModel model = new RegisterViewModel();
            model.Language = language;
            DefineRouterValueLanguages(language, new { returnUrl = returnUrl }, new { returnUrl = returnUrl });
            return View(model);
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "FullName, Email, PhoneNumber, Password, ConfirmPassword, IsAgree, Language")] RegisterViewModel obj, string returnUrl)
        {
            string title = "";
            string message = "";
            string status = Default.Status_Error;
            string url = (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                                                && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                                        ? returnUrl : string.Format("/{0}", obj.Language);

            var languages = (List<Language>)System.Web.HttpContext.Current.Application[SestionName.Languages];
            Language curentLan = (languages != null && languages.Count > 0) ? languages.Where(l => l.Parent == obj.Language).FirstOrDefault() : null;

            if (curentLan != null)
            {
                title = (curentLan.Country == Language.LanguagueCountry.Vietmamese ? "Lỗi" : (curentLan.Country == Language.LanguagueCountry.English ? "Error" : ""));
                message = (curentLan.Country == Language.LanguagueCountry.Vietmamese ? "Dữ liệu gặp lỗi. Xin vui lòng kiểm tra lại." : (curentLan.Country == Language.LanguagueCountry.English ? "Data error. Please check again." : ""));
                try
                {
                    if (ModelState.IsValid)
                    {
                        Customer model = customerService.GetByEmailOrPhone(obj.Email.ToLower(), obj.PhoneNumber);
                        if (model == null)
                        {
                            model = new Customer();
                            model.FullName = !string.IsNullOrEmpty(obj.FullName) ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(obj.FullName.Trim()) : "";
                            model.PhoneNumber = !string.IsNullOrEmpty(obj.PhoneNumber) ? obj.PhoneNumber.Trim() : "";
                            model.Email = !string.IsNullOrEmpty(obj.Email) ? obj.Email.Trim().ToLower() : "";
                            model.PasswordSalt = RandomStringGenerator.RandomString();
                            model.Password = PasswordHelper.GenerateHashedPassword(obj.Password.Trim(), model.PasswordSalt);
                            model.AccountVerifyToken = RandomStringGenerator.RandomStringToken(50).ToLower();
                            model.AccountVerifyDate = DateTime.Now;
                            model.AccountVerified = false;
                            model.Gender = Customer.IsGender.None;
                            model.Birthday = DateTime.Now;
                            customerService.Create(model);

                            if (curentLan.Country == Language.LanguagueCountry.Vietmamese)
                            {
                                var mailSenderPara = paraService.GetByCode(new EmailSTMPConfig().Code);
                                if (mailSenderPara != null)
                                {
                                    var _mailSenderAccount = JsonConvert.DeserializeObject<EmailSTMPConfig>(mailSenderPara.Content.ToString());
                                    var _mailTemplate = mailTemplateService.GetByCode(MailTemplateCode.CONFIRM_NEW_ACCOUNT);

                                    MailTo _mailToObj = new MailTo();
                                    string linkToken = string.Format("{0}{1}", GSIDSessionSiteInformation.UrlAddressSite, Url.Action("VerifyRegistration", "Account", new { language = obj.Language, token = model.AccountVerifyToken }));
                                    _mailToObj.To = model.Email;
                                    _mailToObj.FullName = _mailSenderAccount.FullName;

                                    _mailToObj.Subject = _mailTemplate.SubjectVn;
                                    if (_mailToObj.Subject.IndexOf("{{fullname}}") > 0)
                                        _mailToObj.Subject = _mailToObj.Body.Replace("{{fullname}}", model.FullName);
                                    if (_mailToObj.Subject.IndexOf("{{phonenumber}}") > 0)
                                        _mailToObj.Subject = _mailToObj.Body.Replace("{{phonenumber}}", model.PhoneNumber);
                                    if (_mailToObj.Subject.IndexOf("{{linktoken}}") > 0)
                                        _mailToObj.Subject = _mailToObj.Body.Replace("{{linktoken}}", linkToken);

                                    _mailToObj.Body = _mailTemplate.BodyVn;
                                    if (_mailToObj.Body.IndexOf("{{fullname}}") > 0)
                                        _mailToObj.Body = _mailToObj.Body.Replace("{{fullname}}", model.FullName);
                                    if (_mailToObj.Body.IndexOf("{{phonenumber}}") > 0)
                                        _mailToObj.Body = _mailToObj.Body.Replace("{{phonenumber}}", model.PhoneNumber);
                                    if (_mailToObj.Body.IndexOf("{{linktoken}}") > 0)
                                        _mailToObj.Body = _mailToObj.Body.Replace("{{linktoken}}", linkToken);

                                    Mailer.SendMail(_mailSenderAccount, _mailToObj);
                                }
                                title = "Thông báo";
                                message = "Cảm ơn bạn đã đăng ký. Xin vui lòng xác nhận tài khoản qua email.";
                                status = Default.Status_Sucessfull;
                            }
                            else if (curentLan.Country == Language.LanguagueCountry.English)
                            {
                                var mailSenderPara = paraService.GetByCode(new EmailSTMPConfig().Code);
                                if (mailSenderPara != null)
                                {
                                    var _mailSenderAccount = JsonConvert.DeserializeObject<EmailSTMPConfig>(mailSenderPara.Content.ToString());
                                    var _mailTemplate = mailTemplateService.GetByCode(MailTemplateCode.CONFIRM_NEW_ACCOUNT);

                                    MailTo _mailToObj = new MailTo();
                                    string linkToken = string.Format("{0}{1}", GSIDSessionSiteInformation.UrlAddressSite, Url.Action("VerifyRegistration", "Account", new { language = obj.Language, token = model.AccountVerifyToken }));
                                    _mailToObj.To = model.Email;
                                    _mailToObj.FullName = _mailSenderAccount.FullName;

                                    _mailToObj.Subject = _mailTemplate.SubjectEn;
                                    if (_mailToObj.Subject.IndexOf("{{fullname}}") > 0)
                                        _mailToObj.Subject = _mailToObj.Body.Replace("{{fullname}}", model.FullName);
                                    if (_mailToObj.Subject.IndexOf("{{phonenumber}}") > 0)
                                        _mailToObj.Subject = _mailToObj.Body.Replace("{{phonenumber}}", model.PhoneNumber);
                                    if (_mailToObj.Subject.IndexOf("{{linktoken}}") > 0)
                                        _mailToObj.Subject = _mailToObj.Body.Replace("{{linktoken}}", linkToken);

                                    _mailToObj.Body = _mailTemplate.BodyEn;
                                    if (_mailToObj.Body.IndexOf("{{fullname}}") > 0)
                                        _mailToObj.Body = _mailToObj.Body.Replace("{{fullname}}", model.FullName);
                                    if (_mailToObj.Body.IndexOf("{{phonenumber}}") > 0)
                                        _mailToObj.Body = _mailToObj.Body.Replace("{{phonenumber}}", model.PhoneNumber);
                                    if (_mailToObj.Body.IndexOf("{{linktoken}}") > 0)
                                        _mailToObj.Body = _mailToObj.Body.Replace("{{linktoken}}", linkToken);

                                    Mailer.SendMail(_mailSenderAccount, _mailToObj);
                                }
                                title = "Notification";
                                message = "Thank you for registering. Please confirm your account by email.";
                                status = Default.Status_Sucessfull;
                            }
                        }
                        else
                        {
                            title = (curentLan.Country == Language.LanguagueCountry.Vietmamese ? "Thông báo" : (curentLan.Country == Language.LanguagueCountry.English ? "Notification" : ""));
                            message = (curentLan.Country == Language.LanguagueCountry.Vietmamese ? "Số điện thoại hoặc email của bạn đã tồn tại. Vui lòng đăng nhập!" : (curentLan.Country == Language.LanguagueCountry.English ? "Your phone number or email already exists. Please log in!" : ""));
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
            }

            return Json(new
            {
                Title = title,
                Message = message,
                Status = status,
                Url = url
            }, JsonRequestBehavior.AllowGet);
        }

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult VerifyRegistration(string language, string token)
        {
            HomePageManagementAdminConfig homePageConfig = new HomePageManagementAdminConfig();
            var paraHomePageConfig = paraService.GetByCode(new HomePageManagementAdminConfig().Code);
            if (paraHomePageConfig != null)
                homePageConfig = JsonConvert.DeserializeObject<HomePageManagementAdminConfig>(paraHomePageConfig.Content.ToString());
            ViewBag.HomePageConfig = homePageConfig;
            var model = customerService.GetByTokenRegistration(token);
            if (model != null)
            {
                model.AccountVerified = true;
                customerService.Update(model);
            }

            DefineRouterValueLanguages(language, new { token = token }, new { token = token });
            return View(model);
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Forgot(string language, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            HomePageManagementAdminConfig homePageConfig = new HomePageManagementAdminConfig();
            var paraHomePageConfig = paraService.GetByCode(new HomePageManagementAdminConfig().Code);
            if (paraHomePageConfig != null)
                homePageConfig = JsonConvert.DeserializeObject<HomePageManagementAdminConfig>(paraHomePageConfig.Content.ToString());
            ViewBag.HomePageConfig = homePageConfig;
            ForgotViewModel model = new ForgotViewModel();
            model.Language = language;

            DefineRouterValueLanguages(language, new { returnUrl = returnUrl }, new { returnUrl = returnUrl });
            return View(model);
        }
        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Forgot(ForgotViewModel obj, string returnUrl)
        {
            string title = "";
            string message = "";
            string status = Default.Status_Error;
            string url = (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                                                && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                                        ? returnUrl : string.Format("/{0}", obj.Language);

            var languages = (List<Language>)System.Web.HttpContext.Current.Application[SestionName.Languages];
            Language curentLan = (languages != null && languages.Count > 0) ? languages.Where(l => l.Parent == obj.Language).FirstOrDefault() : null;

            if (curentLan != null)
            {
                title = (curentLan.Country == Language.LanguagueCountry.Vietmamese ? "Lỗi" : (curentLan.Country == Language.LanguagueCountry.English ? "Error" : ""));
                message = (curentLan.Country == Language.LanguagueCountry.Vietmamese ? "Dữ liệu gặp lỗi. Xin vui lòng kiểm tra lại." : (curentLan.Country == Language.LanguagueCountry.English ? "Data error. Please check again." : ""));

                try
                {
                    if (ModelState.IsValid)
                    {
                        string email = obj.Email.Trim().ToLower();
                        Customer model = customerService.GetByEmail(email);
                        if (model != null)
                        {
                            model.ResetPasssToken = RandomStringGenerator.RandomStringToken(50).ToLower();
                            model.ResetPasssDate = DateTime.Now;
                            customerService.Update(model);

                            if (curentLan.Country == Language.LanguagueCountry.Vietmamese)
                            {
                                var mailSenderPara = paraService.GetByCode(new EmailSTMPConfig().Code);
                                if (mailSenderPara != null)
                                {
                                    var _mailSenderAccount = JsonConvert.DeserializeObject<EmailSTMPConfig>(mailSenderPara.Content.ToString());
                                    var _mailTemplate = mailTemplateService.GetByCode(MailTemplateCode.FORGOT_PASSWPRD_ACCOUNT);

                                    MailTo _mailToObj = new MailTo();
                                    string linkToken = string.Format("{0}{1}", GSIDSessionSiteInformation.UrlAddressSite, Url.Action("ResetPassword", "Account", new { language = obj.Language, token = model.ResetPasssToken }));
                                    _mailToObj.To = model.Email;
                                    _mailToObj.FullName = _mailSenderAccount.FullName;

                                    _mailToObj.Subject = _mailTemplate.SubjectVn;
                                    if (_mailToObj.Subject.IndexOf("{{fullname}}") > 0)
                                        _mailToObj.Subject = _mailToObj.Body.Replace("{{fullname}}", model.FullName);
                                    if (_mailToObj.Subject.IndexOf("{{phonenumber}}") > 0)
                                        _mailToObj.Subject = _mailToObj.Body.Replace("{{phonenumber}}", model.PhoneNumber);
                                    if (_mailToObj.Subject.IndexOf("{{linktoken}}") > 0)
                                        _mailToObj.Subject = _mailToObj.Body.Replace("{{linktoken}}", linkToken);

                                    _mailToObj.Body = _mailTemplate.BodyVn;
                                    if (_mailToObj.Body.IndexOf("{{fullname}}") > 0)
                                        _mailToObj.Body = _mailToObj.Body.Replace("{{fullname}}", model.FullName);
                                    if (_mailToObj.Body.IndexOf("{{phonenumber}}") > 0)
                                        _mailToObj.Body = _mailToObj.Body.Replace("{{phonenumber}}", model.PhoneNumber);
                                    if (_mailToObj.Body.IndexOf("{{linktoken}}") > 0)
                                        _mailToObj.Body = _mailToObj.Body.Replace("{{linktoken}}", linkToken);

                                    Mailer.SendMail(_mailSenderAccount, _mailToObj);
                                }
                                title = "Thông báo";
                                message = "Xin vui lòng xác nhận tài khoản qua email.";
                                status = Default.Status_Sucessfull;
                            }
                            else if (curentLan.Country == Language.LanguagueCountry.English)
                            {
                                var mailSenderPara = paraService.GetByCode(new EmailSTMPConfig().Code);
                                if (mailSenderPara != null)
                                {
                                    var _mailSenderAccount = JsonConvert.DeserializeObject<EmailSTMPConfig>(mailSenderPara.Content.ToString());
                                    var _mailTemplate = mailTemplateService.GetByCode(MailTemplateCode.FORGOT_PASSWPRD_ACCOUNT);

                                    MailTo _mailToObj = new MailTo();
                                    string linkToken = string.Format("{0}{1}", GSIDSessionSiteInformation.UrlAddressSite, Url.Action("ResetPassword", "Account", new { language = obj.Language, token = model.AccountVerifyToken }));
                                    _mailToObj.To = model.Email;
                                    _mailToObj.FullName = _mailSenderAccount.FullName;

                                    _mailToObj.Subject = _mailTemplate.SubjectEn;
                                    if (_mailToObj.Subject.IndexOf("{{fullname}}") > 0)
                                        _mailToObj.Subject = _mailToObj.Body.Replace("{{fullname}}", model.FullName);
                                    if (_mailToObj.Subject.IndexOf("{{phonenumber}}") > 0)
                                        _mailToObj.Subject = _mailToObj.Body.Replace("{{phonenumber}}", model.PhoneNumber);
                                    if (_mailToObj.Subject.IndexOf("{{linktoken}}") > 0)
                                        _mailToObj.Subject = _mailToObj.Body.Replace("{{linktoken}}", linkToken);

                                    _mailToObj.Body = _mailTemplate.BodyEn;
                                    if (_mailToObj.Body.IndexOf("{{fullname}}") > 0)
                                        _mailToObj.Body = _mailToObj.Body.Replace("{{fullname}}", model.FullName);
                                    if (_mailToObj.Body.IndexOf("{{phonenumber}}") > 0)
                                        _mailToObj.Body = _mailToObj.Body.Replace("{{phonenumber}}", model.PhoneNumber);
                                    if (_mailToObj.Body.IndexOf("{{linktoken}}") > 0)
                                        _mailToObj.Body = _mailToObj.Body.Replace("{{linktoken}}", linkToken);

                                    Mailer.SendMail(_mailSenderAccount, _mailToObj);
                                }
                                title = "Notification";
                                message = "Please confirm your account by email.";
                                status = Default.Status_Sucessfull;
                            }
                        }
                        else
                        {
                            title = (curentLan.Country == Language.LanguagueCountry.Vietmamese ? "Thông báo" : (curentLan.Country == Language.LanguagueCountry.English ? "Notification" : ""));
                            message = (curentLan.Country == Language.LanguagueCountry.Vietmamese ? "Email của bạn đã không tồn tại. Vui lòng đăng nhập!" : (curentLan.Country == Language.LanguagueCountry.English ? "Your email does not exist. please log in!" : ""));
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
            }

            return Json(new
            {
                Title = title,
                Message = message,
                Status = status,
                Url = url
            }, JsonRequestBehavior.AllowGet);
        }

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult ResetPassword(string language, string token, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            HomePageManagementAdminConfig homePageConfig = new HomePageManagementAdminConfig();
            var paraHomePageConfig = paraService.GetByCode(new HomePageManagementAdminConfig().Code);
            if (paraHomePageConfig != null)
                homePageConfig = JsonConvert.DeserializeObject<HomePageManagementAdminConfig>(paraHomePageConfig.Content.ToString());
            ViewBag.HomePageConfig = homePageConfig;
            ResetPasswordViewModel model = new ResetPasswordViewModel();
            model.Language = language;

            if (!string.IsNullOrEmpty(token))
            {
                token = token.Trim().ToLower();
                Customer _cusObj = customerService.GetByTokenResetPassword(token);
                if (_cusObj != null)
                {
                    model.Token = token;
                    model.Language = language;
                }
            }

            DefineRouterValueLanguages(language, new { token = token, returnUrl = returnUrl }, new { token = token, returnUrl = returnUrl });
            return View(model);
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordViewModel obj, string returnUrl)
        {
            string title = "";
            string message = "";
            string status = Default.Status_Error;
            string url = (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                                                && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                                        ? returnUrl : string.Format("/{0}", obj.Language);

            var languages = (List<Language>)System.Web.HttpContext.Current.Application[SestionName.Languages];
            Language curentLan = (languages != null && languages.Count > 0) ? languages.Where(l => l.Parent == obj.Language).FirstOrDefault() : null;

            if (curentLan != null)
            {
                title = (curentLan.Country == Language.LanguagueCountry.Vietmamese ? "Lỗi" : (curentLan.Country == Language.LanguagueCountry.English ? "Error" : ""));
                message = (curentLan.Country == Language.LanguagueCountry.Vietmamese ? "Dữ liệu gặp lỗi. Xin vui lòng kiểm tra lại." : (curentLan.Country == Language.LanguagueCountry.English ? "Data error. Please check again." : ""));

                try
                {
                    if (ModelState.IsValid)
                    {
                        string token = obj.Token.Trim().ToLower();
                        Customer model = customerService.GetByTokenResetPassword(token);
                        if (model != null)
                        {
                            model.ResetPasssToken = "";
                            model.PasswordSalt = RandomStringGenerator.RandomString();
                            model.Password = PasswordHelper.GenerateHashedPassword(obj.Password.Trim().ToLower(), model.PasswordSalt);
                            customerService.Update(model);
                            if (curentLan.Country == Language.LanguagueCountry.Vietmamese)
                            {
                                title = "Thông báo";
                                message = "Mật khẩu đã cập nhật thành công. Xin vui lòng đăng nhập hệ thống.";
                                status = Default.Status_Sucessfull;
                            }
                            else if (curentLan.Country == Language.LanguagueCountry.English)
                            {

                                title = "Notification";
                                message = "Password has been updated successfully. Please login to the system.";
                                status = Default.Status_Sucessfull;
                            }
                        }
                        else
                        {
                            title = (curentLan.Country == Language.LanguagueCountry.Vietmamese ? "Thông báo" : (curentLan.Country == Language.LanguagueCountry.English ? "Notification" : ""));
                            message = (curentLan.Country == Language.LanguagueCountry.Vietmamese ? "Email của bạn đã không tồn tại. Vui lòng đăng nhập!" : (curentLan.Country == Language.LanguagueCountry.English ? "Your email does not exist. please log in!" : ""));
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
        public ActionResult LogOff(string language)
        {
            FormsService.SignOut();
            string returnLogin = Url.Action("Login", "Account", new { language = language });
            return RedirectToLocal(returnLogin);
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