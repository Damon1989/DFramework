using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.Web.Administration;
using MyMvcTest.Helper;

namespace MyMvcTest.Controllers
{
    public class WebSiteController : Controller
    {
        // GET: WebSite
        public ActionResult Index()
        {
            //new TaskFactory().StartNew(() => { WebSiteHelper.RecoveryWebSite(); });

            ServerManager serverManager = new ServerManager();
            foreach (var pool in serverManager.ApplicationPools)
            {
                LoggerHelper.WriteLine($"{pool.Name}");
            }

            LoggerHelper.WriteLine("=====================");
            for (int i = 0; i < serverManager.ApplicationPools.Count; i++)
            {
                LoggerHelper.WriteLine(serverManager.ApplicationPools[i].Name);
            }

            return View();
        }
    }
}