using DFramework.Infrastructure;
using System;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace DFramework.JsonNet
{
    public class JsonConvertImpl : IJsonConvert
    {
        public T DeserializeAnonymousType<T>(string value, T anonymousTypeObject, bool serializeNonPublic, bool loopSerialize = false, bool useCamelCase = false)
        {
            return JsonConvert.DeserializeAnonymousType(value, anonymousTypeObject, JsonHelper.GetCustomJsonSerializerSettings(serializeNonPublic, loopSerialize, useCamelCase));
        }

        public dynamic DeserializeDynamicObject(string json, bool serializeNonPublic = true, bool loopSerialize = false, bool useCamelCase = false)
        {
            return json.ToDynamicObject(serializeNonPublic, loopSerialize, useCamelCase);
        }

        public dynamic DeserializeDynamicObjects(string json, bool serializeNonPublic = true, bool loopSerialize = false, bool useCamelCase = false)
        {
            return json.ToDynamicObjects(serializeNonPublic, loopSerialize, useCamelCase);
        }

        public object DeserializeObject(string value, bool serializeNonPublic = true, bool loopSerialize = false, bool useCamelCase = false)
        {
            return value.ToJsonObject(serializeNonPublic, loopSerialize, useCamelCase);
        }

        public T DeserializeObject<T>(string value, bool serializeNonPublic = true, bool loopSerialize = false, bool useCamelCase = false)
        {
            return value.ToJsonObject<T>(serializeNonPublic, loopSerialize, useCamelCase);
        }

        public object DeserializeObject(string value, Type type, bool serializeNonPublic = true, bool loopSerialize = false, bool useCamelCase = false)
        {
            return value.ToJsonObject(type, serializeNonPublic, loopSerialize, useCamelCase);
        }

        public XmlDocument DeserializeXmlNode(string value)
        {
            return JsonConvert.DeserializeXmlNode(value);
        }

        public XmlDocument DeserializeXmlNode(string value, string deserializeRootElementName)
        {
            return JsonConvert.DeserializeXmlNode(value, deserializeRootElementName);
        }

        public XmlDocument DeserializeXmlNode(string value, string deserializeRootElementName, bool writeArrayAttribute)
        {
            return JsonConvert.DeserializeXmlNode(value, deserializeRootElementName, writeArrayAttribute);
        }

        public XDocument DeserializeXNode(string value)
        {
            return JsonConvert.DeserializeXNode(value);
        }

        public XDocument DeserializeXNode(string value, string deserializeRootElementName)
        {
            return JsonConvert.DeserializeXNode(value, deserializeRootElementName);
        }

        public XDocument DeserializeXNode(string value, string deserializeRootElementName, bool writeArrayAttribute)
        {
            return JsonConvert.DeserializeXNode(value, deserializeRootElementName, writeArrayAttribute);
        }

        public void PopulateObject(string value, object target, bool serializeNonPublic = true, bool loopSerialize = false, bool useCamelCase = false)
        {
            JsonConvert.PopulateObject(value, target, JsonHelper.GetCustomJsonSerializerSettings(serializeNonPublic, loopSerialize, useCamelCase));
        }

        public string SerializeObject(object value, bool serializeNonPublish = true, bool loopSerialize = false, bool useCamelCase = false, bool ignoreNullValue = false, bool useStringEnumConvert = true)
        {
            return value.ToJson(serializeNonPublish, loopSerialize, useCamelCase, ignoreNullValue, useStringEnumConvert);
        }

        public string SerializeXmlNode(XmlNode node)
        {
            return JsonConvert.SerializeXmlNode(node);
        }

        public string SerializeXNode(XObject node)
        {
            return JsonConvert.SerializeXNode(node);
        }
    }
}