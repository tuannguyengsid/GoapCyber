using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GSID.Admin.Attributes;
using System.Linq;
using System.Web;

namespace GSID.Admin.ViewModels.MongoModels
{
    public class DistrictFilterViewModel
    {
        public List<District> List { get; set; }
        public string Keyword { get; set; }

        [Display(Name = "Thời gian tạo")]
        public string BeginAddDateString { get; set; }
        public string EndAddDateString { get; set; }
        public List<Country> Countries { get; set; }
        public string[] CountryId { get; set; }
        public List<Province> Provinces { get; set; }
        public string[] ProvinceId { get; set; }
    }
    public class DistrictFormFilterProvinceViewModel
    {
        public List<Province> Provinces { get; set; }
        public string[] ProvinceId { get; set; }
    }

    public class DistrictCreateViewModel
    {
        public string Name { get; set; }
        [Display(Name = "Tên(Vi)"), Required(ErrorMessage = "Tên(Vi) buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        public string NameVn { get; set; }
        [Display(Name = "Tên(En)"), Required(ErrorMessage = "Tên(En) buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        public string NameEn { get; set; }
        [Display(Name = "Quốc gia"), Required(ErrorMessage = "Quốc gia buộc phải nhập.")]
        public string CountryId { get; set; }
        [Display(Name = "Tỉnh/thành phố"), Required(ErrorMessage = "Tỉnh/thành phố buộc phải nhập.")]
        public string ProvinceId { get; set; }
        [Display(Name = "Kích hoạt"), Required(ErrorMessage = "Kích hoạt thông tin buộc phải chọn")]
        public bool IsDeleted { get; set; }
        public List<Province> Provinces { get; set; }
        public List<Country> Countries { get; set; }
    }

    public class DistrictEditViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        [Display(Name = "Tên(Vi)"), Required(ErrorMessage = "Tên(Vi) buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        public string NameVn { get; set; }
        [Display(Name = "Tên(En)"), Required(ErrorMessage = "Tên(En) buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        public string NameEn { get; set; }
        [Display(Name = "Quốc gia"), Required(ErrorMessage = "Quốc gia buộc phải nhập.")]
        public string CountryId { get; set; }
        [Display(Name = "Tỉnh/thành phố"), Required(ErrorMessage = "Tỉnh/thành phố buộc phải nhập.")]
        public string ProvinceId { get; set; }
        [Display(Name = "Kích hoạt"), Required(ErrorMessage = "Kích hoạt thông tin buộc phải chọn")]
        public bool IsDeleted { get; set; }
        public List<Province> Provinces { get; set; }
        public List<Country> Countries { get; set; }
    }
    public class DistrictFilterModel
    {
        [Display(Name = "Quận/huyện"), Required(ErrorMessage = "Quận/huyện buộc phải nhập.")]
        public string DistrictId { get; set; }
        public List<District> Districts { get; set; }
    }
}