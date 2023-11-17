using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Models;
using DAL;

namespace StudentManager
{
    public partial class FrmAttendanceQuery : Form
    {
        private AttendanceService objAttendance = new AttendanceService();
     
        public FrmAttendanceQuery()
        {
            InitializeComponent();
            this.dgvStudentList.AutoGenerateColumns = false;
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            DateTime beginTime = Convert.ToDateTime(this.dtpTime.Text);
            DateTime endTime = beginTime.AddDays(1.0);
            this.dgvStudentList.DataSource = objAttendance.GetStudentBySignDate(beginTime, endTime, this.txtName.Text.Trim());
            new Common.DataGridViewStyle().DgvStyle1(this.dgvStudentList);

            this.lblCount.Text = objAttendance.GetStudentCount().ToString();
            this.lblReal.Text = objAttendance.GetSignStudents(beginTime, endTime).ToString();
            this.lblAbsenceCount.Text = (Convert.ToInt32(this.lblCount.Text) - Convert.ToInt32(this.lblReal.Text)).ToString();
        }
        //添加行号
        private void dgvStudentList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Common.DataGridViewStyle.DgvRowPostPaint(this.dgvStudentList, e);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
