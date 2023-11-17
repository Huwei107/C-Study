using FX.Entity;
using FX.MainForms.Public.Forms;
using FX.ORM.Websharp.ORM.Base;
//-----------------------------------------------------------------------
// <copyright company="工品一号" file="ExceptionHelper.cs">
//  Copyright (c)  V1.0.0.0  
//  创建作者:   刘少林
//  创建时间:   2018-03-29
//  功能描述:   ComboBox控件帮助类
//  历史版本:
//          2018-03-29 刘少林 创建ExceptionHelper.cs
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;

namespace FX.MainForms
{
    /// <summary>
    ///  异常帮助类
    /// </summary>
    public class HelperException
    {
        /// <summary>
        /// 展示异常报错统一呈现异常窗体
        /// </summary>
        /// <param name="currentForm">发生异常的窗体</param>
        /// <param name="exception">发生异常对象</param>
        /// <param name="extendInfo">附加扩展信息描述</param>
        /// <returns>frmException控件对象，方便链式操作</returns>
        public static frmException ShowExceptionForm(frmBase currentForm, FXException exception, string extendInfo = "")
        {
            frmException exceptionForm = new frmException(exception, extendInfo, currentForm);
            HelperFormBuilder.ShowForm(exceptionForm, null, currentForm.Text, true);
            return exceptionForm;
        }

        ///// <summary>
        ///// 展示异常报错统一呈现异常窗体
        ///// </summary>
        ///// <param name="currentForm">发生异常的窗体</param>
        ///// <param name="exception">发生异常对象</param>
        ///// <param name="extendInfo">附加扩展信息描述</param>
        ///// <returns>frmException控件对象，方便链式操作</returns>
        //public static frmException ShowExceptionForm(Form currentForm, FXException exception, string extendInfo = "")
        //{
        //    frmException exceptionForm = new frmException(exception, extendInfo, currentForm);
        //    HelperFormBuilder.ShowForm(exceptionForm, null, currentForm.Text, true);
        //    return exceptionForm;
        //}

        /// <summary>
        /// 展示异常报错统一呈现异常窗体
        /// </summary>
        /// <param name="caption">异常窗体标题</param>
        /// <param name="exception">发生异常对象</param>
        /// <param name="displayName">附加扩展信息描述</param>
        /// <returns>extendInfo控件对象，方便链式操作</returns>
        public static frmException ShowExceptionForm(string caption, FXException exception, string extendInfo = "")
        {
            frmException exceptionForm = new frmException(exception, caption, null);
            HelperFormBuilder.ShowForm(exceptionForm, null, caption, true);
            return exceptionForm;
        }
    }
}
