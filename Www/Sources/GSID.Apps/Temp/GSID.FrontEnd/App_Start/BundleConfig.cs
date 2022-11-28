using System.Web;
using System.Web.Optimization;

namespace GSID.FrontEnd
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {


            bundles.UseCdn = true;   //enable CDN support





            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-2.1.1.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

             bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/jquery.form.js",
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/plugins/metisMenu/jquery.metisMenu.js",
                      "~/Scripts/plugins/slimscroll/jquery.slimscroll.min.js",
                      "~/Scripts/plugins/dataTables/jquery.dataTables.js",
                      "~/Scripts/plugins/dataTables/dataTables.bootstrap.js",
                      "~/Scripts/plugins/dataTables/dataTables.responsive.js",
                      "~/Scripts/plugins/dataTables/dataTables.tableTools.min.js",
                      "~/Scripts/erpchanlap.js",
                      "~/Scripts/plugins/pace/pace.min.js",
                      "~/Scripts/plugins/peity/jquery.peity.min.js",
                      "~/Scripts/plugins/iCheck/icheck.min.js",
                      "~/Scripts/demo/peity-demo.js",
                      "~/Scripts/plugins/datapicker/bootstrap-datepicker.js"));


            
            bundles.Add(new StyleBundle("~/css/bootstrap-selector").Include(
                      "~/Content/vendors/bootstrap-selector/css/bootstrap-select.min.css",
                       "~/Content/css/bootstrap.min.css"));

            bundles.Add(new StyleBundle("~/css/styles/fonts-1/css").Include(
                      "~/Content/vendors/elagent/style.css",
                      "~/Content/vendors/themify-icon/themify-icons.css",
                      "~/Content/vendors/flaticon/flaticon.css",
                      "~/Content/fonts/averta/style.css",
                      "~/Content/fonts/icomoon/icomoon.css",
                      "~/Content/fonts/Ionicons/style.css",
                      "~/Content/fonts/flaticon/style.css",
                      "~/Content/fonts/iconfont/style.css",
                      "~/Content/vendors/animation/animate.css",
                      "~/Content/vendors/elagent/style.css",
                      "~/Content/vendors/elagent/style.css",
                      "~/Content/vendors/font-awesome/css/all.css"));

            bundles.Add(new StyleBundle("~/css/styles/fonts/fontawesome5/all").Include(
                      "~/Content/fonts/fontawesome5.10.1/css/fontawesome.css",
                      "~/Content/fonts/fontawesome5.10.1/css/brands.css",
                      "~/Content/fonts/fontawesome5.10.1/css/solid.css",
                      "~/Content/vendors/font-awesome/css/all.css"));

            bundles.Add(new StyleBundle("~/css/vendors/plugin/all").Include(
                      "~/Content/vendors/owl-carousel/assets/owl.carousel.min.css",
                      "~/Content/vendors/magnify-pop/magnific-popup.css",
                      "~/Content/vendors/nice-select/nice-select.css",
                      "~/Content/vendors/scroll/jquery.mCustomScrollbar.min.css",
                      "~/Content/vendors/themepunch//css/settings.min.css",
                      "~/Content/js/sweetalert2@11/sweetalert2.min.css"));

            bundles.Add(new StyleBundle("~/css/site").Include(
                      "~/Content/css/style.css",
                      "~/Content/css/responsive.css"));


            //bundles.Add(new StyleBundle("~/Content/css")
            //            .IncludeDirectory("~/Content/css", "*.css"));







            //bundles.Add(new StyleBundle("~/Content/styles").Include(
            //          "~/Content/css/bootstrap.min.css",
            //          "~/Content/vendors/bootstrap-selector/css/bootstrap-select.min.css",
            //          "~/Content/vendors/font-awesome/css/all.css",
            //          "~/Content/vendors/elagent/style.css",
            //          "~/Content/vendors/themify-icon/themify-icons.css",
            //          "~/Content/vendors/flaticon/flaticon.css",
            //          "~/Content/fonts/averta/style.css",
            //          "~/Content/fonts/fontawesome5.10.1/css/fontawesome.css",
            //          "~/Content/fonts/fontawesome5.10.1/css/brands.css",
            //          "~/Content/fonts/fontawesome5.10.1/css/solid.css",
            //          "~/Content/fonts/fontawesome4.7.0/style.css",
            //          "~/Content/fonts/Linearicons_v1.0.0/style.css",
            //          "~/Content/fonts/icomoon/icomoon.css",
            //          "~/Content/fonts/Ionicons/style.css",
            //          "~/Content/fonts/flaticon/style.css",
            //          "~/Content/fonts/iconfont/style.css",
            //          "~/Content/vendors/animation/animate.css",
            //          "~/Content/vendors/owl-carousel/assets/owl.carousel.min.css",
            //          "~/Content/vendors/magnify-pop/magnific-popup.css",
            //          "~/Content/vendors/nice-select/nice-select.css",
            //          "~/Content/vendors/font-awesome/css/all.css",
            //          "~/Content/vendors/elagent/style.css",
            //          "~/Content/vendors/scroll/jquery.mCustomScrollbar.min.css",
            //          "~/Content/vendors/themepunch//css/settings.min.css",
            //          "~/Content/js/sweetalert2@11/sweetalert2.min.css",
            //          "~/Content/css/style.css",
            //          "~/Content/css/responsive.css"));

            BundleTable.EnableOptimizations = true;
        }
    }
}
