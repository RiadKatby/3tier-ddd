using RefactorName.Core;
using System.Web;
using System.Web.Optimization;

namespace RefactorName.WebApp
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region Scripts Bundles

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-1.10.2.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));


            //Jquey helpers
            bundles.Add(new ScriptBundle("~/bundles/jqueryhelpers").Include(
                        "~/Scripts/jquery.placholder.js",
                        "~/Scripts/jquery.easing.js",
                         "~/Scripts/jquery.filter_input.js",
                         "~/Scripts/jquery.hoverIntent.minified.js",
                         "~/Scripts/jquery.glob.js",
                        "~/Scripts/jQuery.glob.ar-SA.js"));

            bundles.Add(new ScriptBundle("~/bundles/MvcFoolproofjqueryval").Include(
                         "~/Scripts/MicrosoftAjax.js",
                         "~/Scripts/MicrosoftMvcAjax.js",
                         "~/Scripts/MicrosoftMvcValidation.js",
                         "~/Scripts/mvcfoolproof.unobtrusive.js",
                         "~/Scripts/MvcFoolproofJQueryValidation.js",
                         "~/Scripts/MvcFoolproofValidation.js"));

            //Common JS
            bundles.Add(new ScriptBundle("~/bundles/common").Include(
                                "~/Scripts/common.js",
                                "~/Scripts/refactorName.ui.snackbar.js",
                                "~/Scripts/refactorName.ui.confirmable-action-link.js",
                                "~/Scripts/App.js",
                                "~/Scripts/MCI.Validations.js"));

            //lightBox
            bundles.Add(new ScriptBundle("~/bundles/prettyPhoto").Include(
              "~/Scripts/prettyPhoto.js"));


            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/html5shiv.js",
                      "~/Scripts/select2.js",
                      "~/Scripts/select2_locale_ar.js",
                      "~/Scripts/bootstrap-multiselect.js"));

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
             .Include("~/Scripts/jquery.plugin.js",
                        "~/Scripts/jquery.calendars.all.js",
                        "~/Scripts/jquery.calendars.ummalqura.js",
                        "~/Scripts/jquery.calendars-ar.js",
                        "~/Scripts/jquery.calendars.ummalqura-ar.js"));


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

            //Main
            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/font.css",
                "~/Content/Site.css",
                "~/Content/Main.css",
                "~/Content/style.css",
                "~/Content/refactorName.ui.snackbar.css",
                "~/Content/font-awesome.css"
                ));

            //fin uploader             
            bundles.Add(new StyleBundle("~/Content/finuploader").Include(
                "~/Content/fin-uploader.css"));

            //Calendar css
            bundles.Add(new StyleBundle("~/Content/datepicker")
               .Include("~/Content/jquery.calendars.picker.css")
                );

            //bootstrap css
            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                "~/Content/bootstrap.css",
                "~/Content/bootstrap-theme.css",
                "~/Content/bootstrap-theme-custom.css",
                "~/Content/bootstrap-rtl.css",
                "~/Content/bootstrap-multiselect.css",
                "~/Content/select2.css"
                //"~/Content/select2-bootstrap.css"
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

            BundleTable.EnableOptimizations = false;// Settings.Provider.EnableOptimizations;
        }
    }
}
