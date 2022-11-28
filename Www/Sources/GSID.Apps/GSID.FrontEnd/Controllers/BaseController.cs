using GSID.Model.MongodbModels;
using GSID.Setting;
using GSID.FrontEnd.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using GSID.Service.MongoRepositories.Service;
using GSID.Model.ExtraEntities;
using Newtonsoft.Json;
using GSID.FrontEnd.Models;
using System.Web.Routing;
using System.Web.Mvc.Html;

namespace GSID.FrontEnd.Controllers
{
    public class BaseController : Controller
    {
        public int PageSize = 20;//Số lượng item hiển thị
        public int PageVisit = 5;//Số lượng trang hiển thị
        public int PageIndex = 1;//Vị trí trang
        public string pathForSaving { get; set; }
        public string pathForUrl { get; set; }
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            string cultureName = null;

            // Attempt to read the culture cookie from Request
            HttpCookie cultureCookie = Request.Cookies["_culture"];
            if (cultureCookie != null)
                cultureName = cultureCookie.Value;
            else
                cultureName = "vn";
            //cultureName = Request.UserLanguages != null && Request.UserLanguages.Length > 0 ?
            //        Request.UserLanguages[0] :  // obtain it from HTTP header AcceptLanguages
            //        null;
            // Validate culture name
            cultureName = CultureHelper.GetImplementedCulture(cultureName); // This is safe

            // Modify current thread's cultures            
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            return base.BeginExecuteCore(callback, state);
        }

        protected string DefineUrlLanguages(string currentLanguage, string urlVi, string urlEn)
        {
            string url = string.Empty;
            var languages = (List<Language>)System.Web.HttpContext.Current.Application[SestionName.Languages];
            if (languages != null)
            {
                var lanCurent = !string.IsNullOrEmpty(currentLanguage) ? languages.Where(l => l.Parent == currentLanguage).FirstOrDefault()
                                                                            : languages.Where(l => l.Default == true).FirstOrDefault();
                if (lanCurent != null)
                {
                    Response.Cookies["_culture"].Value = lanCurent.Parent;
                    Response.Cookies["_culture"].Expires = DateTime.Now.AddDays(30);

                    languages.Select(X => {
                        X.Current = false;
                        return X;
                    }).Where(i => i.Country == lanCurent.Country)
                                    .Select(Y => {
                                        Y.Current = true;
                                        return Y;
                                    }).ToList();
                    url = lanCurent.Parent;
                }

                languages.Where(i => i.Country == Language.LanguagueCountry.Vietmamese).Select(S => { S.Url = urlVi; return S; }).ToList();
                languages.Where(i => i.Country == Language.LanguagueCountry.English).Select(S => { S.Url = urlEn; return S; }).ToList();

                ViewBag.Languages = languages;
                System.Web.HttpContext.Current.Session[SestionName.Languages] = languages;
            }

            return url;
        }

        protected string DefineRouterValueLanguages(string currentLanguage = "", object routerValueVi = null, object routerValueEn = null)
        {
            //mẫu
            //IDictionary<string, object> routeValues = new Dictionary<string, object>();
            //routeValues.Add("EmployeeID", 1);
            //@Html.ActionLink("Employee Details", "EmployeeDetails", "Employee", new RouteValueDictionary(routeValues), null);


            string actionName       = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName   = this.ControllerContext.RouteData.Values["controller"].ToString();
            string url              = string.Empty;
            var languages           = (List<Language>)System.Web.HttpContext.Current.Application[SestionName.Languages];
            
            if (languages != null)
            {
                //vietnam
                var routeValuesDictionaryVi = new RouteValueDictionary(routerValueVi);
                if (!routeValuesDictionaryVi.ContainsKey("language"))
                    routeValuesDictionaryVi.Add("language", "vn");
                languages.Where(i => i.Country == Language.LanguagueCountry.Vietmamese).Select(S => { S.Url = Url.Action(actionName, controllerName, routeValuesDictionaryVi, this.Request.Url.Scheme); return S; }).ToList();

                var routeValuesDictionaryEn = new RouteValueDictionary(routerValueEn);
                if (!routeValuesDictionaryEn.ContainsKey("language"))
                    routeValuesDictionaryEn.Add("language", "en");
                languages.Where(i => i.Country == Language.LanguagueCountry.English).Select(S => { S.Url = Url.Action(actionName, controllerName, routeValuesDictionaryEn, this.Request.Url.Scheme); return S; }).ToList();

                var lanCurent = !string.IsNullOrEmpty(currentLanguage) ? languages.Where(l => l.Parent == currentLanguage).FirstOrDefault()
                                                                            : languages.Where(l => l.Default == true).FirstOrDefault();
                if (lanCurent != null)
                {
                    Response.Cookies["_culture"].Value = lanCurent.Parent;
                    Response.Cookies["_culture"].Expires = DateTime.Now.AddDays(30);

                    languages.Select(X => {
                        X.Current = false;
                        return X;
                    }).Where(i => i.Country == lanCurent.Country)
                                    .Select(Y => {
                                        Y.Current = true;
                                        return Y;
                                    }).ToList();
                    url = lanCurent.Url;
                }

                ViewBag.Languages = languages;
                System.Web.HttpContext.Current.Session[SestionName.Languages] = languages;
            }

            return url;
        }

        protected string DefineRouterValueLanguages(string currentLanguage = "", string urlVn = "", string urlEn = "")
        {
            //mẫu
            //IDictionary<string, object> routeValues = new Dictionary<string, object>();
            //routeValues.Add("EmployeeID", 1);
            //@Html.ActionLink("Employee Details", "EmployeeDetails", "Employee", new RouteValueDictionary(routeValues), null);


            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            string url = string.Empty;
            var languages = (List<Language>)System.Web.HttpContext.Current.Application[SestionName.Languages];

            if (languages != null)
            {
                //vietnam
                languages.Where(i => i.Country == Language.LanguagueCountry.Vietmamese).Select(S => { S.Url = urlVn; return S; }).ToList();
                languages.Where(i => i.Country == Language.LanguagueCountry.English).Select(S => { S.Url = urlEn; return S; }).ToList();

                var lanCurent = !string.IsNullOrEmpty(currentLanguage) ? languages.Where(l => l.Parent == currentLanguage).FirstOrDefault()
                                                                            : languages.Where(l => l.Default == true).FirstOrDefault();
                if (lanCurent != null)
                {
                    Response.Cookies["_culture"].Value = lanCurent.Parent;
                    Response.Cookies["_culture"].Expires = DateTime.Now.AddDays(30);

                    languages.Select(X => {
                        X.Current = false;
                        return X;
                    }).Where(i => i.Country == lanCurent.Country)
                                    .Select(Y => {
                                        Y.Current = true;
                                        return Y;
                                    }).ToList();
                    url = lanCurent.Url;
                }

                ViewBag.Languages = languages;
                System.Web.HttpContext.Current.Session[SestionName.Languages] = languages;
            }

            return url;
        }


        //public static MvcHtmlString ActionLinkSafeId(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes)
        //{
        //    var routeValuesDictionary = new RouteValueDictionary(routeValues);
        //    MvcHtmlString link = htmlHelper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes);
        //    if (routeValuesDictionary.ContainsKey("id") && routeValuesDictionary["id"].ToString().Contains('.'))
        //    {
        //        if (routeValuesDictionary.Count == 1)
        //        {
        //            var searchValue = string.Format("/{0}", routeValuesDictionary["id"]);
        //            var replaceValue = string.Format("?id={0}", routeValuesDictionary["id"]);
        //            link = new MvcHtmlString(link.ToString().Replace(searchValue, replaceValue));
        //        }
        //        else
        //        {
        //            var searchValue = string.Format("/{0}?", routeValuesDictionary["id"]);
        //            var replaceValue = string.Format("?id={0}&", routeValuesDictionary["id"]);
        //            link = new MvcHtmlString(link.ToString().Replace(searchValue, replaceValue));
        //        }
        //    }

        //    return link;
        //}

        public static SiteInformationConfig GSIDSessionSiteInformation
        {
            get
            {
                var sesionConfig = System.Web.HttpContext.Current.Session[SestionName.gsidSessionSiteInformationFront];
                if (sesionConfig != null)
                    return (SiteInformationConfig)sesionConfig;
                else {
                    IParameterService paraService = DependencyResolver.Current.GetService<IParameterService>();
                    var obj = paraService.GetByCode((new SiteInformationConfig()).Code);
                    var config = obj != null ? JsonConvert.DeserializeObject<SiteInformationConfig>(obj.Content.ToString()) : null;
                    System.Web.HttpContext.Current.Session[SestionName.gsidSessionSiteInformationFront] = config;

                    return config;
                }
            }
            set
            {
                System.Web.HttpContext.Current.Session[SestionName.gsidSessionSiteInformationFront] = value;
            }
        }

        public string GetCulture()
        { 
            HttpCookie cultureCookie = Request.Cookies["_culture"];
            return cultureCookie != null ? cultureCookie.Value : "vn";
        }

        public ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Login", "Account");
        }

        #region File
        /// <summary>
        /// Creates the folder if needed.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        protected bool CreateFolderIfNeeded(string path)
        {
            bool result = true;
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception)
                {
                    /*TODO: You must process this exception.*/
                    result = false;
                }
            }
            return result;
        }
        #endregion

        public int ConvertInt(string value)
        {
            int val = 0;
            if (int.TryParse(value, out val))
            {
                return val;
            }
            return -1;
        }

        public double ConvertDouble(string value)
        {
            double val = 0;
            if (double.TryParse(value, out val))
            {
                return val;
            }
            return -1;
        }

        public Nullable<DateTime> ConvertDateTimeIsNull(string value)
        {
            DateTime val ;
            if (DateTime.TryParse(value, out val))
            {
                return val;
            }
            return null;
        }
        public string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }

        public string GetIPAddress()
        {
            string ipAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = Request.ServerVariables["REMOTE_ADDR"];
            }
            return ipAddress;
        }


    }
}