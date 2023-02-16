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
        private StudentClassService objStudentClassService = new StudentClassService();
        private StudentService objStudnetService = new StudentService();
        public FrmEditStudent()
        {
            InitializeComponent();
        }
        public FrmEditStudent(Student objStudent):this()
        {
            this.cboClassName.DataSource = objStudentClassService.GetAllClass();
            this.cboClassName.DisplayMember = "ClassName";
            this.cboClassName.ValueMember = "ClassId";

            this.txtStudentId.Text = objStudent.StudentId.ToString();
            this.txtStudentName.Text = objStudent.StudentName;
            this.txtStudentIdNo.Text = objStudent.StudentIdNo;
            this.txtPhoneNumber.Text = objStudent.PhoneNumber;
            if (objStudent.Gender == "男")
                this.rdoMale.Checked = true;
            if (objStudent.Gender == "女")
                this.rdoFemale.Checked = true;
            this.cboClassName.Text = objStudent.ClassName;
            this.txtCardNo.Text = objStudent.CardNo;
            this.txtAddress.Text = objStudent.StudentAddress;
            this.dtpBirthday.Text = objStudent.Birthday.ToShortDateString();
            this.pbStu.Image = objStudent.StuImage.Length != 0 ? (Image)new Common.SerializeObjectToString().DeserializeObject(objStudent.StuImage)
                : Image.FromFile("default.png");
        }  

        //提交修改
        private void btnModify_Click(object sender, EventArgs e)
        {
            #region 数据验证
            if (this.txtStudentName.Text.Trim().Length == 0)
            {
                MessageBox.Show("学员姓名不能为空！", "提示");
                this.txtStudentName.Focus();
                this.txtStudentName.SelectAll();
                return;
            }
            if (!this.rdoFemale.Checked && !this.rdoMale.Checked)
            {
                MessageBox.Show("学员性别不能为空！", "提示");
                return;
            }
            int age = DateTime.Now.Year - Convert.ToDateTime(this.dtpBirthday.Text).Year;
            if (age > 35 || age < 18)
            {
                MessageBox.Show("年龄必须在18到35之间！", "提示");
                return;
            }
            if (this.cboClassName.SelectedIndex == -1)
            {
                MessageBox.Show("请选择学员班级！", "提示");
                this.cboClassName.Focus();
                return;
            }
            if (this.txtStudentIdNo.Text.Trim().Length == 0)
            {
                MessageBox.Show("身份证号不能为空！", "提示");
                this.txtStudentIdNo.Focus();
                return;
            }
            if (!Common.DataValidate.IsIdentityCard(this.txtStudentIdNo.Text.Trim()))
            {
                MessageBox.Show("身份证号不符合要求！", "提示");
                this.txtStudentIdNo.Focus();
                this.txtStudentIdNo.SelectAll();
                return;
            }
            string birthday = Convert.ToDateTime(this.dtpBirthday.Text).ToString("yyyyMMdd");
            if (!this.txtStudentIdNo.Text.Trim().Contains(birthday))
            {
                MessageBox.Show("身份证号和出生日期不匹配！", "提示");
                this.txtStudentIdNo.Focus();
                this.txtStudentIdNo.SelectAll();
                return;
            }
            if (objStudnetService.IsIdNoExisted(this.txtStudentIdNo.Text.Trim(), this.txtStudentId.Text.Trim()))
            {
                MessageBox.Show("身份证号不能和其他学员相同！", "提示");
                this.txtStudentIdNo.Focus();
                this.txtStudentIdNo.SelectAll();
                return;
            }
            if (this.txtCardNo.Text.Trim().Length == 0)
            {
                MessageBox.Show("考勤卡号不能为空！", "提示");
                this.txtCardNo.Focus();
                return;
            }
            if (!Common.DataValidate.IsInteger(this.txtCardNo.Text.Trim()))
            {
                MessageBox.Show("考勤卡号必须是数字！", "提示");
                this.txtCardNo.Focus();
                this.txtCardNo.SelectAll();
                return;
            }
            if (objStudnetService.IsCardNoExisted(this.txtCardNo.Text.Trim(), this.txtStudentId.Text.Trim()))
            {
                MessageBox.Show("考勤卡号不能和其他学员相同！", "提示");
                this.txtCardNo.Focus();
                this.txtCardNo.SelectAll();
                return;
            }
            #endregion

            //封装对象
            Student objStudent = new Student()
            {
                StudentName = this.txtStudentName.Text.Trim(),
                Gender = this.rdoMale.Checked ? "男" : "女",
                Birthday = Convert.ToDateTime(this.dtpBirthday.Text),
                StudentIdNo = this.txtStudentIdNo.Text.Trim(),
                PhoneNumber = this.txtPhoneNumber.Text.Trim(),
                StudentAddress = this.txtAddress.Text.Trim(),
                ClassId = Convert.ToInt32(this.cboClassName.SelectedValue),
                ClassName = this.cboClassName.Text,
                Age = DateTime.Now.Year - Convert.ToDateTime(this.dtpBirthday.Text).Year,
                CardNo = this.txtCardNo.Text.Trim(),
                StuImage = this.pbStu.Image != null ? new Common.SerializeObjectToString().SerializeObject(this.pbStu.Image) : "",
                StudentId = Convert.ToInt32(this.txtStudentId.Text.Trim())
            };
            try
            {
                if (objStudnetService.ModifyStudent(objStudent) == 1)
                {
                    MessageBox.Show("学员信息修改成功！", "提示");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //选择照片
        private void btnChoseImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog objFileDialog = new OpenFileDialog();
            DialogResult result = objFileDialog.ShowDialog();
            if (result == DialogResult.OK)
                this.pbStu.Image = Image.FromFile(objFileDialog.FileName);
        }
    }
}