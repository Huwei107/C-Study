//-----------------------------------------------------------------------
// <copyright company="工品一号" file="ChineseCardIdHelper.cs">
//  Copyright (c)  V1.0.0.0  
//  创建作者:   刘少林
//  创建时间:   2017-2-14 8:36:18
//  功能描述:   中国身份证号码帮助类
//  历史版本:
//          2017-5-16 8:36:18 刘少林 创建ChineseCardIdHelper类
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;

namespace FX.MainForms
{
    /// <summary>
    /// 中国身份证号码帮助类
    /// </summary>
    public class HelperChineseCardId
    {
        /// <summary>
        /// 获取身份证对应的生日日期
        /// </summary>
        /// <param name="cardId">身份证号码</param>
        /// <returns>生日日期</returns>
        public static DateTime GetBirthday(string cardId)
        {
            string val = GetValue(cardId, 1);
            return DateTime.Parse(val);
        }

        /// <summary>
        /// 获取身份证对应的性别(1:女性,2:男性)
        /// </summary>
        /// <param name="cardId">身份证号码</param>
        /// <remarks>1:女性,2:男性</remarks>
        /// <returns>性别标志</returns>
        public static int GetGender(string cardId)
        {
            string val = GetValue(cardId, 2);
            return int.Parse(val);
        }

        /// <summary>
        /// 获取身份证特殊数据
        /// </summary>
        /// <param name="chardId">身份证号码</param>
        /// <param name="valueFlag">获取值标志,1:生日,2:性别</param>
        /// <returns></returns>
        private static string GetValue(string chardId, byte valueFlag = 0)
        {
            string birthday = string.Empty;
            string sex = string.Empty;
            if (chardId.Length == 18)//处理18位的身份证号码从号码中得到生日和性别代码
            {
                birthday = chardId.Substring(6, 4) + "-" + chardId.Substring(10, 2) + "-" + chardId.Substring(12, 2);
                sex = chardId.Substring(14, 3);
            }
            if (chardId.Length == 15)
            {
                birthday = "19" + chardId.Substring(6, 2) + "-" + chardId.Substring(8, 2) + "-" + chardId.Substring(10, 2);
                sex = chardId.Substring(12, 3);
            }
            if (valueFlag == 1)
            {
                return birthday;
            }
            else if (valueFlag == 2)
            {
                if (int.Parse(sex) % 2 == 0)//性别代码为偶数是女性奇数为男性
                {
                    return "1";
                }
                else
                {
                    return "2";
                }
            }
            return string.Empty;
        }
    }
}
