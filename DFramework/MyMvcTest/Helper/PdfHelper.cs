using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spire.Pdf;

namespace MyMvcTest.Helper
{
    public class PdfHelper
    {
        /// <summary>
        /// pdfFilePath  D:\\temp\\.archivetemp9131023075315045X0_42c9136563494fdeabc95264f62694ea.pdf
        /// targetFilePath  $"D:\\temp\\{Guid.NewGuid()}.png"
        /// </summary>
        /// <param name="pdfFilePath"></param>
        /// <param name="targetFilePath"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public static string ConvertToPng(string pdfFilePath,string targetFilePath,int pageIndex=0)
        {
            var doc = new PdfDocument();
            try
            {
                if (!File.Exists(pdfFilePath)) return string.Empty;
                doc.LoadFromFile(pdfFilePath);
                var bmp = doc.SaveAsImage(pageIndex);
                bmp.Save(targetFilePath, ImageFormat.Png);
                return targetFilePath;
            }
            catch (Exception e)
            {
                return string.Empty;
            }
            finally
            {
                doc.Dispose();
            }
        }
    }
}
