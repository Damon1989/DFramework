using System;
using System.Collections.Generic;
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
        ///
        /// A child container shares the parent's configuration,but can be configured with
        /// different settings or lifetime.
        IContainer CreateChildContainer();

        /// <summary>
        ///  Register a type mapping with the container,where the created instances will
        ///  use the given LifttimeManager
        /// </summary>
        /// <param name="from">System.Type that will be requested.</param>
        /// <param name="to">System.Type that will actually be returned.</param>
        /// <param name="name">Name to use for registration,null if a default registration.</param>
        /// <param name="lifetime"></param>
        /// <param name="injections"></param>
        /// <returns></returns>
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