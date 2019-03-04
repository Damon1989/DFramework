using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using NPOI.HSSF.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

namespace MyMvcTest.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public FileResult DownloadMapTemplete()
        {
            //var memory = new NpoiMemoryStream();
            //var workbook = new XSSFWorkbook();
            //var sheet = workbook.CreateSheet("导入地图数据模板");
            //var row = sheet.CreateRow(0);
            //row.CreateCell(0).SetCellValue("项目名称");
            //row.CreateCell(1).SetCellValue("地图名称");
            //row.CreateCell(2).SetCellValue("地图类型");
            //row.CreateCell(3).SetCellValue("经纬度");

            //var sheet1 = workbook.GetSheetAt(0);
            //var regions = new CellRangeAddressList(1, 65535, 2, 2);//约束范围：c2到c65535
            //var helper=new XSSFDataValidationHelper((XSSFSheet)sheet1);//获得一个数据验证Helper
            //var validation =
            //    helper.CreateValidation(helper.CreateExplicitListConstraint(new string[] {"项目", "标段", "桥梁", "隧道"}),
            //        regions);//创建约束
            //validation.CreateErrorBox("错误","请按右侧下拉箭头选择!");
            //validation.ShowErrorBox = true;//显示上面提示=Ture
            //sheet1.AddValidationData(validation);//添加进去
            //sheet1.ForceFormulaRecalculation = true;

            //memory.AllowClose = false;
            //workbook.Write(memory);
            //memory.Flush();
            //memory.Position = 0;    // 指定内存流起始值

            var memory = GetFileTemplatejl("1111", new[] {"项目名称", "1", "2"}, new CellValidation()
            {
                FirstRow = 1,
                LastRow = 65535,
                FirstCol = 0,
                LastCol = 0,
                ListOfValues = new[] {"项目", "标段", "桥梁", "隧道"}
            }
            //    , new CellValidation()
            //{
            //    FirstRow = 1,
            //    LastRow = 65535,
            //    FirstCol = 1,
            //    LastCol = 1,
            //    ListOfValues = new[] { "项目1", "标段2", "桥梁3", "隧道" }
            //}
                );

            return File(memory, "application/vnd.ms-excel", "数据模板.xlsx");

        }

        public NpoiMemoryStream GetFileTemplate(string fileName,string[] heads,params CellValidation[] cellValidations)
        {
            var memory = new NpoiMemoryStream();
            var workbook = new XSSFWorkbook();
            var sheet = workbook.CreateSheet(fileName);
            var row = sheet.CreateRow(0);
            for (var i = 0; i < heads.Length; i++)
            {
                row.CreateCell(i).SetCellValue(heads[i]);
            }

            var sheet1 = workbook.GetSheetAt(0);

            cellValidations.ForEach(item =>
            {
                var regions = new CellRangeAddressList(item.FirstRow,item.LastRow,item.FirstCol, item.LastCol);//约束范围：c2到c65535
                var helper = new XSSFDataValidationHelper((XSSFSheet)sheet1);//获得一个数据验证Helper
                var validation =
                    helper.CreateValidation(helper.CreateExplicitListConstraint(item.ListOfValues),
                        regions);//创建约束
                validation.CreateErrorBox("错误", "请按右侧下拉箭头选择!");
                validation.ShowErrorBox = true;//显示上面提示=Ture
                sheet1.AddValidationData(validation);//添加进去
            });
            
            sheet1.ForceFormulaRecalculation = true;

            memory.AllowClose = false;
            workbook.Write(memory);
            memory.Flush();
            memory.Position = 0;    // 指定内存流起始值

            return memory;
        }

        public NpoiMemoryStream GetFileTemplatejl(string fileName, string[] heads, params CellValidation[] cellValidations)
        {
            var memory = new NpoiMemoryStream();
            var workbook = new XSSFWorkbook();


            var sheet = workbook.CreateSheet("sheet1");//创建sheet

            var row = sheet.CreateRow(0);//添加行

            row.CreateCell(0).SetCellValue("a");//单元格写值
            row.CreateCell(1).SetCellValue("b");//单元格写值
            row.CreateCell(2).SetCellValue("c");//单元格写值
            

            row = sheet.CreateRow(1);//添加行

            row.CreateCell(0).SetCellValue("aa");//单元格写值
            row.CreateCell(1).SetCellValue("bb");//单元格写值
            row.CreateCell(2).SetCellValue("cc");//单元格写值

            row = sheet.CreateRow(2);//添加行

            row.CreateCell(0).SetCellValue("aaa");//单元格写值
            row.CreateCell(1).SetCellValue("bbb");//单元格写值
            row.CreateCell(2).SetCellValue("ccc");//单元格写值

            row = sheet.CreateRow(3);//添加行

            row.CreateCell(0).SetCellValue("b");//单元格写值
            row.CreateCell(1).SetCellValue("c");//单元格写值


            var range=workbook.CreateName();
            range.NameName = "a";
            var colName = GetExcelColumnName(0);//根据序号获取列名，具体代码见下文

            range.RefersToFormula = string.Format("{0}!${3}${2}:${3}${1}",
                "sheet1",
                4,
                2,
                "A");

            var range1 = workbook.CreateName();
            range1.NameName = "b";
            range1.RefersToFormula = string.Format("{0}!${3}${2}:${3}${1}",
                "sheet1",
                4,
                2,
                "B");

            //var range2 = workbook.CreateName();
            //range2.NameName = "c";
            //range2.RefersToFormula = string.Format("{0}!${3}${2}:${3}${1}",
            //    "sheet1",
            //    3,
            //    2,
            //    "C");



            sheet = workbook.CreateSheet(fileName);
             row = sheet.CreateRow(0);
            for (var i = 0; i < heads.Length; i++)
            {
                row.CreateCell(i).SetCellValue(heads[i]);
            }

            var sheet1 = workbook.GetSheet(fileName);

            cellValidations.ForEach(item =>
            {
                var regions = new CellRangeAddressList(item.FirstRow, item.LastRow, item.FirstCol, item.LastCol);//约束范围：c2到c65535

                var helper = new XSSFDataValidationHelper((XSSFSheet)sheet);//获得一个数据验证Helper
                var validation =
                    helper.CreateValidation(
                        helper.CreateFormulaListConstraint("a"),
                        //helper.CreateExplicitListConstraint(item.ListOfValues),
                        regions);//创建约束
                validation.CreateErrorBox("错误", "请按右侧下拉箭头选择!");
                validation.ShowErrorBox = true;//显示上面提示=Ture
                sheet1.AddValidationData(validation);//添加进去
            });

            cellValidations.ForEach(item =>
            {
                var regions = new CellRangeAddressList(item.FirstRow, item.LastRow, item.FirstCol + 1, item.LastCol + 1);//约束范围：c2到c65535

                var helper = new XSSFDataValidationHelper((XSSFSheet)sheet);//获得一个数据验证Helper
                var validation =
                    helper.CreateValidation(
                        helper.CreateFormulaListConstraint("INDIRECT($A2)"),
                        //helper.CreateExplicitListConstraint(item.ListOfValues),
                        regions);//创建约束
                validation.CreateErrorBox("错误", "请按右侧下拉箭头选择!");
                validation.ShowErrorBox = true;//显示上面提示=Ture
                sheet1.AddValidationData(validation);//添加进去
            });


            sheet1.ForceFormulaRecalculation = true;

            memory.AllowClose = false;
            workbook.Write(memory);
            memory.Flush();
            memory.Position = 0;    // 指定内存流起始值

            return memory;
        }

        private string GetExcelColumnName(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = string.Empty;
            int modulo;
            while (dividend>0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo) + columnName;
                dividend = (int) (dividend - modulo / 26);
            }

            return columnName;
        }
    }

    //新建类 重写Npoi流方法
    public class NpoiMemoryStream : MemoryStream
    {
        public NpoiMemoryStream()
        {
            AllowClose = true;
        }

        public bool AllowClose { get; set; }

        public override void Close()
        {
            if (AllowClose)
                base.Close();
        }
    }

    public class CellValidation
    {
        public int FirstRow { get; set; }
        public int LastRow { get; set; }
        public int FirstCol{ get; set; }
        public int LastCol { get; set; }
        public string[] ListOfValues { get; set; }

    }
}