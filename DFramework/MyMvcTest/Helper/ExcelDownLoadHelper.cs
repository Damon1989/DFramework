using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using DFramework.Infrastructure;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using DCommon;

namespace MyMvcTest.Helper
{
    public static class ExcelDownLoadHelper
    {
        public static NpoiMemoryStream GetFileTemplate(string fileName
                                                     , string[] heads
                                                     , params CellValidation[] cellValidations)
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
                    if (item.FirstCellLocation != null)
                    {
                        var regions = new CellRangeAddressList(item.FirstCellLocation.FirstRow, item.FirstCellLocation.LastRow, item.FirstCellLocation.FirstCol, item.FirstCellLocation.LastCol);//约束范围：c2到c65535

                        var helper = new XSSFDataValidationHelper((XSSFSheet)sheet);//获得一个数据验证Helper
                        var validation =
                            helper.CreateValidation(
                                helper.CreateFormulaListConstraint(item.FirstList[0].Value),
                                regions);//创建约束
                        validation.CreateErrorBox("错误", "请按右侧下拉箭头选择!");
                        validation.ShowErrorBox = true;//显示上面提示
                        sheet.AddValidationData(validation);//添加进去
                    }

                }
                else
                {
                    if (item.ListOfValues != null && item.ListOfValues.Any())
                    {
                        if (item.FirstCellLocation != null)
                        {
                            var regions = new CellRangeAddressList(item.FirstCellLocation.FirstRow, item.FirstCellLocation.LastRow, item.FirstCellLocation.FirstCol, item.FirstCellLocation.LastCol);//约束范围：c2到c65535
                            var helper = new XSSFDataValidationHelper((XSSFSheet)sheet);//获得一个数据验证Helper
                            var validation =
                                helper.CreateValidation(helper.CreateExplicitListConstraint(item.ListOfValues),
                                    regions);//创建约束
                            validation.CreateErrorBox("错误", "请按右侧下拉箭头选择!");
                            validation.ShowErrorBox = true;//显示上面提示
                            sheet.AddValidationData(validation);//添加进去
                        }

                    }
                }
            });

            cellValidations?.ToList().ForEach(item =>
            {
                if (item.SecondList != null && item.SecondList.Any())
                {
                    if (item.SecondCellLocation != null)
                    {
                        var regions = new CellRangeAddressList(item.SecondCellLocation.FirstRow, item.SecondCellLocation.LastRow, item.SecondCellLocation.FirstCol, item.SecondCellLocation.LastCol);//约束范围：c2到c65535

                        var helper = new XSSFDataValidationHelper((XSSFSheet)sheet);//获得一个数据验证Helper
                        var validation =
                            helper.CreateValidation(
                                helper.CreateFormulaListConstraint($"INDIRECT(${Index2ColName(item.FirstCellLocation.FirstCol)}2)"),
                                regions);//创建约束
                        validation.CreateErrorBox("错误", "请按右侧下拉箭头选择!");
                        validation.ShowErrorBox = true;//显示上面提示=Ture
                        sheet.AddValidationData(validation);//添加进去
                    }
                    else
                    {
                        if (item.FirstCellLocation != null)
                        {
                            var regions = new CellRangeAddressList(item.FirstCellLocation.FirstRow, item.FirstCellLocation.LastRow, item.FirstCellLocation.FirstCol + 1, item.FirstCellLocation.LastCol + 1);//约束范围：c2到c65535

                            var helper = new XSSFDataValidationHelper((XSSFSheet)sheet);//获得一个数据验证Helper
                            var validation =
                                helper.CreateValidation(
                                    helper.CreateFormulaListConstraint($"INDIRECT(${Index2ColName(item.FirstCellLocation.FirstCol)}2)"),
                                    regions);//创建约束
                            validation.CreateErrorBox("错误", "请按右侧下拉箭头选择!");
                            validation.ShowErrorBox = true;//显示上面提示=Ture
                            sheet.AddValidationData(validation);//添加进去
                        }
                    }

                }
            });

            sheet.ForceFormulaRecalculation = true;

            var memory = new NpoiMemoryStream {AllowClose = false};

            workbook.Write(memory);
            memory.Flush();
            memory.Position = 0;    // 指定内存流起始值
            return memory;

        }

        public static NpoiMemoryStream GetFileTemplate(string fileName
                                                     , List<HeadInfo> heads
                                                     , int templteRowCount
                                                     , params CellValidation[] cellValidations)
        {
            return GetFileTemplate(fileName, heads, null, null, templteRowCount, cellValidations);
        }

        public static NpoiMemoryStream GetFileTemplate(string fileName
                                                     , List<HeadInfo> heads
                                                     , List<CellContent> cellContents
                                                     , int templteRowCount
                                                     ,params CellValidation[] cellValidations)
        {
            return GetFileTemplate(fileName, heads, null, cellContents, templteRowCount, cellValidations);
        }

        public static NpoiMemoryStream GetFileDownloadData(string fileName
                                                         , List<HeadInfo> heads
                                                         , List<List<ContentInfo>> contentInfo)
        {
            return GetFileTemplate(fileName, heads, contentInfo, null, 65535, null);
        }

        public static NpoiMemoryStream GetFileTemplate(string fileName
                                                     , List<HeadInfo> heads
                                                     , List<CellContent> cellContents)
        {
            return GetFileTemplate(fileName, heads, null, cellContents, 65535, null);
        }

        /// <summary>
        /// 下载excel模板
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <param name="heads">表头</param>
        /// <param name="contentInfo">导出excel数据源</param>
        /// <param name="cellContents"></param>
        /// <param name="templteRowCount">模板或者导出excel样式设置行数</param>
        /// <param name="cellValidations">验证数据源</param>
        /// <returns></returns>
        public static NpoiMemoryStream GetFileTemplate(string fileName
                                                     , List<HeadInfo> heads
                                                     , List<List<ContentInfo>> contentInfo = null
                                                     , List<CellContent> cellContents = null
                                                     , int templteRowCount = 65535
                                                     , params CellValidation[] cellValidations)
        {

            #region 内容特殊字符过滤  待处理
            for (var i = 0; i < cellValidations?.Length; i++)
            {
                if (cellValidations[i].FirstList == null || !cellValidations[i].FirstList.Any()) continue;

                cellValidations[i].FirstList[0].Key = ReplaceSpecialCharacter(cellValidations[i].FirstList[0].Key);
                cellValidations[i].FirstList[0].Value = ReplaceSpecialCharacter(cellValidations[i].FirstList[0].Value);
                //foreach (var item in cellValidations[i].FirstList)
                //{
                //    item.Key =ReplaceSpecialCharacter(item.Key);
                //    item.Value =ReplaceSpecialCharacter(item.Value);
                //}
            }
            #endregion

            var workbook = new XSSFWorkbook();

            var sheet = workbook.CreateSheet(fileName);

            #region set heads

            var rowCount = contentInfo?.Count ?? templteRowCount;
            var newRow = sheet.CreateRow(0);
            for (var i = 0; i < heads.Count; i++)
            {
                sheet.SetColumnWidth(i, heads[i].ColumnWidth);
                var style = GetCellStyle(workbook, heads[i].CellStyle);
                var cell = newRow.CreateCell(i);
                cell.CellStyle = style;
                cell.SetCellValue(heads[i].Name);
                for (var j = 1; j <= rowCount; j++)
                {
                    var row = sheet.GetRow(j) ?? sheet.CreateRow(j);
                    cell = row.CreateCell(i);
                    cell.CellStyle = style;
                }
            }
            #endregion

            #region 初始化导出excel数据源
            for (var i = 0; i < contentInfo?.Count; i++)
            {
                var contentRow = sheet.CreateRow(i + 1);
                for (var j = 0; j < contentInfo[i]?.Count; j++)
                {
                    var contentCell = contentRow.CreateCell(j);
                    contentCell.SetCellValue(contentInfo[i][j].Name);
                }
            }
            #endregion

            #region set datasource
            cellValidations?.ToList().ForEach(item =>
            {
                if (item.FirstList == null) return;

                var dataSourceSheetName = $"{item.FirstList?[0]?.Value.Replace(" ", "")}";
                var dataSourceSheet = workbook.CreateSheet(dataSourceSheetName);//创建sheet

                /*
                 * FirstList
                 * 1 2 3    
                 * 2
                 * 3
                 */


                #region init datasource
                for (var i = 0; i < item.FirstList.Count; i++)
                {
                    var row = dataSourceSheet.GetRow(i) ?? dataSourceSheet.CreateRow(i);//添加行

                    row.CreateCell(0).SetCellValue(i == 0
                        ? item.FirstList[i].Value.Replace(" ", "")
                        : item.FirstList[i].Value);//单元格写值

                    if (i <= 0) continue;


                    if (item.SecondList != null && item.SecondList.Any())
                    {
                        dataSourceSheet.GetRow(0).CreateCell(i).SetCellValue(item.FirstList[i].Value);//一级列头

                        var cellDatas = item.SecondList.Where(c => c.Key == item.FirstList[i].Key).ToList();

                        for (var j = 0; j < cellDatas.Count; j++)
                        {
                            var secondRow = dataSourceSheet.GetRow(j + 1) ?? dataSourceSheet.CreateRow(j + 1);
                            secondRow.CreateCell(i).SetCellValue(cellDatas[j].Value);//单元格写值        
                        }
                    }
                }
                #endregion

                #region Range
                for (var i = 0; i < item.FirstList.Count; i++)
                {
                    if (i == 0)
                    {
                        var range = workbook.CreateName();
                        range.NameName = item.FirstList[i].Value.Replace(" ", "");
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
                                range.NameName = item.FirstList[i].Value.Replace(" ", "");
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

            #region set cellContents
            cellContents?.ForEach(item =>
            {
                var writeSheet = workbook.GetSheetAt(item.SheetIndex) ??
                                 workbook.CreateSheet(item.SheetIndex.ToString());
                var i = 0;
                item.ListOfValues?.ForEach(info =>
                {
                    var writeRow = writeSheet.GetRow(item.FirstRow + i) ?? writeSheet.CreateRow(item.FirstRow + i);
                    var writeCell = writeRow.GetCell(item.FirstCol) ?? writeRow.CreateCell(item.FirstCol);
                    writeCell.SetCellValue(info);
                    i++;
                });
            });
            #endregion

            #region 设置验证
            cellValidations?.ToList().ForEach(item =>
                {
                    if (item.FirstList != null && item.FirstList.Any())
                    {
                        if (item.FirstCellLocation != null)
                        {
                            var regions = new CellRangeAddressList(item.FirstCellLocation.FirstRow, item.FirstCellLocation.LastRow, item.FirstCellLocation.FirstCol, item.FirstCellLocation.LastCol);//约束范围：c2到c65535

                            var helper = new XSSFDataValidationHelper((XSSFSheet)sheet);//获得一个数据验证Helper
                            var validation =
                                    helper.CreateValidation(
                                        helper.CreateFormulaListConstraint(item.FirstList[0].Value),
                                        regions);//创建约束
                            validation.CreateErrorBox("错误", "请按右侧下拉箭头选择!");
                            validation.ShowErrorBox = true;//显示上面提示
                            sheet.AddValidationData(validation);//添加进去
                        }
                    }
                    else
                    {
                        if (item.ListOfValues != null && item.ListOfValues.Any())
                        {
                            if (item.FirstCellLocation != null)
                            {
                                var regions = new CellRangeAddressList(item.FirstCellLocation.FirstRow, item.FirstCellLocation.LastRow, item.FirstCellLocation.FirstCol, item.FirstCellLocation.LastCol);//约束范围：c2到c65535
                                var helper = new XSSFDataValidationHelper((XSSFSheet)sheet);//获得一个数据验证Helper
                                var validation =
                                        helper.CreateValidation(helper.CreateExplicitListConstraint(item.ListOfValues),
                                            regions);//创建约束
                                validation.CreateErrorBox("错误", "请按右侧下拉箭头选择!");
                                validation.ShowErrorBox = true;//显示上面提示
                                sheet.AddValidationData(validation);//添加进去
                            }

                        }
                    }
                });
            #endregion

            #region 设置验证
            cellValidations?.ToList().ForEach(item =>
                {
                    if (item.SecondList != null && item.SecondList.Any())
                    {
                        if (item.SecondCellLocation != null)
                        {
                            var regions = new CellRangeAddressList(item.SecondCellLocation.FirstRow, item.SecondCellLocation.LastRow, item.SecondCellLocation.FirstCol, item.SecondCellLocation.LastCol);//约束范围：c2到c65535

                            var helper = new XSSFDataValidationHelper((XSSFSheet)sheet);//获得一个数据验证Helper
                            var validation =
                                    helper.CreateValidation(
                                        helper.CreateFormulaListConstraint($"INDIRECT(${Index2ColName(item.FirstCellLocation.FirstCol)}2)"),
                                        regions);//创建约束
                            validation.CreateErrorBox("错误", "请按右侧下拉箭头选择!");
                            validation.ShowErrorBox = true;//显示上面提示=Ture
                            sheet.AddValidationData(validation);//添加进去
                        }
                        else if (item.FirstCellLocation != null)
                        {
                            var regions = new CellRangeAddressList(item.FirstCellLocation.FirstRow, item.FirstCellLocation.LastRow, item.FirstCellLocation.FirstCol + 1, item.FirstCellLocation.LastCol + 1);//约束范围：c2到c65535

                            var helper = new XSSFDataValidationHelper((XSSFSheet)sheet);//获得一个数据验证Helper
                            var validation =
                                    helper.CreateValidation(
                                        helper.CreateFormulaListConstraint($"INDIRECT(${Index2ColName(item.FirstCellLocation.FirstCol)}2)"),
                                        regions);//创建约束
                            validation.CreateErrorBox("错误", "请按右侧下拉箭头选择!");
                            validation.ShowErrorBox = true;//显示上面提示=Ture
                            sheet.AddValidationData(validation);//添加进去
                        }

                    }
                });
            #endregion

            sheet.ForceFormulaRecalculation = true;

            var memory = new NpoiMemoryStream { AllowClose = false };

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
                if (colName.Length > 0)
                {
                    index--;
                }
                var remainder = index % 26;
                colName = ((char)(remainder + num)) + colName;
                index = (int)((index - remainder) / 26);
            } while (index > 0);
            return colName;
        }

        private static string ReplaceSpecialCharacter(string text)
        {
            if (string.IsNullOrEmpty(text)) return "";
            //将特殊字符全部替换为下划线
            string pattern = "[\\[ \\] \\^ \\-_*×――(^)$%~!@#$…&%￥—+=<>《》!！??？:：•`·、。，；,;\"‘’“”-]";
            return Regex.Replace(text, pattern, "");

        }

        /// <summary>
        /// 获取年月日
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime? GetDate(object date)
        {
            if (date == null)
                return null;
            DateTime exTime = DateTime.Now;
            string dateStr = date.ToString();
            DateTime colDate = DateTime.MinValue;
            if (DateTime.TryParse(date.ToString(), out exTime))
            {
                colDate = exTime;
            }
            else
            {
                int num;
                dateStr = dateStr.Replace("/", string.Empty).Replace("－", string.Empty).Replace(".", string.Empty).Replace("_", string.Empty)
                       .Replace("-", string.Empty).Replace("年", string.Empty).Replace("月", string.Empty).Replace("日", string.Empty);
                if (dateStr.Length < 8)
                {
                    DateTime curTime = DateTime.Now;
                    if (DateTime.TryParse(date.ToString(), out curTime))
                    {
                        colDate = curTime;
                    }
                    else
                    {
                        double d_date = 0;
                        if (double.TryParse(dateStr, out d_date))
                        {
                            colDate = DateTime.FromOADate(d_date);
                        }
                    }
                }
                else
                {
                    if (int.TryParse(dateStr, out num))
                    {
                        if (dateStr.Length == 8)
                        {
                            dateStr = dateStr.Substring(0, 4) + "-" + dateStr.Substring(4, 2) + "-" + dateStr.Substring(6, 2);
                            //colDate = DateTime.Parse(dateStr);
                            DateTime.TryParse(dateStr, out colDate);
                        }
                    }
                }
            }
            return colDate;
        }

        #region 定义单元格常用到样式
        public static ICellStyle GetCellStyle(XSSFWorkbook wb, CellStyleEnum str)
        {
            var cellStyle = wb.CreateCellStyle();

            switch (str)
            {
                case CellStyleEnum.头:
                    // cellStyle.FillPattern = FillPatternType.LEAST_DOTS;  
                    //cellStyle.SetFont(font12);
                    break;
                case CellStyleEnum.月份:
                    IDataFormat monthStyle = wb.CreateDataFormat();
                    cellStyle.DataFormat = monthStyle.GetFormat("yyyy-MM");
                    break;
                case CellStyleEnum.日期:
                    IDataFormat datastyle1 = wb.CreateDataFormat();
                    cellStyle.DataFormat = datastyle1.GetFormat("yyyy-MM-dd");
                    break;
                case CellStyleEnum.时间:
                    IDataFormat datastyle = wb.CreateDataFormat();
                    cellStyle.DataFormat = datastyle.GetFormat("yyyy-mm-dd");
                    break;
                case CellStyleEnum.数字:
                    cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00");
                    break;
                case CellStyleEnum.钱:
                    IDataFormat format = wb.CreateDataFormat();
                    cellStyle.DataFormat = format.GetFormat("￥#,##0");
                    break;
                case CellStyleEnum.url:
                    //fontcolorblue.Underline = 1;
                    break;
                case CellStyleEnum.百分比:
                    cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00%");
                    break;
                case CellStyleEnum.中文大写:
                    IDataFormat format1 = wb.CreateDataFormat();
                    cellStyle.DataFormat = format1.GetFormat("[DbNum2][$-804]0");
                    break;
                case CellStyleEnum.科学计数法:
                    cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00E+00");
                    break;
                case CellStyleEnum.文本:
                    cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("@");
                    break;
                case CellStyleEnum.默认:
                    break;
            }


            return cellStyle;


        }
        #endregion  
    }

    #region 定义单元格常用到样式的枚举
    public enum CellStyleEnum
    {
        头,
        url,
        时间,
        日期,
        月份,
        数字,
        钱,
        百分比,
        中文大写,
        科学计数法,
        文本,
        默认
    }
    #endregion

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
        public CellLocation FirstCellLocation { get; set; }
        public CellLocation SecondCellLocation { get; set; }

        public string[] ListOfValues { get; set; }
        public List<CellData> FirstList { get; set; }
        public List<CellData> SecondList { get; set; }

        public CellValidation()
        {

        }

        public CellValidation(int firstCol, int lastCol, string[] listOfValues)
            : this(new CellLocation(firstCol, lastCol), listOfValues, null, null, null)
        {
        }

        public CellValidation(int firstRow, int lastRow, int firstCol, int lastCol, string[] listOfValues)
        : this(new CellLocation(firstRow, lastRow, firstCol, lastCol), listOfValues, null, null, null)
        {
        }

        public CellValidation(int firstCol, int lastCol,
            List<CellData> firstList, List<CellData> secondList)
        : this(new CellLocation(firstCol, lastCol), null, firstList, null, secondList)
        {

        }

        public CellValidation(int firstRow, int lastRow, int firstCol, int lastCol,
            List<CellData> firstList, List<CellData> secondList)
            : this(new CellLocation(firstRow, lastRow, firstCol, lastCol), null, firstList, null, secondList)
        {

        }

        public CellValidation(int firstCol, int lastCol,
            List<CellData> firstList, int secondFirstCol, int secondLastCol, List<CellData> secondList)
            : this(new CellLocation(firstCol, lastCol), null, firstList, new CellLocation(secondFirstCol, secondLastCol), secondList)
        {

        }



        public CellValidation(int firstRow
                                        , int lastRow
                                        , int firstCol
                                        , int lastCol
                                        , List<CellData> firstList
                                        , int secondFirstRow
                                        , int secondLastRow
                                        , int secondFirstCol
                                        , int secondLastCol
                                        , List<CellData> secondList)
            : this(new CellLocation(firstRow, lastRow, firstCol, lastCol), null, firstList, new CellLocation(secondFirstRow, secondLastRow, secondFirstCol, secondLastCol), secondList)
        {

        }

        public CellValidation(CellLocation firstCellLocation,
            List<CellData> firstList, CellLocation secondCellLocation, List<CellData> secondList)
            : this(firstCellLocation, null, firstList, secondCellLocation, secondList)
        {

        }
        public CellValidation(CellLocation firstCellLocation, string[] listOfValues,
            List<CellData> firstList, CellLocation secondCellLocation, List<CellData> secondList)
        {
            FirstCellLocation = firstCellLocation;
            ListOfValues = listOfValues;
            FirstList = firstList;
            SecondCellLocation = secondCellLocation;
            SecondList = secondList;

        }

    }

    public class CellLocation
    {
        public int FirstRow { get; set; }
        public int LastRow { get; set; }
        public int FirstCol { get; set; }
        public int LastCol { get; set; }

        public CellLocation()
        {

        }

        public CellLocation(int firstCol, int lastCol)
        : this(1, 65535, firstCol, lastCol)
        {

        }

        public CellLocation(int firstRow, int lastRow, int firstCol, int lastCol)
        {
            FirstRow = firstRow;
            LastRow = lastRow;
            FirstCol = firstCol;
            LastCol = lastCol;
        }
    }

    public class CellContent
    {
        public int SheetIndex { get; set; }
        public int FirstRow { get; set; }
        public int LastRow { get; set; }
        public int FirstCol { get; set; }
        public int LastCol { get; set; }
        public string[] ListOfValues { get; set; }

        public CellContent(int firstRow, int firstCol, string[] listOfValues)
: this(firstRow, 0, firstCol, 0, listOfValues, 0)
        {

        }

        public CellContent(int firstRow, int firstCol, string[] listOfValues, int sheetIndex)
: this(firstRow, 0, firstCol, 0, listOfValues, sheetIndex)
        {

        }

        public CellContent(int firstRow, int lastRow, int firstCol, int lastCol, string[] listOfValues)
        : this(firstRow, lastRow, firstCol, lastCol, listOfValues, 0)
        {

        }

        public CellContent(int firstRow, int lastRow, int firstCol, int lastCol, string[] listOfValues, int sheetIndex)
        {
            FirstRow = firstRow;
            LastRow = lastRow;
            FirstCol = firstCol;
            LastCol = lastCol;
            ListOfValues = listOfValues;
            SheetIndex = sheetIndex;
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

    public class HeadInfo
    {
        public string Name { get; set; }
        public CellStyleEnum CellStyle { get; set; }
        public int ColumnWidth { get; set; }

        public HeadInfo() { }

        public HeadInfo(string name, CellStyleEnum cellStyle, int columnWidth = 30 * 256)
        {
            Name = name;
            CellStyle = cellStyle;
            ColumnWidth = columnWidth;
        }
    }

    public class ContentInfo : HeadInfo
    {
        public ContentInfo() { }

        public ContentInfo(string name) : base(name, CellStyleEnum.默认)
        {
            this.Name = name;
        }
    }
}