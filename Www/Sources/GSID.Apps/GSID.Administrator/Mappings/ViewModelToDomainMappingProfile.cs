using AutoMapper;
using GSID.Model.MongodbModels;
using GSID.Admin.ViewModels.MongoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GSID.Model.ExtraEntities;
using GSID.Admin.Areas.PageManagement.ViewModels;
using GSID.Admin.Helpers;

namespace GSID.Admin.Mappings
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "ViewModelToDomainMappings"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<User, UserCreateViewModel>();
            Mapper.CreateMap<User, UserEditViewModel>();
            Mapper.CreateMap<Borough, BoroughCreateViewModel>();
            Mapper.CreateMap<Borough, BoroughEditViewModel>();
            Mapper.CreateMap<Product, ProductCreateViewModel>();
            Mapper.CreateMap<Product, ProductEditViewModel>().IgnoreAllNonExisting();
            Mapper.CreateMap<SocialNetworkConfig, SocialNetworkConfigCreateViewModel>();
            Mapper.CreateMap<SocialNetworkConfig, SocialNetworkConfigEditViewModel>();
            Mapper.CreateMap<QuickLink, QuickLinkCreateViewModel>();
            Mapper.CreateMap<QuickLink, QuickLinkEditViewModel>();
            Mapper.CreateMap<Customer, CustomerCreateViewModel>();
            Mapper.CreateMap<Customer, CustomerEditViewModel>();
            Mapper.CreateMap<ProductOverviewBlock, ProductOverviewBlockCreateViewModel>();
            Mapper.CreateMap<ProductOverviewBlock, ProductOverviewBlockEditViewModel>();
            Mapper.CreateMap<ProductCategory, ProductCategoryCreateViewModel>();
            Mapper.CreateMap<ProductCategory, ProductCategoryEditViewModel>();
            Mapper.CreateMap<NewsCategory, NewsCategoryCreateViewModel>();
            Mapper.CreateMap<NewsCategory, NewsCategoryEditViewModel>();
            Mapper.CreateMap<News, NewsCreateViewModel>();
            Mapper.CreateMap<News, NewsEditViewModel>();
            Mapper.CreateMap<Country, CountryCreateViewModel>();
            Mapper.CreateMap<Country, CountryEditViewModel>();
            Mapper.CreateMap<District, DistrictCreateViewModel>();
            Mapper.CreateMap<District, DistrictEditViewModel>();
            Mapper.CreateMap<Province, ProvinceCreateViewModel >();
            Mapper.CreateMap<Province, ProvinceEditViewModel>();
            Mapper.CreateMap<Ward, WardCreateViewModel >();
            Mapper.CreateMap<Ward, WardEditViewModel>();
            Mapper.CreateMap<Role, RoleCreateViewModel>();
            Mapper.CreateMap<Role, RoleEditViewModel>();
            Mapper.CreateMap<Permission, PermissionCreateViewModel>();
            Mapper.CreateMap<Permission, PermissionEditViewModel>();
            Mapper.CreateMap<Site, SiteCreateViewModel>();
            Mapper.CreateMap<Site, SiteEditViewModel>();
            Mapper.CreateMap<Department, DepartmentCreateViewModel>();
            Mapper.CreateMap<Department, DepartmentEditViewModel>();
            Mapper.CreateMap<Position, PositionCreateViewModel>();
            Mapper.CreateMap<Position, PositionEditViewModel>();
            Mapper.CreateMap<MenuNode, MenuNodeCreateViewModel>();
            Mapper.CreateMap<MenuNode, MenuNodeEditViewModel>();
            Mapper.CreateMap<TagPost, TagPostCreateViewModel>();
            Mapper.CreateMap<TagPost, TagPostEditViewModel>();
            Mapper.CreateMap<Partner, PartnerCreateViewModel>();
            Mapper.CreateMap<Partner, PartnerEditViewModel>();
            Mapper.CreateMap<Career, CareerCreateViewModel>();
            Mapper.CreateMap<Career, CareerEditViewModel>();
            Mapper.CreateMap<Recruitment, RecruitmentCreateViewModel>();
            Mapper.CreateMap<Recruitment, RecruitmentEditViewModel>();
            Mapper.CreateMap<RecruitmentTag, RecruitmentTagCreateViewModel>();
            Mapper.CreateMap<RecruitmentTag, RecruitmentTagEditViewModel>();
            Mapper.CreateMap<ContactMessage, ContactMessageCreateViewModel>();
            Mapper.CreateMap<ContactMessage, ContactMessageEditViewModel>();
            Mapper.CreateMap<Contact, ContactCreateViewModel>();
            Mapper.CreateMap<Contact, ContactEditViewModel>();
            Mapper.CreateMap<CurriculumVitae, CurriculumVitaeCreateViewModel>();
            Mapper.CreateMap<CurriculumVitae, CurriculumVitaeEditViewModel>();
            Mapper.CreateMap<ImageLibrary, ImageLibraryCreateViewModel>();
            Mapper.CreateMap<ImageLibrary, ImageLibraryEditViewModel>();
            Mapper.CreateMap<Image, ImageCreateViewModel>();
            Mapper.CreateMap<Image, ImageEditViewModel>();
            Mapper.CreateMap<MailTemplate, MailTemplateCreateViewModel>();
            Mapper.CreateMap<MailTemplate, MailTemplateEditViewModel>();
            Mapper.CreateMap<ProjectCategory, ProjectCategoryCreateViewModel>();
            Mapper.CreateMap<ProjectCategory, ProjectCategoryEditViewModel>();
            Mapper.CreateMap<ProjectSkill, ProjectSkillCreateViewModel>();
            Mapper.CreateMap<ProjectSkill, ProjectSkillEditViewModel>();


            Mapper.CreateMap<HomePageManagementAdminConfig, HomePageViewModel>();
            Mapper.CreateMap<ProductDetailPageManagementAdminConfig, ProductDetailPageViewModel>();
            Mapper.CreateMap<NewsPageManagementAdminConfig, NewsPageViewModel>();
            Mapper.CreateMap<CategoryPageManagementAdminConfig, CategoryPageViewModel>();
            Mapper.CreateMap<NewsDetailPageManagementAdminConfig, NewsDetailPageViewModel>();
            Mapper.CreateMap<FooterPageManagementAdminConfig, FooterContentViewModel>();
            Mapper.CreateMap<AboutPageManagementAdminConfig, AboutPageViewModel>();
            Mapper.CreateMap<ContactUsPageManagementAdminConfig, ContactPageViewModel>();
            Mapper.CreateMap<PostPageManagementAdminConfig, PostPageViewModel>();
            Mapper.CreateMap<PostDetailPageManagementAdminConfig, PostDetailPageViewModel>();
            Mapper.CreateMap<RecruitmentPageManagementAdminConfig, RecruitmentPageViewModel>();
            Mapper.CreateMap<RecruitmentDetailPageManagementAdminConfig, RecruitmentDetailPageViewModel>();
            Mapper.CreateMap<ImageLibraryPageManagementAdminConfig, ImageLibraryPageViewModel>();
            Mapper.CreateMap<PopupSubcribesPageManagementAdminConfig, PopupSubcribesViewModel>();
        } 
    }
}