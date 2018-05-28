using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFramework
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class SemanticsAttribute : Attribute
    {
        public SemanticsAttribute(Semantics type)
        {
            Type = type;
        }

        public Semantics Type { get; set; }
    }
}