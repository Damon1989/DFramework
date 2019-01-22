using DFramework.Infrastructure.Logging;
using DFramework.IoC;
using System;
using System.Collections.Generic;

namespace DFramework.Infrastructure
{
    public static class JsonHelper
    {
        private static IJsonConvert JsonConvert => IoCFactory.Resolve<IJsonConvert>();
        private static ILogger jsonLogger => IoCFactory.Resolve<ILoggerFactory>().Create(typeof(JsonHelper).Name);

        public static string ToJson(this object obj,
                                    bool serializeNonPublic = true,
                                    bool loopSerialize = false,
                                    bool useCamelCase = false,
                                    bool ignoreNullValue = false)
        {
            return JsonConvert.SerializeObject(obj, serializeNonPublic, loopSerialize, useCamelCase, ignoreNullValue);
        }

        public static object ToJsonObject(this string json,
                                          bool serializeNonPublic = true,
                                          bool loopSerialize = false,
                                          bool useCamelCase = false)
        {
            return JsonConvert.DeserializeObject(json, serializeNonPublic, loopSerialize, useCamelCase);
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
                return JsonConvert.DeserializeObject(json, jsonType, serializeNonPublic, loopSerialize, useCamelCase);
            }
            catch (Exception ex)
            {
                jsonLogger?.Error($"ToJsonObject Failed {json}", ex);
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
                return JsonConvert.DeserializeObject<T>(json, serializeNonPublic, loopSerialize, useCamelCase);
            }
            catch (Exception ex)
            {
                jsonLogger?.Error($"ToJsonObject Failed {json}", ex);
                return default(T);
            }
        }

        public static dynamic ToDynamicObject(this string json,
                                              bool serializeNonPublic = true,
                                              bool loopSerialize = false,
                                              bool useCamelCase = false)
        {
            return JsonConvert.DeserializeDynamicObject(json, serializeNonPublic, loopSerialize, useCamelCase);
        }

        public static List<dynamic> ToDynamicObjects(this string json,
                                                     bool serializeNonPublic = true,
                                                     bool loopSerialize = false,
                                                     bool useCamelCase = false)
        {
            return JsonConvert.DeserializeDynamicObjects(json, serializeNonPublic, loopSerialize, useCamelCase);
        }
    }
}