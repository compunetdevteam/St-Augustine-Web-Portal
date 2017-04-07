using System.Web.Optimization;

namespace StAugustine
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                "~/Scripts/jquery-{version}.js"
            // needed for drag/move events in fullcalendar

            ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.unobtrusive*",
                "~/Scripts/jquery.validate*",
                  "~/Scripts/chosen.jquery.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/site.css",
                "~/Content/bootstrap.css",
                // "~/Content/fullcalendar.css",
                //"~/Content/fullcalendar.print.css",
                "~/Content/chosen.css"
            ));

            bundles.Add(new StyleBundle("~/Content/fullcalendarcss").Include(
                    "~/Content/themes/jquery.ui.all.css",
                    "~/Content/fullcalendar.css"));

            //Calendar Script file

            bundles.Add(new ScriptBundle("~/bundles/fullcalendarjs").Include(
                       "~/Scripts/jquery-ui-{version}.js",

                "~/Scripts/bootstrap.js",
                "~/Scripts/bootstrap-modal.js",
                      "~/Scripts/fullcalendar.min.js"));

            BundleTable.EnableOptimizations = true;
        }
    }

}
