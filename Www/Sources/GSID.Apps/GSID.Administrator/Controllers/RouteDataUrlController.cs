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
    public class RouteDataUrlController :BaseAuthenticationController
    {
        // GET: RouteDataUrl
        private readonly IRouteDataUrlService routeDataUrlService;

        public RouteDataUrlController(IRouteDataUrlService _routeDataUrlService)
        {
            routeDataUrlService = _routeDataUrlService;
        }

        #region Title
        [HttpPost]
        public JsonResult IsTitlePageVnAvailable(string TitleVn, string RouteDataUrlVnId, string TitleEn)
        {
            TitleVn = !string.IsNullOrEmpty(TitleVn) ? TitleVn.Trim().ToLower() : "";
            TitleEn = !string.IsNullOrEmpty(TitleEn) ? TitleEn.Trim().ToLower() : "";
            if (TitleVn == TitleEn)
                return Json(false, JsonRequestBehavior.AllowGet);
            else
                // Check if the NameEn already exists
                return Json(routeDataUrlService.IsTitleAvailable(TitleVn, RouteDataUrlVnId) != null ? false : true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult IsTitlePageEnAvailable(string TitleEn, string RouteDataUrlEnId, string TitleVn)
        {
            TitleVn = !string.IsNullOrEmpty(TitleVn) ? TitleVn.Trim().ToLower() : "";
            TitleEn = !string.IsNullOrEmpty(TitleEn) ? TitleEn.Trim().ToLower() : "";
            if (TitleVn == TitleEn)
                return Json(false, JsonRequestBehavior.AllowGet);
            else
                // Check if the NameEn already exists
                return Json(routeDataUrlService.IsTitleAvailable(TitleEn, RouteDataUrlEnId) != null ? false : true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult IsTitleVnAvailable(string TitleSEOVn, string RouteDataUrlVnId, string TitleSEOEn)
        {
            TitleSEOVn = !string.IsNullOrEmpty(TitleSEOVn) ? TitleSEOVn.Trim().ToLower() : "";
            TitleSEOEn = !string.IsNullOrEmpty(TitleSEOEn) ? TitleSEOEn.Trim().ToLower() : "";
            if (TitleSEOVn == TitleSEOEn)
                return Json(false, JsonRequestBehavior.AllowGet);
            else
                // Check if the NameEn already exists
                return Json(routeDataUrlService.IsTitleAvailable(TitleSEOVn, RouteDataUrlVnId) != null ? false : true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult IsTitleEnAvailable(string TitleSEOEn, string RouteDataUrlEnId, string TitleSEOVn)
        {
            TitleSEOVn = !string.IsNullOrEmpty(TitleSEOVn) ? TitleSEOVn.Trim().ToLower() : "";
            TitleSEOEn = !string.IsNullOrEmpty(TitleSEOEn) ? TitleSEOEn.Trim().ToLower() : "";
            if (TitleSEOVn == TitleSEOEn)
                return Json(false, JsonRequestBehavior.AllowGet);
            else
                // Check if the NameEn already exists
                return Json(routeDataUrlService.IsTitleAvailable(TitleSEOEn, RouteDataUrlEnId) != null ? false : true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Keyword

        [HttpPost]
        public JsonResult IsKeywordPageVnAvailable(string KeywordSEOVn, string RouteDataUrlVnId, string KeywordSEOEn)
        {
            KeywordSEOVn = !string.IsNullOrEmpty(KeywordSEOVn) ? KeywordSEOVn.Trim().ToLower() : "";
            KeywordSEOEn = !string.IsNullOrEmpty(KeywordSEOEn) ? KeywordSEOEn.Trim().ToLower() : "";
            if (KeywordSEOVn == KeywordSEOEn)
                return Json(false, JsonRequestBehavior.AllowGet);
            else
                // Check if the NameEn already exists
                return Json(routeDataUrlService.IsKeywordAvailable(KeywordSEOVn, RouteDataUrlVnId) != null ? false : true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult IsKeywordPageEnAvailable(string KeywordSEOEn, string RouteDataUrlEnId, string KeywordSEOVn)
        {
            KeywordSEOVn = !string.IsNullOrEmpty(KeywordSEOVn) ? KeywordSEOVn.Trim().ToLower() : "";
            KeywordSEOEn = !string.IsNullOrEmpty(KeywordSEOEn) ? KeywordSEOEn.Trim().ToLower() : "";
            if (KeywordSEOVn == KeywordSEOEn)
                return Json(false, JsonRequestBehavior.AllowGet);
            else
                // Check if the NameEn already exists
                return Json(routeDataUrlService.IsKeywordAvailable(KeywordSEOEn, RouteDataUrlEnId) != null ? false : true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult IsKeywordVnAvailable(string KeywordSEOVn, string RouteDataUrlVnId, string KeywordSEOEn)
        {
            KeywordSEOVn = !string.IsNullOrEmpty(KeywordSEOVn) ? KeywordSEOVn.Trim().ToLower() : "";
            KeywordSEOEn = !string.IsNullOrEmpty(KeywordSEOEn) ? KeywordSEOEn.Trim().ToLower() : "";
            if (KeywordSEOVn == KeywordSEOEn)
                return Json(false, JsonRequestBehavior.AllowGet);
            else
                // Check if the NameEn already exists
                return Json(routeDataUrlService.IsKeywordAvailable(KeywordSEOVn, RouteDataUrlVnId) != null ? false : true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult IsKeywordEnAvailable(string KeywordSEOEn, string RouteDataUrlEnId, string KeywordSEOVn)
        {
            KeywordSEOVn = !string.IsNullOrEmpty(KeywordSEOVn) ? KeywordSEOVn.Trim().ToLower() : "";
            KeywordSEOEn = !string.IsNullOrEmpty(KeywordSEOEn) ? KeywordSEOEn.Trim().ToLower() : "";
            if (KeywordSEOVn == KeywordSEOEn)
                return Json(false, JsonRequestBehavior.AllowGet);
            else
                // Check if the NameEn already exists
                return Json(routeDataUrlService.IsKeywordAvailable(KeywordSEOEn, RouteDataUrlEnId) != null ? false : true, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Description
        [HttpPost]
        public JsonResult IsDescriptionPageVnAvailable(string DescriptionVn, string RouteDataUrlVnId, string DescriptionEn)
        {
            DescriptionVn = !string.IsNullOrEmpty(DescriptionVn) ? DescriptionVn.Trim().ToLower() : "";
            DescriptionEn = !string.IsNullOrEmpty(DescriptionEn) ? DescriptionEn.Trim().ToLower() : "";
            if (DescriptionVn == DescriptionEn)
                return Json(false, JsonRequestBehavior.AllowGet);
            else
                // Check if the NameEn already exists
                return Json(routeDataUrlService.IsDescriptionAvailable(DescriptionVn, RouteDataUrlVnId) != null ? false : true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult IsDescriptionPageEnAvailable(string DescriptionEn, string RouteDataUrlEnId, string DescriptionVn)
        {
            DescriptionVn = !string.IsNullOrEmpty(DescriptionVn) ? DescriptionVn.Trim().ToLower() : "";
            DescriptionEn = !string.IsNullOrEmpty(DescriptionEn) ? DescriptionEn.Trim().ToLower() : "";
            if (DescriptionVn == DescriptionEn)
                return Json(false, JsonRequestBehavior.AllowGet);
            else
                // Check if the NameEn already exists
                return Json(routeDataUrlService.IsDescriptionAvailable(DescriptionEn, RouteDataUrlEnId) != null ? false : true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult IsDescriptionVnAvailable(string DescriptionSEOVn, string RouteDataUrlVnId, string DescriptionSEOEn)
        {
            DescriptionSEOVn = !string.IsNullOrEmpty(DescriptionSEOVn) ? DescriptionSEOVn.Trim().ToLower() : "";
            DescriptionSEOEn = !string.IsNullOrEmpty(DescriptionSEOEn) ? DescriptionSEOEn.Trim().ToLower() : "";
            if (DescriptionSEOVn == DescriptionSEOEn)
                return Json(false, JsonRequestBehavior.AllowGet);
            else
                // Check if the NameEn already exists
                return Json(routeDataUrlService.IsDescriptionAvailable(DescriptionSEOVn, RouteDataUrlVnId) != null ? false : true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult IsDescriptionEnAvailable(string DescriptionSEOEn, string RouteDataUrlEnId, string DescriptionSEOVn)
        {
            DescriptionSEOVn = !string.IsNullOrEmpty(DescriptionSEOVn) ? DescriptionSEOVn.Trim().ToLower() : "";
            DescriptionSEOEn = !string.IsNullOrEmpty(DescriptionSEOEn) ? DescriptionSEOEn.Trim().ToLower() : "";
            if (DescriptionSEOVn == DescriptionSEOEn)
                return Json(false, JsonRequestBehavior.AllowGet);
            else
                // Check if the NameEn already exists
                return Json(routeDataUrlService.IsDescriptionAvailable(DescriptionSEOEn, RouteDataUrlEnId) != null ? false : true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Url
        [HttpPost]
        public JsonResult IsUrlVnIdAvailable(string SlugSEOVn, string RouteDataUrlVnId, string SlugSEOEn, string TitleSEOVn, bool IsUrlRequiredVn)
        {
            SlugSEOVn = !string.IsNullOrEmpty(SlugSEOVn) ? SlugSEOVn.Trim().ToLower() : "";
            SlugSEOEn = !string.IsNullOrEmpty(SlugSEOEn) ? SlugSEOEn.Trim().ToLower() : "";

            if (IsUrlRequiredVn && string.IsNullOrEmpty(SlugSEOVn))
                return Json(false, JsonRequestBehavior.AllowGet);
            else if (SlugSEOVn == SlugSEOEn || (!string.IsNullOrEmpty(TitleSEOVn) && string.IsNullOrEmpty(SlugSEOVn)))
                return Json(false, JsonRequestBehavior.AllowGet);
            else
                return Json(routeDataUrlService.IsUrlAvailable(SlugSEOVn, RouteDataUrlVnId) != null ? false : true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult IsUrlEnIdAvailable(string SlugSEOEn, string RouteDataUrlEnId, string SlugSEOVn, string TitleSEOEn, bool IsUrlRequiredEn)
        {
            SlugSEOVn = !string.IsNullOrEmpty(SlugSEOVn) ? SlugSEOVn.Trim().ToLower() : "";
            SlugSEOEn = !string.IsNullOrEmpty(SlugSEOEn) ? SlugSEOEn.Trim().ToLower() : "";

            if (IsUrlRequiredEn && string.IsNullOrEmpty(SlugSEOEn))
                return Json(false, JsonRequestBehavior.AllowGet);
            else if (SlugSEOVn == SlugSEOEn || (!string.IsNullOrEmpty(TitleSEOEn) && string.IsNullOrEmpty(SlugSEOEn)))
                return Json(false, JsonRequestBehavior.AllowGet);
            else
                return Json(routeDataUrlService.IsUrlAvailable(SlugSEOEn, RouteDataUrlEnId) != null ? false : true, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}