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
namespace GSID.FrontEnd.Controllers
{
    public class HomeController : BaseAuthenticationController
    {
        private readonly IRouteDataUrlService routeDataUrlService;
        private readonly IRecruitmentService recruitmentService;
        private readonly INewsService newsService;
        private readonly IProductService prdService;
        private readonly IProductCategoryService prdCateService;
        private readonly IPartnerService partnerService;
        private readonly IParameterService paraService;
        public HomeController(IRouteDataUrlService _routeDataUrlService,
                                IParameterService _paraService, 
                                IRecruitmentService _recruitmentService,
                                INewsService _newsService,
                                IProductCategoryService _prdCateService,
                                IProductService _prdService,
                                IPartnerService _partnerService) {
            routeDataUrlService = _routeDataUrlService;
            paraService     = _paraService;
            recruitmentService = _recruitmentService;
            prdService      = _prdService;
            prdCateService  = _prdCateService;
            newsService     = _newsService;
            partnerService = _partnerService;
        }

        // GET: Home
        public ActionResult Index()
        {

            //return RedirectToLocal(string.Format("/{0}", DefineUrlLanguages("", "", "")));
            return RedirectToLocal("");
        }

        public async Task<ActionResult> HomePage(string language = "")
        {
            HomePageManagementAdminConfig modelHomepage = new HomePageManagementAdminConfig();
            var paraHomePageConfig = paraService.GetByCode(new HomePageManagementAdminConfig().Code);
            if (paraHomePageConfig != null)
            {
                modelHomepage = JsonConvert.DeserializeObject<HomePageManagementAdminConfig>(paraHomePageConfig.Content.ToString());
                modelHomepage.RouteDataUrlVn = routeDataUrlService.GetBy(modelHomepage.RouteDataUrlVnId ?? "");
                modelHomepage.RouteDataUrlEn = routeDataUrlService.GetBy(modelHomepage.RouteDataUrlEnId ?? "");
                if (modelHomepage.RouteDataUrlVn != null && modelHomepage.RouteDataUrlEn != null)
                    DefineRouterValueLanguages(language, modelHomepage.RouteDataUrlVn.Url, modelHomepage.RouteDataUrlEn.Url);
                
                if (modelHomepage.Banners != null)
                    modelHomepage.Banners = modelHomepage.Banners.OrderBy(o => o.Index).ToList();
                ViewBag.Partners = partnerService.GetAll(false);
                ViewBag.Posts = newsService.GetAllLatest(3, false);
            }

            ViewBag.ProductCategories = prdCateService.GetAll(false);
            ViewBag.Recruitments = recruitmentService.GetAllByExpirationDate(false);

            return View(modelHomepage);
        }
    }
}