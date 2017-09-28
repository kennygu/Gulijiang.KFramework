using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Gulijiang.KFramework.WebExtensions
{
    /// <summary>
    /// 数据返回类
    /// </summary>
    [DataContract]
    public class DataResult : BaseResult
    {
        public DataResult()
        {

        }
        public DataResult(Object data)
        {
            Data = data;
        }
        [DataMember]
        public Object Data
        {
            get;
            set;
        }
    }
}
