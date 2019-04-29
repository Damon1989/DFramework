using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyMvcTest.Helper;

namespace MyMvcTest.Controllers
{
    public class ValidateCodeController : Controller
    {
        // GET: ValidateCode
        public ActionResult Index()
        {
            for (int i = 0; i < 100000; i++)
            {
                LoggerHelper.WriteLine("my name is damon","D");
            }
            
            return View();
        }

        public ActionResult GetValidateCode()
        {
            string code = ValidateCodeHelper.GenerateValidateCode(5);
            Session["ValidateCode"] = code;
            byte[] bytes = ValidateCodeHelper.GenerateValidateGraphic(code);
            return File(bytes, @"image/jpeg");
        }

        [HttpPost]
        public ActionResult SendSms(string phone, string code)
        {
            var ccs = Session["ValidateCode"].ToString();
            var sss = ccs != code;
            if (Session["ValidateCode"].ToString() != code)
            {
                return Json("错误");
            }

            return Json("正确");
        }
    }
}