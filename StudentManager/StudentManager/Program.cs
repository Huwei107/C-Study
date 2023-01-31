using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Models;

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

            //创建登录窗体
            FrmUserLogin objFrmLogin = new FrmUserLogin();
            DialogResult result = objFrmLogin.ShowDialog();
            //判断登录是否成功
            if (result == DialogResult.OK)
            {
                Application.Run(new FrmMain());
            }
            else
            {
                Application.Exit();//退出整个应用程序
            }
        }

        //定义全局变量，保存用户对象
        public static SysAdmin objCurrentAdmin = null;

    }
}
