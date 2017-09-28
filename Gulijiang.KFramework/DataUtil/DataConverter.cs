
/*-----*-----*-----*-----*-----*-----*-----*-----*-----*-----*-----*-----*-----*-----*-----*-----*-----*
 文 件 名:  DataConverter
 描    述:  工具类--字符转换类.
 创 建 者:  kenny
 创建日期:  2011年02月01日
*-----*-----*-----*-----*-----*-----*-----*-----*-----*-----*-----*-----*-----*-----*-----*-----*-----*/
using System;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Gulijiang.KFramework.DataUtil
{
    public class DataConverter
    {
        #region 转bool类型
        /// <summary>
        /// 判断输入的字符是否是"true"/"yes"/"1",不区分大小写
        /// </summary>
        /// <param name="input">字符类型</param>
        /// <returns>返回Boolean型</returns>
        public static bool CBool(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                input = input.Trim();
                return (((string.Compare(input, "true", true) == 0) || (string.Compare(input, "yes", true) == 0)) || (string.Compare(input, "1", true) == 0));
            }
            return false;
        }

        /// <summary>
        /// 判断输入的字符是否是"true"/"yes"/"1",不区分大小写
        /// </summary>
        /// <param name="input">字符类型</param>
        /// <returns>返回Boolean型</returns>
        public static bool CBool(object obj)
        {
            if (null != obj)
            {
                string input = obj.ToString().Trim();
                return CBool(input);
            }
            return false;
        }
        #endregion



        /// <summary>
        /// 判断输入的字符是否是"true"/"yes"/"1",不区分大小写
        /// </summary>
        /// <param name="bol">字符类型</param>
        /// <returns>checked=\"checked\</returns>
        public static string CCheck(string bol)
        {
            return CBool(bol) ? "checked=\"checked\"" : "";
        }

        #region bool转int类型
        /// <summary>
        /// bool转int类型
        /// </summary>
        /// <param name="obj">传入的参数</param>
        /// <returns>1or0</returns>
        public static int CBoolToInt(object obj)
        {
            if (CBool(obj))
            {
                return 1;
            }
            return 0;
        }
        #endregion

        #region 时间类型的转化
        /// <summary>
        /// 时间类型转化
        /// </summary>
        /// <param name="input">对象类型,空则返回当前时间,否则返回输入时间</param>
        /// <returns>返回时间</returns>
        public static DateTime CDate(object input)
        {
            if (!Convert.IsDBNull(input) && !object.Equals(input, null))
            {
                return CDate(input.ToString());
            }
            return DateTime.Now;
        }

        /// <summary>
        /// 时间类型转化
        /// </summary>
        /// <param name="input">转化失败返回当前时间,否则返回输入时间</param>
        /// <returns>返回时间</returns>
        public static DateTime CDate(string input)
        {
            DateTime now;
            if (!DateTime.TryParse(input, out now))
            {
                now = DateTime.Now;
            }
            return now;
        }
        #endregion

        #region decimal类型的转化
        /// <summary>
        /// decimal类项转化
        /// </summary>
        /// <param name="input">空则输出0,否则输出输入值</param>
        /// <returns>返回decimal类型</returns>
        public static decimal CDecimal(object input)
        {
            if (!Convert.IsDBNull(input) && !object.Equals(input, null))
            {
                return CDecimal(input.ToString());
            }
            return 0M;
        }
        /// <summary>
        /// Decimal类型转化
        /// </summary>
        /// <param name="input">字符串类型</param>
        /// <returns>deckmal类型</returns>
        public static decimal CDecimal(string input)
        {
            decimal num;
            decimal.TryParse(input, out num);
            return num;
        }

        /// <summary>
        /// Decimal类型转化
        /// </summary>
        /// <param name="input">转化失败返回后一个默认值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>decimal类型</returns>
        public static decimal CDecimal(string input, decimal defaultValue)
        {
            decimal num;
            if (decimal.TryParse(input, out num))
            {
                return num;
            }
            return defaultValue;
        }


        #endregion

        #region double类型的转化
        /// <summary>
        /// double类型转化
        /// </summary>
        /// <param name="input">空则返回0.0,否则返回输入值</param>
        /// <returns>double类型</returns>
        public static double CDouble(object input)
        {
            if (!Convert.IsDBNull(input) && !object.Equals(input, null))
            {
                return CDouble(input.ToString());
            }
            return 0.0;
        }

        /// <summary>
        /// double类型转化
        /// </summary>
        /// <param name="input">字符串类型的double数字</param>
        /// <returns>double类型</returns>
        public static double CDouble(string input)
        {
            double num;
            double.TryParse(input, out num);
            return num;
        }

        /// <summary>
        /// double类型的转化
        /// </summary>
        /// <param name="input">object</param>
        /// <param name="defaultValue">定义返回默认的值</param>
        /// <returns></returns>
        public static double CDouble(object input, double defaultValue)
        {
            double returnValue = CDouble(input);
            if (returnValue == 0.0)
            {
                return defaultValue;
            }
            return returnValue;
        }

        /// <summary>
        /// decimal类项转化-保留二位小数----------------kennyadd
        /// </summary>
        /// <param name="input">空则输出0.00,否则输出输入值</param>
        /// <returns>返回decimal类型</returns>
        public static string CDouble2(object input)
        {
            if (!Convert.IsDBNull(input) && !object.Equals(input, null))
            {
                return CDouble(input.ToString()).ToString("F2");
                ;
            }
            return "0.00";
        }
        #endregion

        #region float类型的转化
        /// <summary>
        /// float类型转化
        /// </summary>
        /// <param name="input">为空返回0F,否则返回输入值</param>
        /// <returns>float类型</returns>
        public static float CFloat(object input)
        {
            if (!Convert.IsDBNull(input) && !object.Equals(input, null))
            {
                return CFloat(input.ToString());
            }
            return 0f;
        }

        /// <summary>
        /// float类型转化
        /// </summary>
        /// <param name="input">字符串类型得float数字</param>
        /// <returns>float类型</returns>
        public static float CFloat(string input)
        {
            float num;
            float.TryParse(input, out num);
            return num;
        }

        /// <summary>
        /// float类型
        /// </summary>
        /// <param name="input"></param>
        /// <param name="defaultValue">定义返回的值</param>
        /// <returns></returns>
        public static float CFloat(object input, float defaultValue)
        {
            float returnValue = CFloat(input);
            if (returnValue == 0f)
            {
                return defaultValue;
            }
            return returnValue;
        }
        #endregion

        #region int类型的转化
        /// <summary>
        /// 整型类型转化
        /// </summary>
        /// <param name="input">为空返回0,否则返回输入值</param>
        /// <returns>整型类型</returns>
        public static int CLng(object input)
        {
            if (!Convert.IsDBNull(input) && !object.Equals(input, null))
            {
                return CLng(input.ToString());
            }
            return 0;
        }

        /// <summary>
        /// 整型类型转化
        /// </summary>
        /// <param name="input">字符类型整数</param>
        /// <returns>整型类型</returns>
        public static int CLng(string input)
        {
            int num;
            int.TryParse(input, out num);
            return num;
        }
        /// <summary>
        /// 整型类型转化
        /// </summary>
        /// <param name="input">转化失败返回后一个默认值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>整型类型</returns>
        public static int CLng(object input, int defaultValue)
        {
            if (!Convert.IsDBNull(input) && !object.Equals(input, null))
            {
                return CLng(input.ToString(), defaultValue);
            }
            return defaultValue;
        }

        /// <summary>
        /// Decimal类型转化
        /// </summary>
        /// <param name="input">转化失败返回后一个默认值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>整型类型</returns>
        public static int CLng(string input, int defaultValue)
        {
            int num;
            if (int.TryParse(input, out num))
            {
                return num;
            }
            return defaultValue;
        }
        #endregion

        #region short类型转化
        /// <summary>
        /// 短整型类型转化
        /// </summary>
        /// <param name="input">为空返回0,否则返回输入值</param>
        /// <returns>整型类型</returns>
        public static short CShort(object input)
        {
            if (!Convert.IsDBNull(input) && !object.Equals(input, null))
            {
                return CShort(input.ToString());
            }
            return 0;
        }

        /// <summary>
        /// 短整型类型转化
        /// </summary>
        /// <param name="input">字符类型整数</param>
        /// <returns>整型类型</returns>
        public static short CShort(string input)
        {
            short num;
            short.TryParse(input, out num);
            return num;
        }

        /// <summary>
        /// 短整型类型转化
        /// </summary>
        /// <param name="input">转化失败返回后一个默认值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>整型类型</returns>
        public static short CShort(object input, short defaultValue)
        {
            if (!Convert.IsDBNull(input) && !object.Equals(input, null))
            {
                return CShort(input.ToString(), defaultValue);
            }
            return defaultValue;
        }

        /// <summary>
        /// 短整型类型转化
        /// </summary>
        /// <param name="input">转化失败返回后一个默认值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>整型类型</returns>
        public static short CShort(string input, short defaultValue)
        {
            short num;
            if (short.TryParse(input, out num))
            {
                return num;
            }
            return defaultValue;
        }
        #endregion


        /// <summary>
        /// 字符串如果操过指定长度则将超出的部分用指定字符串代替
        /// </summary>
        /// <param name="p_SrcString">要检查的字符串</param>
        /// <param name="p_Length">指定长度</param>
        /// <param name="p_TailString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string GetSubString(string p_SrcString, int p_Length, string p_TailString)
        {
            return GetSubString(p_SrcString, 0, p_Length, p_TailString);
        }


        /// <summary>
        /// 取指定长度的字符串
        /// </summary>
        /// <param name="p_SrcString">要检查的字符串</param>
        /// <param name="p_StartIndex">起始位置</param>
        /// <param name="p_Length">指定长度</param>
        /// <param name="p_TailString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string GetSubString(string p_SrcString, int p_StartIndex, int p_Length, string p_TailString)
        {
            string myResult = p_SrcString;

            //当是日文或韩文时(注:中文的范围:\u4e00 - \u9fa5, 日文在\u0800 - \u4e00, 韩文为\xAC00-\xD7A3)
            if (System.Text.RegularExpressions.Regex.IsMatch(p_SrcString, "[\u0800-\u4e00]+") ||
                System.Text.RegularExpressions.Regex.IsMatch(p_SrcString, "[\xAC00-\xD7A3]+"))
            {
                //当截取的起始位置超出字段串长度时
                if (p_StartIndex >= p_SrcString.Length)
                {
                    return "";
                }
                else
                {
                    return p_SrcString.Substring(p_StartIndex,
                                                   ((p_Length + p_StartIndex) > p_SrcString.Length) ? (p_SrcString.Length - p_StartIndex) : p_Length);
                }
            }

            if (p_Length >= 0)
            {
                byte[] bsSrcString = Encoding.Default.GetBytes(p_SrcString);

                //当字符串长度大于起始位置
                if (bsSrcString.Length > p_StartIndex)
                {
                    int p_EndIndex = bsSrcString.Length;

                    //当要截取的长度在字符串的有效长度范围内
                    if (bsSrcString.Length > (p_StartIndex + p_Length))
                    {
                        p_EndIndex = p_Length + p_StartIndex;
                    }
                    else
                    {   //当不在有效范围内时,只取到字符串的结尾

                        p_Length = bsSrcString.Length - p_StartIndex;
                        p_TailString = "";
                    }
                    int nRealLength = p_Length;
                    int[] anResultFlag = new int[p_Length];
                    byte[] bsResult = null;

                    int nFlag = 0;
                    for (int i = p_StartIndex; i < p_EndIndex; i++)
                    {

                        if (bsSrcString[i] > 127)
                        {
                            nFlag++;
                            if (nFlag == 3)
                            {
                                nFlag = 1;
                            }
                        }
                        else
                        {
                            nFlag = 0;
                        }

                        anResultFlag[i] = nFlag;
                    }

                    if ((bsSrcString[p_EndIndex - 1] > 127) && (anResultFlag[p_Length - 1] == 1))
                    {
                        nRealLength = p_Length + 1;
                    }

                    bsResult = new byte[nRealLength];

                    Array.Copy(bsSrcString, p_StartIndex, bsResult, 0, nRealLength);

                    myResult = Encoding.Default.GetString(bsResult);

                    myResult = myResult + p_TailString;
                }
            }
            return myResult;
        }

        /// <summary>
        /// 自定义的替换字符串函数
        /// </summary>
        public static string ReplaceString(string SourceString, string SearchString, string ReplaceString, bool IsCaseInsensetive)
        {
            return Regex.Replace(SourceString, Regex.Escape(SearchString), ReplaceString, IsCaseInsensetive ? RegexOptions.IgnoreCase : RegexOptions.None);
        }
        /// <summary>
        /// 将全角数字转换为数字
        /// </summary>
        /// <param name="SBCCase"></param>
        /// <returns></returns>
        public static string SBCCaseToNumberic(string SBCCase)
        {
            char[] c = SBCCase.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                byte[] b = System.Text.Encoding.Unicode.GetBytes(c, i, 1);
                if (b.Length == 2)
                {
                    if (b[1] == 255)
                    {
                        b[0] = (byte)(b[0] + 32);
                        b[1] = 0;
                        c[i] = System.Text.Encoding.Unicode.GetChars(b)[0];
                    }
                }
            }
            return new string(c);
        }

        /// <summary>
        /// 将字符串转换为Color
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Color ToColor(string color)
        {
            int red, green, blue = 0;
            char[] rgb;
            color = color.TrimStart('#');
            color = Regex.Replace(color.ToLower(), "[g-zG-Z]", "");
            switch (color.Length)
            {
                case 3:
                    rgb = color.ToCharArray();
                    red = Convert.ToInt32(rgb[0].ToString() + rgb[0].ToString(), 16);
                    green = Convert.ToInt32(rgb[1].ToString() + rgb[1].ToString(), 16);
                    blue = Convert.ToInt32(rgb[2].ToString() + rgb[2].ToString(), 16);
                    return Color.FromArgb(red, green, blue);
                case 6:
                    rgb = color.ToCharArray();
                    red = Convert.ToInt32(rgb[0].ToString() + rgb[1].ToString(), 16);
                    green = Convert.ToInt32(rgb[2].ToString() + rgb[3].ToString(), 16);
                    blue = Convert.ToInt32(rgb[4].ToString() + rgb[5].ToString(), 16);
                    return Color.FromArgb(red, green, blue);
                default:
                    return Color.FromName(color);

            }
        }
       

        public static string ChangeLatLon(string str)
        {
            double dou = DataConverter.CDouble(str);
            double changeDou = dou / 1000000;
            double douDegree = (changeDou - Math.Floor(changeDou)) * 100 / 60;
            double finnally = Math.Floor(changeDou) + douDegree;
            return finnally.ToString("f5");
        }

        public static long CIpToNum(string ip)
        {
            long ipNum = 0;
            if (!DataValidator.IsIP(ip))
            {
                ipNum = 0;
            }
            else
            {
                string[] ipArr = ip.Split('.');
                ipNum = CLng((ipArr[0])) * 16777216 + CLng(ipArr[1]) * 65536 + CLng(ipArr[2]) * 256 + CLng(ipArr[3]);
            }
            return ipNum;
        }
    }
}
