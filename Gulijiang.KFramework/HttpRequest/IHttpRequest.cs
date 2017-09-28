using Gulijiang.KFramework.HttpRequest.Common;
using System.Collections.Generic;
using System.Net;

namespace Gulijiang.KFramework.HttpRequest
{
    /// <summary>
    /// 网络请求接口
    /// </summary>
    public interface IHttpRequest
    {
        /// <summary>
        /// 网络请求
        /// </summary>
        /// <returns></returns>
        string Request();
        /// <summary>
        /// 异步网络请求
        /// </summary>
        /// <param name="callback"></param>
        void RequestAsync(AsyncCallback<string> callback);
        /// <summary>
        /// 获取网络请求对象
        /// </summary>
        /// <returns></returns>
        List<Cookie> GetResponseObj();
    }
}
