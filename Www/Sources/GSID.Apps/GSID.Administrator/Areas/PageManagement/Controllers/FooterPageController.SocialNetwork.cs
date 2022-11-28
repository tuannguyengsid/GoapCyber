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
using GSID.Admin.Helpers;

namespace GSID.Admin.Areas.PageManagement.Controllers
{
    public partial class FooterPageController
    {
        //[RBAC]
        public ActionResult PartialListSocial()
        {
            SocialNetworkManagementAdminConfig model = new SocialNetworkManagementAdminConfig();
            var para = paraService.GetByCode(new SocialNetworkManagementAdminConfig().Code);
            if (para != null)
                model = JsonConvert.DeserializeObject<SocialNetworkManagementAdminConfig>(para.Content.ToString());

            ViewBag.SiteInformation = GSIDSessionSiteInformation;

            return PartialView(model);
        }

        //[RBAC]
        public ActionResult PartialCreateSocial()
        {
            SocialNetworkConfigCreateViewModel model = new SocialNetworkConfigCreateViewModel();
            model.IsDeleted = true;
            model.Sort = 1;
            return PartialView(model);
        }

        [HttpPost]
        //[RBAC]
        public ActionResult PartialCreateSocial([Bind(Include = "Slug, Group, Sort, IsDeleted")]SocialNetworkConfigCreateViewModel obj)
        {
            string title = Message.TITLE_ERROR;
            string message = Message.CONTENT_POSTDATA_DEFAUT_ERROR;
            string status = Default.Status_Error;
            string id = string.Empty;
            try
            {
                if (ModelState.IsValid )
                {
                    SocialNetworkManagementAdminConfig model = new SocialNetworkManagementAdminConfig();

                    var paraConfig = paraService.GetByCode(model.Code);
                    if (paraConfig != null)
                        model = JsonConvert.DeserializeObject<SocialNetworkManagementAdminConfig>(paraConfig.Content.ToString());

                    if (model.Social == null)
                        model.Social = new List<SocialNetworkConfig>();
                    SocialNetworkConfig social = new SocialNetworkConfig();
                    social.Id           = Guid.NewGuid();
                    social.Slug         = obj.Slug;
                    social.Group = obj.Group;
                    social.IsRedirect = SocialNetworkConfig.SocialIsRedirect.Redirect;
                    social.Sort         = obj.Sort;
                    social.IsFooter     = true;
                    social.IsDeleted    = !obj.IsDeleted;
                    model.Social.Add(social);

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
                        paraConfig.AddedByDate = DateTime.Now;
                        paraConfig.IsDeleted = false;
                        id = paraService.Create(paraConfig);

                        title = Message.TITLE_REPORT;
                        message = Message.CONTENT_POSTDATA_UPDATE_SUCCESSFULL;
                        status = Default.Status_Sucessfull;
                    }

                    title = Message.TITLE_REPORT;
                    message = Message.CONTENT_POSTDATA_CREATE_SUCCESSFULL;
                    status = Default.Status_Sucessfull;
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
                Id = id,
                Title = title,
                Message = message,
                Status = status
            }, JsonRequestBehavior.AllowGet);
        }

        //[RBAC]
        public ActionResult PartialUpdateSocial(Guid gsid)
        {
            SocialNetworkConfigEditViewModel model = new SocialNetworkConfigEditViewModel();
            SocialNetworkManagementAdminConfig paraConfig = new SocialNetworkManagementAdminConfig() { };
            var para = paraService.GetByCode(new SocialNetworkManagementAdminConfig().Code);
            if (para != null)
            {
                paraConfig = JsonConvert.DeserializeObject<SocialNetworkManagementAdminConfig>(para.Content.ToString());
                if (paraConfig != null
                        && paraConfig.Social != null
                        && paraConfig.Social.Count > 0) {

                    var obj = paraConfig.Social.Where(u => u.Id == gsid).FirstOrDefault();
                    if (obj != null)
                    {
                        model.Id        = obj.Id;
                        model.Slug      = obj.Slug;
                        model.Group     = obj.Group;
                        //model.IsRedirect = SocialNetworkConfig.SocialIsRedirect.Redirect;
                        model.Sort              = obj.Sort;
                        //model.IsFooter          = obj.IsFooter;
                        model.IsDeleted         = !obj.IsDeleted;
                    }
                }
            }
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult PartialUpdateSocial([Bind(Include = "Id, Slug, Group, Sort, IsDeleted")]SocialNetworkConfigEditViewModel obj)
        {
            string title = Message.TITLE_ERROR;
            string message = Message.CONTENT_POSTDATA_DEFAUT_ERROR;
            string status = Default.Status_Error;
            try
            {
                if (ModelState.IsValid)
                {
                    var paraConfig = paraService.GetByCode(new SocialNetworkManagementAdminConfig().Code);
                    if (paraConfig != null)
                    {
                        var model = JsonConvert.DeserializeObject<SocialNetworkManagementAdminConfig>(paraConfig.Content.ToString());
                        if (model != null && model.Social != null && model.Social.Count > 0)
                        {
                            var objBanner = model.Social.Where(i => i.Id == obj.Id).FirstOrDefault();
                            if (objBanner != null)
                            {
                                model.Social.Where(i => i.Id == obj.Id)
                                                    .Select(S => {
                                                        S.Slug          = obj.Slug;
                                                        S.Group         = obj.Group;
                                                        S.IsRedirect    = SocialNetworkConfig.SocialIsRedirect.Redirect;
                                                        S.Sort          = obj.Sort;
                                                        S.IsFooter      = true;
                                                        S.IsDeleted     = !obj.IsDeleted;
                                                        return S;
                                                    }).ToList();
                                paraConfig.Content = JsonConvert.SerializeObject(model);
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
        //[RBAC]
        public ActionResult DeleteSocial(Guid id)
        {
            string status = ((int)StatusDelete.Error).ToString();
            string message = Message.CONTENT_POSTDATA_DELETE_ERROR_OR_EMPTY;
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var model = paraService.GetByCode(new SocialNetworkManagementAdminConfig().Code);
            if (model != null)
            {
                var config = JsonConvert.DeserializeObject<SocialNetworkManagementAdminConfig>(model.Content.ToString());
                var _hasDelete = config.Social.FirstOrDefault(p => p.Id == id);
                if (_hasDelete != null)
                    config.Social.Remove(_hasDelete);

                model.Content = JsonConvert.SerializeObject(config);
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