using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Gulijiang.KFramework.WebExtensions
{
    /// <summary>
    /// grid的数据
    /// </summary>
    [DataContract]
    public class GridData
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="trows">行数据</param>
        /// <param name="ttotal">总数量</param>
        public GridData(Object trows, int ttotal)
        {
            rows = trows;
            total = ttotal;
        }
        /// <summary>
        /// 行数据
        /// </summary>
        [DataMember]
        public Object rows
        {
            get;
            set;
        }
        /// <summary>
        /// 总数量
        /// </summary>
        [DataMember]
        public int total
        {
            get;
            set;
        }
    }

}
