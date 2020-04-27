namespace DCommon
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Web;
    using Microsoft.Office.Interop.Excel;
    using Excel = Microsoft.Office.Interop.Excel;
    public class ExcelConverter
    {
        public ExcelConverter()
        {
        }

        public static string ConvertTopdf(string excelFilePath, string fileExtension = "xls")
        {
            Application app = null;
            Workbooks workbooks = null;
            Workbook workbook = null;
            var filePathTemp = string.Empty;
            var targetFilePath = excelFilePath.Replace(fileExtension, "pdf");

            object lobjMissing = System.Reflection.Missing.Value;
            try
            {
                app = new Application { Visible = false };
                workbooks = app.Workbooks;
                workbook = workbooks.Open(excelFilePath, true, true, lobjMissing, lobjMissing,
                    lobjMissing, true, lobjMissing, lobjMissing, lobjMissing, lobjMissing, lobjMissing, false,
                    lobjMissing, lobjMissing);
                filePathTemp = System.IO.Path.GetTempPath() + Guid.NewGuid() + ".xls" +
                               (workbook.HasVBProject ? "m" : "x");
                workbook.SaveAs(filePathTemp, XlFileFormat.xlWorkbookNormal,
                    Type.Missing, Type.Missing, Type.Missing, false,
                    XlSaveAsAccessMode.xlNoChange, Type.Missing,
                    false, Type.Missing, Type.Missing, Type.Missing);
                workbook.ExportAsFixedFormat(XlFixedFormatType.xlTypePDF,
                    targetFilePath, XlFixedFormatQuality.xlQualityStandard, Type.Missing,
                    false, Type.Missing, Type.Missing, false, Type.Missing);
                workbooks.Close();
                app.Quit();
            }
            catch (Exception e)
            {
                return excelFilePath;
            }
            finally
            {
                try
                {
                    //if (workbook != null)
                    //{
                    //    workbook.Close(Type.Missing, Type.Missing, Type.Missing);
                    //    Marshal.ReleaseComObject(workbook);
                    //}
                    if (workbooks != null)
                    {
                        workbooks.Close();
                        Marshal.ReleaseComObject(workbooks);
                    }
                    if (app != null)
                    {
                        app.Quit();
                        Marshal.ReleaseComObject(app);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                //主动激活垃圾回收器，主要是避免超大批量转文档时，内存占用过多，而垃圾回收器并不是时刻都在运行！
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return targetFilePath;
        }

        //将excel文档转换成PDF格式
        public static string Convert(string sourcePath, XlFixedFormatType targetType = XlFixedFormatType.xlTypePDF)
        {
            bool result;
            object missing = Type.Missing;
            Excel.Application application = null;
            Workbook workBook = null;
            object target = sourcePath.Replace("xls", "pdf");
            try
            {
                application = new Excel.Application();

                object type = targetType;
                workBook = application.Workbooks.Open(sourcePath, missing, missing, missing, missing, missing,
                    missing, missing, missing, missing, missing, missing, missing, missing, missing);

                workBook.ExportAsFixedFormat(targetType, target, XlFixedFormatQuality.xlQualityStandard, true, false, missing, missing, missing, missing);
                result = true;
            }
            catch
            {
                result = false;
            }
            finally
            {
                if (workBook != null)
                {
                    workBook.Close(true, missing, missing);
                    workBook = null;
                }
                if (application != null)
                {
                    application.Quit();
                    application = null;
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return target.ToString();
        }
    }
}