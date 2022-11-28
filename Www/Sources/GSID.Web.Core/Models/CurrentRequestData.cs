using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GSID.Web.Core.Models
{
    public class CurrentRequestData
    {
        private const string UserSessionId = "current.usersessionGuid";
        private static bool? _databaseIsInstalled;

        //public static User CurrentUser
        //{
        //    get { return (User)CurrentContext.Items["current.user"]; }
        //    set { CurrentContext.Items["current.user"] = value; }
        //}



        //public static DateTime Now
        //{
        //    get { return TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo); }
        //}

        //public static bool CurrentUserIsAdmin
        //{
        //    get { return CurrentUser != null && CurrentUser.IsAdmin; }
        //}


        //public static Guid UserGuid
        //{
        //    get
        //    {
        //        if (UserGuidOverride.HasValue)
        //            return UserGuidOverride.Value;
        //        if (CurrentUser != null) return CurrentUser.Guid;
        //        string o = CurrentContext.Request.Cookies[UserSessionId] != null
        //            ? CurrentContext.Request.Cookies[UserSessionId].Value
        //            : null;
        //        Guid result;
        //        if (o == null || !Guid.TryParse(o, out result))
        //        {
        //            result = Guid.NewGuid();
        //            AddCookieToResponse(UserSessionId, result.ToString(), Now.AddMonths(3));
        //        }
        //        return result;
        //    }
        //    set { UserGuidOverride = value; }
        //}
        
        //private static void AddCookieToResponse(string key, string value, DateTime expiry)
        //{
        //    var userGuidCookie = new HttpCookie(key)
        //    {
        //        Value = value,
        //        Expires = expiry
        //    };
        //    CurrentContext.Response.Cookies.Add(userGuidCookie);
        //}
    }
}
