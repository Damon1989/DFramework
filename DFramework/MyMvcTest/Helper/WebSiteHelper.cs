using System;
using System.Threading;
using DCommon;
using Microsoft.Web.Administration;

namespace MyMvcTest.Helper
{
    public class WebSiteHelper
    {
        public static readonly int SleepTime = 60;
        private static ServerManager serverManager => new ServerManager();

        public static void RecoveryWebSite()
        {
            while (true)
            {
                try
                {
                    //if (!string.IsNullOrEmpty(appPoolName))
                    //{
                        
                    //    LoggerHelper.WriteLine("-----------------");
                    //    var pools = serverManager.ApplicationPools;
                    //    foreach (var pool in pools)
                    //    {
                    //        if (pool.Name.Equals(appPoolName))
                    //        {
                    //            if (pool != null && pool.State == ObjectState.Stopped)
                    //            {
                    //                LoggerHelper.WriteLine($"检测到应用池{appPoolName}停止服务");
                    //                LoggerHelper.WriteLine($"正在启动应用池{appPoolName}");
                    //                if (pool.Start() == ObjectState.Started)
                    //                    LoggerHelper.WriteLine($"成功启动应用池{appPoolName}");
                    //                else
                    //                    LoggerHelper.WriteLine($"启动应用池{appPoolName}失败.{SleepTime / 60}秒后重试启动");
                    //            }
                    //        }
                    //    }
                    //}




                    var sites = serverManager.Sites;
                    foreach (var site in sites)
                    {
                        ApplicationDefaults defaults = site.ApplicationDefaults;
                        var appPoolName = defaults.ApplicationPoolName;
                        LoggerHelper.WriteLine($"appPoolName：{appPoolName}");
                        //if (site != null && site.State == ObjectState.Stopped)
                        //{
                        //    LoggerHelper.WriteLine($"检测到网站{websiteName}停止服务");
                        //    LoggerHelper.WriteLine($"正在启动网站{websiteName}");
                        //    if (site.Start() == ObjectState.Started)
                        //        LoggerHelper.WriteLine("成功启动网站");
                        //    else
                        //        LoggerHelper.WriteLine($"启动网站{websiteName}失败.{SleepTime / 60}秒后重试启动");
                        //}
                    }


                    //if (!string.IsNullOrEmpty(websiteName))
                    //{
                    //    var site = serverManager.Sites[websiteName];
                        
                    //}
                }
                catch (Exception e)
                {
                    LoggerHelper.WriteLine($"{e.Message}");
                }

                GC.Collect();
                Thread.Sleep(SleepTime*50);
            }
        }
    }
}