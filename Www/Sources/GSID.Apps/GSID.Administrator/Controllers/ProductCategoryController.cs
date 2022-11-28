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
using GSID.Model.ExtraEntities;
using Newtonsoft.Json;
using System.Web;
using System.Threading.Tasks;

namespace GSID.Admin.Controllers
{
    public class ProductCategoryController : BaseAuthenticationController
    {
        // GET: ProductCategory
        private readonly IRouteDataUrlService routeDataUrlService;
        private readonly IProductCategoryService prdCateService;
        private readonly IProductService prdService;
        private readonly IParameterService paraService;
        // GET: ProductCategory
        public ProductCategoryController(IRouteDataUrlService _routeDataUrlService, 
                                            IProductCategoryService _prdCateService,
                                            IProductService _prdService,
                                            IParameterService _paraService)
        {
            routeDataUrlService = _routeDataUrlService;
            prdCateService = _prdCateService;
            paraService = _paraService;
            prdService = _prdService;
        }

        [RBAC]
        public async Task<ActionResult> Index(string p)
        {
            ProductCategoryFilterViewModel model = new ProductCategoryFilterViewModel();
            ViewBag.ActiveMenu = RBACUser.RootPermissionId(Request);

            return View(model);
        }

        public async Task<ActionResult> PartialIndex(string p, string Keyword, string BeginAddDateString, string EndAddDateString)
        {
            var _beginAddDate = ConvertDateTimeIsNull(BeginAddDateString);
            var _endAdddDate = ConvertDateTimeIsNull(EndAddDateString);
            var model = prdCateService.GetAllBySearch(Keyword, _beginAddDate, _endAdddDate);

            PageIndex = p.ConvertIntPaging();
            ViewBag.TotalPage = (Math.Ceiling((double)model.Count / PageSize));
            ViewBag.CurrentPage = PageIndex;
            ViewBag.PageVisit = PageVisit;
            ViewBag.PageSize = PageSize;
            ViewBag.CountTotal = model.Count();
            model = model.Skip(PageSize * (PageIndex - 1))
                                    .Take(PageSize)
                                        .OrderBy(c => c.Sort)
                                            .ToList();
            ViewBag.ActiveMenu = RBACUser.RootPermissionId(Request);

            return PartialView(model);
        }

        [RBAC]
        public ActionResult Create()
        {
            ProductCategoryCreateViewModel model = new ProductCategoryCreateViewModel();
            model.IsDeleted = true;
            model.IsUrlRequiredVn = false;
            model.IsUrlRequiredEn = false;
            ViewBag.ActiveMenu = RBACUser.RootPermissionId(Request);
            return View(model);
        }

        [HttpPost]
        [RBAC]
        public ActionResult Create([Bind(Include =  "TitleSEOVn, TitleSEOEn, SlugSEOVn, SlugSEOEn, DescriptionSEOVn, DescriptionSEOEn, KeywordSEOVn, KeywordSEOEn, RouteDataUrlVnId, RouteDataUrlEnId, H1SEOVn, H1SEOEn, H2SEOVn, H2SEOEn, H3SEOVn, H3SEOEn, H4SEOVn, H4SEOEn, H5SEOVn, H5SEOEn, H6SEOVn, H6SEOEn, HtmlMetaRawVn, HtmlMetaRawEn, IsUrlRequiredVn, IsUrlRequiredEn, "
                                                        + "OgTitleVn, OgSite_nameVn, OgDescriptionVn, OgTitleEn, OgSite_nameEn, OgDescriptionEn, OgType, OgImageSrc, "
                                                        + "NameVn, NameEn, SlugVn, SlugEn, FullSlugVn, FullSlugEn, ShortDescriptionVn, ShortDescriptionEn, IsShowSearch, IsShowMenu, Sort, IsDeleted, ImageSrc, HomePageBackgroundSrc, BreakScrumBackgroundSrc")]ProductCategoryCreateViewModel obj)
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
                    ProductCategory model = Mapper.Map<ProductCategoryCreateViewModel, ProductCategory>(obj);

                    model.NameVn                    = !string.IsNullOrEmpty(obj.NameVn) ? obj.NameVn.Trim() : "";
                    model.NameEn                    = !string.IsNullOrEmpty(obj.NameEn) ? obj.NameEn.Trim() : "";
                    model.ImageSrc                  = obj.ImageSrc;
                    model.BreakScrumBackgroundSrc   = obj.BreakScrumBackgroundSrc;
                    model.HomePageBackgroundSrc     = obj.HomePageBackgroundSrc;
                    model.ImageSrc                  = obj.ImageSrc;
                    model.IsDeleted                 = !RBACUser.HasPermission("RecycleBin", Request.RequestContext.RouteData.Values["controller"].ToString()) ? false : !obj.IsDeleted;
                    model.AddedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
                    
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
                            routerVn.Controller     = "Category";
                            routerVn.Action         = "Detail";
                            routerVn.Area           = "";
                            routerVn.IsLanguage     = RouteDataUrl.RouteDataIsLanguage.Vn;
                            routerVn.IsType         = RouteDataUrl.RouteDataIsType.CategoryDetail;
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
                            routerEn.Controller     = "Category";
                            routerEn.Action         = "Detail";
                            routerEn.Area           = "";
                            routerEn.IsLanguage     = RouteDataUrl.RouteDataIsLanguage.En;
                            routerEn.IsType         = RouteDataUrl.RouteDataIsType.CategoryDetail;
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
                    id      = prdCateService.Create(model);
                    title   = Message.TITLE_REPORT;
                    message = Message.CONTENT_POSTDATA_CREATE_SUCCESSFULL;
                    status  = Default.Status_Sucessfull;
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
            var model = Mapper.Map<ProductCategory, ProductCategoryEditViewModel>(prdCateService.GetBy(gsid));
            if (model == null)
                Response.Redirect(string.Format("/Error/NotFound?url={0}", Request.Url), true);

            model.FullSlugVn = String.Format("/vn/san-pham/{0}", model.SlugVn);
            model.FullSlugEn = String.Format("/en/product/{0}", model.SlugEn);

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
        public ActionResult Update([Bind(Include =  "TitleSEOVn, TitleSEOEn, SlugSEOVn, SlugSEOEn, DescriptionSEOVn, DescriptionSEOEn, KeywordSEOVn, KeywordSEOEn, RouteDataUrlVnId, RouteDataUrlEnId, H1SEOVn, H1SEOEn, H2SEOVn, H2SEOEn, H3SEOVn, H3SEOEn, H4SEOVn, H4SEOEn, H5SEOVn, H5SEOEn, H6SEOVn, H6SEOEn, HtmlMetaRawVn, HtmlMetaRawEn, IsUrlRequiredVn, IsUrlRequiredEn, "
                                                        + "OgTitleVn, OgSite_nameVn, OgDescriptionVn, OgTitleEn, OgSite_nameEn, OgDescriptionEn, OgType, OgImageSrc, "
                                                        + "Id, NameVn, NameEn, SlugVn, SlugEn, FullSlugVn, FullSlugEn, ShortDescriptionVn, ShortDescriptionEn, IsDeleted, Sort, ImageSrc, HomePageBackgroundSrc, BreakScrumBackgroundSrc")]ProductCategoryEditViewModel obj)
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
                    var model = prdCateService.GetBy(obj.Id);
                    if (model != null)
                    {
                        model.NameVn                = !string.IsNullOrEmpty(obj.NameVn) ? obj.NameVn.Trim() : "";
                        model.NameEn                = !string.IsNullOrEmpty(obj.NameEn) ? obj.NameEn.Trim() : "";
                        model.SlugVn                = !string.IsNullOrEmpty(obj.SlugVn) ? obj.SlugVn.Trim().ToLower() : "";
                        model.SlugEn                = !string.IsNullOrEmpty(obj.SlugEn) ? obj.SlugEn.Trim().ToLower() : "";
                        model.ShortDescriptionVn        = obj.ShortDescriptionVn;
                        model.ShortDescriptionEn        = obj.ShortDescriptionEn;
                        model.ImageSrc                  = obj.ImageSrc;
                        model.BreakScrumBackgroundSrc   = obj.BreakScrumBackgroundSrc;
                        model.HomePageBackgroundSrc     = obj.HomePageBackgroundSrc;
                        model.Sort                      = obj.Sort;
                        model.IsDeleted                 = isRecycleBinPer ? !obj.IsDeleted : model.IsDeleted;
                        model.EditedBy                  = GSIDSessionFacade.GSIDSessionUserLogon.Id;

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
                                routerVn.Controller     = "Category";
                                routerVn.Action         = "Detail";
                                routerVn.Area           = "";
                                routerVn.IsLanguage     = RouteDataUrl.RouteDataIsLanguage.Vn;
                                routerVn.IsType         = RouteDataUrl.RouteDataIsType.CategoryDetail;
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
                                routerEn.Controller     = "Category";
                                routerEn.Action         = "Detail";
                                routerEn.Area           = "";
                                routerEn.IsLanguage     = RouteDataUrl.RouteDataIsLanguage.En;
                                routerEn.IsType         = RouteDataUrl.RouteDataIsType.CategoryDetail;
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

                        prdCateService.Update(model);

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

            var prds = prdService.GetAllByCategory(id);
            if (prds.Count <= 0)
            {
                var _hasObjDelete = prdCateService.Delete(id);
                if (_hasObjDelete)
                    status = ((int)StatusDelete.Deleted).ToString();
            }
            else
            {
                message = string.Format(Message.CONTENT_POSTDATA_DELETE_ERROR_OR_ERROR_EXIST_DATA, "Nhóm sản phẩm", "sản phẩm");
            }

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

            var _hasRecycleBin = prdCateService.GetBy(id);
            _hasRecycleBin.DeletedByDate = DateTime.Now;
            _hasRecycleBin.DeletedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
            _hasRecycleBin.IsDeleted = !isDeleted;
            prdCateService.Update(_hasRecycleBin);

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
        public JsonResult IsNameVnAvailable(string NameVn)
        {
            // Check if the NameVn already exists
            return Json(prdCateService.IsNameVnAvailable(NameVn) != null ? false : true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult IsNameVnIdAvailable(string NameVn, string Id)
        {
            // Check if the NameVn already exists
            return Json(prdCateService.IsNameVnAvailable(NameVn, Id) != null ? false : true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult IsNameEnAvailable(string NameEn)
        {
            // Check if the NameEn already exists
            return Json(prdCateService.IsNameEnAvailable(NameEn) != null ? false : true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult IsNameEnIdAvailable(string NameEn, string Id)
        {
            // Check if the NameEn already exists
            return Json(prdCateService.IsNameEnAvailable(NameEn, Id) != null ? false : true, JsonRequestBehavior.AllowGet);
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