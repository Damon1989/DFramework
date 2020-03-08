using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyMvcTest.App_Start
{
    public static class ParameterHelper
    {
        public static Dictionary<string, string> GetParametersInfo(ActionExecutedContext filterContext)
        {
            var paraDic = new Dictionary<string, string>();

            foreach (var item in filterContext.ActionDescriptor.GetParameters())
            {
                var itemType = item.ParameterType;

                if (itemType.IsGenericType) //泛型
                {
                    var paraName = filterContext.ActionDescriptor.GetParameters()[0].ParameterName;
                    for (var i = 0; i < 500; i++)
                        foreach (var itemInfo in itemType.GenericTypeArguments)
                        {
                            var infos = itemInfo.GetProperties();
                            foreach (var info in infos)
                                if (info.CanRead)
                                {
                                    var key1 = $"{paraName}[{i}].{info.Name}";
                                    var key2 = $"[{i}].{info.Name}";
                                    var propertyValue1 =
                                        filterContext.Controller.ValueProvider.GetValue(key1);
                                    var propertyValue2 =
                                        filterContext.Controller.ValueProvider.GetValue(key2);
                                    if (propertyValue1 == null && propertyValue2 == null) continue;
                                    if (!paraDic.ContainsKey(key1) && propertyValue1 != null)
                                        paraDic.Add(key1, propertyValue1.AttemptedValue);
                                    if (!paraDic.ContainsKey(key2) && propertyValue2 != null)
                                        paraDic.Add(key2, propertyValue2.AttemptedValue);
                                }
                        }
                }
                else if (itemType.IsClass && itemType.Name != "String")
                {
                    var infos = itemType.GetProperties();
                    var paraName = filterContext.ActionDescriptor.GetParameters()[0].ParameterName;
                    foreach (var info in infos)
                        if (info.CanRead)
                        {
                            var isGenericType = info.PropertyType.IsGenericType;

                            if (isGenericType)
                            {
                                var keyName = $"{paraName}.{info.Name}";
                                for (var i = 0; i < 500; i++)
                                {
                                    var arguments = info.PropertyType.GenericTypeArguments;
                                    foreach (var argu in arguments)
                                        foreach (var property in argu.GetProperties())
                                        {
                                            var newKeyName1 = $"{keyName}[{i}].{property.Name}";
                                            var newKeyName2 = $"[{i}].{property.Name}";
                                            var propertyValue1 =
                                                filterContext.Controller.ValueProvider.GetValue(newKeyName1);
                                            var propertyValue2 =
                                                filterContext.Controller.ValueProvider.GetValue(newKeyName2);
                                            if (propertyValue1 == null && propertyValue2 == null) continue;
                                            if (!paraDic.ContainsKey(newKeyName1) && propertyValue1 != null)
                                                paraDic.Add(newKeyName1,
                                                    propertyValue1.AttemptedValue);
                                            if (!paraDic.ContainsKey(newKeyName2) && propertyValue2 != null)
                                                paraDic.Add(newKeyName2,
                                                    propertyValue2.AttemptedValue);
                                        }
                                }
                            }
                            else
                            {
                                var keyName1 = info.Name;
                                var keyName2 = $"{paraName}.{info.Name}";
                                var propertyValue1 =
                                    filterContext.Controller.ValueProvider.GetValue(keyName1);
                                var propertyValue2 =
                                    filterContext.Controller.ValueProvider.GetValue(keyName2);
                                if (!paraDic.ContainsKey(keyName1) && propertyValue1 != null)
                                    paraDic.Add(keyName1, propertyValue1.AttemptedValue);
                                if (!paraDic.ContainsKey(keyName2) && propertyValue2 != null)
                                    paraDic.Add(keyName2, propertyValue2.AttemptedValue);
                            }
                        }
                }
                else
                {
                    var parameterValue = filterContext.Controller.ValueProvider.GetValue(item.ParameterName);
                    if (!paraDic.ContainsKey(item.ParameterName))
                        paraDic.Add(item.ParameterName, null == parameterValue ? "" : parameterValue.AttemptedValue);
                }
            }

            return paraDic;
        }
    }
}