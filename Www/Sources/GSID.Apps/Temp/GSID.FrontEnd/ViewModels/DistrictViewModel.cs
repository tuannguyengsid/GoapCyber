using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GSID.FrontEnd.ViewModels
{
    public class DistrictCreateViewModel
    {
        [Display(Name = "Tên"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        public string NameVn { get; set; }
        [Display(Name = "Tên"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        public string NameEn { get; set; }
        [Display(Name = "Tỉnh/thành phố"), Required(ErrorMessage = "Tỉnh/thành phố buộc phải nhập.")]
        public string ProvinceId { get; set; }

        [Display(Name = "Cụm từ đồng bộ dữ liệu")]
        public string KeywordSync { get; set; }
        [Display(Name = "Kích hoạt"), Required(ErrorMessage = "Kích hoạt thông tin buộc phải chọn")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Thiết lập mặc định"), Required(ErrorMessage = "Thiết lập mặc định thông tin buộc phải chọn")]
        public Nullable<bool> IsDefault { get; set; }
        public List<Province> Provinces { get; set; }
    }

    public class DistrictEditViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Tên"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        public string NameVn { get; set; }
        [Display(Name = "Tên"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        public string NameEn { get; set; }
        [Display(Name = "Tỉnh/thành phố"), Required(ErrorMessage = "Tỉnh/thành phố buộc phải nhập.")]
        public string ProvinceId { get; set; }

        [Display(Name = "Cụm từ đồng bộ dữ liệu")]
        public string KeywordSync { get; set; }
        [Display(Name = "Kích hoạt"), Required(ErrorMessage = "Kích hoạt thông tin buộc phải chọn")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Thiết lập mặc định"), Required(ErrorMessage = "Thiết lập mặc định thông tin buộc phải chọn")]
        public Nullable<bool> IsDefault { get; set; }
        public List<Province> Provinces { get; set; }
    }
}