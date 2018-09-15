using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Configuration;
using Event.PlayWithAkka.Common;
using Console = System.Console;

namespace Event.PlayWithAkka.Client
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var config = ConfigurationFactory.ParseString(@"
                akka{
                    actor{
                        provider=""Akka.Remote.RemoteActorRefProvider,Akka.Remote""
                    }
                    remote{
                        helios.tcp{
                            transport-class=""Akka.Remote.Transport.Helios.HeliosTcpTransport,Akka.Remote""
                            applied-adapters=[]
                            transport-protocol=tcp
                            port=0
                            hostname=localhost
                        }
                    }
                }
            ");

            using (var system = ActorSystem.Create("MyClient", config))
            {
                var greeting = system.ActorSelection("akka.tcp://MyServer@localhost:8081/user/Greeting");

                while (true)
                {
                    var input = Console.ReadLine();
                    if (input.Equals("sayHello"))
                    {
                        greeting.Tell(new GreetingMessage() { Id = DateTime.Now.ToString() });
                    }
                }
            }
        }
    }
}