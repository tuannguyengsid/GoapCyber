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
    public class AboutUsController : BaseAuthenticationController
    {
        private readonly IRouteDataUrlService routeDataUrlService;
        private readonly IParameterService paraService;
        public AboutUsController(IRouteDataUrlService _routeDataUrlService, 
            IParameterService _paraService)
        {
            routeDataUrlService = _routeDataUrlService;
            paraService = _paraService;
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

            AboutPageManagementAdminConfig modelAboutUs = new AboutPageManagementAdminConfig();
            var paraAboutPageConfig = paraService.GetByCode(new AboutPageManagementAdminConfig().Code);
            if (paraAboutPageConfig != null)
            {
                modelAboutUs = JsonConvert.DeserializeObject<AboutPageManagementAdminConfig>(paraAboutPageConfig.Content.ToString());
                modelAboutUs.RouteDataUrlVn = routeDataUrlService.GetBy(modelAboutUs.RouteDataUrlVnId ?? "");
                modelAboutUs.RouteDataUrlEn = routeDataUrlService.GetBy(modelAboutUs.RouteDataUrlEnId ?? "");

                if (modelAboutUs.Visions != null)
                    modelAboutUs.Visions = modelAboutUs.Visions.OrderBy(o => o.Index).ToList();
                if (modelAboutUs.CoreValues != null)
                    modelAboutUs.CoreValues = modelAboutUs.CoreValues.OrderBy(o => o.Index).ToList();

                DefineRouterValueLanguages(language, modelAboutUs.RouteDataUrlVn.Url, modelAboutUs.RouteDataUrlEn.Url);
            }

            return View(modelAboutUs);
        }
    }
}