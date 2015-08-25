using System.Web;
using System.Web.Optimization;

namespace Llprk.Web.UI
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/admin").Include(
                        "~/Scripts/globalize/globalize.js",
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/knockout-3.3.0.js",
                        "~/Scripts/knockout.mapping.js",
                        "~/Scripts/underscore.js",
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/toastr.js",
                        "~/Scripts/ckeditor.js",
                        "~/Scripts/utils.js",
                        "~/Scripts/rx.js",
                        "~/Scripts/rx.time.js",
                        "~/Scripts/app/BaseViewModel.js")
                        .IncludeDirectory("~/Scripts/bootstrap", "*.js", true)
                        .IncludeDirectory("~/Scripts/ko.extenders", "*.js", true)
                        .IncludeDirectory("~/Scripts/ko.bindings", "*.js", true)
                        .IncludeDirectory("~/Areas/Admin/Scripts", "*.js", true));

            bundles.Add(new ScriptBundle("~/bundles/store").Include(
                        "~/Scripts/globalize/globalize.js",
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/underscore.js",
                        "~/Scripts/knockout-3.3.0.js",
                        "~/Scripts/knockout.mapping.js",
                        "~/Scripts/ko.bindings/ko.currency.js",
                        "~/Areas/Store/Scripts/knockout.validation.js",
                        "~/Scripts/toastr.js",
                        "~/Scripts/utils.js",
                        "~/Scripts/app/BaseViewModel.js")
                        .IncludeDirectory("~/Areas/Store/Scripts", "*.js", true));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/admin").Include(
                "~/Areas/Admin/Content/bootstrap/bootstrap.css",
                "~/Areas/Admin/Content/utils.css",
                "~/Areas/Admin/Content/codemirror.css",
                "~/Areas/Admin/Content/toastr.css",
                "~/Areas/Admin/Content/admin.css")
                .IncludeDirectory("~/Areas/Admin/Scripts/codemirror/", "*.css", true));

            bundles.Add(new StyleBundle("~/Content/shop").Include(
                "~/Content/bootstrap/bootstrap.css",
                "~/Content/utils.css",
                "~/Content/shop.css"));

            bundles.Add(new StyleBundle("~/Content/shop2").Include(
                "~/Content/bootstrap.css",
                "~/Content/utils.css",
                "~/Content/shop2.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));

#if !DEBUG
            BundleTable.EnableOptimizations = true;
#endif
        }
    }
}