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
using System.IO;
using GSID.Web.Core.Helpers;
using System.Globalization;

namespace GSID.FrontEnd.Controllers
{
    public class RecruitmentController : BaseAuthenticationController
    {
        private readonly IRouteDataUrlService routeDataUrlService;
        private readonly IRecruitmentService recruitmentService;
        private readonly IImageService imgService;
        private readonly IContactService contactService;
        private readonly ICurriculumVitaeService cvService;
        private readonly ICareerService careerService;
        private readonly ISiteService siteService;
        private readonly IPositionService positionService;
        private readonly IDepartmentService departmentService;
        private readonly IRecruitmentTagService recruitmentTagService;
        private readonly IMailTemplateService mailTemplateService;
        private readonly IParameterService paraService;
        string[] allowedExtensions = new[] { ".jpg", ".png", ".jpeg", ".gif", ".pdf", ".doc", ".docx", ".xls", ".xlsx" };

        public RecruitmentController(IRouteDataUrlService _routeDataUrlService, 
                                        IParameterService _paraService,
                                        IImageService _imgService,
                                        IContactService _contactService,
                                        ICurriculumVitaeService _cvService,
                                        IRecruitmentService _recruitmentService,
                                        ISiteService _siteService, 
                                        IPositionService _positionService,
                                        IDepartmentService _departmentService,
                                        IRecruitmentTagService _recruitmentTagService,
                                        IMailTemplateService _mailTemplateService,
                                        ICareerService _careerService) {

            routeDataUrlService = _routeDataUrlService; 
            paraService = _paraService;
            imgService = _imgService;
            recruitmentService = _recruitmentService;
            cvService = _cvService;
            contactService = _contactService;
            careerService = _careerService;
            siteService = _siteService;
            positionService = _positionService;
            departmentService = _departmentService;
            recruitmentTagService = _recruitmentTagService;
            mailTemplateService = _mailTemplateService;
        }

        public ActionResult Index(string language = "",string p = "")
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

            ImageLibraryPageManagementAdminConfig modelImageLibrary = new ImageLibraryPageManagementAdminConfig();
            var paraImageLibrary = paraService.GetByCode(new ImageLibraryPageManagementAdminConfig().Code);
            if (paraImageLibrary != null)
            {
                modelImageLibrary = JsonConvert.DeserializeObject<ImageLibraryPageManagementAdminConfig>(paraImageLibrary.Content.ToString());
                modelImageLibrary.RouteDataUrlVn = routeDataUrlService.GetBy(modelImageLibrary.RouteDataUrlVnId ?? "");
                modelImageLibrary.RouteDataUrlEn = routeDataUrlService.GetBy(modelImageLibrary.RouteDataUrlEnId ?? "");
            }
            ViewBag.ImageLibraryConfig = modelImageLibrary;


            RecruitmentViewModel model = null;
            RecruitmentPageManagementAdminConfig modelRecruitment = new RecruitmentPageManagementAdminConfig();
            var paraRecruitmentPageConfig = paraService.GetByCode(new RecruitmentPageManagementAdminConfig().Code);
            if (paraRecruitmentPageConfig != null)
            {
                modelRecruitment            = JsonConvert.DeserializeObject<RecruitmentPageManagementAdminConfig>(paraRecruitmentPageConfig.Content.ToString());
                modelRecruitment.RouteDataUrlVn = routeDataUrlService.GetBy(modelRecruitment.RouteDataUrlVnId ?? "");
                modelRecruitment.RouteDataUrlEn = routeDataUrlService.GetBy(modelRecruitment.RouteDataUrlEnId ?? "");

                modelRecruitment.Slider             = modelRecruitment.Slider != null ? modelRecruitment.Slider.OrderBy(o => o.Index).ToList(): new List<RecruitmentPageManagementAdminConfig.RecruitmentPageManagementSectionSliderAdminConfig>();
                modelRecruitment.AboutCompanyItems  = modelRecruitment.AboutCompanyItems != null ? modelRecruitment.AboutCompanyItems.OrderBy(o => o.Index).ToList() : new List<RecruitmentPageManagementAdminConfig.RecruitmentPageManagementSectionAboutCompanyAdminConfig>();
                modelRecruitment.WhyChooseItems     = modelRecruitment.WhyChooseItems != null ? modelRecruitment.WhyChooseItems.OrderBy(o => o.Index).ToList() : new List<RecruitmentPageManagementAdminConfig.RecruitmentPageManagementSectionWhyChooseAdminConfig>();
                ViewBag.RecruitmentPage             = modelRecruitment;
                ViewBag.Images      = imgService.GetAllByLatest(4, false);

                model               = new RecruitmentViewModel();
                model.Careers       = careerService.GetAllShowRecruitmentPage(true, false);
                model.Recruitments  = recruitmentService.GetAllByExpirationDate(false);

                model.PageIndex     = p.ConvertIntPaging();
                model.PageSize      = modelRecruitment.RecruitmentPagingItem;
                model.CountTotal    = model.Recruitments.Count();
                model.CurrentPage   = model.PageIndex;
                model.PageVisit     = model.PageVisit;
                model.TotalPage     = (Math.Ceiling((double)model.CountTotal / model.PageSize));
                model.Recruitments  = model.Recruitments.Skip(model.PageSize * (model.PageIndex - 1))
                                            .Take(model.PageSize)
                                                .OrderByDescending(c => c.AddedByDate ?? DateTime.MaxValue)
                                                    .ToList();

                DefineRouterValueLanguages(language, modelRecruitment.RouteDataUrlVn.Url, modelRecruitment.RouteDataUrlEn.Url);
            }

            return View(model);
        }

        public ActionResult Career(string language = "", string urlRouteId = "", string p = "")
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

            ImageLibraryPageManagementAdminConfig modelImageLibrary = new ImageLibraryPageManagementAdminConfig();
            var paraImageLibrary = paraService.GetByCode(new ImageLibraryPageManagementAdminConfig().Code);
            if (paraImageLibrary != null)
            {
                modelImageLibrary = JsonConvert.DeserializeObject<ImageLibraryPageManagementAdminConfig>(paraImageLibrary.Content.ToString());
                modelImageLibrary.RouteDataUrlVn = routeDataUrlService.GetBy(modelImageLibrary.RouteDataUrlVnId ?? "");
                modelImageLibrary.RouteDataUrlEn = routeDataUrlService.GetBy(modelImageLibrary.RouteDataUrlEnId ?? "");
            }
            ViewBag.ImageLibraryConfig = modelImageLibrary;


            RecruitmentViewModel model = new RecruitmentViewModel();
            var languages = (List<Language>)System.Web.HttpContext.Current.Application[SestionName.Languages];
            Language curentLan = (languages != null && languages.Count > 0) ? languages.Where(l => l.Parent == language).FirstOrDefault() : null;

            if (curentLan != null)
            {
                RecruitmentPageManagementAdminConfig modelRecruitment = new RecruitmentPageManagementAdminConfig();
                var paraRecruitmentPageConfig = paraService.GetByCode(new RecruitmentPageManagementAdminConfig().Code);
                if (paraRecruitmentPageConfig != null)
                {
                    modelRecruitment = JsonConvert.DeserializeObject<RecruitmentPageManagementAdminConfig>(paraRecruitmentPageConfig.Content.ToString()); modelRecruitment.Slider = modelRecruitment.Slider != null ? modelRecruitment.Slider.OrderBy(o => o.Index).ToList() : new List<RecruitmentPageManagementAdminConfig.RecruitmentPageManagementSectionSliderAdminConfig>();
                    modelRecruitment.RouteDataUrlVn = routeDataUrlService.GetBy(modelRecruitment.RouteDataUrlVnId ?? "");
                    modelRecruitment.RouteDataUrlEn = routeDataUrlService.GetBy(modelRecruitment.RouteDataUrlEnId ?? "");

                    modelRecruitment.AboutCompanyItems = modelRecruitment.AboutCompanyItems != null ? modelRecruitment.AboutCompanyItems.OrderBy(o => o.Index).ToList() : new List<RecruitmentPageManagementAdminConfig.RecruitmentPageManagementSectionAboutCompanyAdminConfig>();
                    modelRecruitment.WhyChooseItems = modelRecruitment.WhyChooseItems != null ? modelRecruitment.WhyChooseItems.OrderBy(o => o.Index).ToList() : new List<RecruitmentPageManagementAdminConfig.RecruitmentPageManagementSectionWhyChooseAdminConfig>();
                    ViewBag.RecruitmentPage = modelRecruitment;
                    ViewBag.Images = imgService.GetAllByLatest(4, false);

                    model = new RecruitmentViewModel();
                    model.Careers = careerService.GetAllShowRecruitmentPage(true, false); 
                    model.Career = (curentLan.Country == Language.LanguagueCountry.Vietmamese) ? careerService.GetRouterVn(urlRouteId) : careerService.GetRouterEn(urlRouteId);
                    if (model.Career != null)
                    {
                        model.Recruitments = model.Career != null ? recruitmentService.GetAllByExpirationDate(model.Career.Id, false) : new List<Recruitment>();

                        model.PageIndex     = p.ConvertIntPaging();
                        model.PageSize      = modelRecruitment.RecruitmentPagingItem;
                        model.CountTotal    = model.Recruitments.Count();
                        model.CurrentPage   = model.PageIndex;
                        model.PageVisit     = model.PageVisit;
                        model.TotalPage     = (Math.Ceiling((double)model.CountTotal / model.PageSize));
                        model.Recruitments  = model.Recruitments.Skip(model.PageSize * (model.PageIndex - 1))
                                                    .Take(model.PageSize)
                                                        .OrderByDescending(c => c.AddedByDate ?? DateTime.MaxValue)
                                                            .ToList();

                        DefineRouterValueLanguages(language, model.Career.RouteDataUrlVn.Url, model.Career.RouteDataUrlEn.Url);
                    }
                }
            }
            return View(model);
        }

        public ActionResult GetAllByFilterCareer(string language = "", string careerurlid = "", string p = "")
        {
            RecruitmentViewModel model = new RecruitmentViewModel();
            var languages = (List<Language>)System.Web.HttpContext.Current.Application[SestionName.Languages];
            Language curentLan = (languages != null && languages.Count > 0) ? languages.Where(l => l.Parent == language).FirstOrDefault() : null;

            if (curentLan != null)
            {
                RecruitmentPageManagementAdminConfig modelRecruitment = new RecruitmentPageManagementAdminConfig();
                var paraRecruitmentPageConfig = paraService.GetByCode(new RecruitmentPageManagementAdminConfig().Code);
                if (paraRecruitmentPageConfig != null)
                {
                    modelRecruitment = JsonConvert.DeserializeObject<RecruitmentPageManagementAdminConfig>(paraRecruitmentPageConfig.Content.ToString());
                    model = new RecruitmentViewModel();

                    if (!string.IsNullOrEmpty(careerurlid) && careerurlid != "*")
                    {
                        model.Career = (curentLan.Country == Language.LanguagueCountry.Vietmamese) ? careerService.GetRouterVn(careerurlid) : careerService.GetRouterEn(careerurlid);
                        model.Recruitments = model.Career != null ? recruitmentService.GetAllByExpirationDate(model.Career.Id, false) : new List<Recruitment>();
                    }
                    else
                    {
                        model.Recruitments = recruitmentService.GetAllByExpirationDate(false);
                    }

                    model.PageIndex = p.ConvertIntPaging();
                    model.PageSize = modelRecruitment.RecruitmentPagingItem;
                    model.CountTotal = model.Recruitments.Count();
                    model.CurrentPage = model.PageIndex;
                    model.PageVisit = model.PageVisit;
                    model.TotalPage = (Math.Ceiling((double)model.CountTotal / model.PageSize));
                    model.Recruitments = model.Recruitments.Skip(model.PageSize * (model.PageIndex - 1))
                                                .Take(model.PageSize)
                                                    .OrderByDescending(c => c.AddedByDate ?? DateTime.MaxValue)
                                                        .ToList();
                }
            }
            return PartialView(model);
        }

        public ActionResult Detail(string language = "", string urlRouteId = "", string returnUrl = "")
        {
            RecruitmentDetailViewModel model = null;

            var languages = (List<Language>)System.Web.HttpContext.Current.Application[SestionName.Languages];
            Language curentLan = (languages != null && languages.Count > 0) ? languages.Where(l => l.Parent == language).FirstOrDefault() : null;
            if (curentLan != null)
            {
                RecruitmentDetailPageManagementAdminConfig modelDetailRecruitment = new RecruitmentDetailPageManagementAdminConfig();
                var paraRecruitmentDetailPageConfig = paraService.GetByCode(new RecruitmentDetailPageManagementAdminConfig().Code);
                if (paraRecruitmentDetailPageConfig != null)
                    modelDetailRecruitment = JsonConvert.DeserializeObject<RecruitmentDetailPageManagementAdminConfig>(paraRecruitmentDetailPageConfig.Content.ToString());
               
                ViewBag.RecruitmentDetailPage = modelDetailRecruitment;

                if (curentLan.Country == Language.LanguagueCountry.Vietmamese)
                {
                    var recruitment = recruitmentService.GetRouterVn(urlRouteId);
                    if (recruitment != null)
                    {
                        model = new RecruitmentDetailViewModel();
                        model.returnUrl = !string.IsNullOrEmpty(returnUrl) ? returnUrl : Url.Action("Index", "Recruitment", new { language = language });
                        model.Recruitment = recruitment;
                        model.Career = recruitment.Career;
                        model.Site = !string.IsNullOrEmpty(recruitment.SiteId) ? siteService.GetBy(recruitment.SiteId) : null;
                        model.Position = !string.IsNullOrEmpty(recruitment.PositionId) ? positionService.GetBy(recruitment.PositionId) : null;
                        model.Department = !string.IsNullOrEmpty(recruitment.DepartmentId) ? departmentService.GetBy(recruitment.DepartmentId) : null;
                        model.RecruitmentTag = !string.IsNullOrEmpty(recruitment.RecruitmentTagId) ? recruitmentTagService.GetBy(recruitment.RecruitmentTagId) : null;
                        model.Related = recruitmentService.GetAllByRelated(recruitment, 10, false);

                        DefineRouterValueLanguages(language, recruitment.RouteDataUrlVn.Url, recruitment.RouteDataUrlEn.Url);
                    }
                }
                else if (curentLan.Country == Language.LanguagueCountry.English)
                {
                    var recruitment = recruitmentService.GetRouterVn(urlRouteId);
                    if (recruitment != null)
                    {
                        model = new RecruitmentDetailViewModel();
                        model.returnUrl = !string.IsNullOrEmpty(returnUrl) ? returnUrl : Url.Action("Index", "Recruitment", new { language = language });
                        model.Recruitment = recruitment;
                        model.Career = recruitment.Career;
                        model.Site = !string.IsNullOrEmpty(recruitment.SiteId) ? siteService.GetBy(recruitment.SiteId) : null;
                        model.Position = !string.IsNullOrEmpty(recruitment.PositionId) ? positionService.GetBy(recruitment.PositionId) : null;
                        model.Department = !string.IsNullOrEmpty(recruitment.DepartmentId) ? departmentService.GetBy(recruitment.DepartmentId) : null;
                        model.RecruitmentTag = !string.IsNullOrEmpty(recruitment.RecruitmentTagId) ? recruitmentTagService.GetBy(recruitment.RecruitmentTagId) : null;
                        model.Related = recruitmentService.GetAllByRelated(recruitment, 10, false);

                        DefineRouterValueLanguages(language, recruitment.RouteDataUrlVn.Url, recruitment.RouteDataUrlEn.Url);
                    }
                }
            }

            return View(model);
        }
        
        public ActionResult RecruitmentSubmit(string language = "")
        {
            RecruitmentSubmitViewModel model = new RecruitmentSubmitViewModel();
            model.Careers   = careerService.GetAll(false);
            model.Language  = language;
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult RecruitmentSubmit([Bind(Include = "FullName, Email, PhoneNumber, CareerId, Language, FileUpload")] RecruitmentSubmitViewModel obj)
        {
            var languages = (List<Language>)System.Web.HttpContext.Current.Application[SestionName.Languages];
            Language curentLan = (languages != null && languages.Count > 0) ? languages.Where(l => l.Parent == obj.Language).FirstOrDefault() : null;
            string title    = "";
            string message  = "";
            string status   = Default.Status_Error;

            if (curentLan != null)
            {
                title = (curentLan.Country == Language.LanguagueCountry.Vietmamese ? "Lỗi" : (curentLan.Country == Language.LanguagueCountry.English ? "Error" : ""));
                message = (curentLan.Country == Language.LanguagueCountry.Vietmamese ? "Dữ liệu gặp lỗi. Xin vui lòng kiểm tra lại." : (curentLan.Country == Language.LanguagueCountry.English ? "Data error. Please check again." : ""));
                try
                {
                    if (ModelState.IsValid && obj.FileUpload != null && obj.FileUpload.ContentLength != 0)
                    {
                        var extension = Path.GetExtension(obj.FileUpload.FileName).ToLower();

                        if (allowedExtensions.Contains(extension))
                        {
                            var contact = contactService.GetByEmail(obj.Email.ToLower());
                            if (contact == null)
                            {
                                contact = new Contact();
                                contact.Email = !string.IsNullOrEmpty(obj.Email) ? obj.Email.Trim().ToLower() : "";
                                contact.FullName = !string.IsNullOrEmpty(obj.FullName) ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(obj.FullName.Trim()) : "";
                                contact.PhoneNumber = obj.PhoneNumber;
                                contact.IsContact = true;
                                contact.Id = contactService.Create(contact);
                            }
                            if (contact != null)
                            {
                                CurriculumVitae model = new CurriculumVitae();
                                model.ContactId = contact.Id;
                                model.CareerId  = obj.CareerId;
                                model.IsDeleted = false;
                                model.FileSrc   = UploadFile(obj.FileUpload, extension);

                                string curriculumVitaeId = cvService.Create(model);
                                if (!string.IsNullOrEmpty(curriculumVitaeId))
                                {
                                    if (curentLan.Country == Language.LanguagueCountry.Vietmamese)
                                    {
                                        var mailSenderPara = paraService.GetByCode(new EmailSTMPConfig().Code);
                                        if (mailSenderPara != null)
                                        {
                                            var _mailSenderAccount = JsonConvert.DeserializeObject<EmailSTMPConfig>(mailSenderPara.Content.ToString());
                                            var _mailTemplate = mailTemplateService.GetByCode(MailTemplateCode.NOTIFICATION_CURRICULUMVITAE);

                                            if (_mailSenderAccount != null && _mailTemplate != null)
                                            {
                                                MailTo _mailToObj = new MailTo();
                                                string linkBackEnd = string.Format("{0}/{1}", GSIDSessionSiteInformation.UrlBackEndSite, "CurriculumVitae");
                                                _mailToObj.To = _mailSenderAccount.EmailNotification;
                                                _mailToObj.FullName = _mailSenderAccount.FullName;
                                                string datenow = DateTime.Now.ToString("hh:mm:ss tt dd/MM/yyyy");

                                                _mailToObj.Subject = _mailTemplate.SubjectVn;
                                                if (_mailToObj.Subject.IndexOf("{{fullname}}") > 0)
                                                    _mailToObj.Subject = _mailToObj.Subject.Replace("{{fullname}}", contact.FullName);
                                                if (_mailToObj.Subject.IndexOf("{{phonenumber}}") > 0)
                                                    _mailToObj.Subject = _mailToObj.Subject.Replace("{{phonenumber}}", contact.PhoneNumber);
                                                if (_mailToObj.Subject.IndexOf("{{email}}") > 0)
                                                    _mailToObj.Subject = _mailToObj.Subject.Replace("{{email}}", contact.Email);
                                                if (_mailToObj.Subject.IndexOf("{{linkbackend}}") > 0)
                                                    _mailToObj.Subject = _mailToObj.Subject.Replace("{{linkbackend}}", linkBackEnd);
                                                if (_mailToObj.Subject.IndexOf("{{datetimenow}}") > 0)
                                                    _mailToObj.Subject = _mailToObj.Subject.Replace("{{datetimenow}}", linkBackEnd);

                                                _mailToObj.Body = _mailTemplate.BodyVn;
                                                if (_mailToObj.Body.IndexOf("{{fullname}}") > 0)
                                                    _mailToObj.Body = _mailToObj.Body.Replace("{{fullname}}", contact.FullName);
                                                if (_mailToObj.Body.IndexOf("{{phonenumber}}") > 0)
                                                    _mailToObj.Body = _mailToObj.Body.Replace("{{phonenumber}}", contact.PhoneNumber);
                                                if (_mailToObj.Body.IndexOf("{{linkbackend}}") > 0)
                                                    _mailToObj.Body = _mailToObj.Body.Replace("{{linkbackend}}", linkBackEnd);
                                                if (_mailToObj.Body.IndexOf("{{datetimenow}}") > 0)
                                                    _mailToObj.Body = _mailToObj.Body.Replace("{{datetimenow}}", datenow);

                                                Mailer.SendMail(_mailSenderAccount, _mailToObj);
                                            }
                                        }
                                    }
                                    else if (curentLan.Country == Language.LanguagueCountry.English)
                                    {
                                        var mailSenderPara = paraService.GetByCode(new EmailSTMPConfig().Code);
                                        if (mailSenderPara != null)
                                        {
                                            var _mailSenderAccount = JsonConvert.DeserializeObject<EmailSTMPConfig>(mailSenderPara.Content.ToString());
                                            var _mailTemplate = mailTemplateService.GetByCode(MailTemplateCode.NOTIFICATION_CURRICULUMVITAE);

                                            if (_mailSenderAccount != null && _mailTemplate != null)
                                            {
                                                MailTo _mailToObj = new MailTo();
                                                string linkBackEnd = string.Format("{0}/{1}", GSIDSessionSiteInformation.UrlBackEndSite, "CurriculumVitae");
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
                                                if (_mailToObj.Subject.IndexOf("{{linkbackend}}") > 0)
                                                    _mailToObj.Subject = _mailToObj.Subject.Replace("{{linkbackend}}", linkBackEnd);
                                                if (_mailToObj.Subject.IndexOf("{{datetimenow}}") > 0)
                                                    _mailToObj.Subject = _mailToObj.Subject.Replace("{{datetimenow}}", linkBackEnd);

                                                _mailToObj.Body = _mailTemplate.BodyEn;
                                                if (_mailToObj.Body.IndexOf("{{fullname}}") > 0)
                                                    _mailToObj.Body = _mailToObj.Body.Replace("{{fullname}}", contact.FullName);
                                                if (_mailToObj.Body.IndexOf("{{phonenumber}}") > 0)
                                                    _mailToObj.Body = _mailToObj.Body.Replace("{{phonenumber}}", contact.PhoneNumber);
                                                if (_mailToObj.Body.IndexOf("{{email}}") > 0)
                                                    _mailToObj.Body = _mailToObj.Body.Replace("{{email}}", contact.Email);
                                                if (_mailToObj.Body.IndexOf("{{linkbackend}}") > 0)
                                                    _mailToObj.Body = _mailToObj.Body.Replace("{{linkbackend}}", linkBackEnd);
                                                if (_mailToObj.Body.IndexOf("{{datetimenow}}") > 0)
                                                    _mailToObj.Body = _mailToObj.Body.Replace("{{datetimenow}}", datenow);

                                                Mailer.SendMail(_mailSenderAccount, _mailToObj);
                                            }
                                        }
                                    }
                                }
                                title = (curentLan.Country == Language.LanguagueCountry.Vietmamese ? "Thông báo" : (curentLan.Country == Language.LanguagueCountry.English ? "Notification" : ""));
                                message = (curentLan.Country == Language.LanguagueCountry.Vietmamese ? "Đã gửi tin nhắn thành công. Chúng tôi sẽ kiểm tra và phản hồi sớm nhất đến bạn." : (curentLan.Country == Language.LanguagueCountry.English ? "Message sent successfully. We will check and respond to you as soon as possible" : ""));
                                status = Default.Status_Sucessfull;
                            }
                        }
                        else
                        {
                            message = (curentLan.Country == Language.LanguagueCountry.Vietmamese ? "Xin vui lòng chèn tập tin đúng định dạng: .jpg, .png, .jpeg, .gif, .pdf, .doc, .docx, .xls, .xlsx" : (curentLan.Country == Language.LanguagueCountry.English ? "Please insert the file in the correct format: .jpg, .png, .jpeg, .gif, .pdf, .doc, .docx, .xls, .xlsx" : ""));
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

        public ActionResult RecruitmentDetailSubmit(string language = "", string CareerId = "", string RecruitmentId = "", string returnUrl = "")
        {
            RecruitmentDetailSubmitViewModel model = new RecruitmentDetailSubmitViewModel();
            model.returnUrl     = returnUrl;
            model.Careers       = careerService.GetAll(false);
            model.CareerId      = CareerId;
            model.RecruitmentId = RecruitmentId;
            model.Language      = language;
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult RecruitmentDetailSubmit([Bind(Include = "FullName, Email, PhoneNumber, CareerId, RecruitmentId, Language, FileUpload")] RecruitmentDetailSubmitViewModel obj)
        {
            var languages = (List<Language>)System.Web.HttpContext.Current.Application[SestionName.Languages];
            Language curentLan = (languages != null && languages.Count > 0) ? languages.Where(l => l.Parent == obj.Language).FirstOrDefault() : null;
            string title    = "";
            string message  = "";
            string status   = Default.Status_Error;

            if (curentLan != null)
            {
                title = (curentLan.Country == Language.LanguagueCountry.Vietmamese ? "Lỗi" : (curentLan.Country == Language.LanguagueCountry.English ? "Error" : ""));
                message = (curentLan.Country == Language.LanguagueCountry.Vietmamese ? "Dữ liệu gặp lỗi. Xin vui lòng kiểm tra lại." : (curentLan.Country == Language.LanguagueCountry.English ? "Data error. Please check again." : ""));
                try
                {
                    if (ModelState.IsValid && obj.FileUpload != null && obj.FileUpload.ContentLength != 0)
                    {
                        var extension = Path.GetExtension(obj.FileUpload.FileName).ToLower();

                        if (allowedExtensions.Contains(extension))
                        {
                            var contact = contactService.GetByEmail(obj.Email.ToLower());
                            if (contact == null)
                            {
                                contact = new Contact();
                                contact.Email       = !string.IsNullOrEmpty(obj.Email) ? obj.Email.Trim().ToLower() : "";
                                contact.FullName    = !string.IsNullOrEmpty(obj.FullName) ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(obj.FullName.Trim()) : "";
                                contact.PhoneNumber = obj.PhoneNumber;
                                contact.IsContact   = true;
                                contact.Id          = contactService.Create(contact);
                            }
                            if (contact != null)
                            {
                                CurriculumVitae model = new CurriculumVitae();
                                model.ContactId     = contact.Id;
                                model.CareerId      = obj.CareerId;
                                model.RecruitmentId = obj.RecruitmentId;
                                model.IsDeleted     = false;
                                model.FileSrc       = UploadFile(obj.FileUpload, extension);

                                string curriculumVitaeId = cvService.Create(model);

                                if (!string.IsNullOrEmpty(curriculumVitaeId))
                                {
                                    if (curentLan.Country == Language.LanguagueCountry.Vietmamese)
                                    {
                                        var mailSenderPara = paraService.GetByCode(new EmailSTMPConfig().Code);
                                        if (mailSenderPara != null)
                                        {
                                            var _mailSenderAccount = JsonConvert.DeserializeObject<EmailSTMPConfig>(mailSenderPara.Content.ToString());
                                            var _mailTemplate = mailTemplateService.GetByCode(MailTemplateCode.NOTIFICATION_CURRICULUMVITAE);

                                            if (_mailSenderAccount != null && _mailTemplate != null)
                                            {
                                                MailTo _mailToObj = new MailTo();
                                                string linkBackEnd = string.Format("{0}/{1}", GSIDSessionSiteInformation.UrlBackEndSite, "CurriculumVitae");
                                                _mailToObj.To = _mailSenderAccount.EmailNotification;
                                                _mailToObj.FullName = _mailSenderAccount.FullName;
                                                string datenow = DateTime.Now.ToString("hh:mm:ss tt dd/MM/yyyy");

                                                _mailToObj.Subject = _mailTemplate.SubjectVn;
                                                if (_mailToObj.Subject.IndexOf("{{fullname}}") > 0)
                                                    _mailToObj.Subject = _mailToObj.Subject.Replace("{{fullname}}", contact.FullName);
                                                if (_mailToObj.Subject.IndexOf("{{phonenumber}}") > 0)
                                                    _mailToObj.Subject = _mailToObj.Subject.Replace("{{phonenumber}}", contact.PhoneNumber);
                                                if (_mailToObj.Subject.IndexOf("{{email}}") > 0)
                                                    _mailToObj.Subject = _mailToObj.Subject.Replace("{{email}}", contact.Email);
                                                if (_mailToObj.Subject.IndexOf("{{linkbackend}}") > 0)
                                                    _mailToObj.Subject = _mailToObj.Subject.Replace("{{linkbackend}}", linkBackEnd);
                                                if (_mailToObj.Subject.IndexOf("{{datetimenow}}") > 0)
                                                    _mailToObj.Subject = _mailToObj.Subject.Replace("{{datetimenow}}", linkBackEnd);

                                                _mailToObj.Body = _mailTemplate.BodyVn;
                                                if (_mailToObj.Body.IndexOf("{{fullname}}") > 0)
                                                    _mailToObj.Body = _mailToObj.Body.Replace("{{fullname}}", contact.FullName);
                                                if (_mailToObj.Body.IndexOf("{{phonenumber}}") > 0)
                                                    _mailToObj.Body = _mailToObj.Body.Replace("{{phonenumber}}", contact.PhoneNumber);
                                                if (_mailToObj.Body.IndexOf("{{email}}") > 0)
                                                    _mailToObj.Body = _mailToObj.Body.Replace("{{email}}", contact.Email);
                                                if (_mailToObj.Body.IndexOf("{{linkbackend}}") > 0)
                                                    _mailToObj.Body = _mailToObj.Body.Replace("{{linkbackend}}", linkBackEnd);
                                                if (_mailToObj.Body.IndexOf("{{datetimenow}}") > 0)
                                                    _mailToObj.Body = _mailToObj.Body.Replace("{{datetimenow}}", datenow);

                                                Mailer.SendMail(_mailSenderAccount, _mailToObj);
                                            }
                                        }
                                    }
                                    else if (curentLan.Country == Language.LanguagueCountry.English)
                                    {
                                        var mailSenderPara = paraService.GetByCode(new EmailSTMPConfig().Code);
                                        if (mailSenderPara != null)
                                        {
                                            var _mailSenderAccount = JsonConvert.DeserializeObject<EmailSTMPConfig>(mailSenderPara.Content.ToString());
                                            var _mailTemplate = mailTemplateService.GetByCode(MailTemplateCode.NOTIFICATION_CURRICULUMVITAE);
                                            if (_mailSenderAccount != null && _mailTemplate != null)
                                            {
                                                MailTo _mailToObj = new MailTo();
                                                string linkBackEnd = string.Format("{0}/{1}", GSIDSessionSiteInformation.UrlBackEndSite, "CurriculumVitae");
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
                                                if (_mailToObj.Subject.IndexOf("{{linkbackend}}") > 0)
                                                    _mailToObj.Subject = _mailToObj.Subject.Replace("{{linkbackend}}", linkBackEnd);
                                                if (_mailToObj.Subject.IndexOf("{{datetimenow}}") > 0)
                                                    _mailToObj.Subject = _mailToObj.Subject.Replace("{{datetimenow}}", linkBackEnd);

                                                _mailToObj.Body = _mailTemplate.BodyEn;
                                                if (_mailToObj.Body.IndexOf("{{fullname}}") > 0)
                                                    _mailToObj.Body = _mailToObj.Body.Replace("{{fullname}}", contact.FullName);
                                                if (_mailToObj.Body.IndexOf("{{phonenumber}}") > 0)
                                                    _mailToObj.Body = _mailToObj.Body.Replace("{{phonenumber}}", contact.PhoneNumber);
                                                if (_mailToObj.Body.IndexOf("{{email}}") > 0)
                                                    _mailToObj.Body = _mailToObj.Body.Replace("{{email}}", contact.Email);
                                                if (_mailToObj.Body.IndexOf("{{linkbackend}}") > 0)
                                                    _mailToObj.Body = _mailToObj.Body.Replace("{{linkbackend}}", linkBackEnd);
                                                if (_mailToObj.Body.IndexOf("{{datetimenow}}") > 0)
                                                    _mailToObj.Body = _mailToObj.Body.Replace("{{datetimenow}}", datenow);

                                                Mailer.SendMail(_mailSenderAccount, _mailToObj);
                                            }
                                        }
                                    }
                                }

                                title = (curentLan.Country == Language.LanguagueCountry.Vietmamese ? "Thông báo" : (curentLan.Country == Language.LanguagueCountry.English ? "Notification" : ""));
                                message = (curentLan.Country == Language.LanguagueCountry.Vietmamese ? "Đã gửi tin nhắn thành công. Chúng tôi sẽ kiểm tra và phản hồi sớm nhất đến bạn." : (curentLan.Country == Language.LanguagueCountry.English ? "Message sent successfully. We will check and respond to you as soon as possible" : ""));
                                status = Default.Status_Sucessfull;
                            }
                        }
                        else
                        {
                            message = (curentLan.Country == Language.LanguagueCountry.Vietmamese ? "Xin vui lòng chèn tập tin đúng định dạng: .jpg, .png, .jpeg, .gif, .pdf, .doc, .docx, .xls, .xlsx" : (curentLan.Country == Language.LanguagueCountry.English ? "Please insert the file in the correct format: .jpg, .png, .jpeg, .gif, .pdf, .doc, .docx, .xls, .xlsx" : ""));
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

        private string UploadFile(HttpPostedFileBase file, string extension)
        {
            string src = ""; 
            var objPath = paraService.GetByCode((new SaveFilePathConfig()).Code);
            if (objPath != null)
            {
                var paraPath    = JsonConvert.DeserializeObject<SaveFilePathConfig>(objPath.Content.ToString());
                pathForUrl      = paraPath.RecruitmentPath;
                pathForSaving   = string.Format(@"{0}{1}", GSIDSessionSiteInformation.PathAddressSiteMultimedia, pathForUrl);
            }

            if (this.CreateFolderIfNeeded(pathForSaving))
            {
                try
                {
                    string filename = string.Format("{0}{1}", Guid.NewGuid().ToString().Replace("-", ""), extension);
                    file.SaveAs(Path.Combine(pathForSaving, filename));

                    src = string.Format("{0}/{1}", pathForUrl.Replace(@"\", "/"), filename);
                }
                catch
                {
                }
            }
            return src;
        }
    }
}