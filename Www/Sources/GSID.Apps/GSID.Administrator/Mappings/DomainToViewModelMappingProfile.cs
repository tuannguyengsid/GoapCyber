using AutoMapper;
using GSID.Model.MongodbModels;
using GSID.Admin.ViewModels.MongoModels;
using GSID.Admin.Areas.PageManagement.ViewModels;
using GSID.Model.ExtraEntities;
using GSID.Admin.Helpers;

namespace GSID.Admin.Mappings
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "DomainToViewModelMappings"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<UserCreateViewModel, User>();
            Mapper.CreateMap<UserEditViewModel, User>();
            Mapper.CreateMap<BoroughCreateViewModel, Borough>();
            Mapper.CreateMap<BoroughEditViewModel, Borough>();
            Mapper.CreateMap<CountryCreateViewModel, Country>();
            Mapper.CreateMap<CountryEditViewModel, Country>();
            Mapper.CreateMap<ProductCreateViewModel, Product>().IgnoreAllNonExisting();
            Mapper.CreateMap<ProductEditViewModel, Product>().IgnoreAllNonExisting();
            Mapper.CreateMap<QuickLinkCreateViewModel, QuickLink>();
            Mapper.CreateMap<QuickLinkEditViewModel, QuickLink>();
            Mapper.CreateMap<SocialNetworkConfigCreateViewModel, SocialNetworkConfig>();
            Mapper.CreateMap<SocialNetworkConfigEditViewModel, SocialNetworkConfig>();
            Mapper.CreateMap<CustomerCreateViewModel, Customer>();
            Mapper.CreateMap<CustomerEditViewModel, Customer>();
            Mapper.CreateMap<ProductOverviewBlockCreateViewModel, ProductOverviewBlock>();
            Mapper.CreateMap<ProductOverviewBlockEditViewModel, ProductOverviewBlock>();
            Mapper.CreateMap<ProductCategoryCreateViewModel, ProductCategory>();
            Mapper.CreateMap<ProductCategoryEditViewModel, ProductCategory>();
            Mapper.CreateMap<NewsCategoryCreateViewModel, NewsCategory>();
            Mapper.CreateMap<NewsCategoryEditViewModel, NewsCategory>();
            Mapper.CreateMap<NewsCreateViewModel, News>();
            Mapper.CreateMap<NewsEditViewModel, News>();
            Mapper.CreateMap<DistrictCreateViewModel, District>();
            Mapper.CreateMap<DistrictEditViewModel, District>();
            Mapper.CreateMap<ProvinceCreateViewModel, Province>();
            Mapper.CreateMap<ProvinceEditViewModel, Province>();
            Mapper.CreateMap<WardCreateViewModel, Ward>();
            Mapper.CreateMap<WardEditViewModel, Ward>();
            Mapper.CreateMap<RoleCreateViewModel, Role>();
            Mapper.CreateMap<RoleEditViewModel, Role>();
            Mapper.CreateMap<PermissionCreateViewModel, Permission>();
            Mapper.CreateMap<PermissionEditViewModel, Permission>();
            Mapper.CreateMap<SiteCreateViewModel, Site>();
            Mapper.CreateMap<SiteEditViewModel, Site>();
            Mapper.CreateMap<DepartmentCreateViewModel, Department>();
            Mapper.CreateMap<DepartmentEditViewModel, Department>();
            Mapper.CreateMap<PositionCreateViewModel, Position>();
            Mapper.CreateMap<PositionEditViewModel, Position>();
            Mapper.CreateMap<MenuNodeCreateViewModel, MenuNode>();
            Mapper.CreateMap<MenuNodeEditViewModel, MenuNode>();
            Mapper.CreateMap<TagPostCreateViewModel, TagPost>();
            Mapper.CreateMap<TagPostEditViewModel, TagPost>();
            Mapper.CreateMap<CareerCreateViewModel, Career>();
            Mapper.CreateMap<CareerEditViewModel, Career>();
            Mapper.CreateMap<PartnerCreateViewModel, Partner>();
            Mapper.CreateMap<PartnerEditViewModel, Partner>();
            Mapper.CreateMap<RecruitmentCreateViewModel, Recruitment>();
            Mapper.CreateMap<RecruitmentEditViewModel, Recruitment>();
            Mapper.CreateMap<RecruitmentTagCreateViewModel, RecruitmentTag>();
            Mapper.CreateMap<RecruitmentTagEditViewModel, RecruitmentTag>();
            Mapper.CreateMap<ContactMessageCreateViewModel, ContactMessage>();
            Mapper.CreateMap<ContactMessageEditViewModel, ContactMessage>();
            Mapper.CreateMap<ContactCreateViewModel, Contact>();
            Mapper.CreateMap<ContactEditViewModel, Contact>();
            Mapper.CreateMap<CurriculumVitaeCreateViewModel, CurriculumVitae>();
            Mapper.CreateMap<CurriculumVitaeEditViewModel, CurriculumVitae>();
            Mapper.CreateMap<ImageLibraryCreateViewModel, ImageLibrary>();
            Mapper.CreateMap<ImageLibraryEditViewModel, ImageLibrary>();
            Mapper.CreateMap<ImageCreateViewModel, Image>();
            Mapper.CreateMap<ImageEditViewModel, Image>();
            Mapper.CreateMap<MailTemplateCreateViewModel, MailTemplate>();
            Mapper.CreateMap<MailTemplateEditViewModel, MailTemplate>();
            Mapper.CreateMap<ProjectCategoryCreateViewModel, ProjectCategory>();
            Mapper.CreateMap<ProjectCategoryEditViewModel, ProjectCategory>();
            Mapper.CreateMap<ProjectSkillCreateViewModel, ProjectSkill>();
            Mapper.CreateMap<ProjectSkillEditViewModel, ProjectSkill>();



            Mapper.CreateMap<HomePageViewModel, HomePageManagementAdminConfig>();
            Mapper.CreateMap<ProductDetailPageViewModel, ProductDetailPageManagementAdminConfig>();
            Mapper.CreateMap<NewsPageViewModel, NewsPageManagementAdminConfig>();
            Mapper.CreateMap<CategoryPageViewModel, CategoryPageManagementAdminConfig>();
            Mapper.CreateMap<NewsDetailPageViewModel, NewsDetailPageManagementAdminConfig>();
            Mapper.CreateMap<FooterContentViewModel, FooterPageManagementAdminConfig>();
            Mapper.CreateMap<AboutPageViewModel, AboutPageManagementAdminConfig>();
            Mapper.CreateMap<ContactPageViewModel, ContactUsPageManagementAdminConfig>();
            Mapper.CreateMap<PostPageViewModel, PostPageManagementAdminConfig>();
            Mapper.CreateMap<PostDetailPageViewModel, PostDetailPageManagementAdminConfig>();
            Mapper.CreateMap<RecruitmentPageViewModel, RecruitmentPageManagementAdminConfig>();
            Mapper.CreateMap<RecruitmentDetailPageViewModel, RecruitmentDetailPageManagementAdminConfig>();
            Mapper.CreateMap<ImageLibraryPageViewModel, ImageLibraryPageManagementAdminConfig>();
            Mapper.CreateMap<PopupSubcribesViewModel, PopupSubcribesPageManagementAdminConfig>();



        }
    }
}