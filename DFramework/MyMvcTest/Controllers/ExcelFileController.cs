using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MyMvcTest.Helper;

namespace MyMvcTest.Controllers
{
    public class ExcelFileController:Controller
    {
        public ActionResult File()
        {
            var heads=new List<HeadInfo>()
            {
                new HeadInfo("序号"),
                new HeadInfo("费用科目编码"),
                new HeadInfo("费用科目名称")
            };

            // list  下拉数据源
            var list = new List<MyMvcTest.Helper.CellValidation>()
            {
                new MyMvcTest.Helper.CellValidation("费用科目",1, 1, new List<MyMvcTest.Helper.CellData>
                {
                    new MyMvcTest.Helper.CellData("费用科目名称", "费用代码名称"),
                    new MyMvcTest.Helper.CellData("火车票", "EXP101"),
                    new MyMvcTest.Helper.CellData("飞机票", "EXP105"),
                    new MyMvcTest.Helper.CellData("汽车票", "EXP106")
                })
            };

            var memory = ExcelDownLoadHelper.GetSimpleFileTemplate("模板", heads,

                list, (sheet) =>
                {
                    for (int i = 1; i <= 200; i++)
                    {
                        sheet.GetRow(i).GetCell(2)
                            ?.SetCellFormula("VLOOKUP($B" + (i + 1) + ",费用科目!$A$1:$B$9,2,FALSE)");
                    }
                },200
            );

            return File(memory, "application/vnd.ms-excel", "数据模板.xlsx");
        }


        public ActionResult DynamicFile()
        {
            var heads = new List<HeadInfo>()
            {
                new HeadInfo("序号"),
                new HeadInfo("邮箱"),
                new HeadInfo("公司"),
                new HeadInfo("部门名称"),
                new HeadInfo("部门编码"),
                new HeadInfo("人员编码"),
                new HeadInfo("姓名")
            };

            // list  下拉数据源
            var list = new List<MyMvcTest.Helper.CellValidation<ExcelEmployeeInfo>>()
            {
                new MyMvcTest.Helper.CellValidation<ExcelEmployeeInfo>("人员信息",1, 1, new List<MyMvcTest.Helper.ExcelEmployeeInfo>
                {
                    new ExcelEmployeeInfo(){Email = "邮箱",Code = "人员编号",Name = "人员名称",Company = "所属公司",OrgCd = "部门编码",OrgName = "部门名称"},
                    new ExcelEmployeeInfo(){Email = "wangting01@nuctech.com",Code = "91010002",Name = "汪婷",Company = "智能产品事业部",OrgCd = "9101008010",OrgName = "智能产品_供应链部"},
                    new ExcelEmployeeInfo(){Email = "anning@nuctech.com",Code = "89697",Name = "赵建1",Company = "智能产品事业部",OrgCd = "9101007010",OrgName = "智能产品_制造部"},
                    new ExcelEmployeeInfo(){Email = "baiweiguo@nuctech.com",Code = "1933",Name = "闫云松",Company = "智能产品事业部",OrgCd = "9101005010",OrgName = "智能产品_市场推广部"},
                    new ExcelEmployeeInfo(){Email = "baiweiguo@nuctech.com",Code = "0094",Name = "张恒",Company = "智能产品事业部",OrgCd = "9101009010",OrgName = "智能产品_质量工艺部"},
                })
            };

            var memory = ExcelDownLoadHelper.GetSimpleFileTemplate<ExcelEmployeeInfo>("模板", heads,

                list, (sheet) =>
                {
                    for (int i = 1; i <= 200; i++)
                    {
                        sheet.GetRow(i).GetCell(2)
                            ?.SetCellFormula("VLOOKUP($B" + (i + 1) + ",人员信息!$A:D,4,FALSE)");
                        sheet.GetRow(i).GetCell(3)
                            ?.SetCellFormula("VLOOKUP($B" + (i + 1) + ",人员信息!$A:F,6,FALSE)");
                        sheet.GetRow(i).GetCell(4)
                            ?.SetCellFormula("VLOOKUP($B" + (i + 1) + ",人员信息!$A:E,5,FALSE)");
                        sheet.GetRow(i).GetCell(5)
                            ?.SetCellFormula("VLOOKUP($B" + (i + 1) + ",人员信息!$A:B,2,FALSE)");
                        sheet.GetRow(i).GetCell(6)
                            ?.SetCellFormula("VLOOKUP($B" + (i + 1) + ",人员信息!$A:C,3,FALSE)");
                    }
                }
            );

            return File(memory, "application/vnd.ms-excel", "数据模板.xlsx");
        }
    }

}
