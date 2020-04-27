using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace DCommon
{
    public class XmlHelper
    {
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
            xmlDocument.Load((xmlFilePath.GetServerMapPath()));

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
            doc.Load(xmlFilePath.GetServerMapPath());
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
            doc.Load(xmlFilePath.GetServerMapPath());
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
    }

    public class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding => Encoding.UTF8;
    }
}