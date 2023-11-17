using ADO_NETDemo.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebDemo
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnGetCount_Click(object sender, EventArgs e)
        {
            StudentService objStuService = new StudentService();
            int class1 = objStuService.GetStuCountByClassId("1");
            int class2 = objStuService.GetStuCountByClassId("2");
            this.ltaInfo.Text = string.Format("1班的学生总数：{0}，2班的学生总数：{1}", class1, class2);
        }
    }
}