using GSID.Data.Mongodb;
using GSID.Data.Mongodb.MongoCore;
using GSID.Model.MongodbModels;
using GSID.Service;
using GSID.Setting;
using GSID.Web.Core.Authentication;
using GSID.Web.Core.Extensions;
using GSID.Admin.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using static GSID.Model.MongodbModels.User;
using GSID.Service.MongoRepositories.Service;
using System.Threading.Tasks;

namespace GSID.Admin.Controllers
{
    public class InitController : BaseAuthenticationController
    {
        // GET: ControlPanel/Init
        private readonly IGSIDMongoRepository repository;
        private readonly IPermissionService permissionService;
        private readonly ICountryService countryService;
        private readonly IProvinceService provinceService;
        private readonly IDistrictService districtService;
        private readonly IWardService wardService;
        private readonly IRoleService roleService;
        private readonly IUserService userService;

        public InitController(IGSIDMongoRepository _repository,
                                IPermissionService _permissionService,
                                ICountryService _countryService,
                                IProvinceService _provinceService,
                                IDistrictService _districtService,
                                IWardService _wardService,
                                IRoleService _roleService,
                                IUserService _userService) {

            repository          = _repository;
            permissionService   = _permissionService;
            countryService      = _countryService;
            provinceService     = _provinceService;
            districtService     = _districtService;
            wardService             = _wardService;
            roleService             = _roleService;
            userService             = _userService;
        }

        public ActionResult Navigation()
        {
            RBACUser per = new RBACUser();
            var model = per.GetListMenu();
            return PartialView(model);
        }


        public ActionResult InitData()
        {
            //CancellationTokenSource source = new CancellationTokenSource();
            //CancellationToken token = source.Token;
            //repository.DropDatabase(token);
            //DbContext.Current.DropDatabase();

            //#region permission
            //permissionService.DeleteAll();
            //string parentId = string.Empty;
            //Permission permission;





            //#region Quản lý trang
            //permission = new Permission();
            //permission.ParentId = string.Empty;
            //permission.Name = "Quản lý trang";
            //permission.Description = "enterprise-index";
            //permission.IsMenu = true;
            //permission.Url = "/Economic/Enterprise";
            //permission.Icon = "<i class='ion-flash'></i>";
            //permission.Sort = 1;
            //permission.IsDeleted = false;
            //permission.AddedByDate = DateTime.Now;
            //parentId = permissionService.Create(permission);

            //////////////////////////////////////////////////////////////
            //permission = new Permission();
            //permission.ParentId     = parentId;
            //permission.Name         = "Xem danh sách";
            //permission.Description  = "enterprise-index";
            //permission.IsMenu       = false;
            //permission.Url          = "";
            //permission.Icon         = "";
            //permission.Sort         = 1;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //permissionService.Create(permission);


            //permission = new Permission();
            //permission.ParentId = parentId;
            //permission.Name     = "Tạo mới";
            //permission.Description  = "enterprise-create";
            //permission.IsMenu       = false;
            //permission.Url          = "";
            //permission.Icon         = "";
            //permission.Sort         = 2;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //permissionService.Create(permission);

            //permission = new Permission();
            //permission.ParentId     = parentId;
            //permission.Name         = "Cập nhật";
            //permission.Description  = "enterprise-update";
            //permission.IsMenu       = false;
            //permission.Url          = "";
            //permission.Icon         = "";
            //permission.Sort         = 3;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //permissionService.Create(permission);

            //permission = new Permission();
            //permission.ParentId     = parentId;
            //permission.Name         = "Xóa";
            //permission.Description  = "enterprise-delete";
            //permission.IsMenu       = false;
            //permission.Url          = "";
            //permission.Icon         = "";
            //permission.Sort         = 4;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //permissionService.Create(permission);

            //permission = new Permission();
            //permission.ParentId     = parentId;
            //permission.Name = "Đồng bộ dữ liệu";
            //permission.Description  = "enterprise-importdata";
            //permission.IsMenu       = false;
            //permission.Url          = "";
            //permission.Icon         = "";
            //permission.Sort         = 5;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //permissionService.Create(permission);

            //permission = new Permission();
            //permission.ParentId     = parentId;
            //permission.Name         = "Xuất dữ liệu Excel";
            //permission.Description  = "enterprise-exporttoexcel";
            //permission.IsMenu       = false;
            //permission.Url          = "";
            //permission.Icon         = "";
            //permission.Sort         = 6;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //permissionService.Create(permission);

            //permission = new Permission();
            //permission.ParentId     = parentId;
            //permission.Name         = "Gửi Email";
            //permission.Description  = "enterprise-sentemail";
            //permission.IsMenu       = false;
            //permission.Url          = "";
            //permission.Icon         = "";
            //permission.Sort         = 7;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //permissionService.Create(permission);

            //permission = new Permission();
            //permission.ParentId     = parentId;
            //permission.Name         = "Báo cáo vi phạm";
            //permission.Description  = "report-reportviolation";
            //permission.IsMenu       = false;
            //permission.Url          = "";
            //permission.Icon         = "";
            //permission.Sort         = 8;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //permissionService.Create(permission);

            //permission = new Permission();
            //permission.ParentId     = parentId;
            //permission.Name           = "Tìm kiếm";
            //permission.Description  = "enterprise-searchadvance";
            //permission.IsMenu       = false;
            //permission.Url          = "";
            //permission.Icon         = "";
            //permission.Sort         = 9;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //permissionService.Create(permission);


            //permission = new Permission();
            //permission.ParentId = parentId;
            //permission.Name = "Tìm kiếm";
            //permission.Description = "enterprise-searchadvance";
            //permission.IsMenu = false;
            //permission.Url = "";
            //permission.Icon = "";
            //permission.Sort = 9;
            //permission.IsDeleted = false;
            //permission.AddedByDate = DateTime.Now;
            //permissionService.Create(permission);
            //#endregion

            //#region Hộ kinh doanh
            //permission = new Permission();
            //permission.ParentId     = string.Empty;
            //permission.Name         = "Hộ kinh doanh";
            //permission.Description  = "householdbusiness-index";
            //permission.IsMenu       = true;
            //permission.Url          = "/Economic/HouseHoldBusiness";
            //permission.Icon         = "<i class='ion-ios-home-outline'></i>";
            //permission.Sort         = 2;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //parentId = permissionService.Create(permission);



            //permission = new Permission();
            //permission.ParentId     = parentId;
            //permission.Name         = "Xem danh sách";
            //permission.Description  = "householdbusiness-index";
            //permission.IsMenu       = false;
            //permission.Url          = "";
            //permission.Icon         = "";
            //permission.Sort         = 1;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //permissionService.Create(permission);

            //permission = new Permission();
            //permission.ParentId     = parentId;
            //permission.Name         = "Tạo mới";
            //permission.Description  = "householdbusiness-create";
            //permission.IsMenu       = false;
            //permission.Url          = "";
            //permission.Icon         = "";
            //permission.Sort         = 2;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //permissionService.Create(permission);

            //permission = new Permission();
            //permission.ParentId     = parentId;
            //permission.Name         = "Cập nhật";
            //permission.Description  = "householdbusiness-update";
            //permission.IsMenu       = false;
            //permission.Url          = "";
            //permission.Icon         = "";
            //permission.Sort         = 3;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //permissionService.Create(permission);

            //permission = new Permission();
            //permission.ParentId     = parentId;
            //permission.Name         = "Xóa";
            //permission.Description  = "householdbusiness-delete";
            //permission.IsMenu       = false;
            //permission.Url          = "";
            //permission.Icon         = "";
            //permission.Sort         = 4;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //permissionService.Create(permission);

            //permission = new Permission();
            //permission.ParentId     = parentId;
            //permission.Name         = "Đồng bộ dữ liệu";
            //permission.Description  = "householdbusiness-importdata";
            //permission.IsMenu       = false;
            //permission.Url          = "";
            //permission.Icon         = "";
            //permission.Sort         = 5;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //permissionService.Create(permission);

            //permission = new Permission();
            //permission.ParentId     = parentId;
            //permission.Name         = "Xuất dữ liệu Excel";
            //permission.Description  = "householdbusiness-exporttoexcel";
            //permission.IsMenu       = false;
            //permission.Url          = "";
            //permission.Icon         = "";
            //permission.Sort         = 6;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //permissionService.Create(permission);

            //permission = new Permission();
            //permission.ParentId     = parentId;
            //permission.Name         = "Gửi Email";
            //permission.Description  = "householdbusiness-sentemail";
            //permission.IsMenu       = false;
            //permission.Url          = "";
            //permission.Icon         = "";
            //permission.Sort         = 7;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //permissionService.Create(permission);

            //permission = new Permission();
            //permission.ParentId     = parentId;
            //permission.Name = "Báo cáo vi phạm";
            //permission.Description = "report-reportviolation";
            //permission.IsMenu       = false;
            //permission.Url          = "";
            //permission.Icon         = "";
            //permission.Sort         = 8;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //permissionService.Create(permission);


            //permission = new Permission();
            //permission.ParentId = parentId;
            //permission.Name = "Tìm kiếm";
            //permission.Description = "householdbusiness-searchadvance";
            //permission.IsMenu = false;
            //permission.Url = "";
            //permission.Icon = "";
            //permission.Sort = 9;
            //permission.IsDeleted = false;
            //permission.AddedByDate = DateTime.Now;
            //permissionService.Create(permission);
            //#endregion

            //#region Vi phạm
            //permission = new Permission();
            //permission.ParentId     = string.Empty;
            //permission.Name         = "Vi phạm";
            //permission.Description  = "reportViolation-index";
            //permission.IsMenu       = true;
            //permission.Url          = "/ReportViolation";
            //permission.Icon         = "<i class='ion-alert-circled'></i>";
            //permission.Sort         = 3;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //parentId = permissionService.Create(permission);

            //permission = new Permission();
            //permission.ParentId = parentId;
            //permission.Name = "Xem danh sách";
            //permission.Description = "reportViolation-index";
            //permission.IsMenu = false;
            //permission.Url = "";
            //permission.Icon = "";
            //permission.Sort = 1;
            //permission.IsDeleted = false;
            //permission.AddedByDate = DateTime.Now;
            //permissionService.Create(permission);

            //permission = new Permission();
            //permission.ParentId     = parentId;
            //permission.Name         = "Tạo mới";
            //permission.Description  = "reportViolation-create";
            //permission.IsMenu       = false;
            //permission.Url          = "";
            //permission.Icon         = "";
            //permission.Sort         = 1;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //permissionService.Create(permission);

            //permission = new Permission();
            //permission.ParentId     = parentId;
            //permission.Name         = "Cập nhật";
            //permission.Description  = "reportViolation-update";
            //permission.IsMenu       = false;
            //permission.Url          = "";
            //permission.Icon         = "";
            //permission.Sort         = 2;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //permissionService.Create(permission);

            //permission = new Permission();
            //permission.ParentId     = parentId;
            //permission.Name         = "Xóa";
            //permission.Description  = "reportViolation-delete";
            //permission.IsMenu       = false;
            //permission.Url          = "";
            //permission.Icon         = "";
            //permission.Sort         = 3;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //permissionService.Create(permission);

            //#endregion

            //#region Tình trạng hoạt động  
            //permission = new Permission();
            //permission.ParentId     = string.Empty;
            //permission.Name         = "Tình trạng hoạt động";
            //permission.Description  = "businessstatus-index";
            //permission.IsMenu       = true;
            //permission.Url          = "/BusinessStatus";
            //permission.Icon         = "<i class='ion-speedometer'></i>";
            //permission.Sort         = 4;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //parentId = permissionService.Create(permission);

            //permission = new Permission();
            //permission.ParentId = parentId;
            //permission.Name = "Xem danh sách";
            //permission.Description = "businessstatus-index";
            //permission.IsMenu = false;
            //permission.Url = "";
            //permission.Icon = "";
            //permission.Sort = 1;
            //permission.IsDeleted = false;
            //permission.AddedByDate = DateTime.Now;
            //permissionService.Create(permission);

            //permission = new Permission();
            //permission.ParentId     = parentId;
            //permission.Name         = "Tạo mới";
            //permission.Description  = "businessstatus-create";
            //permission.IsMenu       = false;
            //permission.Url          = "";
            //permission.Icon         = "";
            //permission.Sort         = 1;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //permissionService.Create(permission);

            //permission = new Permission();
            //permission.ParentId     = parentId;
            //permission.Name         = "Cập nhật";
            //permission.Description  = "businessstatus-update";
            //permission.IsMenu       = false;
            //permission.Url          = "";
            //permission.Icon         = "";
            //permission.Sort         = 2;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //permissionService.Create(permission);

            //permission = new Permission();
            //permission.ParentId     = parentId;
            //permission.Name         = "Xóa";
            //permission.Description  = "businessstatus-delete";
            //permission.IsMenu       = false;
            //permission.Url          = "";
            //permission.Icon         = "";
            //permission.Sort         = 3;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //permissionService.Create(permission);

            //#endregion

            //#region Loại hình doanh nghiệp
            //permission = new Permission();
            //permission.ParentId     = string.Empty;
            //permission.Name         = "Loại hình doanh nghiệp";
            //permission.Description  = "businesstype-index";
            //permission.IsMenu       = true;
            //permission.Url          = "/BusinessType";
            //permission.Icon         = "<i class='ion-ios-barcode-outline'></i>";
            //permission.Sort         = 5;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //parentId = permissionService.Create(permission);

            //permission = new Permission();
            //permission.ParentId = parentId;
            //permission.Name = "Xem danh sách";
            //permission.Description = "businesstype-index";
            //permission.IsMenu = false;
            //permission.Url = "";
            //permission.Icon = "";
            //permission.Sort = 1;
            //permission.IsDeleted = false;
            //permission.AddedByDate = DateTime.Now;
            //permissionService.Create(permission);

            //permission = new Permission();
            //permission.ParentId     = parentId;
            //permission.Name         = "Tạo mới";
            //permission.Description  = "businesstype-create";
            //permission.IsMenu       = false;
            //permission.Url          = "";
            //permission.Icon         = "";
            //permission.Sort         = 1;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //permissionService.Create(permission);

            //permission = new Permission();
            //permission.ParentId     = parentId;
            //permission.Name         = "Cập nhật";
            //permission.Description  = "businesstype-update";
            //permission.IsMenu       = false;
            //permission.Url          = "";
            //permission.Icon         = "";
            //permission.Sort         = 2;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //permissionService.Create(permission);

            //permission = new Permission();
            //permission.ParentId     = parentId;
            //permission.Name         = "Xóa";
            //permission.Description  = "businesstype-delete";
            //permission.IsMenu       = false;
            //permission.Url          = "";
            //permission.Icon         = "";
            //permission.Sort         = 3;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //permissionService.Create(permission);
            //#endregion

            //#region Nguồn vốn
            //permission = new Permission();
            //permission.ParentId     = string.Empty;
            //permission.Name         = "Nguồn vốn";
            //permission.Description  = "capitalorigin-index";
            //permission.IsMenu       = true;
            //permission.Url          = "/CapitalOrigin";
            //permission.Icon         = "<i class='ion-paintbucket'></i>";
            //permission.Sort         = 6;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //parentId = permissionService.Create(permission);

            //permission = new Permission();
            //permission.ParentId = parentId;
            //permission.Name = "Xem danh sách";
            //permission.Description = "capitalorigin-index";
            //permission.IsMenu = false;
            //permission.Url = "";
            //permission.Icon = "";
            //permission.Sort = 1;
            //permission.IsDeleted = false;
            //permission.AddedByDate = DateTime.Now;
            //permissionService.Create(permission);

            //permission = new Permission();
            //permission.ParentId     = parentId;
            //permission.Name         = "Tạo mới";
            //permission.Description  = "capitalorigin-create";
            //permission.IsMenu       = false;
            //permission.Url          = "";
            //permission.Icon         = "";
            //permission.Sort         = 2;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //permissionService.Create(permission);

            //permission = new Permission();
            //permission.ParentId     = parentId;
            //permission.Name         = "Cập nhật";
            //permission.Description  = "capitalorigin-update";
            //permission.IsMenu       = false;
            //permission.Url          = "";
            //permission.Icon         = "";
            //permission.Sort         = 3;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //permissionService.Create(permission);

            //permission = new Permission();
            //permission.ParentId     = parentId;
            //permission.Name         = "Xóa";
            //permission.Description  = "capitalorigin-delete";
            //permission.IsMenu       = false;
            //permission.Url          = "";
            //permission.Icon         = "";
            //permission.Sort         = 4;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //permissionService.Create(permission);
            //#endregion

            //#region Ngành nghề
            //permission = new Permission();
            //permission.ParentId     = string.Empty;
            //permission.Name         = "Ngành nghề";
            //permission.Description  = "career-index";
            //permission.IsMenu       = true;
            //permission.Url          = "/Career";
            //permission.Icon         = "<i class='ion-grid'></i>";
            //permission.Sort         = 7;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //parentId = permissionService.Create(permission);

            //permission = new Permission();
            //permission.ParentId = parentId;
            //permission.Name = "Xem danh sách";
            //permission.Description = "career-index";
            //permission.IsMenu = false;
            //permission.Url = "";
            //permission.Icon = "";
            //permission.Sort = 1;
            //permission.IsDeleted = false;
            //permission.AddedByDate = DateTime.Now;
            //permissionService.Create(permission);

            //permission = new Permission();
            //permission.ParentId     = parentId;
            //permission.Name         = "Tạo mới";
            //permission.Description  = "career-create";
            //permission.IsMenu       = false;
            //permission.Url          = "";
            //permission.Icon         = "";
            //permission.Sort         = 1;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //permissionService.Create(permission);

            //permission = new Permission();
            //permission.ParentId     = parentId;
            //permission.Name         = "Cập nhật";
            //permission.Description  = "career-update";
            //permission.IsMenu       = false;
            //permission.Url          = "";
            //permission.Icon         = "";
            //permission.Sort         = 2;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //permissionService.Create(permission);

            //permission = new Permission();
            //permission.ParentId     = parentId;
            //permission.Name         = "Xóa";
            //permission.Description  = "career-delete";
            //permission.IsMenu       = false;
            //permission.Url          = "";
            //permission.Icon         = "";
            //permission.Sort         = 3;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //permissionService.Create(permission);
            //#endregion

            //#region Lĩnh vực
            //permission = new Permission();
            //permission.ParentId     = string.Empty;
            //permission.Name         = "Lĩnh vực";
            //permission.Description  = "field-index";
            //permission.IsMenu       = true;
            //permission.Url          = "/Field";
            //permission.Icon         = "<i class='ion-ios-toggle'></i>";
            //permission.Sort         = 8;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //parentId = permissionService.Create(permission);

            //permission = new Permission();
            //permission.ParentId = parentId;
            //permission.Name = "Xem danh sách";
            //permission.Description = "field-index";
            //permission.IsMenu = false;
            //permission.Url = "";
            //permission.Icon = "";
            //permission.Sort = 1;
            //permission.IsDeleted = false;
            //permission.AddedByDate = DateTime.Now;
            //permissionService.Create(permission);

            //permission = new Permission();
            //permission.ParentId     = parentId;
            //permission.Name         = "Tạo mới";
            //permission.Description  = "field-create";
            //permission.IsMenu       = false;
            //permission.Url          = "";
            //permission.Icon         = "";
            //permission.Sort         = 1;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //permissionService.Create(permission);

            //permission = new Permission();
            //permission.ParentId     = parentId;
            //permission.Name         = "Cập nhật";
            //permission.Description  = "field-update";
            //permission.IsMenu       = false;
            //permission.Url          = "";
            //permission.Icon         = "";
            //permission.Sort         = 2;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //permissionService.Create(permission);

            //permission = new Permission();
            //permission.ParentId     = parentId;
            //permission.Name         = "Xóa";
            //permission.Description  = "field-delete";
            //permission.IsMenu       = false;
            //permission.Url          = "";
            //permission.Icon         = "";
            //permission.Sort         = 3;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //permissionService.Create(permission);
            //#endregion

            //#region Danh mục vi phạm
            //permission = new Permission();
            //permission.ParentId     = string.Empty;
            //permission.Name         = "Danh mục vi phạm";
            //permission.Description  = "ReportViolationCategory-index";
            //permission.IsMenu       = true;
            //permission.Url          = "/ReportViolationCategory";
            //permission.Icon         = "<i class='ion-alert'></i>";
            //permission.Sort         = 9;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //parentId = permissionService.Create(permission);

            //permission = new Permission();
            //permission.ParentId = parentId;
            //permission.Name = "Xem danh sách";
            //permission.Description = "ReportViolationCategory-index";
            //permission.IsMenu = false;
            //permission.Url = "";
            //permission.Icon = "";
            //permission.Sort = 1;
            //permission.IsDeleted = false;
            //permission.AddedByDate = DateTime.Now;
            //permissionService.Create(permission);

            //permission = new Permission();
            //permission.ParentId     = parentId;
            //permission.Name         = "Tạo mới";
            //permission.Description  = "ReportViolationCategory-create";
            //permission.IsMenu       = false;
            //permission.Url          = "";
            //permission.Icon         = "";
            //permission.Sort         = 1;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //permissionService.Create(permission);

            //permission = new Permission();
            //permission.ParentId     = parentId;
            //permission.Name         = "Cập nhật";
            //permission.Description  = "ReportViolationCategory-update";
            //permission.IsMenu       = false;
            //permission.Url          = "";
            //permission.Icon         = "";
            //permission.Sort         = 2;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //permissionService.Create(permission);

            //permission = new Permission();
            //permission.ParentId     = parentId;
            //permission.Name         = "Xóa";
            //permission.Description  = "ReportViolationCategory-delete";
            //permission.IsMenu       = false;
            //permission.Url          = "";
            //permission.Icon         = "";
            //permission.Sort         = 3;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //permissionService.Create(permission);
            //#endregion

            //#region Cấu hình
            //permission = new Permission();
            //permission.ParentId     = string.Empty;
            //permission.Name         = "Cấu hình";
            //permission.Description  = string.Empty;
            //permission.IsMenu       = true;
            //permission.Url          = "#";
            //permission.Icon         = "<i class='ion-settings'></i>";
            //permission.Sort         = 9;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //parentId = permissionService.Create(permission);

            //#region Phường/xã
            //string childId = string.Empty;
            //permission = new Permission();
            //permission.ParentId     = parentId;
            //permission.Name         = "Phường/xã";
            //permission.Description  = "ward-index";
            //permission.IsMenu       = true;
            //permission.Url          = "/Ward";
            //permission.Icon         = string.Empty;
            //permission.Sort         = 1;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //childId = permissionService.Create(permission);

            //permission = new Permission();
            //permission.ParentId = parentId;
            //permission.Name = "Xem danh sách";
            //permission.Description = "ward-index";
            //permission.IsMenu = false;
            //permission.Url = "";
            //permission.Icon = "";
            //permission.Sort = 1;
            //permission.IsDeleted = false;
            //permission.AddedByDate = DateTime.Now;
            //permissionService.Create(permission);

            //permission = new Permission();
            //permission.ParentId     = childId;
            //permission.Name         = "Tạo mới";
            //permission.Description  = "ward-create";
            //permission.IsMenu       = false;
            //permission.Url          = "";
            //permission.Icon         = "";
            //permission.Sort         = 1;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //permissionService.Create(permission);

            //permission = new Permission();
            //permission.ParentId     = childId;
            //permission.Name         = "Cập nhật";
            //permission.Description  = "ward-update";
            //permission.IsMenu       = false;
            //permission.Url          = "";
            //permission.Icon         = "";
            //permission.Sort         = 2;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //permissionService.Create(permission);

            //permission = new Permission();
            //permission.ParentId     = childId;
            //permission.Name         = "Xóa";
            //permission.Description  = "ward-delete";
            //permission.IsMenu       = false;
            //permission.Url          = "";
            //permission.Icon         = "";
            //permission.Sort         = 3;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //permissionService.Create(permission);
            //#endregion

            //#region Quản trị viên
            //permission = new Permission();
            //permission.ParentId     = parentId;
            //permission.Name         = "Quản trị viên";
            //permission.Description  = "user-index";
            //permission.IsMenu       = true;
            //permission.Url          = "/User";
            //permission.Icon         = string.Empty;
            //permission.Sort         = 2;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //childId = permissionService.Create(permission);

            //permission = new Permission();
            //permission.ParentId = parentId;
            //permission.Name = "Xem danh sách";
            //permission.Description = "user-index";
            //permission.IsMenu = false;
            //permission.Url = "";
            //permission.Icon = "";
            //permission.Sort = 1;
            //permission.IsDeleted = false;
            //permission.AddedByDate = DateTime.Now;
            //permissionService.Create(permission);

            //permission = new Permission();
            //permission.ParentId     = childId;
            //permission.Name         = "Tạo mới";
            //permission.Description  = "user-create";
            //permission.IsMenu       = false;
            //permission.Url          = "";
            //permission.Icon         = "";
            //permission.Sort         = 1;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //permissionService.Create(permission);

            //permission = new Permission();
            //permission.ParentId     = childId;
            //permission.Name         = "Cập nhật";
            //permission.Description  = "user-update";
            //permission.IsMenu       = false;
            //permission.Url          = "";
            //permission.Icon         = "";
            //permission.Sort         = 2;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //permissionService.Create(permission);

            //permission = new Permission();
            //permission.ParentId     = childId;
            //permission.Name         = "Xóa";
            //permission.Description  = "user-delete";
            //permission.IsMenu       = false;
            //permission.Url          = "";
            //permission.Icon         = "";
            //permission.Sort         = 3;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //permissionService.Create(permission);
            //#endregion

            //#region Phân quyền chức năng
            //permission = new Permission();
            //permission.ParentId     = parentId;
            //permission.Name         = "Phân quyền chức năng";
            //permission.Description  = "role-index";
            //permission.IsMenu       = true;
            //permission.Url          = "/Role";
            //permission.Icon         = string.Empty;
            //permission.Sort         = 3;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //childId = permissionService.Create(permission);


            //permission = new Permission();
            //permission.ParentId = parentId;
            //permission.Name = "Xem danh sách";
            //permission.Description = "role-index";
            //permission.IsMenu = false;
            //permission.Url = "";
            //permission.Icon = "";
            //permission.Sort = 1;
            //permission.IsDeleted = false;
            //permission.AddedByDate = DateTime.Now;
            //permissionService.Create(permission);

            //permission = new Permission();
            //permission.ParentId     = childId;
            //permission.Name         = "Tạo mới";
            //permission.Description  = "role-create";
            //permission.IsMenu       = false;
            //permission.Url          = "";
            //permission.Icon         = "";
            //permission.Sort         = 1;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //permissionService.Create(permission);

            //permission = new Permission();
            //permission.ParentId     = childId;
            //permission.Name         = "Cập nhật";
            //permission.Description  = "role-update";
            //permission.IsMenu       = false;
            //permission.Url          = "";
            //permission.Icon         = "";
            //permission.Sort         = 2;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //permissionService.Create(permission);

            //permission = new Permission();
            //permission.ParentId     = childId;
            //permission.Name         = "Xóa";
            //permission.Description  = "role-delete";
            //permission.IsMenu       = false;
            //permission.Url          = "";
            //permission.Icon         = "";
            //permission.Sort         = 3;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //permissionService.Create(permission);
            //#endregion

            //#region Phân quyền dữ liệu
            //permission = new Permission();
            //permission.ParentId     = parentId;
            //permission.Name         = "Phân quyền dữ liệu";
            //permission.Description  = "wardrole-index";
            //permission.IsMenu       = true;
            //permission.Url          = "/WardRole";
            //permission.Icon         = string.Empty;
            //permission.Sort         = 4;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //childId = permissionService.Create(permission);

            //permission = new Permission();
            //permission.ParentId = parentId;
            //permission.Name = "Xem danh sách";
            //permission.Description = "wardrole-index";
            //permission.IsMenu = false;
            //permission.Url = "";
            //permission.Icon = "";
            //permission.Sort = 1;
            //permission.IsDeleted = false;
            //permission.AddedByDate = DateTime.Now;
            //permissionService.Create(permission);

            //permission = new Permission();
            //permission.ParentId     = childId;
            //permission.Name         = "Tạo mới";
            //permission.Description  = "wardrole-create";
            //permission.IsMenu       = false;
            //permission.Url          = "";
            //permission.Icon         = "";
            //permission.Sort         = 1;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //permissionService.Create(permission);

            //permission = new Permission();
            //permission.ParentId     = childId;
            //permission.Name         = "Cập nhật";
            //permission.Description  = "wardrole-update";
            //permission.IsMenu       = false;
            //permission.Url          = "";
            //permission.Icon         = "";
            //permission.Sort         = 2;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //permissionService.Create(permission);

            //permission = new Permission();
            //permission.ParentId     = childId;
            //permission.Name         = "Xóa";
            //permission.Description  = "wardrole-delete";
            //permission.IsMenu       = false;
            //permission.Url          = "";
            //permission.Icon         = "";
            //permission.Sort         = 3;
            //permission.IsDeleted    = false;
            //permission.AddedByDate  = DateTime.Now;
            //permissionService.Create(permission);
            //#endregion

            //#endregion












            //#endregion


            //#region role
            //roleService.DeleteAll();
            //var role = new Role();
            //role.Name       = "Quản trị viên";
            //role.IsSysAdmin = true;
            //role.IsDeleted  = false;
            //roleService.Create(role);

            //wardRoleService.DeleteAll();
            //var wardRole    = new WardRole();
            //var wards       = wardService.GetAll();
            //wardRole.Name           = "Quản trị viên";
            //wardRole.IsDeleted      = false;
            //wardRoleService.Create(wardRole, wards);


            //userService.DeleteAll();
            //var user = new User();
            //user.Email          = "support@g-sid.com";
            //user.FirstName      = "Administrator";
            //user.LastName       = "G-SID";
            //user.PasswordSalt   = RandomStringGenerator.RandomString();
            //user.Password       = PasswordHelper.GenerateHashedPassword("123456", user.PasswordSalt);
            //user.IsType         = UserIsType.UserinSystem;
            //string id = userService.Create(user);

            //if (user.Id != null) {
            //    userToRoleService.DeleteAll();
            //    RoleToUser u2role = new RoleToUser();
            //    u2role.UserId = user.Id;
            //    u2role.RoleId = role.Id;
            //    userToRoleService.Create(u2role);

            //    wardRoleToUserService.DeleteAll();
            //    WardRoleToUser wrole2u = new WardRoleToUser();
            //    wrole2u.UserId = user.Id;
            //    wrole2u.WardRoleId = wardRole.Id;
            //    wardRoleToUserService.Create(wrole2u);
            //}

            //#endregion






            return View();
        }
    }
}