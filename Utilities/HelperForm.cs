//-----------------------------------------------------------------------
// <copyright company="工品一号" file="FormHelper.cs">
//  Copyright (c)  V1.0.0.0  
//  创建作者:   刘少林
//  创建时间:   2018-03-29
//  功能描述:   FormHelper控件帮助类
//  历史版本:
//          2018-03-29 刘少林 创建FormHelper.cs
// </copyright>
//-----------------------------------------------------------------------
using FX.Entity;
using FX.ORM.Websharp.ORM.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;

namespace FX.MainForms
{
    /// <summary>
    ///  FormHelper控件帮助类
    /// </summary>
    public class HelperForm
    {
        /// <summary>
        /// 设置窗体底部状态条显示内容
        /// </summary>
        /// <param name="frm">窗体对象</param>
        /// <param name="msg">显示内容</param>
        /// <param name="position">显示Label索引,1:显示第一个label,2：显示在第二个Label</param>
        public static void SetExecutableBottomStatus(frmBase frm, string msg, byte position = 2)
        {
#warning 这个地方为了代码编译通过，临时注销了代码，一定要记得恢复！
            //if (position == 2)
            //{
            //    frm.lblStatus2.Text = msg;
            //}
            //else if (position == 1)
            //{
            //    frm.lblStatus1.Text = msg;
            //}
        }

        private static StringBuilder container = new StringBuilder(128);

        /// <summary>
        /// 设置窗体底部状态条显示记录状态被修改日志描述
        /// </summary>
        /// <param name="frm">窗体对象</param>
        /// <param name="status">修改审核状态</param>
        /// <param name="position">显示Label索引,1:显示第一个label,2：显示在第二个Label</param>
        public static void SetExecutableBottomStatus(frmBase frm, Enum status, byte position = 2)
        {
            container.Remove(0, container.Length);
            container.AppendFormat("操作时间：{0:20}，", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
            container.AppendFormat("操作人：{0:10}，", PersistenceGlobalData.CurrentUser.Name);
            container.AppendFormat("状态被设置为：{0:10}，", HelperEnum.GetEnumDesc(status));
            SetExecutableBottomStatus(frm, container.ToString(), position);
        }

    }
}
