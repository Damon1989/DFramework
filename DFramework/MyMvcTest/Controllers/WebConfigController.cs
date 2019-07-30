using System.Web.Mvc;
using MyMvcTest.Helper;

namespace MyMvcTest.Controllers
{
    public class WebConfigController : Controller
    {
        // GET: WebConfig
        public ActionResult Index()
        {
            ViewBag.AppSettings = WebConfigHelper.GetWebConfigSettings();
            return View();
        }

        public ActionResult ChangeYear()
        {
            WebConfigHelper.ModifyAppSetting("Year", "2019");
            ViewBag.AppSettings = WebConfigHelper.GetWebConfigSettings();
            return View("Index");
        }

        public ActionResult YearChange(string year)
        {
            WebConfigHelper.ModifyAppSetting("Year",year);
            ViewBag.AppSettings = WebConfigHelper.GetWebConfigSettings();
            return View("Index");
        }
    }
}