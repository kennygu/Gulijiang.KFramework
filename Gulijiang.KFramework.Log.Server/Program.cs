using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gulijiang.KFramework.Log.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            LogService tservice = new LogService();
            tservice.Run();
        }
    }
}
