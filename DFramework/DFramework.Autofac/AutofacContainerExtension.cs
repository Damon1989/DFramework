using Autofac;
using Autofac.Builder;
using DFramework.IoC;

namespace DFramework.Autofac
{
    internal static class AutofacContainerExtension
    {
        public static IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle>
            InstanceLifetime<TLimit, TActivatorData, TRegistrationStyle>(
                this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> builder,
                Lifetime lifetime)
        {
            switch (lifetime)
            {
                case Lifetime.PerRequest:
                    builder.InstancePerRequest();
                    break;

                case Lifetime.Singleton:
                    builder.SingleInstance();
                    break;

                case Lifetime.Hierarchical:
                    builder.InstancePerLifetimeScope();
                    break;

                case Lifetime.Transient:
                    builder.InstancePerDependency();
                    break;
            }
            return builder;
        }
    }
}