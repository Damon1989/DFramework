using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleFactorySample
{
    abstract class Product
    {
        //所有产品类的公共业务方法
        public void MethodSame()
        {
            //公共方法的实现
        }

        //声明抽象业务方法
        public abstract void MethodDiff();
    }

    class ConcreteProductA:Product
    {
        //实现业务方法
        public override void MethodDiff()
        {
            //业务方法的实现
        }
    }

    class ConcreteProductB : Product
    {
        //实现业务方法
        public override void MethodDiff()
        {
            //业务方法的实现
        }
    }

    class Factory
    {
        //静态工厂方法
        public static Product GetProduct(string arg)
        {
            Product product = null;
            if (arg.Equals("A"))
            {
                product=new ConcreteProductA();
            }else if (arg.Equals("B"))
            {
                product=new ConcreteProductB();
            }

            return product;
        }
    }

    public class SimpleFactory
    {
        
    }
}
