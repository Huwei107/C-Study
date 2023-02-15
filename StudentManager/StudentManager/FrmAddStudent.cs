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
        private StudentClassService objStudentClassSerivce = new StudentClassService();
        private StudentService objStudentService = new StudentService();
        List<Student> stuList = new List<Student>();

        public FrmAddStudent()
        {
            InitializeComponent();
      
            //��ʼ���༶������
            this.cboClassName.DataSource = objStudentClassSerivce.GetAllClass();
            this.cboClassName.DisplayMember = "ClassName";
            this.cboClassName.ValueMember = "ClassId";
            this.cboClassName.SelectedIndex = -1;

            this.dgvStudentList.AutoGenerateColumns = false;//��ֹ�Զ�������
        }
        //�����ѧԱ
        private void btnAdd_Click(object sender, EventArgs e)
        {
            #region ������֤
            if (this.txtStudentName.Text.Trim().Length == 0)
            {
                MessageBox.Show("ѧ����������Ϊ�գ�", "��ʾ");
                this.txtStudentName.Focus();
                return;
            }
            if (!this.rdoFemale.Checked && !this.rdoMale.Checked)
            {
                MessageBox.Show("��ѡ��ѧ���Ա�", "��ʾ");
                return;
            }
            int age = DateTime.Now.Year - Convert.ToDateTime(this.dtpBirthday.Text).Year;
            if (age > 35 || age < 18)
            {
                MessageBox.Show("���������18��35֮�䣡", "��ʾ");
                return;
            }
            if (this.cboClassName.SelectedIndex == -1)
            {
                MessageBox.Show("��ѡ��ѧ���༶��", "��ʾ");
                this.cboClassName.Focus();
                return;
            }
            if (this.txtStudentIdNo.Text.Trim().Length == 0)
            {
                MessageBox.Show("���֤�Ų���Ϊ�գ�", "��ʾ");
                this.txtStudentIdNo.Focus();
                return;
            }
            if (!Common.DataValidate.IsIdentityCard(this.txtStudentIdNo.Text.Trim()))
            {
                MessageBox.Show("���֤�Ų�����Ҫ��", "��ʾ");
                this.txtStudentIdNo.Focus();
                this.txtStudentIdNo.SelectAll();
                return;
            }
            //��֤���֤�еĳ����������û�ѡ��ĳ��������Ƿ�һ��
            string birthday = Convert.ToDateTime(this.dtpBirthday.Text).ToString("yyyyMMdd");
            if (!this.txtStudentIdNo.Text.Trim().Contains(birthday))
            {
                MessageBox.Show("���֤�źͳ������ڲ�ƥ�䣡", "��ʾ");
                this.txtStudentIdNo.Focus();
                this.txtStudentIdNo.SelectAll();
                return;
            }
            if (this.txtCardNo.Text.Trim().Length == 0)
            {
                MessageBox.Show("���ڿ��Ų���Ϊ�գ�", "��ʾ");
                this.txtCardNo.Focus();
                return;
            }
            if (!Common.DataValidate.IsInteger(this.txtCardNo.Text.Trim()))
            {
                MessageBox.Show("���ڿ��ű��������֣�", "��ʾ");
                this.txtCardNo.Focus();
                this.txtCardNo.SelectAll();
                return;
            }
            if (objStudentService.IsIdNoExisted(this.txtStudentIdNo.Text.Trim()))
            {
                MessageBox.Show("���֤���Ѵ��ڣ�", "��ʾ");
                this.txtStudentIdNo.Focus();
                this.txtStudentIdNo.SelectAll();
                return;
            }
            if (objStudentService.IsCardNoExisted(this.txtCardNo.Text.Trim()))
            {
                MessageBox.Show("���ڿ����Ѵ��ڣ�", "��ʾ");
                this.txtCardNo.Focus();
                this.txtCardNo.SelectAll();
                return;
            }
            #endregion

            //��װѧ������
            Student objStudent = new Student()
            {
                StudentName = this.txtStudentName.Text.Trim(),
                Gender=this.rdoMale.Checked?"��":"Ů",
                Birthday=Convert.ToDateTime(this.dtpBirthday.Text),
                StudentIdNo=this.txtStudentIdNo.Text.Trim(),
                PhoneNumber = this.txtPhoneNumber.Text.Trim(),
                StudentAddress=this.txtAddress.Text.Trim(),
                ClassId=Convert.ToInt32(this.cboClassName.SelectedValue),
                ClassName = this.cboClassName.Text,
                Age=DateTime.Now.Year-Convert.ToDateTime(this.dtpBirthday.Text).Year,
                CardNo=this.txtCardNo.Text.Trim(),
                StuImage = this.pbStu.Image!=null?new Common.SerializeObjectToString().SerializeObject(this.pbStu.Image):""
            };
            //���ú�̨���ݷ��ʷ���
            try
            {
                int studentId = objStudentService.AddStudent(objStudent);
                if (studentId > 1)
                {
                    //ͬ����ʾ��ӵ�ѧԱ
                    objStudent.StudentId = studentId;
                    this.stuList.Add(objStudent);
                    this.dgvStudentList.DataSource = null;
                    this.dgvStudentList.DataSource = this.stuList;
                    //ѯ���Ƿ�������
                    DialogResult result = MessageBox.Show("��ѧԱ��ӳɹ����Ƿ������ӣ�", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        //����û��������Ϣ
                        foreach (Control item in this.gbstuinfo.Controls)
                        {
                            if (item is TextBox)
                            {
                                item.Text = "";
                            }
                            else if (item is RadioButton)
                            {
                                ((RadioButton)item).Checked = false;
                            }
                            //this.rdoFemale.Checked = false;
                            //this.rdoMale.Checked = false;
                            this.cboClassName.SelectedIndex = -1;
                            this.pbStu.Image = null;
                            this.txtStudentName.Focus();
                        }
                    }
                    else
                    {
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("���ѧԱ�������ݷ����쳣��" + ex.Message);
            }
        }
        //�رմ���
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void FrmAddStudent_KeyDown(object sender, KeyEventArgs e)
        {
       

        }
        //ѡ������Ƭ
        private void btnChoseImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog objOpenFileDialog = new OpenFileDialog();
            DialogResult result = objOpenFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.pbStu.Image = Image.FromFile(objOpenFileDialog.FileName);
            }
        }
        //��������ͷ
        private void btnStartVideo_Click(object sender, EventArgs e)
        {
         
        }
        //����
        private void btnTake_Click(object sender, EventArgs e)
        {
        
        }
        //�����Ƭ
        private void btnClear_Click(object sender, EventArgs e)
        {
            this.pbStu.Image = null;
        }

        //����к�
        private void dgvStudentList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Common.DataGridViewStyle.DgvRowPostPaint(this.dgvStudentList, e);
        }

     
    }
}