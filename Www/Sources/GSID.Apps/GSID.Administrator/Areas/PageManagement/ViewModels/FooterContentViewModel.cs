using GSID.Model.ExtraEntities;
using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GSID.Admin.Attributes;
using System.Web.Mvc;
using static GSID.Model.ExtraEntities.SocialNetworkConfig;

namespace GSID.Admin.Areas.PageManagement.ViewModels
{
    public class FooterContentViewModel
    {
        [Display(Name = "Nội dung(Vn)")]
        [AllowHtml]
        public string ContentVn { get; set; }
        [Display(Name = "Nội dung(En)")]
        [AllowHtml]
        public string ContentEn { get; set; }

        public string ImageSrc { get; set; }
        public string ImageChange { get; set; }

        [Display(Name = "Nội dung(Vn)")]
        [AllowHtml]
        public string CookieMessageVn { get; set; }
        [Display(Name = "Nội dung(En)")]
        [AllowHtml]
        public string CookieMessageEn { get; set; }
    }

    public class SocialNetworkConfigCreateViewModel
    {
        [Display(Name = "Mạng xã hội"), Required(ErrorMessage = "Mạng xã hội buộc phải chọn.")]
        public SocialIsGroup Group { get; set; }
        //[Display(Name = "Dạng chia sẻ"), Required(ErrorMessage = "Dạng chia sẻ buộc phải chọn.")]
        //public SocialIsRedirect IsRedirect { get; set; }
        [Display(Name = "Đường dẫn")]
        [AllowHtml]
        public string Slug { get; set; }
        [Display(Name = "Thứ tự hiển thị"), Required(ErrorMessage = "Thứ tự hiển thị buộc phải nhập.")]
        public int Sort { get; set; }
        //[Display(Name = "Hiện chân trang"), Required(ErrorMessage = "Hiện chân trang thông tin buộc phải chọn")]
        //public bool IsFooter { get; set; }
        [Display(Name = "Kích hoạt"), Required(ErrorMessage = "Kích hoạt thông tin buộc phải chọn")]
        public bool IsDeleted { get; set; }
    }

    public class SocialNetworkConfigEditViewModel
    {
        public Guid Id { get; set; }
        [Display(Name = "Mạng xã hội"), Required(ErrorMessage = "Mạng xã hội buộc phải chọn.")]
        public SocialIsGroup Group { get; set; }
        //[Display(Name = "Dạng chia sẻ"), Required(ErrorMessage = "Dạng chia sẻ buộc phải chọn.")]
        //public SocialIsRedirect IsRedirect { get; set; }
        [Display(Name = "Đường dẫn")]
        [AllowHtml]
        public string Slug { get; set; }
        [Display(Name = "Thứ tự hiển thị"), Required(ErrorMessage = "Thứ tự hiển thị buộc phải nhập.")]
        public int Sort { get; set; }
        //[Display(Name = "Hiện chân trang"), Required(ErrorMessage = "Hiện chân trang thông tin buộc phải chọn")]
        //public bool IsFooter { get; set; }
        [Display(Name = "Kích hoạt"), Required(ErrorMessage = "Kích hoạt thông tin buộc phải chọn")]
        public bool IsDeleted { get; set; }
    }

}