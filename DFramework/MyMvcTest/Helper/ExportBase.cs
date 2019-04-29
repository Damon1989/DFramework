using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;
using DFramework.Infrastructure;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace MyMvcTest.Helper
{
    public class ExportBase
    {
        private static readonly int DefaultWidth = 100;

        /// <summary>
        ///     文件保存地址
        /// </summary>
        private static string SaveExportUrl => "~"; //ConfigurationManager.AppSettings["SaveExportUrl"];

        /// <summary>
        ///     导出Pdf文件
        /// </summary>
        private static string ExportPdfFile => "是"; // ConfigurationManager.AppSettings["ExportPdfFile"];

        /// <summary>
        ///     导出压缩加密文件
        /// </summary>
        private static string ExportZipFile => "否"; // ConfigurationManager.AppSettings["ExportZipFile"];

        #region 拷贝模块，返回生成文件地址

        /// <summary>
        ///     拷贝模块，返回生成文件地址
        /// </summary>
        /// <returns></returns>
        private static string GetExportFilePath(string fileName, string template = "Export.xls", string creatorId = "")
        {
            try
            {
                #region copy template

                fileName = Regex.Replace(fileName, "[\\\\/:*?\"<>|]", "");

                creatorId = string.IsNullOrEmpty(creatorId) ? Guid.NewGuid().ToString("N") : creatorId;
                var filePath = MapPath($"~/Export/{template}");
                var extension = Path.GetExtension(filePath);
                var userPath =
                    MapPath($"{SaveExportUrl}/Export/{creatorId}/{fileName}{extension}");

                var existDir = MapPath($"{SaveExportUrl}/Export/{creatorId}");
                if (Directory.Exists(existDir))
                {
                    var strFiles = Directory.GetFiles(existDir);
                    foreach (var file in strFiles) File.Delete(file);
                }
                else
                {
                    Directory.CreateDirectory(existDir);
                }

                File.Copy(filePath, userPath, true);

                #endregion copy template

                return userPath;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        #endregion 拷贝模块，返回生成文件地址


        public static string GetEnumDescription(object value)
        {
            var fi = value.GetType().GetField(value.ToString());
            var attributes = (DescriptionAttribute[]) fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }

        #region 导出信息

        public string ExportList<T>(T model,
            string fileName,
            string userId = "",
            string account = "",
            bool exportPdf = false)
        {
            try
            {
                #region write dictionary

                var dicValue = new Dictionary<string, string>();
                var dicValueProperty = new Dictionary<string, string>();
                var properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
                properties.ForEach(item =>
                {
                    var attribute = (DescriptionAttribute) item.GetCustomAttribute(typeof(DescriptionAttribute), false);
                    if (attribute != null)
                    {
                        var des = attribute.Description.Split('!');
                        if (des.Length > 2)
                            if (!dicValue.ContainsKey(des[0]) && !dicValueProperty.ContainsKey(des[0]))
                            {
                                dicValue.Add(des[0], des[1]);
                                dicValueProperty.Add(des[0], des[2]);
                            }
                    }
                });

                #endregion write dictionary

                #region userPath

                var userPath = GetExportFilePath(fileName);
                if (userPath.Equals(string.Empty)) return string.Empty;

                #endregion

                #region create hssfWorkbook

                HSSFWorkbook hssfWorkbook;

                using (var file = new FileStream(userPath, FileMode.Open, FileAccess.Read))
                {
                    hssfWorkbook = new HSSFWorkbook(file);
                }

                #endregion create hssfWorkbook

                #region cellstyle

                var cellstyle = hssfWorkbook.CreateCellStyle();
                var format = hssfWorkbook.CreateDataFormat();
                cellstyle.DataFormat = format.GetFormat("@");
                cellstyle.BorderBottom = BorderStyle.Thin;
                cellstyle.BorderTop = BorderStyle.Thin;
                cellstyle.BorderLeft = BorderStyle.Thin;
                cellstyle.BorderRight = BorderStyle.Thin;
                cellstyle.VerticalAlignment = VerticalAlignment.Center;
                cellstyle.FillBackgroundColor = HSSFColor.Red.Index;
                cellstyle.WrapText = true;

                var font = hssfWorkbook.CreateFont();
                font.Boldweight = (short) FontBoldWeight.Normal;
                font.FontHeightInPoints = 11;
                cellstyle.SetFont(font);


                var contentstyle = hssfWorkbook.CreateCellStyle();
                contentstyle.DataFormat = format.GetFormat("@");
                contentstyle.BorderBottom = BorderStyle.Thin;
                contentstyle.BorderTop = BorderStyle.Thin;
                contentstyle.BorderLeft = BorderStyle.Thin;
                contentstyle.BorderRight = BorderStyle.Thin;
                contentstyle.VerticalAlignment = VerticalAlignment.Center;
                contentstyle.FillBackgroundColor = HSSFColor.Red.Index;
                contentstyle.WrapText = true;

                var contentFont = hssfWorkbook.CreateFont();
                contentFont.Boldweight = (short) FontBoldWeight.Normal;
                contentFont.FontHeightInPoints = 9;
                contentstyle.SetFont(contentFont);


                var titlecellstyle = hssfWorkbook.CreateCellStyle();
                titlecellstyle.BorderBottom = BorderStyle.Thin;
                titlecellstyle.BorderTop = BorderStyle.Thin;
                titlecellstyle.BorderLeft = BorderStyle.Thin;
                titlecellstyle.BorderRight = BorderStyle.Thin;
                titlecellstyle.VerticalAlignment = VerticalAlignment.Center;
                titlecellstyle.FillBackgroundColor = HSSFColor.Red.Index;
                titlecellstyle.WrapText = true;

                var titlefont = hssfWorkbook.CreateFont();
                titlefont.Boldweight = (short) FontBoldWeight.Bold;
                titlefont.FontHeightInPoints = 11;
                titlecellstyle.SetFont(titlefont);


                var maintitlecellstyle = hssfWorkbook.CreateCellStyle();
                maintitlecellstyle.BorderBottom = BorderStyle.None;
                maintitlecellstyle.BorderTop = BorderStyle.None;
                maintitlecellstyle.BorderLeft = BorderStyle.None;
                maintitlecellstyle.BorderRight = BorderStyle.None;
                maintitlecellstyle.VerticalAlignment = VerticalAlignment.Center;
                maintitlecellstyle.Alignment = HorizontalAlignment.Center;
                maintitlecellstyle.FillBackgroundColor = HSSFColor.Red.Index;
                maintitlecellstyle.WrapText = true;

                var maintitlefont = hssfWorkbook.CreateFont();
                maintitlefont.Boldweight = (short) FontBoldWeight.Bold;
                maintitlefont.FontHeightInPoints = 20;
                maintitlecellstyle.SetFont(maintitlefont);


                var subtitlecellstyle = hssfWorkbook.CreateCellStyle();
                subtitlecellstyle.BorderBottom = BorderStyle.None;
                subtitlecellstyle.BorderTop = BorderStyle.None;
                subtitlecellstyle.BorderLeft = BorderStyle.None;
                subtitlecellstyle.BorderRight = BorderStyle.None;
                subtitlecellstyle.VerticalAlignment = VerticalAlignment.Center;
                subtitlecellstyle.FillBackgroundColor = HSSFColor.Red.Index;
                subtitlecellstyle.WrapText = true;

                var subtitleFont = hssfWorkbook.CreateFont();
                subtitleFont.Boldweight = (short) FontBoldWeight.Normal;
                subtitleFont.FontHeightInPoints = 12;
                subtitlecellstyle.SetFont(subtitleFont);

                #endregion cellstyle


                ISheet sheet;

                sheet = hssfWorkbook.GetSheetAt(0);
                hssfWorkbook.SetSheetName(0, "sheetName");


                var childRowCount = 0;
                for (var rownum = 0; rownum < 30; rownum++)
                {
                    var keyarray = dicValue.Keys.Where(c => c.Split(',')[0] == rownum.ToString()).ToArray();

                    if (!keyarray.Any()) continue;

                    var row = sheet.CreateRow(rownum + childRowCount);

                    if (keyarray.Count() == 1) //该行只包含一个元素
                    {
                        var key = keyarray[0];
                        var colrow = key.Split(',')[0].GetInt() + childRowCount;
                        var valueproperty = dicValueProperty[key];
                        var valuearray = dicValue[key].Split(',');
                        var valuePropertyArray = valueproperty.Split(',');
                        var valueHeight = valuePropertyArray[0].GetInt();
                        var valueWidth = valuePropertyArray[1].GetInt();

                        if (valuearray.Length == 2)
                            if (valuearray[1] == "Title")
                            {
                                //设置标题
                                MergedCell(sheet, colrow, colrow, 0, valuearray[0].GetInt() - 1, maintitlecellstyle);
                                var maincell = row.CreateCell(0);
                                var mainchildvalue = model.GetType().GetProperty(valuearray[1]).GetValue(model, null);
                                if (mainchildvalue != null) maincell.SetCellValue(mainchildvalue.ToString());
                                maincell.CellStyle = maintitlecellstyle;
                                sheet.GetRow(rownum).Height = (short) (100 * valueHeight);
                                //sheet.SetColumnWidth(1, 100 * valueWidth);
                                continue;
                            }

                        if (valuearray.Length == 2)
                        {
                            MergedCell(sheet, colrow, colrow, 0, valuearray[0].GetInt() - 1, titlecellstyle);
                            var maincell = row.CreateCell(0);
                            var mainchildvalue = model.GetType().GetProperty(valuearray[1]).GetValue(model, null);
                            if (mainchildvalue != null) maincell.SetCellValue(mainchildvalue.ToString());
                            maincell.CellStyle = cellstyle;
                            sheet.GetRow(row.RowNum).Height = (short) (100 * valueHeight);
                            sheet.SetColumnWidth(0, int.Parse(valuearray[0]) * DefaultWidth * valueWidth);
                            continue;
                        }

                        if (valuearray.Length == 3) //该行只包含一个元素
                        {
                            MergedCell(sheet, colrow, colrow, 1, valuearray[0].GetInt() - 1, titlecellstyle);

                            var cell = row.CreateCell(0);
                            cell.SetCellValue(valuearray[2]);
                            cell.CellStyle = cellstyle;


                            var cellValue = row.CreateCell(1);
                            cellValue.SetCellValue(model.GetType().GetProperty(valuearray[1]).GetValue(model, null)
                                .ToString());
                            cellValue.CellStyle = contentstyle;

                            sheet.GetRow(row.RowNum).Height = (short) (100 * valueHeight);
                        }

                        if (valuearray.Length == 4)
                        {
                            #region DisplayName

                            MergedCell(sheet, colrow, colrow, 0, valuearray[0].GetInt() - 1, titlecellstyle);
                            var cell = row.CreateCell(0);
                            cell.SetCellValue(valuearray[2]);
                            cell.CellStyle = cellstyle;
                            sheet.GetRow(colrow).Height = (short) (100 * valueHeight);

                            #endregion DisplayName

                            #region DisplayValue

                            if (!model.GetType().GetProperty(valuearray[1]).PropertyType.Name.Equals("String"))
                            {
                                var list =
                                    model.GetType().GetProperty(valuearray[1]).GetValue(model, null) as
                                        IEnumerable<object>;
                                if (list != null)
                                {
                                    PropertyInfo[] childProperties = null;
                                    list.ForEach(item =>
                                    {
                                        childProperties = item.GetType()
                                            .GetProperties(BindingFlags.Instance | BindingFlags.Public);
                                    });

                                    var dicListValue = new Dictionary<string, string>();
                                    var dicListValueProperty = new Dictionary<string, string>();

                                    childProperties?.ToArray().ForEach(item =>
                                    {
                                        var attribute =
                                            (DescriptionAttribute) item.GetCustomAttribute(typeof(DescriptionAttribute),
                                                false);
                                        if (attribute != null)
                                        {
                                            var description = attribute.Description;
                                            var des = description.Split('!');
                                            if (des.Length == 3)
                                                if (!dicListValue.ContainsKey(des[0]))
                                                {
                                                    dicListValue.Add(des[0], des[1]);
                                                    dicListValueProperty.Add(des[0], des[2]);
                                                }
                                        }
                                    });

                                    childRowCount++;
                                    var titleRow = sheet.CreateRow(rownum + childRowCount);
                                    dicListValue.ForEach(item =>
                                    {
                                        var mergedColumn = int.Parse(item.Key.Split(',')[1]);
                                        var cellcol = int.Parse(item.Key.Split(',')[0]);
                                        var newcell = titleRow.CreateCell(cellcol);
                                        newcell.SetCellValue(item.Value.Split(',')[1]);
                                        newcell.CellStyle = cellstyle;

                                        if (mergedColumn > 1)
                                            MergedCell(sheet, titleRow.RowNum, titleRow.RowNum, cellcol,
                                                cellcol + mergedColumn - 1, cellstyle);

                                        dicListValueProperty.ForEach(p =>
                                        {
                                            if (p.Key == item.Key)
                                            {
                                                sheet.GetRow(titleRow.RowNum).Height =
                                                    (short) (100 * int.Parse(p.Value.Split(',')[0]));
                                                sheet.SetColumnWidth(cellcol,
                                                    DefaultWidth * int.Parse(p.Value.Split(',')[1]));
                                            }
                                        });
                                    });


                                    list.ForEach(item =>
                                    {
                                        childRowCount++;
                                        var newrow = sheet.GetRow(rownum + childRowCount) ??
                                                     sheet.CreateRow(rownum + childRowCount);
                                        dicListValue.ForEach(info =>
                                        {
                                            var cellcol = int.Parse(info.Key.Split(',')[0]);
                                            var newcell = newrow.CreateCell(cellcol);
                                            newcell.SetCellValue(item.GetType().GetProperty(info.Value.Split(',')[0])
                                                .GetValue(item, null).ToString());
                                            newcell.CellStyle = contentstyle;
                                            var mergedColumn = int.Parse(info.Key.Split(',')[1]);
                                            if (mergedColumn > 1)
                                                MergedCell(sheet, newrow.RowNum, newrow.RowNum, cellcol,
                                                    cellcol + mergedColumn - 1, contentstyle);
                                            dicListValueProperty.ForEach(p =>
                                            {
                                                if (p.Key == info.Key)
                                                    sheet.GetRow(newrow.RowNum).Height =
                                                        (short) (100 * int.Parse(p.Value.Split(',')[0]));
                                            });
                                        });
                                    });
                                }
                            }
                            else
                            {
                                cell = row.CreateCell(1);
                                var childvalue = model.GetType().GetProperty(valuearray[1]).GetValue(model, null);
                                if (childvalue != null)
                                {
                                    cell.SetCellValue(childvalue.ToString());
                                    cell.CellStyle = contentstyle;
                                }
                            }

                            #endregion DisplayValue
                        }
                    }
                    else
                    {
                        for (var colnum = 0; colnum < keyarray.Count(); colnum++)
                        {
                            var key = keyarray[colnum];

                            var colrow = key.Split(',')[0].GetInt() + childRowCount; //行号
                            var col = key.Split(',')[1].GetInt(); //列号

                            var valueproperty = dicValueProperty[key]; //valuepproperty
                            var valuearray = dicValue[key].Split(',');
                            var valuePropertyArray = valueproperty.Split(',');
                            var valueHeight = valuePropertyArray[0].GetInt();
                            var valueWidth = valuePropertyArray[1].GetInt();

                            if (valuearray[1] == "SubTitle")
                            {
                                //设置副标题
                                MergedCell(sheet, colrow, colrow, 0, valuearray[0].GetInt() - 1, subtitlecellstyle);
                                var maincell = row.CreateCell(0);
                                var mainchildvalue = model.GetType().GetProperty(valuearray[1]).GetValue(model, null);
                                if (mainchildvalue != null) maincell.SetCellValue(mainchildvalue.ToString());
                                maincell.CellStyle = subtitlecellstyle;
                                sheet.GetRow(rownum).Height = (short) (100 * valueHeight);
                                continue;
                            }

                            if (valuearray[1] == "Number")
                            {
                                var cellNum = int.Parse(keyarray[1].Split(',')[1]);
                                MergedCell(sheet, colrow, colrow, cellNum, cellNum + valuearray[0].GetInt() - 1,
                                    subtitlecellstyle);
                                var maincell = row.CreateCell(cellNum);
                                var mainchildvalue = model.GetType().GetProperty(valuearray[1]).GetValue(model, null);
                                if (mainchildvalue != null) maincell.SetCellValue(mainchildvalue.ToString());
                                maincell.CellStyle = subtitlecellstyle;
                                sheet.GetRow(rownum).Height = (short) (100 * valueHeight);
                                continue;
                            }


                            #region DisplayName

                            var cell = row.CreateCell(col);
                            cell.SetCellValue(valuearray[2]);
                            cell.CellStyle = cellstyle;
                            sheet.GetRow(rownum).Height = (short) (100 * valueHeight);
                            sheet.SetColumnWidth(col, DefaultWidth * valueWidth);

                            #endregion DisplayName

                            #region DisplayValue

                            cell = row.CreateCell(col + 1);

                            var value = model.GetType()?.GetProperty(valuearray[1])?.GetValue(model, null);
                            if (value != null) cell.SetCellValue(value.ToString());

                            cell.CellStyle = contentstyle;

                            #endregion DisplayValue

                            #region marge

                            if (valuearray[0].GetInt() > 2)
                                MergedCell(sheet, colrow, colrow, col + 1, col + valuearray[0].GetInt() - 1,
                                    contentstyle);

                            var margedCol = valuearray[0].GetInt() - 1;
                            for (var i = 0; i < margedCol; i++)
                                sheet.SetColumnWidth(col + 1 + i, DefaultWidth * valueWidth);

                            #endregion
                        }
                    }
                }

                using (var fs = File.OpenWrite(userPath))
                {
                    hssfWorkbook.Write(fs);
                }

                if (exportPdf && ExportPdfFile.Equals("是")) userPath = CreateOfficePdfFrom(userPath);
                if (ExportZipFile.Equals("是") && !exportPdf) userPath = CreateExcelZipFile(userPath, "xls", account);

                return userPath;
            }
            catch (Exception exception)
            {
                return exception.Message;
            }
        }

        #endregion 导出信息

        public string CreateExcelZipFile(string filePath, string fileExtension = "xls", string password = "")
        {
            var fileInfo = filePath.Split('\\');
            var fileName = fileInfo[fileInfo.Length - 1];
            var zipFilePath = filePath.Replace(fileExtension, "zip");
            //生成的压缩文件为***.zip
            using (var fsOut = File.Create(zipFilePath))
            {
                using (var zipStream = new ZipOutputStream(fsOut))
                {
                    zipStream.Password = password;
                    var fi = new FileInfo(filePath);
                    var entryName = fileName;
                    var newEntry = new ZipEntry(entryName)
                    {
                        DateTime = fi.LastWriteTime,
                        Size = fi.Length
                    };
                    zipStream.PutNextEntry(newEntry);

                    var buffer = new byte[4096];
                    using (var streamReader = File.OpenRead(filePath))
                    {
                        StreamUtils.Copy(streamReader, zipStream, buffer);
                    }

                    zipStream.CloseEntry();
                    zipStream.IsStreamOwner = false;
                    zipStream.Finish();
                    zipStream.Close();
                }
            }

            File.Delete(filePath);
            return zipFilePath;
        }

        public string CreateOfficePdfFrom(string filePath)
        {
            return ExcelConverter.ConvertTopdf(filePath);
        }

        #region 合并单元格

        /// <summary>
        ///     合并单元格
        /// </summary>
        /// <param name="sheet">要合并单元格所在的sheet</param>
        /// <param name="rowstart">开始行的索引</param>
        /// <param name="rowend">结束行的索引</param>
        /// <param name="colstart">开始列的索引</param>
        /// <param name="colend">结束列的索引</param>
        /// <param name="cellStyle">单元格样式</param>
        public static void MergedCell(ISheet sheet, int rowstart, int rowend, int colstart, int colend,
            ICellStyle cellStyle = null)
        {
            var cellRangeAddress = new CellRangeAddress(rowstart, rowend, colstart, colend);
            sheet.AddMergedRegion(cellRangeAddress);
            if (cellStyle == null) return;
            for (var i = cellRangeAddress.FirstRow; i <= cellRangeAddress.LastRow; i++)
            {
                var row = HSSFCellUtil.GetRow(i, (HSSFSheet) sheet);
                for (var j = cellRangeAddress.FirstColumn; j <= cellRangeAddress.LastColumn; j++)
                {
                    var cell = HSSFCellUtil.GetCell(row, (short) j);
                    if (cell != null) cell.CellStyle = cellStyle;
                }
            }
        }

        #endregion

        public static string MapPath(string strPath)
        {
            if (HttpContext.Current != null) return HostingEnvironment.MapPath(strPath);

            strPath = strPath.Replace("~/", "").Replace("/", "\\");
            if (strPath.StartsWith("\\")) strPath = strPath.TrimStart('\\');

            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
        }
    }
}