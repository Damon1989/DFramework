using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DFramework.Infrastructure;

namespace DFramework.Domain
{
    public abstract class ValueObject<T>
    {
        public T Clone(object newValues = null)
        {
            var cloned = this.ToJson().ToJsonObject<T>();
            newValues?.GetType().GetProperties()
                .ForEach(p => { cloned.SetValueByKey(p.Name, p.GetValue(newValues)); });
            return cloned;
        }

        public static bool operator !=(ValueObject<T> a, ValueObject<T> b)
        {
            return NotEqualOperator(a, b);
        }

        public static bool operator ==(ValueObject<T> a, ValueObject<T> b)
        {
            return EqualOperator(a, b);
        }

        protected static bool EqualOperator(ValueObject<T> left, ValueObject<T> right)
        {
            if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null))
            {
                return false;
            }
            return ReferenceEquals(left, null) || left.Equals(right);
        }

        protected static bool NotEqualOperator(ValueObject<T> left, ValueObject<T> right)
        {
            return !EqualOperator(left, right);
        }

        protected virtual IEnumerable<object> GetAtomicValues()
        {
            return GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Select(p => p.GetValue(this, null));
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }
            var other = (ValueObject<T>)obj;
            var thisValues = GetAtomicValues().GetEnumerator();
            var otherValues = other.GetAtomicValues().GetEnumerator();
            while (thisValues.MoveNext() && otherValues.MoveNext())
            {
                if (ReferenceEquals(thisValues.Current, null) ^ ReferenceEquals(otherValues.Current, null))
                {
                    return false;
                }
                if (thisValues.Current != null && !thisValues.Current.Equals(otherValues.Current))
                {
                    return false;
                }
            }
            return !thisValues.MoveNext() && !otherValues.MoveNext();
        }

        public override int GetHashCode()
        {
            return GetAtomicValues()
                .Select(x => x != null ? x.GetHashCode() : 0)
                .Aggregate((x, y) => x ^ y);
        }
    }
}