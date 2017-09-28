using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting.Channels;
using System.Configuration;
using System.Collections;
using Gulijiang.KFramework.Net.Transfer; 
namespace Gulijiang.KFramework.Log
{
    /// <summary>
    /// 日志客户端
    /// </summary>
    public static class LogClient
    {
        static LogClient()
        {
            LogTransfer = new TransferClient(ConfigurationSettings.AppSettings["LogServerIP"].ToString(), int.Parse(ConfigurationSettings.AppSettings["LogServerPort"]));
        }
        /// <summary>
        /// 日志传输对象
        /// </summary>
        public static TransferClient LogTransfer
        {
            get;
            set;
        }
        private static readonly object syncEnqueueObj = new object();
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="logRequest"></param>
        public static void Log(LogRequest logRequest)
        {
            lock (syncEnqueueObj)
            {
                try
                {
                    LogTransfer.Send(logRequest);
                }
                catch (Exception ex)
                {

                }

            }
        }
    }
}
