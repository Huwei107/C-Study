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
using Models.ExtendModels;

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
        //���հ༶��ѯ
        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (this.cboClass.SelectedIndex == -1)
            {
                MessageBox.Show("��ѡ��༶��", "��ʾ");
                return;
            }
            this.stuList = objStudentService.GetStudentByClass(this.cboClass.Text);
            this.dgvStudentList.DataSource = stuList;
            new Common.DataGridViewStyle().DgvStyle1(this.dgvStudentList);
        }
        //����ѧ�Ų�ѯ
        private void btnQueryById_Click(object sender, EventArgs e)
        {
            if (this.txtStudentId.Text.Trim().Length == 0)
            {
                MessageBox.Show("������ѧ�ţ�", "��ʾ");
                this.txtStudentId.Focus();
                return;
            }
            if (!Common.DataValidate.IsInteger(this.txtStudentId.Text.Trim()))
            {
                MessageBox.Show("ѧ�ű�������������", "��ʾ");
                this.txtStudentId.Focus();
                this.txtStudentId.SelectAll();
                return;
            }
            Student objStudent = objStudentService.GetStudentById(this.txtStudentId.Text.Trim());
            if (objStudent == null)
            {
                MessageBox.Show("ѧԱ��Ϣ������", "��ʾ");
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
        //˫��ѡ�е�ѧԱ������ʾ��ϸ��Ϣ
        private void dgvStudentList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string studentId = dgvStudentList.CurrentRow.Cells["StudentId"].Value.ToString();
            Student objStudent = objStudentService.GetStudentById(studentId);
            if (objStudent == null)
            {
                MessageBox.Show("ѧԱ��Ϣ������", "��ʾ");
                return;
            }
            FrmStudentInfo objFrm = new FrmStudentInfo(objStudent);
            objFrm.ShowDialog();
        }
        //�޸�ѧԱ����
        private void btnEidt_Click(object sender, EventArgs e)
        {
            if (this.dgvStudentList.RowCount == 0)
            {
                MessageBox.Show("û��Ҫ�޸ĵ�ѧԱ��Ϣ��", "��ʾ");
                return;
            }
            if (this.dgvStudentList.CurrentRow == null)
            {
                MessageBox.Show("��ѡ��Ҫ�޸ĵ���Ϣ��", "��ʾ");
                return;
            }
            //��ȡѧ��
            string studentId = this.dgvStudentList.CurrentRow.Cells["StudentId"].Value.ToString();
            Student objStudent = objStudentService.GetStudentById(studentId);
            //��ʾ�޸���Ϣ����
            FrmEditStudent objFrm = new FrmEditStudent(objStudent);
            if (objFrm.ShowDialog() == DialogResult.OK)
            {
                //ͬ��ˢ��,�ʺ�С������
                btnQuery_Click(null, null);
            }
        }
        //ɾ��ѧԱ����
        private void btnDel_Click(object sender, EventArgs e)
        {
            if (this.dgvStudentList.RowCount == 0)
            {
                MessageBox.Show("û��ѧԱ��Ϣ����ɾ����", "��ʾ");
                return;
            }
            if (this.dgvStudentList.CurrentRow == null)
            {
                MessageBox.Show("����ѡ����Ҫɾ����ѧԱ��Ϣ��", "��ʾ");
                return;
            }

            DialogResult result = MessageBox.Show("ȷ��Ҫɾ����", "��ʾ", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Cancel)
                return;
            string studentId = this.dgvStudentList.CurrentRow.Cells["StudentId"].Value.ToString();
            try
            {
                if (objStudentService.DeleteStudent(studentId) == 1)
                {
                    btnQuery_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "��ʾ");
            }

        }
        //��������
        private void btnNameDESC_Click(object sender, EventArgs e)
        {
            if (this.dgvStudentList.RowCount == 0)
            {
                return;
            }
            this.stuList.Sort(new NameDesc());
            this.dgvStudentList.Refresh();//ֻ���ڲ�ѯ��ˢ�£���ɾ�Ĳ�����
        }
        //ѧ�Ž���
        private void btnStuIdDESC_Click(object sender, EventArgs e)
        {
            if (this.dgvStudentList.RowCount == 0)
            {
                return;
            }
            this.stuList.Sort(new StuIdDesc());
            this.dgvStudentList.Refresh();
        }
        //����к�
        private void dgvStudentList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Common.DataGridViewStyle.DgvRowPostPaint(this.dgvStudentList, e);
        }
        //��ӡ��ǰѧԱ��Ϣ
        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (this.dgvStudentList.RowCount == 0 || this.dgvStudentList.CurrentRow == null)
            {
                return;
            }
            //��ȡ��ǰ�е�ѧ��
            string studentId = this.dgvStudentList.CurrentRow.Cells["StudentId"].Value.ToString();
            //����ѧ�Ż�ȡѧԱ����
            StudentExt student = new StudentExt();
            student.StudentObj = objStudentService.GetStudentById(studentId);
            //����Excelģ��ʵ�ִ�ӡԤ��
            ExcelPrint.PrintStudent printStudent = new ExcelPrint.PrintStudent();
            printStudent.ExcelPrint(student);
        }

        //�ر�
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //������Excel
        private void btnExport_Click(object sender, EventArgs e)
        {
            
        }

        //�Ҽ��˵��޸�ѧԱ
        private void tsmiModifyStu_Click(object sender, EventArgs e)
        {
            btnEidt_Click(null, null);
        }

        private void tsmidDeleteStu_Click(object sender, EventArgs e)
        {
            btnDel_Click(null, null);
        }
    }

    #region ʵ������
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