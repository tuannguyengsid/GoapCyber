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
using GSID.FrontEnd.Helpers;
using GSID.Model.MongodbModels;

namespace GSID.FrontEnd.Controllers
{
    public class ErrorController : BaseController
    {
        private readonly IRouteDataUrlService routeDataUrlService;
        private readonly IParameterService paraService;
        public ErrorController(IRouteDataUrlService _routeDataUrlService,
                                    IParameterService _paraService)
        {
            routeDataUrlService = _routeDataUrlService;
            paraService = _paraService;
        }

        // GET: Error
        public ActionResult NotFound(string language = "", string url = "")
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
            ViewBag.Url = url;
            //DefineRouterValueLanguages(language, modelPostPage.RouteDataUrlVn.Url.ReplaceQueryStringParam("p", p), modelPostPage.RouteDataUrlEn.Url.ReplaceQueryStringParam("p", p));

            return View();
        }
    }
}