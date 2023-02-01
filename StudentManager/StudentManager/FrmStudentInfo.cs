using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Models;

namespace StudentManager
{
    public partial class FrmStudentInfo : Form
    {
        public FrmStudentInfo()
        {
            InitializeComponent();
        }

        public FrmStudentInfo(StudentExt objStudentExt):this()
        {
            this.lblStudentName.Text = objStudentExt.StudentName;
            this.lblGender.Text = objStudentExt.Gender;
            this.lblBirthday.Text = objStudentExt.Birthday.ToShortDateString();
            this.lblClass.Text = objStudentExt.ClassName;
            this.lblStudentIdNo.Text = objStudentExt.StudentIdNo;
            this.lblCardNo.Text = objStudentExt.CardNo;
            this.lblPhoneNumber.Text = objStudentExt.PhoneNumber;
            this.lblAddress.Text = objStudentExt.StudentAddress;
        }

        //¹Ø±Õ
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}