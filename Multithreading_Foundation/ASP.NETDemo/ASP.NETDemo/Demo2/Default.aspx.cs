using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ASP.NET_Demo2.Demo5
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //取出Cookie值
                if (Request.Cookies["UserName"] != null)
                    Response.Write("用户名=" + Request.Cookies["UserName"].Value);
                if (Request.Cookies["UserPhone"] != null)
                    Response.Write("用户电话=" + Request.Cookies["UserPhone"].Value);
            }
        }

        protected void btnSaveToCookie_Click(object sender, EventArgs e)
        {
            //方法一：保存Cookie并设置有效期
            Response.Cookies["UserName"].Expires = DateTime.Now.AddDays(1.0);
            Response.Cookies["UserName"].Value = this.txtUserName.Text.Trim();

            //方法二：
            HttpCookie hcookie = new HttpCookie("UserPhone", "12345678");
            hcookie.Expires = DateTime.Now.AddDays(1.0);
            Response.Cookies.Add(hcookie);
        }
    }
}