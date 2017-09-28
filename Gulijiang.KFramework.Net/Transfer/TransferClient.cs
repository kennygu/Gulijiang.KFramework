
using Gulijiang.KFramework.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Gulijiang.KFramework.Net.Transfer
{
    public class TransferClient
    {
        /// <summary>
        /// 构造对象传输客户端
        /// </summary>
        /// <param name="ip">服务端IP</param>
        /// <param name="port">服务端端口</param>
        public TransferClient(string ip, int port)
        {
            ServerIP = IPAddress.Parse(ip);
            ServerPort = port;
        }
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
        /// 发送消息
        /// 以json格式传输
        /// </summary>
        public void Send(Object sendObj)
        {
            string strJson = JsonHelper.ObjectToJson(sendObj);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(ServerIP, ServerPort);
            Byte[] buffer = System.Text.Encoding.GetEncoding("gb2312").GetBytes(strJson);
            socket.Send(buffer);
            socket.Shutdown(SocketShutdown.Send);
        }
    }
}
