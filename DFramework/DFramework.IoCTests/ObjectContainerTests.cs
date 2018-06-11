using System;
using DFramework.Config;
using DFramework.IoC;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;

namespace DFramework.IoCTests
{
    [TestClass]
    public class ObjectContainerTests
    {
        [TestMethod]
        public void ResolveAutofac()
        {
            Configuration.Instance
                         .UseAutofacContainer()
                         .RegisterAssemblyTypes(Assembly.GetExecutingAssembly());

            var container = IoCFactory.Instance.CurrentContainer;
            //container.RegisterType<MyClass, MyClass>();
            container.RegisterType(typeof(IMyClass<>), typeof(MyClass<>));

            for (int i = 0; i < 10000; i++)
            {
                using (var scope = IoCFactory.Instance.CurrentContainer.CreateChildContainer())
                {
                    try
                    {
                        var myClass = scope.Resolve<IMyClass<YourClass>>();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                    //var myClass = scope.Resolve<MyClass>();
                    //Assert.AreEqual(myClass.Container, myClass.YourClass.Container);
                }
            }
        }
    }
}