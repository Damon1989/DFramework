﻿namespace DCommon.RawHelper
{
    using System;
    using System.Reflection;

    public static class Enum
    {
        /// <summary>
        ///     获取描述,使用System.ComponentModel.Description特性设置描述
        /// </summary>
        /// <param name="type">枚举类型</param>
        /// <param name="member">成员名、值、实例均可</param>
        public static string GetDescription(Type type, object member)
        {
            return Reflection.GetDescription(type, GetName(type, member));
        }

        public static string GetName<TEnum>(object member)
        {
            return GetName(Common.GetType<TEnum>(), member);
        }

        public static string GetName(Type type, object member)
        {
            if (type == null || member == null) return string.Empty;

            if (member is string) return member.ToString();

            if (type.GetTypeInfo().IsEnum == false) return string.Empty;

            return GetName(type, member);
        }

        /// <summary>
        ///     获取成员值
        /// </summary>
        /// <param name="type">枚举类型</param>
        /// <param name="member">成员名、值、实例均可</param>
        public static int GetValue(Type type, object member)
        {
            var value = member.SafeString();
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(member));
            return (int)System.Enum.Parse(type, member.ToString(), true);
        }

        /// <summary>
        ///     获取成员值
        /// </summary>
        /// <typeparam name="TEnum">枚举类型</typeparam>
        /// <param name="member">成员名、值、实例均可，范例:Enum1枚举有成员A=0,可传入"A"、0、Enum1.A，获取值0</param>
        public static int GetValue<TEnum>(object member)
        {
            return GetValue(Common.GetType<TEnum>(), member);
        }

        /// <summary>
        ///     获取实例
        /// </summary>
        /// <typeparam name="TEnum">枚举类型</typeparam>
        /// <param name="member">成员名或值,范例:Enum1枚举有成员A=0,则传入"A"或"0"获取 Enum1.A</param>
        public static TEnum Parse<TEnum>(object member)
        {
            var value = member.SafeString();
            if (string.IsNullOrWhiteSpace(value))
            {
                if (typeof(TEnum).IsGenericType)
                    return default;
                throw new ArgumentNullException(nameof(member));
            }

            return (TEnum)System.Enum.Parse(Common.GetType<TEnum>(), value, true);
        }
    }
}