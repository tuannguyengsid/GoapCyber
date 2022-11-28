using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GSID.Web.Core.Authentication
{
    public class CustomAuthorization : AuthorizeAttribute
    {
        public string LoginPage { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.HttpContext.Response.Redirect(LoginPage + "?returnUrl=" + filterContext.HttpContext.Request.Url.PathAndQuery);
            }
            base.OnAuthorization(filterContext);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult(LoginPage + "?returnUrl=" + filterContext.HttpContext.Request.Url.PathAndQuery);
        }
    }
}
