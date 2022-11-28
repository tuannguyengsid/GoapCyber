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

namespace GSID.FrontEnd.Controllers
{
    public class GalleryController : BaseAuthenticationController
    {
        private readonly IRouteDataUrlService routeDataUrlService;
        private readonly IImageLibraryService imgLibraryService;
        private readonly IImageService imgService;
        private readonly IParameterService paraService;

        public GalleryController(IRouteDataUrlService _routeDataUrlService,
                                    IParameterService _paraService,
                                    IImageService _imgService,
                                    IImageLibraryService _imgLibraryService)
        {
            routeDataUrlService = _routeDataUrlService;
            paraService = _paraService;
            imgService = _imgService;
            imgLibraryService = _imgLibraryService;
        }

        public ActionResult Index(string language = "")
        {
            var languages = (List<Language>)System.Web.HttpContext.Current.Application[SestionName.Languages];
            Language curentLan = (languages != null && languages.Count > 0) ? languages.Where(l => l.Parent == language).FirstOrDefault() : null;
            ImageLibraryViewModel model = null;

            if (curentLan != null)
            {
                ImageLibraryPageManagementAdminConfig modelImageLibraryPage = new ImageLibraryPageManagementAdminConfig();
                var paraImageLibraryPageConfig = paraService.GetByCode(new ImageLibraryPageManagementAdminConfig().Code);
                if (paraImageLibraryPageConfig != null)
                {
                    modelImageLibraryPage = JsonConvert.DeserializeObject<ImageLibraryPageManagementAdminConfig>(paraImageLibraryPageConfig.Content.ToString());
                    modelImageLibraryPage.RouteDataUrlVn = routeDataUrlService.GetBy(modelImageLibraryPage.RouteDataUrlVnId ?? "");
                    modelImageLibraryPage.RouteDataUrlEn = routeDataUrlService.GetBy(modelImageLibraryPage.RouteDataUrlEnId ?? "");

                    DefineRouterValueLanguages(language, modelImageLibraryPage.RouteDataUrlVn.Url, modelImageLibraryPage.RouteDataUrlEn.Url);
                }
                ViewBag.ImageLibraryPage = modelImageLibraryPage;

                model = new ImageLibraryViewModel();
                model.Images = imgService.GetAll(false);
                model.ImageLibraries = imgLibraryService.GetAll(false);
            }
            return View(model);
        }
    }
}