using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GSID.Admin.Attributes;
using System.Web.Mvc;

namespace GSID.Admin.ViewModels.MongoModels
{
    public class NewsFilterViewModel
    {
        public List<News> List { get; set; }
        [Display(Name = "Thẻ")]
        public string[] TagPostId { get; set; }
        public List<TagPost> TagPosts { get; set; }
        [Display(Name = "Danh mục")]
        public string[] NewsCategoryId { get; set; }
        public List<NewsCategory> NewsCategories { get; set; }
        public string Keyword { get; set; }

        [Display(Name = "Thời gian tạo")]
        public string BeginAddDateString { get; set; }
        public string EndAddDateString { get; set; }
    }

    public class NewsCreateViewModel : SEOEntityViewModel
    {
        [Display(Name = "Tiêu đề"), Required(ErrorMessage = "Tiêu đề buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        [MyRemoteAttribute("IsNameVnAvailable", "News", "", HttpMethod = "POST", ErrorMessage = "Tên này đã tồn tại")]
        public string NameVn { get; set; }
        [Display(Name = "Tiêu đề"), Required(ErrorMessage = "Tiêu đề buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        [MyRemoteAttribute("IsNameEnAvailable", "News", "", HttpMethod = "POST", ErrorMessage = "Tên này đã tồn tại")]
        public string NameEn { get; set; }
        [Display(Name = "Đường dẫn"), Required(ErrorMessage = "Đường dẫn buộc phải nhập.")]
        public string SlugVn { get; set; }
        [Display(Name = "Đường dẫn"), Required(ErrorMessage = "Đường dẫn buộc phải nhập.")]
        public string FullSlugVn { get; set; }
        [Display(Name = "Đường dẫn"), Required(ErrorMessage = "Đường dẫn buộc phải nhập.")]
        public string SlugEn { get; set; }
        [Display(Name = "Đường dẫn"), Required(ErrorMessage = "Đường dẫn buộc phải nhập.")]
        public string FullSlugEn { get; set; }
        [Display(Name = "Mô tả ngắn")]
        public string ShortDescriptionVn { get; set; }
        [Display(Name = "Mô tả ngắn")]
        public string ShortDescriptionEn { get; set; }
        [Display(Name = "Mô tả")]
        [AllowHtml]
        public string DescriptionVn { get; set; }
        [Display(Name = "Mô tả")]
        [AllowHtml]
        public string DescriptionEn { get; set; }
        [Display(Name = "Danh mục"), Required(ErrorMessage = "Danh mục buộc phải chọn.")]
        public string NewsCategoryId { get; set; }
        [Display(Name = "Thẻ")]
        public string[] TagPostIds { get; set; }
        [Display(Name = "Kích hoạt"), Required(ErrorMessage = "Kích hoạt thông tin buộc phải chọn")]
        public bool IsDeleted { get; set; }
        [Display(Name = "Hình ảnh đại diện"), Required(ErrorMessage = "Hình ảnh đại diện buộc phải nhập.")]
        public string ImageSrc { get; set; }
        public List<TagPost> TagPosts { get; set; }
        public List<NewsCategory> NewsCategories { get; set; }
        public string NewsCategorySlugVn { get; set; }
        public string NewsCategorySlugEn { get; set; }
    }

    public class NewsEditViewModel : SEOEntityViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Tiêu đề"), Required(ErrorMessage = "Tiêu đề buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        [MyRemoteAttribute("IsNameVnIdAvailable", "News", "", AdditionalFields = "Id", HttpMethod = "POST", ErrorMessage = "Tên này đã tồn tại")]
        public string NameVn { get; set; }
        [Display(Name = "Tiêu đề"), Required(ErrorMessage = "Tiêu đề buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        [MyRemoteAttribute("IsNameEnIdAvailable", "News", "", AdditionalFields = "Id", HttpMethod = "POST", ErrorMessage = "Tên này đã tồn tại")]
        public string NameEn { get; set; }
        [Display(Name = "Đường dẫn"), Required(ErrorMessage = "Đường dẫn buộc phải nhập.")]
        public string SlugVn { get; set; }
        [Display(Name = "Đường dẫn"), Required(ErrorMessage = "Đường dẫn buộc phải nhập.")]
        public string FullSlugVn { get; set; }
        [Display(Name = "Đường dẫn"), Required(ErrorMessage = "Đường dẫn buộc phải nhập.")]
        public string SlugEn { get; set; }
        [Display(Name = "Đường dẫn"), Required(ErrorMessage = "Đường dẫn buộc phải nhập.")]
        public string FullSlugEn { get; set; }
        [Display(Name = "Mô tả ngắn")]
        public string ShortDescriptionVn { get; set; }
        [Display(Name = "Mô tả ngắn")]
        public string ShortDescriptionEn { get; set; }
        [Display(Name = "Mô tả")]
        [AllowHtml]
        public string DescriptionVn { get; set; }
        [Display(Name = "Mô tả")]
        [AllowHtml]
        public string DescriptionEn { get; set; }
        [Display(Name = "Danh mục"), Required(ErrorMessage = "Danh mục buộc phải chọn.")]
        public string NewsCategoryId { get; set; }
        [Display(Name = "Thẻ")]
        public string[] TagPostIds { get; set; }
        [Display(Name = "Kích hoạt"), Required(ErrorMessage = "Kích hoạt thông tin buộc phải chọn")]
        public bool IsDeleted { get; set; }
        [Display(Name = "Hình ảnh đại diện"), Required(ErrorMessage = "Hình ảnh đại diện buộc phải nhập.")]
        public string ImageSrc { get; set; }
        public List<TagPost> TagPosts { get; set; }
        public List<NewsCategory> NewsCategories { get; set; }
        public string NewsCategorySlugVn { get; set; }
        public string NewsCategorySlugEn { get; set; }
    }
}