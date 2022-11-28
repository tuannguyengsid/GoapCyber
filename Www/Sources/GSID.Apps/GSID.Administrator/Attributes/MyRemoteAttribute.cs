using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GSID.Admin.Attributes
{
    public class MyRemoteAttribute : RemoteAttribute
    {
        public MyRemoteAttribute(string action, string controller, string area)
            : base(action, controller, area)
        {
            this.RouteData["area"] = area;
        }
    }
}