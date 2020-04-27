using System;
using System.Web.Mvc;
using DCommon;
using MyMvcTest.Helper;

namespace MyMvcTest.Controllers
{
    public class ValidateCodeController : Controller
    {
        // GET: ValidateCode
        public ActionResult Index()
        {
            //for (int i = 0; i < 100000; i++)
            //{
            //    LoggerHelper.WriteLine("my name is damon","D");
            //}
            try
            {
                var i = 0;
                var s = i / 0;
            }
            catch (Exception e)
            {
                LoggerHelper.WriteLine($"{e}");
            }

            return View();
        }

        public ActionResult GetValidateCode()
        {
            var code = ValidateCodeHelper.GenerateValidateCode(5);
            Session["ValidateCode"] = code;
            var bytes = ValidateCodeHelper.GenerateValidateGraphic(code);
            return File(bytes, @"image/jpeg");
        }

        [HttpPost]
        public ActionResult SendSms(string phone, string code)
        {
            var ccs = Session["ValidateCode"].ToString();
            var sss = ccs != code;
            if (Session["ValidateCode"].ToString() != code) return Json("错误");

            return Json("正确");
        }
    }
}