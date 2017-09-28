using Gulijiang.KFramework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gulijiang.KFramework.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// 将日期转换为短字符串格式yyyy-MM-dd
        /// </summary>
        /// <param name="s">日期</param>
        /// <returns></returns>
        public static string ToShortDate(this DateTime s)
        {
            return s.ToString("yyyy-MM-dd");
        }
        /// <summary>
        /// 将日期转换为
        /// </summary>
        /// <param name="s">日期</param>
        /// <returns></returns>
        public static string ToLongDateTime(this DateTime s)
        {
            return s.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
