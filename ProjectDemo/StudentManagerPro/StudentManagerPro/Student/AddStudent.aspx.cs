using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using Models;

namespace StudentManagerPro.Students
{
    public partial class AddStudent : System.Web.UI.Page
    {
        private StudentClassService objStudentClassService = new StudentClassService();
        private StudentService objStudentService = new StudentService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //初始化班级下拉框
                this.ddlClass.DataSource = objStudentClassService.GetAllClass();
                this.ddlClass.DataTextField = "ClassName";
                this.ddlClass.DataValueField = "ClassId";
                this.ddlClass.DataBind();//调用数据绑定方法
            }
            this.ltaMsg.Text = "";
        }

        protected void btnAddStudent_Click(object sender, EventArgs e)
        {
            if (this.txtValidateCode.Text.Trim() != Session["CheckCode"].ToString())
            {
                this.ltaMsg.Text = "<script type='text/javascript'>alert('验证码不正确！')</script>";
                return;
            }

            if (objStudentService.IsIdNoExisted(this.txtStuIdNo.Text.Trim()))
            {
                this.ltaMsg.Text = "<script type='text/javascript'>alert('身份证号已经被注册！')</script>";
                return;
            }

            Student objStudent = new Student()
            {
                StudentName=this.txtStuName.Text.Trim(),
                Gender = this.ddlGender.Text.Trim(),
                Birthday = Convert.ToDateTime(this.txtStuBirthday.Text.Trim()),
                ClassId = Convert.ToInt32(this.ddlClass.SelectedValue),
                PhoneNumber = this.txtPhoneNumber.Text.Trim(),
                StudentAddress=this.txtStuAddress.Text.Trim(),
                StudentIdNo = this.txtStuIdNo.Text.Trim()
            };
            try
            {
                int newStudentId = objStudentService.AddStudent(objStudent);
                if (newStudentId > 0)
                {
                    Response.Redirect("~/Student/UpLoadImage.aspx?id=" + newStudentId);
                }
            }
            catch (Exception ex)
            {

                this.ltaMsg.Text = "<script type='text/javascript'>alert('" + ex.Message + "')</script>";
            }
        }       
    }
}