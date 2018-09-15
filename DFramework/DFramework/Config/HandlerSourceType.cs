using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFramework.Config
{
    public enum HandlerSourceType
    {
        /// <summary>
        ///     Indicates that the configuration value represented by the Source
        ///     attribute is a type name.
        /// </summary>
        Type,

        /// <summary>
        ///     Indicates that the configuration value represented by the Source
        ///     attribute is an assembly name.
        /// </summary>
        Assembly
    }
}