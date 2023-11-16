//-----------------------------------------------------------------------
// <copyright company="工品一号" file="NumberHelper.cs">
//  Copyright (c)  V1.0.0.0  
//  创建作者:   刘少林
//  创建时间:   2017-2-13 18:11:36
//  功能描述:   数字操作集合(常用操作数字方法)
//  历史版本:
//          2017-2-13 18:11:36 刘少林 创建NumberHelper类
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace FX.MainForms
{
    /// <summary>
    /// 数字操作集合(常用操作数字方法)
    /// </summary>
    public class HelperNumber
    {
        /// <summary>
        /// 数值格式正则表达对象(不包括科学计数法,以及程序中数值格式[0.1f,12M等])
        /// </summary>
        private static readonly Regex NumReg = new Regex(RegexFormat.NumberRegexFormat);

        /// <summary>
        /// 判断是否是数字(0,整数,浮点型以及正负数字)
        /// </summary>
        /// <param name="num">待判断字符串</param>
        /// <returns>布尔值,false非数字格式,true数字格式</returns>
        public static bool IsNumberFormat(string num)
        {
            bool d = NumReg.IsMatch(num.Trim());
            return NumReg.IsMatch(num.Trim());

        } //end method

        /// <summary>
        /// 判断是否正数
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static bool IsPositiveNumber(string num)
        {
            Regex regNum = new Regex(@"^[+]{0,1}(\d+)$|^[+]{0,1}(\d+\.\d+)$");
            return regNum.IsMatch(num.Trim());
        }

        /// <summary>
        /// 数字在0-1之间的2位小数，包含1
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static bool IsOToOneNumber(string num)
        {
            Regex regNum = new Regex(@"^(0(\.\d{1,2})?|1(\.0{1,2})?)$");
            return regNum.IsMatch(num.Trim());
        }
        /// <summary>
        /// 判断是否整数
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static bool IsIntegerNumber(string num)
        {
            Regex regNum = new Regex("^-?\\d+$");
            return regNum.IsMatch(num.Trim());
        }

        /// <summary>
        /// 判断是否正整数
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static bool IsPositiveInteger(string num)
        {
            Regex regNum = new Regex(@"^([0-9]{1,})$");
            return regNum.IsMatch(num.Trim());
        }

        /// <summary>
        /// 判断字符串是否是整数字符串
        /// </summary>
        /// <param name="num">待判断字符串</param>
        /// <returns>布尔值,false非整数格式,true整数格式</returns>
        public static bool IsIntegerFormat(string num)
        {
            return IsNumberFormat(num) && num.IndexOf('.') < 0;

        }//end method

        /// <summary>
        /// 判断字符串是否是浮点型数字符串
        /// (此浮点型为带小数点数字,实际上整数也是浮点型数值的一种)
        /// </summary>
        /// <param name="num">待判断字符串</param>
        /// <returns>布尔值,false非浮点型格式,true浮点型格式</returns>
        public static bool IsFloatFormat(string num)
        {
            return IsNumberFormat(num) && num.IndexOf('.') > 0;
        }//end method

        /// <summary>
        /// 判断字符串是否是包含英文逗号
        /// </summary>
        /// <param name="num">待判断字符串</param>
        /// <returns>布尔值,false,true</returns>
        public static bool IsContainComma(string num)
        {
            Regex reg = new Regex(@"^[,]+$");
            return reg.IsMatch(num.Trim());

        }//end method

        /// <summary>
        /// 获取两个数相除后的商和余数
        /// </summary>
        /// <param name="dividend">被除数</param>
        /// <param name="divisor">除数</param>
        /// <returns>decimal[0]:商,decimal[1]:余数</returns>
        public static decimal[] divideAndRemainder(decimal dividend, decimal divisor)
        {
            decimal[] result = new decimal[2];
            if (dividend != 0 && divisor != 0)
            {
                result[0] = Math.Floor(dividend / divisor);
                result[1] = dividend % divisor;
            }
            else
            {
                result[0] = 0;
                result[1] = 0;
            }           
            return result;
        }



    }//end class

}//end namespace
