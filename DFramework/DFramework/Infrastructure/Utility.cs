using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Xml.Serialization;
using System.Xml;

namespace DFramework.Infrastructure
{
    public static class Utility
    {
        public static IEnumerable<T> OrEmptyIfNull<T>(this IEnumerable<T> source)
        {
            return source ?? Enumerable.Empty<T>();
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> act)
        {
            foreach (var element in source.OrEmptyIfNull())
            {
                act(element);
            }
            return source;
        }

        #region IP

        /// <summary>
        /// 获取本机IP
        /// </summary>
        public static IPAddress HostIPv4
        {
            get
            {
                return Dns.GetHostEntry(Dns.GetHostName())
                            .AddressList
                            .First(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
            }
        }

        /// <summary>
        /// 获取客户端IP地址(无视代理)
        /// </summary>
        /// <returns>若失败则返回回送地址</returns>
        public static string GetHostAddress()
        {
            try
            {
                var userHostAddress = System.Web.HttpContext.Current.Request.UserHostAddress;
                if (string.IsNullOrEmpty(userHostAddress))
                {
                    userHostAddress = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                //最后判断获取是否成功，并检查IP地址的格式（检查其格式非常重要）
                if (!string.IsNullOrEmpty(userHostAddress) && IsIp(userHostAddress))
                {
                    return userHostAddress;
                }
                return "127.0.0.1";
            }
            catch
            {
                return "127.0.0.1";
            }
        }

        private static bool IsIp(string ip)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        #endregion IP

        public static string XmlSerialize<T>(T entity)
        {
            string utf8Result;
            var serializer = new XmlSerializer(typeof(T));
            using (var writer = new Utf8StringWriter())
            {
                var xm = new XmlSerializerNamespaces();
                serializer.Serialize(writer, entity, xm);
                utf8Result = writer.ToString();
            }
            return utf8Result;
        }

        public static T DeXmlSerialize<T>(string xmlString)
        {
            T resultObject;
            var serializer = new XmlSerializer(typeof(T));
            using (var reader = new StringReader(xmlString))
            {
                resultObject = (T)serializer.Deserialize(reader);
            }
            return resultObject;
        }

        public static T DeXmlSerializeByFilePath<T>(string xmlFilePath)
        {
            T resultObject;
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(GetServerMapPath(xmlFilePath));

            var serializer = new XmlSerializer(typeof(T));
            using (var reader = new StringReader(xmlDocument.InnerXml))
            {
                resultObject = (T)serializer.Deserialize(reader);
            }
            return resultObject;
        }

        public static string[] GetXmlElementNameArray(string xmlFilePath)
        {
            var doc = new XmlDocument();
            doc.Load(GetServerMapPath(xmlFilePath));
            XmlNode rootNode = doc.DocumentElement;

            if (rootNode == null)
            {
                return new string[0];
            }
            var array = new List<string>();
            for (int i = 0; i < rootNode.ChildNodes.Count; i++)
            {
                array.Add(rootNode.ChildNodes[i].Name);
            }
            return string.Join(",", array).Split(',');
        }

        public static XmlElement GetXmlElement(string xmlFilePath)
        {
            var doc = new XmlDocument();
            doc.Load(GetServerMapPath(xmlFilePath));
            return doc.DocumentElement;
        }

        public static string GetXmlElementValue(XmlElement rootNode, string xmlName)
        {
            if (rootNode == null)
            {
                return string.Empty;
            }
            var result = string.Empty;
            for (int i = 0; i < rootNode.ChildNodes.Count; i++)
            {
                if (!rootNode.ChildNodes[i].Name.Equals(xmlName, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
                result = rootNode.ChildNodes[i].InnerText;
                break;
            }
            return result;
        }

        public static string GetServerMapPath(string filePath)
        {
            return MapPath(filePath);
        }

        public static string MapPath(string strPath)
        {
            if (System.Web.HttpContext.Current != null)
            {
                return HostingEnvironment.MapPath(strPath);
            }
            else//非web程序引用
            {
                strPath = strPath.Replace("~/", "").Replace("/", "\\");
                if (strPath.StartsWith("\\"))
                {
                    strPath = strPath.TrimStart('\\');
                }
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
            }
        }
    }

    public class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding => Encoding.UTF8;
    }
}