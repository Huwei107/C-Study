using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StudentManagerPro.Students
{
    public partial class UpLoadImage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] == null)
                {
                    Response.Redirect("~/ErrorPage.html");
                }
                ViewState["StudentId"] = Request.QueryString["id"];
            }
            this.ltaMsg.Text = "";
        }
        //上传照片
        protected void btnUpLoadImage_Click(object sender, EventArgs e)
        {
            //【1】判断是否有文件
            if (!this.fulStuImage.HasFile) return;
            //【2】获取文件大小，判断文件大小是否符合要求
            double fileLength = this.fulStuImage.FileContent.Length / (1024.0 * 1024.0);
            if (fileLength > 1.0)
            {
                this.ltaMsg.Text = "<script>alert('图片最大不能超过1MB！')</script>";
                return;
            }
            //【3】获取图片文件名，并修改成规范的文件名
        }
    }
}