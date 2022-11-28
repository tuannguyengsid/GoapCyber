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
        public ActionResult PartialListAboutCompany()
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

        public ActionResult PartialCreateAboutCompany()
        {
            CreateAboutCompanyRecruitmentPageViewModel model = new CreateAboutCompanyRecruitmentPageViewModel();
            model.Index = 1;
            return PartialView(model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult PartialCreateAboutCompany( [Bind(Include = "Count, NameVn, NameEn, Index, ImageAboutCompanySrc")] CreateAboutCompanyRecruitmentPageViewModel obj)
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

                    var objAboutCompanyItem = new RecruitmentPageManagementAdminConfig.RecruitmentPageManagementSectionAboutCompanyAdminConfig();
                    if (model.AboutCompanyItems == null)
                        model.AboutCompanyItems = new List<RecruitmentPageManagementAdminConfig.RecruitmentPageManagementSectionAboutCompanyAdminConfig>();

                    objAboutCompanyItem.Id = Guid.NewGuid();
                    objAboutCompanyItem.Index = obj.Index;
                    objAboutCompanyItem.Count = obj.Count;
                    objAboutCompanyItem.NameVn = obj.NameVn;
                    objAboutCompanyItem.NameEn = obj.NameEn;
                    objAboutCompanyItem.ImageSrc = obj.ImageAboutCompanySrc;
                    model.AboutCompanyItems.Add(objAboutCompanyItem);

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

        public ActionResult PartialUpdateAboutCompany(Guid gsid)
        {
            EditAboutCompanyRecruitmentPageViewModel model = new EditAboutCompanyRecruitmentPageViewModel();
            RecruitmentPageManagementAdminConfig paraConfig = new RecruitmentPageManagementAdminConfig() { };
            var para = paraService.GetByCode(new RecruitmentPageManagementAdminConfig().Code);
            if (para != null)
            {
                paraConfig = JsonConvert.DeserializeObject<RecruitmentPageManagementAdminConfig>(para.Content.ToString());
                if (paraConfig != null
                        && paraConfig.AboutCompanyItems != null
                        && paraConfig.AboutCompanyItems.Count > 0)
                {

                    var obj = paraConfig.AboutCompanyItems.Where(u => u.Id == gsid).FirstOrDefault();
                    if (obj != null)
                    {
                        model.Id = obj.Id;
                        model.Count = obj.Count; 
                        model.NameVn = obj.NameVn; 
                        model.NameEn = obj.NameEn;
                        model.ImageAboutCompanySrc = obj.ImageSrc;
                        model.Index = obj.Index;
                    }
                }
            }
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult PartialUpdateAboutCompany( [Bind(Include = "Id, Count, NameVn, NameEn, Index, ImageAboutCompanySrc")] EditAboutCompanyRecruitmentPageViewModel obj)
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
                        if (model != null && model.AboutCompanyItems != null && model.AboutCompanyItems.Count > 0)
                        {
                            var objAboutCompanyItem = model.AboutCompanyItems.Where(i => i.Id == obj.Id).FirstOrDefault();
                            if (objAboutCompanyItem != null)
                            {
                                model.AboutCompanyItems.Where(i => i.Id == obj.Id)
                                                    .Select(S => {
                                                        S.Count     = obj.Count;
                                                        S.NameVn    = obj.NameVn;
                                                        S.NameEn    = obj.NameEn;
                                                        S.ImageSrc  = obj.ImageAboutCompanySrc;
                                                        S.Index     = obj.Index;
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
        public ActionResult DeleteAboutCompany(Guid id)
        {
            string status = ((int)StatusDelete.Error).ToString();
            string message = Message.CONTENT_POSTDATA_DELETE_ERROR_OR_EMPTY;
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var model = paraService.GetByCode(new RecruitmentPageManagementAdminConfig().Code);
            if (model != null)
            {
                var config = JsonConvert.DeserializeObject<RecruitmentPageManagementAdminConfig>(model.Content.ToString());
                var _hasDelete = config.AboutCompanyItems.FirstOrDefault(p => p.Id == id);
                if (_hasDelete != null)
                    config.AboutCompanyItems.Remove(_hasDelete);
            
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