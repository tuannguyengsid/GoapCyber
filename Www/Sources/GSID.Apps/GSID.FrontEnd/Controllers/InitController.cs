using GSID.Data.Mongodb;
using GSID.Data.Mongodb.MongoCore;
using GSID.Model.MongodbModels;
using GSID.Service;
using GSID.Setting;
using GSID.Web.Core.Authentication;
using GSID.Web.Core.Extensions;
using GSID.FrontEnd.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using static GSID.Model.MongodbModels.User;
using GSID.Service.MongoRepositories.Service;
using GSID.FrontEnd.ViewModels;
using GSID.Model.ExtraEntities;
using Newtonsoft.Json;
using AutoMapper;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;
using GSID.Setting.FormKeys;
using System.Net;
using GSID.FrontEnd.Models;
using System.Globalization;

namespace GSID.FrontEnd.Controllers
{
    public class InitController : BaseAuthenticationController
    {
        // GET: ControlPanel/Init
        private readonly IRouteDataUrlService routeDataUrlService;
        private readonly IGSIDMongoRepository repository;
        private readonly INewsService newsService;
        private readonly ICustomerService customerService;
        private readonly IQuickLinkService quickLinkService;
        private readonly IProductService prdService;
        private readonly IProductCategoryService prdCateService;
        private readonly IPermissionService permissionService;
        private readonly IParameterService paraService;
        private readonly IMenuNodeService menuService;
        private readonly ICountryService countryService;
        private readonly IProvinceService provinceService;
        private readonly IDistrictService districtService;
        private readonly IContactService contactService;

        public InitController(IGSIDMongoRepository _repository,
                                IRouteDataUrlService _routeDataUrlService,
                                INewsService _newsService,
                                IContactService _contactService,
                                ICustomerService _customerService,
                                IQuickLinkService _quickLinkService,
                                IProductService _prdService,
                                IProductCategoryService _prdCateService,
                                IParameterService _paraService,
                                IMenuNodeService _menuService,
                                IPermissionService _permissionService,
                                ICountryService _countryService,
                                IProvinceService _provinceService,
                                IDistrictService _districtService) {
            repository              = _repository;
            routeDataUrlService = _routeDataUrlService;
            contactService = _contactService;
            newsService             = _newsService;
            customerService         = _customerService;
            quickLinkService        = _quickLinkService;
            prdService              = _prdService;
            prdCateService          = _prdCateService;
            paraService             = _paraService;
            menuService             = _menuService;
            permissionService       = _permissionService;
            countryService          = _countryService;
            provinceService         = _provinceService;
            districtService         = _districtService;
        }

        public ActionResult FooterCompanyInformation(string language = "")
        {
            SocialNetworkManagementAdminConfig SocialNetwork = new SocialNetworkManagementAdminConfig();
            var paraSocialNetwork = paraService.GetByCode(new SocialNetworkManagementAdminConfig().Code);
            if (paraSocialNetwork != null)
                SocialNetwork = JsonConvert.DeserializeObject<SocialNetworkManagementAdminConfig>(paraSocialNetwork.Content.ToString());

            if (SocialNetwork.Social != null && SocialNetwork.Social.Count > 0)
                SocialNetwork.Social = SocialNetwork.Social.Where(c => c.IsFooter == true).OrderBy(o => o.Sort).ToList();

            ViewBag.SocialNetwork = SocialNetwork;

            return PartialView();
        }

        public ActionResult MenuHorizone(string language = "")
        {
            var model = menuService.GetAll(false);
            ViewBag.MenuParents = model.Where(w => w.ParentId == "").ToList();

            HomePageManagementAdminConfig modelHomepage = new HomePageManagementAdminConfig();
            var paraHomePageConfig = paraService.GetByCode(new HomePageManagementAdminConfig().Code);
            if (paraHomePageConfig != null)
            {
                modelHomepage = JsonConvert.DeserializeObject<HomePageManagementAdminConfig>(paraHomePageConfig.Content.ToString());
                modelHomepage.RouteDataUrlVn = routeDataUrlService.GetBy(modelHomepage.RouteDataUrlVnId ?? "");
                modelHomepage.RouteDataUrlEn = routeDataUrlService.GetBy(modelHomepage.RouteDataUrlVnId ?? "");
            }
            ViewBag.HomePageConfig = modelHomepage;

            return PartialView(model);
        }

        public ActionResult QuickLink(string language = "")
        {
            var model = quickLinkService.GetAllByParent("", false);
            ViewBag.Language = language;
            return PartialView(model);
        }

        public ActionResult QuickLinkParent(string language = "", string parentId = "")
        {
            var QuickLinkObj        = quickLinkService.GetBy(parentId);
            ViewBag.Language        = language;
            ViewBag.QuickLinkParent = QuickLinkObj;

            var model = quickLinkService.GetAllByParent(parentId, false);
            return PartialView(model);
        }

        public ActionResult FooterSocialMedia(string language = "")
        {
            SocialNetworkManagementAdminConfig model = new SocialNetworkManagementAdminConfig();
            var para = paraService.GetByCode(new SocialNetworkManagementAdminConfig().Code);
            if (para != null) { 
                model = JsonConvert.DeserializeObject<SocialNetworkManagementAdminConfig>(para.Content.ToString());
                if (model.Social != null)
                    model.Social = model.Social.Where(w=> w.IsDeleted == false).OrderBy(o => o.Sort).ToList();
            }

            return PartialView(model);
        }

        public ActionResult PopupForm(string language = "")
        {
            PopupSubcribesPageManagementAdminConfig model = new PopupSubcribesPageManagementAdminConfig();
            var para = paraService.GetByCode(new PopupSubcribesPageManagementAdminConfig().Code);
            if (para != null)
                model = JsonConvert.DeserializeObject<PopupSubcribesPageManagementAdminConfig>(para.Content.ToString());

            // Attempt to read the culture cookie from Request
            HttpCookie popupformCookie = Request.Cookies["_popupform"];
            if (popupformCookie != null)
                model.IsActive = popupformCookie.Value == "1" ? false : true;

            return PartialView(model);
        }

        public ActionResult PopupSubmitForm(string language = "")
        {
            ContactViewModel model = new ContactViewModel();
            model.Language = language;
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult PopupSubmitForm([Bind(Include = "Email, Language")] ContactViewModel obj)
        {
            var languages = (List<Language>)System.Web.HttpContext.Current.Application[SestionName.Languages];
            Language curentLan = (languages != null && languages.Count > 0) ? languages.Where(l => l.Parent == obj.Language).FirstOrDefault() : null;
            string title = "";
            string message = "";
            string status = Default.Status_Error;

            if (curentLan != null)
            {
                title = (curentLan.Country == Language.LanguagueCountry.Vietmamese ? "Lỗi" : (curentLan.Country == Language.LanguagueCountry.English ? "Error" : ""));
                message = (curentLan.Country == Language.LanguagueCountry.Vietmamese ? "Dữ liệu gặp lỗi. Xin vui lòng kiểm tra lại." : (curentLan.Country == Language.LanguagueCountry.English ? "Data error. Please check again." : ""));
                try
                {
                    if (ModelState.IsValid)
                    {
                        var contact = contactService.GetByEmail(obj.Email.ToLower());
                        if (contact == null)
                        {
                            contact = new Contact();
                            contact.Email = !string.IsNullOrEmpty(obj.Email) ? obj.Email.Trim().ToLower() : "";
                            contact.IsSubscribe = true;
                            contact.Id = contactService.Create(contact);
                        }
                        if (contact != null)
                        {
                            contact.IsSubscribe = true;
                            //model.AddedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
                            contactService.Update(contact);

                            title = (curentLan.Country == Language.LanguagueCountry.Vietmamese ? "Thông báo" : (curentLan.Country == Language.LanguagueCountry.English ? "Notification" : ""));
                            message = (curentLan.Country == Language.LanguagueCountry.Vietmamese ? "Đã gửi tin nhắn thành công. Chúng tôi sẽ kiểm tra và phản hồi sớm nhất đến bạn" : (curentLan.Country == Language.LanguagueCountry.English ? "Message sent successfully. We will check and respond to you as soon as possible" : ""));
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
            }

            return Json(new
            {
                Title = title,
                Message = message,
                Status = status
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult PopupCloseForm()
        {
            Response.Cookies["_popupform"].Value = "1";
            Response.Cookies["_popupform"].Expires = DateTime.Now.AddDays(30);
            return Json(new
            {
                Status = Default.Status_Sucessfull
            }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult InitData()
        {
            return View();
        }
    }
}