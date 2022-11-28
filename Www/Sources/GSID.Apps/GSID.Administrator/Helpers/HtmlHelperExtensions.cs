using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Mvc.Ajax;

namespace GSID.Admin.Helpers
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString RawActionLink(this AjaxHelper ajaxHelper, string rawHtml, string action, string controller, object routeValues, AjaxOptions ajaxOptions, object htmlAttributes)
        {
            //string anchor = ajaxHelper.ActionLink("##holder##", action, controller, routeValues, ajaxOptions, htmlAttributes).ToString();
            //return MvcHtmlString.Create(anchor.Replace("##holder##", rawHtml));
            /* Updated code to use a generated guid as from the suggestion of Eddy Nguyen */
            string holder = Guid.NewGuid().ToString();
            string anchor = ajaxHelper.ActionLink(holder, action, controller, routeValues, ajaxOptions, htmlAttributes).ToString();
            return MvcHtmlString.Create(anchor.Replace(holder, rawHtml));
        }

        public static MvcHtmlString RawActionLink(this HtmlHelper htmlHelper, string rawHtml, string action, string controller, object routeValues, object htmlAttributes)
        {
            //string anchor = htmlHelper.ActionLink("##holder##", action, controller, routeValues, htmlAttributes).ToString();
            //return MvcHtmlString.Create(anchor.Replace("##holder##", rawHtml));
            /* Updated code to use a generated guid as from the suggestion of Eddy Nguyen */
            string holder = Guid.NewGuid().ToString();
            string anchor = htmlHelper.ActionLink(holder, action, controller, routeValues, htmlAttributes).ToString();
            return MvcHtmlString.Create(anchor.Replace(holder, rawHtml));
        }

        public static MvcHtmlString RawActionLinkIfRole(this HtmlHelper htmlHelper, string rawHtml, string action, string controller, object routeValues, object htmlAttributes)
        {
            //string anchor = htmlHelper.ActionLink("##holder##", action, controller, routeValues, htmlAttributes).ToString();
            //return MvcHtmlString.Create(anchor.Replace("##holder##", rawHtml));
            /* Updated code to use a generated guid as from the suggestion of Eddy Nguyen */

            var requiredPermission = string.Format("{0}-{1}", controller, action).ToLower();
            RBACUser requestingUser = new RBACUser();
            if (requestingUser.IsSysAdmin
                    || requestingUser.HasPermission(requiredPermission))
            {
                string holder = Guid.NewGuid().ToString();
                string anchor = htmlHelper.ActionLink(holder, action, controller, routeValues, htmlAttributes).ToString();
                return MvcHtmlString.Create(anchor.Replace(holder, rawHtml));
            }
            return MvcHtmlString.Empty;
        }

        public static MvcHtmlString Button(this HtmlHelper helper,
                                     string innerHtml,
                                     object htmlAttributes)
        {
            return Button(helper, innerHtml,
                          HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes)
            );
        }

        public static MvcHtmlString Button(this HtmlHelper helper,
                                           string innerHtml,
                                           IDictionary<string, object> htmlAttributes)
        {
            var builder = new TagBuilder("button");
            builder.InnerHtml = innerHtml;
            builder.MergeAttributes(htmlAttributes);
            return MvcHtmlString.Create(builder.ToString());
        }

        public static MvcHtmlString IfRole(this MvcHtmlString value, string action, string controller)
        {
            var requiredPermission = string.Format("{0}-{1}", controller, action).ToLower();
            RBACUser requestingUser = new RBACUser();
            if (requestingUser.IsSysAdmin
                    || requestingUser.HasPermission(requiredPermission))
                return value;

            return MvcHtmlString.Empty;
        }

        public static string ImageOrDefault(string imgSrc, string imgDefaultSrc)
        {
            return !string.IsNullOrEmpty(imgSrc) ? imgSrc : imgDefaultSrc;
        }

        public static string ImageOrDefault(string imgSrc, string imgDefaultSrc, string httpStatic)
        {
            return !string.IsNullOrEmpty(imgSrc) ? string.Format("{0}/{1}", httpStatic, imgSrc) : imgDefaultSrc;
        }
    }

    public class StringExtension
    {

        public static string ImageOrDefault(string imgSrc, string imgDefaultSrc)
        {
            return !string.IsNullOrEmpty(imgSrc) ? imgSrc : imgDefaultSrc;
        }

        public static string ImageOrDefault(string imgSrc, string imgDefaultSrc, string httpStatic)
        {
            return !string.IsNullOrEmpty(imgSrc) ? string.Format("{0}/{1}", httpStatic, imgSrc) : imgDefaultSrc;
        }
    }
}