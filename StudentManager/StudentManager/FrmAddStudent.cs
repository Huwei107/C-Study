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
         
            //��ʼ���༶������
            this.cboClassName.DataSource = objClassService.GetAllClass();
            this.cboClassName.DisplayMember = "ClassName";
            this.cboClassName.ValueMember = "ClassId";
        }

        //�����ѧԱ
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (Common.DataValidate.IsIdentityCard(txtStudentIdNo.Text.Trim()))
            {
                MessageBox.Show("���֤�Ų�����Ҫ��", "��֤��ʾ");
                txtStudentIdNo.Focus();
                return;
            }
        }
        //�رմ���
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //�����Ѿ����ر� 
        private void FrmAddStudent_FormClosed(object sender, FormClosedEventArgs e)
        {
             FrmMain.objFrmAddStudent = null;//������ر�ʱ���������������
        }
    }
}