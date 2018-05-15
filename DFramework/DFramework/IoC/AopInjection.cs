using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFramework.IoC
{
    public class InterfaceInterceptorInjection : Injection
    {
    }

    public class TransparentProxyInterceptorInjection : Injection
    {
    }

    public class VirtualMethodInterceptorInjection : Injection
    {
    }

    public class InterceptionBehaviorInjection : Injection
    {
        public Type BehaviorType { get; }

        public InterceptionBehaviorInjection(Type behaviorType)
        {
            BehaviorType = behaviorType;
        }
    }

    public class InterceptionBehaviorInjection<TInterceptionBehavior> : InterceptionBehaviorInjection
    {
        public InterceptionBehaviorInjection() : base(typeof(TInterceptionBehavior))
        {
        }
    }
}