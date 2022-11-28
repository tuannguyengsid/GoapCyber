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
    public partial class RecruitmentPageController
    {
        public ActionResult PartialListSlider()
        {
            RecruitmentPageViewModel model = new RecruitmentPageViewModel();

            RecruitmentPageManagementAdminConfig paraConfig = new RecruitmentPageManagementAdminConfig();
            var para = paraService.GetByCode(new RecruitmentPageManagementAdminConfig().Code);
            if (para != null)
            {
                paraConfig = JsonConvert.DeserializeObject<RecruitmentPageManagementAdminConfig>(para.Content.ToString());
                model = Mapper.Map<RecruitmentPageManagementAdminConfig, RecruitmentPageViewModel>(paraConfig);
                model.Slider = paraConfig.Slider;
            }

            return PartialView(model);
        }

        public ActionResult PartialCreateSlider()
        {
            CreateSliderRecruitmentPageViewModel model = new CreateSliderRecruitmentPageViewModel();
            model.Index = 1;
            return PartialView(model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult PartialCreateSlider([Bind(Include = "Index, ImageSliderRecruitmentPageSrc")] CreateSliderRecruitmentPageViewModel obj)
        {
            string title = Message.TITLE_ERROR;
            string message = Message.CONTENT_POSTDATA_DEFAUT_ERROR;
            string status = Default.Status_Error;
            string id = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    RecruitmentPageManagementAdminConfig model = new RecruitmentPageManagementAdminConfig();

                    var paraConfig = paraService.GetByCode(model.Code);
                    if (paraConfig != null)
                        model = JsonConvert.DeserializeObject<RecruitmentPageManagementAdminConfig>(paraConfig.Content.ToString());

                    var slider = new RecruitmentPageManagementAdminConfig.RecruitmentPageManagementSectionSliderAdminConfig();

                    if (model.Slider == null)
                        model.Slider = new List<RecruitmentPageManagementAdminConfig.RecruitmentPageManagementSectionSliderAdminConfig>();

                    slider.Id = Guid.NewGuid();
                    slider.Index = obj.Index;

                    slider.ImageSrc = obj.ImageSliderRecruitmentPageSrc;
                    model.Slider.Add(slider);

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

        public ActionResult PartialUpdateSlider(Guid gsid)
        {
            EditSliderRecruitmentPageViewModelViewModel model = new EditSliderRecruitmentPageViewModelViewModel();
            RecruitmentPageManagementAdminConfig paraConfig = new RecruitmentPageManagementAdminConfig() { };
            var para = paraService.GetByCode(new RecruitmentPageManagementAdminConfig().Code);
            if (para != null)
            {
                paraConfig = JsonConvert.DeserializeObject<RecruitmentPageManagementAdminConfig>(para.Content.ToString());
                if (paraConfig != null
                        && paraConfig.Slider != null
                        && paraConfig.Slider.Count > 0)
                {

                    var obj = paraConfig.Slider.Where(u => u.Id == gsid).FirstOrDefault();
                    if (obj != null)
                    {
                        model.Id = obj.Id;
                        model.ImageSliderRecruitmentPageSrc = obj.ImageSrc;
                        model.Index = obj.Index;
                    }
                }
            }
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult PartialUpdateSlider(HttpPostedFileBase file, [Bind(Include = "Id, Index, ImageSliderRecruitmentPageSrc")] EditSliderRecruitmentPageViewModelViewModel obj)
        {
            string title = Message.TITLE_ERROR;
            string message = Message.CONTENT_POSTDATA_DEFAUT_ERROR;
            string status = Default.Status_Error;
            try
            {
                if (ModelState.IsValid)
                {
                    var paraConfig = paraService.GetByCode(new RecruitmentPageManagementAdminConfig().Code);
                    if (paraConfig != null)
                    {
                        var model = JsonConvert.DeserializeObject<RecruitmentPageManagementAdminConfig>(paraConfig.Content.ToString());
                        if (model != null && model.Slider != null && model.Slider.Count > 0)
                        {
                            var objSlider = model.Slider.Where(i => i.Id == obj.Id).FirstOrDefault();
                            if (objSlider != null)
                            {
                                model.Slider.Where(i => i.Id == obj.Id).Select(S => {
                                                                            S.ImageSrc = obj.ImageSliderRecruitmentPageSrc;
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
        public ActionResult DeleteSlider(Guid id)
        {
            string status = ((int)StatusDelete.Error).ToString();
            string message = Message.CONTENT_POSTDATA_DELETE_ERROR_OR_EMPTY;
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var model = paraService.GetByCode(new RecruitmentPageManagementAdminConfig().Code);
            if (model != null)
            {
                var config = JsonConvert.DeserializeObject<RecruitmentPageManagementAdminConfig>(model.Content.ToString());
                var _hasDelete = config.Slider.FirstOrDefault(p => p.Id == id);
                if (_hasDelete != null)
                    config.Slider.Remove(_hasDelete);

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