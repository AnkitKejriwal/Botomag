using System.Web;
using System.Web.Optimization;

namespace Botomag
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/unobtrusive-ajax").Include(
                        "~/Scripts/jquery.unobtrusive-ajax.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/owl-carousel").Include(
                        "~/Scripts/owl.carousel.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqGrid").Include(
                        "~/Scripts/jquery.jqGrid.js",
                        "~/Scripts/grid.locale-ru.js"));

            bundles.Add(new ScriptBundle("~/bundles/main").Include(
                        "~/Scripts/mainscripts.js"));

            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                        "~/Content/bootstrap.css",
                        "~/Content/normalize.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/owl-carousel").Include(
                        "~/Content/owl.carousel.css"));

            bundles.Add(new StyleBundle("~/Content/jqGrid").Include(
                        "ui.jqgrid-bootstrap.css"));
        }
    }
}