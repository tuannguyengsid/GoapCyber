using Autofac;
using Autofac.Integration.Mvc;
using GSID.Data.Mongodb.FrameworkCore;
using GSID.Data.Mongodb.MongoCore;
using GSID.Model.MongodbModels;
using GSID.Service;
using GSID.Web.Core.Authentication;
using GSID.Admin.DI.Autofac.Modules;
using GSID.Admin.Mappings;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MvcSiteMapProvider.Loader;
using MvcSiteMapProvider.Web.Mvc;
using MvcSiteMapProvider.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using GSID.Service.MongoRepositories.Service;

namespace GSID.Admin
{
    public static class Bootstrapper
    {
        public static void Run()
        {
            SetAutofacContainer();
            //Configure AutoMapper
            AutoMapperConfigurationControlPanel.Configure();
        }
        private static void SetAutofacContainer()
        {

            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            // Register modules
            builder.RegisterModule(new MvcSiteMapProviderModule()); // Required
            builder.RegisterModule(new MvcModule()); // Required by MVC. Typically already part of your setup (double check the contents of the module).

            builder.RegisterType<GSIDMongoRepository>().As<IGSIDMongoRepository>().SingleInstance();
            //builder.RegisterType<DatabaseFactory>().As<IDatabaseFactory>().InstancePerHttpRequest();
            //builder.RegisterAssemblyTypes(typeof(IUserMongoRepository).Assembly)
            //.Where(t => t.Name.EndsWith("Repository"))
            //.AsImplementedInterfaces().InstancePerHttpRequest();
            builder.RegisterAssemblyTypes(typeof(UserService).Assembly)
           .Where(t => t.Name.EndsWith("Service"))
           .AsImplementedInterfaces().InstancePerHttpRequest();

            builder.RegisterAssemblyTypes(typeof(DefaultFormsAuthentication).Assembly)
         .Where(t => t.Name.EndsWith("Authentication"))
         .AsImplementedInterfaces().InstancePerHttpRequest();

            //builder.Register(c => new UserManager<User>(new UserStore<User>(new ERPChanLapEntities())))
            //    .As<UserManager<User>>().InstancePerHttpRequest();

            builder.RegisterFilterProvider();
            Autofac.IContainer container = builder.Build();
            // Setup global sitemap loader (required)
            MvcSiteMapProvider.SiteMaps.Loader = container.Resolve<ISiteMapLoader>();

            // Check all configured .sitemap files to ensure they follow the XSD for MvcSiteMapProvider (optional)
            var validator = container.Resolve<ISiteMapXmlValidator>();
            validator.ValidateXml(HostingEnvironment.MapPath("~/Mvc.sitemap"));

            // Register the Sitemaps routes for search engines (optional)
            XmlSiteMapController.RegisterRoutes(RouteTable.Routes);

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}