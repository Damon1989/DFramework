﻿namespace DCommon
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public static partial class Extensions
    {
        public static IQueryable<T> WhereIf<T>(
            this IQueryable<T> queryable,
            bool condition,
            Expression<Func<T, bool>> predicate)
        {
            return condition ? queryable.Where(predicate) : queryable;
        }
    }
}