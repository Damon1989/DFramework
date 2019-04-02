using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace DFramework.Infrastructure
{
    public static class Extension
    {
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static string GetDescription(this object obj)
        {
            var fi = obj.GetType().GetField(obj.ToString());
            var arrDesc = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return arrDesc.Length > 0 ? arrDesc[0].Description : null;
        }

        public static List<string> SplitStringByFenHao(this string splitStr)
        {
            return splitStr.IsNullOrEmpty() ? new List<string>() : splitStr.Split(';').ToList();
        }

        public static List<T> SplitStringByFenHao<T>(this string splitStr)
        {
            if (splitStr.IsNullOrEmpty())
            {
                return Array.Empty<T>().ToList();
            }
            var result=new List<T>();
            
            var sList=splitStr.Split(';').ToList();
            sList.ForEach(item =>
            {
                if (typeof(T)==typeof(int))
                {
                    result.Add((T)(object)int.Parse(item));
                }
                else
                {
                    result.Add((T)Convert.ChangeType(item,typeof(T)));
                }

            });
            return result;
        }

        public static bool IsNullOrCountZero<T>(this IEnumerable<T> source)
        {
            if (source == null || !source.Any())
                return true;
            return false;
        }

        public static string GetServerMapPath(this string filePath)
        {
            return MapPath(filePath);
        }

        public static string MapPath(this string strPath)
        {
            if (HttpContext.Current != null)
            {
                return HostingEnvironment.MapPath(strPath);
            }
            else//非web程序引用
            {
                strPath = strPath.Replace("~/", "").Replace("/", "\\");
                if (strPath.StartsWith("\\"))
                {
                    strPath = strPath.TrimStart('\\');
                }
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
            }
        }
    }
}