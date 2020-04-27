﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace DCommon
{
    public class FileUploadHelper
    {
        /// <summary>
        ///   var files = new Dictionary<string, string> { { "1", "c:\\test\\1.book" }, { "2", "c:\\test\\1.jpg" } };
        ///   Console.WriteLine(HttpPost(m_address, null, files));
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <param name="files"></param>
        /// <returns></returns>
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
    }
}