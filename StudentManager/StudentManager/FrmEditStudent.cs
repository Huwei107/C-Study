using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Models;
using DAL;

namespace StudentManager
{
    public partial class FrmEditStudent : Form
    {
        private StudentClassService objStudentClassService = new StudentClassService();
        public FrmEditStudent()
        {
            InitializeComponent();
        }
        public FrmEditStudent(Student objStudent):this()
        {
            this.cboClassName.DataSource = objStudentClassService.GetAllClass();
            this.cboClassName.DisplayMember = "ClassName";
            this.cboClassName.ValueMember = "ClassId";

            this.txtStudentId.Text = objStudent.StudentId.ToString();
            this.txtStudentName.Text = objStudent.StudentName;
            this.txtStudentIdNo.Text = objStudent.StudentIdNo;
            this.txtPhoneNumber.Text = objStudent.PhoneNumber;
            if (objStudent.Gender == "男")
                this.rdoMale.Checked = true;
            if (objStudent.Gender == "女")
                this.rdoFemale.Checked = true;
            this.cboClassName.Text = objStudent.ClassName;
            this.txtCardNo.Text = objStudent.CardNo;
            this.txtAddress.Text = objStudent.StudentAddress;
            this.dtpBirthday.Text = objStudent.Birthday.ToShortDateString();
            this.pbStu.Image = objStudent.StuImage.Length != 0 ? (Image)new Common.SerializeObjectToString().DeserializeObject(objStudent.StuImage)
                : Image.FromFile("default.png");
        }  

        //提交修改
        private void btnModify_Click(object sender, EventArgs e)
        {
        
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //选择照片
        private void btnChoseImage_Click(object sender, EventArgs e)
        {
            //OpenFileDialog objFileDialog = new OpenFileDialog();
            //DialogResult result = objFileDialog.ShowDialog();
            //if (result == DialogResult.OK)
            //    this.pbStu.Image = Image.FromFile(objFileDialog.FileName);
        }
    }
}