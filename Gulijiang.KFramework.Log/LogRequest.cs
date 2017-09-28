using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gulijiang.KFramework.Log
{
    [Serializable]
    public class LogRequest
    {
        public string APPID
        {
            get;
            set;
        }
        public int LogType
        {
            get;
            set;
        }
        public string BusinessID
        {
            get;
            set;
        }
        public string LogDesc
        {
            get;
            set;
        }
        public DateTime CreateTime
        {
            get;
            set;
        }
        public int CreateMillisecond
        {
            get;
            set;
        }
        public int LogID
        {
            get;
            set;
        }
    }
}
