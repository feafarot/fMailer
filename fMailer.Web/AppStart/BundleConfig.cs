// ------------------------------------------------------------------------
// <copyright file="BundleConfig.cs" company="feafarot">
//   Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

using System.Web;
using System.Web.Optimization;

namespace fMailer.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Scripts
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include("~/Scripts/jquery-ui-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include("~/Scripts/jquery.unobtrusive*", "~/Scripts/jquery.validate*"));
            bundles.Add(new ScriptBundle("~/bundles/knockout").Include("~/Scripts/knockout-{version}.js", "~/Scripts/knockout.mapping-latest.js"));
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/Scripts/modernizr-*"));

            // Styles
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/Common.css"));
            bundles.Add(new StyleBundle("~/Content/themes/base/css")
                   .Include("~/Content/themes/base/jquery.ui.core.css",
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
        }
    }
}