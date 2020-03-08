using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace DFramework.FileUploadTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var files = new Dictionary<string, string> { { "1", "c:\\test\\1.book" }, { "2", "c:\\test\\1.jpg" } };
            //var files = new Dictionary<string, string> { { "1", "c:\\test\\1.book"  } };
            //var files = new Dictionary<string, string> { { "1", "c:\\test\\1.jpg" } };

            Console.WriteLine(HttpPost(m_address, null, files));

            //UploadFile("c:\\test\\1.book", "c:\\test\\" + Guid.NewGuid().ToString().Replace("-", ""));
            //UploadFile("c:\\test\\demo1.rar", "c:\\test\\" + Guid.NewGuid().ToString().Replace("-", ""));
            //UploadFile("c:\\test\\1.rar", "c:\\test\\" + Guid.NewGuid().ToString().Replace("-", ""));
            //Console.ReadLine();
        }

        //private static string m_address = "http://localhost:30863/LessonWork/UploadYcFileToLessonWork/4a6788f7-63d7-44ad-9909-cf87022fca65/b6b1ef91-36bd-4f73-8d77-292ec0555a9e?name=123";
        //private static string m_address = "http://kct.age06.com/Age06.ImplementSupport/LessonWork/UploadYcFileToLessonWork/4a6788f7-63d7-44ad-9909-cf87022fca65/cdd77fb3-5595-4f36-9882-88cb47fc3d83";
        //private static string m_address = "http://test.age06.com/ImplementSupportUAT/LessonWork/UploadYcFileToLessonWork/4a6788f7-63d7-44ad-9909-cf87022fca65/b6b1ef91-36bd-4f73-8d77-292ec0555a9e?name=1234";
        private static string m_address = "http://kct.age06.com/Age06.ImplementSupport/LessonWork/UploadYcFileToLessonWork/b7bdd069-f4bf-46f6-b325-7062952fcd1f/afa5ad68-5d85-4650-8691-73ba62dc0697?name=集体学习模板0828";

        public static string HttpPost(string url, Dictionary<string, string> postData, Dictionary<string, string> files)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            CookieContainer cookieContainer = new CookieContainer();
            request.CookieContainer = cookieContainer;
            request.AllowAutoRedirect = true;
            request.Method = "POST";
            string boundary = DateTime.Now.Ticks.ToString("X"); // 随机分隔线
            request.ContentType = "multipart/form-data;charset=utf-8;boundary=" + boundary;

            byte[] itemBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");
            byte[] endBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");

            Stream postStream = request.GetRequestStream();
            postStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);

            //写入文本
            if (postData != null && postData.Count > 0)
            {
                var keys = postData.Keys;
                foreach (var key in keys)
                {
                    string strHeader = string.Format("Content-Disposition: form-data; name=\"{0}\"\r\n\r\n", key);
                    byte[] strByte = System.Text.Encoding.UTF8.GetBytes(strHeader);
                    postStream.Write(strByte, 0, strByte.Length);

                    byte[] value = System.Text.Encoding.UTF8.GetBytes(string.Format("{0}", postData[key]));
                    postStream.Write(value, 0, value.Length);

                    postStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);
                }
            }
            postStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);

            //写入文件
            if (files != null && files.Count > 0)
            {
                var keys = files.Keys;

                foreach (var key in keys)
                {
                    string filePath = files[key];
                    int pos = filePath.LastIndexOf("\\");
                    string fileName = filePath.Substring(pos + 1);
                    StringBuilder sbHeader = new StringBuilder(string.Format("Content-Disposition:form-data;name=\"{0}\";filename=\"{1}\"\r\nContent-Type:application/octet-stream\r\n\r\n", key, fileName));
                    byte[] postHeaderBytes = Encoding.UTF8.GetBytes(sbHeader.ToString());

                    FileStream fs = new FileStream(files[key], FileMode.Open, FileAccess.Read);
                    byte[] bArr = new byte[fs.Length];
                    fs.Read(bArr, 0, bArr.Length);
                    fs.Close();
                    postStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
                    postStream.Write(bArr, 0, bArr.Length);

                    postStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);
                }
            }
            postStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length); //结束标志
            postStream.Close();
            //发送请求并获取相应回应数据
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            //直到request.GetResponse()程序才开始向目标网页发送Post请求
            Stream instream = response.GetResponseStream();
            StreamReader sr = new StreamReader(instream, Encoding.UTF8);
            //返回结果网页（html）代码
            string content = sr.ReadToEnd();
            return content;
        }

        public static void UploadFile(string sourceFilePath, string targetFilePath)
        {
            var sourceStream = new FileStream(sourceFilePath, FileMode.Open);
            var targetStream = new FileStream(targetFilePath, FileMode.Create);
            sourceStream.CopyTo(targetStream);
        }
    }
}