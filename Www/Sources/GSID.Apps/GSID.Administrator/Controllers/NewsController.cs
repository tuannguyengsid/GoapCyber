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
using System.Configuration;
using System.Web;
using GSID.Model.ExtraEntities;
using Newtonsoft.Json;
using GSID.Service.MongoRepositories.Service;
using System.Threading.Tasks;

namespace GSID.Admin.Controllers
{
    public class NewsController : BaseAuthenticationController
    {
        // GET: News
        private readonly IRouteDataUrlService routeDataUrlService;
        private readonly INewsCategoryService newsCateService;
        private readonly ITagPostService tagPostService;
        private readonly INewsService newsService;
        private readonly IParameterService paraService;
        // GET: News
        public NewsController(IRouteDataUrlService _routeDataUrlService,
                                INewsService _newsService,
                                ITagPostService _tagPostService,
                                IParameterService _paraService,
                                INewsCategoryService newsCateService)
        {
            routeDataUrlService = _routeDataUrlService;
            tagPostService = _tagPostService;
            newsService = _newsService;
            paraService = _paraService;
            this.newsCateService = newsCateService;
        }

        [RBAC]
        public async Task<ActionResult> Index(string p)
        {
            NewsFilterViewModel model = new NewsFilterViewModel();
            model.TagPosts = tagPostService.GetAll(false);

            model.NewsCategories = new List<NewsCategory>();
            var xxx = newsCateService.GetAllParent("");
            foreach (var x in xxx)
            {
                var xx = new NewsCategory();
                xx.ParentId = x.ParentId;
                xx.Id = x.Id;
                xx.NameVn = x.NameVn;
                xx.NameEn = x.NameEn;
                model.NewsCategories.Add(xx);
                var yyy = newsCateService.GetAllParent(x.Id);
                foreach (var y in yyy)
                {
                    var yy = new NewsCategory();
                    yy.Id = y.Id;
                    yy.ParentId = y.ParentId;
                    yy.NameVn = y.NameVn;
                    yy.NameEn = y.NameEn;
                    model.NewsCategories.Add(yy);

                    var zzz = newsCateService.GetAllParent(y.Id);
                    foreach (var z in zzz)
                    {
                        var zz = new NewsCategory();
                        zz.Id = z.Id;
                        zz.ParentId = z.ParentId;
                        zz.NameVn = z.NameVn;
                        zz.NameEn = z.NameEn;
                        model.NewsCategories.Add(zz);
                    }
                }
            }

            ViewBag.ActiveMenu = RBACUser.RootPermissionId(Request);

            return View(model);
        }

        public async Task<ActionResult> PartialIndex(string p, string Keyword, string BeginAddDateString, string EndAddDateString, string[] TagPostId, string[] NewsCategoryId)
        {
            var _beginAddDate = ConvertDateTimeIsNull(BeginAddDateString);
            var _endAdddDate = ConvertDateTimeIsNull(EndAddDateString);
            var model = newsService.GetAllBySearch(Keyword, _beginAddDate, _endAdddDate, TagPostId, NewsCategoryId);

            PageIndex = p.ConvertIntPaging();
            ViewBag.TotalPage = (Math.Ceiling((double)model.Count / PageSize));
            ViewBag.CurrentPage = PageIndex;
            ViewBag.PageVisit = PageVisit;
            ViewBag.PageSize = PageSize;
            ViewBag.CountTotal = model.Count();
            model = model.Skip(PageSize * (PageIndex - 1))
                                    .Take(PageSize)
                                        .OrderByDescending(c => c.AddedByDate ?? DateTime.Now)
                                            .ToList();

            return PartialView(model);
        }

        [RBAC]
        public ActionResult Create()
        {
            NewsCreateViewModel model = new NewsCreateViewModel();
            model.SlugVn    = model.SlugEn = "/";
            model.TagPosts = tagPostService.GetAll(false);

            model.NewsCategories = new List<NewsCategory>();
            var xxx = newsCateService.GetAllParent("");
            foreach (var x in xxx)
            {
                var xx = new NewsCategory();
                xx.ParentId = x.ParentId;
                xx.Id = x.Id;
                xx.NameVn = x.NameVn;
                xx.NameEn = x.NameEn;
                model.NewsCategories.Add(xx);
                var yyy = newsCateService.GetAllParent(x.Id);
                foreach (var y in yyy)
                {
                    var yy = new NewsCategory();
                    yy.Id = y.Id;
                    yy.ParentId = y.ParentId;
                    yy.NameVn = y.NameVn;
                    yy.NameEn = y.NameEn;
                    model.NewsCategories.Add(yy);

                    var zzz = newsCateService.GetAllParent(y.Id);
                    foreach (var z in zzz)
                    {
                        var zz = new NewsCategory();
                        zz.Id = z.Id;
                        zz.ParentId = z.ParentId;
                        zz.NameVn = z.NameVn;
                        zz.NameEn = z.NameEn;
                        model.NewsCategories.Add(zz);
                    }
                }
            }

            model.IsDeleted = true;
            model.IsUrlRequiredVn = false;
            model.IsUrlRequiredEn = false;
            ViewBag.ActiveMenu = RBACUser.RootPermissionId(Request);
            return View(model);
        }

        [HttpPost]
        [RBAC]
        public ActionResult Create([Bind(Include = "TitleSEOVn, TitleSEOEn, SlugSEOVn, SlugSEOEn, DescriptionSEOVn, DescriptionSEOEn, KeywordSEOVn, KeywordSEOEn, RouteDataUrlVnId, RouteDataUrlEnId, H1SEOVn, H1SEOEn, H2SEOVn, H2SEOEn, H3SEOVn, H3SEOEn, H4SEOVn, H4SEOEn, H5SEOVn, H5SEOEn, H6SEOVn, H6SEOEn, HtmlMetaRawVn, HtmlMetaRawEn, IsUrlRequiredVn, IsUrlRequiredEn, "
                                                        + "OgTitleVn, OgSite_nameVn, OgDescriptionVn, OgTitleEn, OgSite_nameEn, OgDescriptionEn, OgType, OgImageSrc, "
                                                        + "NameVn, NameEn, SlugVn, SlugEn, FullSlugVn, FullSlugEn, ShortDescriptionVn, ShortDescriptionEn, DescriptionVn, DescriptionEn, TagPostIds, NewsCategoryId, IsDeleted, ImageSrc")]NewsCreateViewModel obj)
        {
            string title = Message.TITLE_ERROR;
            string message = Message.CONTENT_POSTDATA_DEFAUT_ERROR;
            string status = Default.Status_Error;
            string id = string.Empty;
            try
            {
                var isRecycleBinPer = RBACUser.HasPermission("RecycleBin", Request.RequestContext.RouteData.Values["controller"].ToString());
                if (!isRecycleBinPer)
                    ModelState.Remove("IsDeleted");
                if (ModelState.IsValid)
                {
                    News model = Mapper.Map<NewsCreateViewModel, News>(obj);
                    model.ImageSrc      = obj.ImageSrc;
                    model.IsDeleted     = !RBACUser.HasPermission("RecycleBin", Request.RequestContext.RouteData.Values["controller"].ToString()) ? false : !obj.IsDeleted;
                    model.AddedBy       = GSIDSessionFacade.GSIDSessionUserLogon.Id;

                    var routerVn = !string.IsNullOrEmpty(obj.SlugSEOVn) ? routeDataUrlService.GetByUrl(obj.SlugSEOVn) : null;
                    var routerEn = !string.IsNullOrEmpty(obj.SlugSEOEn) ? routeDataUrlService.GetByUrl(obj.SlugSEOEn) : null;
                    if (routerVn == null && routerEn == null)
                    {
                        if (!string.IsNullOrEmpty(obj.TitleSEOVn) ||
                            !string.IsNullOrEmpty(obj.SlugSEOVn) ||
                            !string.IsNullOrEmpty(obj.DescriptionSEOVn) ||
                            !string.IsNullOrEmpty(obj.KeywordSEOVn) ||
                            !string.IsNullOrEmpty(obj.H1SEOVn) ||
                            !string.IsNullOrEmpty(obj.H2SEOVn) ||
                            !string.IsNullOrEmpty(obj.H3SEOVn) ||
                            !string.IsNullOrEmpty(obj.H4SEOVn) ||
                            !string.IsNullOrEmpty(obj.H5SEOVn) ||
                            !string.IsNullOrEmpty(obj.H6SEOVn) ||
                            !string.IsNullOrEmpty(obj.HtmlMetaRawVn))
                        {
                            routerVn                = new RouteDataUrl();
                            routerVn.Title          = obj.TitleSEOVn;
                            routerVn.Url            = !string.IsNullOrEmpty(obj.SlugSEOVn) ? obj.SlugSEOVn.ToLower().Trim() : "";
                            routerVn.Description    = obj.DescriptionSEOVn;
                            routerVn.Keyword        = obj.KeywordSEOVn;
                            routerVn.H1             = obj.H1SEOVn;
                            routerVn.H2             = obj.H2SEOVn;
                            routerVn.H3             = obj.H3SEOVn;
                            routerVn.H4             = obj.H4SEOVn;
                            routerVn.H5             = obj.H5SEOVn;
                            routerVn.H6             = obj.H6SEOVn;
                            routerVn.HtmlMetaRaw    = obj.HtmlMetaRawVn;
                            routerVn.Controller     = "Post";
                            routerVn.Action         = "Detail";
                            routerVn.Area           = "";
                            routerVn.IsLanguage     = RouteDataUrl.RouteDataIsLanguage.Vn;
                            routerVn.IsType         = RouteDataUrl.RouteDataIsType.PostDetail;
                            //OpenGraphMetaData
                            routerVn.OgTitle        = obj.OgTitleVn;
                            routerVn.OgSite_name    = obj.OgSite_nameVn;
                            routerVn.OgDescription  = obj.OgDescriptionVn;
                            routerVn.OgType         = obj.OgType;
                            routerVn.OgImageSrc     = obj.OgImageSrc;
                            routerVn.IsUrlRequired  = false;
                            model.RouteDataUrlVnId  = routeDataUrlService.Create(routerVn);
                        }

                        if (!string.IsNullOrEmpty(obj.TitleSEOEn) ||
                            !string.IsNullOrEmpty(obj.SlugSEOEn) ||
                            !string.IsNullOrEmpty(obj.DescriptionSEOEn) ||
                            !string.IsNullOrEmpty(obj.KeywordSEOEn) ||
                            !string.IsNullOrEmpty(obj.H1SEOEn) ||
                            !string.IsNullOrEmpty(obj.H2SEOEn) ||
                            !string.IsNullOrEmpty(obj.H3SEOEn) ||
                            !string.IsNullOrEmpty(obj.H4SEOEn) ||
                            !string.IsNullOrEmpty(obj.H5SEOEn) ||
                            !string.IsNullOrEmpty(obj.H6SEOEn) ||
                            !string.IsNullOrEmpty(obj.HtmlMetaRawEn))
                        {
                            routerEn                = new RouteDataUrl();
                            routerEn.Title          = obj.TitleSEOEn;
                            routerEn.Url            = !string.IsNullOrEmpty(obj.SlugSEOEn) ? obj.SlugSEOEn.ToLower().Trim() : "";
                            routerEn.Description    = obj.DescriptionSEOEn;
                            routerEn.Keyword        = obj.KeywordSEOEn;
                            routerEn.H1             = obj.H1SEOEn;
                            routerEn.H2             = obj.H2SEOEn;
                            routerEn.H3             = obj.H3SEOEn;
                            routerEn.H4             = obj.H4SEOEn;
                            routerEn.H5             = obj.H5SEOEn;
                            routerEn.H6             = obj.H6SEOEn;
                            routerEn.HtmlMetaRaw    = obj.HtmlMetaRawEn;
                            routerEn.Controller     = "Post";
                            routerEn.Action         = "Detail";
                            routerEn.Area           = "";
                            routerEn.IsLanguage     = RouteDataUrl.RouteDataIsLanguage.En;
                            routerEn.IsType         = RouteDataUrl.RouteDataIsType.PostDetail;
                            //OpenGraphMetaData
                            routerEn.OgTitle        = obj.OgTitleEn;
                            routerEn.OgSite_name    = obj.OgSite_nameEn;
                            routerEn.OgDescription  = obj.OgDescriptionEn;
                            routerEn.OgType         = obj.OgType;
                            routerEn.OgImageSrc     = obj.OgImageSrc;
                            routerEn.IsUrlRequired  = false;
                            model.RouteDataUrlEnId  = routeDataUrlService.Create(routerEn);
                        }
                    }

                    id = newsService.Create(model);
                    title = Message.TITLE_REPORT;
                    message = Message.CONTENT_POSTDATA_CREATE_SUCCESSFULL;
                    status = Default.Status_Sucessfull;
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
                Id = id,
                Title = title,
                Message = message,
                Status = status
            }, JsonRequestBehavior.AllowGet);
        }

        [RBAC]
        public ActionResult Update(string gsid)
        {
            var obj = newsService.GetBy(gsid);
            if (obj == null)
                Response.Redirect(string.Format("/Error/NotFound?url={0}", Request.Url), true);

            var model = Mapper.Map<News, NewsEditViewModel>(obj);
            var cate = !string.IsNullOrEmpty(obj.NewsCategoryId) ? newsCateService.GetBy(obj.NewsCategoryId): null;
            if (cate != null)
            {
                model.FullSlugVn = String.Format("/vn/tin-tuc/{0}/{1}", cate.SlugVn, model.SlugVn);
                model.FullSlugEn = String.Format("/en/news/{0}/{1}", cate.SlugEn, model.SlugEn);
                model.NewsCategorySlugVn = cate.SlugVn;
                model.NewsCategorySlugEn = cate.SlugEn;
            }

            model.TagPosts = tagPostService.GetAll(false);
            
            model.NewsCategories = new List<NewsCategory>();
            var xxx = newsCateService.GetAllParent("");
            foreach (var x in xxx)
            {
                var xx = new NewsCategory();
                xx.ParentId = x.ParentId;
                xx.Id = x.Id;
                xx.NameVn = x.NameVn;
                xx.NameEn = x.NameEn;
                model.NewsCategories.Add(xx);
                var yyy = newsCateService.GetAllParent(x.Id);
                foreach (var y in yyy)
                {
                    var yy = new NewsCategory();
                    yy.Id = y.Id;
                    yy.ParentId = y.ParentId;
                    yy.NameVn = string.Format("__{0}", y.NameVn);
                    yy.NameEn = string.Format("__{0}", y.NameEn);
                    model.NewsCategories.Add(yy);

                    var zzz = newsCateService.GetAllParent(y.Id);
                    foreach (var z in zzz)
                    {
                        var zz = new NewsCategory();
                        zz.Id = z.Id;
                        zz.ParentId = z.ParentId;
                        zz.NameVn = string.Format("_____{0}", z.NameVn);
                        zz.NameEn = string.Format("_____{0}", z.NameEn);
                        model.NewsCategories.Add(zz);
                    }
                }
            }
            model.IsDeleted = !model.IsDeleted;

            model.IsUrlRequiredVn = false;
            model.IsUrlRequiredEn = false;
            var routerVn = !string.IsNullOrEmpty(model.RouteDataUrlVnId) ? routeDataUrlService.GetBy(model.RouteDataUrlVnId) : null;
            if (routerVn != null)
            {
                model.TitleSEOVn        = routerVn.Title;
                model.SlugSEOVn         = routerVn.Url;
                model.DescriptionSEOVn  = routerVn.Description;
                model.KeywordSEOVn      = routerVn.Keyword;
                model.H1SEOVn           = routerVn.H1;
                model.H2SEOVn           = routerVn.H2;
                model.H3SEOVn           = routerVn.H3;
                model.H4SEOVn           = routerVn.H4;
                model.H5SEOVn           = routerVn.H5;
                model.H6SEOVn           = routerVn.H6;
                model.HtmlMetaRawVn     = routerVn.HtmlMetaRaw;
                model.OgTitleVn         = routerVn.OgTitle;
                model.OgSite_nameVn     = routerVn.OgSite_name;
                model.OgDescriptionVn   = routerVn.OgDescription;
                model.OgType            = routerVn.OgType;
                model.OgImageSrc        = routerVn.OgImageSrc;
            }

            var routerEn = !string.IsNullOrEmpty(model.RouteDataUrlEnId) ? routeDataUrlService.GetBy(model.RouteDataUrlEnId) : null;
            if (routerEn != null)
            {
                model.TitleSEOEn        = routerEn.Title;
                model.SlugSEOEn         = routerEn.Url;
                model.DescriptionSEOEn  = routerEn.Description;
                model.KeywordSEOEn      = routerEn.Keyword;
                model.H1SEOEn           = routerEn.H1;
                model.H2SEOEn           = routerEn.H2;
                model.H3SEOEn           = routerEn.H3;
                model.H4SEOEn           = routerEn.H4;
                model.H5SEOEn           = routerEn.H5;
                model.H6SEOEn           = routerEn.H6;
                model.HtmlMetaRawEn     = routerEn.HtmlMetaRaw;
                model.OgTitleEn         = routerEn.OgTitle;
                model.OgSite_nameEn     = routerEn.OgSite_name;
                model.OgDescriptionEn   = routerEn.OgDescription;
                model.OgType            = routerEn.OgType;
                model.OgImageSrc        = routerEn.OgImageSrc;
            }

            ViewBag.ActiveMenu = RBACUser.RootPermissionId(Request);
            return View(model);
        }

        [HttpPost]
        [RBAC]
        public ActionResult Update([Bind(Include = "TitleSEOVn, TitleSEOEn, SlugSEOVn, SlugSEOEn, DescriptionSEOVn, DescriptionSEOEn, KeywordSEOVn, KeywordSEOEn, RouteDataUrlVnId, RouteDataUrlEnId, H1SEOVn, H1SEOEn, H2SEOVn, H2SEOEn, H3SEOVn, H3SEOEn, H4SEOVn, H4SEOEn, H5SEOVn, H5SEOEn, H6SEOVn, H6SEOEn, HtmlMetaRawVn, HtmlMetaRawEn, IsUrlRequiredVn, IsUrlRequiredEn, "
                                                        + "OgTitleVn, OgSite_nameVn, OgDescriptionVn, OgTitleEn, OgSite_nameEn, OgDescriptionEn, OgType, OgImageSrc, "
                                                        + "Id, NameVn, NameEn, SlugVn, SlugEn, FullSlugVn, FullSlugEn, ShortDescriptionVn, ShortDescriptionEn, DescriptionVn, DescriptionEn, TagPostIds, NewsCategoryId, IsDeleted, ImageSrc")]NewsEditViewModel obj)
        {
            string title = Message.TITLE_ERROR;
            string message = Message.CONTENT_POSTDATA_DEFAUT_ERROR;
            string status = Default.Status_Error;
            try
            {
                var isRecycleBinPer = RBACUser.HasPermission("RecycleBin", Request.RequestContext.RouteData.Values["controller"].ToString());
                if (!isRecycleBinPer)
                    ModelState.Remove("IsDeleted");
                if (ModelState.IsValid)
                {
                    var model = newsService.GetBy(obj.Id);
                    if (model != null)
                    {
                        model.NameVn                = obj.NameVn;
                        model.NameEn                = obj.NameEn;
                        model.SlugVn                = !string.IsNullOrEmpty(obj.SlugVn) ? obj.SlugVn.Trim().ToLower() : "";
                        model.SlugEn                = !string.IsNullOrEmpty(obj.SlugEn) ? obj.SlugEn.Trim().ToLower() : "";
                        model.ShortDescriptionVn    = obj.ShortDescriptionVn;
                        model.ShortDescriptionEn    = obj.ShortDescriptionEn;
                        model.DescriptionVn         = obj.DescriptionVn;
                        model.DescriptionEn         = obj.DescriptionEn;
                        model.TagPostIds            = obj.TagPostIds;
                        model.NewsCategoryId        = obj.NewsCategoryId;
                        model.ImageSrc              = obj.ImageSrc;
                        
                        model.IsDeleted = isRecycleBinPer ? !obj.IsDeleted : model.IsDeleted;
                        model.EditedBy  = GSIDSessionFacade.GSIDSessionUserLogon.Id;
                        var routerVn = !string.IsNullOrEmpty(obj.SlugSEOVn) ? routeDataUrlService.GetByUrl(obj.SlugSEOVn, model.RouteDataUrlVnId) : null;
                        var routerEn = !string.IsNullOrEmpty(obj.SlugSEOEn) ? routeDataUrlService.GetByUrl(obj.SlugSEOEn, model.RouteDataUrlEnId) : null;
                        if (routerVn == null && routerEn == null)
                        {
                            if (!string.IsNullOrEmpty(obj.TitleSEOVn) ||
                                !string.IsNullOrEmpty(obj.SlugSEOVn) ||
                                !string.IsNullOrEmpty(obj.DescriptionSEOVn) ||
                                !string.IsNullOrEmpty(obj.KeywordSEOVn) ||
                                !string.IsNullOrEmpty(obj.H1SEOVn) ||
                                !string.IsNullOrEmpty(obj.H2SEOVn) ||
                                !string.IsNullOrEmpty(obj.H3SEOVn) ||
                                !string.IsNullOrEmpty(obj.H4SEOVn) ||
                                !string.IsNullOrEmpty(obj.H5SEOVn) ||
                                !string.IsNullOrEmpty(obj.H6SEOVn) ||
                                !string.IsNullOrEmpty(obj.HtmlMetaRawVn))
                            {
                                routerVn = !string.IsNullOrEmpty(model.RouteDataUrlVnId) ? routeDataUrlService.GetBy(model.RouteDataUrlVnId) : null;
                                bool _flagCreateRouterVn = false;
                                if (routerVn == null)
                                {
                                    routerVn = new RouteDataUrl();
                                    _flagCreateRouterVn = true;
                                }

                                routerVn.Title          = obj.TitleSEOVn;
                                routerVn.Url            = !string.IsNullOrEmpty(obj.SlugSEOVn) ? obj.SlugSEOVn.ToLower().Trim() : "";
                                routerVn.Description    = obj.DescriptionSEOVn;
                                routerVn.Keyword        = obj.KeywordSEOVn;
                                routerVn.H1             = obj.H1SEOVn;
                                routerVn.H2             = obj.H2SEOVn;
                                routerVn.H3             = obj.H3SEOVn;
                                routerVn.H4             = obj.H4SEOVn;
                                routerVn.H5             = obj.H5SEOVn;
                                routerVn.H6             = obj.H6SEOVn;
                                routerVn.HtmlMetaRaw    = obj.HtmlMetaRawVn;
                                routerVn.Controller     = "Post";
                                routerVn.Action         = "Detail";
                                routerVn.Area           = "";
                                routerVn.IsLanguage     = RouteDataUrl.RouteDataIsLanguage.Vn;
                                routerVn.IsType         = RouteDataUrl.RouteDataIsType.PostDetail;
                                //OpenGraphMetaData
                                routerVn.OgTitle        = obj.OgTitleVn;
                                routerVn.OgSite_name    = obj.OgSite_nameVn;
                                routerVn.OgDescription  = obj.OgDescriptionVn;
                                routerVn.OgType         = obj.OgType;
                                routerVn.OgImageSrc     = obj.OgImageSrc;
                                routerVn.IsUrlRequired  = false;

                                if (_flagCreateRouterVn)
                                    model.RouteDataUrlVnId = routeDataUrlService.Create(routerVn);
                                else
                                    routeDataUrlService.Update(routerVn);
                            }

                            if (!string.IsNullOrEmpty(obj.TitleSEOEn) ||
                                !string.IsNullOrEmpty(obj.SlugSEOEn) ||
                                !string.IsNullOrEmpty(obj.DescriptionSEOEn) ||
                                !string.IsNullOrEmpty(obj.KeywordSEOEn) ||
                                !string.IsNullOrEmpty(obj.H1SEOEn) ||
                                !string.IsNullOrEmpty(obj.H2SEOEn) ||
                                !string.IsNullOrEmpty(obj.H3SEOEn) ||
                                !string.IsNullOrEmpty(obj.H4SEOEn) ||
                                !string.IsNullOrEmpty(obj.H5SEOEn) ||
                                !string.IsNullOrEmpty(obj.H6SEOEn) ||
                                !string.IsNullOrEmpty(obj.HtmlMetaRawEn))
                            {
                                routerEn = !string.IsNullOrEmpty(model.RouteDataUrlVnId) ? routeDataUrlService.GetBy(model.RouteDataUrlVnId) : null;
                                bool _flagCreateRouterEn = false;
                                if (routerEn == null)
                                {
                                    routerEn = new RouteDataUrl();
                                    _flagCreateRouterEn = true;
                                }

                                routerEn.Title          = obj.TitleSEOEn;
                                routerEn.Url            = !string.IsNullOrEmpty(obj.SlugSEOEn) ? obj.SlugSEOEn.ToLower().Trim() : "";
                                routerEn.Description    = obj.DescriptionSEOEn;
                                routerEn.Keyword        = obj.KeywordSEOEn;
                                routerEn.H1             = obj.H1SEOEn;
                                routerEn.H2             = obj.H2SEOEn;
                                routerEn.H3             = obj.H3SEOEn;
                                routerEn.H4             = obj.H4SEOEn;
                                routerEn.H5             = obj.H5SEOEn;
                                routerEn.H6             = obj.H6SEOEn;
                                routerEn.HtmlMetaRaw    = obj.HtmlMetaRawEn;
                                routerEn.Controller     = "Post";
                                routerEn.Action         = "Detail";
                                routerEn.Area           = "";
                                routerEn.IsLanguage     = RouteDataUrl.RouteDataIsLanguage.En;
                                routerEn.IsType         = RouteDataUrl.RouteDataIsType.PostDetail;
                                //OpenGraphMetaData
                                routerEn.OgTitle        = obj.OgTitleEn;
                                routerEn.OgSite_name    = obj.OgSite_nameEn;
                                routerEn.OgDescription  = obj.OgDescriptionEn;
                                routerEn.OgType         = obj.OgType;
                                routerEn.OgImageSrc     = obj.OgImageSrc;
                                routerEn.IsUrlRequired  = false;

                                if (_flagCreateRouterEn)
                                    model.RouteDataUrlEnId = routeDataUrlService.Create(routerEn);
                                else
                                    routeDataUrlService.Update(routerEn);
                            }
                        }
                        newsService.Update(model);

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

        [RBAC]
        [HttpPost]
        public ActionResult Duplicate(string id)
        {
            string title = Message.TITLE_ERROR;
            string status = ((int)AjaxStatus.Error).ToString();
            string message = Message.CONTENT_POSTDATA_DELETE_ERROR_OR_EMPTY;
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var obj = newsService.GetBy(id);
            if (obj == null)
                Response.Redirect(string.Format("/Error/NotFound?url={0}", Request.Url), true);

            var random = RandomStringGenerator.RandomStringToken(6);

            var objNew = new News();
            string extension, pathForSaving, filePathSrc;

            var routerOldVn = routeDataUrlService.GetBy(obj.RouteDataUrlVnId ?? "");
            if (routerOldVn != null)
            {
                var routerVn = new RouteDataUrl();
                routerVn.Title = routerOldVn.Title;
                routerVn.Url = string.Format("ban-sao-{0}-{1}", random, routerOldVn.Url).ToLower();
                routerVn.Description = routerOldVn.Description;
                routerVn.Keyword = routerOldVn.Keyword;
                routerVn.H1 = routerOldVn.H1;
                routerVn.H2 = routerOldVn.H2;
                routerVn.H3 = routerOldVn.H3;
                routerVn.H4 = routerOldVn.H4;
                routerVn.H5 = routerOldVn.H5;
                routerVn.H6 = routerOldVn.H6;
                routerVn.Controller = routerOldVn.Controller;
                routerVn.Action = routerOldVn.Action;
                routerVn.Area = routerOldVn.Area;
                routerVn.IsLanguage = routerOldVn.IsLanguage;
                routerVn.IsType = routerOldVn.IsType;
                routerVn.OgTitle = routerOldVn.OgTitle;
                routerVn.OgSite_name = routerOldVn.OgSite_name;
                routerVn.OgDescription = routerOldVn.OgDescription;
                routerVn.OgType = routerOldVn.OgType;
                routerVn.OgImageSrc = routerOldVn.OgImageSrc;
                objNew.RouteDataUrlVnId = routeDataUrlService.Create(routerVn);
            }

            var routerOldEn = routeDataUrlService.GetBy(obj.RouteDataUrlEnId ?? "");
            if (routerOldEn != null)
            {
                var routerEn = new RouteDataUrl();
                routerEn.Title = routerOldEn.Title;
                routerEn.Url = string.Format("copy-{0}-{1}", random, routerOldEn.Url).ToLower();
                routerEn.Description = routerOldEn.Description;
                routerEn.Keyword = routerOldEn.Keyword;
                routerEn.H1 = routerOldEn.H1;
                routerEn.H2 = routerOldEn.H2;
                routerEn.H3 = routerOldEn.H3;
                routerEn.H4 = routerOldEn.H4;
                routerEn.H5 = routerOldEn.H5;
                routerEn.H6 = routerOldEn.H6;
                routerEn.Controller = routerOldEn.Controller;
                routerEn.Action = routerOldEn.Action;
                routerEn.Area = routerOldEn.Area;
                routerEn.IsLanguage = routerOldEn.IsLanguage;
                routerEn.IsType = routerOldEn.IsType;
                routerEn.OgTitle = routerOldVn.OgTitle;
                routerEn.OgSite_name = routerOldVn.OgSite_name;
                routerEn.OgDescription = routerOldVn.OgDescription;
                routerEn.OgType = routerOldVn.OgType;
                routerEn.OgImageSrc = routerOldVn.OgImageSrc;
                objNew.RouteDataUrlVnId = routeDataUrlService.Create(routerEn);
            }

            objNew.NameVn = string.Format("[Bản sao {0}] - {1}", random, obj.NameVn);
            objNew.NameEn = string.Format("[copy {0}] - {1}", random, obj.NameEn);
            objNew.ShortDescriptionVn = obj.ShortDescriptionVn;
            objNew.ShortDescriptionEn = obj.ShortDescriptionEn;
            objNew.DescriptionVn = obj.DescriptionVn;
            objNew.DescriptionEn = obj.DescriptionEn;
            objNew.TagPostIds = obj.TagPostIds;
            objNew.ImageSrc = obj.ImageSrc;
            objNew.IsDeleted = true;
            string newId = newsService.Create(objNew);

            status = ((int)AjaxStatus.Sucessfull).ToString();

            return Json(new
            {
                Id = newId,
                Title = title,
                Message = message,
                Status = status
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [RBAC]
        public ActionResult Delete(string id)
        {
            string status = ((int)StatusDelete.Error).ToString();
            string message = Message.CONTENT_POSTDATA_DELETE_ERROR_OR_EMPTY;
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var _hasDelete = newsService.Delete(id);
            if (_hasDelete)
                status = ((int)StatusDelete.Deleted).ToString();

            return Json(new
            {
                Status = status,
                Message = message
            });
        }

        [HttpPost]
        [RBAC]
        public ActionResult DeleteAll(string[] ids)
        {
            string status = ((int)StatusDelete.Error).ToString();
            string message = Message.CONTENT_POSTDATA_DELETE_ERROR_OR_EMPTY;
            if (ids == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var _hasDelete = newsService.Delete(ids);
            if (_hasDelete)
                status = ((int)StatusDelete.Deleted).ToString();

            return Json(new
            {
                Status = status,
                Message = message
            });
        }


        [HttpPost]
        [RBAC]
        public ActionResult RecycleBin(string id, bool isDeleted)
        {
            string title = Message.TITLE_ERROR;
            string status = ((int)StatusDelete.Error).ToString();
            string message = Message.CONTENT_POSTDATA_DELETE_ERROR_OR_EMPTY;
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var _hasRecycleBin = newsService.GetBy(id);
            _hasRecycleBin.DeletedByDate = DateTime.Now;
            _hasRecycleBin.DeletedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
            _hasRecycleBin.IsDeleted = !isDeleted;
            newsService.Update(_hasRecycleBin);

            title = Message.TITLE_REPORT;
            message = Message.CONTENT_POSTDATA_UPDATE_SUCCESSFULL;
            status = Default.Status_Sucessfull;

            return Json(new
            {
                Title = title,
                Message = message,
                Status = status
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [RBAC]
        public ActionResult GetCategory(string CateId)
        {
            string status = ((int)StatusDelete.Error).ToString();
            string message = Message.CONTENT_POSTDATA_DELETE_ERROR_OR_EMPTY;
            if (CateId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var _has = newsCateService.GetBy(CateId);
            message = Message.CONTENT_POSTDATA_UPDATE_SUCCESSFULL;
            status = Default.Status_Sucessfull;

            return Json(new
            {
                SlugVn = _has.SlugVn,
                SlugEn = _has.SlugEn,
                Message = message,
                Status = status
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult IsNameVnAvailable(string NameVn)
        {
            // Check if the NameVn already exists
            return Json(newsService.IsNameVnAvailable(NameVn) != null ? false : true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult IsNameVnIdAvailable(string NameVn, string Id)
        {
            // Check if the NameVn already exists
            return Json(newsService.IsNameVnAvailable(NameVn, Id) != null ? false : true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult IsNameEnAvailable(string NameEn)
        {
            // Check if the NameEn already exists
            return Json(newsService.IsNameEnAvailable(NameEn) != null ? false : true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult IsNameEnIdAvailable(string NameEn, string Id)
        {
            // Check if the NameEn already exists
            return Json(newsService.IsNameEnAvailable(NameEn, Id) != null ? false : true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult IsUrlVnIdAvailable(string SlugVn, string RouteDataUrlVnId, string SlugEn)
        {
            SlugVn = !string.IsNullOrEmpty(SlugVn) ? SlugVn.Trim().ToLower() : "";
            SlugEn = !string.IsNullOrEmpty(SlugEn) ? SlugEn.Trim().ToLower() : "";
            if (SlugVn == SlugEn)
                return Json(false, JsonRequestBehavior.AllowGet);
            else
                // Check if the NameEn already exists
                return Json(routeDataUrlService.IsUrlAvailable(SlugVn, RouteDataUrlVnId) != null ? false : true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult IsUrlEnIdAvailable(string SlugEn, string RouteDataUrlEnId, string SlugVn)
        {
            SlugVn = !string.IsNullOrEmpty(SlugVn) ? SlugVn.Trim().ToLower() : "";
            SlugEn = !string.IsNullOrEmpty(SlugEn) ? SlugEn.Trim().ToLower() : "";
            if (SlugVn == SlugEn)
                return Json(false, JsonRequestBehavior.AllowGet);
            else
                // Check if the NameEn already exists
                return Json(routeDataUrlService.IsUrlAvailable(SlugEn, RouteDataUrlEnId) != null ? false : true, JsonRequestBehavior.AllowGet);
        }

    }
}