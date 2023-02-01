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
        private static StudentClassService objClassService = new StudentClassService();

        public FrmEditStudent()
        {
            InitializeComponent();

            //初始化班级下拉框
            this.cboClassName.DataSource = objClassService.GetAllClass();
            this.cboClassName.DisplayMember = "ClassName";
            this.cboClassName.ValueMember = "ClassId";
        }

        public FrmEditStudent(StudentExt objStudentExt):this()
        {
            
        }
 

        //提交修改
        private void btnModify_Click(object sender, EventArgs e)
        {
        
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}