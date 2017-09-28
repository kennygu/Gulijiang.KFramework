using System;
using System.Collections.Generic;
using System.Text;

using System.Collections;
using System.Text.RegularExpressions;

namespace Gulijiang.KFramework.HttpRequest.Common
{
    /// <summary>
    /// �ַ����ж�
    /// </summary>
    public class WordHelper
    {
        Type type = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType;

        #region  bool CheckNullstr(string Getstr)�ж��Ƿ��ǿ�ֵ
        /// <summary>
        /// Getstr�õ������ж��Ƿ��ǿ�ֵ
        /// </summary>
        /// <param name="Getstr">��Ҫ����ֵ</param>
        /// <param name="GetShow">����ֶεĹ���˵��������,sex</param>
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

        #region bool CheckNumber(string GetNum)������ʽ �ж��Ƿ������ָ�ʽ
        /// <summary>
        /// �ж��Ƿ������ָ�ʽ
        /// </summary>
        /// <param name="GetNum"></param>
        public bool CheckNumber(string GetNum)
        {
            //^[+-]?\d+(\.\d+)?$�������ֺ�С��     ���ֺ�С��^\d+(\.\d+)?$
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

        #region string CheckReg(string RegInput, string WebText)������ʽƥ�� �û������ֵ
        /// <summary>
        /// �������ܣ�������ʽƥ�� �û������ֵ
        /// �����ж��Ƿ�Ϊ��ֵ
        /// ����Session[����]��������������ƥ��
        /// ������Ϣ
        /// userInput	������ʽ
        /// WebText		��Ҫ����ƥ�������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public string CheckReg(string RegInput, string WebText)
        {

            if (CheckNullstr(RegInput) == false || CheckNullstr(WebText) == false)
            {
                //throw new Exception("��������Ϊ��");
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

        #region bool toFilter(string thePara)���Ƿ��ַ�
        /// <summary>
        /// ���Ƿ��ַ�����ֹsqlע��
        /// ��������ǿգ�����false
        /// ��������а����Ƿ��ַ�������false
        ///// ���򷵻�    true
        /// </summary>
        /// <param name="thePara"></param>
        /// <returns></returns>
        public bool toFilter(string thePara)
        {
            string[] BadCode = new string[] { "'", "\"", "exec", "cmd", ">", "<", "and", "=", "\\", ";" };
            try
            {
                if (CheckNullstr(thePara) == false)          //��������ǿ�ֵ������false
                {
                    throw new Exception("����Ϊ��");
                }
                else
                {
                    for (int i = 0; i < BadCode.Length; i++)
                    {
                        if (thePara.IndexOf(BadCode[i]) > 0)
                        {
                            throw new Exception("�����Ƿ��ַ�");
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

        #region ����������ַ���Ƿ�������֣�����������֣����б���{1,9}1-9������ƥ��
        /// <summary>
        /// ����������ַ���Ƿ�������֣�����������֣����б���{1,9}1-9������ƥ��
        /// </summary>
        /// <param name="userInput">�û���������ݣ�Ҳ����Ҫ��������</param>
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
                ///		���ƥ�人�ֵĻ��������ֱ���
                userInput = rgx.Replace(userInput, System.Web.HttpUtility.UrlEncode(m.Value.ToString().Trim(), System.Text.Encoding.GetEncoding("GB2312")));
                m = m.NextMatch();
            }
            return userInput;
        }
        #endregion

        #region ���������ʽ��
        /// <summary>
        /// �������ܣ�����û������ֵ
        /// �����ж��Ƿ�Ϊ��ֵ
        /// ����Session[����]��������������ƥ��
        /// ������Ϣ
        /// userInput	������ʽ
        /// WebText		��ȡ����ҳ��Ϣ���洢��Session�У�����Ϊ�գ�
        /// IsGood		�Ƿ�ƥ��	����LBL_err.text
        /// return_str	ͨ������ƥ�䣬���ص���Ϣ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public string[] Check_url_Array(string userInput, string WebText)
        {
                if (userInput == null || userInput == "")
                {
                    throw new Exception("������ƥ���������ʽ");
                }
                if (WebText == null || WebText == "")
                {
                    throw new Exception("�����¶�ȡ��ҳ����");
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

        #region ����ƥ�����������
        /// <summary>
        /// �������ܣ�����û������ֵ
        /// �����ж��Ƿ�Ϊ��ֵ
        /// ����Session[����]��������������ƥ��
        /// ������Ϣ
        /// userInput	�û������ֵ����  ������ʽ
        /// WebText		��ȡ����ҳ��Ϣ���洢��Session�У�����Ϊ�գ�
        /// IsGood		�Ƿ�ƥ��	����LBL_err.text
        /// return_str	ͨ������ƥ�䣬���ص���Ϣ
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
                    return "������ƥ���������ʽ";
                }
                if (WebText == null || WebText == "")
                {
                    return "�����¶�ȡ��ҳ���ݡ�";
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
                return "�޷�ƥ������";
            }

        }
        #endregion

        #region string[] Check_url_string(string userInput, string WebText)�ڶ�ȡҳ�泬�����ӵ�ʱ�򣬷�������2007-3-20
        /// <summary>
        /// ���س����ӵ�����
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

        #region ȥ��HTML���
        /// <summary>
        /// ȥ��HTML���
        /// </summary>
        /// <param name="Htmlstring">����HTML��Դ�� </param>
        /// <returns>�Ѿ�ȥ���������</returns>
        public string NoHTML(string Htmlstring)
        {
            //ɾ���ű�
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //ɾ��HTML
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
            //Htmlstring = Regex.Replace(Htmlstring, @"<.*?>", "", RegexOptions.IgnoreCase);//      ������б�ǩ
            Htmlstring = Regex.Replace(Htmlstring, @"<script.*>[\s\S]*?</script>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<td.*?>", "", RegexOptions.IgnoreCase);//      ���td
            Htmlstring = Regex.Replace(Htmlstring, @"</td>", "", RegexOptions.IgnoreCase);//      ���td
            Htmlstring = Regex.Replace(Htmlstring, @"<div.*?>", "", RegexOptions.IgnoreCase);//      ���div
            Htmlstring = Regex.Replace(Htmlstring, @"</div>", "", RegexOptions.IgnoreCase);//      ���td
            //Htmlstring.Replace("<", "");
            // Htmlstring.Replace(">", "");
            //Htmlstring.Replace("\r\n", "");
            //Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();

            return Htmlstring;
        }
        #endregion

    }
}
