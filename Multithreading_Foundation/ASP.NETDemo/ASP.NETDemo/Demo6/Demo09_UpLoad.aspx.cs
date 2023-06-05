using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApp
{
    public partial class Demo09_UpLoad : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            //【1】判断文件是否存在
            if (!this.ful.HasFile) return;
            //【2】获取文件大小，判断是否符合设置要求（变成MB）
            double fileLength = this.ful.FileContent.Length / (1024.0 * 1024.0);
            //获取配置文件中上传文件大小的限制
            double limitedLength = Convert.ToDouble(System.Configuration.ConfigurationManager.AppSettings["PhysicsObjectLength"]);
            limitedLength = limitedLength / 1024.0;//转换成MB单位
            //判断实际文件大小是否符合要求
            if (fileLength > limitedLength)
            {
                //  this.ltaMsg.Text = "上传文件大小不能超过" + limitedLength + "MB";
                this.ltaMsg.Text = "<script type='text/javascript'>alert('上传文件最大不能超过" + limitedLength + "M')</script>";
                return;
            }
            //【3】获取文件名，判断文件扩展是否符合要求
            string fileName = this.ful.FileName;
            //判断文件名是否是exe文件
            if (fileName.Substring(fileName.LastIndexOf(".") + 1).ToLower() == "exe")
            {
                this.ltaMsg.Text = "<script type='text/javascript'>alert('上传文件不能是exe文件')</script>";
                return;
            }
            //修改文件名  
            fileName = DateTime.Now.ToString("yyyyMMddhhssms") + "_" + fileName;

            //【4】获取服务器文件夹路径
            string path = Server.MapPath("~/UploadFilePath");
            //【5】上传文件
            try
            {
                this.ful.SaveAs(path + "/" + fileName);
                this.ltaMsg.Text = "<script type='text/javascript'>alert('文件上传成功！')</script>";
            }
            catch (Exception ex)
            {
                this.ltaMsg.Text = "<script type='text/javascript'>alert('文件上传失败！" + ex.Message + "')</script>";
            }
        }
    }
}