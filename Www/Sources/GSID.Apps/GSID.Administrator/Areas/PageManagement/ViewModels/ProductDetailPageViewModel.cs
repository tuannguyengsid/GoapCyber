using GSID.Model.MongodbModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GSID.Admin.Attributes;
using System.Web.Mvc;

namespace GSID.Admin.Areas.PageManagement.ViewModels
{
    public class ProductDetailPageViewModel
    {
        //[Display(Name = "Tên(Vn)"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        //[StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        //[AllowHtml]
        //public string NameVn { get; set; }
        //[Display(Name = "Tên(En)"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        //[StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        //[AllowHtml]
        //public string NameEn { get; set; }
        //[Display(Name = "Đường dẫn(Vn)")]
        //public string SlugVn { get; set; }
        //[Display(Name = "Đường dẫn(En)")]
        //public string SlugEn { get; set; }
        public string MenuActiveId { get; set; }
        public List<MenuNode> MenuNodes { get; set; }
    }
}