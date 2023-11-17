using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DAL;
using Models;

namespace StudentManager
{
    public partial class FrmAttendance : Form
    {
        private AttendanceService objAttendance = new AttendanceService();
        private StudentService objStudentService = new StudentService();
        private List<Student> stuList = new List<Student>();

        public FrmAttendance()
        {
            InitializeComponent();

            this.dgvStudentList.AutoGenerateColumns = false;

            timer1_Tick(null, null);//省去1秒的延迟

            //显示应到、实到、缺勤人数
            //显示应到总数
            this.lblCount.Text = objAttendance.GetStudentCount().ToString();
            ShowStat();
            
        }
        private void ShowStat()
        {
            //显示实际出勤人数
            this.lblReal.Text = objAttendance.GetSignStudents().ToString();
            //显示缺勤人数
            this.lblAbsenceCount.Text = (Convert.ToInt32(this.lblCount.Text) - Convert.ToInt32(this.lblReal.Text)).ToString();
        }
        //显示当前时间
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.lblYear.Text = DateTime.Now.Year.ToString();
            this.lblMonth.Text = DateTime.Now.Month.ToString();
            this.lblDay.Text = DateTime.Now.Day.ToString();
            this.lblTime.Text = DateTime.Now.ToLongTimeString();

            //显示星期
            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    this.lblWeek.Text = "一";
                    break;
                case DayOfWeek.Tuesday:
                    this.lblWeek.Text = "二";
                    break;
                case DayOfWeek.Wednesday:
                    this.lblWeek.Text = "三";
                    break;
                case DayOfWeek.Thursday:
                    this.lblWeek.Text = "四";
                    break;
                case DayOfWeek.Friday:
                    this.lblWeek.Text = "五";
                    break;
                case DayOfWeek.Saturday:
                    this.lblWeek.Text = "六";
                    break;
                case DayOfWeek.Sunday:
                    this.lblWeek.Text = "日";
                    break;
            }
        }
        //学员打卡        
        private void txtStuCardNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13 && this.txtStuCardNo.Text.Trim().Length != 0)
            {
                //显示学员信息
                Student objStudent = objStudentService.GetStudentByCardNo(txtStuCardNo.Text.Trim());
                if (objStudent == null)
                {
                    MessageBox.Show("卡号不正确！", "提示");
                    this.lblInfo.Text = "打卡失败！";
                    this.txtStuCardNo.Focus();
                    this.txtStuCardNo.SelectAll();
                    this.lblStuName.Text = "";
                    this.lblStuClass.Text = "";
                    this.lblStuId.Text = "";
                    this.pbStu.Image = null;
                }
                else
                {
                    this.lblStuName.Text = objStudent.StudentName;
                    this.lblStuClass.Text = objStudent.ClassName;
                    this.lblStuId.Text = objStudent.StudentId.ToString();
                    if (objStudent.StuImage != null && objStudent.StuImage.Length != 0)
                    {
                        this.pbStu.Image = (Image)new Common.SerializeObjectToString().DeserializeObject(objStudent.StuImage);
                    }
                    else
                    {
                        this.pbStu.Image = Image.FromFile("default.png");
                    }
                    //添加打卡信息
                    string result = objAttendance.AddRecord(this.txtStuCardNo.Text.Trim());
                    if (result != "success")
                    {
                        this.lblInfo.Text = "打卡失败！";
                        MessageBox.Show(result, "提示");
                    }
                    else
                    {
                        this.lblInfo.Text = "打卡成功！";
                        //更新已经打卡的学员总数和缺勤人数
                        ShowStat();
                        objStudent.SignTime = DateTime.Now;
                        this.txtStuCardNo.Text = "";
                        this.txtStuCardNo.Focus();
                        stuList.Add(objStudent);
                        this.dgvStudentList.DataSource = null;
                        this.dgvStudentList.DataSource = stuList;
                    }
                }
            }
        }
        //结束打卡
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvStudentList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Common.DataGridViewStyle.DgvRowPostPaint(this.dgvStudentList, e);
        }
    }
}
