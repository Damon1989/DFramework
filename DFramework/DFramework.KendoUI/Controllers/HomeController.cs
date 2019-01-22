using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using DFramework.KendoUI.Domain;
using DFramework.KendoUI.Models;
using DFramework.UnitOfWork;
using DFramework.Zxing;
using ZXing;
using ZXing.Common;

namespace DFramework.KendoUI.Controllers
{
    public class HomeController : Controller
    {
        //private readonly KendoDbContext _kendoDbContext;
        //private readonly IUnitOfWork _appUnitOfWork;
        //private readonly IKendoUIRepository _domainRepository;

        //public HomeController(KendoDbContext kendoDbContext,
        //    IUnitOfWork appUnitOfWork,
        //    IKendoUIRepository domainRepository)
        //{
        //    //_kendoDbContext = kendoDbContext;
        //    //_appUnitOfWork = appUnitOfWork;
        //    //_domainRepository = domainRepository;
        //}

        // GET: Home
        public ActionResult Index()
        {
            //_kendoDbContext.Files.FirstOrDefault();
            //var node = new Node();
            //_domainRepository.Add(node);
            //_kendoDbContext.Nodes.Add(node);
            //_appUnitOfWork.Commit();
            return View();
        }

        public ActionResult UnderScore()
        {
            return View();
        }

        public ActionResult Kendo()
        {
            return View();
        }

        public ActionResult DataSourceRemote()
        {
            return View();
        }

        public ActionResult Dialog()
        {
            return View();
        }

        public ActionResult PredefinedDialogs()
        {
            return View();
        }

        public ActionResult DialogMVVM()
        {
            return View();
        }

        public ActionResult Extend()
        {
            //_kendoDbContext.Files.FirstOrDefault();
            //var node = new Node();
            //_domainRepository.Add(node);
            //_kendoDbContext.Nodes.Add(node);
            //_appUnitOfWork.Commit();
            return View();
        }

        public ActionResult Mvvm()
        {
            return View();
        }

        public ActionResult Alert()
        {
            return View();
        }

        public ActionResult Alterts()
        {
            return View();
        }

        public ActionResult Select2()
        {
            return View();
        }

        public ActionResult JsTree()
        {
            return View();
        }

        public ActionResult JsTreeStyle()
        {
            return View();
        }

        public ActionResult JsRegular()
        {
            return View();
        }

        public ActionResult JsRender()
        {
            return View();
        }

        public ActionResult SweetAlert2()
        {
            return View();
        }

        public ActionResult BarCode()
        {
            ZXingHelper.GenerateBarCode("023456789012");
            return View();
        }

        public ActionResult ReadBarCode()
        {
            var result = ZXingHelper.ReadCode($"{AppDomain.CurrentDomain.BaseDirectory}/EAN_13-023456789012.jpg");
            ViewBag.Result = result;
            return View();
        }

        public ActionResult QrCode()
        {
            ZXingHelper.GenerateQrCode("123456927123", logoFilePath: $"{AppDomain.CurrentDomain.BaseDirectory}Images\\logo.png");
            ZXingHelper.GenerateQrCode("123456927023", logoFilStream: new FileStream($"{AppDomain.CurrentDomain.BaseDirectory}Images\\logo.png", FileMode.Open));
            var result = ZXingHelper.ReadCode(
                $"{AppDomain.CurrentDomain.BaseDirectory}Zxing\\qrcode\\2018\\6\\QR_CODE\\123456927123.jpg",
                BarcodeFormat.QR_CODE);
            var result1 = ZXingHelper.ReadCode(
                $"{AppDomain.CurrentDomain.BaseDirectory}Zxing\\qrcode\\2018\\6\\QR_CODE\\123456927023.jpg",
                BarcodeFormat.QR_CODE);
            ViewBag.Result = result;
            ViewBag.Result1 = result1;
            return View();
        }

        public ActionResult Promise()
        {
            return View();
        }
    }
}