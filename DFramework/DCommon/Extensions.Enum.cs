namespace DCommon
{
    using System;

    using Convert = RawHelper.Convert;

    public static partial class Extensions
    {
        /// <summary>
        ///     获取枚举描述,使用System.ComponentModel.Description特性设置描述
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static string Description(this Enum instance)
        {
            return instance == null ? string.Empty : RawHelper.Enum.GetDescription(instance.GetType(), instance);
        }

        /// <summary>
        ///     获取枚举值
        /// </summary>
        /// <param name="instance">
        ///     <枚举实例/ param>
        ///         <returns></returns>
        public static int Value(this Enum instance)
        {
            return instance == null ? 0 : RawHelper.Enum.GetValue(instance.GetType(), instance);
        }

        /// <summary>
        ///     获取枚举值
        /// </summary>
        /// <typeparam name="TResult">返回值类型</typeparam>
        /// <param name="instance">枚举实例</param>
        /// <returns></returns>
        public static TResult Value<TResult>(this Enum instance)
        {
            return instance == null ? default : Convert.To<TResult>(Value(instance));
        }
    }
}