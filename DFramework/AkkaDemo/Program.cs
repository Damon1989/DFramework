using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaDemo
{
    //https://www.cnblogs.com/richieyang/p/4945905.html
    internal class Program
    {
        private static void Main(string[] args)
        {
            //Create a new actor system(a container for your actors)
            var system = ActorSystem.Create("MySystem");
            ActorSystem.Create("MySystem");
            //Create your actor and get a reference to it.
            var greeter = system.ActorOf<GreetingActor>("greeter");

            var random = new Random();
            var i = 100;
            while (i > 0)
            {
                //Send a message to the actor
                greeter.Tell(new GreetingMessage() { Id = i.ToString() });
                i--;
            }

            Console.ReadLine();
        }
    }
}