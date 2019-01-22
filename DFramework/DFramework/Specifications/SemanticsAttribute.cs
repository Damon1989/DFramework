using System;

namespace DFramework
{
    /// <summary>
    /// Represents that the decorated classes would have semantics meanings.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class SemanticsAttribute : Attribute
    {
        /// <summary>
        /// Constructs a new instance of the Semantics Attribute with parameters.
        /// </summary>
        /// <param name="type">The type of the semantics.</param>
        public SemanticsAttribute(Semantics type)
        {
            Type = type;
        }

        #region Public Properties
        /// <summary>
        ///  Gets or sets the type of the semantics.
        /// </summary>
        public Semantics Type { get; set; } 
        #endregion
    }
}