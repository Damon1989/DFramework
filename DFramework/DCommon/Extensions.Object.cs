namespace DCommon
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;

    using DCommon.RawHelper;

    using Convert = System.Convert;
    using Enum = System.Enum;

    public static partial class Extensions
    {
        /// <summary>
        ///     Used to simplify and beautify casting an object to a type
        /// </summary>
        /// <typeparam name="T">Type to be casted</typeparam>
        /// <param name="obj">Object to cast</param>
        /// <returns>Casted object</returns>
        public static T As<T>(this object obj)
            where T : class
        {
            return (T)obj;
        }

        public static string GetDescription(this object obj)
        {
            var fi = obj.GetType().GetField(obj.ToString());
            var arrDesc = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return arrDesc.Length > 0 ? arrDesc[0].Description : null;
        }

        /// <summary>
        ///     Check if an item is in a list
        /// </summary>
        /// <typeparam name="T">Type of the items</typeparam>
        /// <param name="item">Item to check</param>
        /// <param name="list">List of items</param>
        /// <returns></returns>
        public static bool IsIn<T>(this T item, params T[] list)
        {
            return list.Contains(item);
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

        /// <summary>
        ///     Converts given object to a value or enum type using <see cref="System.Convert.ChangeType(object, Type)" /> or
        ///     <see cref="System.Enum.Parse(Type, string)" /> method.
        /// </summary>
        /// <typeparam name="T">Type of the target object</typeparam>
        /// <param name="obj">Object to be converted</param>
        /// <returns></returns>
        public static T To<T>(this object obj)
            where T : struct
        {
            if (typeof(T) == typeof(Guid) || typeof(T) == typeof(TimeSpan))
                return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(obj.ToString());

            if (typeof(T).IsEnum)
            {
                if (Enum.IsDefined(typeof(T), obj)) return (T)Enum.Parse(typeof(T), obj.ToString());
                throw new ArgumentException($"Enum type undefined '{obj}'");
            }

            return (T)Convert.ChangeType(obj, typeof(T), CultureInfo.InvariantCulture);
        }
    }
}