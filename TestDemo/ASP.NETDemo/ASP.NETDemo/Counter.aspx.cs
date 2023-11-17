using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ASP.NETDemo
{
    public partial class Counter : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["Counter"] = 0;
            }
        }

        protected void btnCount_Click(object sender, EventArgs e)
        {
            int counter = Convert.ToInt32(ViewState["Counter"]);
            counter++;
            this.ltcount.Text = counter.ToString();
            ViewState["Counter"] = counter;
        }
    }
}