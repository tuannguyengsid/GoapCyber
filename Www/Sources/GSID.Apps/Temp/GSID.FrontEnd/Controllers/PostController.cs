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
    public class PostController : BaseController
    {
        private readonly INewsService newsService;
        private readonly ITagPostService tagPostService;
        private readonly IParameterService paraService;
        private readonly IRouteDataUrlService routeDataUrlService;
        public PostController(IRouteDataUrlService _routeDataUrlService,
                                    IParameterService _paraService,
                                    INewsService _newsService,
                                    ITagPostService _tagPostService)
        {
            paraService = _paraService;
            newsService = _newsService;
            tagPostService = _tagPostService;
            routeDataUrlService = _routeDataUrlService;
        }

        // GET: Post
        public async Task<ActionResult> Index(string language = "", string p = "")
        {
            PostViewModel model = new PostViewModel();

            HomePageManagementAdminConfig modelHomepage = new HomePageManagementAdminConfig();
            var paraHomePageConfig = paraService.GetByCode(new HomePageManagementAdminConfig().Code);
            if (paraHomePageConfig != null)
            {
                modelHomepage = JsonConvert.DeserializeObject<HomePageManagementAdminConfig>(paraHomePageConfig.Content.ToString());
                modelHomepage.RouteDataUrlVn = routeDataUrlService.GetBy(modelHomepage.RouteDataUrlVnId ?? "");
                modelHomepage.RouteDataUrlEn = routeDataUrlService.GetBy(modelHomepage.RouteDataUrlEnId ?? "");

            }
            ViewBag.HomePageConfig = modelHomepage;

            PostPageManagementAdminConfig modelPostPage = new PostPageManagementAdminConfig();
            var paraPostPageConfig = paraService.GetByCode(new PostPageManagementAdminConfig().Code);
            if (paraPostPageConfig != null)
            {
                modelPostPage = JsonConvert.DeserializeObject<PostPageManagementAdminConfig>(paraPostPageConfig.Content.ToString());
                modelPostPage.RouteDataUrlVn = routeDataUrlService.GetBy(modelPostPage.RouteDataUrlVnId ?? "");
                modelPostPage.RouteDataUrlEn = routeDataUrlService.GetBy(modelPostPage.RouteDataUrlEnId ?? "");

                model.Posts         = newsService.GetAll(false);
                model.PageIndex     = p.ConvertIntPaging();
                model.TotalPage     = (Math.Ceiling((double)model.Posts.Count / model.PageSize));
                model.CurrentPage   = model.PageIndex;
                model.PageVisit     = model.PageVisit;
                model.PageSize      = model.PageSize;
                model.CountTotal    = model.Posts.Count();
                model.Posts         = model.Posts.Skip(model.PageSize * (model.PageIndex - 1))
                                        .Take(model.PageSize)
                                            .OrderByDescending(c => c.EditedByDate ?? c.AddedByDate ?? DateTime.MaxValue)
                                                .ToList();

                //DefineRouterValueLanguages(language, modelPostPage.RouteDataUrlVn.Url.ReplaceQueryStringParam("p", p), modelPostPage.RouteDataUrlEn.Url.ReplaceQueryStringParam("p", p));
            }
            ViewBag.PostPageConfig = modelPostPage;

            return View(model);
        }

        public async Task<ActionResult> Detail(string language = "", string urlRouteId = "")
        {
            PostViewModel model = null;

            HomePageManagementAdminConfig modelHomepage = new HomePageManagementAdminConfig();
            var paraHomePageConfig = paraService.GetByCode(new HomePageManagementAdminConfig().Code);
            if (paraHomePageConfig != null)
            {
                modelHomepage = JsonConvert.DeserializeObject<HomePageManagementAdminConfig>(paraHomePageConfig.Content.ToString());
                modelHomepage.RouteDataUrlVn = routeDataUrlService.GetBy(modelHomepage.RouteDataUrlVnId ?? "");
                modelHomepage.RouteDataUrlEn = routeDataUrlService.GetBy(modelHomepage.RouteDataUrlEnId ?? "");
            }
            ViewBag.HomePageConfig = modelHomepage;

            PostPageManagementAdminConfig modelPostPage = new PostPageManagementAdminConfig();
            var paraPostPageConfig = paraService.GetByCode(new PostPageManagementAdminConfig().Code);
            if (paraPostPageConfig != null)
            {
                modelPostPage = JsonConvert.DeserializeObject<PostPageManagementAdminConfig>(paraPostPageConfig.Content.ToString());
                modelPostPage.RouteDataUrlVn = routeDataUrlService.GetBy(modelPostPage.RouteDataUrlVnId ?? "");
                modelPostPage.RouteDataUrlEn = routeDataUrlService.GetBy(modelPostPage.RouteDataUrlEnId ?? "");
            }
            ViewBag.PostPageConfig = modelPostPage;

            PostDetailPageManagementAdminConfig modelDetailPostPage = new PostDetailPageManagementAdminConfig();
            var paraPostDetailPageConfig = paraService.GetByCode(new PostDetailPageManagementAdminConfig().Code);
            if (paraPostPageConfig != null)
            {
                modelDetailPostPage = JsonConvert.DeserializeObject<PostDetailPageManagementAdminConfig>(paraPostDetailPageConfig.Content.ToString());
            }
            ViewBag.PostDetailPageConfig = modelDetailPostPage;

            var languages = (List<Language>)System.Web.HttpContext.Current.Application[SestionName.Languages];
            Language curentLan = (languages != null && languages.Count > 0) ? languages.Where(l => l.Parent == language).FirstOrDefault() : null;

            if (curentLan != null)
            {
                model   = new PostViewModel();

                model.Post = (curentLan.Country == Language.LanguagueCountry.Vietmamese ? newsService.GetRouterVn(urlRouteId) : (curentLan.Country == Language.LanguagueCountry.English ? newsService.GetRouterEn(urlRouteId) : null));
                if (model.Post != null)
                {
                    model.TagPosts = model.Post.TagPostIds != null ? tagPostService.GetAllByIds(model.Post.TagPostIds, false) : new List<TagPost>();
                    model.Related = model.Post.TagPostIds != null ? newsService.GetAllByRelated(model.Post.TagPostIds, 3, false) : new List<News>();
                    model.Latest = model.Related != null ? newsService.GetAllLatest(modelDetailPostPage.RelastItem, false) : new List<News>();

                    DefineRouterValueLanguages(language, model.Post.RouteDataUrlVn.Url, model.Post.RouteDataUrlEn.Url);
                }
            }

            return View(model);
        }

        public async Task<ActionResult> TagFilter(string language = "", string urlRouteId = "", string p = "")
        {
            PostViewModel model = new PostViewModel();

            HomePageManagementAdminConfig modelHomepage = new HomePageManagementAdminConfig();
            var paraHomePageConfig = paraService.GetByCode(new HomePageManagementAdminConfig().Code);
            if (paraHomePageConfig != null)
            {
                modelHomepage = JsonConvert.DeserializeObject<HomePageManagementAdminConfig>(paraHomePageConfig.Content.ToString());
                modelHomepage.RouteDataUrlVn = routeDataUrlService.GetBy(modelHomepage.RouteDataUrlVnId ?? "");
                modelHomepage.RouteDataUrlEn = routeDataUrlService.GetBy(modelHomepage.RouteDataUrlEnId ?? "");
            }
            ViewBag.HomePageConfig = modelHomepage;

            PostPageManagementAdminConfig modelPostPage = new PostPageManagementAdminConfig();
            var paraPostPageConfig = paraService.GetByCode(new PostPageManagementAdminConfig().Code);
            if (paraPostPageConfig != null)
            {
                modelPostPage = JsonConvert.DeserializeObject<PostPageManagementAdminConfig>(paraPostPageConfig.Content.ToString());
                modelPostPage.RouteDataUrlVn = routeDataUrlService.GetBy(modelPostPage.RouteDataUrlVnId ?? "");
                modelPostPage.RouteDataUrlEn = routeDataUrlService.GetBy(modelPostPage.RouteDataUrlEnId ?? "");
                model.Posts = newsService.GetAll(false);
            }
            ViewBag.PostPageConfig = modelPostPage;

            var languages = (List<Language>)System.Web.HttpContext.Current.Application[SestionName.Languages];
            Language curentLan = (languages != null && languages.Count > 0) ? languages.Where(l => l.Parent == language).FirstOrDefault() : null;

            if (curentLan != null)
            {
                model = new PostViewModel();
                model.TagPost = (curentLan.Country == Language.LanguagueCountry.Vietmamese ? tagPostService.GetRouterVn(urlRouteId) : (curentLan.Country == Language.LanguagueCountry.English ? tagPostService.GetRouterEn(urlRouteId) : null));
                if (model.TagPost != null)
                {
                    model.Posts = newsService.GetAllByTag(model.TagPost.Id , false);
                    DefineRouterValueLanguages(language, model.TagPost.RouteDataUrlVn.Url.ReplaceQueryStringParam("p", p), model.TagPost.RouteDataUrlEn.Url.ReplaceQueryStringParam("p", p));
                }
            }

            return View(model);
        }

        
    }
}