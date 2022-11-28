using GSID.Admin.Areas.PageManagement.ViewModels;
using GSID.Admin.Controllers;
using GSID.Model.ExtraEntities;
using GSID.Setting;
using GSID.Setting.FormKeys;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GSID.Model.MongodbModels;
using static GSID.Model.MongodbModels.Parameter;
using AutoMapper;
using System.IO;
using GSID.Service.MongoRepositories.Service;

namespace GSID.Admin.Areas.PageManagement.Controllers
{
    public partial class RecruitmentPageController : BaseAuthenticationController
    {
        private readonly IRouteDataUrlService routeDataUrlService;
        private readonly IParameterService paraService;
        private readonly IMenuNodeService menuNodeService;
        // GET: CategoryPage
        public RecruitmentPageController(IRouteDataUrlService _routeDataUrlService, 
            IParameterService _paraService,
            IMenuNodeService _menuNodeService)
        {
            routeDataUrlService = _routeDataUrlService;
            paraService = _paraService;
            menuNodeService = _menuNodeService;
        }

        // GET: PageManagement/CategoryPage
        public ActionResult Index()
        {
            RecruitmentPageViewModel model = new RecruitmentPageViewModel();

            var paraConfig = new RecruitmentPageManagementAdminConfig();
            var para = paraService.GetByCode(paraConfig.Code);
            if (para != null)
            {
                paraConfig = JsonConvert.DeserializeObject<RecruitmentPageManagementAdminConfig>(para.Content.ToString());
                model = Mapper.Map<RecruitmentPageManagementAdminConfig, RecruitmentPageViewModel>(paraConfig);
                var routerVn = routeDataUrlService.GetBy(model.RouteDataUrlVnId ?? "");
                if (routerVn != null)
                {
                    model.TitleVn           = routerVn.Title;
                    model.SlugVn            = routerVn.Url;
                    model.DescriptionVn     = routerVn.Description;
                    model.KeywordVn         = routerVn.Keyword;
                    model.OgTitleVn         = routerVn.OgTitle;
                    model.OgSite_nameVn     = routerVn.OgSite_name;
                    model.OgDescriptionVn   = routerVn.OgDescription;
                    model.OgType            = routerVn.OgType;
                    model.OgImageSrc        = routerVn.OgImageSrc;
                }

                var routerEn = routeDataUrlService.GetBy(model.RouteDataUrlEnId ?? "");
                if (routerEn != null)
                {
                    model.TitleEn           = routerEn.Title;
                    model.SlugEn            = routerEn.Url;
                    model.DescriptionEn     = routerEn.Description;
                    model.KeywordEn         = routerEn.Keyword;
                    model.OgTitleEn         = routerEn.OgTitle;
                    model.OgSite_nameEn     = routerEn.OgSite_name;
                    model.OgDescriptionEn   = routerEn.OgDescription;
                }
            }
            model.MenuNodes = menuNodeService.GetAllParent("");
            return View(model);
        }

        [HttpPost]
        public ActionResult EditPage([Bind(Include = "NameVn, NameEn, TitleVn, TitleEn, SlugVn, SlugEn, DescriptionVn, DescriptionEn, KeywordVn, KeywordEn, RouteDataUrlVnId, RouteDataUrlEnId, , H1Vn, H1En, H2Vn, H2En, H3Vn, H3En, H4Vn, H4En, H5Vn, H5En, H6Vn, H6En, OgImageSrc,"
                                                            + "OgTitleVn, OgSite_nameVn, OgDescriptionVn, OgTitleEn, OgSite_nameEn, OgDescriptionEn, OgType, " 
                                                            + "AboutCompanyTitleVn, AboutCompanyTitleEn, AboutCompanyDescriptionVn, AboutCompanyDescriptionEn, WhyChooseTitleVn, WhyChooseTitleEn, GalleryTitleVn, GalleryTitleEn, RecruitmentTitleVn, RecruitmentTitleEn, RecruitmentPagingItem, MenuActiveId")] RecruitmentPageViewModel obj)
        {
            string title = Message.TITLE_ERROR;
            string message = Message.CONTENT_POSTDATA_DEFAUT_ERROR;
            string status = Default.Status_Error;
            try
            {
                if (ModelState.IsValid)
                {
                    RecruitmentPageManagementAdminConfig model = new RecruitmentPageManagementAdminConfig();

                    var para = paraService.GetByCode(model.Code);
                    if (para != null)
                        model = JsonConvert.DeserializeObject<RecruitmentPageManagementAdminConfig>(para.Content.ToString());
                    var routerVn = routeDataUrlService.GetByUrl(obj.SlugVn);
                    if (routerVn != null)
                        routerVn = routeDataUrlService.GetBy(obj.RouteDataUrlVnId ?? "");
                    if (routerVn != null)
                    {
                        routerVn.Title = obj.TitleVn;
                        routerVn.Url = !string.IsNullOrEmpty(obj.SlugVn) ? obj.SlugVn.ToLower().Trim() : "";
                        routerVn.Description = obj.DescriptionVn;
                        routerVn.Keyword = obj.KeywordVn;
                        routerVn.H1             = obj.H1Vn;
                        routerVn.H2             = obj.H2Vn;
                        routerVn.H3             = obj.H3Vn;
                        routerVn.H4             = obj.H4Vn;
                        routerVn.H5             = obj.H5Vn;
                        routerVn.H6             = obj.H6Vn;
                        routerVn.Controller = "Recruitment";
                        routerVn.Action = "Index";
                        routerVn.Area = "";
                        routerVn.IsLanguage = RouteDataUrl.RouteDataIsLanguage.Vn;
                        routerVn.IsType = RouteDataUrl.RouteDataIsType.Recruitment;
                        //OpenGraphMetaData
                        routerVn.OgTitle        = obj.OgTitleVn;
                        routerVn.OgSite_name    = obj.OgSite_nameVn;
                        routerVn.OgDescription  = obj.OgDescriptionVn;
                        routerVn.OgType         = obj.OgType;
                        routerVn.OgImageSrc = obj.OgImageSrc;
                        routeDataUrlService.Update(routerVn);
                    }
                    else
                    {
                        routerVn = new RouteDataUrl();
                        routerVn.Title = obj.TitleVn;
                        routerVn.Url = !string.IsNullOrEmpty(obj.SlugVn) ? obj.SlugVn.ToLower().Trim() : "";
                        routerVn.Description    = obj.DescriptionVn;
                        routerVn.Keyword        = obj.KeywordVn;
                        routerVn.H1             = obj.H1Vn;
                        routerVn.H2             = obj.H2Vn;
                        routerVn.H3             = obj.H3Vn;
                        routerVn.H4             = obj.H4Vn;
                        routerVn.H5             = obj.H5Vn;
                        routerVn.H6             = obj.H6Vn;
                        routerVn.Controller = "Recruitment";
                        routerVn.Action = "Index";
                        routerVn.Area = "";
                        routerVn.IsLanguage = RouteDataUrl.RouteDataIsLanguage.Vn;
                        routerVn.IsType = RouteDataUrl.RouteDataIsType.Recruitment;
                        //OpenGraphMetaData
                        routerVn.OgTitle        = obj.OgTitleVn;
                        routerVn.OgSite_name    = obj.OgSite_nameVn;
                        routerVn.OgDescription  = obj.OgDescriptionVn;
                        routerVn.OgType         = obj.OgType;
                        routerVn.OgImageSrc = obj.OgImageSrc;
                        model.RouteDataUrlVnId = routeDataUrlService.Create(routerVn);
                    }

                    var routerEn = routeDataUrlService.GetByUrl(obj.SlugEn);
                    if (routerEn != null)
                        routerEn = routeDataUrlService.GetBy(obj.RouteDataUrlEnId ?? "");
                    if (routerEn != null)
                    {
                        routerEn.Title = obj.TitleEn;
                        routerEn.Url = !string.IsNullOrEmpty(obj.SlugEn) ? obj.SlugEn.ToLower().Trim() : "";
                        routerEn.Description    = obj.DescriptionEn;
                        routerEn.Keyword        = obj.KeywordEn;
                        routerEn.H1             = obj.H1En;
                        routerEn.H2             = obj.H2En;
                        routerEn.H3             = obj.H3En;
                        routerEn.H4             = obj.H4En;
                        routerEn.H5             = obj.H5En;
                        routerEn.H6             = obj.H6En;
                        routerEn.Controller = "Recruitment";
                        routerEn.Action = "Index";
                        routerEn.Area = "";
                        routerEn.IsLanguage = RouteDataUrl.RouteDataIsLanguage.En;
                        routerEn.IsType = RouteDataUrl.RouteDataIsType.Recruitment;
                        //OpenGraphMetaData
                        routerEn.OgTitle        = obj.OgTitleEn;
                        routerEn.OgSite_name    = obj.OgSite_nameEn;
                        routerEn.OgDescription  = obj.OgDescriptionEn;
                        routerEn.OgType         = obj.OgType;
                        routerEn.OgImageSrc = obj.OgImageSrc;
                        routeDataUrlService.Update(routerEn);
                    }
                    else
                    {
                        routerEn = new RouteDataUrl();
                        routerEn.Title = obj.TitleEn;
                        routerEn.Url = !string.IsNullOrEmpty(obj.SlugEn) ? obj.SlugEn.ToLower().Trim() : "";
                        routerEn.Description    = obj.DescriptionEn;
                        routerEn.Keyword        = obj.KeywordEn;
                        routerEn.H1             = obj.H1En;
                        routerEn.H2             = obj.H2En;
                        routerEn.H3             = obj.H3En;
                        routerEn.H4             = obj.H4En;
                        routerEn.H5             = obj.H5En;
                        routerEn.H6             = obj.H6En;
                        routerEn.Controller = "Recruitment";
                        routerEn.Action = "Index";
                        routerEn.Area = "";
                        routerEn.IsLanguage = RouteDataUrl.RouteDataIsLanguage.En;
                        routerEn.IsType = RouteDataUrl.RouteDataIsType.Recruitment;
                        //OpenGraphMetaData
                        routerEn.OgTitle        = obj.OgTitleEn;
                        routerEn.OgSite_name    = obj.OgSite_nameEn;
                        routerEn.OgDescription  = obj.OgDescriptionEn;
                        routerEn.OgType         = obj.OgType;
                        routerEn.OgImageSrc = obj.OgImageSrc;
                        model.RouteDataUrlEnId = routeDataUrlService.Create(routerEn);
                    }

                    model.NameVn                    = obj.NameVn;
                    model.NameEn                    = obj.NameEn;
                    model.AboutCompanyTitleVn       = obj.AboutCompanyTitleVn;
                    model.AboutCompanyTitleEn       = obj.AboutCompanyTitleEn;
                    model.AboutCompanyDescriptionVn = obj.AboutCompanyDescriptionVn;
                    model.AboutCompanyDescriptionEn = obj.AboutCompanyDescriptionEn;
                    model.WhyChooseTitleVn          = obj.WhyChooseTitleVn;
                    model.WhyChooseTitleEn          = obj.WhyChooseTitleEn;
                    model.GalleryTitleVn            = obj.GalleryTitleVn;
                    model.GalleryTitleEn            = obj.GalleryTitleEn;
                    model.RecruitmentTitleVn        = obj.RecruitmentTitleVn;
                    model.RecruitmentTitleEn        = obj.RecruitmentTitleEn;
                    model.RecruitmentPagingItem     = obj.RecruitmentPagingItem;
                    model.MenuActiveId = obj.MenuActiveId;

                    if (para != null)
                    {
                        para.Content = JsonConvert.SerializeObject(model);
                        //model.EditedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
                        para.EditedByDate = DateTime.Now;
                        paraService.Update(para);

                        title   = Message.TITLE_REPORT;
                        message = Message.CONTENT_POSTDATA_UPDATE_SUCCESSFULL;
                        status  = Default.Status_Sucessfull;
                    }
                    else
                    {
                        para = new Parameter();
                        para.Code = model.Code;
                        para.Type = ParameterType.PageManagement;
                        para.Name = "";
                        para.Content = JsonConvert.SerializeObject(model);
                        //model.AddedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
                        para.AddedByDate = DateTime.Now;
                        para.IsDeleted = false;
                        paraService.Create(para);

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
        public JsonResult IsUrlVnIdAvailable(string SlugVn, string RouteDataUrlVnId)
        {
            // Check if the NameEn already exists
            return Json(routeDataUrlService.IsUrlAvailable(SlugVn, RouteDataUrlVnId) != null ? false : true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult IsUrlEnIdAvailable(string SlugEn, string RouteDataUrlEnId)
        {
            // Check if the NameEn already exists
            return Json(routeDataUrlService.IsUrlAvailable(SlugEn, RouteDataUrlEnId) != null ? false : true, JsonRequestBehavior.AllowGet);
        }
    }
}