using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DFramework.Infrastructure;

namespace DFramework.Domain
{
    //public abstract class ValueObject<T>
    //{
    //    public T Clone(object newValues = null)
    //    {
    //        var cloned = this.ToJson().ToJsonObject<T>();
    //        newValues?.GetType().GetProperties()
    //            .ForEach(p => { cloned.SetValueByKey(p.Name, p.GetValue(newValues)); });
    //        return cloned;
    //    }

    //    public static bool operator !=(ValueObject<T> a, ValueObject<T> b)
    //    {
    //        return NotEqualOperator(a, b);
    //    }

    //    public static bool operator ==(ValueObject<T> a, ValueObject<T> b)
    //    {
    //        return EqualOperator(a, b);
    //    }

    //    protected static bool EqualOperator(ValueObject<T> left, ValueObject<T> right)
    //    {
    //        if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null))
    //        {
    //            return false;
    //        }
    //        return ReferenceEquals(left, null) || left.Equals(right);
    //    }

    //    protected static bool NotEqualOperator(ValueObject<T> left, ValueObject<T> right)
    //    {
    //        return !EqualOperator(left, right);
    //    }

    //    protected virtual IEnumerable<object> GetAtomicValues()
    //    {
    //        return GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
    //            .Select(p => p.GetValue(this, null));
    //    }

    //    public override bool Equals(object obj)
    //    {
    //        if (obj == null || obj.GetType() != GetType())
    //        {
    //            return false;
    //        }
    //        var other = (ValueObject<T>)obj;
    //        var thisValues = GetAtomicValues().GetEnumerator();
    //        var otherValues = other.GetAtomicValues().GetEnumerator();
    //        while (thisValues.MoveNext() && otherValues.MoveNext())
    //        {
    //            if (ReferenceEquals(thisValues.Current, null) ^ ReferenceEquals(otherValues.Current, null))
    //            {
    //                return false;
    //            }
    //            if (thisValues.Current != null && !thisValues.Current.Equals(otherValues.Current))
    //            {
    //                return false;
    //            }
    //        }
    //        return !thisValues.MoveNext() && !otherValues.MoveNext();
    //    }

    //    public override int GetHashCode()
    //    {
    //        return GetAtomicValues()
    //            .Select(x => x != null ? x.GetHashCode() : 0)
    //            .Aggregate((x, y) => x ^ y);
    //    }
    //}

    public abstract class ValueObject : ICloneable
    {
        public object Clone()
        {
            return this.DeepClone();
        }

        public T Clone<T>()
        {
            return (T)Clone();
        }

        public object Clone(object newValues)
        {
            return Clone<object>(newValues);
        }

        public T Clone<T>(object newValues)
        {
            var cloned = Clone();
            newValues?.GetType().GetProperties().ForEach(p => { cloned.SetValueByKey(p.Name, p.GetValue(newValues)); });
            return (T)cloned;
        }

        public static bool operator !=(ValueObject a, ValueObject b)
        {
            return NotEqualOperator(a, b);
        }

        public static bool operator ==(ValueObject a, ValueObject b)
        {
            return EqualOperator(a, b);
        }

        /// <summary>
        ///     Helper function for implementing overloaded equality operator.
        /// </summary>
        /// <param name="left">Left-hand side object.</param>
        /// <param name="right">Right-hand side object.</param>
        /// <returns></returns>
        protected static bool EqualOperator(ValueObject left, ValueObject right)
        {
            if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null))
                return false;
            return ReferenceEquals(left, null) || left.Equals(right);
        }

        /// <summary>
        ///     Helper function for implementing overloaded inequality operator.
        /// </summary>
        /// <param name="left">Left-hand side object.</param>
        /// <param name="right">Right-hand side object.</param>
        /// <returns></returns>
        protected static bool NotEqualOperator(ValueObject left, ValueObject right)
        {
            return !EqualOperator(left, right);
        }

        /// <summary>
        ///     To be overridden in inheriting clesses for providing a collection of atomic values of
        ///     this Value Object.
        /// </summary>
        /// <returns>Collection of atomic values.</returns>
        protected virtual IEnumerable<object> GetAtomicValues()
        {
            return GetType().GetProperties().Select(p => p.GetValue(this, null));
        }

        /// <summary>
        ///     Compares two Value Objects according to atomic values returned by <see cref="GetAtomicValues" />.
        /// </summary>
        /// <param name="obj">Object to compare to.</param>
        /// <returns>True if objects are considered equal.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
                return false;
            var other = (ValueObject)obj;
            var thisValues = GetAtomicValues().GetEnumerator();
            var otherValues = other.GetAtomicValues().GetEnumerator();
            while (thisValues.MoveNext() && otherValues.MoveNext())
            {
                if (ReferenceEquals(thisValues.Current, null) ^ ReferenceEquals(otherValues.Current, null))
                    return false;
                if (thisValues.Current != null && !thisValues.Current.Equals(otherValues.Current))
                    return false;
            }
            return !thisValues.MoveNext() && !otherValues.MoveNext();
        }

        /// <summary>
        ///     Returns hashcode value calculated according to a collection of atomic values
        ///     returned by <see cref="GetAtomicValues" />.
        /// </summary>
        /// <returns>Hashcode value.</returns>
        public override int GetHashCode()
        {
            return GetAtomicValues()
                .Select(x => x != null ? x.GetHashCode() : 0)
                .Aggregate((x, y) => x ^ y);
        }
    }
}