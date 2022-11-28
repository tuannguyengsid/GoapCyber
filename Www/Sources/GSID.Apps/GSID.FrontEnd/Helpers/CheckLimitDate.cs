using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace GSID.WebApp.Helpers
{
    public class CheckLimitDate : System.Web.Mvc.ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var limitdate = new DateTime(2016, 9, 27);

            if (DateTime.Now > limitdate)
            {
                context.Result = new RedirectResult("http://g-sid.com");
            }
        }
    }
}