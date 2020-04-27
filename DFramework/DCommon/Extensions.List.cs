namespace DCommon
{
    using System;
    using System.Collections.Generic;

    public static partial class Extensions
    {
        public static void Add<T>(this List<T> list, bool condition, T item)
        {
            if (list == null) throw new ArgumentException(nameof(list));

            if (condition) list.Add(item);
        }
    }
}