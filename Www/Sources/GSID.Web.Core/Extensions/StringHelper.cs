using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GSID.Web.Core.Extensions
{
    public static class StringHelper
    {
        public static string BreakUpString(this string value)
        {
            return Regex.Replace(value,
                                 "((?<=[a-z])[A-Z]|[A-Z](?=[a-z]))",
                                 " $1",
                                 RegexOptions.Compiled).Trim();
        }

        public static List<int> GetIntList(this string value)
        {
            return string.IsNullOrWhiteSpace(value)
                       ? new List<int>()
                       : value.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(s => Convert.ToInt32(s.Trim()))
                             .ToList();
        }

        public static int GetIntValue(this string value, int defaultValue = 0)
        {
            int val;
            return string.IsNullOrWhiteSpace(value)
                       ? defaultValue
                       : int.TryParse(value, out val) ? val : defaultValue;
        }

        public static string ToString(this IEnumerable<int> value)
        {
            if (value == null || !value.Any())
                return string.Empty;

            return string.Join(",", value);
        }

        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source.IndexOf(toCheck, comp) >= 0;
        }

        public static bool HasValue(this string value)
        {
            return !String.IsNullOrWhiteSpace(value);
        }

        public static string StripHtml(this string htmlString, string replaceHtmlWith = " ")
        {
            if (string.IsNullOrWhiteSpace(htmlString))
                return "";
            return Regex.Replace(htmlString, @"<(.|\n)*?>", replaceHtmlWith);
        }

        public static string TruncateString(this string text, int maxCharacters, string trailingText = "...")
        {
            if (string.IsNullOrEmpty(text) || maxCharacters <= 0)
                return text;
            var returnString = "";
            if (text.Length >= maxCharacters)
                returnString = text.Substring(0, maxCharacters) + trailingText;
            else
                returnString = text;

            return returnString;
        }

        public static string FormatWith(this string format, params object[] args)
        {
            if (format == null)
                throw new ArgumentNullException("format");
            return string.Format(format, args);
        }

        public static string FormatWith(this string format, IFormatProvider provider, params object[] args)
        {
            if (format == null)
                throw new ArgumentNullException("format");
            return string.Format(provider, format, args);
        }

        #region ToTitleCase
        private static CultureInfo ci = new CultureInfo("en-US");
        //Convert all first latter
        public static string ToTitleCase(this string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = str.ToLower();
                var strArray = str.Split(' ');
                if (strArray.Length > 1)
                {
                    strArray[0] = ci.TextInfo.ToTitleCase(strArray[0]);
                    return string.Join(" ", strArray);
                }
                return ci.TextInfo.ToTitleCase(str);
            }
            else
                return string.Empty;
           
        }

        public static string ToTitleCase(this string str, TitleCase tcase)
        {
            str = str.ToLower();
            switch (tcase)
            {
                case TitleCase.First:
                    var strArray = str.Split(' ');
                    if (strArray.Length > 1)
                    {
                        strArray[0] = ci.TextInfo.ToTitleCase(strArray[0]);
                        return string.Join(" ", strArray);
                    }
                    break;
                case TitleCase.All:
                    return ci.TextInfo.ToTitleCase(str);
                default:
                    break;
            }
            return ci.TextInfo.ToTitleCase(str);
        }
        #endregion

        /// <summary>
        /// takes a substring between two anchor strings (or the end of the string if that anchor is null)
        /// </summary>
        /// <param name="this">a string</param>
        /// <param name="from">an optional string to search after</param>
        /// <param name="until">an optional string to search before</param>
        /// <param name="comparison">an optional comparison for the search</param>
        /// <returns>a substring based on the search</returns>
        public static string Substring(this string @this, string from = null, string until = null, StringComparison comparison = StringComparison.InvariantCulture)
        {
            var fromLength = (from ?? string.Empty).Length;
            var startIndex = !string.IsNullOrEmpty(from)
                ? @this.IndexOf(from, comparison) + fromLength
                : 0;

            if (startIndex < fromLength) { throw new ArgumentException("from: Failed to find an instance of the first anchor"); }

            var endIndex = !string.IsNullOrEmpty(until)
            ? @this.IndexOf(until, startIndex, comparison)
            : @this.Length;

            if (endIndex < 0) { throw new ArgumentException("until: Failed to find an instance of the last anchor"); }

            var subString = @this.Substring(startIndex, endIndex - startIndex);
            return subString;
        }
    }

    public enum TitleCase
    {
        First,
        All
    }
}
