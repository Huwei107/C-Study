//-----------------------------------------------------------------------
// <copyright company="工品一号" file="URLHelper.cs">
//  Copyright (c)  V1.0.0.0  
//  创建作者:   刘少林
//  创建时间:   2017-2-14 8:42:17
//  功能描述:   URL地址操作常用方法集
//  历史版本:
//          2017-2-14 8:42:17 刘少林 创建URLHelper类
// </copyright>
//-----------------------------------------------------------------------

using System.Text.RegularExpressions;
namespace FX.MainForms
{
    /// <summary>
    /// URL地址操作常用方法集
    /// </summary>
    public class HelperURL
    {
        /// <summary>
        /// 获取指定URL参数格式是否正确(只检测常规URL[http(s)形式地址])
        /// </summary>
        /// <param name="url">待检测URL地址</param>
        /// <returns>布尔值,true格式正确,false格式不正确</returns>
        public static bool IsWellFormattedNormalUrl(string url)
        {
            Regex r = new Regex(RegexFormat.NormalUrlRegexFormat, RegexOptions.IgnoreCase);
            if (url != null)
            {
                return r.IsMatch(url.Trim());
            }
            else
            {
                return false;
            }
        }//end method
    }//end class
}//end namespace
