using System;
using System.Web.Optimization;

namespace WTO.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();

            //Scripts
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/contents/js/jquery.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/contents/js/bootstrap.min.js"));
            //bundles.Add(new ScriptBundle("~/bundles/UI").Include("~/contents/js/bootstrap-multiselect.js"));
            bundles.Add(new ScriptBundle("~/bundles/UI").Include("~/scripts/jquery-ui-1.8.20.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/master").Include("~/JQuery/master.js"));
            //bundles.Add(new ScriptBundle("~/bundles/validation").Include("~/Scripts/SlideSideMenu.js"));
            //bundles.Add(new ScriptBundle("~/bundles/validation").Include("~/Scripts/validations.js"));

            //CSS
            bundles.Add(new StyleBundle("~/Content/bootstrap").Include("~/contents/css/css/bootstrap.min.css"));
            //bundles.Add(new StyleBundle("~/Content/bootstrap").Include("~/contents/css/css/bootstrap.min.css", "~/Content/css/bootstrap/css/bootstrap-multiselect.css"));
            bundles.Add(new StyleBundle("~/Content/UI").Include("~/contents/css/css/jquery-ui.css"));
            bundles.Add(new StyleBundle("~/Content/css").Include("~/contents/css/css/wto.css"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            BundleTable.EnableOptimizations = false;
        }
    }
}