using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;

namespace GSID.FrontEnd.Mappings
{
    public class AutoMapperConfigurationControlPanel
    {
        public static void Configure()
        {
            Mapper.Initialize(x =>
            {
                x.AddProfile<DomainToViewModelMappingProfile>();
                x.AddProfile<ViewModelToDomainMappingProfile>();
            });
        }
    }
}