using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GSID.Web.Core.Extensions
{
    public static class ModelBinderExtensions
    {
        public static string GetValueFromRequest(this ControllerContext controllerContext, string request)
        {
            return controllerContext.HttpContext.Request[request];
        }
    }
}
