using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GSID.Admin.Attributes;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GSID.Admin.ViewModels.MongoModels
{
    public class PermissionCreateViewModel
    {
        [Display(Name = "Lồng chức năng")]
        public string ParentId { get; set; }
        [Display(Name = "Tên chức năng"), Required(ErrorMessage = "Tên chức năng buộc phải chọn")]
        public string Name { get; set; }
        [Display(Name = "Icon")]
        [AllowHtml]
        public string Icon { get; set; }
        [Display(Name = "Mô tả")]
        public string Description { get; set; }
        [Display(Name = "Loại chức năng"), Required(ErrorMessage = "Loại chức năng buộc phải chọn")]
        public bool? IsMenu { get; set; }
        [Display(Name = "Đường dẫn truy cập")]
        public string Url { get; set; }
        [Display(Name = "Thứ tự sắp xếp")]
        public int? Sort { get; set; }
        public List<Permission> ListParent { get; set; }
        [Display(Name = "Kích hoạt"), Required(ErrorMessage = "Kích hoạt buộc phải chọn")]
        public bool IsDeleted { get; set; }
    }

    public class PermissionEditViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Lồng chức năng")]
        public string ParentId { get; set; }
        [Display(Name = "Tên chức năng"), Required(ErrorMessage = "Tên chức năng buộc phải chọn")]
        public string Name { get; set; }
        [Display(Name = "Icon")]
        [AllowHtml]
        public string Icon { get; set; }
        [Display(Name = "Mô tả")]
        public string Description { get; set; }
        [Display(Name = "Loại chức năng"), Required(ErrorMessage = "Loại chức năng buộc phải chọn")]
        public bool? IsMenu { get; set; }
        [Display(Name = "Đường dẫn truy cập")]
        public string Url { get; set; }
        [Display(Name = "Thứ tự sắp xếp")]
        public int? Sort { get; set; }
        public List<Permission> ListParent { get; set; }
        [Display(Name = "Kích hoạt"), Required(ErrorMessage = "Kích hoạt buộc phải chọn")]
        public bool IsDeleted { get; set; }
    }
}