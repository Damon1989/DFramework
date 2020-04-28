using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCommon
{
    public static partial class Extensions
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

        /// <summary>
        /// 获取格式化字符串，带时分秒，格式："yyyy-MM-dd HH:mm:ss"
        /// </summary>
        /// <param name="dateTime">日期</param>
        /// <param name="removeSecond">是否移除秒</param>
        public static string ToDateTimeString(this DateTime dateTime, bool removeSecond = false)
        {
            if (removeSecond)
                return dateTime.ToString("yyyy-MM-dd HH:mm");
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 获取格式化字符串，带时分秒，格式："yyyy-MM-dd HH:mm:ss"
        /// </summary>
        /// <param name="dateTime">日期</param>
        /// <param name="removeSecond">是否移除秒</param>
        public static string ToDateTimeString(this DateTime? dateTime, bool removeSecond = false)
        {
            if (dateTime == null)
                return string.Empty;
            return ToDateTimeString(dateTime.Value, removeSecond);
        }

        /// <summary>
        /// 获取格式化字符串，不带时分秒，格式："yyyy-MM-dd"
        /// </summary>
        /// <param name="dateTime">日期</param>
        public static string ToDateString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 获取格式化字符串，不带时分秒，格式："yyyy-MM-dd"
        /// </summary>
        /// <param name="dateTime">日期</param>
        public static string ToDateString(this DateTime? dateTime)
        {
            if (dateTime == null)
                return string.Empty;
            return ToDateString(dateTime.Value);
        }

        /// <summary>
        /// 获取格式化字符串，不带年月日，格式："HH:mm:ss"
        /// </summary>
        /// <param name="dateTime">日期</param>
        public static string ToTimeString(this DateTime dateTime)
        {
            return dateTime.ToString("HH:mm:ss");
        }

        /// <summary>
        /// 获取格式化字符串，不带年月日，格式："HH:mm:ss"
        /// </summary>
        /// <param name="dateTime">日期</param>
        public static string ToTimeString(this DateTime? dateTime)
        {
            if (dateTime == null)
                return string.Empty;
            return ToTimeString(dateTime.Value);
        }

        /// <summary>
        /// 获取格式化字符串，带毫秒，格式："yyyy-MM-dd HH:mm:ss.fff"
        /// </summary>
        /// <param name="dateTime">日期</param>
        public static string ToMillisecondString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

        /// <summary>
        /// 获取格式化字符串，带毫秒，格式："yyyy-MM-dd HH:mm:ss.fff"
        /// </summary>
        /// <param name="dateTime">日期</param>
        public static string ToMillisecondString(this DateTime? dateTime)
        {
            if (dateTime == null)
                return string.Empty;
            return ToMillisecondString(dateTime.Value);
        }

        /// <summary>
        /// 获取格式化字符串，不带时分秒，格式："yyyy年MM月dd日"
        /// </summary>
        /// <param name="dateTime">日期</param>
        public static string ToChineseDateString(this DateTime dateTime)
        {
            return string.Format("{0}年{1}月{2}日", dateTime.Year, dateTime.Month, dateTime.Day);
        }

        /// <summary>
        /// 获取格式化字符串，不带时分秒，格式："yyyy年MM月dd日"
        /// </summary>
        /// <param name="dateTime">日期</param>
        public static string ToChineseDateString(this DateTime? dateTime)
        {
            if (dateTime == null)
                return string.Empty;
            return ToChineseDateString(dateTime.SafeValue());
        }

        /// <summary>
        /// 获取格式化字符串，带时分秒，格式："yyyy年MM月dd日 HH时mm分"
        /// </summary>
        /// <param name="dateTime">日期</param>
        /// <param name="removeSecond">是否移除秒</param>
        public static string ToChineseDateTimeString(this DateTime dateTime, bool removeSecond = false)
        {
            StringBuilder result = new StringBuilder();
            result.AppendFormat("{0}年{1}月{2}日", dateTime.Year, dateTime.Month, dateTime.Day);
            result.AppendFormat(" {0}时{1}分", dateTime.Hour, dateTime.Minute);
            if (removeSecond == false)
                result.AppendFormat("{0}秒", dateTime.Second);
            return result.ToString();
        }

        /// <summary>
        /// 获取格式化字符串，带时分秒，格式："yyyy年MM月dd日 HH时mm分"
        /// </summary>
        /// <param name="dateTime">日期</param>
        /// <param name="removeSecond">是否移除秒</param>
        public static string ToChineseDateTimeString(this DateTime? dateTime, bool removeSecond = false)
        {
            if (dateTime == null)
                return string.Empty;
            return ToChineseDateTimeString(dateTime.Value, removeSecond);
        }

        /// <summary>
        /// 获取描述
        /// </summary>
        /// <param name="span">时间间隔</param>
        public static string Description(this TimeSpan span)
        {
            StringBuilder result = new StringBuilder();
            if (span.Days > 0)
                result.AppendFormat("{0}天", span.Days);
            if (span.Hours > 0)
                result.AppendFormat("{0}小时", span.Hours);
            if (span.Minutes > 0)
                result.AppendFormat("{0}分", span.Minutes);
            if (span.Seconds > 0)
                result.AppendFormat("{0}秒", span.Seconds);
            if (span.Milliseconds > 0)
                result.AppendFormat("{0}毫秒", span.Milliseconds);
            if (result.Length > 0)
                return result.ToString();
            return $"{span.TotalSeconds * 1000}毫秒";
        }
    }
}
