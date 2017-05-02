using System.Web;
using System.Web.Optimization;

namespace RefactorName.WebApp
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        //public static void RegisterBundles(BundleCollection bundles)
        //{
        //    bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
        //                "~/Scripts/jquery-{version}.js"));

        //    bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
        //                "~/Scripts/jquery.validate*"));

        //    // Use the development version of Modernizr to develop with and learn from. Then, when you're
        //    // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
        //    bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
        //                "~/Scripts/modernizr-*"));

        //    bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
        //              "~/Scripts/bootstrap.js",
        //              "~/Scripts/respond.js"));

        //    bundles.Add(new StyleBundle("~/Content/css").Include(
        //              "~/Content/bootstrap.css",
        //              "~/Content/site.css"));

        //    bundles.Add(new StyleBundle("~/Content/templateCss").Include(
        //        "~/Content/template.css",
        //        "~/Content/MCI-Alerts.css",
        //        "~/Content/bootstrap-multiselect.css",
        //        "~/Content/select2.css"
        //        ));

        //    //Layout Jquery primary files
        //    bundles.Add(new ScriptBundle("~/bundles/layoutJquery").Include(
        //                "~/Scripts/jquery.min.js",
        //                "~/Scripts/bootstrap.min.js",
        //                 "~/Scripts/mega-menu.js",
        //                 "~/Scripts/sidebar.js",
        //                 "~/Scripts/chosen.jquery.js",
        //                 "~/Scripts/bootstrap-checkbox.min.js"));
        //}


        public static void RegisterBundles(BundleCollection bundles)
        {
            #region Scripts Bundles

            //Layout Jquery primary files
            bundles.Add(new ScriptBundle("~/bundles/layoutJquery").Include(
                        "~/Scripts/jquery.min.js",
                        "~/Scripts/bootstrap.min.js",
                         "~/Scripts/mega-menu.js",
                         "~/Scripts/sidebar.js",
                         "~/Scripts/chosen.jquery.js",
                         "~/Scripts/bootstrap-checkbox.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/templateJquery").Include(
                "~/Scripts/jquery.filter_input.js",
                "~/Scripts/jquery.placholder.js",
                "~/Scripts/select2.full.js",
                "~/Scripts/select2.ar.js",
                "~/Scripts/select2_locale_ar.js",
                "~/Scripts/bootstrap-multiselect.js",
                "~/Scripts/common.js",
                "~/Scripts/jquery.easing.js",
                "~/Scripts/MCI-Alerts.js",
                "~/Scripts/MCI.Validations.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-1.10.2.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/MvcFoolproofjqueryval").Include(
             "~/Scripts/MicrosoftAjax.js",
             "~/Scripts/MicrosoftMvcAjax.js",
             "~/Scripts/MicrosoftMvcValidation.js",
             "~/Scripts/mvcfoolproof.unobtrusive.js",
             "~/Scripts/MvcFoolproofJQueryValidation.js",
             "~/Scripts/MvcFoolproofValidation.js"));


            //lightBox
            bundles.Add(new ScriptBundle("~/bundles/prettyPhoto").Include(
              "~/Scripts/prettyPhoto.js"));


            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));


            //fine-Uploader
            bundles.Add(new ScriptBundle("~/bundles/finuploader")
                .Include("~/Scripts/fin-uploader/header.js",
                        "~/Scripts/fin-uploader/util.js",
                        "~/Scripts/fin-uploader/features.js",
                        "~/Scripts/fin-uploader/promise.js",
                        "~/Scripts/fin-uploader/button.js",
                        "~/Scripts/fin-uploader/ajax.requester.js",
                        "~/Scripts/fin-uploader/deletefile.ajax.requester.js",
                        "~/Scripts/fin-uploader/handler.base.js",
                        "~/Scripts/fin-uploader/window.receive.message.js",
                        "~/Scripts/fin-uploader/handler.form.js",
                        "~/Scripts/fin-uploader/handler.xhr.js",
                        "~/Scripts/fin-uploader/uploader.basic.js",
                        "~/Scripts/fin-uploader/dnd.js",
                        "~/Scripts/fin-uploader/uploader.js",
                        "~/Scripts/fin-uploader/jquery-plugin.js",
                        "~/Scripts/fin-uploader/instance.js"));

            //Hijri datePicker
            bundles.Add(new ScriptBundle("~/bundles/datepicker")
                .Include("~/Scripts/calendar/jquery.plugin.js",
                        "~/Scripts/calendar/jquery.calendars.js",
                        "~/Scripts/calendar/jquery.calendars.plus.js",
                        "~/Scripts/calendar/jquery.calendars.picker-en.js",
                        "~/Scripts/calendar/jquery.calendars.ummalqura.min.js",
                        "~/Scripts/calendar/jquery.calendars.picker.js",
                        "~/Scripts/calendar/jquery.calendars.ummalqura.js",
                        "~/Scripts/calendar/jquery.calendars-ar.js",
                        "~/Scripts/calendar/jquery.calendars.ummalqura-ar.js",
                        "~/Scripts/calendar/jquery.calendars.picker-ar.js"
                ));


            //Signature
            bundles.Add(new ScriptBundle("~/bundles/signature")
                .IncludeDirectory("~/Scripts/signiture", "*.js"));

            //typeahead
            bundles.Add(new ScriptBundle("~/bundles/typeahead")
                .Include("~/Scripts/typeahead.bundle.js",
                "~/Scripts/handlebars.js")
                );


            //Star rating
            bundles.Add(new ScriptBundle("~/bundles/starrating").Include(
                "~/Scripts/star-rating.js",
                "~/Scripts/star-rating-defaults.js"));

            //MCI High Charts
            bundles.Add(new ScriptBundle("~/bundles/mcicharts")
                .IncludeDirectory("~/Scripts/mciCharts", "*.js"));


            //Angular Multiselect tree
            bundles.Add(new ScriptBundle("~/bundles/angulartree").Include(
                "~/Scripts/angular.min.js",
                "~/Scripts/angular-multi-select-tree-0.1.0.js",
                "~/Scripts/angular-multi-select-tree-0.1.0.tpl.js",
                "~/Scripts/angular-multi-select-tree-0.1.0-main.js"));

            #endregion

            #region Styles Bundles

            //Main css
            bundles.Add(new StyleBundle("~/Content/mainCss").Include(
                "~/Content/layout.css",
                "~/Content/baseRTL.css",
                "~/Content/components.css",
                "~/Content/admin-css.css",
                "~/Content/font-awesome.min.css"
                ));

            bundles.Add(new StyleBundle("~/Content/templateCss").Include(
                "~/Content/template.css",
                "~/Content/MCI-Alerts.css",
                "~/Content/bootstrap-multiselect.css",
                "~/Content/select2.css"
                ));


            //fin uploader             
            bundles.Add(new StyleBundle("~/Content/finuploader").Include(
                "~/Content/fin-uploader.css"));

            ////Calendar css
            //bundles.Add(new StyleBundle("~/Content/datepicker")
            //   .Include("~/Content/jquery.calendars.picker.css")
            //    );

            //bootstrap css
            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                "~/Content/bootstrap.css",
                "~/Content/bootstrap-theme.css",
                "~/Content/bootstrap-theme-custom.css",
                "~/Content/bootstrap-rtl.css",
                "~/Content/bootstrap-multiselect.css",
                "~/Content/select2.css"
                ));

            //lightbox css
            bundles.Add(new StyleBundle("~/Content/prettyPhoto").Include(
                "~/Content/prettyPhoto.css"));

            //signature
            bundles.Add(new StyleBundle("~/Content/signature").Include(
                "~/Content/jquery.signature.css"));

            //star rating
            bundles.Add(new StyleBundle("~/Content/starrating").Include(
                "~/Content/star-rating.css"));

            //typeahead
            bundles.Add(new StyleBundle("~/Content/typeahead").Include(
                "~/Content/typeahead.css"));

            //angular tree
            bundles.Add(new StyleBundle("~/Content/angulartree").Include(
                "~/Content/angular-multi-select-tree-0.1.0.css"));
            #endregion

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
#if (DEBUG)
            BundleTable.EnableOptimizations = false;
#else
            BundleTable.EnableOptimizations = true;
#endif

        }
    }
}
