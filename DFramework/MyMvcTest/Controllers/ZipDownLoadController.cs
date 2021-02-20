using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyMvcTest.Controllers
{
    public class ZipDownLoadController : Controller
    {
        // GET: ZipDownLoad
        public ActionResult Index()
        {
            return View();
        }

        public FilePathResult DownLoadFile()
        {
            string filePath = Server.MapPath("~/Content/11.zip");//路径
            return File(filePath, "text/plain", "welcome.zip"); //welcome.txt是客户端保存的名字
        }
    }
}