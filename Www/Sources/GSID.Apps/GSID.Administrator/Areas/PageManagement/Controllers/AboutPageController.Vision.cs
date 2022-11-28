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
using GSID.Service.MongoRepositories.Service;
using System.IO;

namespace GSID.Admin.Areas.PageManagement.Controllers
{
    public partial class AboutPageController
    {
        public ActionResult PartialListVision()
        {
            AboutPageViewModel model = new AboutPageViewModel();

            AboutPageManagementAdminConfig paraConfig = new AboutPageManagementAdminConfig();
            var para = paraService.GetByCode(new AboutPageManagementAdminConfig().Code);
            if (para != null)
            {
                paraConfig = JsonConvert.DeserializeObject<AboutPageManagementAdminConfig>(para.Content.ToString());
                model = Mapper.Map<AboutPageManagementAdminConfig, AboutPageViewModel>(paraConfig);
                model.Visions = paraConfig.Visions;
            }

            return PartialView(model);
        }

        public ActionResult PartialCreateVision()
        {
            CreateSectionVisionAboutPageViewModel model = new CreateSectionVisionAboutPageViewModel();
            model.Index = 1;
            return PartialView(model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult PartialCreateVision([Bind(Include = "NameVn, NameEn, DescriptionVn, DescriptionEn, Index")] CreateSectionVisionAboutPageViewModel obj)
        {
            string title = Message.TITLE_ERROR;
            string message = Message.CONTENT_POSTDATA_DEFAUT_ERROR;
            string status = Default.Status_Error;
            string id = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    AboutPageManagementAdminConfig model = new AboutPageManagementAdminConfig();

                    var paraConfig = paraService.GetByCode(model.Code);
                    if (paraConfig != null)
                        model = JsonConvert.DeserializeObject<AboutPageManagementAdminConfig>(paraConfig.Content.ToString());

                    var vision = new AboutPageManagementAdminConfig.AboutPageManagementSectionVisionAdminConfig();

                    if (model.Visions == null)
                        model.Visions = new List<AboutPageManagementAdminConfig.AboutPageManagementSectionVisionAdminConfig>();

                    vision.Id = Guid.NewGuid();
                    vision.NameVn = obj.NameVn;
                    vision.NameEn = obj.NameEn;
                    vision.DescriptionVn = obj.DescriptionVn;
                    vision.DescriptionEn = obj.DescriptionEn;
                    vision.Index = obj.Index;
                    model.Visions.Add(vision);

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

        public ActionResult PartialUpdateVision(Guid gsid)
        {
            EditSectionVisionAboutPageViewModel model = new EditSectionVisionAboutPageViewModel();
            AboutPageManagementAdminConfig paraConfig = new AboutPageManagementAdminConfig() { };
            var para = paraService.GetByCode(new AboutPageManagementAdminConfig().Code);
            if (para != null)
            {
                paraConfig = JsonConvert.DeserializeObject<AboutPageManagementAdminConfig>(para.Content.ToString());
                if (paraConfig != null
                        && paraConfig.Visions != null
                        && paraConfig.Visions.Count > 0)
                {

                    var obj = paraConfig.Visions.Where(u => u.Id == gsid).FirstOrDefault();
                    if (obj != null)
                    {
                        model.Id = obj.Id;
                        model.NameVn = obj.NameVn;
                        model.NameEn = obj.NameEn;
                        model.DescriptionVn = obj.DescriptionVn;
                        model.DescriptionEn = obj.DescriptionEn;
                        model.Index = obj.Index;
                    }
                }
            }
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult PartialUpdateVision([Bind(Include = "Id, NameVn, NameEn, DescriptionVn, DescriptionEn, Index, ImageChange")] EditSectionVisionAboutPageViewModel obj)
        {
            string title = Message.TITLE_ERROR;
            string message = Message.CONTENT_POSTDATA_DEFAUT_ERROR;
            string status = Default.Status_Error;
            try
            {
                if (ModelState.IsValid)
                {
                    var paraConfig = paraService.GetByCode(new AboutPageManagementAdminConfig().Code);
                    if (paraConfig != null)
                    {
                        var model = JsonConvert.DeserializeObject<AboutPageManagementAdminConfig>(paraConfig.Content.ToString());
                        if (model != null && model.Visions != null && model.Visions.Count > 0)
                        {
                            var objVision = model.Visions.Where(i => i.Id == obj.Id).FirstOrDefault();
                            if (objVision != null)
                            {

                                model.Visions.Where(i => i.Id == obj.Id)
                                                    .Select(S => {
                                                        S.NameVn        = obj.NameVn;
                                                        S.NameEn        = obj.NameEn;
                                                        S.DescriptionVn = obj.DescriptionVn;
                                                        S.DescriptionEn = obj.DescriptionEn;
                                                        S.Index         = obj.Index;
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
        public ActionResult DeleteVision(Guid id)
        {
            string status = ((int)StatusDelete.Error).ToString();
            string message = Message.CONTENT_POSTDATA_DELETE_ERROR_OR_EMPTY;
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var model = paraService.GetByCode(new AboutPageManagementAdminConfig().Code);
            if (model != null)
            {
                var config = JsonConvert.DeserializeObject<AboutPageManagementAdminConfig>(model.Content.ToString());
                var _hasDelete = config.Visions.FirstOrDefault(p => p.Id == id);
                if (_hasDelete != null)
                {
                    config.Visions.Remove(_hasDelete);
                }

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