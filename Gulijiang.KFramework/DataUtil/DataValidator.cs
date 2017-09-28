/*-----*-----*-----*-----*-----*-----*-----*-----*-----*-----*-----*-----*-----*-----*-----*-----*-----*
 文 件 名:  DataValidator
 描    述:  工具类--数据验证类.
 创 建 者:  kenny
 创建日期:  2011年02月01日
*-----*-----*-----*-----*-----*-----*-----*-----*-----*-----*-----*-----*-----*-----*-----*-----*-----*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Gulijiang.KFramework.DataUtil
{
    public class DataValidator
    {
        public static bool IsNull(object obj)
        {
            return obj != null && obj.ToString().Length > 0;
        }

        public static bool IsAreaCode(string input)
        {
            return ((IsNumber(input) && (input.Length >= 3)) && (input.Length <= 5));
        }

        public static bool IsDouble(object Expression)
        {
            if (Expression != null)
            {
                return Regex.IsMatch(Expression.ToString(), @"^([0-9])[0-9]*(\.\w*)?$");
            }
            return false;
        }


        /// <summary>
        /// 检测是否是Decimal型数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsDecimal(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            return Regex.IsMatch(input, "^[0-9]+[.]?[0-9]+$");
        }

        /// <summary>
        /// 检测是否是带符号得Decimal类型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsDecimalSign(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            return Regex.IsMatch(input, "^[+-]?[0-9]+[.]?[0-9]+$");
        }
        /// <summary>
        /// 邮件检测
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsEmail(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            return Regex.IsMatch(input, @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
        }
        /// <summary>
        /// IP检测
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsIP(string input)
        {
            return (!string.IsNullOrEmpty(input) && Regex.IsMatch(input.Trim(), @"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$"));
        }

        /// <summary>
        /// 判断对象是否为Int32类型的数字
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public static bool IsNumeric(object Expression)
        {
            if (Expression != null)
            {
                string str = Expression.ToString();
                if (str.Length > 0 && str.Length <= 11 && Regex.IsMatch(str, @"^[-]?[0-9]*[.]?[0-9]*$"))
                {
                    if ((str.Length < 10) || (str.Length == 10 && str[0] == '1') || (str.Length == 11 && str[0] == '-' && str[1] == '1'))
                    {
                        return true;
                    }
                }
            }
            return false;

        }
        /// <summary>
        /// 整数检测
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNumber(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            return Regex.IsMatch(input, "^[0-9]+$");
        }
        /// <summary>
        /// 带符号整数检测
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNumberSign(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            return Regex.IsMatch(input, "^[+-]?[0-9]+$");
        }

        public static bool IsPostCode(string input)
        {
            return (IsNumber(input) && (input.Length == 6));
        }
        /// <summary>
        /// url检测
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsUrl(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            return Regex.IsMatch(input, @"^http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$");
        }

        /// <summary>
        /// IP验证
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsValidId(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            input = input.Replace("|", "").Replace(",", "").Replace("-", "").Replace(" ", "").Trim();
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            return IsNumber(input);
        }
        /// <summary>
        /// 用户名验证.包括长度不能超过20,用户名中包含特殊字符的验证
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static bool IsValidUserName(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return false;
            }
            if (userName.Length > 50)
            {
                return false;
            }
            if (userName.Trim().Length == 0)
            {
                return false;
            }
            if (userName.Trim(new char[] { '.' }).Length == 0)
            {
                return false;
            }
            string str = "\\/\"[]:|<>+=;,?*@'";
            for (int i = 0; i < userName.Length; i++)
            {
                if (str.IndexOf(userName[i]) >= 0)
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 判断是否是车牌号码
        /// </summary>
        /// <param name="plateNum">车牌号码</param>
        /// <returns></returns>
        public static bool IsPlateNum(string plateNum)
        {
            if (string.IsNullOrEmpty(plateNum))
            {
                return false;
            }

            if (plateNum.Trim().Length < 7)
            {
                return false;
            }
            string s = plateNum[0].ToString();
            Regex rx = new Regex("^[\u4e00-\u9fa5]$");
            if (!rx.IsMatch(s))
            {
                return false;
            }
            s = plateNum[1].ToString();
            rx = new Regex("[A-Z]{1}");
            if (!rx.IsMatch(s))
            {
                return false;
            }
            s = plateNum.Substring(2).Trim();
            if (s.Length != 5)
            {
                return false;
            }
            rx = new Regex("[0-9A-Z]{5}");
            if (rx.IsMatch(s))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 身份证检测
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static bool isIdentity(string identity)
        {
            if (string.Empty == identity || "" == identity) return false;
            string regIden = @"^(\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$";
            return Regex.IsMatch(identity, regIden);
        }

        /// <summary>
        /// 手机验证
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static bool isMobilePhone(string mobile)
        {
            if (string.Empty == mobile || "" == mobile) return false;
            string regMobile = @"^(130|131|132|133|134|135|136|137|138|139|150|151|152|153|154|155|156|157|158|159|180|181|182|183|185|188|189)\d{8}$";
            return Regex.IsMatch(mobile, regMobile);
        }

        /// <summary>
        /// 验证固定电话:0000-00000000
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static bool isFixPhone(string phone)
        {
            if (string.Empty == phone || "" == phone) return false;
            string regPhone = @"^(\d{3,4}-)?\d{7,8})";
            return Regex.IsMatch(phone, regPhone);
        }

        /// <summary>
        /// 验证固定电话和手机号码
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static bool isPhone(string phone)
        {
            if (string.Empty == phone || "" == phone) return false;
            string regphone = @"^(\d{3,4}-)?\d{7,8})$|(1[3-8][0-9]{9})";
            return Regex.IsMatch(phone, regphone);
        }

        /// <summary>
        /// 判断字符串是否是时间.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool isDate(string date)
        {
            if (string.Empty == date || "" == date) return false;
            try
            {
                DateTime.Parse(date);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 检测是否是中文
        /// </summary>
        /// <param name="strCh"></param>
        /// <returns></returns>
        public static bool isCH(string strCh)
        {
            if (string.Empty == strCh || "" == strCh) return false;
            string regCH = @"[\u4e00-\u9fa5]";
            return Regex.IsMatch(strCh, regCH);
        }

        /// <summary>
        /// 返回指定IP是否在指定的IP数组所限定的范围内, IP数组内的IP地址可以使用*表示该IP段任意, 例如192.168.1.*
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="iparray"></param>
        /// <returns></returns>
        public static bool InIPArray(string ip, string[] iparray)
        {

            string[] userip = SplitString(ip, @".");
            for (int ipIndex = 0; ipIndex < iparray.Length; ipIndex++)
            {
                string[] tmpip = SplitString(iparray[ipIndex], @".");
                int r = 0;
                for (int i = 0; i < tmpip.Length; i++)
                {
                    if (tmpip[i] == "*")
                    {
                        return true;
                    }

                    if (userip.Length > i)
                    {
                        if (tmpip[i] == userip[i])
                        {
                            r++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }

                }
                if (r == 4)
                {
                    return true;
                }
            }
            return false;

        }
        /// <summary>
        /// 分割字符串
        /// </summary>
        public static string[] SplitString(string strContent, string strSplit)
        {
            if (strContent.IndexOf(strSplit) < 0)
            {
                string[] tmp = { strContent };
                return tmp;
            }
            return Regex.Split(strContent, Regex.Escape(strSplit), RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 判断文件名是否为浏览器可以直接显示的图片文件名
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>是否可以直接显示</returns>
        public static bool IsImgFilename(string filename)
        {
            filename = filename.Trim();
            if (filename.EndsWith(".") || filename.IndexOf(".") == -1)
            {
                return false;
            }
            string extname = filename.Substring(filename.LastIndexOf(".") + 1).ToLower();
            return (extname == "jpg" || extname == "jpeg" || extname == "png" || extname == "bmp" || extname == "gif");
        }

        /// <summary>
        /// 判断string数组是否存在第listindex个参数
        /// </summary>
        /// <param name="mlist">string数组</param>
        /// <param name="listindex">从零开始</param>
        /// <returns>是否存在</returns>
        public static bool IsHaveList(List<string> mlist,int listindex)
        {
            if (mlist==null)
            {
                return false;
            }
            if(mlist.Count>=listindex)
            {
                return mlist[listindex].Length > 0;

            }
            return false;
        }
    }
}
