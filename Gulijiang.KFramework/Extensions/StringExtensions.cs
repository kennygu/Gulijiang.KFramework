using Gulijiang.KFramework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gulijiang.KFramework.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// 字符串加密
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Encrypt(this string s)
        {
            return DESEncrypt.Encrypt(s);
        }
        /// <summary>
        /// 字符串加密--带Key加密
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Encrypt(this string s, string key)
        {
            return DESEncrypt.Encrypt(s,key);
        }
        /// <summary>
        /// 字符串解密
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Decrypt(this string s)
        {
            return DESEncrypt.Decrypt(s);
        }
        /// <summary>
        /// 字符串解密--带Key解密
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Decrypt(this string s,string key)
        {
            return DESEncrypt.Decrypt(s, key);
        }
        /// <summary>
        /// 字符串转换为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <returns></returns>
        public static T JsonToObject<T>(this string s)
        {
            T res = Json.JsonHelper.JsonToObject<T>(s);
            return res;
        }
    }
}
