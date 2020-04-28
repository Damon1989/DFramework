using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return value == null ? "" : Description(value.Value);
        }
    }
}
