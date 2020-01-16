using System;

namespace MyMvcTest.Helper
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// 19
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DateTimeToYy(this DateTime dt)
        {
            return dt.ToString("yy");
        }
        /// <summary>
        /// 2019
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DateTimeToYyyy(this DateTime dt)
        {
            return dt.ToString("yyyy");
        }

        /// <summary>
        /// 05
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DateTimeToMonth(this DateTime dt)
        {
            return dt.ToString("MM");
        }
        /// <summary>
        /// 09
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DateTimeToDd(this DateTime dt)
        {
            return dt.ToString("dd");
        }

        /// <summary>
        /// 周一
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DateTimeToDdd(this DateTime dt)
        {
            return dt.ToString("ddd");
        }
        /// <summary>
        /// 星期一
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DateTimeToDddd(this DateTime dt)
        {
            return dt.ToString("dddd");
        }
        /// <summary>
        /// 12小时制的小时数
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DateTimeToHh12(this DateTime dt)
        {
            return dt.ToString("hh");
        }
        /// <summary>
        /// 24小时制的小时数
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DateTimeToHh24(this DateTime dt)
        {
            return dt.ToString("HH");
        }

        /// <summary>
        /// 分钟数
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DateTimeToMinutes(this DateTime dt)
        {
            return dt.ToString("mm");
        }

        /// <summary>
        /// 秒数
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DateTimeToSs(this DateTime dt)
        {
            return dt.ToString("ss");
        }
        /// <summary>
        /// 毫秒数前2位
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DateTimeToFf(this DateTime dt)
        {
            return dt.ToString("ff");
        }
        /// <summary>
        /// 毫秒数前3位
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DateTimeToFff(this DateTime dt)
        {
            return dt.ToString("fff");
        }
        /// <summary>
        /// 毫秒数前4位
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DateTimeToFfff(this DateTime dt)
        {
            return dt.ToString("ffff");
        }

        /// <summary>
        /// yyyy-MM-dd
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DateTimeFormat1(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// yyyy-MM-dd HH:mm:ss:ffff
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DateTimeFormat11(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss:ffff");
        }

        /// <summary>
        /// yyyy/MM/dd
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DateTimeFormat2(this DateTime dt)
        {
            return dt.ToString("yyyy/MM/dd");
        }

        /// <summary>
        /// yyyy/MM/dd HH:mm:ss:ffff
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DateTimeFormat22(this DateTime dt)
        {
            return dt.ToString("yyyy/MM/dd HH:mm:ss:ffff");
        }
    }
}