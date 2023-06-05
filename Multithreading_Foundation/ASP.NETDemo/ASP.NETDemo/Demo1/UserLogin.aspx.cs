using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ASP.NET_Demo2.Demo04
{
    public partial class UserLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            //验证信息

            //查询用户信息，验证用户名和密码是否正确
            if (this.txtUserName.Text.Trim() == "xiaowang" && this.txtPwd.Text.Trim() == "12345")
            {
                //保存用户登录信息
                Session["CurrentUser"] = this.txtUserName.Text.Trim();
                //登录成功后跳转到首页
                Response.Redirect("Default.aspx");
            }
            else
            {
                this.lInfo.Text = "用户名或密码错误！";
            }
        }
    }
}