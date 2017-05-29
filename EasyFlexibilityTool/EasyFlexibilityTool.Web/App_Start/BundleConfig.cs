using System.Web.Optimization;

namespace EasyFlexibilityTool.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/site").Include(
                      "~/Scripts/respond.js",
                      "~/Scripts/moment.js",
                      "~/Scripts/toastr.js",
                      "~/Scripts/jsgrid-{version}.js",
                      "~/Scripts/konva-{version}.js",
                      "~/Scripts/jquery.modal.js",
                      "~/Scripts/exif.js",
                      "~/Scripts/theme/core.min.js",
                      "~/Scripts/theme/script.js",
                      "~/Scripts/site.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/toastr.css",
                      "~/Content/jsgrid-{version}.css",
                      "~/Content/jsgrid-theme-{version}.css",
                      "~/Content/font-awesome.css",
                      "~/Content/jquery.modal.css",
                      "~/Content/themes/base/jquery-ui.css",
                      "~/Content/themes/base/datepicker.css",
                      "~/Content/theme/style.css",
                      "~/Content/site.css"));
        }
    }
}
