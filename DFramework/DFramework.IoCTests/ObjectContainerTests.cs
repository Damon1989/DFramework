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

            for (int i = 0; i < 10000; i++)
            {
                using (var scope = IoCFactory.Instance.CurrentContainer.CreateChildContainer())
                {
                    var myClass = scope.Resolve<MyClass>();
                    Assert.AreEqual(myClass.Container, myClass.YourClass.Container);
                }
            }
        }
    }
}