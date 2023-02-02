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
        private static StudentService objStudentService = new StudentService();

        public FrmEditStudent()
        {
            InitializeComponent();

            //��ʼ���༶������
            this.cboClassName.DataSource = objClassService.GetAllClass();
            this.cboClassName.DisplayMember = "ClassName";
            this.cboClassName.ValueMember = "ClassId";
        }

        public FrmEditStudent(StudentExt objStudentExt):this()
        {
            this.txtStudentId.Text = Convert.ToString(objStudentExt.StudentId);
            this.txtStudentName.Text = objStudentExt.StudentName;
            if(objStudentExt.Gender=="��")
            {
                this.rdoMale.Checked=true;
            }else
            {
                this.rdoFemale.Checked=true;
            }
            this.dtpBirthday.Text = objStudentExt.Birthday.ToShortDateString();
            this.cboClassName.Text = objStudentExt.ClassName;
            this.txtStudentIdNo.Text = objStudentExt.StudentIdNo;
            this.txtCardNo.Text = objStudentExt.CardNo;
            this.txtPhoneNumber.Text = objStudentExt.PhoneNumber;
            this.txtAddress.Text = objStudentExt.StudentAddress;
        }
 

        //�ύ�޸�
        private void btnModify_Click(object sender, EventArgs e)
        {
            #region ��֤��Ϣ

            //�ж����֤���Ƿ��ظ�
            if(objStudentService.IsIdNoExisted(this.txtStudentIdNo.Text.Trim(), this.txtStudentId.Text.Trim()))
            {
                MessageBox.Show("���֤���ظ���", "��ʾ");
                this.txtStudentIdNo.Focus();
                this.txtStudentIdNo.SelectAll();
                return;
            }
            #endregion

            #region ��װѧ������
            Student objStudent = new Student()
            {
                StudentId = Convert.ToInt32(this.txtStudentId.Text.Trim()),
                StudentName = this.txtStudentName.Text.Trim(),
                Gender = this.rdoMale.Checked ? "��" : "Ů",
                Birthday = Convert.ToDateTime(this.dtpBirthday.Text),
                StudentIdNo = this.txtStudentIdNo.Text.Trim(),
                PhoneNumber = this.txtPhoneNumber.Text.Trim(),
                StudentAddress = this.txtAddress.Text.Trim(),
                CardNo = this.txtCardNo.Text.Trim(),
                ClassId = Convert.ToInt32(this.cboClassName.SelectedValue),
                Age = DateTime.Now.Year - Convert.ToDateTime(this.dtpBirthday.Text).Year
            };
            #endregion

            #region �ύ�޸�
            try
            {
                if (objStudentService.ModifyStudent(objStudent) == 1)
                {
                    MessageBox.Show("�޸ĳɹ���", "��ʾ");
                    this.DialogResult = DialogResult.OK;//�����޸ĳɹ�����Ϣ
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            #endregion
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}