using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BuilderSample
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            Builder builder = new ConcreteBuilder1(); //可通过配置文件实现
            Director director = new Director(builder);
            Product product = director.Construct();
            Console.WriteLine(product.PartA);
            Console.WriteLine(product.PartB);
            Console.WriteLine(product.PartC);
            Console.Read();
            */

            ActorBuilder ab;

            string builderType = ConfigurationManager.AppSettings["builder"];
            ab = (ActorBuilder) Assembly.Load("BuilderSample").CreateInstance(builderType);

            ActorController ac=new ActorController();
            Actor actor = ac.Construct(ab);

            Console.WriteLine("{0}的外观：", actor.Type);
            Console.WriteLine("性别：{0}", actor.Sex);
            Console.WriteLine("面容：{0}", actor.Face);
            Console.WriteLine("服装：{0}", actor.Costume);
            Console.WriteLine("发型：{0}", actor.HairStyle);
            Console.Read();
        }
    }
}
