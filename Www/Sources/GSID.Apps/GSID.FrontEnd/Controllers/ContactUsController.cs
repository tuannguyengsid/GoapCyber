using GSID.FrontEnd.Models;
using GSID.FrontEnd.ViewModels;
using GSID.Model.ExtraEntities;
using GSID.Service.MongoRepositories.Service;
using GSID.Setting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GSID.Web.Core.Extensions;
using GSID.Model.MongodbModels;
using GSID.FrontEnd.Helpers;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using GSID.Web.Core.Helpers;

namespace GSID.FrontEnd.Controllers
{
    public class ContactUsController : BaseController
    {
        private readonly IRouteDataUrlService routeDataUrlService;
        private readonly IParameterService paraService;
        private readonly IContactMessageService contactMessageService;
        private readonly IContactService contactService;
        private readonly INewsService newsService;
        private readonly ITagPostService tagPostService;
        private readonly IMailTemplateService mailTemplateService;
        public ContactUsController(IRouteDataUrlService _routeDataUrlService, 
                                    IParameterService _paraService,
                                    IContactMessageService _contactMessageService, 
                                    IContactService _contactService,
                                    IMailTemplateService _mailTemplateService,
                                    INewsService _newsService,
                                    ITagPostService _tagPostService)
        {
            routeDataUrlService = _routeDataUrlService;
            paraService = _paraService;
            newsService = _newsService;
            tagPostService = _tagPostService;
            contactService = _contactService;
            contactMessageService = _contactMessageService;
            mailTemplateService = _mailTemplateService;
        }

        public ActionResult Index(string language = "")
        {
            HomePageManagementAdminConfig modelHomepage = new HomePageManagementAdminConfig();
            var paraHomePageConfig = paraService.GetByCode(new HomePageManagementAdminConfig().Code);
            if (paraHomePageConfig != null)
            {
                modelHomepage = JsonConvert.DeserializeObject<HomePageManagementAdminConfig>(paraHomePageConfig.Content.ToString());
                modelHomepage.RouteDataUrlVn = routeDataUrlService.GetBy(modelHomepage.RouteDataUrlVnId ?? "");
                modelHomepage.RouteDataUrlEn = routeDataUrlService.GetBy(modelHomepage.RouteDataUrlEnId ?? "");
            }
            ViewBag.HomePageConfig = modelHomepage;

            ContactUsPageManagementAdminConfig modelContactUs = new ContactUsPageManagementAdminConfig();
            var paraAboutPageConfig = paraService.GetByCode(new ContactUsPageManagementAdminConfig().Code);
            if (paraAboutPageConfig != null)
            {
                modelContactUs = JsonConvert.DeserializeObject<ContactUsPageManagementAdminConfig>(paraAboutPageConfig.Content.ToString());
                modelContactUs.RouteDataUrlVn = routeDataUrlService.GetBy(modelContactUs.RouteDataUrlVnId ?? "");
                modelContactUs.RouteDataUrlEn = routeDataUrlService.GetBy(modelContactUs.RouteDataUrlEnId ?? "");

                DefineRouterValueLanguages(language, modelContactUs.RouteDataUrlVn.Url, modelContactUs.RouteDataUrlEn.Url);
            }

            return View(modelContactUs);
        }

        public ActionResult ContactMessage(string language = "")
        {
            ContactMessageViewModel model = new ContactMessageViewModel();
            model.Language = language;
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult ContactMessage([Bind(Include = "FullName, Email, PhoneNumber, Message, Language")] ContactMessageViewModel obj)
        {
            var languages = (List<Language>)System.Web.HttpContext.Current.Application[SestionName.Languages];
            Language curentLan = (languages != null && languages.Count > 0) ? languages.Where(l => l.Parent == obj.Language).FirstOrDefault() : null;
            string title    = "";
            string message  = "";
            string status   = Default.Status_Error;

            if (curentLan != null)
            {
                title   = (curentLan.Country == Language.LanguagueCountry.Vietmamese ? "Lỗi" : (curentLan.Country == Language.LanguagueCountry.English ? "Error" : ""));
                message = (curentLan.Country == Language.LanguagueCountry.Vietmamese ? "Dữ liệu gặp lỗi. Xin vui lòng kiểm tra lại." : (curentLan.Country == Language.LanguagueCountry.English ? "Data error. Please check again." : ""));
                try
                {
                    if (ModelState.IsValid)
                    {
                        ContactMessage model = new ContactMessage();
                        var contact = contactService.GetByEmail(obj.Email.ToLower());
                        if (contact == null)
                        {
                            contact = new Contact();
                            contact.Email       = !string.IsNullOrEmpty(obj.Email) ? obj.Email.Trim().ToLower() : "";
                            contact.PhoneNumber = obj.PhoneNumber;
                            contact.FullName    = !string.IsNullOrEmpty(obj.FullName) ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(obj.FullName.Trim()) : "";
                            contact.IsContact = true;
                            contact.Id = contactService.Create(contact);
                        }
                        if (contact != null)
                        {
                            model.ContactId = contact.Id;
                            model.Message = obj.Message;
                            model.IsDeleted = !model.IsDeleted;
                            //model.AddedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
                            contactMessageService.Create(model);

                            if (curentLan.Country == Language.LanguagueCountry.Vietmamese)
                            {
                                var mailSenderPara = paraService.GetByCode(new EmailSTMPConfig().Code);
                                if (mailSenderPara != null)
                                {
                                    var _mailSenderAccount = JsonConvert.DeserializeObject<EmailSTMPConfig>(mailSenderPara.Content.ToString());
                                    var _mailTemplate = mailTemplateService.GetByCode(MailTemplateCode.NOTIFICATION_CONTACTMESSAGE);

                                    MailTo _mailToObj = new MailTo();
                                    string linkBackEnd = string.Format("{0}/{1}", GSIDSessionSiteInformation.UrlBackEndSite, "ContactMessage");
                                    _mailToObj.To = _mailSenderAccount.EmailNotification;
                                    _mailToObj.FullName = _mailSenderAccount.FullName;
                                    string datenow = DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy");

                                    _mailToObj.Subject = _mailTemplate.SubjectVn;
                                    if (_mailToObj.Subject.IndexOf("{{fullname}}") > 0)
                                        _mailToObj.Subject = _mailToObj.Subject.Replace("{{fullname}}", contact.FullName);
                                    if (_mailToObj.Subject.IndexOf("{{phonenumber}}") > 0)
                                        _mailToObj.Subject = _mailToObj.Subject.Replace("{{phonenumber}}", contact.PhoneNumber);
                                    if (_mailToObj.Subject.IndexOf("{{email}}") > 0)
                                        _mailToObj.Subject = _mailToObj.Subject.Replace("{{email}}", contact.Email);
                                    if (_mailToObj.Subject.IndexOf("{{linktoken}}") > 0)
                                        _mailToObj.Subject = _mailToObj.Subject.Replace("{{linkbackend}}", linkBackEnd);
                                    if (_mailToObj.Subject.IndexOf("{{datetimenow}}") > 0)
                                        _mailToObj.Subject = _mailToObj.Subject.Replace("{{datetimenow}}", datenow);
                                    if (_mailToObj.Subject.IndexOf("{{message}}") > 0)
                                        _mailToObj.Subject = _mailToObj.Subject.Replace("{{message}}", model.Message);

                                    _mailToObj.Body = _mailTemplate.BodyVn;
                                    if (_mailToObj.Body.IndexOf("{{fullname}}") > 0)
                                        _mailToObj.Body = _mailToObj.Body.Replace("{{fullname}}", contact.FullName);
                                    if (_mailToObj.Body.IndexOf("{{phonenumber}}") > 0)
                                        _mailToObj.Body = _mailToObj.Body.Replace("{{phonenumber}}", contact.PhoneNumber);
                                    if (_mailToObj.Body.IndexOf("{{email}}") > 0)
                                        _mailToObj.Body = _mailToObj.Body.Replace("{{email}}", contact.Email);
                                    if (_mailToObj.Body.IndexOf("{{linktoken}}") > 0)
                                        _mailToObj.Body = _mailToObj.Body.Replace("{{linkbackend}}", linkBackEnd);
                                    if (_mailToObj.Body.IndexOf("{{datetimenow}}") > 0)
                                        _mailToObj.Body = _mailToObj.Body.Replace("{{datetimenow}}", datenow);
                                    if (_mailToObj.Body.IndexOf("{{message}}") > 0)
                                        _mailToObj.Body = _mailToObj.Body.Replace("{{message}}", model.Message);

                                    Mailer.SendMail(_mailSenderAccount, _mailToObj);
                                }
                            }
                            else if (curentLan.Country == Language.LanguagueCountry.English)
                            {
                                var mailSenderPara = paraService.GetByCode(new EmailSTMPConfig().Code);
                                if (mailSenderPara != null)
                                {
                                    var _mailSenderAccount = JsonConvert.DeserializeObject<EmailSTMPConfig>(mailSenderPara.Content.ToString());
                                    var _mailTemplate = mailTemplateService.GetByCode(MailTemplateCode.NOTIFICATION_CONTACTMESSAGE);

                                    MailTo _mailToObj = new MailTo();
                                    string linkBackEnd = string.Format("{0}/{1}", GSIDSessionSiteInformation.UrlBackEndSite, "ContactMessage");
                                    _mailToObj.To = _mailSenderAccount.EmailNotification;
                                    _mailToObj.FullName = _mailSenderAccount.FullName;
                                    string datenow = DateTime.Now.ToString("hh:mm:ss tt dd/MM/yyyy");

                                    _mailToObj.Subject = _mailTemplate.SubjectEn;
                                    if (_mailToObj.Subject.IndexOf("{{fullname}}") > 0)
                                        _mailToObj.Subject = _mailToObj.Subject.Replace("{{fullname}}", contact.FullName);
                                    if (_mailToObj.Subject.IndexOf("{{phonenumber}}") > 0)
                                        _mailToObj.Subject = _mailToObj.Subject.Replace("{{phonenumber}}", contact.PhoneNumber);
                                    if (_mailToObj.Subject.IndexOf("{{email}}") > 0)
                                        _mailToObj.Subject = _mailToObj.Subject.Replace("{{email}}", contact.Email);
                                    if (_mailToObj.Subject.IndexOf("{{linktoken}}") > 0)
                                        _mailToObj.Subject = _mailToObj.Subject.Replace("{{linkbackend}}", linkBackEnd);
                                    if (_mailToObj.Subject.IndexOf("{{datetimenow}}") > 0)
                                        _mailToObj.Subject = _mailToObj.Subject.Replace("{{datetimenow}}", datenow);
                                    if (_mailToObj.Subject.IndexOf("{{message}}") > 0)
                                        _mailToObj.Subject = _mailToObj.Subject.Replace("{{message}}", model.Message);

                                    _mailToObj.Body = _mailTemplate.BodyEn;
                                    if (_mailToObj.Body.IndexOf("{{fullname}}") > 0)
                                        _mailToObj.Body = _mailToObj.Body.Replace("{{fullname}}", contact.FullName);
                                    if (_mailToObj.Body.IndexOf("{{phonenumber}}") > 0)
                                        _mailToObj.Body = _mailToObj.Body.Replace("{{phonenumber}}", contact.PhoneNumber);
                                    if (_mailToObj.Body.IndexOf("{{email}}") > 0)
                                        _mailToObj.Body = _mailToObj.Body.Replace("{{email}}", contact.Email);
                                    if (_mailToObj.Body.IndexOf("{{linktoken}}") > 0)
                                        _mailToObj.Body = _mailToObj.Body.Replace("{{linkbackend}}", linkBackEnd);
                                    if (_mailToObj.Body.IndexOf("{{datetimenow}}") > 0)
                                        _mailToObj.Body = _mailToObj.Body.Replace("{{datetimenow}}", datenow);
                                    if (_mailToObj.Body.IndexOf("{{message}}") > 0)
                                        _mailToObj.Body = _mailToObj.Body.Replace("{{message}}", model.Message);

                                    Mailer.SendMail(_mailSenderAccount, _mailToObj);
                                }
                            }

                            title = (curentLan.Country == Language.LanguagueCountry.Vietmamese ? "Thông báo" : (curentLan.Country == Language.LanguagueCountry.English ? "Notification" : ""));
                            message = (curentLan.Country == Language.LanguagueCountry.Vietmamese ? "Đã gửi tin nhắn thành công. Chúng tôi sẽ kiểm tra và phản hồi sớm nhất đến bạn" : (curentLan.Country == Language.LanguagueCountry.English ? "Message sent successfully. We will check and respond to you as soon as possible" : ""));
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
            }

            return Json(new
            {
                Title = title,
                Message = message,
                Status = status
            }, JsonRequestBehavior.AllowGet);
        }
    }
}