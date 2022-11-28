using AutoMapper;
using GSID.Model.Enums;
using GSID.Model.MongodbModels;
using GSID.Service;
using GSID.Setting;
using GSID.Setting.FormKeys;
using GSID.Web.Core.Authentication;
using GSID.Web.Core.Extensions;
using GSID.Admin.Helpers;
using GSID.Admin.ViewModels.MongoModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using GSID.Service.MongoRepositories.Service;
using System.Threading.Tasks;
using Newtonsoft.Json;
using GSID.Model.ExtraEntities;
using System.Web;
using System.Globalization;

namespace GSID.Admin.Controllers
{
    public class ProductOverviewBlockController : BaseAuthenticationController
    {
        private readonly IProductService prdService;
        private readonly IProductCategoryService prdCateService;
        private readonly IProductOverviewBlockService prdOverviewBlockService;

        private readonly IParameterService paraService;
        // GET: ProductOverviewBlock
        public ProductOverviewBlockController(IProductService _prdService,
                                    IProductCategoryService _prdCateService,
                                    IParameterService _paraService,
                                    IProductOverviewBlockService _prdOverviewBlockService)
        {
            prdService = _prdService;
            paraService = _paraService;
            prdCateService = _prdCateService;
            prdOverviewBlockService = _prdOverviewBlockService;
        }


        public ActionResult PartialIndex(string prdId, ProductOverviewBlock.IsLanguage language)
        {
            var model = prdOverviewBlockService.GetAllByProduct(prdId, language);
            ViewBag.Language = (int)language;
            return PartialView(model);
        }

        public ActionResult PartialCreate(string prdId, ProductOverviewBlock.IsLanguage language)
        {
            ProductOverviewBlockCreateViewModel model = new ProductOverviewBlockCreateViewModel();
            model.ProductId = prdId;
            model.Language = language;
            return PartialView(model);
        }

        [HttpPost]
        public async Task<ActionResult> Create([Bind(Include = "ProductId, Name, Description, Sort, Language, IsDeleted, ImagePrdBlockSrc")] ProductOverviewBlockCreateViewModel obj)
        {
            string title = Message.TITLE_ERROR;
            string message = Message.CONTENT_POSTDATA_DEFAUT_ERROR;
            string status = Default.Status_Error;
            string id = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    ProductOverviewBlock model = Mapper.Map<ProductOverviewBlockCreateViewModel, ProductOverviewBlock>(obj);

                    model.Name      = !string.IsNullOrEmpty(obj.Name) ? obj.Name.Trim() : "";
                    model.Sort      = obj.Sort;
                    model.ImageSrc  = obj.ImagePrdBlockSrc;
                    model.IsDeleted = !obj.IsDeleted;
                    model.AddedBy   = GSIDSessionFacade.GSIDSessionUserLogon.Id;
                    id = prdOverviewBlockService.Create(model);

                    title = Message.TITLE_REPORT;
                    message = Message.CONTENT_POSTDATA_CREATE_SUCCESSFULL;
                    status = Default.Status_Sucessfull;
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
                Id = id,
                Title = title,
                Message = message,
                Status = status
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PartialUpdate(string gsid)
        {
            var obj = prdOverviewBlockService.GetBy(gsid);
            if (obj == null)
                Response.Redirect(string.Format("/Error/NotFound?url={0}", Request.Url), true);

            var model = Mapper.Map<ProductOverviewBlock, ProductOverviewBlockEditViewModel>(obj);
            model.ImagePrdBlockSrc = obj.ImageSrc;
            model.IsDeleted = !model.IsDeleted;

            return PartialView(model);
        }

        [HttpPost]
        public async Task<ActionResult> Update([Bind(Include = "Id, ProductId, Name, Description, Sort, Language, IsDeleted, ImagePrdBlockSrc")] ProductOverviewBlockEditViewModel obj)
        {
            string title = Message.TITLE_ERROR;
            string message = Message.CONTENT_POSTDATA_DEFAUT_ERROR;
            string status = Default.Status_Error;
            try
            {
                if (ModelState.IsValid)
                {
                    var model = prdOverviewBlockService.GetBy(obj.Id);
                    if (model != null)
                    {
                        model.Name          = !string.IsNullOrEmpty(obj.Name) ? obj.Name.Trim() : "";
                        model.Description   = obj.Description;
                        model.Sort          = obj.Sort;
                        model.IsDeleted     = !obj.IsDeleted;
                        model.ImageSrc      = obj.ImagePrdBlockSrc;
                        model.EditedBy      = GSIDSessionFacade.GSIDSessionUserLogon.Id;
                        prdOverviewBlockService.Update(model);

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

        [HttpPost]
        public ActionResult Delete(string id)
        {
            string status = ((int)StatusDelete.Error).ToString();
            string message = Message.CONTENT_POSTDATA_DELETE_ERROR_OR_EMPTY;
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var _hasDelete = prdOverviewBlockService.Delete(id);
            if (_hasDelete)
                status = ((int)StatusDelete.Deleted).ToString();
            return Json(new
            {
                Status = status,
                Message = message
            });
        }
    }
}