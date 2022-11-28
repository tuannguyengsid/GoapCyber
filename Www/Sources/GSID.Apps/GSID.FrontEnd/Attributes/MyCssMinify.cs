using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace GSID.FrontEnd.Attributes
{
    //public class MyStyleBundle : Bundle
    //{
    //    public MyStyleBundle(string virtualPath) : base(virtualPath, new MyCssMinify())
    //    {
    //    }

    //    public MyStyleBundle(string virtualPath, string cdnPath) : base(virtualPath, cdnPath, new MyCssMinify())
    //    {
    //    }
    //}

    //public class MyCssMinify : IBundleTransform
    //{
    //    internal static readonly MyCssMinify Instance = new MyCssMinify();

    //    internal static string CssContentType = "text/css";


    //    public virtual void Process(BundleContext context, BundleResponse response)
    //    {
    //        if (context == null)
    //        {
    //            throw new ArgumentNullException("context");
    //        }
    //        if (response == null)
    //        {
    //            throw new ArgumentNullException("response");
    //        }
    //        if (!context.EnableInstrumentation)
    //        {
    //            // CssCompress.Go- This is your CSS compression implementation
    //            // You can use the library " Uglify"
    //            response.Content = CssCompress.Go(response.Content);
    //        }
    //        response.ContentType = CssContentType;
    //    }
    //}
}