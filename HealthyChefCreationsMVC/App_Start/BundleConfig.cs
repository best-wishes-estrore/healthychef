using System.Web;
using System.Web.Optimization;

namespace HealthyChefCreationsMVC
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            // added for new scripts and sheets

            bundles.Add(new ScriptBundle("~/bundles/HealthyChefScripts").Include(
                      "~/Scripts/jquery-1.9.1.min.js",
                      "~/Scripts/jquery-ui-1.10.2.custom/js/jquery-ui-1.10.2.custom.min.js",
                      "~/Scripts/jquery.validate.min.js",
                      "~/Scripts/jquery.validate.unobtrusive.min.js",
                      "~/Scripts/jquery.cookie.js",
                      "~/Scripts/functions.js",
                      "~/Scripts/jquery.formatCurrency-1.4.0.min.js",
                      "~/Scripts/loading.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/HealthyChefStyles").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/style.css",
                      "~/Content/font.css",
                      "~/Content/Base.css",
                      "~/Scripts/jquery-ui-1.10.2.custom/css/south-street/jquery-ui-1.10.2.custom.min.css"
                      ));
        }
    }
}
