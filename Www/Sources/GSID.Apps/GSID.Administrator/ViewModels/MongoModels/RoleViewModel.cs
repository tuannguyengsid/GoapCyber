using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GSID.Admin.Attributes;
using System.Linq;
using System.Web;

namespace GSID.Admin.ViewModels.MongoModels
{
    public class RoleCreateViewModel
    {
        [Display(Name = "Tên quyền"), Required(ErrorMessage = "Tên quyền bắt buộc phải nhập")]
        public string Name { get; set; }
        [Display(Name = "Mô tả")]
        public string Description { get; set; }
        [Display(Name = "Loại quyền"), Required(ErrorMessage = "Loại quyền")]
        public bool? IsSysAdmin { get; set; }
        [Display(Name = "Chọn chức năng phân quyền"), Required(ErrorMessage = "Chức năng phân quyền buộc phải chọn")]
        public string ListPermisison { get; set; }
        public string Listundetermined { get; set; }
        [Display(Name = "Kích hoạt"), Required(ErrorMessage = "Kích hoạt buộc phải chọn")]
        public bool IsDeleted { get; set; }
    }
    public class RoleEditViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Tên quyền"), Required(ErrorMessage = "Tên quyền bắt buộc phải nhập")]
        public string Name { get; set; }
        [Display(Name = "Mô tả")]
        public string Description { get; set; }
        [Display(Name = "Loại quyền"), Required(ErrorMessage = "Loại quyền")]
        public bool? IsSysAdmin { get; set; }
        [Display(Name = "Chọn chức năng phân quyền"), Required(ErrorMessage = "Chức năng phân quyền buộc phải chọn")]
        public string ListPermisison { get; set; }
        public string Listundetermined { get; set; }
        [Display(Name = "Kích hoạt"), Required(ErrorMessage = "Kích hoạt buộc phải chọn")]
        public bool IsDeleted { get; set; }
    }
}