using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using DAL;
using Models;

namespace StudentManager
{
    public partial class FrmStudentManage : Form
    {
        private StudentClassService objStudentClassService = new StudentClassService();
        private StudentService objStudentService = new StudentService();

        public FrmStudentManage()
        {
            InitializeComponent();

            //初始化班级下拉框
            this.cboClass.DataSource = objStudentClassService.GetAllClass();
            this.cboClass.DisplayMember = "ClassName";
            this.cboClass.ValueMember = "ClassId";
            this.cboClass.SelectedIndex = -1;//默认不选中
        }
        //按照班级查询
        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (this.cboClass.SelectedIndex == -1)
            {
                MessageBox.Show("请选择班级！", "提示");
                return;
            }
            this.dgvStudentList.AutoGenerateColumns = false;//不显示未封装的属性
            //执行查询
            this.dgvStudentList.DataSource = objStudentService.GetStudentByClass(this.cboClass.Text);
        }
        //根据学号查询
        private void btnQueryById_Click(object sender, EventArgs e)
        {
            if (this.txtStudentId.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入学号！", "提示");
                this.txtStudentId.Focus();
                return;
            }
            if (!Common.DataValidate.IsInteger(this.txtStudentId.Text.Trim()))
            {
                MessageBox.Show("学号必须是数字！", "提示");
                this.txtStudentId.Focus();
                this.txtStudentId.SelectAll();
                return;
            }

            StudentExt objStudent = objStudentService.GetStudentById(this.txtStudentId.Text.Trim());
            if (objStudent == null)
            {
                MessageBox.Show("学号信息不存在！", "提示");
            }
            else
            {
                FrmStudentInfo objFrmStudentInfo = new FrmStudentInfo(objStudent);
                objFrmStudentInfo.Show();
            }
        }
        private void txtStudentId_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13 && this.txtStudentId.Text.Trim().Length != 0)
            {
                btnQueryById_Click(null, null);
            }
        }
        //双击选中的学员对象并显示详细信息
        private void dgvStudentList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
          
        }
        //修改学员对象
        private void btnEidt_Click(object sender, EventArgs e)
        {
            if (this.dgvStudentList.RowCount == 0)
            {
                MessageBox.Show("没有可以修改的信息！", "提示");
                return;
            }
            if (this.dgvStudentList.CurrentRow == null)
            {
                MessageBox.Show("请选中要修改的学员信息！", "提示");
                return;
            }
            //获取学号
            string studentId = this.dgvStudentList.CurrentRow.Cells["StudentId"].Value.ToString();
            //获取要修改的学员的详细信息
            StudentExt objStudentExt = objStudentService.GetStudentById(studentId);
            //显示要修改的学员窗口
            FrmEditStudent objFrmEditStudent = new FrmEditStudent(objStudentExt);
            objFrmEditStudent.ShowDialog();
        }       
        //删除学员对象
        private void btnDel_Click(object sender, EventArgs e)
        {
          
        }
        private void FrmSearchStudent_FormClosed(object sender, FormClosedEventArgs e)
        {
            FrmMain.objFrmStudentManage = null;//当窗体关闭时，将对象窗体清理掉
        }
        //关闭
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}