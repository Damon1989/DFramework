using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace DFramework.ServiceBusDemo
{
    internal class Program
    {
        private static string ServerFQDN;
        private static int HttpPort = 9355;
        private static int TcpPort = 9354;
        private static string ServiceNamespace = "ServiceBusDefaultNamespace";

        private static void Main(string[] args)
        {
            ServerFQDN = System.Net.Dns.GetHostEntry(string.Empty).HostName;

            ServiceBusConnectionStringBuilder connBuilder =
                new ServiceBusConnectionStringBuilder
                {
                    ManagementPort = HttpPort,
                    RuntimePort = TcpPort
                };
            connBuilder.Endpoints.Add(new UriBuilder { Scheme = "sb", Host = ServerFQDN, Path = ServiceNamespace }.Uri);
            connBuilder.StsEndpoints.Add(new UriBuilder
            {
                Scheme = "https",
                Host = ServerFQDN,
                Port = HttpPort,
                Path = ServiceNamespace
            }.Uri);

            MessagingFactory messageFactory = MessagingFactory.CreateFromConnectionString(connBuilder.ToString());
            NamespaceManager namespaceManager = NamespaceManager.CreateFromConnectionString(connBuilder.ToString());

            if (namespaceManager == null)
            {
                Console.WriteLine("\nUnepected Error");
                return;
            }

            string queueName = "ServiceBusQueueSample";
            if (namespaceManager.QueueExists(queueName))
            {
                namespaceManager.DeleteQueue(queueName);
            }

            namespaceManager.CreateQueue(queueName);

            QueueClient myQueueClient = messageFactory.CreateQueueClient(queueName);

            try
            {
                BrokeredMessage sendMessage = new BrokeredMessage("Hello World!");
                myQueueClient.Send(sendMessage);

                //Receive the message from the queue
                BrokeredMessage receivedMessage = myQueueClient.Receive(TimeSpan.FromSeconds(5));

                if (receivedMessage != null)
                {
                    Console.WriteLine(string.Format($"Message received:{receivedMessage.GetBody<string>()}"));
                    receivedMessage.Complete();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unexpected exception {e}");
                throw;
            }
            finally
            {
                if (messageFactory != null)
                {
                    messageFactory.Close();
                }
                Console.WriteLine("Press ENTER to clean up and exit.");
                Console.ReadLine();
            }
        }
    }
}