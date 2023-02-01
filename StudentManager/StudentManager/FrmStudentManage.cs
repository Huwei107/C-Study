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

            //��ʼ���༶������
            this.cboClass.DataSource = objStudentClassService.GetAllClass();
            this.cboClass.DisplayMember = "ClassName";
            this.cboClass.ValueMember = "ClassId";
            this.cboClass.SelectedIndex = -1;//Ĭ�ϲ�ѡ��
        }
        //���հ༶��ѯ
        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (this.cboClass.SelectedIndex == -1)
            {
                MessageBox.Show("��ѡ��༶��", "��ʾ");
                return;
            }
            this.dgvStudentList.AutoGenerateColumns = false;//����ʾδ��װ������
            //ִ�в�ѯ
            this.dgvStudentList.DataSource = objStudentService.GetStudentByClass(this.cboClass.Text);
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
                MessageBox.Show("ѧ�ű��������֣�", "��ʾ");
                this.txtStudentId.Focus();
                this.txtStudentId.SelectAll();
                return;
            }

            StudentExt objStudent = objStudentService.GetStudentById(this.txtStudentId.Text.Trim());
            if (objStudent == null)
            {
                MessageBox.Show("ѧ����Ϣ�����ڣ�", "��ʾ");
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
        //˫��ѡ�е�ѧԱ������ʾ��ϸ��Ϣ
        private void dgvStudentList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
          
        }
        //�޸�ѧԱ����
        private void btnEidt_Click(object sender, EventArgs e)
        {
            if (this.dgvStudentList.RowCount == 0)
            {
                MessageBox.Show("û�п����޸ĵ���Ϣ��", "��ʾ");
                return;
            }
            if (this.dgvStudentList.CurrentRow == null)
            {
                MessageBox.Show("��ѡ��Ҫ�޸ĵ�ѧԱ��Ϣ��", "��ʾ");
                return;
            }
            //��ȡѧ��
            string studentId = this.dgvStudentList.CurrentRow.Cells["StudentId"].Value.ToString();
            //��ȡҪ�޸ĵ�ѧԱ����ϸ��Ϣ
            StudentExt objStudentExt = objStudentService.GetStudentById(studentId);
            //��ʾҪ�޸ĵ�ѧԱ����
            FrmEditStudent objFrmEditStudent = new FrmEditStudent(objStudentExt);
            objFrmEditStudent.ShowDialog();
        }       
        //ɾ��ѧԱ����
        private void btnDel_Click(object sender, EventArgs e)
        {
          
        }
        private void FrmSearchStudent_FormClosed(object sender, FormClosedEventArgs e)
        {
            FrmMain.objFrmStudentManage = null;//������ر�ʱ���������������
        }
        //�ر�
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}