using DFramework.Infrastructure.Logging;
using DFramework.IoC;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DFramework.JsonNet
{
    public class CustomContractResolver : DefaultContractResolver
    {
        private readonly bool _serializeNonPublic;
        private readonly bool _lowerCase;

        public CustomContractResolver(bool serializeNonPublic, bool lowerCase)
        {
            _serializeNonPublic = serializeNonPublic;
            _lowerCase = lowerCase;
        }

        protected override string ResolvePropertyName(string propertyName)
        {
            return _lowerCase ? propertyName.ToLower() : propertyName;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            if (_serializeNonPublic)
            {
                var prop = base.CreateProperty(member, memberSerialization);
                if (!prop.Writable)
                {
                    var property = member as PropertyInfo;
                    if (property != null)
                    {
                        var hasPrivateSetter = property.GetSetMethod(true) != null;
                        prop.Writable = hasPrivateSetter;
                    }
                }
                return prop;
            }
            else
            {
                return base.CreateProperty(member, memberSerialization);
            }
        }
    }

    public static class JsonHelper
    {
        private static readonly ConcurrentDictionary<string, JsonSerializerSettings> SettingDictionary = new ConcurrentDictionary<string, JsonSerializerSettings>();

        private static readonly ILogger JsonLogger = null;//IoCFactory.IsInit()
                                                          //? IoCFactory.Resolve<ILoggerFactory>().Create(typeof(JsonHelper).Name) : null;

        internal static JsonSerializerSettings InternalGetCustomJsonSerializerSettings(bool serializeNonPublic,
                                                                                       bool loopSerialize,
                                                                                       bool useCamelCase,
                                                                                       bool useStringEnumConvert,
                                                                                       bool ignoreSerializeableAttribute,
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
            if (useStringEnumConvert)
            {
                customSettings.Converters.Add(new StringEnumConverter());
            }
            ((DefaultContractResolver)customSettings.ContractResolver).IgnoreSerializableAttribute = ignoreSerializeableAttribute;
            if (useCamelCase)
            {
                var resolver = customSettings.ContractResolver as DefaultContractResolver;
                if (resolver != null)
                {
                    resolver.NamingStrategy = new CamelCaseNamingStrategy
                    {
                        ProcessDictionaryKeys = true,
                        OverrideSpecifiedNames = true
                    };
                }
            }
            if (ignoreNullValue)
            {
                customSettings.NullValueHandling = NullValueHandling.Ignore;
            }
            return customSettings;
        }

        public static JsonSerializerSettings GetCustomJsonSerializerSettings(bool serializeNonPublic,
                                                                             bool loopSerialize,
                                                                             bool useCamcelCase,
                                                                             bool useStringEnumConvert = true,
                                                                             bool ignoreSerializeableAttribute = true,
                                                                             bool ignoreNullValue = false,
                                                                             bool useCached = true,
                                                                             bool lowerCase = false)
        {
            JsonSerializerSettings settings = null;
            if (useCached)
            {
                var key = $"{serializeNonPublic}{loopSerialize}{useCamcelCase}{useStringEnumConvert}";
                settings = SettingDictionary.GetOrAdd(key,
                                                    k => InternalGetCustomJsonSerializerSettings(serializeNonPublic,
                                                                                                 loopSerialize,
                                                                                                 useCamcelCase,
                                                                                                 useStringEnumConvert,
                                                                                                 ignoreSerializeableAttribute,
                                                                                                 ignoreNullValue,
                                                                                                 lowerCase));
            }
            else
            {
                settings = InternalGetCustomJsonSerializerSettings(serializeNonPublic,
                                                                   loopSerialize,
                                                                   useCamcelCase,
                                                                   useStringEnumConvert,
                                                                   ignoreSerializeableAttribute,
                                                                   ignoreNullValue,
                                                                   lowerCase);
            }
            return settings;
        }

        public static string ToJson(this object obj,
                                    bool serializeNonPublic = true,
                                    bool loopSerialize = false,
                                    bool useCamelCase = false,
                                    bool ignoreNullValue = false,
                                    bool useStringEnumConvert = true)
        {
            return JsonConvert.SerializeObject(obj,
                                               GetCustomJsonSerializerSettings(serializeNonPublic, loopSerialize, useCamelCase, useStringEnumConvert, ignoreNullValue: ignoreNullValue));
        }

        public static object ToJsonObject(this string json,
                                          bool serializeNonPublic = true,
                                          bool loopSerialize = false,
                                          bool useCamelCase = false)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(json))
                {
                    return null;
                }
                return JsonConvert.DeserializeObject(json, GetCustomJsonSerializerSettings(serializeNonPublic, loopSerialize, useCamelCase));
            }
            catch (Exception ex)
            {
                JsonLogger?.Error($"ToJsonObject Failed {json}", ex);
                return null;
            }
        }

        public static object ToJsonObject(this string json,
                                          Type jsonType,
                                          bool serializeNonPublic = true,
                                          bool loopSerialize = false,
                                          bool useCamelCase = false)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return null;
            }
            try
            {
                if (jsonType == typeof(List<dynamic>))
                {
                    return json.ToDynamicObjects(serializeNonPublic, loopSerialize, useCamelCase);
                }
                if (jsonType == typeof(object))
                {
                    return json.ToDynamicObject(serializeNonPublic, loopSerialize, useCamelCase);
                }
                return JsonConvert.DeserializeObject(json, jsonType, GetCustomJsonSerializerSettings(serializeNonPublic, loopSerialize, useCamelCase));
            }
            catch (Exception ex)
            {
                JsonLogger?.Error($"ToJsonObject Failed {json}", ex);
                return null;
            }
        }

        public static T ToJsonObject<T>(this string json,
                                        bool serializeNonPublic = true,
                                        bool loopSerialize = false,
                                        bool useCamelCase = false)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return default(T);
            }
            try
            {
                if (typeof(T) == typeof(List<dynamic>))
                {
                    return (T)(object)json.ToDynamicObjects(serializeNonPublic, loopSerialize, useCamelCase);
                }
                if (typeof(T) == typeof(object))
                {
                    return json.ToDynamicObject(serializeNonPublic, loopSerialize, useCamelCase);
                }
                return JsonConvert.DeserializeObject<T>(json,
                                                        GetCustomJsonSerializerSettings(serializeNonPublic, loopSerialize, useCamelCase));
            }
            catch (Exception ex)
            {
                JsonLogger?.Error($"ToJsonObject Failed {json}", ex);
                return default(T);
            }
        }

        public static dynamic ToDynamicObject(this string json,
                                              bool serializeNonPublic = true,
                                              bool loopSerialize = false,
                                              bool useCamelCase = false)
        {
            return json.ToJsonObject<JObject>(serializeNonPublic, loopSerialize, useCamelCase);
        }

        public static List<dynamic> ToDynamicObjects(this string json,
                                                     bool serializeNonPublic = true,
                                                     bool loopSerialize = false,
                                                     bool useCamelCase = false)
        {
            return json.ToJsonObject<JArray>(serializeNonPublic, loopSerialize)
                       .Cast<dynamic>()
                       .ToList();
        }
    }
}