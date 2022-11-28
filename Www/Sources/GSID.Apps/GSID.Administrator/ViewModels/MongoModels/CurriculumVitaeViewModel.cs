using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GSID.Admin.Attributes;
using System.Linq;
using System.Web;
using static GSID.Model.MongodbModels.Position;

namespace GSID.Admin.ViewModels.MongoModels
{
    public class CurriculumVitaeFilterViewModel
    {
        public List<CurriculumVitae> List { get; set; }
        public string Keyword { get; set; }

        [Display(Name = "Thời gian tạo")]
        public string BeginAddDateString { get; set; }
        public string EndAddDateString { get; set; }
        [Display(Name = "Nơi làm việc")]
        public string[] SiteId { get; set; }
        public List<Site> Sites { get; set; }

        [Display(Name = "Vị trí")]
        public string[] PositionId { get; set; }
        public List<Position> Positions { get; set; }

        [Display(Name = "Phòng ban")]
        public string[] DepartmentId { get; set; }
        public List<Department> Departments { get; set; }
        [Display(Name = "Ngành nghề")]
        public string[] CareerId { get; set; }
        public List<Career> Careers { get; set; }

        [Display(Name = "Thẻ")]
        public string RecruitmentTagId { get; set; }
        public List<RecruitmentTag> RecruitmentTags { get; set; }

        [Display(Name = "Ngày hết hạn")]
        public string BeginExpirationDateString { get; set; }
        public string EndExpirationDateString { get; set; }
    }

    public class CurriculumVitaeCreateViewModel
    {
        [Display(Name = "Tên"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        public string NameVn { get; set; }
        [Display(Name = "Tên"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        public string NameEn { get; set; }
        [Display(Name = "Phòng ban"), Required(ErrorMessage = "Phòng ban buộc phải chọn.")]
        public string DepartmentId { get; set; }
        [Display(Name = "Kích hoạt"), Required(ErrorMessage = "Kích hoạt thông tin buộc phải chọn")]
        public bool IsDeleted { get; set; }
        public List<Department> Departments { get; set; }
    }

    public class CurriculumVitaeEditViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Tên"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        public string NameVn { get; set; }
        [Display(Name = "Tên"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        public string NameEn { get; set; }
        [Display(Name = "Phòng ban"), Required(ErrorMessage = "Phòng ban buộc phải chọn.")]
        public string DepartmentId { get; set; }
        [Display(Name = "Kích hoạt"), Required(ErrorMessage = "Kích hoạt thông tin buộc phải chọn")]
        public bool IsDeleted { get; set; }
        public List<Department> Departments { get; set; }
    }
}