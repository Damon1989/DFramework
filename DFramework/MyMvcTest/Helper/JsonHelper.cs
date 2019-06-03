using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace MyMvcTest.Helper
{
    public class CustomContractResolver : DefaultContractResolver
    {
        private readonly bool _lowerCase;
        private readonly bool _serializeNonPublic;

        public CustomContractResolver(bool serializeNonPublic, bool lowerCase)
        {
            _serializeNonPublic = serializeNonPublic;
            _lowerCase = lowerCase;
        }

        protected override string ResolvePropertyName(string propertyName)
        {
            return _lowerCase ? propertyName.ToLower() : propertyName;
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            if (!_serializeNonPublic)
            {
                return base.CreateProperties(type, memberSerialization);
            }

            var props = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var jsonProps = new List<JsonProperty>();
            foreach (var prop in props)
            {
                jsonProps.Add(base.CreateProperty(prop, memberSerialization));
            }

            jsonProps.ForEach(item =>
            {
                item.Writable = true;
                item.Readable = true;
            });
            return jsonProps;

        }

        //protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        //{
        //    if (_serializeNonPublic)
        //    {
        //        var prop = base.CreateProperty(member, memberSerialization);
        //        if (!prop.Writable)
        //        {
        //            var property = member as PropertyInfo;
        //            if (property != null)
        //            {
        //                prop.Writable = true;
        //                prop.Readable = true;
        //                //var hasPrivateSetter = property.GetSetMethod(true) != null;
        //                //prop.Writable = hasPrivateSetter;
        //            }
        //        }

        //        return prop;
        //    }

        //    return base.CreateProperty(member, memberSerialization);
        //}

    }


    public static class JsonHelper
    {
        private static readonly ConcurrentDictionary<string, JsonSerializerSettings> SettingDictionary =
            new ConcurrentDictionary<string, JsonSerializerSettings>();


        internal static JsonSerializerSettings InternalGetCustomJsonSerializerStrings(bool serializeNonPublic,
            bool loopSerialize,
            bool useCamelCase,
            bool useStringEnumConvert,
            bool ignoreSerializableAttribute,
            bool ignoreNullValue,
            bool lowerCase)
        {
            var customSettings = new JsonSerializerSettings
            {
                ContractResolver = new CustomContractResolver(serializeNonPublic, lowerCase)
            };
            if (loopSerialize)
            {
                customSettings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
                customSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
            }

            if (useStringEnumConvert) customSettings.Converters.Add(new StringEnumConverter());

            ((DefaultContractResolver) customSettings.ContractResolver).IgnoreSerializableAttribute =
                ignoreSerializableAttribute;
            if (useCamelCase)
            {
                var resolver = (DefaultContractResolver) customSettings.ContractResolver;
                resolver.NamingStrategy = new CamelCaseNamingStrategy
                {
                    ProcessDictionaryKeys = true,
                    OverrideSpecifiedNames = true
                };
            }

            if (ignoreNullValue) customSettings.NullValueHandling = NullValueHandling.Ignore;

            return customSettings;
        }

        public static JsonSerializerSettings GetCustomJsonSerializerSettings(bool serializeNonPublic,
            bool loopSerialize,
            bool useCamelCase,
            bool useStringEnumConvert = true,
            bool ignoreSerializableAttribute = true,
            bool ignoreNullValue = false,
            bool useCached = true,
            bool lowerCase = false)
        {
            JsonSerializerSettings settings = null;
            if (useCached)
            {
                var key = $"{serializeNonPublic}{loopSerialize}{useCamelCase}{useStringEnumConvert}";
                settings = SettingDictionary.GetOrAdd(key, k => InternalGetCustomJsonSerializerStrings(
                    serializeNonPublic,
                    loopSerialize,
                    useCamelCase,
                    useStringEnumConvert,
                    ignoreSerializableAttribute,
                    ignoreNullValue, lowerCase));
            }
            else
            {
                settings = InternalGetCustomJsonSerializerStrings(
                    serializeNonPublic,
                    loopSerialize,
                    useCamelCase,
                    useStringEnumConvert,
                    ignoreSerializableAttribute,
                    ignoreNullValue, lowerCase);
            }

            return settings;
        }


        public static string ToJsonString(this object obj,
            bool serializeNonPublic = true,
            bool loopSerialize = false,
            bool useCamelCase = false,
            bool ignoreNullValue = false,
            bool useStringEnumConvert = true)
        {
            return JsonConvert.SerializeObject(obj,
                GetCustomJsonSerializerSettings(serializeNonPublic, loopSerialize, useCamelCase, useStringEnumConvert,
                    ignoreNullValue: ignoreNullValue));
        }

        public static object ToJsonObject(this string json,
            bool serializeNonPublic = true,
            bool loopSerialize = false,
            bool useCamelCase = false)
        {
            if (string.IsNullOrEmpty(json)) return null;
            return JsonConvert.DeserializeObject(json,
                GetCustomJsonSerializerSettings(serializeNonPublic, loopSerialize, useCamelCase));
        }

        public static T ToJsonObject<T>(this string json,
            bool serializeNonPublic = true,
            bool loopSerialize = false,
            bool useCamelCase = false)
        {
            if (string.IsNullOrEmpty(json)) return default(T);
            return JsonConvert.DeserializeObject<T>(json,
                GetCustomJsonSerializerSettings(serializeNonPublic, loopSerialize, useCamelCase));
        }
    }
}