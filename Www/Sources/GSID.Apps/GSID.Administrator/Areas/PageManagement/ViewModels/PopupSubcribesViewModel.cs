using GSID.Model.MongodbModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GSID.Admin.Attributes;
using System.Web.Mvc;

namespace GSID.Admin.Areas.PageManagement.ViewModels
{
    public class PopupSubcribesViewModel
    {
        [Display(Name = "Tiêu đề"), Required(ErrorMessage = "Tiêu đề buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        [AllowHtml]
        public string NameVn { get; set; }
        [Display(Name = "Tiêu đề"), Required(ErrorMessage = "Tiêu đề buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        [AllowHtml]
        public string NameEn { get; set; }

        [Display(Name = "Mô tả"), Required(ErrorMessage = "Mô tả buộc phải nhập.")]
        [AllowHtml]
        public string DescriptionVn { get; set; }
        [Display(Name = "Mô tả"), Required(ErrorMessage = "Mô tả buộc phải nhập.")]
        [AllowHtml]
        public string DescriptionEn { get; set; }

        public string BackgroundVnSrc { get; set; }
        public string BackgroundEnSrc { get; set; }

        //[Display(Name = "Nội dung thông báo Cookie"), Required(ErrorMessage = "Nội dung thông báo Cookie buộc phải nhập.")]
        //public string CookieWarningVn { get; set; }
        //[Display(Name = "Nội dung thông báo Cookie"), Required(ErrorMessage = "Nội dung thông báo Cookie buộc phải nhập.")]
        //public string CookieWarningEn { get; set; }

        [Display(Name = "Kích hoạt"), Required(ErrorMessage = "Kích hoạt buộc phải chọn.")]
        public bool IsActive { get; set; }

    }
}