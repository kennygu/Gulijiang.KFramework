using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
namespace Gulijiang.KFramework.Log
{
    /// <summary>
    /// 日志队列处理
    /// </summary>
    public static class LogQueue
    {
        private static Queue<LogRequest> logQueue = null;
        static LogQueue()
        {
            logQueue = new Queue<LogRequest>();

        }
        private static readonly object syncEnqueueObj = new object();
        /// <summary>
        /// 插入日志队列
        /// </summary>
        /// <param name="logRequest"></param>
        public static void Enqueue(LogRequest logRequest)
        {
            lock (syncEnqueueObj)
            {
                if (logRequest != null)
                {
                    //插入队列内容
                    logQueue.Enqueue(logRequest);
                }

            }
        }
        private static readonly object syncDequeueObj = new object();
        /// <summary>
        /// 读取队列
        /// </summary>
        /// <returns></returns>
        public static LogRequest Dequeue()
        {
            lock (syncDequeueObj)
            {
                if (logQueue.Count != 0)
                {
                    //读取列内容
                    LogRequest logRequest = logQueue.Dequeue();
                    return logRequest;
                }
                else
                    return null;
            }
        }
    }
}
