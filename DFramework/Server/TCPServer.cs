using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    //https://blog.csdn.net/subin_iecas/article/details/80289915
    public class TCPServer
    {
        private byte[] result = new byte[1024];

        //最大的监听数量
        private int MaxClientCount;

        //IP地址
        private string IP;

        //端口号
        private int Port;

        //客户端列表
        private List<Socket> ClientSockets;

        //IP终端
        private IPEndPoint IpEndPoint;

        //服务端Socket
        private Socket ServerSocket;

        //当前客户端Socket;
        private Socket ClientSocket;

        public TCPServer(int port, int count)
        {
            this.IP = IPAddress.Any.ToString();
            this.Port = port;
            this.MaxClientCount = count;
            this.ClientSockets = new List<Socket>();

            //初始化IP终端
            this.IpEndPoint = new IPEndPoint(IPAddress.Any, port);

            //初始化服务端Socket
            this.ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //端口绑定
            this.ServerSocket.Bind(this.IpEndPoint);

            //设置监听数目
            this.ServerSocket.Listen(MaxClientCount);
        }

        //定义一个Start方法将构造函数中的方法分离出来
        public void Start()
        {
            //创建服务端线程，实现客户端连接请求的循环监听
            var serverThread = new Thread(this.ListenClientConnect);
            //服务端线程开启
            serverThread.Start();
        }

        //监听客户端链接
        private void ListenClientConnect()
        {
            //设置循环标志位
            while (true)
            {
                //获取连接到服务端的客户端
                this.ClientSocket = this.ServerSocket.Accept();

                //将获取到的客户端添加到客户端列表
                this.ClientSockets.Add(this.ClientSocket);

                //向客户端发送一条消息
                this.SendMessage($"客户端{this.ClientSocket.RemoteEndPoint}已成功连接到服务器");

                //创建客户端消息线程，实现客户端消息的循环监听
                var receiveThread = new Thread(this.ReceiveClient);

                //注意到ReceiveClient方法传入了一个参数
                //实际上这个参数就是此时连接到服务器的客户端
                //即ClientSocket
                receiveThread.Start(this.ClientSocket);
            }
        }

        //接受客户端消息的方法
        private void ReceiveClient(object obj)
        {
            Socket clientSocket = (Socket)obj;
            while (true)
            {
                try
                {
                    //获取数据长度
                    int receiveLength = clientSocket.Receive(result);
                    //获取客户端信息
                    var clientMessage = Encoding.UTF8.GetString(result, 0, receiveLength);
                    //服务端负责将客户端的消息分发给各个客户端
                    this.SendMessage($"客户端{clientSocket.RemoteEndPoint}发来消息{clientMessage}");
                }
                catch (Exception e)
                {
                    //从客户端列表中移除该客户端
                    this.ClientSockets.Remove(clientSocket);
                    //向其它客户端告知该客户端下线
                    this.SendMessage($"服务器发来消息：客户端{clientSocket.RemoteEndPoint}从服务器断开，断开原因{e.Message}");
                    //断开连接
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                    break;
                }
            }
        }

        //向所有的客户端群发消息
        public void SendMessage(string msg)
        {
            //确保消息非空以及客户端列表非空
            if (msg == string.Empty || this.ClientSockets.Count <= 0)
            {
                return;
            }
            //向每一个客户端发送消息
            foreach (var clientSocket in this.ClientSockets)
            {
                clientSocket.Send(Encoding.UTF8.GetBytes(msg));
            }
        }

        //向指定的客户端发送消息
        public void SendMessage(string ip, int port, string msg)
        {
            //构造出一个终端地址
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

            //遍历所有客户端
            foreach (var clientSocket in ClientSockets)
            {
                if (ipEndPoint == (IPEndPoint)clientSocket.RemoteEndPoint)
                {
                    clientSocket.Send(Encoding.UTF8.GetBytes(msg));
                }
            }
        }
    }
}