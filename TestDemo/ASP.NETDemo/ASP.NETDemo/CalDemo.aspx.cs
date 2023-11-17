using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ASP.NETDemo
{
    public partial class CalDemo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int a = Convert.ToInt32(this.txtNum1.Text.Trim());
            int b = Convert.ToInt32(this.txtNum2.Text.Trim());
            this.txtResult.Text = (a + b).ToString();
        }
    }
}