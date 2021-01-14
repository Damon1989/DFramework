using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AbstractFactorySample
{
    class Program
    {
        static void Main(string[] args)
        {
            //使用抽象层定义

            //读取配置文件
            string factoryType = ConfigurationManager.AppSettings["factory"];

            //反射生成对象
            var factory = (ISkinFactory)Assembly.Load("AbstractFactorySample").CreateInstance(factoryType);

            var bt = factory.CreateButton();
            var tf = factory.CreateTextField();
            var cb = factory.CreateComboBox();
            bt.Display();
            tf.Display();
            cb.Display();

            Console.Read();
        }
    }
}
