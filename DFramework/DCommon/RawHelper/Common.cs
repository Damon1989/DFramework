using System;

namespace DCommon.RawHelper
{
    public class Common
    {
        public static Type GetType<T>()
        {
            return GetType(typeof(T));
        }

        public static Type GetType(Type type)
        {
            return Nullable.GetUnderlyingType(type) ?? type;
        }
    }
}