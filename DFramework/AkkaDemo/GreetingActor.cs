using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaDemo
{
    public class GreetingActor : ReceiveActor
    {
        public GreetingActor()
        {
            Receive<GreetingMessage>(greet =>
            {
                Console.WriteLine($"Hello World {greet.Id}");
            });
        }
    }
}