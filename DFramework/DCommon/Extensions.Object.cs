namespace DCommon
{
    using System;
    using System.ComponentModel;

    using DCommon.RawHelper;

    using Enum = System.Enum;

    public static partial class Extensions
    {
        public static string GetDescription(this object obj)
        {
            var fi = obj.GetType().GetField(obj.ToString());
            var arrDesc = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return arrDesc.Length > 0 ? arrDesc[0].Description : null;
        }

        public static bool IsNullOrEmpty(this object str)
        {
            return str == null || string.IsNullOrEmpty(str.ToString());
        }

        public static TEnum Parse<TEnum>(this object member)
        {
            var value = member.SafeString();
            if (string.IsNullOrWhiteSpace(value))
            {
                if (typeof(TEnum).IsGenericType)
                    return default;
                throw new ArgumentNullException(nameof(member));
            }

            return (TEnum)Enum.Parse(Common.GetType<TEnum>(), value, true);
        }

        public static string SafeString(this object input)
        {
            return input?.ToString().Trim() ?? string.Empty;
        }

        public static T SafeValue<T>(this T? value)
            where T : struct
        {
            return value ?? default;
        }
    }
}