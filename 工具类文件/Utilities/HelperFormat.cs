//-----------------------------------------------------------------------
// <copyright company="工品一号" file="FormatHelper.cs">
//  Copyright (c)  V1.0.0.0  
//  创建作者:   刘少林
//  创建时间:   2017-2-13 16:54:46
//  功能描述:   格式帮助类
//  历史版本:
//          2017-2-13 16:54:46 刘少林 创建FormatHelper类
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace FX.MainForms
{
    /// <summary>
    /// 格式帮助类
    /// </summary>
    public class HelperFormat
    {

        /// <summary>
        /// 检测给定参数是否是普通URL地址(HTTP地址)
        /// </summary>
        /// <param name="url">url地址</param>
        /// <returns>布尔值,true是普通URL地址,false不是普通URL地址</returns>
        public static bool IsUrl(string url)
        {
            return HelperURL.IsWellFormattedNormalUrl(url);

        }//end method

        /// <summary>
        /// 中国手机号码格式检测
        /// </summary>
        /// <param name="mobile">手机号码</param>
        /// <returns>布尔值,true为手机号码格式,false则不是</returns>
        public static bool IsMobile(string mobile)
        {
            Regex mobilReg = new Regex(RegexFormat.MobileRegexFormat);
            return mobilReg.IsMatch(mobile.Trim());

        }//end method

        /// <summary>
        /// 检测中国固定号码正则格式(包含传真号码)
        /// </summary>
        /// <param name="telphone">固定号码</param>
        /// <returns>布尔值,true为固定号码格式,false则不是</returns>
        public static bool IsTelphone(string telphone)
        {
            Regex faxReg = new Regex(RegexFormat.TelphoneRegexFormat);
            return faxReg.IsMatch(telphone.Trim());

        }//end method

        /// <summary>
        /// 检测中国邮政编码格式
        /// </summary>
        /// <param name="zipCode">邮政编码</param>
        /// <returns>布尔值,true为邮政编码格式,false则不是</returns>
        public static bool IsZipCode(string zipCode)
        {
            Regex zipReg = new Regex(RegexFormat.ZipCodeRegexFormat);
            return zipReg.IsMatch(zipCode.Trim());

        }//end method

        /// <summary>
        /// 检测电子邮件地址格式
        /// </summary>
        /// <param name="email">电子邮件地址</param>
        /// <returns>布尔值,true为电子邮件地址格式,false则不是</returns>
        public static bool IsEmail(string email)
        {
            Regex mailReg = new Regex(RegexFormat.EmailRegexFormat);
            return mailReg.IsMatch(email.Trim());

        }//end method

        /// <summary>
        /// 验证15位身份证号
        /// </summary>
        /// <param name="id">身份证号</param>
        /// <returns>验证成功为True，否则为False</returns>
        private static bool CheckIDCard18(string id)
        {
            long n = 0;
            if (long.TryParse(id.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(id.Replace('x', '0').Replace('X', '0'), out n) == false)
            {
                return false;//数字验证
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(id.Remove(2)) == -1)
            {
                return false;//省份验证
            }
            string birth = id.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证
            }
            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] Ai = id.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
            }
            int y = -1;
            Math.DivRem(sum, 11, out y);
            if (arrVarifyCode[y] != id.Substring(17, 1).ToLower())
            {
                return false;//校验码验证
            }
            return true;//符合GB11643-1999标准
        }
        /// <summary>
        /// 验证18位身份证号
        /// </summary>
        /// <param name="id">身份证号</param>
        /// <returns>验证成功为True，否则为False</returns>
        private static bool CheckIDCard15(string id)
        {
            long n = 0;
            if (long.TryParse(id, out n) == false || n < Math.Pow(10, 14))
            {
                return false;//数字验证
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(id.Remove(2)) == -1)
            {
                return false;//省份验证
            }
            string birth = id.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证
            }
            return true;//符合15位身份证标准
        }

        /// <summary>
        /// 检测中国身份证号码格式
        /// </summary>
        /// <param name="cardId">身份证号码</param>
        /// <returns>布尔值,true为身份证号码格式,false则不是</returns>
        public static bool IsIdentityCard(string cardId)
        {
            string sid = cardId.Trim();
            if (sid.Length == 18)
            {
                return CheckIDCard18(sid);
            }
            else if (sid.Length == 15)
            {
                return CheckIDCard15(sid);
            }
            else
            {
                return false;
            }
        }//end method

        /// <summary>
        /// 判断是否是数字(0,整数,浮点型以及正负数字)
        /// </summary>
        /// <param name="number">待检测字符串</param>
        /// <returns>布尔值,true为数值格式,false则不是</returns>
        public static bool IsNumber(string number)
        {
            return HelperNumber.IsNumberFormat(number);
        }//end method

        /// <summary>
        /// 检测是否是IPV4地址格式
        /// </summary>
        /// <param name="ipv4">IP地址</param>
        /// <returns>布尔值,true为ip地址格式,false则不是</returns>
        public static bool IsIP(string ip)
        {
            return HelperIP.IsIPv4(ip);
        }//end method

        /// <summary>
        /// 检测汉字(可检测多个汉字)
        /// </summary>
        /// <param name="chinese">待检测字符串</param>
        /// <returns>布尔值,true为汉字格式,false则不是</returns>
        public static bool IsChinese(string chinese)
        {
            Regex chineseReg = new Regex(RegexFormat.ChineseRegexFormat);
            return chineseReg.IsMatch(chinese.Trim());

        }//end method

        /// <summary>
        /// 检测普通账号正则格式
        /// (长度为1-50,只包含下划线,数字,字母,其中各数量和顺序无限制)
        /// </summary>
        /// <param name="account">账号</param>
        /// <returns>布尔值,true为普通账号格式,false则不是</returns>
        public static bool IsNormalAccount(string account)
        {
            Regex accountReg = new Regex(RegexFormat.NormalAccountRegexFormat);
            return accountReg.IsMatch(account.Trim());

        }//end method

    }

    /// <summary>
    /// 常规格式正则格式集合
    /// </summary>
    public class RegexFormat
    {
        /// <summary>
        /// 数值格式(不包括科学计数法,以及程序中数值格式[0.1f,12M等])
        /// </summary>
        public const string NumberRegexFormat = @"^(\+|\-)?(0|[1-9]\d*)?(\.\d+)?$";

        /// <summary>
        /// 普通URL正则格式(像ftp等特殊URL请求地址此正则格式不提供匹配支持)
        /// </summary>
        public const string NormalUrlRegexFormat = @"^(ftp|https?)://([\w-]+\.)+[\w-]+(/[\w-\./?%=#]*)?$";

        /// <summary>
        /// 中国手机号码正则格式
        /// </summary>
        public const string MobileRegexFormat = @"^0?(13[0-9]|15[012356789]|17[0-9]|18[0-9]|14[357])[0-9]{8}$";

        /// <summary>
        /// 中国固定号码正则格式(包含传真号码)
        /// </summary>
        public const string TelphoneRegexFormat = @"^([0][1-9]\d{2,3}\-{0,1})?[1-9]\d{6,7}$";

        /// <summary>
        /// 中国邮政编码正则格式
        /// </summary>
        public const string ZipCodeRegexFormat = @"^[1-9]\d{5}(?!\d)$";

        /// <summary>
        /// 电子邮件地址正则格式
        /// </summary>
        public const string EmailRegexFormat = @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

        /// <summary>
        /// IPv4地址正则格式
        /// </summary>
        public const string IPv4RegexFormat = @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$";

        /// <summary>
        /// 汉字正则格式(可检测多个汉字)
        /// </summary>
        public const string ChineseRegexFormat = @"^[\u4e00-\u9fa5]+$";

        /// <summary>
        /// 普通账号正则格式
        /// </summary>
        public const string NormalAccountRegexFormat = @"^[_a-zA-Z0-9]{1,50}$";

        /// <summary>
        /// 普通时间正则格式
        /// </summary>
        public const string NormalTimeRegexFormat = @"^(((((1[6-9]|[2-9]\d)?\d{2})[\.\-\/](0?[13578]|1[02])[\.\-\/](0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)?\d{2})[\.\-\/](0?[13456789]|1[012])[\.\-\/](0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)?\d{2})[\.\-\/]0?2[\.\-\/](0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)[\.\-\/]0?2[\.\-\/]29))|((((1[6-9]|[2-9]\d)?\d{2})(0[13578]|1[02])(0[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)?\d{2})(0[13456789]|1[012])(0[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)?\d{2})02(0[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)0229)))$";
    }
}


