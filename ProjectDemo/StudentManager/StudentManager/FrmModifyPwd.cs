using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DAL;
using Models;

namespace StudentManager
{
    public partial class FrmModifyPwd : Form
    {
        private SysAdminService objSysAdminService = new SysAdminService();
        public FrmModifyPwd()
        {
            InitializeComponent();
        }
        //修改密码
        private void btnModify_Click(object sender, EventArgs e)
        {
            #region 密码验证
            if (this.txtOldPwd.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入原密码！", "提示");
                txtOldPwd.Focus();
                return;
            }
            if (this.txtOldPwd.Text.Trim() != Program.currentAdmin.LoginPwd)
            {
                MessageBox.Show("请输入正确的原密码！", "提示");
                txtOldPwd.Focus();
                return;
            }
            if (this.txtNewPwd.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入新密码！", "提示");
                txtNewPwd.Focus();
                return;
            } if (this.txtNewPwd.Text.Trim().Length < 6)
            {
                MessageBox.Show("新密码长度不能少于6位！", "提示");
                txtNewPwd.Focus();
                txtNewPwd.SelectAll();
                return;
            }
            if (this.txtNewPwdConfirm.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入确认新密码！", "提示");
                txtNewPwdConfirm.Focus();
                return;
            }
            if (this.txtNewPwdConfirm.Text.Trim().Length < 6)
            {
                MessageBox.Show("新密码长度不能少于6位！", "提示");
                txtNewPwdConfirm.Focus();
                txtNewPwdConfirm.SelectAll();
                return;
            }
            if (this.txtNewPwdConfirm.Text.Trim() != this.txtNewPwd.Text.Trim())
            {
                MessageBox.Show("两次输入的密码不一致！", "提示");
                txtNewPwdConfirm.Focus();
                txtNewPwdConfirm.SelectAll();
                return;
            }
            #endregion

            try
            {
                SysAdmin objAdmin = new SysAdmin()
                {
                    LoginId=Program.currentAdmin.LoginId,
                    LoginPwd=this.txtNewPwdConfirm.Text.Trim()
                };
                if (objSysAdminService.ModfiyPwd(objAdmin) == 1)
                {
                    MessageBox.Show("密码修改成功！", "提示");
                    //同时修改当前保存用的密码
                    Program.currentAdmin.LoginPwd = this.txtNewPwdConfirm.Text.Trim();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
