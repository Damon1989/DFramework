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
}