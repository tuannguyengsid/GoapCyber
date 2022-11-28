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
        public ActionResult PartialListWhyChoose()
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

        public ActionResult PartialCreateWhyChoose()
        {
            CreateWhyChooseRecruitmentPageViewModel model = new CreateWhyChooseRecruitmentPageViewModel();
            model.Index = 1;
            return PartialView(model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult PartialCreateWhyChoose( [Bind(Include = "ShortDescriptionVn, ShortDescriptionEn, Index, ImageWhyChooseRecruitmentSrc")] CreateWhyChooseRecruitmentPageViewModel obj)
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

                    var objWhyChooseItem = new RecruitmentPageManagementAdminConfig.RecruitmentPageManagementSectionWhyChooseAdminConfig();

                    if (model.WhyChooseItems == null)
                        model.WhyChooseItems = new List<RecruitmentPageManagementAdminConfig.RecruitmentPageManagementSectionWhyChooseAdminConfig>();

                    objWhyChooseItem.Id = Guid.NewGuid();
                    objWhyChooseItem.Index = obj.Index;
                    objWhyChooseItem.ShortDescriptionVn = obj.ShortDescriptionVn;
                    objWhyChooseItem.ShortDescriptionEn = obj.ShortDescriptionEn;
                    objWhyChooseItem.ImageSrc = obj.ImageWhyChooseRecruitmentSrc;

                    model.WhyChooseItems.Add(objWhyChooseItem);

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

        public ActionResult PartialUpdateWhyChoose(Guid gsid)
        {
            EditWhyChooseRecruitmentPageViewModelViewModel model = new EditWhyChooseRecruitmentPageViewModelViewModel();
            RecruitmentPageManagementAdminConfig paraConfig = new RecruitmentPageManagementAdminConfig() { };
            var para = paraService.GetByCode(new RecruitmentPageManagementAdminConfig().Code);
            if (para != null)
            {
                paraConfig = JsonConvert.DeserializeObject<RecruitmentPageManagementAdminConfig>(para.Content.ToString());
                if (paraConfig != null
                        && paraConfig.WhyChooseItems != null
                        && paraConfig.WhyChooseItems.Count > 0)
                {

                    var obj = paraConfig.WhyChooseItems.Where(u => u.Id == gsid).FirstOrDefault();
                    if (obj != null)
                    {
                        model.Id = obj.Id;
                        model.ShortDescriptionVn = obj.ShortDescriptionVn;
                        model.ShortDescriptionEn = obj.ShortDescriptionEn;
                        model.ImageWhyChooseRecruitmentSrc = obj.ImageSrc;
                        model.Index = obj.Index;
                    }
                }
            }
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult PartialUpdateWhyChoose([Bind(Include = "Id, ShortDescriptionVn, ShortDescriptionEn, Index, ImageWhyChooseRecruitmentSrc")]EditWhyChooseRecruitmentPageViewModelViewModel obj)
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
                        if (model != null && model.WhyChooseItems != null && model.WhyChooseItems.Count > 0)
                        {
                            var objWhyChooseItem = model.WhyChooseItems.Where(i => i.Id == obj.Id).FirstOrDefault();
                            if (objWhyChooseItem != null)
                            {
                                model.WhyChooseItems.Where(i => i.Id == obj.Id)
                                                    .Select(S => {
                                                        S.ShortDescriptionVn = obj.ShortDescriptionVn;
                                                        S.ShortDescriptionEn = obj.ShortDescriptionEn;
                                                        S.ImageSrc = obj.ImageWhyChooseRecruitmentSrc;
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
        public ActionResult DeleteWhyChoose(Guid id)
        {
            string status = ((int)StatusDelete.Error).ToString();
            string message = Message.CONTENT_POSTDATA_DELETE_ERROR_OR_EMPTY;
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var model = paraService.GetByCode(new RecruitmentPageManagementAdminConfig().Code);
            if (model != null)
            {
                var config = JsonConvert.DeserializeObject<RecruitmentPageManagementAdminConfig>(model.Content.ToString());
                var _hasDelete = config.WhyChooseItems.FirstOrDefault(p => p.Id == id);
                if (_hasDelete != null)
                    config.WhyChooseItems.Remove(_hasDelete);

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