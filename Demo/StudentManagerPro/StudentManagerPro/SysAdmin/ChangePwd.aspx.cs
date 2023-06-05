using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Models;
using DAL;

namespace StudentManagerPro
{
    public partial class ChangePwd : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["CurrentUser"] == null)
                {
                    Response.Redirect("~/AdminLogin.aspx");
                }
            }
        }

        protected void btnChangePwd_Click(object sender, EventArgs e)
        {
            //判断原密码是否正确
            SysAdmin objAdmin = (SysAdmin)Session["CurrentUser"];
            if (objAdmin.LoginPwd != this.txtOldPwd.Text.Trim())
            {
                this.ltaMsg.Text = "<script>alert('原密码不正确！')</script>";
                return;
            }
            //将新密码封装
            objAdmin.LoginPwd = this.txtNewPwd.Text.Trim();
            //调用后台修改密码
            try
            {
                new AdminService().ModifyPwd(objAdmin);
                this.ltaMsg.Text = "<script>alert('密码修改成功！');location='../Default.aspx'</script>";
            }
            catch (Exception ex)
            {

                this.ltaMsg.Text = "<script>alert('" + ex.Message + "')</script>";
            }
        }
      
    }
}