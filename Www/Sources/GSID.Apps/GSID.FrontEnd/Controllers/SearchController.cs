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
    public class SearchController : BaseController
    {
        private readonly IRouteDataUrlService routeDataUrlService;
        private readonly IParameterService paraService;
        private readonly IRecruitmentService recruitmentService;
        private readonly INewsService newsService;
        public SearchController(IRouteDataUrlService _routeDataUrlService, 
                                    IParameterService _paraService,
                                    IRecruitmentService _recruitmentService,
                                    INewsService _newsService)
        {
            routeDataUrlService = _routeDataUrlService;
            paraService = _paraService;
            recruitmentService = _recruitmentService;
            newsService = _newsService;
            PageSize = 20;//Số lượng item hiển thị
            PageVisit = 5;//Số lượng trang hiển thị
            PageIndex = 1;//Vị trí trang
        }
        // GET: Search
        public ActionResult Index(string language = "", string keyword = "")
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

            SearchBoxAdvanceViewModel model = new SearchBoxAdvanceViewModel();
            model.Keyword = keyword;
            model.Posts = newsService.GetAllBySearch(keyword, false);
            model.Recruitments = recruitmentService.GetAllBySearch(keyword, false);

            DefineRouterValueLanguages(language, new { Keyword = keyword }, new { Keyword = keyword });
            return View(model);
        }

        public ActionResult Form(string language = "", string keyword = "")
        {
            SearchBoxViewModel model = new SearchBoxViewModel();
            model.Keyword = keyword;
            return PartialView(model);
        }
    }
}