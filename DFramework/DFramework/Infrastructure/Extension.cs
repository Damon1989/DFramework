using System.ComponentModel;

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
    }
}