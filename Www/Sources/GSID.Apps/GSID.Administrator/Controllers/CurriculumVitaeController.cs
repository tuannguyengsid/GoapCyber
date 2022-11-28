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
    public class CurriculumVitaeController : BaseAuthenticationController
    {
        // GET: CurriculumVitae
        private readonly ICurriculumVitaeService curriculumVitaeService;
        private readonly IContactService contactService;
        private readonly ICareerService careerService;
        private readonly ISiteService siteService;
        private readonly IPositionService positionService;
        private readonly IDepartmentService departmentService;
        private readonly IRecruitmentService recruitmentService;
        private readonly IRecruitmentTagService recruitmentTagService;
        private readonly IParameterService paraService;
        // GET: CurriculumVitae
        public CurriculumVitaeController(ICurriculumVitaeService _curriculumVitaeService,
                                            IContactService _contactService, 
                                            ICareerService _careerService,
                                            ISiteService _siteService,
                                            IPositionService _positionService,
                                            IDepartmentService _departmentService,
                                            IRecruitmentService _recruitmentService,
                                            IRecruitmentTagService _recruitmentTagService,
                                IParameterService _paraService)
        {
            contactService = _contactService;
            curriculumVitaeService = _curriculumVitaeService;
            careerService = _careerService;
            siteService = _siteService;
            positionService = _positionService;
            departmentService = _departmentService;
            recruitmentService = _recruitmentService;
            recruitmentTagService = _recruitmentTagService;
            paraService = _paraService;
        }

        [RBAC]
        public async Task<ActionResult> Index(string p)
        {
            CurriculumVitaeFilterViewModel model = new CurriculumVitaeFilterViewModel();
            ViewBag.ActiveMenu = RBACUser.RootPermissionId(Request);
            model.Sites = siteService.GetAll();
            model.Positions = positionService.GetAll();
            model.Departments = departmentService.GetAll();
            model.Careers = careerService.GetAll();
            model.RecruitmentTags = recruitmentTagService.GetAll();

            return View(model);
        }

        public async Task<ActionResult> PartialIndex(string p, string Keyword, string BeginAddDateString, string EndAddDateString, string BeginExpirationDateString, string EndExpirationDateString, string[] SiteId, string[] PositionId, string[] DepartmentId, string[] CareerId, string[] RecruitmentTagId)
        {
            var _beginAddDate = ConvertDateTimeIsNull(BeginAddDateString);
            var _endAdddDate = ConvertDateTimeIsNull(EndAddDateString);
            var _beginExpirationDateString = ConvertDateTimeIsNull(BeginExpirationDateString);
            var _endExpirationDateString = ConvertDateTimeIsNull(EndExpirationDateString);
            var model = curriculumVitaeService.GetAllBySearch(Keyword, _beginAddDate, _endAdddDate, _beginExpirationDateString, _endExpirationDateString, SiteId, PositionId, DepartmentId, CareerId, RecruitmentTagId);

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

        [HttpPost]
        [RBAC]
        public ActionResult Delete(string id)
        {
            string status = ((int)StatusDelete.Error).ToString();
            string message = Message.CONTENT_POSTDATA_DELETE_ERROR_OR_EMPTY;
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var _hasDelete = curriculumVitaeService.Delete(id);
            status = ((int)StatusDelete.Deleted).ToString();

            return Json(new
            {
                Status = status,
                Message = message
            });
        }
    }
}