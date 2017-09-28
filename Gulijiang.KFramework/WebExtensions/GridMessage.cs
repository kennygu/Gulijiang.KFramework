using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Gulijiang.KFramework.WebExtensions
{
    /// <summary>
    /// grid列表请求消息
    /// </summary>
    [Serializable]
    [DataContract]
    public class GridMessage : BaseMessage
    {
        /// <summary>
        /// 页码
        /// </summary>
        [DataMember]
        public int page
        {
            get;
            set;
        }
        /// <summary>
        /// 页面大小
        /// </summary>
        [DataMember]
        public int rows
        {
            get;
            set;
        }
        /// <summary>
        /// 排序字段
        /// </summary>
        [DataMember]
        public string sort
        {
            get;
            set;
        }
        /// <summary>
        /// 排序方式
        /// </summary>
        [DataMember]
        public string order
        {
            get;
            set;
        }
        /// <summary>
        /// 开始索引
        /// </summary>
        public int StartIndex
        {
            get
            {
                return (page - 1) * rows + 1;
            }
        }
        /// <summary>
        /// 结束索引
        /// </summary>
        public int EndIndex
        {
            get
            {
                return StartIndex + rows - 1;
            }
        }

    }
}
