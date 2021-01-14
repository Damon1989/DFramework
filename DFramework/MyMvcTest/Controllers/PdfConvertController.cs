using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyMvcTest.Helper;

namespace MyMvcTest.Controllers
{
    public class PdfConvertController : Controller
    {
        // GET: PdfConvert
        public string Index()
        {
            //return PdfHelper.ConvertToPng("D:\\temp\\.archivetemp9131023075315045X0_42c9136563494fdeabc95264f62694ea.pdf",$"D\\temp\\{Guid.NewGuid()}.png");
            return PdfHelper.ConvertToPng("D:\\temp\\.archivetemp9131023075315045X0_42c9136563494fdeabc95264f62694ea.pdf", $"D:\\temp\\{Guid.NewGuid()}.png");
        }

        public JsonResult OFD()
        {
            var result= OFDHelper.GetInvoceData("D:\\temp\\050002000213_00000003 (2).ofd");
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}