//-----------------------------------------------------------------------
// <copyright company="工品一号" file="PageRequest.cs">
//  Copyright (c)  V1.0.0.0  
//  创建作者:   王健
//  创建时间:   2023/8/9 11:15:44 
//  功能描述:   
//  历史版本:
//          2023/8/9 11:15:44 王健 创建PageRequest类
// </copyright>
//-----------------------------------------------------------------------

namespace FX.MainForms.Common.Response
{
    /// <summary>
    /// 分页请求基类
    /// </summary>
    public class PageRequest
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页数量
        /// </summary>
        public int PageSize { get; set; }
    }
}
