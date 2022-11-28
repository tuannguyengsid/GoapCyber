using GSID.Data;
using GSID.Model.MongodbModels;
using GSID.Service;
using GSID.Setting;
using GSID.FrontEnd.CustomExceptions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using GSID.FrontEnd.Models;
using GSID.Model.ExtraEntities;
using Newtonsoft.Json;
using GSID.Service.MongoRepositories.Service;
using System.Globalization;

namespace GSID.FrontEnd
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //Install in nuget to pass from mvc 4 to mvc5
            //Install-Package Microsoft.AspNet.WebHelpers
            //Install-Package Microsoft.AspNet.WebPages.Data
            //System.Data.Entity.Database.SetInitializer(new GSIDEcommerceData());
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Bootstrapper.Run();

            MvcHandler.DisableMvcResponseHeader = true;

            var languages = new List<Language>();
            languages.Add(new Language() { Name = "vn", Parent = "vn", Default = true, Url = "vn", Current = true, Country = Language.LanguagueCountry.Vietmamese });
            languages.Add(new Language() { Name = "en", Parent = "en", Default = true, Url = "en", Current = false, Country = Language.LanguagueCountry.English });

            HttpContext.Current.Application.Add(SestionName.Languages, languages);
        }
        protected void Application_PreSendRequestHeaders()
        {
            //Response.Headers.Remove("Server");
            MvcHandler.DisableMvcResponseHeader = true;
            Response.Headers.Set("Server", "MsMserver");
            Response.Headers.Remove("X-AspNet-Version"); //alternative to above solution
            Response.Headers.Remove("X-AspNetMvc-Version"); //alternative to above solution
        }

        protected void Application_Error()
        {
            var exception = Server.GetLastError();
            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);

            if (exception is System.ArgumentException
                    || exception is UrlNotFoundException
                // || exception is HttpException
                )
            {
                Response.Redirect(string.Format("/vn/notfound?url={0}", HttpContext.Current.Request.Url), true);
            }
            else if (exception is SqlException)
            {
                Response.Redirect(urlHelper.RouteUrl("SetupError"), true);
            }
            else
            {
                Response.Redirect(string.Format("/vn/notfound?url={0}", HttpContext.Current.Request.Url), true);
            }
        }

        protected void Application_BeginRequest()
        {
            //// Ensure any request is returned over SSL/TLS in production
            //if (!Request.IsLocal && !Context.Request.IsSecureConnection)
            //{
            //    var redirect = Context.Request.Url.ToString().ToLower(CultureInfo.CurrentCulture).Replace("http:", "https:");
            //    Response.Redirect(redirect);
            //}
        }

        protected void OnSessionStart()
        {
            //HttpContext.Current.Session.Add(SestionName.HistoryView, new List<ProductHistoryView>());
            //IParameterService paraService = DependencyResolver.Current.GetService<IParameterService>();
            //var para = paraService.GetByCode(GSID.Setting.ParameterCodeKey.PARAMETER_KEY_WEBSITE_URL);
            //HttpContext.Current.Session[SestionName.gsidSessionWebTitle] = para;
            var languages = (List<Language>)System.Web.HttpContext.Current.Application[SestionName.Languages];
            if (languages != null)
                HttpContext.Current.Session.Add(SestionName.Languages, languages);

            IParameterService paraService = DependencyResolver.Current.GetService<IParameterService>();
            var obj = paraService.GetByCode((new SiteInformationConfig()).Code);
            var config = obj != null ? JsonConvert.DeserializeObject<SiteInformationConfig>(obj.Content.ToString()) : null;
            System.Web.HttpContext.Current.Session[SestionName.gsidSessionSiteInformationFront] = config;
        }

        protected void Session_End(object sender, EventArgs e)
        {
        }
    }
}
