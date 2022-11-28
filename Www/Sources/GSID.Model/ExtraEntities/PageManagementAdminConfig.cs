using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using GSID.Data.Mongodb.FrameworkCore;
using GSID.Data.Mongodb;
using GSID.Model.MongodbModels;

namespace GSID.Model.ExtraEntities
{

    public class NewsPageManagementAdminConfig
    {
        public NewsPageManagementAdminConfig()
        {
            Code = "PARAMETER_PAGEMANAGEMENT_NEW_CONFIG";
            SlugVn = "/news";
            SlugEn = "/news";

            ActionClient = "Index";
            ControllerClient = "News";
            AreaClient = "";
        }
        public string Code { get; set; }
        public string ActionClient { get; set; }
        public string ControllerClient { get; set; }
        public string AreaClient { get; set; }
        public string MenuActiveId { get; set; }
        public string NameVn { get; set; }
        public string NameEn { get; set; }
        public string SlugVn { get; set; }
        public string SlugEn { get; set; }
    }

    public class NewsDetailPageManagementAdminConfig
    {
        public NewsDetailPageManagementAdminConfig()
        {
            Code = "PARAMETER_PAGEMANAGEMENT_NEWSDETAIL_CONFIG";
            SlugVn = "/";
            SlugEn = "/";

            ActionClient = "NewsDetail";
            ControllerClient = "News";
            AreaClient = "";
        }
        public string Code { get; set; }
        public string ActionClient { get; set; }
        public string ControllerClient { get; set; }
        public string AreaClient { get; set; }
        public string NameVn { get; set; }
        public string NameEn { get; set; }
        public string SlugVn { get; set; }
        public string SlugEn { get; set; }
        public int RelatedItem { get; set; }
        public string BackgroundSrc { get; set; }
    }
    public class CategoryPageManagementAdminConfig
    {
        public CategoryPageManagementAdminConfig()
        {
            Code = "PARAMETER_PAGEMANAGEMENT_CATEGORY_CONFIG";
            SlugVn = "/Product";
            SlugEn = "/Product";

            ActionClient = "Index";
            ControllerClient = "Category";
            AreaClient = "";
        }
        public string Code { get; set; }
        public string ActionClient { get; set; }
        public string ControllerClient { get; set; }
        public string AreaClient { get; set; }
        public string MenuActiveId { get; set; }
        public string NameVn { get; set; }
        public string NameEn { get; set; }
        public string SlugVn { get; set; }
        public string SlugEn { get; set; }
        public string BackgroundSrc { get; set; }
        public string TitleVn { get; set; }
        public string TitleEn { get; set; }
    }

    public class RecruitmentPageManagementAdminConfig
    {
        public RecruitmentPageManagementAdminConfig()
        {
            Code = "PARAMETER_PAGEMANAGEMENT_RECRUITMENT_CONFIG";
        }
        public string Code { get; set; }
        public string MenuActiveId { get; set; }
        public string NameVn { get; set; }
        public string NameEn { get; set; }
        public string RouteDataUrlVnId { get; set; }

        [BsonIgnore]
        public RouteDataUrl RouteDataUrlVn { get; set; }
        public string RouteDataUrlEnId { get; set; }

        [BsonIgnore]
        public RouteDataUrl RouteDataUrlEn { get; set; }

        public List<RecruitmentPageManagementAdminConfig.RecruitmentPageManagementSectionSliderAdminConfig> Slider { get; set; }

        public class RecruitmentPageManagementSectionSliderAdminConfig
        {
            public Guid Id { get; set; }
            public string ImageSrc { get; set; }
            public int Index { get; set; }
        }

        public string AboutCompanyTitleVn { get; set; }
        public string AboutCompanyTitleEn { get; set; }
        public string AboutCompanyDescriptionVn { get; set; }
        public string AboutCompanyDescriptionEn { get; set; }

        public List<RecruitmentPageManagementAdminConfig.RecruitmentPageManagementSectionAboutCompanyAdminConfig> AboutCompanyItems { get; set; }

        public class RecruitmentPageManagementSectionAboutCompanyAdminConfig
        {
            public Guid Id { get; set; }
            public int Count { get; set; }
            public string NameVn { get; set; }
            public string NameEn { get; set; }
            public string ImageSrc { get; set; }
            public int Index { get; set; }
        }


        public string WhyChooseTitleVn { get; set; }
        public string WhyChooseTitleEn { get; set; }

        public List<RecruitmentPageManagementAdminConfig.RecruitmentPageManagementSectionWhyChooseAdminConfig> WhyChooseItems { get; set; }

        public class RecruitmentPageManagementSectionWhyChooseAdminConfig
        {
            public Guid Id { get; set; }
            public string ShortDescriptionVn { get; set; }
            public string ShortDescriptionEn { get; set; }
            public string ImageSrc { get; set; }
            public int Index { get; set; }
        }

        public string GalleryTitleVn { get; set; }
        public string GalleryTitleEn { get; set; }
        public string RecruitmentTitleVn { get; set; }
        public string RecruitmentTitleEn { get; set; }
        public int RecruitmentPagingItem { get; set; }

    }

    public class HomePageManagementAdminConfig
    {
        public HomePageManagementAdminConfig()
        {
            Code = "PARAMETER_PAGEMANAGEMENT_HOME_CONFIG";
        }

        public enum ProductTrendType : int
        {
            LatestUpdates = 1,
            Config = 2
        }
        public string Code { get; set; }
        public string TitleVn { get; set; }
        public string TitleEn { get; set; }
        public string RouteDataUrlVnId { get; set; }
        public string RouteDataUrlEnId { get; set; }

        [BsonIgnore]
        public RouteDataUrl RouteDataUrlVn { get; set; }

        [BsonIgnore]
        public RouteDataUrl RouteDataUrlEn { get; set; }
        public string MenuActiveId { get; set; }
        public List<HomePageManagementBannerAdminConfig> Banners { get; set; }

        public string ProductCategoryTitleVn { get; set; }
        public string ProductCategoryTitleEn { get; set; }
        public string WhyChooseTitleVn { get; set; }
        public string WhyChooseTitleEn { get; set; }
        public string SectionWhyChooseBackgroundSrc { get; set; }

        public List<HomePageManagementWhyChooseUsAdminConfig> WhyChooseUss { get; set; }

        public string CareerTitleVn { get; set; }
        public string CareerTitleEn { get; set; }
        public int CareerTakeItem { get; set; }
        public string CareerBackgroundSrc { get; set; }

        public string PostTitleVn { get; set; }
        public string PostTitleEn { get; set; }

        public string CustomerTitleVn { get; set; }
        public string CustomerTitleEn { get; set; }
        public string CustomerDescriptionVn { get; set; }
        public string CustomerDescriptionEn { get; set; }
        #region not map
        //[BsonIgnore]

        #endregion
    }

    public class HomePageManagementBannerAdminConfig
    {
        public Guid Id { get; set; }
        public string NameVn { get; set; }
        public string NameEn { get; set; }
        public string DescriptionVn { get; set; }
        public string DescriptionEn { get; set; }
        public string ImageSrc { get; set; }
        public int Type { get; set; }
        public string Source { get; set; }
        public int Index { get; set; }
    }

    public class HomePageManagementWhyChooseUsAdminConfig
    {
        public Guid Id { get; set; }
        public string NameVn { get; set; }
        public string NameEn { get; set; }
        public string DescriptionVn { get; set; }
        public string DescriptionEn { get; set; }
        public int Index { get; set; }
    }

    public class AboutPageManagementAdminConfig
    {
        public AboutPageManagementAdminConfig()
        {
            Code = "PARAMETER_PAGEMANAGEMENT_ABOUT_CONFIG";
        }
        
        public string Code { get; set; }

        public string RouteDataUrlVnId { get; set; }
        public string RouteDataUrlEnId { get; set; }

        [BsonIgnore]
        public RouteDataUrl RouteDataUrlVn { get; set; }

        [BsonIgnore]
        public RouteDataUrl RouteDataUrlEn { get; set; }
        public string NameVn { get; set; }
        public string NameEn { get; set; }
        public string DescriptionPageVn { get; set; }
        public string DescriptionPageEn { get; set; }

        public string BreakScrumBackgroundSrc { get; set; }
        public string MenuActiveId { get; set; }


        #region section about
        public string SectionAboutTitleNameVn { get; set; }
        public string SectionAboutTitleNameEn { get; set; }
        public string SectionAboutContentVn { get; set; }
        public string SectionAboutContentEn { get; set; }
        public string SectionAboutContentMoreVn { get; set; }
        public string SectionAboutContentMoreEn { get; set; }
        public string SectionAboutPositionCEOVn { get; set; }
        public string SectionAboutPositionCEOEn { get; set; }
        public string SectionAboutFullNameCEOVn { get; set; }
        public string SectionAboutFullNameCEOEn { get; set; }
        public string AvatarCEOImageSrc { get; set; }
        #endregion


        #region section Vision

        public string SectionVisionTitleNameVn { get; set; }
        public string SectionVisionTitleNameEn { get; set; }
        public string SectionVisionBackgroundSrc { get; set; }
        public List<AboutPageManagementSectionVisionAdminConfig> Visions { get; set; }

        public class AboutPageManagementSectionVisionAdminConfig
        {
            public Guid Id { get; set; }
            public string NameVn { get; set; }
            public string NameEn { get; set; }
            public string DescriptionVn { get; set; }
            public string DescriptionEn { get; set; }
            public int Index { get; set; }
        }
        #endregion


        #region section corevalue

        public string SectionCoreValueTitleNameVn { get; set; }
        public string SectionCoreValueTitleNameEn { get; set; }
        public List<AboutPageManagementSectionCoreValueAdminConfig> CoreValues { get; set; }

        public class AboutPageManagementSectionCoreValueAdminConfig
        {
            public Guid Id { get; set; }
            public string NameVn { get; set; }
            public string NameEn { get; set; }
            public string DescriptionVn { get; set; }
            public string DescriptionEn { get; set; }
            public int Index { get; set; }
            public string ImageSrc { get; set; }
        }
        #endregion
        #region not map
        //[BsonIgnore]

        #endregion
    }

    public class PopupSubcribesPageManagementAdminConfig
    {
        public PopupSubcribesPageManagementAdminConfig()
        {
            Code = "PARAMETER_PAGEMANAGEMENT_POPUPSUBCRIBES_CONFIG";
        }

        public string Code { get; set; }
        public string NameVn { get; set; }
        public string NameEn { get; set; }
        public string DescriptionVn { get; set; }
        public string DescriptionEn { get; set; }
        public string CookieWarningVn { get; set; }
        public string CookieWarningEn { get; set; }
        public string BackgroundVnSrc { get; set; }
        public string BackgroundEnSrc { get; set; }
        public bool IsActive { get; set; }
    }

    public class ContactUsPageManagementAdminConfig
    {
        public ContactUsPageManagementAdminConfig()
        {
            Code = "PARAMETER_PAGEMANAGEMENT_CONTACT_CONFIG";
        }

        public string Code { get; set; }
        public string NameVn { get; set; }
        public string NameEn { get; set; }
        public string DescriptionPageVn { get; set; }
        public string DescriptionPageEn { get; set; }
        public string RouteDataUrlVnId { get; set; }

        [BsonIgnore]
        public RouteDataUrl RouteDataUrlVn { get; set; }

        public string RouteDataUrlEnId { get; set; }
        [BsonIgnore]
        public RouteDataUrl RouteDataUrlEn { get; set; }
        public string BreakScrumBackgroundSrc { get; set; }
        public string MenuActiveId { get; set; }


        #region section Contact
        public string SectionContactUsTitleNameVn { get; set; }
        public string SectionContactUsTitleNameEn { get; set; }
        public string SectionContactUsDescriptionVn { get; set; }
        public string SectionContactUsDescriptionEn { get; set; }
        public string PhoneDescriptionVn { get; set; }
        public string PhoneDescriptionEn { get; set; }
        public string EmailDescriptionVn { get; set; }
        public string EmailDescriptionEn { get; set; }
        public string GoogleMapDescriptionVn { get; set; }
        public string GoogleMapDescriptionEn { get; set; }
        public string GoogleMapUrl { get; set; }
        public string GoogleMapTitleUrlVn { get; set; }
        public string GoogleMapTitleUrlEn { get; set; }
        public string GoogleMapEmbedCodeVn { get; set; }
        public string GoogleMapEmbedCodeEn { get; set; }
        #endregion

        #region not map
        //[BsonIgnore]

        #endregion
    }

    public class PostPageManagementAdminConfig
    {
        public PostPageManagementAdminConfig()
        {
            Code = "PARAMETER_PAGEMANAGEMENT_POST_CONFIG";
        }

        public string Code { get; set; }
        public string NameVn { get; set; }
        public string NameEn { get; set; }
        public string DescriptionPageVn { get; set; }
        public string DescriptionPageEn { get; set; }
        public string RouteDataUrlVnId { get; set; }
        public string RouteDataUrlEnId { get; set; }

        [BsonIgnore]
        public RouteDataUrl RouteDataUrlVn { get; set; }

        [BsonIgnore]
        public RouteDataUrl RouteDataUrlEn { get; set; }
        public string BreakScrumBackgroundSrc { get; set; }
        public string MenuActiveId { get; set; }

        #region not map
        //[BsonIgnore]

        #endregion
    }

    public class ImageLibraryPageManagementAdminConfig
    {
        public ImageLibraryPageManagementAdminConfig()
        {
            Code = "PARAMETER_PAGEMANAGEMENT_IMAGE_LIBRARY_CONFIG";
        }

        public string Code { get; set; }
        public string NameVn { get; set; }
        public string NameEn { get; set; }
        public string RouteDataUrlVnId { get; set; }

        [BsonIgnore]
        public RouteDataUrl RouteDataUrlVn { get; set; }
        public string RouteDataUrlEnId { get; set; }

        [BsonIgnore]
        public RouteDataUrl RouteDataUrlEn { get; set; }
        public string BreakScrumBackgroundSrc { get; set; }
        public string MenuActiveId { get; set; }

        #region not map
        //[BsonIgnore]

        #endregion
    }


    public class ProductDetailPageManagementAdminConfig
    {
        public ProductDetailPageManagementAdminConfig()
        {
            Code = "PARAMETER_PAGEMANAGEMENT_PRODUCT_DETAIL_CONFIG";
            //SlugVn = "/vn/thu-vien-hinh-anh";
            //SlugEn = "/en/gallery";

            ActionClient = "Index";
            ControllerClient = "Product";
            AreaClient = "";
        }

        public string Code { get; set; }
        public string ActionClient { get; set; }
        public string ControllerClient { get; set; }
        public string AreaClient { get; set; }
        //public string NameVn { get; set; }
        //public string NameEn { get; set; }
        //public string SlugVn { get; set; }
        //public string SlugEn { get; set; }
        //public string BreakScrumBackgroundSrc { get; set; }
        public string MenuActiveId { get; set; }

        #region not map
        //[BsonIgnore]

        #endregion
    }


    public class PostDetailPageManagementAdminConfig
    {
        public PostDetailPageManagementAdminConfig()
        {
            Code = "PARAMETER_PAGEMANAGEMENT_POST_DETAIL_CONFIG";
            SlugVn = "";
            SlugEn = "";

            ActionClient = "Detail";
            ControllerClient = "Post";
            AreaClient = "";
        }

        public string Code { get; set; }
        public string ActionClient { get; set; }
        public string ControllerClient { get; set; }
        public string AreaClient { get; set; }
        public string NameVn { get; set; }
        public string NameEn { get; set; }
        public string DescriptionPageVn { get; set; }
        public string DescriptionPageEn { get; set; }
        public string SlugVn { get; set; }
        public string SlugEn { get; set; }
        public string BreakScrumBackgroundSrc { get; set; }
        public string MenuActiveId { get; set; }
        public int RelastItem { get; set; }

        #region not map
        //[BsonIgnore]

        #endregion
    }


    public class RecruitmentDetailPageManagementAdminConfig
    {
        public RecruitmentDetailPageManagementAdminConfig()
        {
            Code = "PARAMETER_PAGEMANAGEMENT_RECRUITEMENT_DETAIL_CONFIG";
            SlugVn = "";
            SlugEn = "";

            ActionClient = "Detail";
            ControllerClient = "Recruitment";
            AreaClient = "";
        }

        public string Code { get; set; }
        public string ActionClient { get; set; }
        public string ControllerClient { get; set; }
        public string AreaClient { get; set; }
        public string NameVn { get; set; }
        public string NameEn { get; set; }
        public string SlugVn { get; set; }
        public string SlugEn { get; set; }
        public string BreakScrumBackgroundSrc { get; set; }
        public string MenuActiveId { get; set; }
        public int RelastItem { get; set; }

        #region not map
        //[BsonIgnore]

        #endregion
    }

    public class FooterPageManagementAdminConfig
    {
        public FooterPageManagementAdminConfig()
        {
            Code = "PARAMETER_PAGEMANAGEMENT_FOOTER_CONFIG";
        }

        public string Code { get; set; }
        public string ContentVn { get; set; }
        public string ContentEn { get; set; }

        public string ImageSrc { get; set; }
        public string CookieMessageVn { get; set; }
        public string CookieMessageEn { get; set; }
    }

    public class SocialNetworkManagementAdminConfig
    {
        public SocialNetworkManagementAdminConfig()
        {
            Code = "PARAMETER_PAGEMANAGEMENT_SOCIALNETWORK_CONFIG";
        }

        public string Code { get; set; }
        public List<SocialNetworkConfig> Social { get; set; }

    }

    public class SocialNetworkConfig
    {
        public enum SocialIsGroup : int
        {
            None = -1,
            Facebook = 1,
            Youtube = 2,
            Instagram = 3,
            Pinterest = 6,
            Linkedin = 7,
            Tiktok = 8,
            Zalo = 9,
        }
        public enum SocialIsRedirect : int
        {
            None = -1,
            Redirect = 1,
            Share = 2
        }
        public Guid Id { get; set; }
        public SocialIsGroup Group { get; set; }
        public SocialIsRedirect IsRedirect { get; set; }
        public string Slug { get; set; }
        public int Sort { get; set; }
        public bool IsFooter { get; set; }
        public bool IsDeleted { get; set; }
    }
}
