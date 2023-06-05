using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ASP.NET_Demo2.Demo3
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.lta.Text = "您是本网站第" + Application["UserVisit"].ToString() + "位访客！" +
                    "   当前在线人数：" + Application["CurrentUsers"].ToString();
            }

        }
        //清除当前用户的Session
        protected void btnClear_Click(object sender, EventArgs e)
        {
            Session.Abandon();
        }
    }
}