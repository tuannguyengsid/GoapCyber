using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GSID.Admin.Attributes;
using System.Linq;
using System.Web;
using static GSID.Model.MongodbModels.Site;

namespace GSID.Admin.ViewModels.MongoModels
{
    public class SiteFilterViewModel
    {
        public List<Site> List { get; set; }
        public string Keyword { get; set; }

        [Display(Name = "Thời gian tạo")]
        public string BeginAddDateString { get; set; }
        public string EndAddDateString { get; set; }
    }

    public class SiteCreateViewModel
    {
        [Display(Name = "Tên"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        [MyRemoteAttribute("IsNameVnAvailable", "Site", "", HttpMethod = "POST", ErrorMessage = "Tên này đã tồn tại")]
        public string NameVn { get; set; }
        [Display(Name = "Tên"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        [MyRemoteAttribute("IsNameEnAvailable", "Site", "", HttpMethod = "POST", ErrorMessage = "Tên này đã tồn tại")]
        public string NameEn { get; set; }
        [Display(Name = "Mô tả")]
        public string DescriptionVn { get; set; }
        [Display(Name = "Mô tả")]
        public string DescriptionEn { get; set; }
        [Display(Name = "Quốc gia"), Required(ErrorMessage = "Quốc gia buộc phải chọn.")]
        public string CountryId { get; set; }
        [Display(Name = "Tỉnh/thành phố"), Required(ErrorMessage = "Tỉnh/thành phố buộc phải chọn.")]
        public string ProvinceId { get; set; }
        [Display(Name = "Quận/huyện")]
        public string DistrictId { get; set; }
        [Display(Name = "Địa chỉ"), Required(ErrorMessage = "Địa chỉ buộc phải nhập.")]
        public string AddressVn { get; set; }
        [Display(Name = "Địa chỉ"), Required(ErrorMessage = "Địa chỉ buộc phải nhập.")]
        public string AddressEn { get; set; }
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Email"), RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Xin vui lòng nhập chính xác địa chỉ email")]
        public string Email { get; set; }
        [Display(Name = "Kích hoạt"), Required(ErrorMessage = "Kích hoạt thông tin buộc phải chọn")]
        public bool IsDeleted { get; set; }
        public List<Province> Provinces { get; set; }
        public List<District> Districts { get; set; }
        public List<Country> Countries { get; set; }
    }

    public class SiteEditViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Tên"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        [MyRemoteAttribute("IsNameVnIdAvailable", "Site", "", AdditionalFields = "Id", HttpMethod = "POST", ErrorMessage = "Tên này đã tồn tại")]
        public string NameVn { get; set; }
        [Display(Name = "Tên"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        [MyRemoteAttribute("IsNameEnIdAvailable", "Site", "", AdditionalFields = "Id", HttpMethod = "POST", ErrorMessage = "Tên này đã tồn tại")]
        public string NameEn { get; set; }
        [Display(Name = "Mô tả")]
        public string DescriptionVn { get; set; }
        [Display(Name = "Mô tả")]
        public string DescriptionEn { get; set; }
        [Display(Name = "Quốc gia"), Required(ErrorMessage = "Quốc gia buộc phải chọn.")]
        public string CountryId { get; set; }
        [Display(Name = "Tỉnh/thành phố"), Required(ErrorMessage = "Tỉnh/thành phố buộc phải chọn.")]
        public string ProvinceId { get; set; }
        [Display(Name = "Quận/huyện")]
        public string DistrictId { get; set; }
        [Display(Name = "Địa chỉ"), Required(ErrorMessage = "Địa chỉ buộc phải nhập.")]
        public string AddressVn { get; set; }
        [Display(Name = "Địa chỉ"), Required(ErrorMessage = "Địa chỉ buộc phải nhập.")]
        public string AddressEn { get; set; }
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Email"), RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Xin vui lòng nhập chính xác địa chỉ email")]
        public string Email { get; set; }
        [Display(Name = "Kích hoạt"), Required(ErrorMessage = "Kích hoạt thông tin buộc phải chọn")]
        public bool IsDeleted { get; set; }
        public List<Province> Provinces { get; set; }
        public List<District> Districts { get; set; }
        public List<Country> Countries { get; set; }
        public string ImageSrc { get; set; }
        public string ImageChange { get; set; }
    }
}