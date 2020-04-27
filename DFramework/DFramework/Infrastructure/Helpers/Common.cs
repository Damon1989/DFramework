using System;

namespace DFramework.Infrastructure.Helpers
{
    /// <summary>
    /// 常用公共操作
    /// </summary>
    public static partial class Common
    {
        /// <summary>
        /// 获取类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Type GetType<T>()
        {
            return GetType(typeof(T));
        }

        /// <summary>
        /// 获取类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetType(Type type)
        {
            return Nullable.GetUnderlyingType(type) ?? type;
        }

        /// <summary>
        /// 换行符
        /// </summary>
        public static string Line => Environment.NewLine;

        //public static string GetPhysicalPath(string relativePath)
        //{
        //    if (string.IsNullOrWhiteSpace(relativePath))
        //    {
        //        return string.Empty;
        //    }
        //    var rootPath=Web
        //}
    }
}