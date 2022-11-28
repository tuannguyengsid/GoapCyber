using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GSID.Admin
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(null, "connector", new { controller = "Files", action = "Browser" });
            routes.MapRoute(null, "Thumbnails/{tmb}", new { controller = "Files", action = "Thumbs", tmb = UrlParameter.Optional });

            routes.MapRoute(
                name: "Login",
                url: "Login",
                defaults: new { controller = "Membership", action = "Login" }
            );

            routes.MapRoute(
                name: "Logout",
                url: "Logout",
                defaults: new { controller = "Membership", action = "LogOff" }
            );

            routes.MapRoute(
              "Default",
              "{controller}/{action}/{id}",
              new { controller = "Home", action = "Index", id = UrlParameter.Optional },
              new[] { "GSID.Admin.Controllers" }
          );
        }
    }
}
