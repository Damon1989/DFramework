using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var port = 2000;
            var host = "127.0.0.1";

            var ip = IPAddress.Parse(host);
            var ipe = new IPEndPoint(ip, port);//服务器地址

            var c = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//创建一个Socket
            c.Connect(ipe);//连接到服务器
            var sendStr = "";
            while (true)
            {
                sendStr = Console.ReadLine();
                var bs = Encoding.ASCII.GetBytes(sendStr);
                Console.WriteLine("发送信息");

                c.Send(bs, bs.Length, 0);//向服务器发送消息
                var rcvStr = "";
                var rcvBytes = new byte[1024];
                int bytes = c.Receive(rcvBytes, rcvBytes.Length, 0);//从服务器接受返回信息
                rcvStr += Encoding.ASCII.GetString(rcvBytes, 0, bytes);
                Console.WriteLine($"client get message:{rcvStr}");//显示服务器返回信息
            }
            c.Close();

            Console.WriteLine("Press Enter to Exit");
            Console.ReadKey();
        }
    }
}