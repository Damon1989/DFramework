using System.ComponentModel;
using System.Reflection;

namespace DCommon.RawHelper
{
    using System;
    using System.Linq;

    public class Reflection
    {
        public static string GetDescription<T>()
        {
            return GetDescription(RawHelper.Common.GetType<T>());
        }

        public static string GetDescription<T>(string memberName)
        {
            return GetDescription(RawHelper.Common.GetType<T>(), memberName);
        }

        public static string GetDescription(Type type, string memberName)
        {
            return type == null || string.IsNullOrWhiteSpace(memberName)
                       ? string.Empty
                       : GetDescription(type.GetTypeInfo().GetMember(memberName).FirstOrDefault());
        }


        /// <summary>
        /// 获取类型成员描述，使用DescriptionAttribute设置描述
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public static string GetDescription(MemberInfo member)
        {
            return member == null
                       ? string.Empty
                       :
                       member.GetCustomAttribute<DescriptionAttribute>() is DescriptionAttribute attribute
                           ?
                           attribute.Description
                           : member.Name;
        }
    }
}