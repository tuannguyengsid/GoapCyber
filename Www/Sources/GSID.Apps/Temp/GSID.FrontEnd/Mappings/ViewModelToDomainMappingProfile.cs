using AutoMapper;
using GSID.Model.MongodbModels;
using GSID.FrontEnd.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GSID.Model.ExtraEntities;

namespace GSID.FrontEnd.Mappings
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

            Mapper.CreateMap<District, DistrictCreateViewModel>();
            Mapper.CreateMap<District, DistrictEditViewModel>();
            Mapper.CreateMap<FilterMenuConfig, FilterMenuConfigViewModel>();
        } 
    }
}