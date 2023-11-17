//-----------------------------------------------------------------------
// <copyright company="工品一号" file="PageResponse.cs">
//  Copyright (c)  V1.0.0.0  
//  创建作者:   王健
//  创建时间:   2023/8/9 11:16:16 
//  功能描述:   
//  历史版本:
//          2023/8/9 11:16:16 王健 创建PageResponse类
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.MainForms.Common.Response
{
    // <summary>
    /// 分页返回值基类
    /// </summary>
    public class PageResponse<T>
    {

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPage { get; set; }

        /// <summary>
        /// 总数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 数据明细
        /// </summary>
        public List<T> Items { get; set; }
    }
}
