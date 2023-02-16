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
    public partial class FrmStudentManage : Form
    {
        private StudentClassService objStudentClassService = new StudentClassService();
        private StudentService objStudentService = new StudentService();
        private List<Student> stuList = null;

        public FrmStudentManage()
        {
            InitializeComponent();

            this.cboClass.DataSource = objStudentClassService.GetAllClass();
            this.cboClass.ValueMember = "ClassId";
            this.cboClass.DisplayMember = "ClassName";
            this.cboClass.SelectedIndex = -1;

            this.dgvStudentList.AutoGenerateColumns = false;
        }
        //按照班级查询
        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (this.cboClass.SelectedIndex == -1)
            {
                MessageBox.Show("请选择班级！", "提示");
                return;
            }
            this.stuList = objStudentService.GetStudentByClass(this.cboClass.Text);
            this.dgvStudentList.DataSource = stuList;
            new Common.DataGridViewStyle().DgvStyle1(this.dgvStudentList);
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
                MessageBox.Show("学号必须是正整数！", "提示");
                this.txtStudentId.Focus();
                this.txtStudentId.SelectAll();
                return;
            }
            Student objStudent = objStudentService.GetStudentById(this.txtStudentId.Text.Trim());
            if (objStudent == null)
            {
                MessageBox.Show("学员信息不存在", "提示");
                this.txtStudentId.Focus();
                this.txtStudentId.SelectAll();
                return;
            }
            FrmStudentInfo objFrm = new FrmStudentInfo(objStudent);
            objFrm.ShowDialog();
        }
        private void txtStudentId_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.txtStudentId.Text.Trim().Length != 0 && e.KeyValue == 13)
            {
                btnQueryById_Click(null, null);
            }
        }
        //双击选中的学员对象并显示详细信息
        private void dgvStudentList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string studentId = dgvStudentList.CurrentRow.Cells["StudentId"].Value.ToString();
            Student objStudent = objStudentService.GetStudentById(studentId);
            if (objStudent == null)
            {
                MessageBox.Show("学员信息不存在", "提示");
                return;
            }
            FrmStudentInfo objFrm = new FrmStudentInfo(objStudent);
            objFrm.ShowDialog();
        }
        //修改学员对象
        private void btnEidt_Click(object sender, EventArgs e)
        {
            if (this.dgvStudentList.RowCount == 0)
            {
                MessageBox.Show("没有要修改的学员信息！", "提示");
                return;
            }
            if (this.dgvStudentList.CurrentRow == null)
            {
                MessageBox.Show("请选中要修改的信息！", "提示");
                return;
            }
            //获取学号
            string studentId = this.dgvStudentList.CurrentRow.Cells["StudentId"].Value.ToString();
            Student objStudent = objStudentService.GetStudentById(studentId);
            //显示修改信息窗口
            FrmEditStudent objFrm = new FrmEditStudent(objStudent);
            objFrm.ShowDialog();
        }
        //删除学员对象
        private void btnDel_Click(object sender, EventArgs e)
        {
           
        }
        //姓名降序
        private void btnNameDESC_Click(object sender, EventArgs e)
        {
            if (this.dgvStudentList.RowCount == 0)
            {
                return;
            }
            this.stuList.Sort(new NameDesc());
            this.dgvStudentList.Refresh();//只限于查询的刷新，增删改不可以
        }
        //学号降序
        private void btnStuIdDESC_Click(object sender, EventArgs e)
        {
            if (this.dgvStudentList.RowCount == 0)
            {
                return;
            }
            this.stuList.Sort(new StuIdDesc());
            this.dgvStudentList.Refresh();
        }
        //添加行号
        private void dgvStudentList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Common.DataGridViewStyle.DgvRowPostPaint(this.dgvStudentList, e);
        }
        //打印当前学员信息
        private void btnPrint_Click(object sender, EventArgs e)
        {
          
        }

        //关闭
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //导出到Excel
        private void btnExport_Click(object sender, EventArgs e)
        {
            
        }

        //右键菜单修改学员
        private void tsmiModifyStu_Click(object sender, EventArgs e)
        {
            btnEidt_Click(null, null);
        }

        private void tsmidDeleteStu_Click(object sender, EventArgs e)
        {
            btnDel_Click(null, null);
        }
    }

    #region 实现排序
    class NameDesc:IComparer<Student>
    {
        public int Compare(Student x, Student y)
        {
            return y.StudentName.CompareTo(x.StudentName);
        }
    }
    class StuIdDesc : IComparer<Student>
    {
        public int Compare(Student x, Student y)
        {
            return y.StudentId.CompareTo(x.StudentId);
        }
    }
    #endregion


}