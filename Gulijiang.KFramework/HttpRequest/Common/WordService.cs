using System;
using System.Collections.Generic;
using System.Text;

using System.Collections;
using System.Text.RegularExpressions;

namespace Gulijiang.KFramework.HttpRequest.Common
{
    /// <summary>
    /// 字符串判断
    /// </summary>
    public class WordHelper
    {
        Type type = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType;

        #region  bool CheckNullstr(string Getstr)判断是否是空值
        /// <summary>
        /// Getstr得到参数判断是否是空值
        /// </summary>
        /// <param name="Getstr">需要检查的值</param>
        /// <param name="GetShow">这个字段的功能说明：姓名,sex</param>
        public bool CheckNullstr(string Getstr)
        {
            try
            {
                Getstr = Getstr.Trim();
                if (Getstr == "" || Getstr == null || Getstr.Length < 1)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        #endregion

        #region bool CheckNumber(string GetNum)正则表达式 判断是否是数字格式
        /// <summary>
        /// 判断是否是数字格式
        /// </summary>
        /// <param name="GetNum"></param>
        public bool CheckNumber(string GetNum)
        {
            //^[+-]?\d+(\.\d+)?$正负数字含小数     数字含小数^\d+(\.\d+)?$
            Regex r = new Regex(@"^\d+(\.\d+)?$");
            if (r.IsMatch(GetNum))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        #endregion

        #region string CheckReg(string RegInput, string WebText)正则表达式匹配 用户输入的值
        /// <summary>
        /// 函数功能：正则表达式匹配 用户输入的值
        /// 首先判断是否为空值
        /// 根据Session[变量]，检查输入的正则匹配
        /// 返回信息
        /// userInput	正则表达式
        /// WebText		需要正则匹配的内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public string CheckReg(string RegInput, string WebText)
        {

            if (CheckNullstr(RegInput) == false || CheckNullstr(WebText) == false)
            {
                //throw new Exception("参数不能为空");
                return "";
            }

            try
            {
                Regex r = new Regex(RegInput);
                Match m = r.Match(WebText);
                if (m.Success)
                {
                    return m.Groups[1].ToString();
                }
                else
                {
                    return "";
                }
            }
            catch 
            {
                return "";
            }
        }
        #endregion

        #region bool toFilter(string thePara)检测非法字符
        /// <summary>
        /// 检测非法字符，防止sql注入
        /// 如果参数是空，返回false
        /// 如果参数中包含非法字符，返回false
        ///// 否则返回    true
        /// </summary>
        /// <param name="thePara"></param>
        /// <returns></returns>
        public bool toFilter(string thePara)
        {
            string[] BadCode = new string[] { "'", "\"", "exec", "cmd", ">", "<", "and", "=", "\\", ";" };
            try
            {
                if (CheckNullstr(thePara) == false)          //如果参数是空值，返回false
                {
                    throw new Exception("参数为空");
                }
                else
                {
                    for (int i = 0; i < BadCode.Length; i++)
                    {
                        if (thePara.IndexOf(BadCode[i]) > 0)
                        {
                            throw new Exception("包含非法字符");
                        }
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }


        }
        #endregion

        #region 检查输入的网址中是否包含汉字，如果包含汉字，进行编码{1,9}1-9个汉字匹配
        /// <summary>
        /// 检查输入的网址中是否包含汉字，如果包含汉字，进行编码{1,9}1-9个汉字匹配
        /// </summary>
        /// <param name="userInput">用户输入的内容，也就是要检查的内容</param>
        /// <returns></returns>
        public string Check_ChineseCode(string userInput)
        {
            string RegStr = "[\u4e00-\u9fa5]{1,9}";
            Regex rgx = new Regex(RegStr);
            Regex r = new Regex(RegStr);
            Match m = r.Match(userInput);
            if (m.Success == false)
            {
                return userInput;
            }
            while (m.Success)
            {
                ///		如果匹配汉字的话，将汉字编码
                userInput = rgx.Replace(userInput, System.Web.HttpUtility.UrlEncode(m.Value.ToString().Trim(), System.Text.Encoding.GetEncoding("GB2312")));
                m = m.NextMatch();
            }
            return userInput;
        }
        #endregion

        #region 检查正则表达式，
        /// <summary>
        /// 函数功能：获得用户输入的值
        /// 首先判断是否为空值
        /// 根据Session[变量]，检查输入的正则匹配
        /// 返回信息
        /// userInput	正则表达式
        /// WebText		读取的网页信息，存储在Session中，不能为空；
        /// IsGood		是否匹配	付给LBL_err.text
        /// return_str	通过正则匹配，返回的信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public string[] Check_url_Array(string userInput, string WebText)
        {
                if (userInput == null || userInput == "")
                {
                    throw new Exception("请输入匹配的正则表达式");
                }
                if (WebText == null || WebText == "")
                {
                    throw new Exception("请重新读取网页内容");
                }
                MatchCollection mc = Regex.Matches(WebText, userInput, RegexOptions.IgnoreCase);
                ArrayList Reg_List = new ArrayList();
                foreach (Match m in mc)
                {
                    Reg_List.Add(m.Groups[1].Value);
                }
                return (string[])Reg_List.ToArray(typeof(string));
        }
        #endregion

        #region 返回匹配的整个内容
        /// <summary>
        /// 函数功能：获得用户输入的值
        /// 首先判断是否为空值
        /// 根据Session[变量]，检查输入的正则匹配
        /// 返回信息
        /// userInput	用户输入的值或者  正则表达式
        /// WebText		读取的网页信息，存储在Session中，不能为空；
        /// IsGood		是否匹配	付给LBL_err.text
        /// return_str	通过正则匹配，返回的信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public string Check_url_string(string userInput, string WebText)
        {
            string GetAllInfo = string.Empty;
            try
            {
                if (userInput == null || userInput == "")
                {
                    return "请输入匹配的正则表达式";
                }
                if (WebText == null || WebText == "")
                {
                    return "请重新读取网页内容。";
                }
                MatchCollection mc = Regex.Matches(WebText, userInput, RegexOptions.IgnoreCase);

                foreach (Match m in mc)
                {
                    GetAllInfo += m.Value;
                }
                return GetAllInfo;
            }
            catch (Exception ex)
            {
                return "无法匹配内容";
            }

        }
        #endregion

        #region string[] Check_url_string(string userInput, string WebText)在读取页面超级链接的时候，返回数组2007-3-20
        /// <summary>
        /// 返回超连接的数组
        /// 
        /// </summary>
        /// <param name="userInput"></param>
        /// <param name="WebText"></param>
        /// <returns></returns>

        public string[] Get_url_Array(string userInput, string WebText)
        {
            MatchCollection mc = Regex.Matches(WebText, userInput, RegexOptions.IgnoreCase);
            ArrayList Url_List = new ArrayList();
            foreach (Match m in mc)
            {
                Url_List.Add(m.Value);
            }
            return (string[])Url_List.ToArray(typeof(string));
        }
        #endregion

        #region 去除HTML标记
        /// <summary>
        /// 去除HTML标记
        /// </summary>
        /// <param name="Htmlstring">包括HTML的源码 </param>
        /// <returns>已经去除后的文字</returns>
        public string NoHTML(string Htmlstring)
        {
            //删除脚本
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML
            /*Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);*/
            //      add new 2006-12-30
            //Htmlstring = Regex.Replace(Htmlstring, @"<.*?>", "", RegexOptions.IgnoreCase);//      清除所有标签
            Htmlstring = Regex.Replace(Htmlstring, @"<script.*>[\s\S]*?</script>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<td.*?>", "", RegexOptions.IgnoreCase);//      清除td
            Htmlstring = Regex.Replace(Htmlstring, @"</td>", "", RegexOptions.IgnoreCase);//      清除td
            Htmlstring = Regex.Replace(Htmlstring, @"<div.*?>", "", RegexOptions.IgnoreCase);//      清除div
            Htmlstring = Regex.Replace(Htmlstring, @"</div>", "", RegexOptions.IgnoreCase);//      清除td
            //Htmlstring.Replace("<", "");
            // Htmlstring.Replace(">", "");
            //Htmlstring.Replace("\r\n", "");
            //Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();

            return Htmlstring;
        }
        #endregion

    }
}
