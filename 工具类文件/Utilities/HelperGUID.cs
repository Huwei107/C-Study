//-----------------------------------------------------------------------
// <copyright company="工品一号" file="GUIDHelper.cs">
//  Copyright (c)  V1.0.0.0  
//  创建作者:   刘少林
//  创建时间:   2017-2-13 16:57:03
//  功能描述:   自定义GUID值类
//  历史版本:
//          2017-2-13 16:57:03 刘少林 创建GUIDHelper类
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace FX.MainForms
{
    /// <summary>
    /// 自定义GUID值类
    /// </summary>
    public class HelperGUID
    {
        /// <summary>
        /// 获取获取自定义内部自定义GUID值
        /// </summary>
        /// <returns></returns>
        public static string GetGuid()
        {
            string guid = Guid.NewGuid().ToString();
            string prefix = DateTime.Now.ToString("yyMMddhhmmss");
            return prefix + guid;
        }

    }//end class
}//end namespace
