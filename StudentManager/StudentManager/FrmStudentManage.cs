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
          
        }
        private void txtStudentId_KeyDown(object sender, KeyEventArgs e)
        {
         
        }
        //˫��ѡ�е�ѧԱ������ʾ��ϸ��Ϣ
        private void dgvStudentList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }
        //�޸�ѧԱ����
        private void btnEidt_Click(object sender, EventArgs e)
        {
          
        }
        //ɾ��ѧԱ����
        private void btnDel_Click(object sender, EventArgs e)
        {
           
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