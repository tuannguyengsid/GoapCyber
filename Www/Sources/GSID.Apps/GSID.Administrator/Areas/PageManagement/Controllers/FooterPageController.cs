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
    public partial class FooterPageController : BaseAuthenticationController
    {
        private readonly IParameterService paraService;
        string pathForSaving;
        string pathForUrl;
        public FooterPageController(IParameterService _paraService)
        {
            paraService = _paraService;
        }
        // GET: PageManagement/FooterPage
        public ActionResult Index()
        {
            FooterContentViewModel model = new FooterContentViewModel();

            var paraConfig = new FooterPageManagementAdminConfig();
            var para = paraService.GetByCode(paraConfig.Code);
            if (para != null)
            {
                paraConfig = JsonConvert.DeserializeObject<FooterPageManagementAdminConfig>(para.Content.ToString());
                model = Mapper.Map<FooterPageManagementAdminConfig, FooterContentViewModel>(paraConfig);
            }
            ViewBag.SiteInformation = GSIDSessionSiteInformation;
            return View(model);
        }

        [HttpPost]
        public ActionResult EditPage(HttpPostedFileBase file, [Bind(Include = "ContentVn, ContentEn, CookieMessageVn, CookieMessageEn, ImageChange")]FooterContentViewModel obj)
        {
            string title = Message.TITLE_ERROR;
            string message = Message.CONTENT_POSTDATA_DEFAUT_ERROR;
            string status = Default.Status_Error;
            try
            {
                if (ModelState.IsValid)
                {
                    FooterPageManagementAdminConfig model = new FooterPageManagementAdminConfig();

                    var para = paraService.GetByCode(model.Code);
                    if (para != null)
                    {
                        model = JsonConvert.DeserializeObject<FooterPageManagementAdminConfig>(para.Content.ToString());
                        if (obj.ImageChange == "2" && !string.IsNullOrEmpty(model.ImageSrc))
                        {
                            try
                            {
                                if (!string.IsNullOrEmpty(model.ImageSrc))
                                {
                                    string filepath = string.Format(@"{0}\{1}", GSIDSessionSiteInformation.PathAddressSiteMultimedia, model.ImageSrc.Replace("/", "\\"));
                                    if (System.IO.File.Exists(filepath))
                                        System.IO.File.Delete(filepath);
                                }
                            }
                            catch (Exception)
                            {
                                message = "Không tìm thấy tập tin để xóa";
                            }
                            model.ImageSrc = string.Empty;
                        }
                    }

                    model.ContentVn = obj.ContentVn;
                    model.ContentEn = obj.ContentEn;
                    model.CookieMessageVn = obj.CookieMessageVn;
                    model.CookieMessageEn = obj.CookieMessageEn;

                    if (file != null && file.ContentLength != 0)
                    {
                        var objPath = paraService.GetByCode((new SaveFilePathConfig()).Code);
                        if (objPath != null)
                        {
                            var paraPath = JsonConvert.DeserializeObject<SaveFilePathConfig>(objPath.Content.ToString());
                            pathForUrl = paraPath.ConfigPath;
                            pathForSaving = string.Format(@"{0}\{1}", GSIDSessionSiteInformation.PathAddressSiteMultimedia, paraPath.ConfigPath);
                        }
                        var allowedExtensions = new[] { ".jpg", ".png", ".jpeg", ".gif" };
                        var extension = Path.GetExtension(file.FileName).ToLower();

                        if (this.CreateFolderIfNeeded(pathForSaving)
                                    && allowedExtensions.Contains(extension))
                        {
                            try
                            {
                                string filename = string.Format("FaQ_{0}{1}", Guid.NewGuid().ToString().Replace("-", ""), extension);
                                file.SaveAs(Path.Combine(pathForSaving, filename));

                                model.ImageSrc = string.Format("{0}/{1}", pathForUrl.Replace(@"\", "/"), filename);
                            }
                            catch
                            {
                            }
                        }
                    }

                    if (para != null)
                    {
                        para.Content = JsonConvert.SerializeObject(model);
                        //model.EditedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
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
    }
}