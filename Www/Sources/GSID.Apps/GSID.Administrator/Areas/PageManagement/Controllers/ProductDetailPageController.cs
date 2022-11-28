﻿using GSID.Admin.Areas.PageManagement.ViewModels;
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
    public class ProductDetailPageController : BaseAuthenticationController
    {
        private readonly IParameterService paraService;
        private readonly IMenuNodeService menuNodeService;
        // GET: User
        public ProductDetailPageController(IParameterService _paraService,
            IMenuNodeService _menuNodeService)
        {
            paraService = _paraService;
            menuNodeService = _menuNodeService;
        }

        public ActionResult Index()
        {
            ProductDetailPageViewModel model = new ProductDetailPageViewModel();
            ProductDetailPageManagementAdminConfig paraConfig = new ProductDetailPageManagementAdminConfig();
            var para = paraService.GetByCode(new ProductDetailPageManagementAdminConfig().Code);
            if (para != null)
            {
                paraConfig = JsonConvert.DeserializeObject<ProductDetailPageManagementAdminConfig>(para.Content.ToString());
                model = Mapper.Map<ProductDetailPageManagementAdminConfig, ProductDetailPageViewModel>(paraConfig);
            }
            model.MenuNodes = menuNodeService.GetAllParent("");

            return View(model);
        }

        [HttpPost]
        public ActionResult EditHomePage([Bind(Include = "MenuActiveId")] ProductDetailPageViewModel obj)
        {
            string title = Message.TITLE_ERROR;
            string message = Message.CONTENT_POSTDATA_DEFAUT_ERROR;
            string status = Default.Status_Error;
            try
            {
                if (ModelState.IsValid)
                {
                    ProductDetailPageManagementAdminConfig model = new ProductDetailPageManagementAdminConfig();

                    var para = paraService.GetByCode(model.Code);
                    if (para != null)
                        model = JsonConvert.DeserializeObject<ProductDetailPageManagementAdminConfig>(para.Content.ToString());
                   
                    model.MenuActiveId = obj.MenuActiveId;

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