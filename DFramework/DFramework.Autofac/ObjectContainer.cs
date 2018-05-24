using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using DFramework.Infrastructure;
using DFramework.IoC;
using IContainer = DFramework.Ioc.IContainer;
using RegistrationExtensions = Autofac.Extras.DynamicProxy.RegistrationExtensions;

namespace DFramework.Autofac
{
    public static class ObjectContainerExtension
    {
        public static ILifetimeScope GetAutofacContainer(this IContainer container)
        {
            return (container as ObjectContainer)?._container;
        }
    }

    public class ObjectContainer : IContainer
    {
        internal ILifetimeScope _container;

        private bool _disposed;

        public ObjectContainer(ILifetimeScope container, IContainer parent = null)
        {
            _container = container;
            Parent = parent;
            //RegisterInstance
        }

        public object ContainerInstance => _container;

        public IContainer Parent { get; set; }

        public IContainer CreateChildContainer()
        {
            var scope = _container.BeginLifetimeScope();
            var container = new ObjectContainer(scope, this);
            return container;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                _container.Dispose();
            }
        }

        public IContainer RegisterInstance(Type t, object instance, Lifetime lifetime = Lifetime.Singleton)
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance(instance)
                .As(t)
                .InstanceLifetime(lifetime);
            builder.Update(_container.ComponentRegistry);
            return this;
        }

        public IContainer RegisterInstance(Type t, string name, object instance, Lifetime lifetime = Lifetime.Singleton)
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance(instance)
                .Named(name, t)
                .InstanceLifetime(lifetime);
            builder.Update(_container.ComponentRegistry);
            return this;
        }

        public IContainer RegisterInstance<TInterface>(TInterface instance, Lifetime lifetime = Lifetime.Singleton)
            where TInterface : class
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance(instance)
                .InstanceLifetime(lifetime);
            builder.Update(_container.ComponentRegistry);
            return this;
        }

        public IContainer RegisterInstance<TInterface>(string name, TInterface instance,
            Lifetime lifetime = Lifetime.Singleton)
            where TInterface : class
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance(instance)
                .Named<TInterface>(name)
                .InstanceLifetime(lifetime);
            builder.Update(_container.ComponentRegistry);
            return this;
        }

        public IContainer RegisterType(Type from, Type to, string name = null, params Injection[] injections)
        {
            var injectionMembers = GetInjectionParameters(injections);
            var builder = new ContainerBuilder();
            dynamic registrationBuilder;
            if (string.IsNullOrEmpty(name))
            {
                if (to.IsGenericType)
                {
                    registrationBuilder = builder.RegisterGeneric(to).As(from);
                }
                else
                {
                    registrationBuilder = builder.RegisterType(to).As(from);
                }
            }
            else
            {
                if (to.IsGenericType)
                {
                    registrationBuilder = builder.RegisterGeneric(to)
                                                .Named(name, from)
                                                .WithParameters(injectionMembers);
                }
                else
                {
                    registrationBuilder = builder.RegisterType(to)
                        .Named(name, from)
                        .WithParameters(injectionMembers);
                }
            }
            RegisterInterceptor(registrationBuilder, injections);
            return this;
        }

        public IContainer RegisterType(Type from, Type to, Lifetime lifetime, params Injection[] injections)
        {
            var injectionMembers = GetInjectionParameters(injections);
            var builder = new ContainerBuilder();
            dynamic registrationBuilder;
            if (to.IsGenericType)
            {
                registrationBuilder = builder.RegisterGeneric(to)
                    .As(from)
                    .WithParameters(injectionMembers)
                    .InstanceLifetime(lifetime);
            }
            else
            {
                registrationBuilder = builder.RegisterType(to)
                    .As(from)
                    .WithParameters(injectionMembers)
                    .InstanceLifetime(lifetime);
            }

            RegisterInterceptor(registrationBuilder, injections);

            builder.Update(_container.ComponentRegistry);
            return this;
        }

        public IContainer RegisterType(Type from, Type to, string name, Lifetime lifetime,
            params Injection[] injections)
        {
            var injectionMembers = GetInjectionParameters(injections);
            var builder = new ContainerBuilder();
            dynamic registrationBuilder;
            if (to.IsGenericType)
                registrationBuilder = builder.RegisterGeneric(to)
                    .Named(name, from)
                    .InstanceLifetime(lifetime)
                    .WithParameters(injectionMembers);
            else
                registrationBuilder = builder.RegisterType(to)
                    .Named(name, from)
                    .InstanceLifetime(lifetime)
                    .WithParameters(injectionMembers);

            RegisterInterceptor(registrationBuilder, injections);

            builder.Update(_container.ComponentRegistry);
            return this;
        }

        public IContainer RegisterType<TFrom, TTo>(Lifetime lifetime, params Injection[] injections) where TTo : TFrom
        {
            var injectionMembers = GetInjectionParameters(injections);
            var builder = new ContainerBuilder();
            dynamic registrationBuilder;
            if (typeof(TTo).IsGenericType)
                registrationBuilder = builder.RegisterGeneric(typeof(TTo))
                    .As(typeof(TFrom))
                    .InstanceLifetime(lifetime)
                    .WithParameters(injectionMembers);
            else
                registrationBuilder = builder.RegisterType(typeof(TTo))
                    .As(typeof(TFrom))
                    .InstanceLifetime(lifetime)
                    .WithParameters(injectionMembers);

            RegisterInterceptor(registrationBuilder, injections);

            builder.Update(_container.ComponentRegistry);
            return this;
        }

        public IContainer RegisterType<TFrom, TTo>(params Injection[] injections) where TTo : TFrom
        {
            var injectionMembers = GetInjectionParameters(injections);
            var builder = new ContainerBuilder();
            dynamic registrationBuilder;
            if (typeof(TTo).IsGenericType)
                registrationBuilder = builder.RegisterGeneric(typeof(TTo))
                    .As(typeof(TFrom))
                    .WithParameters(injectionMembers);
            else
                registrationBuilder = builder.RegisterType(typeof(TTo))
                    .As(typeof(TFrom))
                    .WithParameters(injectionMembers);

            RegisterInterceptor(registrationBuilder, injections);

            builder.Update(_container.ComponentRegistry);
            return this;
        }

        public IContainer RegisterType<TFrom, TTo>(string name, params Injection[] injections) where TTo : TFrom
        {
            var injectionMembers = GetInjectionParameters(injections);
            var builder = new ContainerBuilder();
            dynamic registrationBuilder;
            if (typeof(TTo).IsGenericType)
                registrationBuilder = builder.RegisterGeneric(typeof(TTo))
                    .Named(name, typeof(TFrom))
                    .WithParameters(injectionMembers);
            else
                registrationBuilder = builder.RegisterType(typeof(TTo))
                    .Named(name, typeof(TFrom))
                    .WithParameters(injectionMembers);

            RegisterInterceptor(registrationBuilder, injections);

            builder.Update(_container.ComponentRegistry);
            return this;
        }

        public IContainer RegisterType<TFrom, TTo>(string name, Lifetime lifetime, params Injection[] injections)
            where TTo : TFrom
        {
            var injectionMembers = GetInjectionParameters(injections);
            var builder = new ContainerBuilder();
            dynamic registrationBuilder;
            if (typeof(TTo).IsGenericType)
                registrationBuilder = builder.RegisterGeneric(typeof(TTo))
                    .Named(name, typeof(TFrom))
                    .InstanceLifetime(lifetime)
                    .WithParameters(injectionMembers);
            else
                registrationBuilder = builder.RegisterType(typeof(TTo))
                    .Named(name, typeof(TFrom))
                    .InstanceLifetime(lifetime)
                    .WithParameters(injectionMembers);

            RegisterInterceptor(registrationBuilder, injections);

            builder.Update(_container.ComponentRegistry);
            return this;
        }

        public object Resolve(Type t, params Parameter[] parameters)
        {
            return _container.Resolve(t, GetResolvedParameters(parameters));
        }

        public object Resolve(Type t, string name, params Parameter[] parameters)
        {
            return _container.ResolveNamed(name, t, GetResolvedParameters(parameters));
        }

        public T Resolve<T>(params Parameter[] parameters)
        {
            return _container.Resolve<T>(GetResolvedParameters(parameters));
        }

        public T Resolve<T>(string name, params Parameter[] parameters)
        {
            return _container.ResolveNamed<T>(name, GetResolvedParameters(parameters));
        }

        public IEnumerable<object> ResolveAll(Type type, params Parameter[] parameters)
        {
            var typeToResolve = typeof(IEnumerable<>).MakeGenericType(type);
            return _container.Resolve(typeToResolve, GetResolvedParameters(parameters)) as IEnumerable<object>;
        }

        public IEnumerable<T> ResolveAll<T>(params Parameter[] parameters)
        {
            return _container.Resolve<IEnumerable<T>>(GetResolvedParameters(parameters));
        }

        private IEnumerable<global::Autofac.Core.Parameter> GetResolvedParameters(Parameter[] resolvedParameters)
        {
            var parameters = new List<global::Autofac.Core.Parameter>();
            //parameters.Add(new NamedParameter("container",  this));
            parameters.AddRange(resolvedParameters.Select(p => new NamedParameter(p.Name, p.Value)));
            return parameters;
        }

        private void RegisterInterceptor(dynamic registrationBuilder, Injection[] injections)
        {
            injections.ForEach(injection =>
            {
                if (injection is InterfaceInterceptorInjection)
                {
                    RegistrationExtensions.EnableInterfaceInterceptors(registrationBuilder);
                }
                else if (injection is VirtualMethodInterceptorInjection)
                {
                    RegistrationExtensions.EnableClassInterceptors(registrationBuilder);
                }
                else if (injection is TransparentProxyInterceptorInjection)
                {
                    //RegistrationExtensions.InterceptTransparentProxy(registrationBuilder)
                    //    .UseWcfSafeRelease();
                }
                else if (injection is InterceptionBehaviorInjection)
                {
                    var interceptorType = ((InterceptionBehaviorInjection)injection).BehaviorType;
                    RegistrationExtensions.InterceptedBy(registrationBuilder, interceptorType);
                }
            });
        }

        private IEnumerable<global::Autofac.Core.Parameter> GetInjectionParameters(Injection[] injections)
        {
            var injectionMembers = new List<global::Autofac.Core.Parameter>();
            injections.ForEach(injection =>
            {
                if (injection is ConstructInjection)
                {
                    var constructInjection = injection as ConstructInjection;
                    injectionMembers.AddRange(constructInjection.Parameters
                        .Select(p => new NamedParameter(p.ParameterName, p.ParameterValue)));
                }
                else if (injection is ParameterInjection)
                {
                    var propertyInjection = injection as ParameterInjection;
                    injectionMembers.Add(new NamedParameter(propertyInjection.ParameterName
                        , propertyInjection.ParameterValue));
                }
            });
            return injectionMembers;
        }
    }
}