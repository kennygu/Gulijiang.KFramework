using Gulijiang.KFramework.Net.Transfer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Gulijiang.KFramework.Log.Server
{
    public class LogService
    {
        public void Run()
        {
            Console.WriteLine("receive data.......");

            TransferServer<LogRequest> serverTransfer = new TransferServer<LogRequest>(ConfigurationSettings.AppSettings["LogServerIP"].ToString(), int.Parse(ConfigurationSettings.AppSettings["LogServerPort"]));
            serverTransfer.ReceiveCompleted += serverTransfer_ReceiveCompleted;
            Console.ReadKey();
        }

        void serverTransfer_ReceiveCompleted(object sender, ReceiveEventArgs<LogRequest> args)
        {
            LogQueue.Enqueue(args.ReceiveObject);
        }
    }
}
