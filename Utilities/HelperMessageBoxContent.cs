//-----------------------------------------------------------------------
// <copyright company="工品一号" file="MessageBoxContentHelper.cs">
//  Copyright (c)  V1.0.0.0  
//  创建作者:   刘少林
//  创建时间:   2018-03-29
//  功能描述:   弹出框内容处理(避免显示不好看)
//  历史版本:
//          2018-03-29 刘少林 创建MessageBoxContentHelper.cs
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace FX.MainForms
{
    /// <summary>
    /// 弹出框内容处理(避免显示不好看)
    /// </summary>
    public class HelperMessageBoxContent
    {
        /// <summary>
        /// 包装弹出消息，使它好看
        /// </summary>
        /// <param name="mes">要包装的源字符</param>
        /// <returns>包装后的</returns>
        public static string GetMessage(string mes)
        {
            if (mes.Length < 40)
            {
                string str = "";
                foreach (char var in mes)
                {
                    str += var.ToString() + "  ";
                }

                if (str.Length < 40)
                {
                    int ca = (40 - str.Length) / 2;
                    string s = str.PadLeft(ca + str.Length - 2);//-2是因为前面有图标
                    string e = s.PadRight(40);
                    return e;
                }
            }
            return mes;
        }

        #region 弹出确定对话框配置
        /// <summary>
        /// 弹出确定对话框，为了好看
        /// </summary>
        /// <param name="mes">要弹出的消息</param>
        public static void ShowMessageOK(string mes)
        {
            MessageBox.Show(HelperMessageBoxContent.GetMessage(mes), "系统提醒消息", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        /// <summary>
        /// 弹出确定对话框，为了好看
        /// </summary>
        /// <param name="mes">要弹出的消息</param>
        public static void ShowMessageOK(string mes, int flag = 0)
        {
            MessageBox.Show(flag == 0 ? HelperMessageBoxContent.GetMessage(mes) : mes, "系统提醒消息", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        /// <summary>
        /// 弹出是否对话框，为了好看
        /// </summary>
        /// <param name="mes">要弹出的消息</param>
        /// <returns>是否</returns>
        public static DialogResult ShowMessageYesNo(string mes)
        {
            return MessageBox.Show(HelperMessageBoxContent.GetMessage(mes), "系统提醒消息", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }


        /// <summary>
        /// 弹出是否对话框，为了好看
        /// </summary>
        /// <param name="mes">要弹出的消息</param>
        /// <returns>布尔类型,true:执行,不执行</returns>
        public static bool ShowMessageYesNoReturnBool(string mes)
        {
            return ShowMessageYesNo(mes) == DialogResult.Yes;
        }
        #endregion

        #region 弹出框自动关闭

        //获得这个消息对话框的窗口句柄,可以通过 FindWindow API 来查找对应的窗体句柄。
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        //获得这个消息对话框的关闭对话框,使用 EndDialog
        [DllImport("user32.dll")]
        public static extern bool EndDialog(IntPtr hDlg, out IntPtr nResult);


        /// <summary>
        /// 弹出框自动关闭(在调用MessageBox.Show 前启动一个后台工作线程，这个工作线程等待一定时间后开始查找消息对话框的窗口句柄，找到后调用EndDialog API 函数关闭这个消息对话框。)
        /// </summary>
        /// <param name="text">文本提示</param>
        /// <param name="caption">提示框标题</param>
        /// <param name="buttons">按钮类型</param>
        /// <param name="timeout">自动消失时间设置</param>
        public void ShowMessageBoxTimeout(string text, string caption, MessageBoxButtons buttons, int timeout)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(CloseMessageBox),
                new CloseState(caption, timeout));
            MessageBox.Show(text, caption, buttons);
        }

        /// <summary>
        /// 关闭弹出框(利用线程池调用一个工作线程 CloseMessageBox)
        /// </summary>
        /// <param name="state"></param>
        private static void CloseMessageBox(object state)
        {
            CloseState closeState = state as CloseState;
            Thread.Sleep(closeState.Timeout);
            IntPtr dlg = FindWindow(null, closeState.Caption);

            if (dlg != IntPtr.Zero)
            {
                IntPtr result;
                EndDialog(dlg, out result);
            }
        }
        #endregion
    }

    /// <summary>
    /// 对话框的标题和延时时间通过CloseState这个类传递给CloseMessageBox函数。
    /// </summary>
    public class CloseState
    {
        private int _Timeout;

        /// <summary>
        /// In millisecond毫秒
        /// </summary>
        public int Timeout
        {
            get
            {
                return _Timeout;
            }
        }

        private string _Caption;

        /// <summary>
        /// Caption of dialog对话框标题
        /// </summary>
        public string Caption
        {
            get
            {
                return _Caption;
            }
        }

        public CloseState(string caption, int timeout)
        {
            _Timeout = timeout;
            _Caption = caption;
        }
    }
}
