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
    public partial class AdminLogin : System.Web.UI.Page
    {
        AdminService objAdminService = new AdminService();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ibtnLogin_Click(object sender, ImageClickEventArgs e)
        {
            //【1】封装登录信息
            SysAdmin objAdmin = new SysAdmin()
            {
                LoginId = Convert.ToInt32(this.txtUserId.Text.Trim()),
                LoginPwd = this.txtPwd.Text.Trim()
            };
            //【2】调用数据访问类查询用户信息
            try
            {
                objAdmin = objAdminService.AdminLogin(objAdmin);
                if (objAdmin == null)
                {
                    this.ltaInfo.Text = "<script>alert('用户名或密码错误！')</script>";
                }
                else
                {
                    Session["CurrentUser"] = objAdmin;
                    Response.Redirect("~/Default.aspx", false);
                }
            }
            catch (Exception ex)
            {

                this.ltaInfo.Text = "<script>alert('" + ex.Message + "')</script>";
            }
        }       
    }
}