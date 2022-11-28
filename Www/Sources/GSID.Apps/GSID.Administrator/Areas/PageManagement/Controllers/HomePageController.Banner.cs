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
using System.IO;
using AutoMapper;

namespace GSID.Admin.Areas.PageManagement.Controllers
{
    public partial class HomePageController
    {
        public ActionResult PartialListBanner()
        {
            HomePageViewModel model = new HomePageViewModel();

            HomePageManagementAdminConfig paraConfig = new HomePageManagementAdminConfig();
            var para = paraService.GetByCode(new HomePageManagementAdminConfig().Code);
            if (para != null)
            {
                paraConfig = JsonConvert.DeserializeObject<HomePageManagementAdminConfig>(para.Content.ToString());
                model = Mapper.Map<HomePageManagementAdminConfig, HomePageViewModel>(paraConfig);
                model.Banners = paraConfig.Banners;
            }

            return PartialView(model);
        }

        public ActionResult PartialCreateBanner()
        {
            CreateBannerHomePageViewModel model = new CreateBannerHomePageViewModel();
            model.Index = 1;
            return PartialView(model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult PartialCreateBanner([Bind(Include = "NameVn, NameEn,DescriptionVn, DescriptionEn, Index, Type, Source, ImageBannerHomePageSrc")]CreateBannerHomePageViewModel obj)
        {
            string title = Message.TITLE_ERROR;
            string message = Message.CONTENT_POSTDATA_DEFAUT_ERROR;
            string status = Default.Status_Error;
            string id = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    HomePageManagementAdminConfig model = new HomePageManagementAdminConfig();

                    var paraConfig = paraService.GetByCode(model.Code);
                    if (paraConfig != null)
                        model = JsonConvert.DeserializeObject<HomePageManagementAdminConfig>(paraConfig.Content.ToString());

                    var banner = new HomePageManagementBannerAdminConfig();

                    if (model.Banners == null)
                        model.Banners = new List<HomePageManagementBannerAdminConfig>();

                    banner.Id = Guid.NewGuid();
                    banner.NameVn = obj.NameVn;
                    banner.NameEn = obj.NameEn;
                    banner.DescriptionVn = obj.DescriptionVn;
                    banner.DescriptionEn = obj.DescriptionEn;
                    banner.Type = obj.Type;
                    banner.Source = obj.Source;
                    banner.Index = obj.Index;
                    banner.ImageSrc = obj.ImageBannerHomePageSrc;
                    model.Banners.Add(banner);

                    if (paraConfig != null)
                    {
                        paraConfig.Content = JsonConvert.SerializeObject(model);
                        //model.EditedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
                        paraConfig.EditedByDate = DateTime.Now;
                        paraService.Update(paraConfig);

                        title = Message.TITLE_REPORT;
                        message = Message.CONTENT_POSTDATA_UPDATE_SUCCESSFULL;
                        status = Default.Status_Sucessfull;
                    }
                    else
                    {
                        paraConfig = new Parameter();
                        paraConfig.Code = model.Code;
                        paraConfig.Type = ParameterType.Const;
                        paraConfig.Name = "";
                        paraConfig.Content = JsonConvert.SerializeObject(model);
                        //model.AddedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
                        paraConfig.AddedByDate = DateTime.Now;
                        paraConfig.IsDeleted = false;
                        id = paraService.Create(paraConfig);

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

        public ActionResult PartialUpdateBanner(Guid gsid)
        {
            EditBannerHomePageViewModel model = new EditBannerHomePageViewModel();
            HomePageManagementAdminConfig paraConfig = new HomePageManagementAdminConfig() { };
            var para = paraService.GetByCode(new HomePageManagementAdminConfig().Code);
            if (para != null)
            {
                paraConfig = JsonConvert.DeserializeObject<HomePageManagementAdminConfig>(para.Content.ToString());
                if (paraConfig != null
                        && paraConfig.Banners != null
                        && paraConfig.Banners.Count > 0)
                {

                    var obj = paraConfig.Banners.Where(u => u.Id == gsid).FirstOrDefault();
                    if (obj != null)
                    {
                        model.Id = obj.Id;
                        model.NameVn = obj.NameVn;
                        model.NameEn = obj.NameEn;
                        model.DescriptionVn = obj.DescriptionVn;
                        model.DescriptionEn = obj.DescriptionEn;
                        model.Type = obj.Type;
                        model.Source = obj.Source;
                        model.ImageBannerHomePageSrc = obj.ImageSrc;
                        model.Index = obj.Index;
                    }
                }
            }
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult PartialUpdateBanner([Bind(Include = "Id, NameVn, NameEn, DescriptionVn, DescriptionEn, Index, ImageChange, Type, Source, ImageBannerHomePageSrc")]EditBannerHomePageViewModel obj)
        {
            string title = Message.TITLE_ERROR;
            string message = Message.CONTENT_POSTDATA_DEFAUT_ERROR;
            string status = Default.Status_Error;
            try
            {
                if (ModelState.IsValid)
                {
                    var paraConfig = paraService.GetByCode(new HomePageManagementAdminConfig().Code);
                    if (paraConfig != null)
                    {
                        var model = JsonConvert.DeserializeObject<HomePageManagementAdminConfig>(paraConfig.Content.ToString());
                        if (model != null && model.Banners != null && model.Banners.Count > 0)
                        {
                            var objBanner = model.Banners.Where(i => i.Id == obj.Id).FirstOrDefault();
                            model.Banners.Where(i => i.Id == obj.Id)
                                                    .Select(S => {
                                                        S.NameVn = obj.NameVn;
                                                        S.NameEn = obj.NameEn;
                                                        S.DescriptionVn = obj.DescriptionVn;
                                                        S.DescriptionEn = obj.DescriptionEn;
                                                        S.Type = obj.Type;
                                                        S.Source = obj.Source;
                                                        S.ImageSrc = obj.ImageBannerHomePageSrc;
                                                        S.Index = obj.Index;
                                                        return S;
                                                    }).ToList();
                            paraConfig.Content = JsonConvert.SerializeObject(model);
                            //model.EditedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
                            paraConfig.EditedByDate = DateTime.Now;
                            paraService.Update(paraConfig);

                            title = Message.TITLE_REPORT;
                            message = Message.CONTENT_POSTDATA_UPDATE_SUCCESSFULL;
                            status = Default.Status_Sucessfull;
                        }
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
        public ActionResult DeleteBanner(Guid id)
        {
            string status = ((int)StatusDelete.Error).ToString();
            string message = Message.CONTENT_POSTDATA_DELETE_ERROR_OR_EMPTY;
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var model = paraService.GetByCode(new HomePageManagementAdminConfig().Code);
            if (model != null)
            {
                var config = JsonConvert.DeserializeObject<HomePageManagementAdminConfig>(model.Content.ToString());
                var _hasDelete = config.Banners.FirstOrDefault(p => p.Id == id);
                if (_hasDelete != null)
                    config.Banners.Remove(_hasDelete);

                model.Content = JsonConvert.SerializeObject(config);
                //model.EditedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
                model.EditedByDate = DateTime.Now;
                paraService.Update(model);
                status = ((int)StatusDelete.Deleted).ToString();
            }

            return Json(new
            {
                Status = status,
                Message = message
            });
        }
    }
}