using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gulijiang.KFramework.HttpRequest
{
    [Serializable]
    public class ProxyIP
    {
        public string IP
        {
            get;
            set;
        }
        public int Port
        {
            get;
            set;
        }
        public long IPID
        {
            get;
            set;
        }
        public int SuccessNumber
        {
            get;
            set;
        }
        public int TotalNumber
        {
            get;
            set;
        }
    }
}
