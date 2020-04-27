namespace DCommon
{
    public static partial class Extensions
    {
        public static bool IsNullOrEmpty(this object str)
        {
            return str == null || string.IsNullOrEmpty(str.ToString());
        }

        public static string GetDescription(this object obj)
        {
            var fi = obj.GetType().GetField(obj.ToString());
            var arrDesc = (System.ComponentModel.DescriptionAttribute[])fi.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
            return arrDesc.Length > 0 ? arrDesc[0].Description : null;
        }
    }
}