using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace tms_mka_v2
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {   
            CssRewriteUrlTransform cssWriter = new CssRewriteUrlTransform();

            //login
            bundles.Add(new StyleBundle("~/cssLogin")
                .Include("~/Content/metronic/assets/pages/css/login.min.css", cssWriter));
            bundles.Add(new ScriptBundle("~/jsLogin").Include(
                  "~/Content/metronic/assets/pages/scripts/login.min.js"
            ));

            //layout
            bundles.Add(new StyleBundle("~/cssLayout")
                .Include("~/Content/metronic/assets/layouts/layout3/css/layout.min.css", cssWriter)
                .Include("~/Content/metronic/assets/layouts/layout3/css/themes/default.min.css", cssWriter)
                .Include("~/Content/metronic/assets/layouts/layout3/css/custom.min.css", cssWriter)
                .Include("~/Content/metronic/assets/layouts/layout3/css/custom.min.css", cssWriter));
            bundles.Add(new ScriptBundle("~/jsLayout").Include(
                "~/Content/metronic/assets/layouts/layout3/scripts/layout.min.js",
                "~/Content/metronic/assets/layouts/layout3/scripts/demo.min.js"
            ));



            bundles.Add(new StyleBundle("~/cssKendo")
                .Include("~/Content/kendoCss/kendo.common-bootstrap.min.css", cssWriter)
                .Include("~/Content/kendoCss/kendo.bootstrap.min.css", cssWriter)
            );
            bundles.Add(new StyleBundle("~/cssApp")
                .Include("~/Content/metronic/assets/global/plugins/font-awesome/css/font-awesome.min.css", cssWriter)
                .Include("~/Content/metronic/assets/global/plugins/simple-line-icons/simple-line-icons.min.css", cssWriter)
                .Include("~/Content/metronic/assets/global/plugins/bootstrap/css/bootstrap.min.css", cssWriter)
                .Include("~/Content/metronic/assets/global/plugins/bootstrap-timepicker/css/bootstrap-timepicker.min.css", cssWriter)
                .Include("~/Content/metronic/assets/global/plugins/bootstrap-switch/css/bootstrap-switch.min.css", cssWriter)
                .Include("~/Content/metronic/assets/global/plugins/fullcalendar/fullcalendar.min.css", cssWriter)
                .Include("~/Content/metronic/assets/global/plugins/jquery-ui/jquery-ui.min.css", cssWriter)
                .Include("~/Content/metronic/assets/global/css/components-md.min.css", cssWriter)
                .Include("~/Content/metronic/assets/global/css/plugins-md.min.css", cssWriter)
                .Include("~/Content/sweet-alertCss/sweet-alert.css", cssWriter)
                .Include("~/Content/metronic/assets/global/plugins/bootstrap-toastr/toastr.min.css", cssWriter)
                .Include("~/Content/metronic/assets/global/plugins/jquery-notific8/jquery.notific8.min.css", cssWriter)
                .Include("~/Content/webapp.cs"));


            bundles.Add(new ScriptBundle("~/jsJquery").Include(
                  "~/Content/kendoJs/jquery.min.js"
            ));

            bundles.Add(new ScriptBundle("~/jsJqueryUI").Include(
                "~/Content/metronic/assets/global/plugins/jquery-ui/jquery-ui.min.js"
                ));
           
            bundles.Add(new ScriptBundle("~/jsKendo").Include(
                  "~/Content/kendoJs/kendo.all.min.js",
                  "~/Content/kendoJs/cultures/kendo.culture.id-ID.min.js"
            ));
             
            bundles.Add(new ScriptBundle("~/jsMaps").Include(
                "~/Content/metronic/assets/global/plugins/gmaps/gmaps.min.js",
                "~/Content/metronic/assets/pages/scripts/maps-google.min.js"
                ));

            bundles.Add(new ScriptBundle("~/jsApp").Include(
                "~/Content/webapp.js",
                "~/Content/metronic/assets/global/plugins/bootstrap/js/bootstrap.min.js",
                "~/Content/metronic/assets/global/plugins/bootstrap-timepicker/js/bootstrap-timepicker.min.js",
                "~/Content/metronic/assets/global/plugins/jquery-slimscroll/jquery.slimscroll.min.js",
                "~/Content/metronic/assets/global/plugins/jquery.blockui.min.js",
                "~/Content/metronic/assets/global/plugins/bootstrap-toastr/toastr.min.js",
                "~/Content/metronic/assets/global/plugins/bootbox/bootbox.min.js",
                "~/Content/metronic/assets/global/plugins/bootstrap-confirmation/bootstrap-confirmation.min.js",
                "~/Content/metronic/assets/global/plugins/jquery-notific8/jquery.notific8.min.js",
                "~/Content/metronic/assets/global/plugins/bootstrap-switch/js/bootstrap-switch.min.js",
                "~/Content/metronic/assets/global/scripts/app.min.js",
                "~/Content/metronic/assets/pages/scripts/ui-modals.min.js",
                "~/Content/metronic/assets/global/plugins/moment.min.js",
                "~/Content/metronic/assets/global/plugins/fullcalendar/fullcalendar.min.js",
                "~/Content/metronic/assets/layouts/global/scripts/quick-sidebar.min.js",
                "~/Content/metronic/assets/layouts/global/scripts/quick-nav.min.js",
                "~/Content/sweet-alertJs/sweet-alert.min.js",
                "~/Content/Jszip/jszip.min.js",
                "~/Content/Howler/src/howler.core.js"
            ));

            bundles.IgnoreList.Clear();
            BundleTable.EnableOptimizations = true;
        }
    }
}