using System.Security.Cryptography.X509Certificates;
using System.Xml;
using DFramework.IoC;

namespace DFramework.IoCTests
{
    public class YourClass
    {
        public YourClass(IContainer container)
        {
            Container = container;
        }

        public IContainer Container { get; set; }
    }

    public class MyClass
    {
        public MyClass(YourClass yourClass, IContainer container)
        {
            YourClass = yourClass;
            Container = container;
        }

        public YourClass YourClass { get; set; }
        public IContainer Container { get; set; }
    }

    public class MyClass<T> : IMyClass<T>
    {
        public MyClass(T yourClass, IContainer container)
        {
            YourClass = yourClass;
            Container = container;
        }

        public T YourClass { get; set; }
        public IContainer Container { get; set; }
    }

    public interface IMyClass<T>
    {
        T YourClass { get; set; }
        IContainer Container { get; set; }
    }
}