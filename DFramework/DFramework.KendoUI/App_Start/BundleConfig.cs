using System.Web.Optimization;

namespace DFramework.KendoUI.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                "~/Scripts/common.js",
                "~/Scripts/select2.min.js",
                        "~/Scripts/bootstrap-alert.js"));

            bundles.Add(new ScriptBundle("~/bundles/kendo").Include(
                "~/Scripts/kendo.web.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-theme.css",
                      "~/Content/Site.css",
                "~/Content/kendo.common.min.css",
                "~/Content/kendo.default.min.css",
                "~/Content/select2.min.css"));

            //bundles.Add(new ScriptBundle("~/bundles/ueditor").Include(
            //       "~/ueditor/ueditor.config.js",
            //       "~/ueditor/ueditor.all.js",
            //       "~/ueditor/lang/zh-cn/zh-cn.js",
            //       "~/Scripts/ue.js"));

            // bundles.Add(new ScriptBundle("~/bundles/hubs").Include(
            //       "~/Scripts/Hubs/conference.js",
            //       "~/Scripts/Hubs/userlist.js",
            //       "~/Scripts/Hubs/chat.js"
            //));
            BundleTable.EnableOptimizations = false;
        }
    }
}