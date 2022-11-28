using GSID.Model.ExtraEntities;
using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GSID.Admin.Attributes;
using System.Web.Mvc;
using GSID.Admin.ViewModels.MongoModels;

namespace GSID.Admin.Areas.PageManagement.ViewModels
{
    public class ContactPageViewModel : OpenGraphMetaDataViewModel
    {
        [Display(Name = "Tên trang"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        [AllowHtml]
        public string NameVn { get; set; }
        [Display(Name = "Tên trang"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        [AllowHtml]
        public string NameEn { get; set; }
        [Display(Name = "Mô tả trang")]
        public string DescriptionPageVn { get; set; }
        [Display(Name = "Mô tả trang")]
        public string DescriptionPageEn { get; set; }


        [Display(Name = "Tiêu đề"), Required(ErrorMessage = "Tiêu đề buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        //[MyRemoteAttribute("IsTitlePageVnAvailable", "RouteDataUrl", "", AdditionalFields = "RouteDataUrlVnId, TitleEn", HttpMethod = "POST", ErrorMessage = "Tiêu đề này đã tồn tại")]
        [AllowHtml]
        public string TitleVn { get; set; }
        [Display(Name = "Tiêu đề"), Required(ErrorMessage = "Tiêu đề buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        //[MyRemoteAttribute("IsTitlePageEnAvailable", "RouteDataUrl", "", AdditionalFields = "RouteDataUrlEnId, TitleVn", HttpMethod = "POST", ErrorMessage = "Tiêu đề này đã tồn tại")]
        [AllowHtml]
        public string TitleEn { get; set; }
        public string RouteDataUrlVnId { get; set; }
        public string RouteDataUrlEnId { get; set; }
        [Display(Name = "Đường dẫn"), Required(ErrorMessage = "Đường dẫn buộc phải nhập.")]
        [MyRemoteAttribute("IsUrlVnIdAvailable", "RouteDataUrl", "", AdditionalFields = "RouteDataUrlVnId, SlugEn", HttpMethod = "POST", ErrorMessage = "Đường dẫn này đã tồn tại")]
        public string SlugVn { get; set; }
        [Display(Name = "Đường dẫn"), Required(ErrorMessage = "Đường dẫn buộc phải nhập.")]
        [MyRemoteAttribute("IsUrlEnIdAvailable", "RouteDataUrl", "", AdditionalFields = "RouteDataUrlEnId, SlugVn", HttpMethod = "POST", ErrorMessage = "Đường dẫn này đã tồn tại")]
        public string SlugEn { get; set; }

        [Display(Name = "Thẻ meta keyword")]
        //[MyRemoteAttribute("IsKeywordPageVnAvailable", "RouteDataUrl", "", AdditionalFields = "RouteDataUrlVnId, KeywordEn", HttpMethod = "POST", ErrorMessage = "Meta keyword này đã tồn tại")]
        public string KeywordVn { get; set; }
        [Display(Name = "Thẻ meta keyword")]
        //[MyRemoteAttribute("IsKeywordPageEnAvailable", "RouteDataUrl", "", AdditionalFields = "RouteDataUrlEnId, KeywordVn", HttpMethod = "POST", ErrorMessage = "Meta keyword này đã tồn tại")]
        public string KeywordEn { get; set; }
        [Display(Name = "Thẻ meta description")]
        //[MyRemoteAttribute("IsDescriptionPageVnAvailable", "RouteDataUrl", "", AdditionalFields = "RouteDataUrlVnId, DescriptionEn", HttpMethod = "POST", ErrorMessage = "Meta description này đã tồn tại")]
        public string DescriptionVn { get; set; }
        [Display(Name = "Thẻ meta description")]
        //[MyRemoteAttribute("IsDescriptionPageEnAvailable", "RouteDataUrl", "", AdditionalFields = "RouteDataUrlEnId, DescriptionVn", HttpMethod = "POST", ErrorMessage = "Meta description này đã tồn tại")]
        public string DescriptionEn { get; set; }
        [Display(Name = "H1")]
        public string H1Vn { get; set; }
        [Display(Name = "H1")]
        public string H1En { get; set; }
        [Display(Name = "H2")]
        public string H2Vn { get; set; }
        [Display(Name = "H2")]
        public string H2En { get; set; }
        [Display(Name = "H3")]
        public string H3Vn { get; set; }
        [Display(Name = "H3")]
        public string H3En { get; set; }
        [Display(Name = "H4")]
        public string H4Vn { get; set; }
        [Display(Name = "H4")]
        public string H4En { get; set; }

        [Display(Name = "H5")]
        public string H5Vn { get; set; }
        [Display(Name = "H5")]
        public string H5En { get; set; }

        [Display(Name = "H6")]
        public string H6Vn { get; set; }
        [Display(Name = "H6")]
        public string H6En { get; set; }
        [Display(Name = "Hình nền sitemap")]
        public string BreakScrumBackgroundSrc { get; set; }
        public string MenuActiveId { get; set; }
        public List<MenuNode> MenuNodes { get; set; }


        [Display(Name = "Tiêu đề")]
        public string SectionContactUsTitleNameVn { get; set; }
        [Display(Name = "Tiêu đề")]
        public string SectionContactUsTitleNameEn { get; set; }
        [Display(Name = "Mô tả")]
        public string SectionContactUsDescriptionVn { get; set; }
        [Display(Name = "Mô tả")]
        public string SectionContactUsDescriptionEn { get; set; }
        [Display(Name = "Mô tả")]
        public string PhoneDescriptionVn { get; set; }
        [Display(Name = "Mô tả")]
        public string PhoneDescriptionEn { get; set; }
        [Display(Name = "Mô tả")]
        public string EmailDescriptionVn { get; set; }
        [Display(Name = "Mô tả")]
        public string EmailDescriptionEn { get; set; }
        [Display(Name = "Mô tả")]
        public string GoogleMapDescriptionVn { get; set; }
        [Display(Name = "Mô tả")]
        public string GoogleMapDescriptionEn { get; set; }
        [Display(Name = "Đường dẫn Google map")]
        public string GoogleMapUrl { get; set; }
        [Display(Name = "Tiêu đề liên kết Google map")]
        public string GoogleMapTitleUrlVn { get; set; }
        [Display(Name = "Tiêu đề liên kết Google map")]
        public string GoogleMapTitleUrlEn { get; set; }
        [Display(Name = "Mã nhúng")]
        [AllowHtml]
        public string GoogleMapEmbedCodeVn { get; set; }
        [Display(Name = "Mã nhúng")]
        [AllowHtml]
        public string GoogleMapEmbedCodeEn { get; set; }

    }
}