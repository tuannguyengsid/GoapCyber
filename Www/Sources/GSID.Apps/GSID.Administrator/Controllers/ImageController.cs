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
using GSID.Model.ExtraEntities;
using Newtonsoft.Json;
using System.Web;
using System.Threading.Tasks;

namespace GSID.Admin.Controllers
{
    public class ImageController : BaseAuthenticationController
    {
        // GET: Image
        private readonly IImageLibraryService imgLibraryService;
        private readonly IImageService imgService;
        private readonly IParameterService paraService;
        // GET: Image
        public ImageController(IImageLibraryService _imgLibraryService,
                                    IImageService _imgService,
                                    IParameterService _paraService)
        {
            imgLibraryService = _imgLibraryService;
            imgService = _imgService;
            paraService = _paraService;
        }

        [RBAC]
        public ActionResult Index(string p)
        {
            var model = imgService.GetAll();

            PageIndex = p.ConvertIntPaging();
            ViewBag.TotalPage = (Math.Ceiling((double)model.Count / PageSize));
            ViewBag.CurrentPage = PageIndex;
            ViewBag.PageVisit = PageVisit;
            ViewBag.PageSize = PageSize;
            ViewBag.CountTotal = model.Count();
            model = model.Skip(PageSize * (PageIndex - 1))
                                    .Take(PageSize)
                                        .OrderByDescending(c => c.AddedByDate ?? DateTime.Now)
                                            .ToList();
            ViewBag.ActiveMenu = RBACUser.RootPermissionId(Request);
            return View(model);
        }

        [RBAC]
        public ActionResult Create()
        {
            ImageCreateViewModel model = new ImageCreateViewModel();
            model.ImageLibraries = imgLibraryService.GetAll();
            model.IsDeleted = true;
            ViewBag.ActiveMenu = RBACUser.RootPermissionId(Request);
            return View(model);
        }
        
        [HttpPost]
        [RBAC]
        public ActionResult Create([Bind(Include = "ImageLibraryIds, IsDeleted, ImagePaths")] ImageCreateViewModel obj)
        {
            string title = Message.TITLE_ERROR;
            string message = Message.CONTENT_POSTDATA_DEFAUT_ERROR;
            string status = Default.Status_Error;
            string id = string.Empty;
            try
            {
                if (!RBACUser.HasPermission("RecycleBin", "Image"))
                    ModelState.Remove("IsDeleted");
                if (ModelState.IsValid)
                {
                    var _imgPaths = !string.IsNullOrEmpty(obj.ImagePaths) ? obj.ImagePaths.Split(',').ToList() : null;
                    if (_imgPaths != null && _imgPaths.Count > 0)
                    {
                        foreach (var item in _imgPaths)
                        {
                            Image model = Mapper.Map<ImageCreateViewModel, Image>(obj);
                            model.ImageSrc = item;
                            model.ImageLibraryIds = obj.ImageLibraryIds;
                            model.IsDeleted = !RBACUser.HasPermission("RecycleBin", "Image") ? false : !obj.IsDeleted;
                            model.AddedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
                            id = imgService.Create(model);
                        }

                        title = Message.TITLE_REPORT;
                        message = Message.CONTENT_POSTDATA_CREATE_SUCCESSFULL;
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
                Id = id,
                Title = title,
                Message = message,
                Status = status
            }, JsonRequestBehavior.AllowGet);
        }

        [RBAC]
        public ActionResult Update(string gsid)
        {
            var model = Mapper.Map<Image, ImageEditViewModel>(imgService.GetBy(gsid));
            if (model == null)
                Response.Redirect(string.Format("/Error/NotFound?url={0}", Request.Url), true);

            model.ImageLibraries = imgLibraryService.GetAll();
            model.IsDeleted = !model.IsDeleted;
            ViewBag.ActiveMenu = RBACUser.RootPermissionId(Request);
            return View(model);
        }

        [HttpPost]
        [RBAC]
        public ActionResult Update([Bind(Include = "Id, ImageLibraryIds, IsDeleted, ImageSrc")] ImageEditViewModel obj)
        {
            string title = Message.TITLE_ERROR;
            string message = Message.CONTENT_POSTDATA_DEFAUT_ERROR;
            string status = Default.Status_Error;
            try
            {
                if (!RBACUser.HasPermission("RecycleBin", "Image"))
                    ModelState.Remove("IsDeleted");
                if (ModelState.IsValid)
                {
                    var model = imgService.GetBy(obj.Id);
                    if (model != null)
                    {
                        model.ImageSrc = obj.ImageSrc;
                        model.ImageLibraryIds = obj.ImageLibraryIds;
                        if (RBACUser.HasPermission("RecycleBin", "Image"))
                            model.IsDeleted = !obj.IsDeleted;
                        model.EditedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
                        imgService.Update(model);

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
        [RBAC]
        public ActionResult Delete(string id)
        {
            string status = ((int)StatusDelete.Error).ToString();
            string message = Message.CONTENT_POSTDATA_DELETE_ERROR_OR_EMPTY;
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var _hasDelete = imgService.Delete(id);
            if (_hasDelete)
                status = ((int)StatusDelete.Deleted).ToString();
            
            return Json(new
            {
                Status = status,
                Message = message
            });
        }

        [HttpPost]
        [RBAC]
        public ActionResult RecycleBin(string id, bool isDeleted)
        {
            string title = Message.TITLE_ERROR;
            string status = ((int)StatusDelete.Error).ToString();
            string message = Message.CONTENT_POSTDATA_DELETE_ERROR_OR_EMPTY;
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var _hasRecycleBin = imgService.GetBy(id);
            _hasRecycleBin.DeletedByDate = DateTime.Now;
            _hasRecycleBin.DeletedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
            _hasRecycleBin.IsDeleted = !isDeleted;
            imgService.Update(_hasRecycleBin);

            title = Message.TITLE_REPORT;
            message = Message.CONTENT_POSTDATA_UPDATE_SUCCESSFULL;
            status = Default.Status_Sucessfull;

            return Json(new
            {
                Title = title,
                Message = message,
                Status = status
            }, JsonRequestBehavior.AllowGet);
        }

    }
}