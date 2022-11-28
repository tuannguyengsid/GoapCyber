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
using System.Web;
using System.Configuration;
using static GSID.Model.MongodbModels.Parameter;
using GSID.Model.ExtraEntities;
using Newtonsoft.Json;
using GSID.Service.MongoRepositories.Service;
using GSID.Web.Core.Helpers;

namespace GSID.Admin.Controllers
{
    public class ConfigController : BaseAuthenticationController
    {
        private readonly IParameterService paraService;
        private readonly IUserService uService;
        private readonly INewsService newsService;
        private readonly IProductService productService;
        private readonly IRecruitmentService recruitmentService;
        // GET: User
        public ConfigController(IParameterService _paraService, 
                                    INewsService _newsService,
                                    IProductService _productService,
                                    IRecruitmentService _recruitmentService)
        {
            paraService = _paraService;
            newsService = _newsService;
            productService = _productService;
            recruitmentService = _recruitmentService;
        }
        // GET: Config
        [RBAC]
        public ActionResult Index()
        {
            ViewBag.ElementId = "";
            ViewBag.ActiveMenu = RBACUser.RootPermissionId(Request);
            return View();
        }

        public ActionResult PartialConfigInformation()
        {
            return PartialView();
        }

        public ActionResult PartialConfigSiteInformation()
        {
            SiteInformationViewModel model = new SiteInformationViewModel();
            SiteInformationConfig config = new SiteInformationConfig();
            var obj = paraService.GetByCode(config.Code);
            if (obj != null)
            {
                config = JsonConvert.DeserializeObject<SiteInformationConfig>(obj.Content.ToString());
                model.WhiteLogoSrc              = config.WhiteLogoSrc;
                model.BlackLogoSrc              = config.BlackLogoSrc;
                model.IconSrc                   = config.IconSrc;
                model.UrlBackEndSite            = config.UrlBackEndSite;
                model.Prefix                    = config.Prefix;
                model.UrlAddressSiteMultimedia  = config.UrlAddressSiteMultimedia;
                model.PathAddressSiteMultimedia = config.PathAddressSiteMultimedia;
                model.UrlAddressSite            = config.UrlAddressSite;
                model.SiteTitleVn               = config.SiteTitleVn;
                model.SiteTitleEn               = config.SiteTitleEn;
                model.HotlineVn                 = config.HotlineVn;
                model.HotlineEn                 = config.HotlineEn;
                model.DisplayHotlineVn          = config.DisplayHotlineVn;
                model.DisplayHotlineEn          = config.DisplayHotlineEn;
                model.WorkingTimeVn             = config.WorkingTimeVn;
                model.CompanyNameVn             = config.CompanyNameVn;
                model.CompanyNameEn             = config.CompanyNameEn;
                model.TaxCode                   = config.TaxCode;
                model.BusinessLicenseVn         = config.BusinessLicenseVn;
                model.BusinessLicenseEn         = config.BusinessLicenseEn;
                model.AddressEn                 = config.AddressEn;
                model.AddressVn                 = config.AddressVn;
                model.EmailVn                   = config.EmailVn;
                model.EmailEn                   = config.EmailEn;
                model.KeywordSEOVn              = config.KeywordSEOVn;
                model.KeywordSEOEn              = config.KeywordSEOEn;
                model.DescriptionSEOVn          = config.DescriptionSEOVn;
                model.DescriptionSEOEn          = config.DescriptionSEOEn;
                model.TagHeader                 = config.TagHeader;
                model.CssHeader                 = config.CssHeader;
                model.ScriptHeader              = config.ScriptHeader;
                model.TagFooter                 = config.TagFooter;
                model.ScriptFooter              = config.ScriptFooter;
                model.IsShowAuthenticationPage = config.IsShowAuthenticationPage;

                
            }
            ViewBag.SiteInformation = GSIDSessionSiteInformation;
            return PartialView(model);
        }

        [HttpPost, ValidateInput(false)]
        //[RBAC]
        public ActionResult PartialConfigSiteInformation([Bind(Include = "UrlAddressSiteMultimedia, PathAddressSiteMultimedia,UrlBackEndSite, UrlAddressSite, SiteTitleVn, SiteTitleEn, Prefix, HotlineVn, HotlineEn, DisplayHotlineVn, DisplayHotlineEn, WorkingTimeVn, WorkingTimeEn, AddressVn, AddressEn, CompanyNameVn, CompanyNameEn, TaxCode, BusinessLicenseVn, BusinessLicenseEn, EmailVn, EmailEn, KeywordSEOVn, KeywordSEOEn, DescriptionSEOVn, DescriptionSEOEn, TagHeader, CssHeader, ScriptHeader, TagFooter, ScriptFooter, IsShowAuthenticationPage, IconSrc, BlackLogoSrc, WhiteLogoSrc")]SiteInformationViewModel obj)
        {
            string title = Message.TITLE_ERROR;
            string message = Message.CONTENT_POSTDATA_DEFAUT_ERROR;
            string status = Default.Status_Error;
            string id = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    SiteInformationConfig config = new SiteInformationConfig();
                    var model = paraService.GetByCode(config.Code);
                    if (model != null)
                        config = JsonConvert.DeserializeObject<SiteInformationConfig>(model.Content.ToString());

                    config.WhiteLogoSrc     = obj.WhiteLogoSrc;
                    config.BlackLogoSrc     = obj.BlackLogoSrc;
                    config.IconSrc          = obj.IconSrc;

                    if (config.UrlAddressSiteMultimedia != obj.UrlAddressSiteMultimedia)
                    {
                        var _hasMigrate = MigrateStaticUrl(obj.UrlAddressSiteMultimedia, config.UrlAddressSiteMultimedia);
                        if (_hasMigrate)
                            config.UrlAddressSiteMultimedia = obj.UrlAddressSiteMultimedia;
                    }

                    config.PathAddressSiteMultimedia    = obj.PathAddressSiteMultimedia;
                    config.UrlBackEndSite               = obj.UrlBackEndSite;
                    config.UrlAddressSite               = obj.UrlAddressSite;
                    config.Prefix                       = obj.Prefix;
                    config.SiteTitleVn                  = obj.SiteTitleVn;
                    config.SiteTitleEn                  = obj.SiteTitleEn;
                    config.HotlineVn                    = obj.HotlineVn;
                    config.HotlineEn                    = obj.HotlineEn;
                    config.DisplayHotlineVn             = obj.DisplayHotlineVn;
                    config.DisplayHotlineEn             = obj.DisplayHotlineEn;
                    config.WorkingTimeVn                = obj.WorkingTimeVn;
                    config.WorkingTimeEn                = obj.WorkingTimeEn;
                    config.AddressEn                    = obj.AddressEn;
                    config.AddressVn                    = obj.AddressVn;
                    config.CompanyNameVn                = obj.CompanyNameVn;
                    config.CompanyNameEn                = obj.CompanyNameEn;
                    config.TaxCode                      = obj.TaxCode;
                    config.BusinessLicenseVn            = obj.BusinessLicenseVn;
                    config.BusinessLicenseEn            = obj.BusinessLicenseEn;
                    config.EmailVn                      = obj.EmailVn;
                    config.EmailEn                      = obj.EmailEn;
                    config.KeywordSEOVn                 = obj.KeywordSEOVn;
                    config.KeywordSEOEn                 = obj.KeywordSEOEn;
                    config.DescriptionSEOVn             = obj.DescriptionSEOVn;
                    config.DescriptionSEOEn             = obj.DescriptionSEOEn;
                    config.TagHeader                    = obj.TagHeader;
                    config.CssHeader                    = obj.CssHeader;
                    config.ScriptHeader                 = obj.ScriptHeader;
                    config.TagFooter                    = obj.TagFooter;
                    config.ScriptFooter                 = obj.ScriptFooter;
                    config.IsShowAuthenticationPage     = obj.IsShowAuthenticationPage;

                    GSIDSessionSiteInformation = config;
                    if (model != null)
                    {
                        model.Content = JsonConvert.SerializeObject(config);
                        //model.EditedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
                        model.EditedByDate = DateTime.Now;
                        paraService.Update(model);

                        title = Message.TITLE_REPORT;
                        message = Message.CONTENT_POSTDATA_UPDATE_SUCCESSFULL;
                        status = Default.Status_Sucessfull;
                    }
                    else
                    {
                        model = new Parameter();
                        model.Code = config.Code;
                        model.Type = ParameterType.Const;
                        model.Name = "";
                        model.Content = JsonConvert.SerializeObject(config);
                        //model.AddedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
                        model.AddedByDate = DateTime.Now;
                        model.IsDeleted = false;
                        id = paraService.Create(model);

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
        
        public ActionResult PartialSTMPConfig()
        {
            EmailSTMPConfigViewModel model = new EmailSTMPConfigViewModel();
            EmailSTMPConfig config = new EmailSTMPConfig();
            var obj = paraService.GetByCode(config.Code);
            if (obj != null)
            {
                config = JsonConvert.DeserializeObject<EmailSTMPConfig>(obj.Content.ToString());
                model.FullName = config.FullName;
                model.UserName = config.UserName;
                model.Password = config.Password;
                model.STMPSever = config.STMPSever;
                model.STMPPort = config.STMPPort;
                //model.IMAPSever = config.IMAPSever;
                //model.IMAPPort = config.IMAPPort;
                model.EmailNotification = config.EmailNotification;
            }
            return PartialView(model);
        }

        [HttpPost, ValidateInput(false)]
        //[RBAC]
        public ActionResult PartialSTMPConfig([Bind(Include = "FullName, UserName, Password, STMPSever, STMPPort, EmailNotification")]EmailSTMPConfigViewModel obj)
        {
            string title = Message.TITLE_ERROR;
            string message = Message.CONTENT_POSTDATA_DEFAUT_ERROR;
            string status = Default.Status_Error;
            string id = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    EmailSTMPConfig config = new EmailSTMPConfig();
                    config.FullName = obj.FullName.Trim();
                    config.UserName = obj.UserName.Trim();
                    config.Password = obj.Password.Trim();
                    config.STMPSever = obj.STMPSever.Trim();
                    config.STMPPort = obj.STMPPort;
                    //config.IMAPSever = obj.IMAPSever;
                    //config.IMAPPort = obj.IMAPPort;
                    config.EmailNotification = obj.EmailNotification;

                    MailTo _mailto = new MailTo();
                    _mailto.To          = config.EmailNotification;
                    _mailto.Subject     = string.Format("SMTP test from {0}", config.STMPSever);
                    _mailto.FullName    = config.FullName;
                    _mailto.Body        = "Test message";
                    var _hasTest = Mailer.SendMail(config, _mailto);

                    if (_hasTest)
                    {
                        var model = paraService.GetByCode(config.Code);
                        if (model != null)
                        {
                            model.Content = JsonConvert.SerializeObject(config);
                            //model.EditedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
                            model.EditedByDate = DateTime.Now;
                            paraService.Update(model);

                            title = Message.TITLE_REPORT;
                            message = Message.CONTENT_POSTDATA_UPDATE_SUCCESSFULL;
                            status = Default.Status_Sucessfull;
                        }
                        else
                        {
                            model = new Parameter();
                            model.Code = config.Code;
                            model.Type = ParameterType.Const;
                            model.Name = "";
                            model.Content = JsonConvert.SerializeObject(config);
                            //model.AddedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
                            model.AddedByDate = DateTime.Now;
                            model.IsDeleted = false;
                            id = paraService.Create(model);

                            title = Message.TITLE_REPORT;
                            message = Message.CONTENT_POSTDATA_UPDATE_SUCCESSFULL;
                            status = Default.Status_Sucessfull;
                        }
                    }
                    else
                    {
                        message = Message.CONTENT_EMAIL_CONNECTION_FAILT;
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

        public ActionResult PartialSocialMediaConfigSiteInformation()
        {
            return PartialView();
        }

        public ActionResult EmailImapConfig()
        {
            EmailIMapConfigViewModel model = new EmailIMapConfigViewModel();
            EmailImapConfig config = new EmailImapConfig();
            var obj = paraService.GetByCode(config.Code);
            if (obj != null)
            {
                config = JsonConvert.DeserializeObject<EmailImapConfig>(obj.Content.ToString());
                model.UserName = config.UserName;
                model.Password = config.Password;
                model.InCommingServer = config.InCommingServer;
                model.Port = config.Port;
            }
            return PartialView(model);
        }

        [HttpPost, ValidateInput(false)]
        //[RBAC]
        public ActionResult EmailImapConfig([Bind(Include = "UserName, Password, InCommingServer, Port")]EmailIMapConfigViewModel obj)
        {
            string title = Message.TITLE_ERROR;
            string message = Message.CONTENT_POSTDATA_DEFAUT_ERROR;
            string status = Default.Status_Error;
            string id = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    EmailImapConfig config = new EmailImapConfig();
                    config.UserName = obj.UserName.Trim();
                    config.Password = obj.Password.Trim();
                    config.InCommingServer = obj.InCommingServer.Trim();
                    config.Port = obj.Port;
                    var model = paraService.GetByCode(config.Code);
                    if (model != null)
                    {
                        model.Content = JsonConvert.SerializeObject(config);
                        //model.EditedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
                        model.EditedByDate = DateTime.Now;
                        paraService.Update(model);

                        title = Message.TITLE_REPORT;
                        message = Message.CONTENT_POSTDATA_UPDATE_SUCCESSFULL;
                        status = Default.Status_Sucessfull;
                    }
                    else
                    {
                        model = new Parameter();
                        model.Code = config.Code;
                        model.Type = ParameterType.Const;
                        model.Name = "";
                        model.Content = JsonConvert.SerializeObject(config);
                        //model.AddedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
                        model.AddedByDate = DateTime.Now;
                        model.IsDeleted = false;
                        id = paraService.Create(model);

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

        public ActionResult EmailFollowConfig()
        {
            EmailFollowConfigViewModel model = new EmailFollowConfigViewModel();
            EmailFollowConfig config = new EmailFollowConfig();
            var obj = paraService.GetByCode(config.Code);
            if (obj != null)
            {
                config = JsonConvert.DeserializeObject<EmailFollowConfig>(obj.Content.ToString());
                model.Email = config.Email;
            }
            return PartialView(model);
        }

        [HttpPost, ValidateInput(false)]
        //[RBAC]
        public ActionResult EmailFollowConfig([Bind(Include = "Email")]EmailFollowConfigViewModel obj)
        {
            string title = Message.TITLE_ERROR;
            string message = Message.CONTENT_POSTDATA_DEFAUT_ERROR;
            string status = Default.Status_Error;
            string id = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    EmailFollowConfig config = new EmailFollowConfig();
                    config.Email = obj.Email.Trim();
                    var model = paraService.GetByCode(config.Code);
                    if (model != null)
                    {
                        model.Content = JsonConvert.SerializeObject(config);
                        //model.EditedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
                        model.EditedByDate = DateTime.Now;
                        paraService.Update(model);

                        title = Message.TITLE_REPORT;
                        message = Message.CONTENT_POSTDATA_UPDATE_SUCCESSFULL;
                        status = Default.Status_Sucessfull;
                    }
                    else
                    {
                        model = new Parameter();
                        model.Code = config.Code;
                        model.Type = ParameterType.Const;
                        model.Name = "";
                        model.Content = JsonConvert.SerializeObject(config);
                        //model.AddedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
                        model.AddedByDate = DateTime.Now;
                        model.IsDeleted = false;
                        id = paraService.Create(model);

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

        private bool MigrateStaticUrl(string newUrl, string oldUrl)
        {
            var prods = productService.GetAll();
            prods.ForEach(mapping => {
                mapping.DescriptionVn = mapping.DescriptionVn?.Replace(oldUrl, newUrl);
                mapping.DescriptionEn = mapping.DescriptionEn?.Replace(oldUrl, newUrl);
                productService.Update(mapping);
            });

            var articles = newsService.GetAll();
            articles.ForEach(mapping => {
                mapping.DescriptionVn = mapping.DescriptionVn?.Replace(oldUrl, newUrl);
                mapping.DescriptionEn = mapping.DescriptionEn?.Replace(oldUrl, newUrl);
                newsService.Update(mapping);
            });

            var recruitments = recruitmentService.GetAll();
            recruitments.ForEach(mapping => {
                mapping.DescriptionVn = mapping.DescriptionVn?.Replace(oldUrl, newUrl);
                mapping.DescriptionEn = mapping.DescriptionEn?.Replace(oldUrl, newUrl);
                recruitmentService.Update(mapping);
            });

            return true;
        }
    }
}