using System;
using System.Linq.Expressions;
using System.Reflection;

namespace DFramework.Infrastructure
{
    public static class Utility
    {
        private const string k_base36_digits = "0123456789abcdefghijklmnopqrstuvwxyz";
        private static readonly uint[] _lookup32 = CreateLookup32();

        public static T GetValueByKey<T>(this object obj, string name)
        {
            var retValue = default(T);
            object objValue = null;
            try
            {
                var property = obj.GetType()
                    .GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (property != null) objValue = FastInvoke.GetMethodInvoker(property.GetGetMethod(true))(obj, null);

                if (objValue != null) retValue = (T) objValue;
            }
            catch (Exception)
            {
                retValue = default(T);
            }

            return retValue;
        }

        public static void SetValueByKey(this object obj, string name, object value)
        {
            var property = obj.GetType()
                .GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (property != null) FastInvoke.GetMethodInvoker(property.GetSetMethod(true))(obj, new[] {value});
        }


        public static string ToBase36string(this byte[] bytes, EndianFormat bytesEndian = EndianFormat.Little,
            bool includeProceedingZeros = true)
        {
            var base36_no_zeros = new RadixEncoding(k_base36_digits, bytesEndian, includeProceedingZeros);
            return base36_no_zeros.Encode(bytes);
        }

        public static string ToHexString(this byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException("bytes");
            var lookup32 = _lookup32;
            var result = new char[bytes.Length * 2];
            for (var i = 0; i < bytes.Length; i++)
            {
                var val = lookup32[bytes[i]];
                result[2 * i] = (char) val;
                result[2 * i + 1] = (char) (val >> 16);
            }

            return new string(result);
        }

        private static uint[] CreateLookup32()
        {
            var result = new uint[256];
            for (var i = 0; i < 256; i++)
            {
                var s = i.ToString("X2");
                result[i] = s[0] + ((uint) s[1] << 16);
            }

            return result;
        }

        public static T DeepClone<T>(this T obj)
        {
            return obj.ToJson().ToJsonObject<T>();
            //object retval;
            //using (var ms = new MemoryStream())
            //{
            //    var bf = new BinaryFormatter();
            //    //序列化成流
            //    bf.Serialize(ms, obj);
            //    ms.Seek(0, SeekOrigin.Begin);
            //    //反序列化成对象
            //    retval = bf.Deserialize(ms);
            //    ms.Close();
            //}
            //return (T) retval;
        }

        public static object GetValueByKey(this object obj, string name)
        {
            object objValue = null;

            {
                var property = obj.GetType()
                    .GetProperty(name,
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (property != null)
                    objValue = FastInvoke.GetMethodInvoker(property.GetGetMethod(true))(obj, null);

                return objValue;
            }
        }

        #region Lambda

        public static LambdaExpression GetLambdaExpression(Type type, string propertyName)
        {
            var param = Expression.Parameter(type);
            Expression body = param;
            foreach (var member in propertyName.Split(',')) body = Expression.PropertyOrField(body, member);

            return Expression.Lambda(body, param);
        }

        public static LambdaExpression GetLambdaExpression(Type type, Expression expression)
        {
            var propertyName = expression.ToString();
            var index = propertyName.IndexOf('.');
            propertyName = propertyName.Substring(index + 1);
            return GetLambdaExpression(type, propertyName);
        }

        #endregion Lambda
    }
}