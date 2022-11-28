using System.Web;
using System.Web.Optimization;

namespace GSID.Admin
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
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

            bundles.Add(new StyleBundle("~/Content/css").Include(
                //   "~/Content/css/bootstrap.min.css",
                      "~/Content/font-awesome/css/font-awesome.css",
                      "~/Scripts/plugins/gritter/jquery.gritter.css",
                      "~/Content/css/plugins/dataTables/dataTables.bootstrap.css",
                      "~/Content/css/plugins/dataTables/dataTables.responsive.css",
                      "~/Content/css/plugins/dataTables/dataTables.tableTools.min.css",
                      "~/Content/css/plugins/iCheck/custom.css",
                      "~/Content/css/plugins/toastr/toastr.min.css",
                      "~/Content/css/plugins/chosen/chosen.css",
                      "~/Content/css/plugins/colorpicker/bootstrap-colorpicker.min.css",
                      "~/Content/css/plugins/cropper/cropper.min.css",
                      "~/Content/css/plugins/switchery/switchery.css",
                      "~/Content/css/plugins/jasny/jasny-bootstrap.min.css",
                      "~/Content/css/plugins/nouslider/jquery.nouislider.css",
                      "~/Content/css/plugins/datapicker/datepicker3.css",
                      "~/Content/css/plugins/ionRangeSlider/ion.rangeSlider.css",
                      "~/Content/css/plugins/ionRangeSlider/ion.rangeSlider.skinFlat.css",
                      "~/Content/css/animate.css",
                      "~/Content/css/style.css",
                      "~/Content/css/plugins/blueimp/css/blueimp-gallery.min.css",
                      "~/Content/css/plugins/jsTree/style.min.css",
                      "~/Content/css/plugins/summernote/summernote.css",
                      "~/Content/css/plugins/summernote/summernote-bs3.css"));
        }
    }
}
