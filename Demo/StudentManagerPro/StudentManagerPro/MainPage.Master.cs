using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Models;

namespace StudentManagerPro
{
    public partial class MainPage : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CurrentUser"] == null)
            {
                Response.Redirect("~/AdminLogin.aspx");
            }
            else//如果用户登录成功则显示用户信息
            {
                this.ltaUserName.Text = "欢迎您：" + ((SysAdmin)Session["CurrentUser"]).AdminName;
            }
        }
    }
}