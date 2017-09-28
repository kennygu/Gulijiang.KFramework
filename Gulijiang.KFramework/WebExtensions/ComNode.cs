using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Gulijiang.KFramework.WebExtensions
{
    /// <summary>
    /// combobox节点对象
    /// </summary>
    [DataContract]
    public class ComNode
    {
        /// <summary>
        /// 节点ID
        /// </summary>
        [DataMember]
        public string id
        {
            get;
            set;
        }
        /// <summary>
        /// 节点text
        /// </summary>
        [DataMember]
        public string text
        {
            get;
            set;

        }
        /// <summary>
        /// 搜索内容
        /// </summary>
        [DataMember]
        public string searchtext
        {
            get;
            set;
        }
        /// <summary>
        /// 存放其它数据内容
        /// </summary>
        [DataMember]
        public string content
        {
            get;
            set;
        }
    }
}
