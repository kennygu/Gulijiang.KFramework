using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Gulijiang.KFramework.WebExtensions
{
    /// <summary>
    /// 返回结果基类
    /// </summary>
    [DataContract]
    [Serializable]
    public class BaseResult
    {
        /// <summary>
        /// 无参数构造函数
        /// </summary>
        public BaseResult()
        {
            Code = 1;
            Message = "success";
        }
        /// <summary>
        /// 结果编码
        /// </summary>
        [DataMember]
        public int Code
        {
            get;
            set;
        }
        /// <summary>
        /// 结果消息
        /// </summary>
        [DataMember]
        public string Message
        {
            get;
            set;
        }
    }
}
