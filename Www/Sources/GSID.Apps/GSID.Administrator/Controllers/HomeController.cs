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
using System.Web;
using System.Web.Mvc;
using GSID.Service.MongoRepositories.Service;
using System.Threading.Tasks;

namespace GSID.Admin.Controllers
{
    public class HomeController : BaseAuthenticationController
    {
        private readonly IContactService contactService;
        private readonly IContactMessageService contactMessageService;
        private readonly ICurriculumVitaeService curriculumVitaeService;
        private readonly IRecruitmentService recruitmentService;
        private readonly INewsService newsService;
        public HomeController(IContactMessageService _contactMessageService,
                                IContactService _contactService,
                                ICurriculumVitaeService _curriculumVitaeService,
                                IRecruitmentService _recruitmentService,
                                INewsService _newsService)
        {
            contactMessageService = _contactMessageService;
            contactService = _contactService;
            curriculumVitaeService = _curriculumVitaeService;
            recruitmentService = _recruitmentService;
            newsService = _newsService;
        }

        // GET: Home
        [RBAC]
        public ActionResult Index()
        {
            DesktopViewModel model = new DesktopViewModel();

            model.ContactMessages = contactMessageService.GetAll().Take(model.ItemTake).ToList();
            model.CurriculumVitaes = curriculumVitaeService.GetAll().Take(model.ItemTake).ToList();

            model.TotalContact = contactService.GetAll().Count;
            model.TotalICurriculumVitae = curriculumVitaeService.GetAll().Count;
            model.TotalRecruitment = recruitmentService.GetAll().Count;
            model.TotalPost = newsService.GetAll().Count;
            ViewBag.ActiveMenu = RBACUser.RootPermissionId(Request);
            return View(model);
        }
        
    }
}