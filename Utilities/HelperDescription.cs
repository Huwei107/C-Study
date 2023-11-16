//-----------------------------------------------------------------------
// <copyright company="工品一号" file="HelperDescription.cs">
//  Copyright (c)  V1.0.0.0  
//  创建作者:   王健
//  创建时间:   2023/10/8 15:20:50 
//  功能描述:   
//  历史版本:
//          2023/10/8 15:20:50 王健 创建HelperDescription类
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace FX.MainForms
{
    /// <summary>
    /// 描述工具类
    /// </summary>
    public static class HelperDescription
    {
        /// <summary>
        /// 获取字段描述
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetFieldDescription(this string name ,Type type)
        {
            var fieldInfo = type.GetField(name);
            if (fieldInfo == null) return string.Empty;
            var attributes = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes == null || !attributes.Any()) return string.Empty;
            var description = attributes.First() as DescriptionAttribute;
            return description.Description;
        }


        /// <summary>
        /// 获取属性描述
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetPropertyDescription(this string name, Type type)
        {
            var propertyInfo = type.GetProperty(name);
            if (propertyInfo == null) return string.Empty;
            var attributes = propertyInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes == null || !attributes.Any()) return string.Empty;
            var description = attributes.First() as DescriptionAttribute;
            return description.Description;
        }
    }
}
