using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GSID.Admin.ViewModels.MongoModels
{
    public class OpenGraphMetaDataViewModel
    {
        //[Display(Name = "locale")]
        //public string OgLocaleVn { get; set; }//og:locale
        [Display(Name = "Thẻ <meta property='og:title'>")]
        public string OgTitleVn { get; set; }//og:title
        //[Display(Name = "og:url")]
        //public string OgUrlVn { get; set; }//og:url
        [Display(Name = "Thẻ <meta property='og:site_name'>")]
        public string OgSite_nameVn { get; set; }//og:site_name
        [Display(Name = "Thẻ <meta property='og:description'>")]
        public string OgDescriptionVn { get; set; }//og:description

        //[Display(Name = "locale")]
        //public string OgLocaleEn { get; set; }//og:locale
        [Display(Name = "Thẻ <meta property='og:title'>")]
        public string OgTitleEn { get; set; }//og:title
        //[Display(Name = "og:url")]
        //public string OgUrlEn { get; set; }//og:url
        [Display(Name = "Thẻ <meta property='og:site_name'>")]
        public string OgSite_nameEn { get; set; }//og:site_name
        [Display(Name = "Thẻ <meta property='og:description'>")]
        public string OgDescriptionEn { get; set; }//og:description

        [Display(Name = "Thẻ <meta property='og:type'>")]
        public string OgType { get; set; }//og:type website/article
        [Display(Name = "Thẻ <meta property='og:image'>")]
        public string OgImageSrc { get; set; }//og:image
    }
}