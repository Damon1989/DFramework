﻿using System;
using System.Collections.Generic;

namespace Abp.Extensions
{
    /// <summary>
    ///  Extensions methods for Collections.
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        ///  Checks whatever given collection object is null or has no item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this ICollection<T> source)
        {
            return source == null || source.Count <= 0;
        }

        /// <summary>
        ///  Adds an item to the collection if it's not already in the collection.
        /// </summary>
        /// <typeparam name="T">Type of the items in the collection</typeparam>
        /// <param name="source">Collection</param>
        /// <param name="item">Item to check and add</param>
        /// <returns>Returns True if added,returns False if not </returns>
        public static bool AddIfNotContains<T>(this ICollection<T> source, T item)
        {
            if (source==null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (source.Contains(item))
            {
                return false;
            }

            source.Add(item);
            return true;
        }
    }
}