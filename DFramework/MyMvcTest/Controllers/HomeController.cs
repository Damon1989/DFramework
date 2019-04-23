using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DFramework.Infrastructure;
using Microsoft.Ajax.Utilities;
using MyMvcTest.Helper;
using NPOI.HSSF.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

namespace MyMvcTest.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //var mail = new Mail("123", "432", "15201864775@163.com","417552029@qq.com");
            //var fileList = new List<string> { Extension.GetServerMapPath("~/favicon.ico") };
            //var mail = new Mail("123哈哈哈哈", "433333333333332", "417552029@qq.com",fileList);
            //SendMailHelper.SendMail(mail);
            //SendMailHelper.SendMail(new List<Mail>
            //{
            //    new Mail("1哈哈哈哈", "433333333333332", "417552029@qq.com", fileList),
            //    new Mail("2哈哈哈哈", "433333333333332", "417552029@qq.com", fileList)
            //});

            var exportBase = new ExportBase();
            var filepath = exportBase.ExportList(
                new TravelApplyPdfModel
                {
                    Title="出差申请单",
                    SubTitle="上海优读信息科技",
                    Number= "TAJCGJ1904230010",
                    Applicant="damon",
                    ApplyDept="damondept",
                    Receiver="damonreceiver",
                    PaymentAccount="account",
                    Remark="事由",
                    RemarkInfo="remark info info",
                    AccountingInfo="入账信息",
                    AccountingDept="入账部门111",
                    AccountingProject="入账项目",
                    BusinessType="业务类型111111",
                    TravelRouteList=new List<TravelRoute>
                    {
                        new TravelRoute
                        {
                            StartTime="2019-04-22",
                            StartPlace="2222",
                            EndTime="2019-04-22",
                            TravelCity="城市11",
                            TravelWay="出行方式111",
                            Accommodation="住宿111"
                        },
                        new TravelRoute
                        {
                            StartTime="-2019-04-22",
                            StartPlace="-2222",
                            EndTime="2019-04-22",
                            TravelCity="城市11232",
                            TravelWay="出行方式12211",
                            Accommodation="住宿12211"
                        }
                    },
                    CostBudgetList=new List<CostBudget>
                    {
                        new CostBudget
                        {
                            Currency="CNY",
                            CostProject="费用项目",
                            ApplyAmount="10000"
                        },
                        new CostBudget
                        {
                            Currency="CNY",
                            CostProject="费用项目111",
                            ApplyAmount="2000"
                        }
                    },
                    ApproveInfoList=new List<ApproveInfo>
                    {
                        new ApproveInfo
                        {
                            Category="业务审批",
                            Approver="审批人1",
                            ArriveTime="2019-04-22 16:35:36.890",
                            ApproveTime="2019-04-22 16:35:36.890",
                            ApproveOpinion="审批人1"
                        },
                        new ApproveInfo
                        {
                            Category="业务审批",
                            Approver="审批人2",
                            ArriveTime="2019-04-22 16:35:36.890",
                            ApproveTime="2019-04-22 16:35:36.890",
                            ApproveOpinion="意见1112"
                        }
                    }
                }
                , "class", "11", "22", true);
            ViewBag.FilePath = filepath;
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

        public ActionResult ClassProperty()
        {
            
            var cell = new MyClassA();
            var result = new StringBuilder();
            var properties= cell.GetType().GetProperties(BindingFlags.Public|BindingFlags.Instance);
            AjaxMinExtensions.ForEach(properties, prop =>
            {
                result.Append("<br />");
                result.Append($"{prop.Name} {prop.PropertyType} {prop.GetType().IsClass}");
                result.Append("<br />");
                result.Append($"{prop.PropertyType}");
                result.Append("<br />");
                result.Append($"{prop.PropertyType.HasElementType}");
                result.Append("<br />");
                result.Append($"{prop.PropertyType.IsClass}");
            });


            return Content($"{result.ToString()}");
        }

        public class MyClassA
        {

            public string Id { get; set; }
            public MyClassB ClassB { get; set; }
        }

        public class MyClassB
        {
            public string Name { get; set; }
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

            var memory = GetFileTemplate("1111", new[] {"项目名称", "1", "2"}, new CellValidation()
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

        public FileResult DownloadExcel()
        {
            

            var memory = GetFileTemplate("发票导入模板", new[] { "项目名称", "1", "2" }, new CellValidation(0,0, new List<CellData>
                {
                    new CellData("品类0","品类0"),
                    new CellData("品类1","品类1"),
                    new CellData("品类2","品类2")
                }, new List<CellData>
                {
                    new CellData("品类1", "品名1"),
                    new CellData("品类1", "品名2"),
                    new CellData("品类1", "品名3"),
                    new CellData("品类2", "品名13"),
                    new CellData("品类2", "品名23"),
                    new CellData("品类2", "品名33"),
                })
            , new CellValidation( 2, 2, new List<CellData>
                {
                    new CellData("品类00","品类00"),
                    new CellData("品类11","品类11"),
                    new CellData("品类22","品类22")
                }, new List<CellData>
                {
                    new CellData("品类11", "品名11"),
                    new CellData("品类11", "品名12"),
                    new CellData("品类11", "品名13"),
                    new CellData("品类22", "品名22"),
                    new CellData("品类22", "品名23"),
                    new CellData("品类22", "品名43"),
                })
            
                , new CellValidation( 4, 4, new List<CellData>
                {
                    new CellData("品类000","品类000"),
                    new CellData("品类111","品类111"),
                    new CellData("品类222","品类222")
                }, new List<CellData>
                {
                    new CellData("品类111", "品名11"),
                    new CellData("品类111", "品名12"),
                    new CellData("品类111", "品名13"),
                })
                
                //, new CellValidation()
                //{
                //    FirstRow = 1,
                //    LastRow = 65535,
                //    FirstCol = 4,
                //    LastCol = 4,
                //    FirstList = new List<CellData>
                //    {
                //        new CellData("品类000","品类000"),
                //        new CellData("品类111","品类111"),
                //        new CellData("品类222","品类222")
                //    }
                //}
                );

            return File(memory, "application/vnd.ms-excel", "数据模板.xlsx");

        }

        //public NpoiMemoryStream GetFileTemplate(string fileName,string[] heads,params CellValidation[] cellValidations)
        //{
        //    var memory = new NpoiMemoryStream();
        //    var workbook = new XSSFWorkbook();
        //    var sheet = workbook.CreateSheet(fileName);
        //    var row = sheet.CreateRow(0);
        //    for (var i = 0; i < heads.Length; i++)
        //    {
        //        row.CreateCell(i).SetCellValue(heads[i]);
        //    }

        //    var sheet1 = workbook.GetSheetAt(0);

        //    cellValidations.ForEach(item =>
        //    {
        //        var regions = new CellRangeAddressList(item.FirstRow,item.LastRow,item.FirstCol, item.LastCol);//约束范围：c2到c65535
        //        var helper = new XSSFDataValidationHelper((XSSFSheet)sheet1);//获得一个数据验证Helper
        //        var validation =
        //            helper.CreateValidation(helper.CreateExplicitListConstraint(item.ListOfValues),
        //                regions);//创建约束
        //        validation.CreateErrorBox("错误", "请按右侧下拉箭头选择!");
        //        validation.ShowErrorBox = true;//显示上面提示=Ture
        //        sheet1.AddValidationData(validation);//添加进去
        //    });

        //    sheet1.ForceFormulaRecalculation = true;

        //    memory.AllowClose = false;
        //    workbook.Write(memory);
        //    memory.Flush();
        //    memory.Position = 0;    // 指定内存流起始值

        //    return memory;
        //}

        public NpoiMemoryStream GetFileTemplate(string fileName, string[] heads, params CellValidation[] cellValidations)
        {
            var workbook = new XSSFWorkbook();

            var sheet = workbook.CreateSheet(fileName);
            var style = workbook.CreateCellStyle();
            style.DataFormat = HSSFDataFormat.GetBuiltinFormat("@");
            #region set heads
            var newRow = sheet.CreateRow(0);
            for (var i = 0; i < heads.Length; i++)
            {
                var cell = newRow.CreateCell(i);
                cell.CellStyle = style;
                cell.SetCellValue(heads[i]);
                for (int j = 1; j <= 65535; j++)
                {
                    var row = sheet.GetRow(j) ?? sheet.CreateRow(j);
                    cell = row.CreateCell(i);
                    cell.CellStyle = style;
                }
            }
            #endregion

            #region set datasource
            var dataNum = 0;
            cellValidations?.ToList().ForEach(item =>
            {
                if (item.FirstList == null) return;

                var dataSourceSheetName = $"dataSource{dataNum}";
                dataNum++;
                var dataSourceSheet = workbook.CreateSheet(dataSourceSheetName);//创建sheet

                for (var i = 0; i < item.FirstList.Count; i++)
                {
                    var row = dataSourceSheet.GetRow(i) ?? dataSourceSheet.CreateRow(i);//添加行

                    row.CreateCell(0).SetCellValue(item.FirstList[i].Value);//单元格写值

                    if (i > 0)
                    {
                        if (item.SecondList != null && item.SecondList.Any())
                        {
                            dataSourceSheet.GetRow(0).CreateCell(i).SetCellValue(item.FirstList[i].Value);//一级列头

                            var datas = item.SecondList.Where(c => c.Key == item.FirstList[i].Key).ToList();

                            for (var j = 0; j < datas.Count; j++)
                            {
                                var secondRow = dataSourceSheet.GetRow(j + 1) ?? dataSourceSheet.CreateRow(j + 1);
                                secondRow.CreateCell(i).SetCellValue(datas[j].Value);//单元格写值        
                            }
                        }
                    }
                }

                #region Range
                for (var i = 0; i < item.FirstList.Count; i++)
                {
                    if (i == 0)
                    {
                        var range = workbook.CreateName();
                        range.NameName = item.FirstList[i].Value;
                        range.RefersToFormula = string.Format("{0}!${3}${2}:${3}${1}",
                            dataSourceSheetName,
                            item.FirstList.Count,
                            2,
                            Index2ColName(i));
                    }
                    else
                    {
                        if (item.SecondList != null && item.SecondList.Any())
                        {
                            var cellDatas = item.SecondList.Where(c => c.Key == item.FirstList[i].Key).ToList();
                            if (cellDatas.Any())
                            {
                                var range = workbook.CreateName();
                                range.NameName = item.FirstList[i].Value;
                                range.RefersToFormula = string.Format("{0}!${3}${2}:${3}${1}",
                                    dataSourceSheetName,
                                    cellDatas.Count + 1,
                                    2,
                                    Index2ColName(i));
                            }
                        }
                    }
                }
                #endregion
            });
            #endregion


            cellValidations?.ToList().ForEach(item =>
            {
                if (item.FirstList != null && item.FirstList.Any())
                {
                    var regions = new CellRangeAddressList(item.FirstRow, item.LastRow, item.FirstCol, item.LastCol);//约束范围：c2到c65535

                    var helper = new XSSFDataValidationHelper((XSSFSheet)sheet);//获得一个数据验证Helper
                    var validation =
                        helper.CreateValidation(
                            helper.CreateFormulaListConstraint(item.FirstList[0].Value),
                            regions);//创建约束
                    validation.CreateErrorBox("错误", "请按右侧下拉箭头选择!");
                    validation.ShowErrorBox = true;//显示上面提示
                    sheet.AddValidationData(validation);//添加进去
                }
                else
                {
                    if (item.ListOfValues != null && item.ListOfValues.Any())
                    {
                        var regions = new CellRangeAddressList(item.FirstRow, item.LastRow, item.FirstCol, item.LastCol);//约束范围：c2到c65535
                        var helper = new XSSFDataValidationHelper((XSSFSheet)sheet);//获得一个数据验证Helper
                        var validation =
                            helper.CreateValidation(helper.CreateExplicitListConstraint(item.ListOfValues),
                                regions);//创建约束
                        validation.CreateErrorBox("错误", "请按右侧下拉箭头选择!");
                        validation.ShowErrorBox = true;//显示上面提示
                        sheet.AddValidationData(validation);//添加进去
                    }
                }
            });

            cellValidations?.ToList().ForEach(item =>
            {
                if (item.SecondList != null && item.SecondList.Any())
                {
                    var regions = new CellRangeAddressList(item.FirstRow, item.LastRow, item.FirstCol + 1, item.LastCol + 1);//约束范围：c2到c65535

                    var helper = new XSSFDataValidationHelper((XSSFSheet)sheet);//获得一个数据验证Helper
                    var validation =
                        helper.CreateValidation(
                            helper.CreateFormulaListConstraint($"INDIRECT(${Index2ColName(item.FirstCol)}2)"),
                            regions);//创建约束
                    validation.CreateErrorBox("错误", "请按右侧下拉箭头选择!");
                    validation.ShowErrorBox = true;//显示上面提示=Ture
                    sheet.AddValidationData(validation);//添加进去
                }
            });


            sheet.ForceFormulaRecalculation = true;



            var memory = new NpoiMemoryStream();
            memory.AllowClose = false;

            workbook.Write(memory);
            memory.Flush();
            memory.Position = 0;    // 指定内存流起始值
            return memory;

        }

        private static string Index2ColName(int index)
        {
            if (index < 0)
            {
                return null;
            }
            var num = 65;// A的Unicode码
            var colName = "";
            do
            {
                if (colName.Length>0)
                {
                    index--;
                }
                var remainder = index % 26;
                colName = ((char)(remainder + num)) + colName;
                index = (int)((index - remainder) / 26);
            } while (index > 0);
            return colName;
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
        public List<CellData> FirstList { get; set; }
        public List<CellData> SecondList { get; set; }

        public CellValidation()
        {

        }

        public CellValidation(int firstCol, int lastCol, string[] listOfValues)
            : this(1, 65535, firstCol, lastCol, listOfValues, null, null)
        {
        }

        public CellValidation(int firstRow, int lastRow, int firstCol, int lastCol, string[] listOfValues)
        :this(firstRow,lastRow,firstCol,lastCol,listOfValues,null,null)
        {
        }

        public CellValidation( int firstCol, int lastCol,
            List<CellData> firstList, List<CellData> secondList)
            : this(1, 65535, firstCol, lastCol, null, firstList, secondList)
        {

        }

        public CellValidation(int firstRow, int lastRow, int firstCol, int lastCol,
            List<CellData> firstList, List<CellData> secondList)
            : this(firstRow, lastRow, firstCol, lastCol, null, firstList,secondList)
        {

        }
        public CellValidation(int firstRow, int lastRow, int firstCol, int lastCol, string[] listOfValues,
            List<CellData> firstList, List<CellData> secondList)
        {
            FirstRow = firstRow;
            LastRow = lastRow;
            FirstCol = firstCol;
            LastCol = lastCol;
            ListOfValues = listOfValues;
            FirstList = firstList;
            SecondList = secondList;
        }

    }

    public class CellData
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public CellData()
        {

        }
        public CellData(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}