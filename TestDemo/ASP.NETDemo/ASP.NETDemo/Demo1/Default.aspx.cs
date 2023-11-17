using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ASP.NETDemo.Demo1
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                List<string> cart = new List<string>();
                Session["Cart"] = cart;
            }
            if (Session["CurrentUser"] != null)
            {
                this.ltaMsg.Text = "欢迎您：" + Session["CurrentUser"].ToString();
            }
            else
            {
                this.ltaMsg.Text = "您还没有登录！";
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            //首先判断用户是否登录
            if (Session["CurrentUser"] == null)
            {
                Response.Redirect("UserLogin.aspx");
            }
            else
            {
                foreach (Control item in form1.Controls)
                {
                    if (item is CheckBox)
                    {
                        CheckBox ckd = (CheckBox)item;
                        if (ckd.Checked)
                        {
                            ((List<string>)Session["Cart"]).Add(ckd.Text);
                        }
                    }
                }
                this.Label1.Text = "添加成功！";
            }

            
        }

        protected void btnGWC_Click(object sender, EventArgs e)
        {
            Response.Redirect("ShoppingCar.aspx");
        }
    }
}