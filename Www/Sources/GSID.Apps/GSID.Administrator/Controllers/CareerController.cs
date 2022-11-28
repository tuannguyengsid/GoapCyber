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
    public class CareerController : BaseAuthenticationController
    {
        // GET: Career
        private readonly IRouteDataUrlService routeDataUrlService;
        private readonly ICareerService careerService;
        private readonly IRecruitmentService recruitService;
        private readonly IParameterService paraService;
        // GET: Career
        public CareerController(IRouteDataUrlService _routeDataUrlService,
                                    ICareerService _careerService,
                                    IRecruitmentService _recruitService,
                                    IParameterService _paraService)
        {
            routeDataUrlService = _routeDataUrlService;
            careerService = _careerService;
            paraService = _paraService;
            recruitService = _recruitService;
        }

        [RBAC]
        public async Task<ActionResult> Index(string p)
        {
            CareerFilterViewModel model = new CareerFilterViewModel();
            ViewBag.ActiveMenu = RBACUser.RootPermissionId(Request);

            return View(model);
        }

        public async Task<ActionResult> PartialIndex(string p, string Keyword, string BeginAddDateString, string EndAddDateString)
        {
            var _beginAddDate = ConvertDateTimeIsNull(BeginAddDateString);
            var _endAdddDate = ConvertDateTimeIsNull(EndAddDateString);
            var model = careerService.GetAllBySearch(Keyword, _beginAddDate, _endAdddDate);

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

            return PartialView(model);
        }

        [RBAC]
        public ActionResult Create()
        {
            CareerCreateViewModel model = new CareerCreateViewModel();
            model.SlugSEOVn = model.SlugSEOEn = "/";
            model.IsDeleted = true;
            ViewBag.ActiveMenu = RBACUser.RootPermissionId(Request);
            return View(model);
        }

        [HttpPost]
        [RBAC]
        public ActionResult Create([Bind(Include = "TitleSEOVn, TitleSEOEn, SlugVn, SlugEn, DescriptionSEOVn, DescriptionSEOEn, KeywordSEOVn, KeywordSEOEn, RouteDataUrlVnId, RouteDataUrlEnId, H1SEOVn, H1SEOEn, H2SEOVn, H2SEOEn, H3SEOVn, H3SEOEn, H4SEOVn, H4SEOEn, H5SEOVn, H5SEOEn, H6SEOVn, H6SEOEn,"
                                                        + "OgTitleVn, OgSite_nameVn, OgDescriptionVn, OgTitleEn, OgSite_nameEn, OgDescriptionEn, OgType, OgImageSrc, "
                                                        + "NameVn, NameEn, IsShowRecruitmentPage, Sort, IsDeleted")] CareerCreateViewModel obj)
        {
            string title = Message.TITLE_ERROR;
            string message = Message.CONTENT_POSTDATA_DEFAUT_ERROR;
            string status = Default.Status_Error;
            string id = string.Empty;
            try
            {
                if (!RBACUser.HasPermission("RecycleBin", "Career"))
                    ModelState.Remove("IsDeleted");

                if (ModelState.IsValid)
                {
                    var routerVn = routeDataUrlService.GetByUrl(obj.SlugSEOVn);
                    var routerEn = routeDataUrlService.GetByUrl(obj.SlugSEOEn);
                    if (routerVn == null && routerEn == null && obj.SlugSEOVn != obj.SlugSEOEn)
                    {
                        Career model = Mapper.Map<CareerCreateViewModel, Career>(obj);

                        routerVn            = new RouteDataUrl();
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
                        routerVn.Action         = "Career";
                        routerVn.Area           = "";
                        routerVn.IsLanguage     = RouteDataUrl.RouteDataIsLanguage.Vn;
                        routerVn.IsType         = RouteDataUrl.RouteDataIsType.RecruitmentCareer;
                        //OpenGraphMetaData
                        routerVn.OgTitle        = obj.OgTitleVn;
                        routerVn.OgSite_name    = obj.OgSite_nameVn;
                        routerVn.OgDescription  = obj.OgDescriptionVn;
                        routerVn.OgType         = obj.OgType;
                        routerVn.OgImageSrc     = obj.OgImageSrc;
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
                        routerEn.Action         = "Career";
                        routerEn.Area           = "";
                        routerEn.IsLanguage     = RouteDataUrl.RouteDataIsLanguage.En;
                        routerEn.IsType         = RouteDataUrl.RouteDataIsType.RecruitmentCareer;
                        //OpenGraphMetaData
                        routerEn.OgTitle        = obj.OgTitleEn;
                        routerEn.OgSite_name    = obj.OgSite_nameEn;
                        routerEn.OgDescription  = obj.OgDescriptionEn;
                        routerEn.OgType         = obj.OgType;
                        routerEn.OgImageSrc     = obj.OgImageSrc;
                        model.RouteDataUrlEnId = routeDataUrlService.Create(routerEn);

                        model.NameVn = !string.IsNullOrEmpty(obj.NameVn) ? obj.NameVn.Trim() : "";
                        model.NameEn = !string.IsNullOrEmpty(obj.NameEn) ? obj.NameEn.Trim() : "";
                        model.IsDeleted = !RBACUser.HasPermission("RecycleBin", "Career") ? false : !obj.IsDeleted;
                        model.AddedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
                        id = careerService.Create(model);
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
            var model = Mapper.Map<Career, CareerEditViewModel>(careerService.GetBy(gsid));
            if (model == null)
                Response.Redirect(string.Format("/Error/NotFound?url={0}", Request.Url), true);

            model.IsDeleted = !model.IsDeleted;

            var routerVn = !string.IsNullOrEmpty(model.RouteDataUrlVnId) ? routeDataUrlService.GetBy(model.RouteDataUrlVnId) : null;
            if (routerVn != null)
            {
                model.TitleSEOVn        = routerVn.Title;
                model.SlugSEOVn = routerVn.Url;
                model.DescriptionSEOVn = routerVn.Description;
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
        public ActionResult Update([Bind(Include = "TitleSEOVn, TitleSEOEn, SlugVn, SlugEn, DescriptionSEOVn, DescriptionSEOEn, KeywordSEOVn, KeywordSEOEn, RouteDataUrlVnId, RouteDataUrlEnId, H1SEOVn, H1SEOEn, H2SEOVn, H2SEOEn, H3SEOVn, H3SEOEn, H4SEOVn, H4SEOEn, H5SEOVn, H5SEOEn, H6SEOVn, H6SEOEn, OgImageSrc,"
                                                        + "OgTitleVn, OgSite_nameVn, OgDescriptionVn, OgTitleEn, OgSite_nameEn, OgDescriptionEn, OgType, "
                                                        + "Id, NameVn, NameEn, IsShowRecruitmentPage, Sort, IsDeleted")] CareerEditViewModel obj)
        {
            string title = Message.TITLE_ERROR;
            string message = Message.CONTENT_POSTDATA_DEFAUT_ERROR;
            string status = Default.Status_Error;
            try
            {
                if (!RBACUser.HasPermission("RecycleBin", "Career"))
                    ModelState.Remove("IsDeleted");
                if (ModelState.IsValid)
                {
                    var model = careerService.GetBy(obj.Id);
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
                        routerVn.Action         = "Career";
                        routerVn.Area           = "";
                        routerVn.IsLanguage     = RouteDataUrl.RouteDataIsLanguage.Vn;
                        routerVn.IsType         = RouteDataUrl.RouteDataIsType.RecruitmentCareer;
                        //OpenGraphMetaData
                        routerVn.OgTitle        = obj.OgTitleVn;
                        routerVn.OgSite_name    = obj.OgSite_nameVn;
                        routerVn.OgDescription  = obj.OgDescriptionVn;
                        routerVn.OgType         = obj.OgType;
                        routerVn.OgImageSrc     = obj.OgImageSrc;
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
                        routerEn.Action         = "Career";
                        routerEn.Area           = "";
                        routerEn.IsLanguage     = RouteDataUrl.RouteDataIsLanguage.En;
                        routerEn.IsType         = RouteDataUrl.RouteDataIsType.RecruitmentCareer;
                        //OpenGraphMetaData
                        routerEn.OgTitle        = obj.OgTitleEn;
                        routerEn.OgSite_name    = obj.OgSite_nameEn;
                        routerEn.OgDescription  = obj.OgDescriptionEn;
                        routerEn.OgType         = obj.OgType;
                        routerEn.OgImageSrc     = obj.OgImageSrc;

                        if (_flagCreateRouterEn)
                            model.RouteDataUrlEnId = routeDataUrlService.Create(routerEn);
                        else
                            routeDataUrlService.Update(routerEn);

                        model.NameVn = !string.IsNullOrEmpty(obj.NameVn) ? obj.NameVn.Trim() : "";
                        model.NameEn = !string.IsNullOrEmpty(obj.NameEn) ? obj.NameEn.Trim() : "";
                        model.IsShowRecruitmentPage = obj.IsShowRecruitmentPage;
                        model.Sort = obj.Sort;
                        if (RBACUser.HasPermission("RecycleBin", "Career"))
                            model.IsDeleted = !obj.IsDeleted;
                        model.EditedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
                        careerService.Update(model);

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

            var prds = recruitService.GetAllByCareer(id);
            if (prds.Count <= 0)
            {
                var _hasDelete = careerService.Delete(id);
                if (_hasDelete)
                    status = ((int)StatusDelete.Deleted).ToString();
            }
            else
            {
                message = string.Format(Message.CONTENT_POSTDATA_DELETE_ERROR_OR_ERROR_EXIST_DATA, "Ngành nghề", "tin tuyển dụng");
            }

            return Json(new
            {
                Status = status,
                Message = message
            });
        }

        [HttpPost]
        public ActionResult IsShowRecruitment(string id, bool isShow)
        {
            string title = Message.TITLE_ERROR;
            string status = ((int)StatusDelete.Error).ToString();
            string message = Message.CONTENT_POSTDATA_DELETE_ERROR_OR_EMPTY;
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var _hasShow = careerService.GetBy(id);
            _hasShow.IsShowRecruitmentPage = isShow;
            careerService.Update(_hasShow);

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
        public ActionResult RecycleBin(string id, bool isDeleted)
        {
            string title = Message.TITLE_ERROR;
            string status = ((int)StatusDelete.Error).ToString();
            string message = Message.CONTENT_POSTDATA_DELETE_ERROR_OR_EMPTY;
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var _hasRecycleBin = careerService.GetBy(id);
            _hasRecycleBin.DeletedByDate = DateTime.Now;
            _hasRecycleBin.DeletedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
            _hasRecycleBin.IsDeleted = !isDeleted;
            careerService.Update(_hasRecycleBin);

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
            return Json(careerService.IsNameVnAvailable(NameVn) != null ? false : true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult IsNameVnIdAvailable(string NameVn, string Id)
        {
            // Check if the NameVn already exists
            return Json(careerService.IsNameVnAvailable(NameVn, Id) != null ? false : true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult IsNameEnAvailable(string NameEn)
        {
            // Check if the NameEn already exists
            return Json(careerService.IsNameEnAvailable(NameEn) != null ? false : true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult IsNameEnIdAvailable(string NameEn, string Id)
        {
            // Check if the NameEn already exists
            return Json(careerService.IsNameEnAvailable(NameEn, Id) != null ? false : true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult IsUrlVnIdAvailable(string SlugVn, string RouteDataUrlVnId, string SlugEn)
        {
            SlugVn = !string.IsNullOrEmpty(SlugVn) ? SlugVn.Trim().ToLower() : "";
            SlugEn = !string.IsNullOrEmpty(SlugEn) ? SlugEn.Trim().ToLower() : "";
            if (SlugVn == SlugEn)
                return Json( false, JsonRequestBehavior.AllowGet);
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

        private string InsertOneImageForm(HttpPostedFileBase file, string title)
        {
            string filePathSrc = string.Empty;
            if (file != null && file.ContentLength != 0)
            {
                var objPath = paraService.GetByCode((new SaveFilePathConfig()).Code);
                if (objPath != null)
                {
                    var paraPath = JsonConvert.DeserializeObject<SaveFilePathConfig>(objPath.Content.ToString());
                    pathForUrl = paraPath.RecruitmentPath;
                    pathForSaving = string.Format(@"{0}\{1}", GSIDSessionSiteInformation.PathAddressSiteMultimedia, pathForUrl);
                }

                var extension = "";
                switch (file.ContentType.ToLower())
                {
                    case "image/png":
                        extension = "png";
                        break;
                    case "image/jpeg":
                        extension = "jpeg";
                        break;
                }

                if (this.CreateFolderIfNeeded(pathForSaving))
                {
                    try
                    {
                        string filename = string.Format("{0}{1}.{2}", title, Guid.NewGuid().ToString().Replace("-", ""), extension);
                        file.SaveAs(Path.Combine(pathForSaving, filename));

                        filePathSrc = string.Format("{0}/{1}", pathForUrl.Replace(@"\", "/"), filename);
                    }
                    catch
                    {
                    }
                }
            }
            return filePathSrc;
        }

        private string UpdateOneImageForm(HttpPostedFileBase file, string filePathSrc, string title)
        {
            if (file != null && file.ContentLength != 0)
            {
                if (!string.IsNullOrEmpty(filePathSrc))
                {
                    try
                    {
                        string filepath = string.Format(@"{0}\{1}", GSIDSessionSiteInformation.PathAddressSiteMultimedia, filePathSrc.Replace("/", "\\"));
                        if (System.IO.File.Exists(filepath))
                            System.IO.File.Delete(filepath);
                    }
                    catch
                    {
                    }

                    filePathSrc = string.Empty;
                }

                var objPath = paraService.GetByCode((new SaveFilePathConfig()).Code);
                if (objPath != null)
                {
                    var paraPath = JsonConvert.DeserializeObject<SaveFilePathConfig>(objPath.Content.ToString());
                    pathForUrl = paraPath.RecruitmentPath;
                    pathForSaving = string.Format(@"{0}{1}", GSIDSessionSiteInformation.PathAddressSiteMultimedia, pathForUrl);
                }


                var extension = "";
                switch (file.ContentType.ToLower())
                {
                    case "image/png":
                        extension = "png";
                        break;
                    case "image/jpeg":
                        extension = "jpeg";
                        break;
                }

                if (this.CreateFolderIfNeeded(pathForSaving))
                {
                    try
                    {
                        string filename = string.Format("{0}{1}.{2}", title, Guid.NewGuid().ToString().Replace("-", ""), extension);
                        file.SaveAs(Path.Combine(pathForSaving, filename));

                        filePathSrc = string.Format("{0}/{1}", pathForUrl.Replace(@"\", "/"), filename);
                    }
                    catch
                    {
                    }
                }
            }
            return filePathSrc;
        }
    }
}