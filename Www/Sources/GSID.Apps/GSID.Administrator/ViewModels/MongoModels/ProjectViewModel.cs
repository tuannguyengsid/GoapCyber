using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GSID.Admin.Attributes;
using System.Web.Mvc;

namespace GSID.Admin.ViewModels.MongoModels
{
    public class ProjectFilterViewModel
    {
        public List<Project> List { get; set; }
        public string Keyword { get; set; }

        [Display(Name = "Thời gian tạo")]
        public string BeginAddDateString { get; set; }
        public string EndAddDateString { get; set; }
        public string[] ProjectCategoryId { get; set; }
        public string[] ProjectSkillId { get; set; }
        public string[] PartnerId { get; set; }
        public string[] ProductId { get; set; }
        public List<ProjectCategory> ProjectCategories { get; set; }
        public List<ProjectSkill> ProjectSkills { get; set; }
        public List<Partner> Partners { get; set; }
        public List<Product> Products { get; set; }
    }

    public class ProjectCreateViewModel : SEOEntityViewModel
    {
        [Display(Name = "Tên"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        [MyRemoteAttribute("IsNameVnAvailable", "Project", "", HttpMethod = "POST", ErrorMessage = "Tên này đã tồn tại")]
        public string NameVn { get; set; }
        [Display(Name = "Tên"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        [MyRemoteAttribute("IsNameEnAvailable", "Project", "", HttpMethod = "POST", ErrorMessage = "Tên này đã tồn tại")]
        public string NameEn { get; set; }
        [Display(Name = "Đường dẫn"), Required(ErrorMessage = "Đường dẫn buộc phải nhập.")]
        public string SlugVn { get; set; }
        [Display(Name = "Đường dẫn"), Required(ErrorMessage = "Đường dẫn buộc phải nhập.")]
        public string FullSlugVn { get; set; }
        [Display(Name = "Đường dẫn"), Required(ErrorMessage = "Đường dẫn buộc phải nhập.")]
        public string SlugEn { get; set; }
        [Display(Name = "Đường dẫn"), Required(ErrorMessage = "Đường dẫn buộc phải nhập.")]
        public string FullSlugEn { get; set; }
        [Display(Name = "Đường dẫn dự án")]
        public string Url { get; set; }
        [Display(Name = "Mô tả")]
        [AllowHtml]
        public string DescriptionVn { get; set; }
        [Display(Name = "Mô tả")]
        [AllowHtml]
        public string DescriptionEn { get; set; }
        [Display(Name = "Thời gian thực hiện")]
        public string ProjectBeginDateString { get; set; }
        public string ProjectEndDateString { get; set; }
        [Display(Name = "Nhóm dự án"), Required(ErrorMessage = "Nhóm dự án buộc phải chọn.")]
        public List<string> ProjectCategoryIds { get; set; }
        [Display(Name = "Kỷ năng")]
        public List<string> ProjectSkillIds { get; set; }
        [Display(Name = "Đối tác tham gia")]
        public List<string> PartnerIds { get; set; }
        [Display(Name = "Sản phẩm")]
        public List<string> ProductIds { get; set; }
        [Display(Name = "Danh sách hình ảnh")]
        public List<string> Images { get; set; }//slider
        [Display(Name = "Thứ tự hiển thị"), Required(ErrorMessage = "Thứ tự hiển thị buộc phải nhập.")]
        public int Sort { get; set; }
        [Display(Name = "Kích hoạt"), Required(ErrorMessage = "Kích hoạt thông tin buộc phải chọn")]
        public bool IsDeleted { get; set; }
        [Display(Name = "Hình ảnh đại diện"), Required(ErrorMessage = "Hình ảnh đại diện buộc phải nhập.")]
        public string ImageSrc { get; set; }
        public string ImageMaterialPaths { get; set; }
        public List<ProjectCategory> ProjectCategories { get; set; }
        public List<ProjectSkill> ProjectSkills { get; set; }
        public List<Partner> Partners { get; set; }
        public List<Product> Products { get; set; }
    }

    public class ProjectEditViewModel : SEOEntityViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Tên"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        [MyRemoteAttribute("IsNameVnIdAvailable", "Project", "", AdditionalFields = "Id", HttpMethod = "POST", ErrorMessage = "Tên này đã tồn tại")]
        public string NameVn { get; set; }
        [Display(Name = "Tên"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        [MyRemoteAttribute("IsNameEnIdAvailable", "Project", "", AdditionalFields = "Id", HttpMethod = "POST", ErrorMessage = "Tên này đã tồn tại")]
        public string NameEn { get; set; }
        [Display(Name = "Đường dẫn"), Required(ErrorMessage = "Đường dẫn buộc phải nhập.")]
        public string SlugVn { get; set; }
        [Display(Name = "Đường dẫn"), Required(ErrorMessage = "Đường dẫn buộc phải nhập.")]
        public string FullSlugVn { get; set; }
        [Display(Name = "Đường dẫn"), Required(ErrorMessage = "Đường dẫn buộc phải nhập.")]
        public string SlugEn { get; set; }
        [Display(Name = "Đường dẫn"), Required(ErrorMessage = "Đường dẫn buộc phải nhập.")]
        public string FullSlugEn { get; set; }
        [Display(Name = "Đường dẫn dự án")]
        public string Url { get; set; }
        [Display(Name = "Mô tả")]
        [AllowHtml]
        public string DescriptionVn { get; set; }
        [Display(Name = "Mô tả")]
        [AllowHtml]
        public string DescriptionEn { get; set; }
        [Display(Name = "Thời gian thực hiện")]
        public string ProjectBeginDateString { get; set; }
        public string ProjectEndDateString { get; set; }
        [Display(Name = "Nhóm dự án"), Required(ErrorMessage = "Nhóm dự án buộc phải chọn.")]
        public List<string> ProjectCategoryIds { get; set; }
        [Display(Name = "Kỷ năng")]
        public List<string> ProjectSkillIds { get; set; }
        [Display(Name = "Đối tác tham gia")]
        public List<string> PartnerIds { get; set; }
        [Display(Name = "Sản phẩm")]
        public List<string> ProductIds { get; set; }
        [Display(Name = "Danh sách hình ảnh")]
        public List<string> Images { get; set; }//slider
        [Display(Name = "Thứ tự hiển thị"), Required(ErrorMessage = "Thứ tự hiển thị buộc phải nhập.")]
        public int Sort { get; set; }
        [Display(Name = "Kích hoạt"), Required(ErrorMessage = "Kích hoạt thông tin buộc phải chọn")]
        public bool IsDeleted { get; set; }
        [Display(Name = "Hình ảnh đại diện"), Required(ErrorMessage = "Hình ảnh đại diện buộc phải nhập.")]
        public string ImageSrc { get; set; }
        public string ImageMaterialPaths { get; set; }
        public List<ProjectCategory> ProjectCategories { get; set; }
        public List<ProjectSkill> ProjectSkills { get; set; }
        public List<Partner> Partners { get; set; }
        public List<Product> Products { get; set; }
    }
}