using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GSID.Admin.Attributes;
using System.Web.Mvc;

namespace GSID.Admin.ViewModels.MongoModels
{
    public class RecruitmentFilterViewModel
    {
        public List<Recruitment> List { get; set; }
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

        public string Keyword { get; set; }

        [Display(Name = "Thời gian tạo")]
        public string BeginAddDateString { get; set; }
        public string EndAddDateString { get; set; }

        [Display(Name = "Ngày hết hạn")]
        public string BeginExpirationDateString { get; set; }
        public string EndExpirationDateString { get; set; }
    }

    public class RecruitmentCreateViewModel : SEOEntityViewModel
    {
        [Display(Name = "Tên"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        [MyRemoteAttribute("IsNameVnAvailable", "Recruitment", "", HttpMethod = "POST", ErrorMessage = "Tên này đã tồn tại")]
        public string NameVn { get; set; }
        [Display(Name = "Tên"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        [MyRemoteAttribute("IsNameEnAvailable", "Recruitment", "", HttpMethod = "POST", ErrorMessage = "Tên này đã tồn tại")]
        public string NameEn { get; set; }
        [Display(Name = "Mô tả")]
        [AllowHtml]
        public string DescriptionVn { get; set; }
        [Display(Name = "Mô tả")]
        [AllowHtml]
        public string DescriptionEn { get; set; }
        [Display(Name = "Nơi làm việc"), Required(ErrorMessage = "Nơi làm việc buộc phải nhập.")]
        public string SiteId { get; set; }
        [Display(Name = "Vị trí"), Required(ErrorMessage = "Vị trí buộc phải nhập.")]
        public string PositionId { get; set; }
        [Display(Name = "Phòng ban"), Required(ErrorMessage = "Phòng ban buộc phải nhập.")]
        public string DepartmentId { get; set; }
        [Display(Name = "Ngành nghề"), Required(ErrorMessage = "Ngành nghề buộc phải nhập.")]
        public string CareerId { get; set; }
        [Display(Name = "Kinh nghiệm")]
        public string ExperienceVn { get; set; }
        [Display(Name = "Kinh nghiệm")]
        public string ExperienceEn { get; set; }
        [Display(Name = "Lương")]
        public string SalaryVn { get; set; }
        [Display(Name = "Lương")]
        public string SalaryEn { get; set; }
        [Display(Name = "Ngày hết hạn")]
        public string ExpirationDateString { get; set; }

        [Display(Name = "Thẻ"), Required(ErrorMessage = "Thẻ buộc phải chọn.")]
        public string RecruitmentTagId { get; set; }
        [Display(Name = "Kích hoạt"), Required(ErrorMessage = "Kích hoạt thông tin buộc phải chọn")]
        public bool IsDeleted { get; set; }
        public string ImageSrc { get; set; }
        public List<Site> Sites { get; set; }
        public List<Position> Positions { get; set; }
        public List<Department> Departments { get; set; }
        public List<Career> Careers { get; set; }
        public List<RecruitmentTag> RecruitmentTags { get; set; }
    }

    public class RecruitmentEditViewModel : SEOEntityViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Tên"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        [MyRemoteAttribute("IsNameVnIdAvailable", "Recruitment", "", AdditionalFields = "Id", HttpMethod = "POST", ErrorMessage = "Tên này đã tồn tại")]
        public string NameVn { get; set; }
        [Display(Name = "Tên"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        [MyRemoteAttribute("IsNameEnIdAvailable", "Recruitment", "", AdditionalFields = "Id", HttpMethod = "POST", ErrorMessage = "Tên này đã tồn tại")]
        public string NameEn { get; set; }
        [Display(Name = "Mô tả")]
        [AllowHtml]
        public string DescriptionVn { get; set; }
        [Display(Name = "Mô tả")]
        [AllowHtml]
        public string DescriptionEn { get; set; }
        [Display(Name = "Nơi làm việc"), Required(ErrorMessage = "Nơi làm việc buộc phải nhập.")]
        public string SiteId { get; set; }
        [Display(Name = "Vị trí"), Required(ErrorMessage = "Vị trí buộc phải nhập.")]
        public string PositionId { get; set; }
        [Display(Name = "Phòng ban"), Required(ErrorMessage = "Phòng ban buộc phải nhập.")]
        public string DepartmentId { get; set; }
        [Display(Name = "Ngành nghề"), Required(ErrorMessage = "Ngành nghề buộc phải nhập.")]
        public string CareerId { get; set; }
        public string CareerSlugVn { get; set; }
        public string CareerSlugEn { get; set; }
        [Display(Name = "Kinh nghiệm")]
        public string ExperienceVn { get; set; }
        [Display(Name = "Kinh nghiệm")]
        public string ExperienceEn { get; set; }
        [Display(Name = "Lương")]
        public string SalaryVn { get; set; }
        [Display(Name = "Lương")]
        public string SalaryEn { get; set; }
        [Display(Name = "Ngày hết hạn")]
        public string ExpirationDateString { get; set; }

        [Display(Name = "Thẻ"), Required(ErrorMessage = "Thẻ buộc phải chọn.")]
        public string RecruitmentTagId { get; set; }
        [Display(Name = "Kích hoạt"), Required(ErrorMessage = "Kích hoạt thông tin buộc phải chọn")]
        public bool IsDeleted { get; set; }
        public string ImageSrc { get; set; }
        public List<Site> Sites { get; set; }
        public List<Position> Positions { get; set; }
        public List<Department> Departments { get; set; }
        public List<Career> Careers { get; set; }
        public List<RecruitmentTag> RecruitmentTags { get; set; }
    }

    public class PositionFilterModel
    {
        [Display(Name = "Vị trí"), Required(ErrorMessage = "Vị trí buộc phải nhập.")]
        public string PositionId { get; set; }
        public List<Position> Positions { get; set; }
    }
}