using System.Web;
using System.Web.Optimization;

namespace Admin
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-2.2.4.js",
                        "~/Scripts/bootstrap.js",
                        "~/plugins/bootstrap-select/js/bootstrap-select.js",
                        "~/plugins/bootstrap-select/js/bootstrap-select.min.js",
                        "~/plugins/jquery-slimscroll/jquery.slimscroll.js",
                        "~/plugins/node-waves/waves.js",
                        "~/plugins/jquery-countto/jquery.countTo.js",
                        "~/plugins/raphael/raphael.min.js",
                        "~/plugins/morrisjs/morris.js",
                        "~/plugins/chartjs/Chart.bundle.js",
                        "~/plugins/flot-charts/jquery.flot.js",
                        "~/plugins/flot-charts/jquery.flot.resize.js",
                        "~/plugins/flot-charts/jquery.flot.pie.js",
                        "~/plugins/flot-charts/jquery.flot.categories.js",
                        "~/plugins/flot-charts/jquery.flot.time.js",
                        "~/plugins/jquery-sparkline/jquery.sparkline.js",
                        "~/plugins/autosize/autosize.js",
                        "~/plugins/momentjs/moment.js",
                        "~/plugins/bootstrap-material-datetimepicker/js/bootstrap-material-datetimepicker.js",
                        "~/plugins/bootstrap-datepicker/js/bootstrap-datepicker.js",
                        "~/Scripts/admin.js",
                        "~/Scripts/basic-form-elements.js",
                        "~/Scripts/pages/index.j",
                        "~/Scripts/demo.js",
                        "~/plugins/chartjs/Chart.min.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                         "~/Scripts/jquery.validate*"));

            // Utilizzare la versione di sviluppo di Modernizr per eseguire attività di sviluppo e formazione. Successivamente, quando si è
            // pronti per passare alla produzione, usare lo strumento di compilazione disponibile all'indirizzo https://modernizr.com per selezionare solo i test necessari.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/plugins/bootstrap-select/css/bootstrap-select.css",
                      "~/plugins/node-waves/waves.css",
                      "~/plugins/animate-css/animate.css",
                      "~/plugins/bootstrap-material-datetimepicker/css/bootstrap-material-datetimepicker.css",
                      "~/plugins/bootstrap-datepicker/css/bootstrap-datepicker.css",
                      "~/plugins/morrisjs/morris.css",
                      "~/Content/style.css",
                      "~/Content/themes/all-themes.css",
                      "~/Content/Site.css"));
        }
    }
}
