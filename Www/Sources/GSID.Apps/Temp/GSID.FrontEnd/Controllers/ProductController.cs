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
    public class ProductController : BaseController
    {
        private readonly IProductCategoryService prdCateService;
        private readonly IProductService prdService;
        private readonly IParameterService paraService;
        // GET: Agent
        public ProductController(IProductCategoryService _prdCateService,
                                    IProductService _prdService,
                                    IParameterService _paraService)
        {
            prdCateService = _prdCateService;
            prdService = _prdService;
            paraService = _paraService;
        }

        // GET: Product
        public ActionResult Detail(string language = "", string urlRouteId = "")
        {
            var languages = (List<Language>)System.Web.HttpContext.Current.Application[SestionName.Languages];
            Language curentLan = (languages != null && languages.Count > 0) ? languages.Where(l => l.Parent == language).FirstOrDefault() : null;
            ProductViewModel model = null;

            if (curentLan != null)
            {
                HomePageManagementAdminConfig modelHomepage = new HomePageManagementAdminConfig();
                var paraHomePageConfig = paraService.GetByCode(new HomePageManagementAdminConfig().Code);
                if (paraHomePageConfig != null)
                {
                    modelHomepage = JsonConvert.DeserializeObject<HomePageManagementAdminConfig>(paraHomePageConfig.Content.ToString());
                }
                ViewBag.HomePageConfig = modelHomepage;

                ProductDetailPageManagementAdminConfig modelProductDetailPage = new ProductDetailPageManagementAdminConfig();
                var paraProductPageConfig = paraService.GetByCode(new ProductDetailPageManagementAdminConfig().Code);
                if (paraProductPageConfig != null)
                    modelProductDetailPage = JsonConvert.DeserializeObject<ProductDetailPageManagementAdminConfig>(paraProductPageConfig.Content.ToString());
                ViewBag.ProductDetailPageConfig = modelProductDetailPage;

                model = new ProductViewModel();
                model.Product = (curentLan.Country == Language.LanguagueCountry.Vietmamese ? prdService.GetRouterVn(urlRouteId) : (curentLan.Country == Language.LanguagueCountry.English ? prdService.GetRouterEn(urlRouteId) : null));
                if (model.Product != null && model.Product.ProductCategory != null)
                {
                    model.RelatedProducts = prdService.GetAllByRelated(model.Product.ProductCategory.Id, model.Product.Id, false);
                    DefineRouterValueLanguages(language, model.Product.RouteDataUrlVn.Url, model.Product.RouteDataUrlEn.Url);
                }

            }
            return View(model);
        }
    }
}