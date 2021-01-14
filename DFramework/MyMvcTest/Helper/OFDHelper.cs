using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

namespace MyMvcTest.Helper
{
    public class OFDHelper
    {
        /// <summary>
        /// D:\\temp\\050002000213_00000003 (2).ofd
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static OFDInvoiceData GetInvoceData(string filePath)
        {
            var result = new OFDInvoiceData();
            if (File.Exists(filePath))
            {
                #region ofd 转 zip
                var zipFilePath = $"{filePath.Substring(0, filePath.LastIndexOf("."))}.zip";
                if (File.Exists(zipFilePath))
                {
                    File.Delete(zipFilePath);
                }
                File.Move(filePath, zipFilePath);
                #endregion

                #region zip 解压

                var zipFolder = $"{filePath.Substring(0, filePath.LastIndexOf("."))}";
                if (Directory.Exists(zipFolder))
                {
                    Directory.Delete(zipFolder, true);
                }

                ZipHelper.UnZip(zipFilePath, zipFolder);
                File.Delete(zipFilePath);

                #endregion


                #region 读取xml
                var ofdFile = $"{zipFolder}\\OFD.xml";
                if (File.Exists(ofdFile))
                {
                    var xml = new XmlDocument();
                    xml.Load(ofdFile);

                    XmlNamespaceManager nsp = new XmlNamespaceManager(xml.NameTable);
                    nsp.AddNamespace("ofd", "http://www.ofdspec.org/2016");

                    var nodeList = xml.SelectNodes("//ofd:CustomData", nsp);
                    foreach (XmlNode node in nodeList)
                    {
                        var name = node.Attributes?.Count > 0 ? node.Attributes[0].Value : "";
                        var value = node.InnerText;
                        switch (name)
                        {
                            case "发票代码":
                                result.Code = value;
                                break;
                            case "发票号码":
                                result.Number = value;
                                break;
                            case "合计税额":
                                result.TotalTax = value;
                                break;
                            case "合计金额":
                                result.TotalAmount = value;
                                break;
                            case "开票日期":
                                result.BillingDate = value;
                                break;
                            case "校验码":
                                result.CheckCode = Regex.Replace(value.Trim(), "\\s+", "");
                                break;

                        }
                    }

                    Directory.Delete(zipFolder, true);
                }
                #endregion


            }
            return result;
        }

        public class OFDInvoiceData
        {
            /// <summary>
            /// 发票代码
            /// </summary>
            public string Code { get; set; }
            /// <summary>
            /// 发票号码
            /// </summary>
            public string Number { get; set; }
            /// <summary>
            /// 合计税额
            /// </summary>
            public string TotalTax { get; set; }
            /// <summary>
            /// 合计金额
            /// </summary>
            public string TotalAmount { get; set; }
            /// <summary>
            /// 开票日期
            /// </summary>
            public string BillingDate { get; set; }
            /// <summary>
            /// 校验码
            /// </summary>
            public string CheckCode { get; set; }
        }
    }
}
