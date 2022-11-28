using GSID.Model.MongodbModels;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using GSID.Model.ExtraEntities;
using GSID.Setting;
using Newtonsoft.Json;
using GSID.Service.MongoRepositories.Service;
using System.Web.Optimization;

namespace GSID.FrontEnd.Handlers
{
    public sealed class UrlRouteHandler : IRouteHandler
    {
        string language = "";
        string localeOpenGraph = "";
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            IRouteDataUrlService routeDataUrlService = DependencyResolver.Current.GetService<IRouteDataUrlService>();

            var routeData = requestContext.RouteData.Values;
            var url = routeData["urlRouteHandler"] as string;

            url = url ?? "/";
            url = url == "/" ? "/" : string.Format("/{0}", url);
            url = url.ToLower();
            var route = routeDataUrlService.GetByUrl(url);//var route = UrlHandler.GetRoute(url);

            if (route != null)
            {
                GetLanguages(route);

                switch (route.IsType)
                {
                    case RouteDataUrl.RouteDataIsType.None:
                        break;
                    case RouteDataUrl.RouteDataIsType.HomePage:
                    case RouteDataUrl.RouteDataIsType.AboutUsPage:
                    case RouteDataUrl.RouteDataIsType.Recruitment:
                    case RouteDataUrl.RouteDataIsType.Gallery:
                    case RouteDataUrl.RouteDataIsType.Post:
                    case RouteDataUrl.RouteDataIsType.ContactUs:
                    case RouteDataUrl.RouteDataIsType.CategoryDetail:
                    case RouteDataUrl.RouteDataIsType.ProductDetail:
                    case RouteDataUrl.RouteDataIsType.PostDetail:
                    case RouteDataUrl.RouteDataIsType.PostTag:
                    case RouteDataUrl.RouteDataIsType.RecruitmentCareer:
                    case RouteDataUrl.RouteDataIsType.RecruitmentDetail:
                        routeData["urlRouteId"] = route.Id;
                        routeData["language"] = language;
                        break;
                    default:
                        break;
                }

                routeData["url"] = route.Url;
                routeData["controller"] = route.Controller;
                routeData["action"] = route.Action;
                routeData["id"] = route.Id;
                routeData["urlRouteHandler"] = route;

                //SEO
                routeData["TitleSEO"] = route.Title;
                routeData["KeywordSEO"] = route.Keyword;
                routeData["DescriptionSEO"] = route.Description;
                routeData["TitleSEO"] = route.Title;
                routeData["H1SEO"] = route.H1;
                routeData["H2SEO"] = route.H2;
                routeData["H3SEO"] = route.H3;
                routeData["H4SEO"] = route.H4;
                routeData["H5SEO"] = route.H5;
                routeData["H6SEO"] = route.H6;

                //OpenGraph
                routeData["OgTypeOpenGraph"]        = route.OgType;
                routeData["OgTitleOpenGraph"]       = route.OgTitle;
                routeData["OgSite_nameOpenGraph"] = route.OgSite_name;                
                routeData["OgDescriptionOpenGraph"] = route.OgDescription;
                routeData["OgLocaleOpenGraph"]      = localeOpenGraph;

                IParameterService paraService = DependencyResolver.Current.GetService<IParameterService>();
                var paraSiteInformation = paraService.GetByCode((new SiteInformationConfig()).Code);
                var _sessionSiteInformation = paraSiteInformation != null ? JsonConvert.DeserializeObject<SiteInformationConfig>(paraSiteInformation.Content.ToString()) : null;
                if (_sessionSiteInformation != null)
                {
                    routeData["OgUrlOpenGraph"] = string.Format("{0}{1}", _sessionSiteInformation.UrlAddressSite, url);
                    routeData["OgImageOpenGraph"] = string.Format("{0}/{1}", _sessionSiteInformation.UrlAddressSiteMultimedia, route.OgImageSrc);
                }
            }
            else
            {
                routeData["url"] = url;
                routeData["controller"] = "Error";
                routeData["action"] = "NotFound";

                requestContext.HttpContext.Response.Clear();
                requestContext.HttpContext.Response.Redirect("/vn/notfound?url=" + url);
                requestContext.HttpContext.Response.End();
            }

            return new MvcHandler(requestContext);
        }

        private void GetLanguages(RouteDataUrl route) {
            switch (route.IsLanguage)
            {
                case RouteDataUrl.RouteDataIsLanguage.None:
                    break;
                case RouteDataUrl.RouteDataIsLanguage.Vn:
                    language = "vn";
                    localeOpenGraph = "vi_VN";
                    break;
                case RouteDataUrl.RouteDataIsLanguage.En:
                    language = "en";
                    localeOpenGraph = "en_US";
                    break;
                default:
                    break;
            }
        } 
    }
}