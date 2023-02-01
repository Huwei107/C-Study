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
        private StudentService objStudentService = new StudentService();
     
        public FrmAddStudent()
        {
            InitializeComponent();
         
            //��ʼ���༶������
            this.cboClassName.DataSource = objClassService.GetAllClass();
            this.cboClassName.DisplayMember = "ClassName";
            this.cboClassName.ValueMember = "ClassId";
            this.cboClassName.SelectedIndex = -1;//Ĭ�ϲ�ѡ��
        }

        //�����ѧԱ
        private void btnAdd_Click(object sender, EventArgs e)
        {
            #region ������֤
            if (this.txtStudentName.Text.Trim().Length == 0)
            {
                MessageBox.Show("ѧ����������Ϊ�գ�", "��֤��ʾ");
                this.txtStudentName.Focus();
                return;
            }
            if (!this.rdoFemale.Checked && !this.rdoMale.Checked)
            {
                MessageBox.Show("��ѡ��ѧԱ�Ա�", "��֤��ʾ");
                return;
            }
            if (this.txtStudentIdNo.Text.Trim().Length == 0)
            {
                MessageBox.Show("���֤�Ų���Ϊ�գ�", "��֤��ʾ");
                this.txtStudentIdNo.Focus();
                return;
            }
            if (this.txtCardNo.Text.Trim().Length == 0)
            {
                MessageBox.Show("���ڿ��Ų���Ϊ�գ�", "��֤��ʾ");
                this.txtCardNo.Focus();
                return;
            }
            if (this.txtPhoneNumber.Text.Trim().Length == 0)
            {
                MessageBox.Show("��ϵ�绰����Ϊ�գ�", "��֤��ʾ");
                this.txtPhoneNumber.Focus();
                return;
            }
            if (this.txtAddress.Text.Trim().Length == 0)
            {
                MessageBox.Show("��ͥסַ����Ϊ�գ�", "��֤��ʾ");
                this.txtAddress.Focus();
                return;
            }
            //��֤���֤�Ÿ�ʽ�Ƿ���ȷ
            if (!Common.DataValidate.IsIdentityCard(this.txtStudentIdNo.Text.Trim()))
            {
                MessageBox.Show("���֤�Ų�����Ҫ��", "��֤��ʾ");
                this.txtStudentIdNo.Focus();
                this.txtStudentIdNo.SelectAll();
                return;
            }
            //��֤���֤���Ƿ��ظ�
            if (objStudentService.IsIdNoExisted(this.txtStudentIdNo.Text.Trim()))
            {
                MessageBox.Show("���֤���ظ���", "��֤��ʾ");
                this.txtStudentIdNo.Focus();
                this.txtStudentIdNo.SelectAll();
                return;
            }
            //��֤���֤�źͳ��������Ƿ����Ǻ�

            //��֤��������
            int age = DateTime.Now.Year - Convert.ToDateTime(this.dtpBirthday.Text).Year;
            if (age < 18 || age > 35)
            {
                MessageBox.Show("���������18��35֮�䣡", "��֤��ʾ");
                this.dtpBirthday.Focus();
                return;
            }
            if (this.cboClassName.SelectedIndex == -1)
            {
                MessageBox.Show("��ѡ��ѧԱ�༶��", "��֤��ʾ");
                return;
            }
            #endregion

            #region ��װѧ������
            Student objStudent = new Student()
            {
                StudentName = this.txtStudentName.Text.Trim(),
                Gender=this.rdoMale.Checked?"��":"Ů",
                Birthday=Convert.ToDateTime(this.dtpBirthday.Text),
                StudentIdNo =this.txtStudentIdNo.Text.Trim(),
                PhoneNumber=this.txtPhoneNumber.Text.Trim(),
                StudentAddress=this.txtAddress.Text.Trim(),
                CardNo=this.txtCardNo.Text.Trim(),
                ClassId = Convert.ToInt32(this.cboClassName.SelectedValue),
                Age = DateTime.Now.Year - Convert.ToDateTime(this.dtpBirthday.Text).Year
            };
            #endregion

            #region ���ú�̨���ݷ��ʷ�����Ӷ���
            if (objStudentService.AddStudent(objStudent) == 1)
            {
                try
                {
                    DialogResult result = MessageBox.Show("��ѧԱ��ӳɹ����Ƿ������ӣ�", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        this.cboClassName.SelectedIndex = -1;
                        this.rdoFemale.Checked = false;
                        this.rdoMale.Checked = false;
                        foreach (Control item in this.Controls)
                        {
                            if (item is TextBox)
                                item.Text = "";
                        }
                        this.txtStudentName.Focus();
                    }
                    else
                    {
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            #endregion
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