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
    public class ImageLibraryController : BaseAuthenticationController
    {
        // GET: ImageLibrary
        private readonly IImageLibraryService imgLibraryService;
        private readonly IImageService imgService;
        private readonly IParameterService paraService;
        // GET: ImageLibrary
        public ImageLibraryController(IImageLibraryService _imgLibraryService,
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
            var model = imgLibraryService.GetAll();

            PageIndex = p.ConvertIntPaging();
            ViewBag.TotalPage = (Math.Ceiling((double)model.Count / PageSize));
            ViewBag.CurrentPage = PageIndex;
            ViewBag.PageVisit = PageVisit;
            ViewBag.PageSize = PageSize;
            ViewBag.CountTotal = model.Count();
            model = model.Skip(PageSize * (PageIndex - 1))
                                    .Take(PageSize)
                                        .OrderBy(c => c.Sort)
                                            .ToList();
            ViewBag.ActiveMenu = RBACUser.RootPermissionId(Request);
            return View(model);
        }

        [RBAC]
        public ActionResult Create()
        {
            ImageLibraryCreateViewModel model = new ImageLibraryCreateViewModel();
            model.IsDeleted = true;
            ViewBag.ActiveMenu = RBACUser.RootPermissionId(Request);
            return View(model);
        }

        [HttpPost]
        [RBAC]
        public ActionResult Create([Bind(Include = "NameVn, NameEn, SlugVn, SlugEn, Sort, IsDeleted")] ImageLibraryCreateViewModel obj)
        {
            string title = Message.TITLE_ERROR;
            string message = Message.CONTENT_POSTDATA_DEFAUT_ERROR;
            string status = Default.Status_Error;
            string id = string.Empty;
            try
            {
                if (!RBACUser.HasPermission("RecycleBin", "ImageLibrary"))
                    ModelState.Remove("IsDeleted");
                if (ModelState.IsValid)
                {
                    ImageLibrary model = Mapper.Map<ImageLibraryCreateViewModel, ImageLibrary>(obj);
                    model.NameVn = !string.IsNullOrEmpty(obj.NameVn) ? obj.NameVn.Trim() : "";
                    model.NameEn = !string.IsNullOrEmpty(obj.NameEn) ? obj.NameEn.Trim() : "";
                    model.IsDeleted = !RBACUser.HasPermission("RecycleBin", "ImageLibrary") ? false : !obj.IsDeleted;
                    model.AddedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
                    id = imgLibraryService.Create(model);
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

        [RBAC]
        public ActionResult Update(string gsid)
        {
            var model = Mapper.Map<ImageLibrary, ImageLibraryEditViewModel>(imgLibraryService.GetBy(gsid));
            if (model == null)
                Response.Redirect(string.Format("/Error/NotFound?url={0}", Request.Url), true);

            //model.FullSlugVn = string.Format("/vn/tuyen-dung/{0}", model.SlugVn);
            //model.FullSlugEn = string.Format("/en/recruitment/{0}", model.SlugEn);

            model.IsDeleted = !model.IsDeleted;
            ViewBag.ActiveMenu = RBACUser.RootPermissionId(Request);
            return View(model);
        }

        [HttpPost]
        [RBAC]
        public ActionResult Update([Bind(Include = "Id, NameVn, NameEn, SlugVn, SlugEn, Sort, IsDeleted")] ImageLibraryEditViewModel obj)
        {
            string title = Message.TITLE_ERROR;
            string message = Message.CONTENT_POSTDATA_DEFAUT_ERROR;
            string status = Default.Status_Error;
            try
            {
                if (!RBACUser.HasPermission("RecycleBin", "ImageLibrary"))
                    ModelState.Remove("IsDeleted");
                if (ModelState.IsValid)
                {
                    var model = imgLibraryService.GetBy(obj.Id);
                    if (model != null)
                    {
                        model.NameVn = !string.IsNullOrEmpty(obj.NameVn) ? obj.NameVn.Trim() : "";
                        model.NameEn = !string.IsNullOrEmpty(obj.NameEn) ? obj.NameEn.Trim() : "";
                        model.SlugVn = obj.SlugVn;
                        model.SlugEn = obj.SlugEn;
                        model.Sort = obj.Sort;
                        if (RBACUser.HasPermission("RecycleBin", "ImageLibrary"))
                            model.IsDeleted = !obj.IsDeleted;
                        model.EditedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
                        imgLibraryService.Update(model);

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

            var imgs = imgService.GetAllByImageLibrary(id);
            if (imgs.Count <= 0)
            {
                var _hasDelete = imgLibraryService.Delete(id);
                status = ((int)StatusDelete.Deleted).ToString();
            }
            else
            {
                message = string.Format(Message.CONTENT_POSTDATA_DELETE_ERROR_OR_ERROR_EXIST_DATA, "Thư viện", "hình ảnh");
            }

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

            var _hasRecycleBin = imgLibraryService.GetBy(id);
            _hasRecycleBin.DeletedByDate = DateTime.Now;
            _hasRecycleBin.DeletedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
            _hasRecycleBin.IsDeleted = !isDeleted;
            imgLibraryService.Update(_hasRecycleBin);

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

        [HttpPost]
        public JsonResult IsNameVnAvailable(string NameVn)
        {
            // Check if the NameVn already exists
            return Json(imgLibraryService.IsNameVnAvailable(NameVn) != null ? false : true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult IsNameVnIdAvailable(string NameVn, string Id)
        {
            // Check if the NameVn already exists
            return Json(imgLibraryService.IsNameVnAvailable(NameVn, Id) != null ? false : true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult IsNameEnAvailable(string NameEn)
        {
            // Check if the NameEn already exists
            return Json(imgLibraryService.IsNameEnAvailable(NameEn) != null ? false : true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult IsNameEnIdAvailable(string NameEn, string Id)
        {
            // Check if the NameEn already exists
            return Json(imgLibraryService.IsNameEnAvailable(NameEn, Id) != null ? false : true, JsonRequestBehavior.AllowGet);
        }
    }
}