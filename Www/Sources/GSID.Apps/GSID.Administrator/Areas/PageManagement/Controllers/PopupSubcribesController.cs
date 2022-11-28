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
    public class PopupSubcribesController : BaseAuthenticationController
    {
        private readonly IParameterService paraService;
        // GET: User
        public PopupSubcribesController(IParameterService _paraService)
        {
            paraService = _paraService;
        }

        public ActionResult Index()
        {
            PopupSubcribesViewModel model = new PopupSubcribesViewModel();
            PopupSubcribesPageManagementAdminConfig paraConfig = new PopupSubcribesPageManagementAdminConfig();
            var para = paraService.GetByCode(new PopupSubcribesPageManagementAdminConfig().Code);
            if (para != null)
            {
                paraConfig = JsonConvert.DeserializeObject<PopupSubcribesPageManagementAdminConfig>(para.Content.ToString());
                model = Mapper.Map<PopupSubcribesPageManagementAdminConfig, PopupSubcribesViewModel>(paraConfig);
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult EditPage([Bind(Include = "NameVn, NameEn, DescriptionVn, DescriptionEn, IsActive, BackgroundVnSrc, BackgroundEnSrc")] PopupSubcribesViewModel obj)
        {
            string title = Message.TITLE_ERROR;
            string message = Message.CONTENT_POSTDATA_DEFAUT_ERROR;
            string status = Default.Status_Error;
            try
            {
                if (ModelState.IsValid)
                {
                    PopupSubcribesPageManagementAdminConfig model = new PopupSubcribesPageManagementAdminConfig();

                    var para = paraService.GetByCode(model.Code);
                    if (para != null)
                        model = JsonConvert.DeserializeObject<PopupSubcribesPageManagementAdminConfig>(para.Content.ToString());
                    model.BackgroundVnSrc = obj.BackgroundVnSrc;
                    model.BackgroundEnSrc = obj.BackgroundEnSrc;

                    model.NameVn = obj.NameVn;
                    model.NameEn = obj.NameEn;
                    model.DescriptionVn = obj.DescriptionVn;
                    model.DescriptionEn = obj.DescriptionEn;
                    //model.CookieWarningVn = obj.CookieWarningVn;
                    //model.CookieWarningEn = obj.CookieWarningEn;
                    model.IsActive = obj.IsActive;

                    if (para != null)
                    {
                        para.Content = JsonConvert.SerializeObject(model);
                        //model.EditedBy    = GSIDSessionFacade.GSIDSessionUserLogon.Id;
                        para.EditedByDate = DateTime.Now;
                        paraService.Update(para);

                        title = Message.TITLE_REPORT;
                        message = Message.CONTENT_POSTDATA_UPDATE_SUCCESSFULL;
                        status = Default.Status_Sucessfull;
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
                    message = string.Join(" | ", ModelState.Values
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
    }
}