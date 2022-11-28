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

namespace GSID.Admin.Helpers
{
    public static class GSIDSessionFacade
    {
        public static User GSIDSessionUserLogon
        {
            get {
                User user = (User)HttpContext.Current.Session[SestionName.gsidSessionUserLogon];
                if (user == null && HttpContext.Current.Request.IsAuthenticated) {
                    string[] userData = HttpContext.Current.User.Identity.Name.Split('|');
                    if (userData[5] == "user") {
                        IUserService userService = DependencyResolver.Current.GetService<IUserService>();
                        user = userService.VerifiedAccount(userData[3]);
                        HttpContext.Current.Session[SestionName.gsidSessionUserLogon] = user;
                    }
                    else {
                        HttpContext.Current.Response.Redirect(string.Format("{0}?returnUrl={1}", SestionName.ReturnLogin, HttpContext.Current.Request.Url.PathAndQuery));
                    }
                }
                else if (!HttpContext.Current.Request.IsAuthenticated) {
                    HttpContext.Current.Response.Redirect(string.Format("{0}?returnUrl={1}", SestionName.ReturnLogin, HttpContext.Current.Request.Url.PathAndQuery));
                }
                return user;
            }
            set {
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