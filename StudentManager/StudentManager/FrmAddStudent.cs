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
         
            //初始化班级下拉框
            this.cboClassName.DataSource = objClassService.GetAllClass();
            this.cboClassName.DisplayMember = "ClassName";
            this.cboClassName.ValueMember = "ClassId";
            this.cboClassName.SelectedIndex = -1;//默认不选中
        }

        //添加新学员
        private void btnAdd_Click(object sender, EventArgs e)
        {
            #region 数据验证
            if (this.txtStudentName.Text.Trim().Length == 0)
            {
                MessageBox.Show("学生姓名不能为空！", "验证提示");
                this.txtStudentName.Focus();
                return;
            }
            if (!this.rdoFemale.Checked && !this.rdoMale.Checked)
            {
                MessageBox.Show("请选择学员性别！", "验证提示");
                return;
            }
            if (this.txtStudentIdNo.Text.Trim().Length == 0)
            {
                MessageBox.Show("身份证号不能为空！", "验证提示");
                this.txtStudentIdNo.Focus();
                return;
            }
            if (this.txtCardNo.Text.Trim().Length == 0)
            {
                MessageBox.Show("考勤卡号不能为空！", "验证提示");
                this.txtCardNo.Focus();
                return;
            }
            if (this.txtPhoneNumber.Text.Trim().Length == 0)
            {
                MessageBox.Show("联系电话不能为空！", "验证提示");
                this.txtPhoneNumber.Focus();
                return;
            }
            if (this.txtAddress.Text.Trim().Length == 0)
            {
                MessageBox.Show("家庭住址不能为空！", "验证提示");
                this.txtAddress.Focus();
                return;
            }
            //验证身份证号格式是否正确
            if (!Common.DataValidate.IsIdentityCard(this.txtStudentIdNo.Text.Trim()))
            {
                MessageBox.Show("身份证号不符合要求！", "验证提示");
                this.txtStudentIdNo.Focus();
                this.txtStudentIdNo.SelectAll();
                return;
            }
            //验证身份证号是否重复
            if (objStudentService.IsIdNoExisted(this.txtStudentIdNo.Text.Trim()))
            {
                MessageBox.Show("身份证号重复！", "验证提示");
                this.txtStudentIdNo.Focus();
                this.txtStudentIdNo.SelectAll();
                return;
            }
            //验证身份证号和出生日期是否相吻合

            //验证出生日期
            int age = DateTime.Now.Year - Convert.ToDateTime(this.dtpBirthday.Text).Year;
            if (age < 18 || age > 35)
            {
                MessageBox.Show("年龄必须在18到35之间！", "验证提示");
                this.dtpBirthday.Focus();
                return;
            }
            if (this.cboClassName.SelectedIndex == -1)
            {
                MessageBox.Show("请选择学员班级！", "验证提示");
                return;
            }
            #endregion

            #region 封装学生对象
            Student objStudent = new Student()
            {
                StudentName = this.txtStudentName.Text.Trim(),
                Gender=this.rdoMale.Checked?"男":"女",
                Birthday=Convert.ToDateTime(this.dtpBirthday.Text),
                StudentIdNo =this.txtStudentIdNo.Text.Trim(),
                PhoneNumber=this.txtPhoneNumber.Text.Trim(),
                StudentAddress=this.txtAddress.Text.Trim(),
                CardNo=this.txtCardNo.Text.Trim(),
                ClassId = Convert.ToInt32(this.cboClassName.SelectedValue),
                Age = DateTime.Now.Year - Convert.ToDateTime(this.dtpBirthday.Text).Year
            };
            #endregion

            #region 调用后台数据访问方法添加对象
            if (objStudentService.AddStudent(objStudent) == 1)
            {
                try
                {
                    DialogResult result = MessageBox.Show("新学员添加成功！是否继续添加？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
        //关闭窗体
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //窗体已经被关闭 
        private void FrmAddStudent_FormClosed(object sender, FormClosedEventArgs e)
        {
             FrmMain.objFrmAddStudent = null;//当窗体关闭时，将对象窗体清理掉
        }
    }
}