using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Models;
using DAL;

namespace StudentManager
{
    public partial class FrmUserLogin : Form
    {
        private SysAdminService objSysAdminService = new SysAdminService();

        public FrmUserLogin()
        {
            InitializeComponent();
        }


        //登录
        private void btnLogin_Click(object sender, EventArgs e)
        {
            //数据验证
            if (this.txtLoginId.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入登录账号！", "提示");
                this.txtLoginId.Focus();
                return;
            }
            if (!Common.DataValidate.IsInteger(this.txtLoginId.Text.Trim()))
            {
                MessageBox.Show("登录账号必须为正整数！", "提示");
                this.txtLoginId.Focus();
                return;
            }
            if (this.txtLoginPwd.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入登录密码！", "提示");
                this.txtLoginPwd.Focus();
                return;
            }

            //封装对象
            SysAdmin objAdmin = new SysAdmin()
            {
                LoginId = Convert.ToInt32(this.txtLoginId.Text.Trim()),
                LoginPwd =this.txtLoginPwd.Text.Trim().ToString()
            };
            //和后台交互
            try
            {
                Program.currentAdmin = objSysAdminService.AdminLoginParam(objAdmin);
                if (Program.currentAdmin != null)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("用户名或密码错误！", "提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "数据访问操作异常！");
            }
            //处理交互结果

        }
        //关闭
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void txtLoginId_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.txtLoginId.Text.Trim().Length != 0 && e.KeyValue == 13)//回车
            {
                this.txtLoginPwd.Focus();
                this.txtLoginPwd.SelectAll();
            }
        }

        private void txtLoginPwd_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.txtLoginPwd.Text.Trim().Length != 0 && e.KeyValue == 13)
            {
                btnLogin_Click(null, null);
            }
        }
    }
}
