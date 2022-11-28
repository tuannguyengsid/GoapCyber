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
    public class CategoryPageController : BaseAuthenticationController
    {
        private readonly IParameterService paraService;
        private readonly IMenuNodeService menuNodeService;
        string pathForSaving;
        string pathForUrl;
        // GET: CategoryPage
        public CategoryPageController(IParameterService _paraService,
            IMenuNodeService _menuNodeService)
        {
            paraService = _paraService;
            menuNodeService = _menuNodeService;
        }

        // GET: PageManagement/CategoryPage
        public ActionResult Index()
        {
            CategoryPageViewModel model = new CategoryPageViewModel();

            var paraConfig = new CategoryPageManagementAdminConfig();
            var para = paraService.GetByCode(paraConfig.Code);
            if (para != null)
            {
                paraConfig = JsonConvert.DeserializeObject<CategoryPageManagementAdminConfig>(para.Content.ToString());
                model = Mapper.Map<CategoryPageManagementAdminConfig, CategoryPageViewModel>(paraConfig);
            }
            model.MenuNodes = menuNodeService.GetAll(false);
            return View(model);
        }

        [HttpPost]
        public ActionResult EditPage(HttpPostedFileBase Backgroundfile, [Bind(Include = "NameVn, NameEn, TitleVn, TitleEn, BackgroundImageChange, MenuActiveId")]CategoryPageViewModel obj)
        {
            string title = Message.TITLE_ERROR;
            string message = Message.CONTENT_POSTDATA_DEFAUT_ERROR;
            string status = Default.Status_Error;
            try
            {
                if (ModelState.IsValid)
                {
                    CategoryPageManagementAdminConfig model = new CategoryPageManagementAdminConfig();

                    var para = paraService.GetByCode(model.Code);
                    if (para != null)
                    {
                        model = JsonConvert.DeserializeObject<CategoryPageManagementAdminConfig>(para.Content.ToString());
                        if (obj.BackgroundImageChange == "2" && !string.IsNullOrEmpty(model.BackgroundSrc))
                        {
                            try
                            {
                                string filepath = string.Format(@"{0}\{1}", GSIDSessionSiteInformation.PathAddressSiteMultimedia, model.BackgroundSrc.Replace("/", "\\"));
                                if (System.IO.File.Exists(filepath))
                                    System.IO.File.Delete(filepath);
                            }
                            catch (Exception)
                            {
                                message = "Không tìm thấy tập tin để xóa";
                            }
                            model.BackgroundSrc = string.Empty;
                        }
                    }

                    model.NameVn = obj.NameVn;
                    model.NameEn = obj.NameEn;
                    model.TitleVn = obj.TitleVn;
                    model.TitleEn = obj.TitleEn;
                    model.MenuActiveId = obj.MenuActiveId;
                    
                    if (Backgroundfile != null && Backgroundfile.ContentLength != 0)
                    {
                        var objPath = paraService.GetByCode((new SaveFilePathConfig()).Code);
                        if (objPath != null)
                        {
                            var paraPath = JsonConvert.DeserializeObject<SaveFilePathConfig>(objPath.Content.ToString());
                            pathForUrl = paraPath.ConfigPath;
                            pathForSaving = string.Format(@"{0}\{1}", GSIDSessionSiteInformation.PathAddressSiteMultimedia, paraPath.ConfigPath);
                        }
                        var allowedExtensions = new[] { ".jpg", ".png", ".jpeg", ".gif" };
                        var extension = Path.GetExtension(Backgroundfile.FileName).ToLower();

                        if (this.CreateFolderIfNeeded(pathForSaving)
                                    && allowedExtensions.Contains(extension))
                        {
                            try
                            {
                                string filename = string.Format("FaQ_{0}{1}", Guid.NewGuid().ToString().Replace("-", ""), extension);
                                Backgroundfile.SaveAs(Path.Combine(pathForSaving, filename));

                                model.BackgroundSrc = string.Format("{0}/{1}", pathForUrl.Replace(@"\", "/"), filename);
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