namespace DCommon
{
    using System.Linq;
    using System.Reflection;

    public static class TConverterHelper
    {
        public static object GetPropertyValue(PropertyInfo[] properties, object model, string propertyName)
        {
            try
            {
                if (model == null) return null;

                var info = properties.FirstOrDefault(p => p.Name == propertyName && p.CanWrite && p.CanRead);
                return info == null ? null : info.GetValue(model, null);
            }
            catch
            {
                return null;
            }
        }

        public static T GetTFromT1<T>(object t1, object defaultValue = null)
            where T : new()
        {
            var t = new T();
            var t1Properties = t1.GetType().GetProperties();
            foreach (var info in t.GetType().GetProperties().Where(p => p.CanRead && p.CanWrite))
                try
                {
                    var value = GetPropertyValue(t1Properties, t1, info.Name);
                    if (value != null) info.SetValue(t, value, null);
                    else if (defaultValue != null) info.SetValue(t, defaultValue, null);
                }
                catch
                {
                    // ignored
                }

            return t;
        }

        public static T SetTFromT1<T>(object t1, T t)
            where T : new()
        {
            if (t == null) t = new T();

            var t1Properties = t1.GetType().GetProperties();
            foreach (var info in t.GetType().GetProperties().Where(p => p.CanWrite && p.CanRead))
                try
                {
                    var value = GetPropertyValue(t1Properties, t1, info.Name);
                    if (value != null) info.SetValue(t, value, null);
                }
                catch
                {
                    // ignored
                }

            return t;
        }
    }
}