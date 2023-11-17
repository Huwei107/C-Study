//-----------------------------------------------------------------------
// <copyright company="工品一号" file="DataResponseBase.cs">
//  Copyright (c)  V1.0.0.0  
//  创建作者:   王健
//  创建时间:   2023/8/9 10:36:19 
//  功能描述:   
//  历史版本:
//          2023/8/9 10:36:19 王健 创建DataResponseBase类
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.MainForms.Common.Response
{
    /// <summary>
    /// 接口返回结果类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataResponseBase<T>
    {
        /// <summary>
        /// 响应编码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 响应信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 响应是否执行成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 响应数据对象
        /// </summary>
        public T Data { get; set; }
    }
}
