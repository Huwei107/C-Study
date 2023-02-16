using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Models;
using DAL;

namespace StudentManager
{
    public partial class FrmStudentInfo : Form
    {
        public FrmStudentInfo()
        {
            InitializeComponent();
        }

        public FrmStudentInfo(Student objStudent):this()
        {
            this.lblStudentName.Text = objStudent.StudentName;
            this.lblStudentIdNo.Text = objStudent.StudentIdNo;
            this.lblPhoneNumber.Text = objStudent.PhoneNumber;
            this.lblGender.Text = objStudent.Gender;
            this.lblClass.Text = objStudent.ClassName;
            this.lblCardNo.Text = objStudent.CardNo;
            this.lblAddress.Text = objStudent.StudentAddress;
            this.lblBirthday.Text = objStudent.Birthday.ToShortDateString();
            this.pbStu.Image = objStudent.StuImage.Length != 0 ? (Image)new Common.SerializeObjectToString().DeserializeObject(objStudent.StuImage)
                : Image.FromFile("default.png");
        }
      
        //¹Ø±Õ
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}