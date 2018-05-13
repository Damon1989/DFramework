using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DFramework.IoC;

namespace DFramework.Ioc
{
    public interface IContainer : IDisposable
    {
        /// <summary>
        /// The parent of this container.
        /// </summary>
        IContainer Parent { get; }

        /// <summary>
        /// Create a child container.
        /// </summary>
        /// <returns>The new child container.</returns>
        /// A child container shares the parent's configuration,but can be configured with
        /// different settings or lifetime.
        IContainer CreateChildContainer();

        IContainer RegisterType(Type from, Type to, string name, Lifetime lifetime, params Injection[] injections);

        IContainer RegisterType(Type from, Type to, Lifetime lifetime, params Injection[] injections);

        IContainer RegisterType(Type from, Type to, string name = null, params Injection[] injections);

        IContainer RegisterType<TFrom, TTo>(string name, Lifetime lifetime, params Injection[] injections)
            where TTo : TFrom;

        IContainer RegisterType<TFrom, TTo>(string name, params Injection[] injections) where TTo : TFrom;

        IContainer RegisterType<TFrom, TTo>(params Injection[] injections) where TTo : TFrom;

        IContainer RegisterType<TFrom, TTo>(Lifetime lifetime, params Injection[] injections) where TTo : TFrom;

        IContainer RegisterInstance(Type t, string name, object instance, Lifetime lifetime = Lifetime.Singleton);

        IContainer RegisterInstance(Type t, object instance, Lifetime lifetime = Lifetime.Singleton);

        IContainer RegisterInstance<TInterface>(TInterface instance, Lifetime lifetime = Lifetime.Singleton)
            where TInterface : class;

        IContainer RegisterInstance<TInterface>(string name, TInterface instance,
            Lifetime lifetime = Lifetime.Singleton)
            where TInterface : class;

        object Resolve(Type t, string name, params Parameter[] parameters);

        object Resolve(Type t, params Parameter[] parameters);

        T Resolve<T>(params Parameter[] overrides);

        T Resolve<T>(string name, params Parameter[] overrides);

        IEnumerable<object> ResolveAll(Type t, params Parameter[] parameters);

        IEnumerable<T> ResolveAll<T>(params Parameter[] parameters);
    }
}