using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GSID.Admin.Attributes;
using System.Linq;
using System.Web;

namespace GSID.Admin.ViewModels.MongoModels
{
    public class WardFilterViewModel
    {
        public List<Ward> List { get; set; }
        public string Keyword { get; set; }

        [Display(Name = "Thời gian tạo")]
        public string BeginAddDateString { get; set; }
        public string EndAddDateString { get; set; }
        public List<Country> Countries { get; set; }
        public string[] CountryId { get; set; }
        public List<Province> Provinces { get; set; }
        public string[] ProvinceId { get; set; }
        public List<District> Districts { get; set; }
        public string[] DistrictId { get; set; }
    }
    public class WardCreateViewModel
    {
        [Display(Name = "Tên"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        public string NameVn { get; set; }
        [Display(Name = "Tên"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        public string NameEn { get; set; }

        [Display(Name = "Quận/huyện"), Required(ErrorMessage = "Quận/huyện buộc phải chọn.")]
        public string DistrictId { get; set; }
        [Display(Name = "Cụm từ đồng bộ dữ liệu")]
        public string KeywordSync { get; set; }
        [Display(Name = "Kích hoạt"), Required(ErrorMessage = "Kích hoạt thông tin buộc phải chọn")]
        public bool IsDeleted { get; set; }
        [Display(Name = "Thiết lập mặc định"), Required(ErrorMessage = "Thiết lập mặc định thông tin buộc phải chọn")]
        public Nullable<bool> IsDefault { get; set; }

        public List<District> Districts { get; set; }
    }
    public class WardEditViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Tên"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        public string NameVn { get; set; }
        [Display(Name = "Tên"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        public string NameEn { get; set; }
        [Display(Name = "Quận/huyện"), Required(ErrorMessage = "Quận/huyện buộc phải chọn.")]
        public string DistrictId { get; set; }
        [Display(Name = "Cụm từ đồng bộ dữ liệu")]
        public string KeywordSync { get; set; }
        [Display(Name = "Kích hoạt"), Required(ErrorMessage = "Kích hoạt thông tin buộc phải chọn")]
        public bool IsDeleted { get; set; }
        [Display(Name = "Thiết lập mặc định"), Required(ErrorMessage = "Thiết lập mặc định thông tin buộc phải chọn")]
        public Nullable<bool> IsDefault { get; set; }

        public List<District> Districts { get; set; }
    }
}