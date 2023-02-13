using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Models;

using System.Diagnostics;


namespace StudentManager
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //显示登录窗体
            FrmUserLogin frmLogin = new FrmUserLogin();
            DialogResult result = frmLogin.ShowDialog();
            //判断登录是否成功
            if (result == DialogResult.OK)
            {
                Application.Run(new FrmMain());
            }
            else
            {
                Application.Exit();
            }
        }
        //声明用户信息的全局变量
        public static SysAdmin currentAdmin = null;
    }
}

