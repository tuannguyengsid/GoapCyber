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
using GSID.Service.MongoRepositories.Service;
using System.Threading.Tasks;

namespace GSID.Admin.Controllers
{
    public class NewsCategoryController : BaseAuthenticationController
    {
        // GET: NewsCategory
        private readonly IRouteDataUrlService routeDataUrlService;
        private readonly INewsCategoryService newsCateService;

        public NewsCategoryController(INewsCategoryService _newsCateService, 
                                    IRouteDataUrlService routeDataUrlService)
        {
            newsCateService = _newsCateService;
            this.routeDataUrlService = routeDataUrlService;
        }

        [RBAC]
        public ActionResult Index(string p)
        {
            var model = newsCateService.GetAllParent("");
            ViewBag.ActiveMenu = RBACUser.RootPermissionId(Request);
            return View(model);
        }

        [ChildActionOnly]
        public PartialViewResult PartialIndexChildren(string ParentId)
        {
            var model = newsCateService.GetAllParent(ParentId);
            return PartialView(model);
        }

        [RBAC]
        public ActionResult Create()
        {
            NewsCategoryCreateViewModel model = new NewsCategoryCreateViewModel();

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
                }
            }

            model.IsUrlRequiredVn = false;
            model.IsUrlRequiredEn = false;
            model.IsDeleted = true;
            ViewBag.ActiveMenu = RBACUser.RootPermissionId(Request);
            return View(model);
        }

        [HttpPost]
        [RBAC]
        public ActionResult Create([Bind(Include = "TitleSEOVn, TitleSEOEn, SlugSEOVn, SlugSEOEn, DescriptionSEOVn, DescriptionSEOEn, KeywordSEOVn, KeywordSEOEn, RouteDataUrlVnId, RouteDataUrlEnId, H1SEOVn, H1SEOEn, H2SEOVn, H2SEOEn, H3SEOVn, H3SEOEn, H4SEOVn, H4SEOEn, H5SEOVn, H5SEOEn, H6SEOVn, H6SEOEn, HtmlMetaRawVn, HtmlMetaRawEn, IsUrlRequiredVn, IsUrlRequiredEn, "
                                                        + "OgTitleVn, OgSite_nameVn, OgDescriptionVn, OgTitleEn, OgSite_nameEn, OgDescriptionEn, OgType, OgImageSrc, "
                                                        + "ParentId, NameVn, NameEn, SlugVn, SlugEn, FullSlugVn, FullSlugEn, Sort, IsMenu, IsDeleted")]NewsCategoryCreateViewModel obj)
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
                    NewsCategory model = Mapper.Map<NewsCategoryCreateViewModel, NewsCategory>(obj);

                    if (string.IsNullOrEmpty(obj.ParentId))
                    {
                        model.ParentId = "";
                        model.Level = 1;
                    }
                    else if (!string.IsNullOrEmpty(obj.ParentId))
                    {
                        var objParrent = newsCateService.GetBy(obj.ParentId);
                        model.ParentId = obj.ParentId;
                        model.Level = objParrent != null ? (!string.IsNullOrEmpty(objParrent.ParentId) ? 3 : 2) : 1;

                        objParrent.HasChild = true;
                        newsCateService.Update(objParrent);
                    }

                    model.IsMenu    = obj.IsMenu;
                    model.IsDeleted = !isRecycleBinPer ? false : !obj.IsDeleted;
                    model.AddedBy   = GSIDSessionFacade.GSIDSessionUserLogon.Id;

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
                            routerVn.Action         = "NewsCategory";
                            routerVn.Area           = "";
                            routerVn.IsLanguage     = RouteDataUrl.RouteDataIsLanguage.Vn;
                            routerVn.IsType         = RouteDataUrl.RouteDataIsType.PostCategory;
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
                            routerEn.Action         = "NewsCategory";
                            routerEn.Area           = "";
                            routerEn.IsLanguage     = RouteDataUrl.RouteDataIsLanguage.En;
                            routerEn.IsType         = RouteDataUrl.RouteDataIsType.PostCategory;
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

                    id = newsCateService.Create(model);
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
            var model = Mapper.Map<NewsCategory, NewsCategoryEditViewModel>(newsCateService.GetBy(gsid));
            if (model == null)
                Response.Redirect(string.Format("/Error/NotFound?url={0}", Request.Url), true);

            model.FullSlugVn = String.Format("/vn/tin-tuc/{0}",model.SlugVn);
            model.FullSlugEn = String.Format("/en/news/{0}", model.SlugEn);

            model.NewsCategories = new List<NewsCategory>();

            var xxx = newsCateService.GetAllParent("", model.Id);
            foreach (var x in xxx)
            {
                var xx = new NewsCategory();
                xx.ParentId = x.ParentId;
                xx.Id = x.Id;
                xx.NameVn = x.NameVn;
                xx.NameEn = x.NameEn;
                model.NewsCategories.Add(xx);
                var yyy = newsCateService.GetAllParent(x.Id, model.Id);
                foreach (var y in yyy)
                {
                    var yy = new NewsCategory();
                    yy.Id = y.Id;
                    yy.ParentId = y.ParentId;
                    yy.NameVn = y.NameVn;
                    yy.NameEn = y.NameEn;
                    model.NewsCategories.Add(yy);
                }
            }
            
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
            model.IsUrlRequiredVn = false;
            model.IsUrlRequiredEn = false;
            model.IsDeleted         = !model.IsDeleted;
            ViewBag.ActiveMenu = RBACUser.RootPermissionId(Request);
            return View(model);
        }

        [HttpPost]
        [RBAC]
        public ActionResult Update([Bind(Include = "TitleSEOVn, TitleSEOEn, SlugSEOVn, SlugSEOEn, DescriptionSEOVn, DescriptionSEOEn, KeywordSEOVn, KeywordSEOEn, RouteDataUrlVnId, RouteDataUrlEnId, H1SEOVn, H1SEOEn, H2SEOVn, H2SEOEn, H3SEOVn, H3SEOEn, H4SEOVn, H4SEOEn, H5SEOVn, H5SEOEn, H6SEOVn, H6SEOEn, HtmlMetaRawVn, HtmlMetaRawEn, IsUrlRequiredVn, IsUrlRequiredEn, "
                                                        + "OgTitleVn, OgSite_nameVn, OgDescriptionVn, OgTitleEn, OgSite_nameEn, OgDescriptionEn, OgType, OgImageSrc, "
                                                        + "Id, ParentId, NameVn, NameEn, SlugVn, SlugEn, FullSlugVn, FullSlugEn, Sort, IsMenu, IsDeleted")]NewsCategoryEditViewModel obj)
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
                    var model = newsCateService.GetBy(obj.Id);
                    if (model != null)
                    {
                        if (string.IsNullOrEmpty(obj.ParentId))
                        {
                            model.ParentId = "";
                            model.Level = 1;
                        }
                        else if (!string.IsNullOrEmpty(obj.ParentId))
                        {
                            var objParrent = newsCateService.GetBy(obj.ParentId);
                            model.ParentId = obj.ParentId;
                            model.Level = objParrent != null ? (!string.IsNullOrEmpty(objParrent.ParentId) ? 3 : 2) : 1;

                            objParrent.HasChild = true;
                            newsCateService.Update(objParrent);
                        }

                        model.NameVn        = obj.NameVn;
                        model.NameEn        = obj.NameEn;
                        model.SlugVn        = obj.SlugVn;
                        model.SlugEn        = obj.SlugEn;
                        model.Sort          = obj.Sort;
                        model.IsMenu        = obj.IsMenu;
                        model.IsDeleted     = isRecycleBinPer ? !obj.IsDeleted : model.IsDeleted;
                        model.EditedBy      = GSIDSessionFacade.GSIDSessionUserLogon.Id;

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
                                routerVn.Action         = "NewsCategory";
                                routerVn.Area           = "";
                                routerVn.IsLanguage     = RouteDataUrl.RouteDataIsLanguage.Vn;
                                routerVn.IsType         = RouteDataUrl.RouteDataIsType.PostCategory;
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
                                routerEn.Action         = "NewsCategory";
                                routerEn.Area           = "";
                                routerEn.IsLanguage     = RouteDataUrl.RouteDataIsLanguage.En;
                                routerEn.IsType         = RouteDataUrl.RouteDataIsType.PostCategory;
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

                        newsCateService.Update(model);

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

        [HttpPost]
        [RBAC]
        public ActionResult Delete(string id)
        {
            string status = ((int)StatusDelete.Error).ToString();
            string message = Message.CONTENT_POSTDATA_DELETE_ERROR_OR_EMPTY;
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var _hasDelete = newsCateService.Delete(id);
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

            var _hasRecycleBin = newsCateService.GetBy(id);
            _hasRecycleBin.DeletedByDate = DateTime.Now;
            _hasRecycleBin.DeletedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
            _hasRecycleBin.IsDeleted = !isDeleted;
            newsCateService.Update(_hasRecycleBin);

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
    }
}