using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ASP.NETDemo.Demo1
{
    public partial class ShoppingCar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                List<string> cart = (List<string>)Session["Cart"];
                string info = string.Empty;
                foreach (string item in cart)
                {
                    info += item + ",";
                }
                Response.Write("您选的商品：" + info.TrimEnd(','));
                Response.Write("<br/>您的SessionID为：" + Session.SessionID);
            }
        }
    }
}