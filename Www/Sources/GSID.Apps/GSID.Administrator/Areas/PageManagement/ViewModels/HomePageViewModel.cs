using GSID.Model.ExtraEntities;
using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using GSID.Admin.Attributes;
using static GSID.Model.ExtraEntities.HomePageManagementAdminConfig;
using GSID.Admin.ViewModels.MongoModels;

namespace GSID.Admin.Areas.PageManagement.ViewModels
{
    public class HomePageViewModel: OpenGraphMetaDataViewModel
    {
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



        public string MenuActiveId { get; set; }
        public List<MenuNode> MenuNodes { get; set; }
        public List<HomePageManagementBannerAdminConfig> Banners { get; set; }

        [Display(Name = "Tiêu đề")]
        public string ProductCategoryTitleVn { get; set; }
        [Display(Name = "Tiêu đề")]
        public string ProductCategoryTitleEn { get; set; }


        [Display(Name = "Tiêu đề")]
        public string WhyChooseTitleVn { get; set; }
        [Display(Name = "Tiêu đề")]
        public string WhyChooseTitleEn { get; set; }
        public string SectionWhyChooseBackgroundSrc { get; set; }
        public List<HomePageManagementWhyChooseUsAdminConfig> WhyChooseUss { get; set; }


        [Display(Name = "Tiêu đề")]
        public string PostTitleVn { get; set; }
        [Display(Name = "Tiêu đề")]
        public string PostTitleEn { get; set; }

        [Display(Name = "Tiêu đề")]
        public string CareerTitleVn { get; set; }

        [Display(Name = "Tiêu đề")]
        public string CareerTitleEn { get; set; }
        [Display(Name = "Số lượng tin tuyển dụng hiển thị")]
        public int CareerTakeItem { get; set; }
        public string CareerBackgroundSrc { get; set; }

        [Display(Name = "Tiêu đề")]
        public string CustomerTitleVn { get; set; }
        [Display(Name = "Tiêu đề")]
        public string CustomerTitleEn { get; set; }
        [Display(Name = "Mô tả")]
        public string CustomerDescriptionVn { get; set; }
        [Display(Name = "Mô tả")]
        public string CustomerDescriptionEn { get; set; }

        public List<ProductCategory> ProductCategories { get; set; }
    }

    public class CreateBannerHomePageViewModel
    {
        [Display(Name = "Tên(Vn)")]
        [AllowHtml]
        public string NameVn { get; set; }
        [Display(Name = "Tên(En)")]
        [AllowHtml]
        public string NameEn { get; set; }
        [Display(Name = "Mô tả")]
        [AllowHtml]
        public string DescriptionVn { get; set; }
        [Display(Name = "Mô tả")]
        [AllowHtml]
        public string DescriptionEn { get; set; }
        [Display(Name = "Loại nội dung"), Required(ErrorMessage = "Loại nội dung buộc phải chọn.")]
        public int Type { get; set; }
        [Display(Name = "Youtube Id")]
        public string Source { get; set; }

        [Display(Name = "Thứ tự hiển thị"), Required(ErrorMessage = "Thứ tự hiển thị buộc phải nhập.")]
        public int Index { get; set; }
        public string ImageBannerHomePageSrc { get; set; }
    }

    public class EditBannerHomePageViewModel
    {
        public Guid Id { get; set; }
        [Display(Name = "Tên(Vn)")]
        [AllowHtml]
        public string NameVn { get; set; }
        [Display(Name = "Tên(En)")]
        [AllowHtml]
        public string NameEn { get; set; }
        [Display(Name = "Mô tả")]
        [AllowHtml]
        public string DescriptionVn { get; set; }
        [Display(Name = "Mô tả")]
        [AllowHtml]
        public string DescriptionEn { get; set; }
        [Display(Name = "Loại nội dung"), Required(ErrorMessage = "Loại nội dung buộc phải chọn.")]
        public int Type { get; set; }
        [Display(Name = "Youtube Id")]
        public string Source { get; set; }

        [Display(Name = "Thứ tự hiển thị"), Required(ErrorMessage = "Thứ tự hiển thị buộc phải nhập.")]
        public int Index { get; set; }
        public string ImageBannerHomePageSrc { get; set; }
    }






    public class CreateWhyChooseUsHomePageViewModel
    {
        [Display(Name = "Tên(Vn)"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        [AllowHtml]
        public string NameVn { get; set; }
        [Display(Name = "Tên(En)"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        [AllowHtml]
        public string NameEn { get; set; }
        [Display(Name = "Mô tả")]
        [AllowHtml]
        public string DescriptionVn { get; set; }
        [Display(Name = "Mô tả")]
        [AllowHtml]
        public string DescriptionEn { get; set; }
        [Display(Name = "Thứ tự hiển thị"), Required(ErrorMessage = "Thứ tự hiển thị buộc phải nhập.")]
        public int Index { get; set; }
    }

    public class EditWhyChooseUsHomePageViewModel
    {
        public Guid Id { get; set; }
        [Display(Name = "Tên(Vn)"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        [AllowHtml]
        public string NameVn { get; set; }
        [Display(Name = "Tên(En)"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        [AllowHtml]
        public string NameEn { get; set; }
        [Display(Name = "Mô tả")]
        [AllowHtml]
        public string DescriptionVn { get; set; }
        [Display(Name = "Mô tả")]
        [AllowHtml]
        public string DescriptionEn { get; set; }
        [Display(Name = "Thứ tự hiển thị"), Required(ErrorMessage = "Thứ tự hiển thị buộc phải nhập.")]
        public int Index { get; set; }
        public string ImageSrc { get; set; }
        public string ImageChange { get; set; }
    }
}