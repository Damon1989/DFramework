using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Npoi.Mapper;
using Npoi.Mapper.Attributes;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

namespace DFramework.KendoUI.Controllers
{
    public class NpoiMapperController : Controller
    {
        // GET: NpoiMapper
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ExportFile()
        {
            var persons = new List<Person>()
            {
                new Person("name", 10),
                new Person("name2", 20)
            };
            var mapper = new Mapper();
            try
            {
                mapper.Save("D://test.xlsx", persons, sheetIndex: 0, overwrite: false);

                string path = @"D://test.xlsx";

                //把文件内容导入到工作薄当中，然后关闭文件
                FileStream fs =System.IO.File.OpenRead(path);
                IWorkbook workbook = new XSSFWorkbook(fs);
                fs.Close();

                ISheet sheet = workbook.GetSheetAt(0);
                sheet.AddMergedRegion(new CellRangeAddress(0, 1, 0, 0));
                sheet.AddMergedRegion(new CellRangeAddress(0, 1, 1, 1));
                sheet.AddMergedRegion(new CellRangeAddress(0, 0, 2, 3));
                var row= sheet.GetRow(0);
                //var row = sheet.GetRow(0);
                row.Cells[2].SetCellValue("地址");
                //sheet.AddMergedRegion(new CellRangeAddress(0, 1, 1, 1));
                //sheet.GetRow(0).Cells[0].SetCellValue("年龄");
                //把编辑过后的工作薄重新保存为excel文件
                FileStream fs2 = System.IO.File.Create(@"D://test1.xlsx");
                workbook.Write(fs2);
                fs2.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            return Json("Success",JsonRequestBehavior.AllowGet);
        }
    }

    public class Person
    {
        [Display(Name = "姓名")]
        public string Name { get; set; }
        [Display(Name = "年龄")]
        public int Age { get; set; }
        [Display(Name = "区县")]
        public string District { get; set; }
        [Display(Name = "街道")]
        public string Street { get; set; }

        public Person(string name, int age)
        {
            Name = name;
            Age = age;
            District = "区县";
            Street = "街道";
        }
    }
}