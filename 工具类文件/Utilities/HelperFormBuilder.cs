using FX.MainForms.Public.Forms;
//-----------------------------------------------------------------------
// <copyright company="工品一号" file="FormBuilderHelper.cs">
//  Copyright (c)  V1.0.0.0  
//  创建作者:   刘少林
//  创建时间:   2018-03-29
//  功能描述:   窗体创建帮助类
//  历史版本:
//          2018-03-29 刘少林 创建FormBuilderHelper.cs
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace FX.MainForms
{
    /// <summary>
    /// 窗体创建帮助类
    /// </summary>
    public class HelperFormBuilder
    {
        /// <summary>
        /// 创建窗体
        /// </summary>
        /// <param name="instanceSrc">窗体全路径</param>
        /// <param name="formName">窗体名称</param>
        /// <param name="caption">窗体标题</param>
        /// <param name="isOpen">是否打开新窗体</param>
        /// <returns></returns>
        public static Form CreateFormInstance(string instanceSrc, string formName, string caption, out bool isOpen)
        {
            isOpen = true;
            foreach (Form form in Application.OpenForms)
            {
                if (form.Name != formName)
                    continue;
                if (caption == form.Text)
                {
                    form.Activate();
                    isOpen = false;
                    return form;
                }
            }
            System.Reflection.Assembly _assembly = System.Reflection.Assembly.GetExecutingAssembly();
            return (Form)_assembly.CreateInstance(instanceSrc);
        }

        /// <summary>
        /// show出新的窗口
        /// </summary>
        /// <param name="frm">跳转的界面</param>
        /// <param name="tag">用于窗体传递参数对象</param>
        /// <param name="caption">界面名称</param>
        /// <param name="getSystemShow">true:读配置文件(弹层窗体模式,读取配置文件)，false:单独弹层窗体，系统默认方式展示窗体</param>
        /// <param name="isDialogMode">是否ShowDialog模式,默认启动!</param>
        public static void ShowForm(Form frm, object tag, string caption, bool getSystemShow = true, bool isDialogMode = false)
        {
            //如果是单独弹层窗体，则启用showDialog模式
            //if (getSystemShow)
            //{
            //    isDialogMode = true;
            //}

            frm.Text = caption;
            frm.Tag = tag;
            if (File.Exists(Application.StartupPath + "\\ico.ico"))
            {
                frm.Icon = new System.Drawing.Icon(System.Windows.Forms.Application.StartupPath + "\\ico.ico");
            }
            if (getSystemShow)//默认读系统配置的跳转界面
            {

            }
            else
            {
                frm.FormBorderStyle = FormBorderStyle.FixedDialog;
            }
            //@@201712131713bylsl
            //frm.MaximizeBox = false;
            if (frm is frmException)
            {
                //frm.ShowDialog();  //7-22注释
                //没配置权限登录异常
                //HelperMessageBoxContent.ShowMessageOK("请配置相关权限!");
            }
            else
            {
                if (!isDialogMode)
                {
                    frm.Show();
                }
                else
                {
                    frm.ShowDialog();
                }

            }
        }

        /// <summary>
        /// 右下角提示框
        /// </summary>
        /// <param name="frm">需要弹框的主窗口</param>
        /// <param name="msg">提示信息</param>
        public static void ShowMsgForm(Form frm, string msg)
        {
            ShowMsgForm(frm, msg, 2000);
        }

        /// <summary>
        /// 右下角提示框
        /// </summary>
        /// <param name="frm">需要弹框的主窗口</param>
        /// <param name="msg">提示信息</param>
        /// <param name="msg">悬浮停顿的毫秒数</param>
        public static void ShowMsgForm(Form frm, string msg, int times)
        {
            frmShowMsg showMsg = new frmShowMsg(msg);
            showMsg.Left = frm.Bounds.X + frm.Width - showMsg.Width - 23;
            showMsg.Top = frm.Bounds.Y + frm.Height - showMsg.Height - 23;
            showMsg.Show();
        }
    }
}
