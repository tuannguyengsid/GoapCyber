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

            Mapper.CreateMap<DistrictCreateViewModel, District>();
            Mapper.CreateMap<DistrictEditViewModel, District>();
            Mapper.CreateMap<FilterMenuConfigViewModel, FilterMenuConfig>();



        }
    }
}