using GSID.Data.Mongodb;
using GSID.Model.MongodbModels;
using GSID.Service;
using GSID.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GSID.Service.MongoRepositories.Service;

namespace GSID.FrontEnd.Helpers
{
    public static class GSIDSessionFacade
    {
        public static Customer GSIDSessionUserLogon
        {
            get
            {
                Customer customer = null;
                try
                {
                    UrlHelper url = new UrlHelper(HttpContext.Current.Request.RequestContext);
                    var language = HttpContext.Current.Request["language"];
                    var returnLogin = url.Action("Login", "Account", new { language = language });

                    customer = (Customer)HttpContext.Current.Session[SestionName.gsidSessionUserLogon];
                    if (customer == null && HttpContext.Current.Request.IsAuthenticated)
                    {
                        string[] userData = HttpContext.Current.User.Identity.Name.Split('|');
                        if (userData[6] == "customer")
                        {
                            ICustomerService customerService = DependencyResolver.Current.GetService<ICustomerService>();
                            customer = customerService.GetByEmailOrPhone(userData[3], userData[4]);
                            HttpContext.Current.Session[SestionName.gsidSessionUserLogon] = customer;
                        }
                        else
                        {
                            HttpContext.Current.Response.Redirect(string.Format("{0}?returnUrl={1}", returnLogin, HttpContext.Current.Request.Url.PathAndQuery));
                        }
                    }
                    else if (!HttpContext.Current.Request.IsAuthenticated)
                    {
                        HttpContext.Current.Response.Redirect(string.Format("{0}?returnUrl={1}", returnLogin, HttpContext.Current.Request.Url.PathAndQuery));
                    }
                }
                catch
                {
                }
                return customer;
            }
            set
            {
                HttpContext.Current.Session[SestionName.gsidSessionUserLogon] = value;
            }
        }

        public static void Remove(string sessionVariable)
        {
            HttpContext.Current.Session.Remove(sessionVariable);
        }

        public static void Clear()
        {
            HttpContext.Current.Session.Clear();
        }
    }
}