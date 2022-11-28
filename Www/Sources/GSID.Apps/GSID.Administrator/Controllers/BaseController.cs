using GSID.Model.MongodbModels;
using GSID.Setting;
using GSID.Admin.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using GSID.Model.ExtraEntities;
using Newtonsoft.Json;
using GSID.Service.MongoRepositories.Service;

namespace GSID.Admin.Controllers
{
    public class BaseController : Controller
    {
        public string pathForSaving { get; set; }
        public string pathForUrl { get; set; }
        public int PageSize = 20;
        public int PageVisit = 8;
        public int PageIndex = 1;

        public static SiteInformationConfig GSIDSessionSiteInformation
        {
            get
            {
                var sesionConfig = System.Web.HttpContext.Current.Session[SestionName.gsidSessionSiteInformation];
                if (sesionConfig != null)
                    return (SiteInformationConfig)sesionConfig;
                else
                {
                    IParameterService paraService = DependencyResolver.Current.GetService<IParameterService>();
                    var obj = paraService.GetByCode((new SiteInformationConfig()).Code);
                    var config = obj != null ? JsonConvert.DeserializeObject<SiteInformationConfig>(obj.Content.ToString()) : null;
                    System.Web.HttpContext.Current.Session[SestionName.gsidSessionSiteInformation] = config;

                    return config;
                }
            }
            set
            {
                System.Web.HttpContext.Current.Session[SestionName.gsidSessionSiteInformation] = value;
            }
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
        public Nullable<double> ConvertNullDouble(string value)
        {
            double val = 0;
            if (double.TryParse(value, out val))
                return val;
            return null;
        }

        public Nullable<decimal> ConvertNullDecimal(string value)
        {
            decimal val = 0;
            if (decimal.TryParse(value, out val))
                return val;
            return null;
        }


        

        public Nullable<DateTime> ConvertDateTimeIsNull(string value)
        {
            DateTime val;
            if (DateTime.TryParseExact(value,"d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out val))
                return val;
            
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