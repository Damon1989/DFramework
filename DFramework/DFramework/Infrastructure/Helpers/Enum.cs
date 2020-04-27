using DFramework.Infrastructure.Extensions;
using System;

namespace DFramework.Infrastructure.Helpers
{
    public static partial class Enum
    {
        public static TEnum Parse<TEnum>(object member)
        {
            string value = member.SafeString();
            if (string.IsNullOrWhiteSpace(value))
            {
                if (typeof(TEnum).IsGenericType)
                {
                    return default(TEnum);
                }

                throw new ArgumentNullException(nameof(member));
            }

            return (TEnum)System.Enum.Parse(Common.GetType<TEnum>(), value, true);
        }
    }
}