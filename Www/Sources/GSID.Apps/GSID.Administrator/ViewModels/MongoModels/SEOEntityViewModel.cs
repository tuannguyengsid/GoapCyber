using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GSID.Admin.Attributes;
using System.Web.Mvc;

namespace GSID.Admin.ViewModels.MongoModels
{
    public class SEOEntityViewModel: OpenGraphMetaDataViewModel
    {
        [Display(Name = "Tiêu đề thẻ <title>")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        //[MyRemoteAttribute("IsTitleVnAvailable", "RouteDataUrl", "", AdditionalFields = "RouteDataUrlVnId, TitleSEOVn", HttpMethod = "POST", ErrorMessage = "Tiêu đề này đã tồn tại")]
        [AllowHtml]
        public string TitleSEOVn { get; set; }
        [Display(Name = "Tiêu đề thẻ <title>")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        //[MyRemoteAttribute("IsTitleEnAvailable", "RouteDataUrl", "", AdditionalFields = "RouteDataUrlEnId, TitleSEOVn", HttpMethod = "POST", ErrorMessage = "Tiêu đề này đã tồn tại")]
        [AllowHtml]
        public string TitleSEOEn { get; set; }
        public string RouteDataUrlVnId { get; set; }
        public string RouteDataUrlEnId { get; set; }

        public bool IsUrlRequiredVn { get; set; }
        public bool IsUrlRequiredEn { get; set; }

        [Display(Name = "Đường dẫn")]
        [RegularExpression(@"^\/[0-9a-zA-Z]*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$", ErrorMessage = "Vui lòng nhập đúng định dạng: /duong-dan-url")]
        [MyRemoteAttribute("IsUrlVnIdAvailable", "RouteDataUrl", "", AdditionalFields = "RouteDataUrlVnId, SlugSEOEn, TitleSEOVn, IsUrlRequiredVn", HttpMethod = "POST", ErrorMessage = "Đường dẫn này đã tồn tại")]
        public string SlugSEOVn { get; set; }
        [Display(Name = "Đường dẫn")]
        [RegularExpression(@"^\/[0-9a-zA-Z]*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$", ErrorMessage = "Vui lòng nhập đúng định dạng: /duong-dan-url")]
        [MyRemoteAttribute("IsUrlEnIdAvailable", "RouteDataUrl", "", AdditionalFields = "RouteDataUrlEnId, SlugSEOVn, TitleSEOEn, IsUrlRequiredEn", HttpMethod = "POST", ErrorMessage = "Đường dẫn này đã tồn tại")]
        public string SlugSEOEn { get; set; }

        [Display(Name = "Thẻ <meta name='keywords'>")]
        public string KeywordSEOVn { get; set; }
        [Display(Name = "Thẻ <meta name='keywords'>")]
        public string KeywordSEOEn { get; set; }
        [Display(Name = "Thẻ <meta name='description'>")]
        public string DescriptionSEOVn { get; set; }
        [Display(Name = "Thẻ <meta name='description'>")]
        public string DescriptionSEOEn { get; set; }

        [Display(Name = "Thẻ <h1>")]
        public string H1SEOVn { get; set; }
        [Display(Name = "Thẻ <h1>")]
        public string H1SEOEn { get; set; }
        [Display(Name = "Thẻ <h2>")]
        public string H2SEOVn { get; set; }
        [Display(Name = "Thẻ <h2>")]
        public string H2SEOEn { get; set; }
        [Display(Name = "Thẻ <h3>")]
        public string H3SEOVn { get; set; }
        [Display(Name = "Thẻ <h3>")]
        public string H3SEOEn { get; set; }
        [Display(Name = "Thẻ <h4>")]
        public string H4SEOVn { get; set; }
        [Display(Name = "Thẻ <h4>")]
        public string H4SEOEn { get; set; }

        [Display(Name = "Thẻ <h5>")]
        public string H5SEOVn { get; set; }
        [Display(Name = "Thẻ <h5>")]
        public string H5SEOEn { get; set; }

        [Display(Name = "Thẻ <h6>")]
        public string H6SEOVn { get; set; }
        [Display(Name = "Thẻ <h6>")]
        public string H6SEOEn { get; set; }

        [Display(Name = "Thẻ Meta mở rộng")]
        [AllowHtml]
        public string HtmlMetaRawVn { get; set; }

        [Display(Name = "Thẻ Meta mở rộng")]
        [AllowHtml]
        public string HtmlMetaRawEn { get; set; }
    }
}