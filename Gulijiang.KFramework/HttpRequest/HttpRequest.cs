using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using Gulijiang.KFramework.HttpRequest.Common;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Collections;
using System.Runtime.InteropServices;

namespace Gulijiang.KFramework.HttpRequest
{
    /// <summary>
    /// 网络请求基类
    /// </summary>
    public abstract class HttpRequest : IHttpRequest
    {
        /// <summary>
        /// 请求地址
        /// </summary>
        protected string Uri;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="uri"></param>
        protected HttpRequest(string uri)
        {
            this.Uri = uri;
            this.ReqEncoding = Encoding.UTF8;
            this.RepEncoding = Encoding.UTF8;
            HttpTimeOut = 60000;
        }
        public string Host
        {
            get;
            set;
        }
        public int HttpTimeOut
        {
            get;
            set;
        }
        public string Referer
        {
            get;
            set;
        }
        public string UserAgent
        {
            get;
            set;
        }
        public string Accept
        {
            get;
            set;
        }
        public WebHeaderCollection Headers
        {
            get;
            set;
        }
        /// <summary>
        /// 获取网络请求对象
        /// </summary>
        /// <returns></returns>
        public List<Cookie> GetResponseObj()
        {
            HttpWebRequest req = HttpWebRequest.Create(ConstructUri()) as HttpWebRequest;

            //req.Headers.Add("Cookie", CookieString);
            req.ContentType = "text/html; charset=utf-8";
            req.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; zh-CN; rv:1.9.2.13) Gecko/20101203 AlexaToolbar/alxf-2.0 Firefox/3.6.13";
            req.Accept = "text/html,application/xhtml xml,application/xml;q=0.9,*/*;q=0.8";
            req.KeepAlive = false;
            req.Timeout = HttpTimeOut;

            CookieContainer mco = new CookieContainer();
            req.CookieContainer = mco;
            if (CookieList != null)
            {
                foreach (Cookie tempCookie in CookieList)
                {
                    req.CookieContainer.Add(tempCookie);
                }
            }
            req.Method = HttpMethod.Get;
            List<Cookie> resList = new List<Cookie>();
            try
            {
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                resp.Close();
                resList = GetAllCookies(req.CookieContainer);
                return resList;
            }
            catch (WebException wex)
            {
                throw wex;
                //throw wex;
            }
            finally
            {
                req.Abort();
            }
        }
        public List<Cookie> CookieList
        {
            get;
            set;
        }
        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool InternetGetCookieEx(string pchURL, string pchCookieName, StringBuilder pchCookieData, ref System.UInt32 pcchCookieData, int dwFlags, IntPtr lpReserved);

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int InternetSetCookieEx(string lpszURL, string lpszCookieName, string lpszCookieData, int dwFlags, IntPtr dwReserved);

        private static string GetCookies(string url)
        {
            uint datasize = 256;
            StringBuilder cookieData = new StringBuilder((int)datasize);
            if (!InternetGetCookieEx(url, null, cookieData, ref datasize, 0x2000, IntPtr.Zero))
            {
                if (datasize < 0)
                    return null;

                cookieData = new StringBuilder((int)datasize);
                if (!InternetGetCookieEx(url, null, cookieData, ref datasize, 0x00002000, IntPtr.Zero))
                    return null;
            }
            return cookieData.ToString();
        }
        public static List<Cookie> GetAllCookies(CookieContainer cc)
        {
            List<Cookie> lstCookies = new List<Cookie>();

            Hashtable table = (Hashtable)cc.GetType().InvokeMember("m_domainTable", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField | System.Reflection.BindingFlags.Instance, null, cc, new object[] { });
            StringBuilder sb = new StringBuilder();
            foreach (object pathList in table.Values)
            {
                SortedList lstCookieCol = (SortedList)pathList.GetType().InvokeMember("m_list", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField | System.Reflection.BindingFlags.Instance, null, pathList, new object[] { });
                foreach (CookieCollection colCookies in lstCookieCol.Values)
                    foreach (Cookie c in colCookies)
                    {
                        lstCookies.Add(c);
                        sb.AppendLine(c.Domain + ":" + c.Name + "____" + c.Value + "\r\n");
                    }
            }
            return lstCookies;
        }

        private static readonly object syncRequestObj = new object();
        /// <summary>
        /// 网络请求
        /// </summary>
        /// <returns></returns>
        public virtual string Request()
        {
            HttpWebRequest req = HttpWebRequest.Create(ConstructUri()) as HttpWebRequest;

            req.Timeout = HttpTimeOut;
            if (ProxyIP != null)
            {
                WebProxy proxy = new WebProxy(ProxyIP.IP, ProxyIP.Port);
                req.Proxy = proxy;
            }
            if (!String.IsNullOrEmpty(UserAgent))
                req.UserAgent = UserAgent;
            if (!String.IsNullOrEmpty(Referer))
                req.Referer = Referer;
            if (!String.IsNullOrEmpty(Accept))
                req.Accept = Accept;
            if (!String.IsNullOrEmpty(Host))
                req.Host = Host;
            if (Headers != null)
            {
                for (int i = 0; i < Headers.Count; i++)
                {
                    req.Headers.Add(Headers[i]);
                }
            }

            CookieContainer mco = new CookieContainer();
            req.CookieContainer = mco;
            if (CookieList != null)
            {
                foreach (Cookie tempCookie in CookieList)
                {
                    req.CookieContainer.Add(tempCookie);
                }
            }
            if (!string.IsNullOrEmpty(Method))
            {
                req.Method = Method;
            }
            if (!string.IsNullOrEmpty(ContentType))
            {
                req.ContentType = ContentType;
            }
            PrepareRequest(req);
            if (req.Method == HttpMethod.Post)
            {
                req.ServicePoint.Expect100Continue = false;
                using (var reqStream = req.GetRequestStream())
                {
                    WriteBody(reqStream);
                }
            }
            WebResponse resp = null;
            try
            {
                resp = req.GetResponse();
                HttpWebResponse tempsp = resp as HttpWebResponse;
                string strHtml = RetrieveResponse(resp);
                tempsp.Close();
                return strHtml;
            }
            catch (WebException ex)
            {

                throw ex;
            }
            finally
            {
                req.Abort();
            }
            // }
        }

        /// <summary>
        /// 异步网络请求
        /// </summary>
        /// <param name="callback"></param>
        public void RequestAsync(AsyncCallback<string> callback)
        {
            var thread = new Thread(new ParameterizedThreadStart(DoRequest));
            thread.IsBackground = true;
            thread.Name = string.Format("RequestingThread:{0}", Uri);
            thread.Start(callback);
        }
        public ProxyIP ProxyIP
        {
            get;
            set;
        }
        /// <summary>
        /// Performs the async request.
        /// </summary>
        /// <param name="state"></param>
        private void DoRequest(object state)
        {
            var callback = state as AsyncCallback<string>;
            AsyncCallResult<string> reqResult = new AsyncCallResult<string>();

            try
            {
                reqResult.Data = Request();
                reqResult.Success = true;
            }
            catch (Exception ex)
            {
                reqResult.Success = false;
                reqResult.Exception = ex;
            }
            finally
            {
                callback(reqResult);
            }
        }

        /// <summary>
        /// When overriden in derived classes, constructs the final request uri based on relevent conditions.
        /// </summary>
        /// <returns>The final request uri constucted.</returns>
        protected virtual string ConstructUri()
        {
            return Uri;
        }

        /// <summary>
        /// When overridden in derived classes, appends HTTP headers into the request.
        /// </summary>
        /// <param name="headers">The web header collection object.</param>
        protected virtual void AppendHeaders(WebHeaderCollection headers)
        {
            // Nothing to do.
        }

        /// <summary>
        /// When overridden in derived classes, sets additional request arguments on the web request object.
        /// </summary>
        /// <param name="webRequest">The web request object.</param>
        protected virtual void PrepareRequest(HttpWebRequest webRequest)
        {
            // Nothing to do.
        }

        /// <summary>
        /// When overridden in derived classes, writes data into the request stream.
        /// <remarks>HTTP GET type request is not supported.</remarks>
        /// </summary>
        /// <param name="reqStream">The request stream object.</param>
        protected virtual void WriteBody(Stream reqStream)
        {
            // Nothing to do. 
        }

        /// <summary>
        /// Retrieves the response from the request.
        /// </summary>
        /// <param name="webResponse"></param>
        /// <returns>The server response string(UTF8 decoded).</returns>
        protected virtual string RetrieveResponse(WebResponse webResponse)
        {
            var respContent = string.Empty;

            var respStream = webResponse.GetResponseStream();
            using (StreamReader reader = new StreamReader(respStream, RepEncoding))
            {
                respContent = reader.ReadToEnd();

            }
            webResponse.Close();

            return respContent;
        }
        public string CookieString
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the HTTP method of the request.(i.e: POST, GET etc.)
        /// </summary>
        protected virtual string Method
        {
            get;
            set;
        }
        public virtual Encoding ReqEncoding
        {
            get;
            set;
        }
        public virtual Encoding RepEncoding
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the content type of the request.(i.e: text/plain etc.)
        /// </summary>
        public virtual string ContentType
        {
            get;
            set;
        }
        /// <summary>
        /// 数字证书地址
        /// </summary>
        public virtual string CertPath
        {
            get;
            set;
        }
        /// <summary>
        /// 数字证书密码
        /// </summary>
        public virtual string CertPassword
        {
            get;
            set;
        }



    }
}
