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
    public class RecruitmentPageViewModel : OpenGraphMetaDataViewModel
    {
        [Display(Name = "Tên trang web"), Required(ErrorMessage = "Tên trang web buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        [AllowHtml]
        public string NameVn { get; set; }
        [Display(Name = "Tên trang web"), Required(ErrorMessage = "Tên trang web buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        [AllowHtml]
        public string NameEn { get; set; }

        [Display(Name = "Tiêu đề"), Required(ErrorMessage = "Tiêu đề buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        //[MyRemoteAttribute("IsTitlePageVnAvailable", "RouteDataUrl", "", AdditionalFields = "RouteDataUrlVnId, RouteDataUrlEnId, TitleEn", HttpMethod = "POST", ErrorMessage = "Tiêu đề này đã tồn tại")]
        [AllowHtml]
        public string TitleVn { get; set; }
        [Display(Name = "Tiêu đề"), Required(ErrorMessage = "Tiêu đề buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        //[MyRemoteAttribute("IsTitlePageEnAvailable", "RouteDataUrl", "", AdditionalFields = "RouteDataUrlEnId, RouteDataUrlVnId, TitleVn", HttpMethod = "POST", ErrorMessage = "Tiêu đề này đã tồn tại")]
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
        public List<RecruitmentPageManagementAdminConfig.RecruitmentPageManagementSectionSliderAdminConfig> Slider { get; set; }



        [Display(Name = "Tiêu đề"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        public string AboutCompanyTitleVn { get; set; }
        [Display(Name = "Tiêu đề"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        public string AboutCompanyTitleEn { get; set; }
        [Display(Name = "Mô tả")]
        public string AboutCompanyDescriptionVn { get; set; }
        [Display(Name = "Mô tả")]
        public string AboutCompanyDescriptionEn { get; set; }

        public List<RecruitmentPageManagementAdminConfig.RecruitmentPageManagementSectionAboutCompanyAdminConfig> AboutCompanyItems { get; set; }


        [Display(Name = "Tiêu đề"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        public string WhyChooseTitleVn { get; set; }
        [Display(Name = "Tiêu đề"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        public string WhyChooseTitleEn { get; set; }
        public List<RecruitmentPageManagementAdminConfig.RecruitmentPageManagementSectionWhyChooseAdminConfig> WhyChooseItems { get; set; }

        [Display(Name = "Tiêu đề"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        public string GalleryTitleVn { get; set; }
        [Display(Name = "Tiêu đề"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        public string GalleryTitleEn { get; set; }
        [Display(Name = "Tiêu đề"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        public string RecruitmentTitleVn { get; set; }
        [Display(Name = "Tiêu đề"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        public string RecruitmentTitleEn { get; set; }
        [Display(Name = "Số lượng tin hiển thị"), Required(ErrorMessage = "Số lượng tin hiển thị buộc phải nhập.")]
        public int RecruitmentPagingItem { get; set; }
    }

    public class CreateSliderRecruitmentPageViewModel
    {
        [Display(Name = "Thứ tự hiển thị"), Required(ErrorMessage = "Thứ tự hiển thị buộc phải nhập.")]
        public int Index { get; set; }
        [Display(Name = "Hình ảnh"), Required(ErrorMessage = "Hình ảnh buộc phải nhập.")]
        public string ImageSliderRecruitmentPageSrc { get; set; }
    }

    public class EditSliderRecruitmentPageViewModelViewModel
    {
        public Guid Id { get; set; }
        [Display(Name = "Thứ tự hiển thị"), Required(ErrorMessage = "Thứ tự hiển thị buộc phải nhập.")]
        public int Index { get; set; }
        [Display(Name = "Hình ảnh"), Required(ErrorMessage = "Hình ảnh buộc phải nhập.")]
        public string ImageSliderRecruitmentPageSrc { get; set; }
    }


    public class CreateAboutCompanyRecruitmentPageViewModel
    {
        [Display(Name = "Số đếm"), Required(ErrorMessage = "Số đếm buộc phải nhập.")]
        public int Count { get; set; }
        [Display(Name = "Tiêu đề"), Required(ErrorMessage = "Tiêu đề buộc phải nhập.")]
        public string NameVn { get; set; }
        [Display(Name = "Tiêu đề"), Required(ErrorMessage = "Tiêu đề buộc phải nhập.")]
        public string NameEn { get; set; }
        [Display(Name = "Thứ tự hiển thị"), Required(ErrorMessage = "Thứ tự hiển thị buộc phải nhập.")]
        public int Index { get; set; }
        [Display(Name = "Hình ảnh"), Required(ErrorMessage = "Hình ảnh buộc phải nhập.")]
        public string ImageAboutCompanySrc { get; set; }
    }

    public class EditAboutCompanyRecruitmentPageViewModel
    {
        public Guid Id { get; set; }
        [Display(Name = "Số đếm"), Required(ErrorMessage = "Số đếm buộc phải nhập.")]
        public int Count { get; set; }
        [Display(Name = "Tiêu đề"), Required(ErrorMessage = "Tiêu đề buộc phải nhập.")]
        public string NameVn { get; set; }
        [Display(Name = "Tiêu đề"), Required(ErrorMessage = "Tiêu đề buộc phải nhập.")]
        public string NameEn { get; set; }
        [Display(Name = "Thứ tự hiển thị"), Required(ErrorMessage = "Thứ tự hiển thị buộc phải nhập.")]
        public int Index { get; set; }
        [Display(Name = "Hình ảnh"), Required(ErrorMessage = "Hình ảnh buộc phải nhập.")]
        public string ImageAboutCompanySrc { get; set; }
    }

    public class CreateWhyChooseRecruitmentPageViewModel
    {
        [Display(Name = "Nội dung"), Required(ErrorMessage = "Nội dung buộc phải nhập.")]
        [AllowHtml]
        public string ShortDescriptionVn { get; set; }
        [Display(Name = "Nội dung"), Required(ErrorMessage = "Nội dung buộc phải nhập.")]
        [AllowHtml]
        public string ShortDescriptionEn { get; set; }
        [Display(Name = "Thứ tự hiển thị"), Required(ErrorMessage = "Thứ tự hiển thị buộc phải nhập.")]
        public int Index { get; set; }
        [Display(Name = "Hình ảnh"), Required(ErrorMessage = "Hình ảnh buộc phải nhập.")]
        public string ImageWhyChooseRecruitmentSrc { get; set; }
    }

    public class EditWhyChooseRecruitmentPageViewModelViewModel
    {
        public Guid Id { get; set; }
        [Display(Name = "Nội dung"), Required(ErrorMessage = "Nội dung buộc phải nhập.")]
        [AllowHtml]
        public string ShortDescriptionVn { get; set; }
        [Display(Name = "Nội dung"), Required(ErrorMessage = "Nội dung buộc phải nhập.")]
        [AllowHtml]
        public string ShortDescriptionEn { get; set; }
        [Display(Name = "Thứ tự hiển thị"), Required(ErrorMessage = "Thứ tự hiển thị buộc phải nhập.")]
        public int Index { get; set; }
        [Display(Name = "Hình ảnh"), Required(ErrorMessage = "Hình ảnh buộc phải nhập.")]
        public string ImageWhyChooseRecruitmentSrc { get; set; }
    }




}