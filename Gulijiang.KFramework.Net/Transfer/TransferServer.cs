using Gulijiang.KFramework.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Gulijiang.KFramework.Net.Transfer
{
    public class TransferServer<T>
    {
        /// <summary>
        /// 构造对象传输服务端
        /// </summary>
        /// <param name="ip">服务端IP</param>
        /// <param name="port">服务端端口</param>
        public TransferServer(string ip, int port)
        {
            ServerIP = IPAddress.Parse(ip);
            ServerPort = port;
        }
        /// <summary>
        /// socket对象
        /// </summary>
        private Socket socketServer { get; set; }
        /// <summary>
        /// 服务端IP
        /// </summary>
        public IPAddress ServerIP
        {
            get;
            set;
        }
        /// <summary>
        /// 服务端端口
        /// </summary>
        public int ServerPort
        {
            get;
            set;
        }
        /// <summary>
        /// 接收成功委托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public delegate void ReceiveCompletedEventHandler(object sender, ReceiveEventArgs<T> args);
        /// <summary>
        /// 接收完成事件
        /// </summary>
        public event ReceiveCompletedEventHandler ReceiveCompleted;
        /// <summary>
        /// 发送消息
        /// 以json格式传输
        /// </summary>
        public void Receive()
        {
            socketServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint point = new IPEndPoint(ServerIP, ServerPort);
            socketServer.Bind(point);
            socketServer.Listen(1000);
            while (true)
            {
                Socket client = socketServer.Accept();
                handleSocket(client);
            }
        }
        /// <summary>
        /// 处理接收到的SOCKET
        /// </summary>
        /// <param name="socketClient"></param>
        private void handleSocket(Socket socketClient)
        {
            ThreadPool.QueueUserWorkItem(delegate(object o)
            {
                byte[] bytes = new byte[1024];
                List<byte> datas = new List<byte>();
                int length = socketClient.Receive(bytes);
                while (length > 0)
                {
                    if (length == bytes.Length)
                    {
                        datas.AddRange(bytes);
                    }
                    else
                    {
                        for (int i = 0; i < length; i++)
                        {
                            datas.Add(bytes[i]);
                        }
                    }
                    length = socketClient.Receive(bytes);
                }
                string receiveData = System.Text.Encoding.GetEncoding("gb2312").GetString(datas.ToArray());
                socketClient.Close();
                ReceiveCompleted(this, new ReceiveEventArgs<T>(JsonHelper.JsonToObject<T>(receiveData)));
            });
        }
    }
    public class ReceiveEventArgs<T> : EventArgs
    {
        public ReceiveEventArgs(T receiveObject)
        {
            ReceiveObject = receiveObject;
        }
        public T ReceiveObject
        {
            get;
            set;
        }
    }
}
