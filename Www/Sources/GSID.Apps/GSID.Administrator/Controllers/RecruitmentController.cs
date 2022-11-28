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
    public class RecruitmentController : BaseAuthenticationController
    {
        private readonly IRouteDataUrlService routeDataUrlService;
        private readonly ICareerService careerService;
        private readonly ISiteService siteService;
        private readonly IPositionService positionService;
        private readonly IDepartmentService departmentService;
        private readonly IRecruitmentService recruitmentService;
        private readonly IRecruitmentTagService recruitmentTagService;
        private readonly ICurriculumVitaeService curriculumVitaeService;
        private readonly IParameterService paraService;
        // GET: Career
        public RecruitmentController(IRouteDataUrlService _routeDataUrlService, 
                                        ICareerService _careerService,
                                        ISiteService _siteService,
                                        IPositionService _positionService,
                                        IDepartmentService _departmentService,
                                        IRecruitmentService _recruitmentService,
                                        IRecruitmentTagService _recruitmentTagService,
                                        ICurriculumVitaeService _curriculumVitaeService,
                                        IParameterService _paraService)
        {
            routeDataUrlService = _routeDataUrlService;
            careerService = _careerService;
            siteService = _siteService;
            positionService = _positionService;
            departmentService = _departmentService;
            recruitmentService = _recruitmentService;
            recruitmentTagService = _recruitmentTagService;
            paraService = _paraService;
            curriculumVitaeService = _curriculumVitaeService;
        }

        [RBAC]
        public async Task<ActionResult> Index(string p)
        {
            RecruitmentFilterViewModel model = new RecruitmentFilterViewModel();
            model.Sites = siteService.GetAll();
            model.Positions = positionService.GetAll();
            model.Departments = departmentService.GetAll();
            model.Careers = careerService.GetAll();
            model.RecruitmentTags = recruitmentTagService.GetAll();
            ViewBag.ActiveMenu = RBACUser.RootPermissionId(Request);

            return View(model);
        }

        public async Task<ActionResult> PartialIndex(string p, string Keyword, string BeginAddDateString, string EndAddDateString, string BeginExpirationDateString, string EndExpirationDateString, string[] SiteId, string[] PositionId, string[] DepartmentId, string[] CareerId, string[] RecruitmentTagId)
        {
            var _beginAddDate = ConvertDateTimeIsNull(BeginAddDateString);
            var _endAdddDate = ConvertDateTimeIsNull(EndAddDateString);
            var _beginExpirationDateString = ConvertDateTimeIsNull(BeginExpirationDateString);
            var _endExpirationDateString = ConvertDateTimeIsNull(EndExpirationDateString);
            var model = recruitmentService.GetAllBySearch(Keyword, _beginAddDate, _endAdddDate, _beginExpirationDateString, _endExpirationDateString, SiteId, PositionId,  DepartmentId, CareerId, RecruitmentTagId);

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
            RecruitmentCreateViewModel model = new RecruitmentCreateViewModel();
            model.Sites = siteService.GetAll(false);
            model.Positions = new List<Position>();
            model.Departments = departmentService.GetAll(false);
            model.Careers = careerService.GetAll(false);
            model.RecruitmentTags = recruitmentTagService.GetAll(false);
            model.IsDeleted = true;
            ViewBag.ActiveMenu = RBACUser.RootPermissionId(Request);
            return View(model);
        }

        [HttpPost]
        [RBAC]
        public ActionResult Create([Bind(Include = "TitleSEOVn, TitleSEOEn, SlugVn, SlugEn, DescriptionSEOVn, DescriptionSEOEn, KeywordSEOVn, KeywordSEOEn, RouteDataUrlVnId, RouteDataUrlEnId, H1SEOVn, H1SEOEn, H2SEOVn, H2SEOEn, H3SEOVn, H3SEOEn, H4SEOVn, H4SEOEn, H5SEOVn, H5SEOEn, H6SEOVn, H6SEOEn, OgImageSrc,"
                                                        + "OgTitleVn, OgSite_nameVn, OgDescriptionVn, OgTitleEn, OgSite_nameEn, OgDescriptionEn, OgType, "
                                                        + "NameVn, NameEn, DescriptionVn, DescriptionEn, SiteId, PositionId, DepartmentId, CareerId, ExperienceVn, ExperienceEn, SalaryVn, SalaryEn, ExpirationDateString, RecruitmentTagId, IsDeleted, ImageSrc")] RecruitmentCreateViewModel obj)
        {
            string title = Message.TITLE_ERROR;
            string message = Message.CONTENT_POSTDATA_DEFAUT_ERROR;
            string status = Default.Status_Error;
            string id = string.Empty;
            try
            {
                if (!RBACUser.HasPermission("RecycleBin", "Recruitment"))
                    ModelState.Remove("IsDeleted");
                if (ModelState.IsValid)
                {
                    var routerVn = routeDataUrlService.GetByUrl(obj.SlugSEOVn);
                    var routerEn = routeDataUrlService.GetByUrl(obj.SlugSEOEn);
                    if (routerVn == null && routerEn == null && obj.SlugSEOVn != obj.SlugSEOEn)
                    {
                        Recruitment model = Mapper.Map<RecruitmentCreateViewModel, Recruitment>(obj);

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
                        routerVn.Controller     = "Recruitment";
                        routerVn.Action         = "Detail";
                        routerVn.Area           = "";
                        routerVn.IsLanguage     = RouteDataUrl.RouteDataIsLanguage.Vn;
                        routerVn.IsType         = RouteDataUrl.RouteDataIsType.RecruitmentDetail;
                        //OpenGraphMetaData
                        routerVn.OgTitle        = obj.OgTitleVn;
                        routerVn.OgSite_name    = obj.OgSite_nameVn;
                        routerVn.OgDescription  = obj.OgDescriptionVn;
                        routerVn.OgType         = obj.OgType;
                        routerVn.OgImageSrc = obj.OgImageSrc;
                        model.RouteDataUrlVnId = routeDataUrlService.Create(routerVn);

                        routerEn            = new RouteDataUrl();
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
                        routerEn.Controller     = "Recruitment";
                        routerEn.Action         = "Detail";
                        routerEn.Area           = "";
                        routerEn.IsLanguage     = RouteDataUrl.RouteDataIsLanguage.En;
                        routerEn.IsType         = RouteDataUrl.RouteDataIsType.RecruitmentDetail;
                        //OpenGraphMetaData
                        routerEn.OgTitle        = obj.OgTitleEn;
                        routerEn.OgSite_name    = obj.OgSite_nameEn;
                        routerEn.OgDescription  = obj.OgDescriptionEn;
                        routerEn.OgType         = obj.OgType;
                        routerEn.OgImageSrc = obj.OgImageSrc;
                        model.RouteDataUrlEnId = routeDataUrlService.Create(routerEn);

                        model.ImageSrc = obj.ImageSrc;
                        model.NameVn = !string.IsNullOrEmpty(obj.NameVn) ? obj.NameVn.Trim() : "";
                        model.NameEn = !string.IsNullOrEmpty(obj.NameEn) ? obj.NameEn.Trim() : "";
                        //model.SlugVn = !string.IsNullOrEmpty(obj.SlugVn) ? obj.SlugVn.Trim() : "";
                        //model.SlugEn = !string.IsNullOrEmpty(obj.SlugEn) ? obj.SlugEn.Trim() : "";
                        model.ExpirationDate = ConvertDateTimeIsNull(obj.ExpirationDateString);
                        model.IsDeleted = !RBACUser.HasPermission("RecycleBin", "Recruitment") ? false : !obj.IsDeleted;
                        model.AddedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
                        id = recruitmentService.Create(model);
                        title = Message.TITLE_REPORT;
                        message = Message.CONTENT_POSTDATA_CREATE_SUCCESSFULL;
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
                Id = id,
                Title = title,
                Message = message,
                Status = status
            }, JsonRequestBehavior.AllowGet);
        }

        [RBAC]
        public ActionResult Update(string gsid)
        {
            var obj = recruitmentService.GetBy(gsid);
            if (obj == null)
                Response.Redirect(string.Format("/Error/NotFound?url={0}", Request.Url), true);

            var model = Mapper.Map<Recruitment, RecruitmentEditViewModel>(obj);

            model.ExpirationDateString = (obj.ExpirationDate.HasValue ? obj.ExpirationDate.Value.ToString("dd/MM/yyyy") : "");
            model.Sites             = siteService.GetAll(false);
            model.Departments       = departmentService.GetAll(false);
            model.Positions         = !string.IsNullOrEmpty(model.DepartmentId) ? positionService.GetAllByDepartmentIsDeleted(model.DepartmentId, false) : new List<Position>();
            model.Careers           = careerService.GetAll(false);
            model.RecruitmentTags   = recruitmentTagService.GetAll(false);
            model.IsDeleted = !model.IsDeleted;

            var routerVn = !string.IsNullOrEmpty(model.RouteDataUrlVnId) ? routeDataUrlService.GetBy(model.RouteDataUrlVnId) : null;
            if (routerVn != null)
            {
                model.TitleSEOVn        = routerVn.Title;
                model.SlugSEOVn         = routerVn.Url;
                model.DescriptionSEOVn  = routerVn.Description;
                model.KeywordSEOVn = routerVn.Keyword;
                model.H1SEOVn = routerVn.H1;
                model.H2SEOVn = routerVn.H2;
                model.H3SEOVn = routerVn.H3;
                model.H4SEOVn = routerVn.H4;
                model.H5SEOVn = routerVn.H5;
                model.H6SEOVn = routerVn.H6;
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
                model.SlugSEOEn = routerEn.Url;
                model.DescriptionSEOEn  = routerEn.Description;
                model.KeywordSEOEn      = routerEn.Keyword;
                model.H1SEOEn           = routerEn.H1;
                model.H2SEOEn           = routerEn.H2;
                model.H3SEOEn           = routerEn.H3;
                model.H4SEOEn           = routerEn.H4;
                model.H5SEOEn           = routerEn.H5;
                model.H6SEOEn           = routerEn.H6;
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
        public ActionResult Update( [Bind(Include = "TitleSEOVn, TitleSEOEn, SlugVn, SlugEn, DescriptionSEOVn, DescriptionSEOEn, KeywordSEOVn, KeywordSEOEn, RouteDataUrlVnId, RouteDataUrlEnId, H1SEOVn, H1SEOEn, H2SEOVn, H2SEOEn, H3SEOVn, H3SEOEn, H4SEOVn, H4SEOEn, H5SEOVn, H5SEOEn, H6SEOVn, H6SEOEn, OgImageSrc,"
                                                        + "OgTitleVn, OgSite_nameVn, OgDescriptionVn, OgTitleEn, OgSite_nameEn, OgDescriptionEn, OgType, "
                                                        + "Id, NameVn, NameEn, DescriptionVn, DescriptionEn, SiteId, PositionId, DepartmentId, CareerId, ExperienceVn, ExperienceEn, SalaryVn, SalaryEn, ExpirationDateString, RecruitmentTagId, IsDeleted, ImageSrc")] RecruitmentEditViewModel obj)
        {
            string title = Message.TITLE_ERROR;
            string message = Message.CONTENT_POSTDATA_DEFAUT_ERROR;
            string status = Default.Status_Error;
            try
            {
                if (!RBACUser.HasPermission("RecycleBin", "Recruitment"))
                    ModelState.Remove("IsDeleted");
                if (ModelState.IsValid)
                {
                    var model = recruitmentService.GetBy(obj.Id);
                    if (model != null)
                    {
                        
                        var routerVn = routeDataUrlService.GetBy(obj.RouteDataUrlVnId ?? "");
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
                        routerVn.Controller     = "Recruitment";
                        routerVn.Action         = "Detail";
                        routerVn.Area           = "";
                        routerVn.IsLanguage     = RouteDataUrl.RouteDataIsLanguage.Vn;
                        routerVn.IsType         = RouteDataUrl.RouteDataIsType.RecruitmentDetail;
                        //OpenGraphMetaData
                        routerVn.OgTitle        = obj.OgTitleVn;
                        routerVn.OgSite_name    = obj.OgSite_nameVn;
                        routerVn.OgDescription  = obj.OgDescriptionVn;
                        routerVn.OgType         = obj.OgType;
                        routerVn.OgImageSrc = obj.OgImageSrc;
                        if (_flagCreateRouterVn)
                            model.RouteDataUrlVnId = routeDataUrlService.Create(routerVn);
                        else
                            routeDataUrlService.Update(routerVn);


                        var routerEn = routeDataUrlService.GetBy(obj.RouteDataUrlEnId ?? "");
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
                        routerEn.Controller     = "Recruitment";
                        routerEn.Action         = "Detail";
                        routerEn.Area           = "";
                        routerEn.IsLanguage     = RouteDataUrl.RouteDataIsLanguage.En;
                        routerEn.IsType         = RouteDataUrl.RouteDataIsType.RecruitmentDetail;
                        //OpenGraphMetaData
                        routerEn.OgTitle        = obj.OgTitleEn;
                        routerEn.OgSite_name    = obj.OgSite_nameEn;
                        routerEn.OgDescription  = obj.OgDescriptionEn;
                        routerEn.OgType         = obj.OgType;
                        routerEn.OgImageSrc = obj.OgImageSrc;
                        if (_flagCreateRouterEn)
                            model.RouteDataUrlEnId = routeDataUrlService.Create(routerEn);
                        else
                            routeDataUrlService.Update(routerEn);
                        model.ImageSrc = obj.ImageSrc;
                        model.NameVn = !string.IsNullOrEmpty(obj.NameVn) ? obj.NameVn.Trim() : "";
                        model.NameEn = !string.IsNullOrEmpty(obj.NameEn) ? obj.NameEn.Trim() : "";

                        model.ExpirationDate = ConvertDateTimeIsNull(obj.ExpirationDateString);
                        model.DescriptionVn = obj.DescriptionVn;
                        model.DescriptionEn = obj.DescriptionEn;
                        model.SiteId = obj.SiteId;
                        model.PositionId = obj.PositionId;
                        model.DepartmentId = obj.DepartmentId;
                        model.CareerId = obj.CareerId;
                        model.ExperienceVn = obj.ExperienceVn;
                        model.ExperienceEn = obj.ExperienceEn;
                        model.SalaryVn = obj.SalaryVn;
                        model.SalaryEn = obj.SalaryEn;
                        model.RecruitmentTagId = obj.RecruitmentTagId;
                        if (RBACUser.HasPermission("RecycleBin", "Recruitment"))
                            model.IsDeleted = !obj.IsDeleted;
                        model.EditedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
                        recruitmentService.Update(model);

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
        public ActionResult Duplicate(string id)
        {
            string title = Message.TITLE_ERROR;
            string status = ((int)AjaxStatus.Error).ToString();
            string message = Message.CONTENT_POSTDATA_DELETE_ERROR_OR_EMPTY;
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var obj = recruitmentService.GetBy(id);
            if (obj == null)
                Response.Redirect(string.Format("/Error/NotFound?url={0}", Request.Url), true);

            var random = RandomStringGenerator.RandomStringToken(6);

            var objNew = new Recruitment();
            var routerOldVn = routeDataUrlService.GetBy(obj.RouteDataUrlVnId ?? "");
            if (routerOldVn != null)
            {
                var routerVn            = new RouteDataUrl();
                routerVn.Title          = routerOldVn.Title;
                routerVn.Url            = string.Format("ban-sao-{0}-{1}", random, routerOldVn.Url).ToLower();
                routerVn.Description    = routerOldVn.Description;
                routerVn.Keyword        = routerOldVn.Keyword;
                routerVn.H1             = routerOldVn.H1;
                routerVn.H2             = routerOldVn.H2;
                routerVn.H3             = routerOldVn.H3;
                routerVn.H4             = routerOldVn.H4;
                routerVn.H5             = routerOldVn.H5;
                routerVn.H6             = routerOldVn.H6;
                routerVn.Controller     = routerOldVn.Controller;
                routerVn.Action         = routerOldVn.Action;
                routerVn.Area           = routerOldVn.Area;
                routerVn.IsLanguage     = routerOldVn.IsLanguage;
                routerVn.IsType         = routerOldVn.IsType;
                routerVn.OgTitle        = routerOldVn.OgTitle;
                routerVn.OgSite_name    = routerOldVn.OgSite_name;
                routerVn.OgDescription  = routerOldVn.OgDescription;
                routerVn.OgType         = routerOldVn.OgType;
                routerVn.OgImageSrc     = routerOldVn.OgImageSrc;
                objNew.RouteDataUrlVnId = routeDataUrlService.Create(routerVn);
            }

            var routerOldEn = routeDataUrlService.GetBy(obj.RouteDataUrlEnId ?? "");
            if (routerOldEn != null)
            {
                var routerEn = new RouteDataUrl();
                routerEn.Title          = routerOldEn.Title;
                routerEn.Url            = string.Format("copy-{0}-{1}", random, routerOldEn.Url).ToLower();
                routerEn.Description    = routerOldEn.Description;
                routerEn.Keyword        = routerOldEn.Keyword;
                routerEn.H1             = routerOldEn.H1;
                routerEn.H2             = routerOldEn.H2;
                routerEn.H3             = routerOldEn.H3;
                routerEn.H4             = routerOldEn.H4;
                routerEn.H5             = routerOldEn.H5;
                routerEn.H6             = routerOldEn.H6;
                routerEn.Controller     = routerOldEn.Controller;
                routerEn.Action         = routerOldEn.Action;
                routerEn.Area           = routerOldEn.Area;
                routerEn.IsLanguage     = routerOldEn.IsLanguage;
                routerEn.IsType         = routerOldEn.IsType;
                routerEn.OgTitle        = routerOldVn.OgTitle;
                routerEn.OgSite_name    = routerOldVn.OgSite_name;
                routerEn.OgDescription  = routerOldVn.OgDescription;
                routerEn.OgType         = routerOldVn.OgType;
                routerEn.OgImageSrc     = routerOldVn.OgImageSrc;
                objNew.RouteDataUrlVnId = routeDataUrlService.Create(routerEn);
            }


            objNew.NameVn = string.Format("[Bản sao {0}] - {1}", random, obj.NameVn);
            objNew.NameEn = string.Format("[copy {0}] - {1}", random, obj.NameEn);

            objNew.ExpirationDate = obj.ExpirationDate;
            objNew.DescriptionVn = obj.DescriptionVn;
            objNew.DescriptionEn = obj.DescriptionEn;
            objNew.SiteId = obj.SiteId;
            objNew.PositionId = obj.PositionId;
            objNew.DepartmentId = obj.DepartmentId;
            objNew.CareerId = obj.CareerId;
            objNew.ExperienceVn = obj.ExperienceVn;
            objNew.ExperienceEn = obj.ExperienceEn;
            objNew.SalaryVn = obj.SalaryVn;
            objNew.SalaryEn = obj.SalaryEn;
            objNew.RecruitmentTagId = obj.RecruitmentTagId;
            objNew.ImageSrc     = obj.ImageSrc;
            objNew.IsDeleted = true;
            string newId = recruitmentService.Create(objNew);

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

            var _hasObjDelete = recruitmentService.GetBy(id);
            var recru = curriculumVitaeService.GetAllByRecruitment(id);
            if (recru.Count <= 0 && _hasObjDelete != null)
            {
                var _hasDelete = recruitmentService.Delete(id);
                if (_hasDelete)
                    status = ((int)StatusDelete.Deleted).ToString();
            }
            else
            {
                message = "Hệ thống tồn tại hồ sơ ứng viên ứng tuyển bài viết này. Vui lòng xóa các hồ sơ ứng tuyển liên quan.";
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

            var _hasRecycleBin = recruitmentService.GetBy(id);
            _hasRecycleBin.DeletedByDate = DateTime.Now;
            _hasRecycleBin.DeletedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
            _hasRecycleBin.IsDeleted = !isDeleted;
            recruitmentService.Update(_hasRecycleBin);

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
        public ActionResult GetCareer(string CareerId)
        {
            string status = ((int)StatusDelete.Error).ToString();
            string message = Message.CONTENT_POSTDATA_DELETE_ERROR_OR_EMPTY;
            if (CareerId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var _has = careerService.GetBy(CareerId);
            message = Message.CONTENT_POSTDATA_UPDATE_SUCCESSFULL;
            status = Default.Status_Sucessfull;

            return Json(new
            {
                Model = _has,
                Message = message,
                Status = status
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FormFilterPositionByDepartment(string Department)
        {
            PositionFilterModel model = new PositionFilterModel();
            model.Positions = positionService.GetAllByDepartmentIsDeleted(Department, false);
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult IsNameVnAvailable(string NameVn)
        {
            // Check if the NameVn already exists
            return Json(recruitmentService.IsNameVnAvailable(NameVn) != null ? false : true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult IsNameVnIdAvailable(string NameVn, string Id)
        {
            // Check if the NameVn already exists
            return Json(recruitmentService.IsNameVnAvailable(NameVn, Id) != null ? false : true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult IsNameEnAvailable(string NameEn)
        {
            // Check if the NameEn already exists
            return Json(recruitmentService.IsNameEnAvailable(NameEn) != null ? false : true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult IsNameEnIdAvailable(string NameEn, string Id)
        {
            // Check if the NameEn already exists
            return Json(recruitmentService.IsNameEnAvailable(NameEn, Id) != null ? false : true, JsonRequestBehavior.AllowGet);
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