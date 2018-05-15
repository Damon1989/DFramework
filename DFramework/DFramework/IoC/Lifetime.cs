using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFramework.IoC
{
    public enum Lifetime
    {
        /// <summary>
        /// Represents a component is a transient component.
        /// </summary>
        Transient,

        /// <summary>
        /// Represents a component is a singleton component.
        /// </summary>
        Singleton,

        PerRequest,
        Hierarchical
    }
}