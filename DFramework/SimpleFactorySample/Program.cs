using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleFactorySample
{
    class Program
    {

        static void Main(string[] args)
        {
            /*
             * 简单工厂模式（Simple Factory Pattern）:定义一个工厂类，它可以根据参数的不同返回不同类的实例，被创建的实例通常都具有共同的父类

            短消息发送平台   根据配置文件的参数  利用反射 动态创建类的实例  调用不同供应商短消息平台  (其实是 工厂方法模式)
            Echart 图表

            增加新的对象 破坏 开闭原则


            1、优点
            简单工厂模式解决了客户端直接依赖于具体对象的问题，客户端可以消除直接创建对象的责任
            简单工厂模式也起到了代码复用的作用
            
            2、缺点
            工厂类集中了所有产品创建逻辑，一旦不能正常工作，整个系统都会受到影响
            系统扩展困难，一旦添加新产品就不得不修改工厂逻辑，这样就会造成工厂逻辑过于负责
             */



            /*
            var product = Factory.GetProduct("A");
            product.MethodSame();
            product.MethodDiff();
             */
            //读取配置文件
            string chartStr = ConfigurationManager.AppSettings["chartType"];
            var chart = ChartFactory.GetChart(chartStr); //通过静态工厂方法创建产品
            chart.Display();

            Console.Read();
        }
    }
}
