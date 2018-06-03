using System;

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