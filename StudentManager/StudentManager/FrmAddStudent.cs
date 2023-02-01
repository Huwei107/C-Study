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
    public partial class FrmAddStudent : Form
    {
        private StudentClassService objClassService = new StudentClassService();
     
        public FrmAddStudent()
        {
            InitializeComponent();
         
            //初始化班级下拉框
            this.cboClassName.DataSource = objClassService.GetAllClass();
            this.cboClassName.DisplayMember = "ClassName";
            this.cboClassName.ValueMember = "ClassId";
        }

        //添加新学员
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (Common.DataValidate.IsIdentityCard(txtStudentIdNo.Text.Trim()))
            {
                MessageBox.Show("身份证号不符合要求！", "验证提示");
                txtStudentIdNo.Focus();
                return;
            }
        }
        //关闭窗体
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //窗体已经被关闭 
        private void FrmAddStudent_FormClosed(object sender, FormClosedEventArgs e)
        {
             FrmMain.objFrmAddStudent = null;//当窗体关闭时，将对象窗体清理掉
        }
    }
}