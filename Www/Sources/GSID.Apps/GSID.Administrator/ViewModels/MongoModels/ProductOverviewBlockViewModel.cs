using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GSID.Admin.Attributes;
using System.Web.Mvc;
using static GSID.Model.MongodbModels.ProductOverviewBlock;

namespace GSID.Admin.ViewModels.MongoModels
{
    public class ProductOverviewBlockCreateViewModel
    {
        public string ProductId { get; set; }
        [Display(Name = "Tiêu đề"), Required(ErrorMessage = "Tiêu đề buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        public string Name { get; set; }
        [Display(Name = "Mô tả")]
        [AllowHtml]
        public string Description { get; set; }
        public IsLanguage Language { get; set; }
        [Display(Name = "Thứ tự hiển thị"), Required(ErrorMessage = "Thứ tự hiển thị buộc phải nhập.")]
        public int Sort { get; set; }
        [Display(Name = "Kích hoạt"), Required(ErrorMessage = "Kích hoạt thông tin buộc phải chọn")]
        public bool IsDeleted { get; set; }
        public string ImagePrdBlockSrc { get; set; }
    }

    public class ProductOverviewBlockEditViewModel
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        [Display(Name = "Tiêu đề"), Required(ErrorMessage = "Tiêu đề buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        public string Name { get; set; }
        [Display(Name = "Mô tả")]
        [AllowHtml]
        public string Description { get; set; }
        public IsLanguage Language { get; set; }
        public string ImagePrdBlockSrc { get; set; }
        [Display(Name = "Thứ tự hiển thị"), Required(ErrorMessage = "Thứ tự hiển thị buộc phải nhập.")]
        public int Sort { get; set; }
        [Display(Name = "Kích hoạt"), Required(ErrorMessage = "Kích hoạt thông tin buộc phải chọn")]
        public bool IsDeleted { get; set; }
    }
}