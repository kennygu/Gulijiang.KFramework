using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gulijiang.KFramework.WebExtensions
{
    /// <summary>
    /// 消息基类
    /// </summary>
    [Serializable]
    public class BaseMessage
    {
        /// <summary>
        /// 无参数构造函数
        /// </summary>
        public BaseMessage()
        {
            MessageTime = DateTime.Now;
        }
        /// <summary>
        /// 消息时间
        /// </summary>
        public DateTime MessageTime
        {
            get;
            set;
        }
        /// <summary>
        /// 消息类型
        /// </summary>
        public int Type
        {
            get;
            set;
        }
    }
}
