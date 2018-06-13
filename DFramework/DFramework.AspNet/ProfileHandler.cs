using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DFramework.AspNet;

[assembly: PreApplicationStartMethod(typeof(ProfileHandler), "Init")]

namespace DFramework.AspNet
{
    public class ProfileHandler : IHttpHandler
    {
        private readonly PerformanceCounter _ramCounter;
        private readonly PerformanceCounter _cpuCounter;

        public float CurrentCpuUsage => _cpuCounter.NextValue();

        public float AvailableRAM => _ramCounter.NextValue();

        public static void Init()
        {
            RouteTable.Routes.IgnoreRoute("profile");
        }

        public ProfileHandler()
        {
            _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            _ramCounter = new PerformanceCounter("Memory", "Available MBytes");
        }

        public void ProcessRequest(HttpContext context)
        {
            var availableWorkThreads = 0;
            var availableCompletionPortThreads = 0;
            var maxWorkerThreads = 0;
            var maxCompletionPortThreads = 0;

            ThreadPool.GetMaxThreads(out maxWorkerThreads, out maxCompletionPortThreads);
            ThreadPool.GetAvailableThreads(out availableWorkThreads, out availableCompletionPortThreads);

            context.Response.Write(
                $"CurrentCpuUsage:{CurrentCpuUsage} <br /> AvailableRAM:{AvailableRAM} <br /> work threads:" +
                $"{availableWorkThreads}/{maxWorkerThreads} <br />completionPortThreads:" +
                $"{availableCompletionPortThreads}/{maxCompletionPortThreads}");

            var ipv4 = Dns.GetHostEntry(Dns.GetHostName())
                .AddressList
                .First(x => x.AddressFamily == AddressFamily.InterNetwork);
            context.Response.Write($"<br />host ip:{ipv4}");
        }

        public bool IsReusable => true;
    }
}