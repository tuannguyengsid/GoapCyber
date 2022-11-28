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
    public class CategoryController : BaseController
    {
        private readonly IRouteDataUrlService routeDataUrlService;
        private readonly IProductCategoryService prdCateService;
        private readonly IProductService prdService;
        private readonly IParameterService paraService;
        public List<ProductCategory> ProductCategories { get; set; }
        // GET: Category
        public CategoryController(IRouteDataUrlService _routeDataUrlService,
                                    IProductCategoryService _prdCateService,
                                    IProductService _prdService,
                                    IParameterService _paraService)
        {
            routeDataUrlService = _routeDataUrlService;
            prdCateService      = _prdCateService;
            prdService          = _prdService;
            paraService         = _paraService;
            PageSize = 20;//Số lượng item hiển thị
            PageVisit = 5;//Số lượng trang hiển thị
            PageIndex = 1;//Vị trí trang
        }

        public async Task<ActionResult> Detail(string language = "", string cateslug = "", string urlRouteId = "")
        {
            var languages = (List<Language>)System.Web.HttpContext.Current.Application[SestionName.Languages];
            Language curentLan = (languages != null && languages.Count > 0) ? languages.Where(l => l.Parent == language).FirstOrDefault() : null;
            CategoryViewModel model = null;

            if (curentLan != null)
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

                ProductDetailPageManagementAdminConfig modelProductDetailPage = new ProductDetailPageManagementAdminConfig();
                var paraProductPageConfig = paraService.GetByCode(new ProductDetailPageManagementAdminConfig().Code);
                if (paraProductPageConfig != null)
                    modelProductDetailPage = JsonConvert.DeserializeObject<ProductDetailPageManagementAdminConfig>(paraProductPageConfig.Content.ToString());
                ViewBag.ProductDetailPageConfig = modelProductDetailPage;

                model = new CategoryViewModel();
                if (!string.IsNullOrEmpty(cateslug))
                    model.ProductCategory = (curentLan.Country == Language.LanguagueCountry.Vietmamese ? prdCateService.GetBySlugVn(cateslug) : (curentLan.Country == Language.LanguagueCountry.English ? prdCateService.GetBySlugEn(cateslug) : null));
                else if (!string.IsNullOrEmpty(urlRouteId))
                    model.ProductCategory = (curentLan.Country == Language.LanguagueCountry.Vietmamese ? prdCateService.GetRouterVn(urlRouteId) : (curentLan.Country == Language.LanguagueCountry.English ? prdCateService.GetRouterEn(urlRouteId) : null));

                if (model.ProductCategory != null)
                {
                    model.Products = prdService.GetAllByCategory(model.ProductCategory.Id, false);
                    //DefineRouterValueLanguages(language, model.ProductCategory.RouteDataUrlVn.Url, model.ProductCategory.RouteDataUrlEn.Url);
                }

            }
            return View(model);
        }
    }
}