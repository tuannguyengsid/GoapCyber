using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using GSID.Admin.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GSID.Admin.ViewModels.MongoModels
{
    public class BoroughCreateViewModel
    {
        [Display(Name = "Tên"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        public string Name { get; set; }
        [Display(Name = "Cụm từ đồng bộ dữ liệu")]
        public string KeywordSync { get; set; }
        [Display(Name = "Phường"), Required(ErrorMessage = "Phường buộc phải nhập.")]
        public string WardId { get; set; }
        [Display(Name = "Kích hoạt"), Required(ErrorMessage = "Kích hoạt thông tin buộc phải chọn")]
        public bool IsDeleted { get; set; }
        [Display(Name = "Thiết lập mặc định"), Required(ErrorMessage = "Thiết lập mặc định thông tin buộc phải chọn")]
        public Nullable<bool> IsDefault { get; set; }

        public List<Ward> Wards { get; set; }
    }

    public class BoroughEditViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Tên"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        public string Name { get; set; }
        [Display(Name = "Cụm từ đồng bộ dữ liệu")]
        public string KeywordSync { get; set; }
        [Display(Name = "Phường"), Required(ErrorMessage = "Phường buộc phải nhập.")]
        public string WardId { get; set; }
        [Display(Name = "Kích hoạt"), Required(ErrorMessage = "Kích hoạt thông tin buộc phải chọn")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Thiết lập mặc định"), Required(ErrorMessage = "Thiết lập mặc định thông tin buộc phải chọn")]
        public Nullable<bool> IsDefault { get; set; }

        public List<Ward> Wards { get; set; }
    }
}