namespace DCommon
{
    public static partial class Extensions
    {
        public static string Description(this bool value)
        {
            return value ? "是" : "否";
        }

        public static string Description(this bool? value)
        {
            return value == null ? string.Empty : Description(value.Value);
        }
    }
}