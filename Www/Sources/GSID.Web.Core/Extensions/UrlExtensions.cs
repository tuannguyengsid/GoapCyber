using System;
using System.Collections.Specialized;
using System.Web; // For this you need to reference System.Web assembly from the GAC
using System.Linq;
using System.Text;

namespace GSID.Web.Core.Extensions
{
    public static class UrlExtensions
    {
        public static string ReplaceQueryStringParam(this string currentPageUrl, string paramToReplace, string newValue)
        {
            string urlWithoutQuery = currentPageUrl.IndexOf('?') >= 0
                ? currentPageUrl.Substring(0, currentPageUrl.IndexOf('?'))
                : currentPageUrl;

            string queryString = currentPageUrl.IndexOf('?') >= 0
                ? currentPageUrl.Substring(currentPageUrl.IndexOf('?'))
                : null;

            var queryParamList = queryString != null
                ? HttpUtility.ParseQueryString(queryString)
                : HttpUtility.ParseQueryString(string.Empty);

            if (queryParamList[paramToReplace] != null)
            {
                queryParamList[paramToReplace] = newValue;
            }
            else
            {
                queryParamList.Add(paramToReplace, newValue);
            }
            return String.Format("{0}?{1}", urlWithoutQuery, queryParamList);
        }
    }
}
