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
    public class ContactController : BaseAuthenticationController
    {
        // GET: Contact
        private readonly IContactService contactService;
        private readonly ICurriculumVitaeService curriculumVitaeService;
        private readonly IContactMessageService contactMessageService;
        private readonly IProductService prdService;
        private readonly IParameterService paraService;
        // GET: Contact
        public ContactController(IContactService _contactService, 
                                    ICurriculumVitaeService _curriculumVitaeService,
                                    IContactMessageService _contactMessageService,
                                    IProductService _prdService,
                                    IParameterService _paraService)
        {
            contactService = _contactService;
            curriculumVitaeService = _curriculumVitaeService;
            contactMessageService = _contactMessageService;
            paraService = _paraService;
            prdService = _prdService;
        }

        [RBAC]
        public async Task<ActionResult> Index(string p)
        {
            ContactFilterViewModel model = new ContactFilterViewModel();
            ViewBag.ActiveMenu = RBACUser.RootPermissionId(Request);

            return View(model);
        }

        public async Task<ActionResult> PartialIndex(string p, string Keyword, string BeginAddDateString, string EndAddDateString)
        {
            var _beginAddDate = ConvertDateTimeIsNull(BeginAddDateString);
            var _endAdddDate = ConvertDateTimeIsNull(EndAddDateString);
            var model = contactService.GetAllBySearch(Keyword, _beginAddDate, _endAdddDate);

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

            return PartialView(model);
        }

        [RBAC]
        public ActionResult Create()
        {
            ContactCreateViewModel model = new ContactCreateViewModel();
            model.IsDeleted = true;
            ViewBag.ActiveMenu = RBACUser.RootPermissionId(Request);
            return View(model);
        }

        [HttpPost]
        [RBAC]
        public ActionResult Create([Bind(Include = "FullName, Email, PhoneNumber, IsSubscribe, IsDeleted")] ContactCreateViewModel obj)
        {
            string title = Message.TITLE_ERROR;
            string message = Message.CONTENT_POSTDATA_DEFAUT_ERROR;
            string status = Default.Status_Error;
            string id = string.Empty;
            try
            {
                if (!RBACUser.HasPermission("RecycleBin", "Contact"))
                    ModelState.Remove("IsDeleted");
                if (ModelState.IsValid)
                {
                    Contact model = Mapper.Map<ContactCreateViewModel, Contact>(obj);
                    model.FullName      = !string.IsNullOrEmpty(obj.FullName) ? obj.FullName.Trim() : "";
                    model.PhoneNumber   = !string.IsNullOrEmpty(obj.PhoneNumber) ? obj.PhoneNumber.Trim() : "";
                    model.IsSubscribe = obj.IsSubscribe;
                    model.IsDeleted = !RBACUser.HasPermission("RecycleBin", "Contact") ? false : !obj.IsDeleted;
                    model.AddedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
                    id = contactService.Create(model);
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
            var model = Mapper.Map<Contact, ContactEditViewModel>(contactService.GetBy(gsid));
            if (model == null)
                Response.Redirect(string.Format("/Error/NotFound?url={0}", Request.Url), true);

            model.IsDeleted = !model.IsDeleted;
            ViewBag.ActiveMenu = RBACUser.RootPermissionId(Request);
            return View(model);
        }

        [HttpPost]
        [RBAC]
        public ActionResult Update([Bind(Include = "Id, FullName, Email, PhoneNumber, IsSubscribe, IsDeleted")] ContactEditViewModel obj)
        {
            string title = Message.TITLE_ERROR;
            string message = Message.CONTENT_POSTDATA_DEFAUT_ERROR;
            string status = Default.Status_Error;
            try
            {
                if (!RBACUser.HasPermission("RecycleBin", "Contact"))
                    ModelState.Remove("IsDeleted");
                if (ModelState.IsValid)
                {
                    var model = contactService.GetBy(obj.Id);
                    if (model != null)
                    {
                        model.FullName      = !string.IsNullOrEmpty(obj.FullName) ? obj.FullName.Trim() : "";
                        model.Email         = !string.IsNullOrEmpty(obj.Email) ? obj.Email.Trim().ToLower() : "";
                        model.PhoneNumber   = !string.IsNullOrEmpty(obj.PhoneNumber) ? obj.PhoneNumber.Trim() : "";
                        model.IsSubscribe = obj.IsSubscribe;
                        if (RBACUser.HasPermission("RecycleBin", "Contact"))
                            model.IsDeleted = !obj.IsDeleted;
                        model.EditedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
                        contactService.Update(model);

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

            contactMessageService.DeleteByContact(id);
            var Cvs = curriculumVitaeService.GetAllByContact(id);
            foreach (var cv in Cvs)
            {
                string filepath = string.Empty;
                try
                {
                    if (!string.IsNullOrEmpty(cv.FileSrc))
                    {
                        filepath = string.Format(@"{0}\{1}", GSIDSessionSiteInformation.PathAddressSiteMultimedia, cv.FileSrc.Replace("/", "\\"));
                        if (System.IO.File.Exists(filepath))
                            System.IO.File.Delete(filepath);
                    }
                }
                catch (Exception)
                {
                }
            }

            curriculumVitaeService.DeleteByContact(id);
            contactService.Delete(id);
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

            var _hasRecycleBin = contactService.GetBy(id);
            _hasRecycleBin.DeletedByDate = DateTime.Now;
            _hasRecycleBin.DeletedBy = GSIDSessionFacade.GSIDSessionUserLogon.Id;
            _hasRecycleBin.IsDeleted = !isDeleted;
            contactService.Update(_hasRecycleBin);

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