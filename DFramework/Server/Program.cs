using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            int port = 2000;
            var host = "127.0.0.1";
            var ip = IPAddress.Parse(host);
            var ipe = new IPEndPoint(ip, port);

            var s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//创建一个socket类
            s.Bind(ipe);//绑定
            s.Listen(10);//开始监听
            Console.WriteLine("等待客户端连接");
            var temp = s.Accept();//等待客户端连接
            Console.WriteLine("建立连接");
            while (true)
            {
                var rcvStr = "";
                var rcvBytes = new byte[1024];
                int bytes = temp.Receive(rcvBytes, rcvBytes.Length, 0);//从客户端接受信息
                rcvStr += Encoding.ASCII.GetString(rcvBytes, 0, bytes);

                Console.WriteLine($"Server get message:{rcvStr}");

                var sendStr = "ok! Client send message successful!";
                var bs = Encoding.ASCII.GetBytes(sendStr);
                temp.Send(bs, bs.Length, 0);//向客户端发送数据
            }
            temp.Close();
            s.Close();

            Console.ReadKey();
        }
    }
}