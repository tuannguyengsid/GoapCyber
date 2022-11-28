using System.Web.Mvc;
using GSID.Web.Core.Helpers;
using System;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Expressions;
using System.Web.Mvc.Html;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;

namespace GSID.Web.Core.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static UserHtmlHelper User(this HtmlHelper html)
        {
            return new UserHtmlHelper(html, new UrlHelper(html.ViewContext.RequestContext));
        }

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

        //public static MvcHtmlString RawActionLinkIfRole(this HtmlHelper htmlHelper, string rawHtml, string action, string controller, object routeValues, object htmlAttributes)
        //{
        //    //string anchor = htmlHelper.ActionLink("##holder##", action, controller, routeValues, htmlAttributes).ToString();
        //    //return MvcHtmlString.Create(anchor.Replace("##holder##", rawHtml));
        //    /* Updated code to use a generated guid as from the suggestion of Eddy Nguyen */

        //    var requiredPermission = string.Format("{0}-{1}", controller, action).ToLower();
        //    RBACUser requestingUser = new RBACUser();
        //    if (requestingUser.IsSysAdmin
        //            || requestingUser.HasPermission(requiredPermission))
        //    {
        //        string holder = Guid.NewGuid().ToString();
        //        string anchor = htmlHelper.ActionLink(holder, action, controller, routeValues, htmlAttributes).ToString();
        //        return MvcHtmlString.Create(anchor.Replace(holder, rawHtml));
        //    }
        //    return MvcHtmlString.Empty;
        //}

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

        //public static MvcHtmlString IfRole(this MvcHtmlString value, string action, string controller)
        //{
        //    var requiredPermission = string.Format("{0}-{1}", controller, action).ToLower();
        //    RBACUser requestingUser = new RBACUser();
        //    if (requestingUser.IsSysAdmin
        //            || requestingUser.HasPermission(requiredPermission))
        //        return value;

        //    return MvcHtmlString.Empty;
        ////}
        ///// <summary>
        ///// Adds increased attributed functionality to DropDownListFor.
        ///// </summary>
        ///// <typeparam name="TModel"></typeparam>
        ///// <typeparam name="TProperty"></typeparam>
        ///// <typeparam name="T">The type of object in a list.</typeparam>
        ///// <param name="htmlHelper">The object this method attaches to.</param>
        ///// <param name="expression">The object the selected value binds to.</param>
        ///// <param name="listModel">The Model object for creating this list.</param>
        ///// <returns>MvcHtmlString - Html for Razor pages.</returns>
        //public static MvcHtmlString DropDownListWithAttributesFor<TModel, TProperty, T>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, HtmlSelectListModel<T> listModel)
        //{
        //    var dropdown = new TagBuilder("select");
        //    foreach (var selectAttribute in listModel.SelectAttributes)
        //    {
        //        dropdown.Attributes.Add(selectAttribute.Key, selectAttribute.Value);
        //    }
        //    string currentValue = null;
        //    var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
        //    if (metadata != null && metadata.Model != null)
        //        currentValue = metadata.Model.ToString();

        //    var optionsBuilder = new StringBuilder();
        //    foreach (T item in listModel.DataObjects)
        //    {
        //        BuildOptionTags(listModel, optionsBuilder, item, currentValue);
        //    }
        //    if (string.IsNullOrWhiteSpace(currentValue) && listModel.ShouldUseEmptyValue)
        //    {
        //        optionsBuilder.Insert(0, "<option value=\"\">" + listModel.EmptyValueText + "</option>");
        //    }
        //    dropdown.InnerHtml = optionsBuilder.ToString();

        //    return new MvcHtmlString(dropdown.ToString(TagRenderMode.Normal));
        //}

        //internal static void BuildOptionTags<T>(HtmlSelectListModel<T> listModel, StringBuilder optionsBuilder, T item, string selectedValue)
        //{
        //    var optionAttributes = GetOptionAttributes(listModel, item);
        //    string innerText = GetOptionInnerText(optionAttributes);

        //    if (optionsBuilder == null) { optionsBuilder = new StringBuilder(); }
        //    optionsBuilder.Append("<option ");
        //    bool defaultValueFound = false;
        //    foreach (var attribute in optionAttributes)
        //    {
        //        optionsBuilder.Append(string.Format("{0}=\"{1}\" ", attribute.Key, attribute.Value));
        //        if (attribute.Value == selectedValue)
        //        {
        //            optionsBuilder.Append("selected=\"selected\" ");
        //            defaultValueFound = true;
        //        }
        //    }
        //    optionsBuilder.Append(">" + innerText + "</option>");
        //}

        //internal static string GetOptionInnerText(IDictionary<string, string> optionAttributes)
        //{
        //    string innerText;
        //    if (optionAttributes.TryGetValue("inner-text", out innerText))
        //    {
        //        optionAttributes.Remove("inner-text");
        //    }
        //    return innerText;
        //}

        //internal static Dictionary<string, string> GetOptionAttributes<T>(HtmlSelectListModel<T> listModel, T item)
        //{
        //    var properties = item.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => listModel.OptionAttributes.Values.Contains(p.Name));
        //    var optionAttributes = new Dictionary<string, string>();
        //    foreach (var p in properties)
        //    {
        //        var p1 = p;
        //        foreach (var oa in listModel.OptionAttributes.Where(oa => p1.Name == oa.Value))
        //        {
        //            optionAttributes.Add(oa.Key, p.GetValue(item, null).ToString());
        //        }
        //    }
        //    return optionAttributes;
        //}
    }
    //public class HtmlSelectListModel<T>
    //{
    //    /// <summary>
    //    /// That collection of data elements from which to create a the select options.
    //    /// </summary>
    //    public IEnumerable<T> DataObjects { get; set; }

    //    /// <summary>
    //    /// Often there is an option on top called "--Select item--" that has an empty value.
    //    /// </summary>
    //    public string EmptyValueText { get; set; }

    //    /// <summary>
    //    /// A bool value that checks for whether EmptyValueText is used or not
    //    /// </summary>
    //    public bool ShouldUseEmptyValue
    //    {
    //        get { return !string.IsNullOrWhiteSpace(EmptyValueText); }
    //    }

    //    /// <summary>
    //    /// This will add attributes to the <select></select> html tag.
    //    /// The key is the attribute, the value is the value.
    //    /// For example:
    //    ///     SelectAttributes.add("id", "select-id-1");
    //    /// Would result in this html:
    //    ///     <select id="select-id-1"></select>
    //    /// </summary>
    //    public Dictionary<string, string> SelectAttributes
    //    {
    //        get { return _SelectAttributes ?? (_SelectAttributes = new Dictionary<string, string>()); }
    //        set { _SelectAttributes = value; }
    //    }
    //    private Dictionary<string, string> _SelectAttributes;

    //    /// <summary>
    //    /// This will add attributes to the <option></option> html tag in a select list.
    //    /// The key is the attribute, the value is the property on the object that contains the value.
    //    /// A list of objects will be used to create the options. Using reflection, the property with a
    //    /// name matching the value will be found. If the value is "Country" then a property name Country
    //    /// will be found using reflection. 
    //    /// 
    //    /// Note: If inner-text is used it will not be an attribute but will be the text between the
    //    ///       opening and closing tags.
    //    /// 
    //    /// For example:
    //    ///     OptionAttributes.add("value", "CountryTwoLetter");
    //    ///     OptionAttributes.add("innter-text", "Country");
    //    /// If the the list had to objects and the Country values were "United States" and "Canada",
    //    /// and the CountryTwoLetter values were "US" and "CA", then the result would be this html:
    //    ///     <option value="US">United States</option>
    //    ///     <option value="US">United States</option>
    //    /// 
    //    /// Html data-[name] attributes are supported here as well.
    //    /// </summary>
    //    public Dictionary<string, string> OptionAttributes
    //    {
    //        get { return _OptionAttributes ?? (_OptionAttributes = new Dictionary<string, string>()); }
    //        set { _OptionAttributes = value; }
    //    }
    //    private Dictionary<string, string> _OptionAttributes;
    //}
}