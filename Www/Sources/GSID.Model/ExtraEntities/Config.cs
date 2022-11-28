using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Model.ExtraEntities
{
    public class SiteInformationConfig
    {
        public SiteInformationConfig()
        {
            Code = "PARAMETER_SITEINFOMATION_CONFIG";
        }

        public string Code { get; set; }
        public string WhiteLogoSrc { get; set; }
        public string BlackLogoSrc { get; set; }
        public string IconSrc { get; set; }
        public string UrlBackEndSite { get; set; }
        public string UrlAddressSiteMultimedia { get; set; }
        public string PathAddressSiteMultimedia { get; set; }
        public string UrlAddressSite { get; set; }
        public string SiteTitleVn { get; set; }
        public string SiteTitleEn { get; set; }
        public string Prefix { get; set; }
        public string CompanyNameVn { get; set; }
        public string CompanyNameEn { get; set; }
        public string TaxCode { get; set; }
        public string BusinessLicenseVn { get; set; }
        public string BusinessLicenseEn { get; set; }

        public string HotlineVn { get; set; }
        public string DisplayHotlineVn { get; set; }
        public string HotlineEn { get; set; }
        public string DisplayHotlineEn { get; set; }
        public string WorkingTimeVn { get; set; }
        public string WorkingTimeEn { get; set; }
        public string AddressVn { get; set; }
        public string AddressEn { get; set; }
        public string EmailVn { get; set; }
        public string EmailEn { get; set; }
        public string KeywordSEOVn { get; set; }
        public string KeywordSEOEn { get; set; }
        public string DescriptionSEOVn { get; set; }
        public string DescriptionSEOEn { get; set; }
        public string TagHeader { get; set; }
        public string CssHeader { get; set; }
        public string ScriptHeader { get; set; }
        public string TagFooter { get; set; }
        public string ScriptFooter { get; set; }
        public bool IsShowAuthenticationPage { get; set; }
    }
}
