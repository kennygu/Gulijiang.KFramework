using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Gulijiang.KFramework.EasyUIExtensions;

namespace Gulijiang.KFramework.WebExtensions
{
    /// <summary>
    /// 异常结果基类
    /// </summary>
    [DataContract]
    [Serializable]
    public class ExceptionResult : DataResult
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ex">异常</param>
        public ExceptionResult(Exception ex)
            : base(ex)
        {
            Code = -1;
            Message = ex.Message;
        }
    }
}
