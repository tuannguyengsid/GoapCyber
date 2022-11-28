using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GSID.Admin.Attributes;
using System.Linq;
using System.Web;

namespace GSID.Admin.ViewModels.MongoModels
{
    public class PartnerFilterViewModel
    {
        public List<Partner> List { get; set; }
        public string Keyword { get; set; }

        [Display(Name = "Thời gian tạo")]
        public string BeginAddDateString { get; set; }
        public string EndAddDateString { get; set; }
    }

    public class PartnerCreateViewModel
    {
        [Display(Name = "Tên"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        [MyRemoteAttribute("IsNameVnAvailable", "Partner", "", HttpMethod = "POST", ErrorMessage = "Tên này đã tồn tại")]
        public string NameVn { get; set; }
        [Display(Name = "Tên"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        [MyRemoteAttribute("IsNameEnAvailable", "Partner", "", HttpMethod = "POST", ErrorMessage = "Tên này đã tồn tại")]
        public string NameEn { get; set; }
        [Display(Name = "Liên kết")]
        public string Url { get; set; }
        [Display(Name = "Thứ tự hiển thị"), Required(ErrorMessage = "Thứ tự hiển thị buộc phải nhập.")]
        public int Sort { get; set; }
        [Display(Name = "Kích hoạt"), Required(ErrorMessage = "Kích hoạt thông tin buộc phải chọn")]
        public bool IsDeleted { get; set; }
        public string ImageSrc { get; set; }
    }

    public class PartnerEditViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Tên"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        [MyRemoteAttribute("IsNameVnIdAvailable", "Partner", "", AdditionalFields = "Id", HttpMethod = "POST", ErrorMessage = "Tên này đã tồn tại")]
        public string NameVn { get; set; }
        [Display(Name = "Tên"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        [MyRemoteAttribute("IsNameEnIdAvailable", "Partner", "", AdditionalFields = "Id", HttpMethod = "POST", ErrorMessage = "Tên này đã tồn tại")]
        public string NameEn { get; set; }
        [Display(Name = "Liên kết")]
        public string Url { get; set; }
        [Display(Name = "Thứ tự hiển thị"), Required(ErrorMessage = "Thứ tự hiển thị buộc phải nhập.")]
        public int Sort { get; set; }
        [Display(Name = "Kích hoạt"), Required(ErrorMessage = "Kích hoạt thông tin buộc phải chọn")]
        public bool IsDeleted { get; set; }
        public string ImageSrc { get; set; }
    }
}