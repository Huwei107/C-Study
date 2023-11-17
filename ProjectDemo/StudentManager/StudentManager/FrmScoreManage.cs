using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DAL;
using Models;
using Models.ExtendModels;

namespace StudentManager
{
    public partial class FrmScoreManage : Form
    {
        private ScoreListSerivce objScoreListService = new ScoreListSerivce();
        private StudentClassService objClassService = new StudentClassService();
        public FrmScoreManage()
        {
            InitializeComponent();

            this.cboClass.DataSource = objClassService.GetAllClasses().Tables[0];
            this.cboClass.DisplayMember = "ClassName";
            this.cboClass.ValueMember = "ClassId";
            this.cboClass.SelectedIndex = -1;

            this.cboClass.SelectedIndexChanged += new System.EventHandler(this.cboClass_SelectedIndexChanged);
            this.dgvScoreList.AutoGenerateColumns = false;
        }     
        //根据班级查询      
        private void cboClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cboClass.SelectedIndex == -1)
            {
                MessageBox.Show("请选中班级！", "提示");
                return;
            }
            this.dgvScoreList.DataSource = objScoreListService.QueryScoreList(this.cboClass.Text.Trim());
            new Common.DataGridViewStyle().DgvStyle1(this.dgvScoreList);
            //QueryScore(this.cboClass.SelectedValue.ToString());
            Query(this.cboClass.SelectedValue.ToString());
        }
        //关闭
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void QueryScore(string classId)
        {
            Dictionary<string, string> infoList = objScoreListService.QueryScoreInfo(classId);
            this.lblAttendCount.Text = infoList["stuCount"];
            this.lblCount.Text = infoList["absentCount"];
            this.lblCSharpAvg.Text = infoList["avgCSharp"];
            this.lblDBAvg.Text = infoList["avgDB"];

            List<string> absentList = objScoreListService.QueryAbsentList(classId);
            this.lblList.Items.Clear();
            if (absentList.Count == 0)
            {
                this.lblList.Items.Add("沒有缺考");
            }
            else
            {
                this.lblList.Items.AddRange(absentList.ToArray());
            }
        }

        private void Query(string className)
        {
            //1.定义方法的输出参数（相对于后台调用）
            Dictionary<string, string> dicParam = null;
            List<string> absentList = null;
            //2.执行查询并接收返回的集合与参数
            List<StudentExt> scoreList = objScoreListService.GetScoreInfo(className, out dicParam, out absentList);
            //3.显示查询结果列表
            this.dgvScoreList.AutoGenerateColumns = false;
            this.dgvScoreList.DataSource = scoreList;
            //4.显示统计信息（方法输出参数）
            this.lblAttendCount.Text = dicParam["stuCount"];
            this.lblCount.Text = dicParam["absentCount"];
            this.lblCSharpAvg.Text = dicParam["avgCSharp"];
            this.lblDBAvg.Text = dicParam["avgDB"];
            //5.显示缺考人员列表
            this.lblList.Items.Clear();
            if (absentList.Count == 0)
            {
                this.lblList.Items.Add("沒有缺考");
            }
            else
            {
                this.lblList.Items.AddRange(absentList.ToArray());
            }
        }

        //统计全校考试成绩
        private void btnStat_Click(object sender, EventArgs e)
        {
            this.dgvScoreList.DataSource = objScoreListService.QueryScoreList("");
            //QueryScore("");
            Query("");
            new Common.DataGridViewStyle().DgvStyle1(this.dgvScoreList);
        }

        private void dgvScoreList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Common.DataGridViewStyle.DgvRowPostPaint(this.dgvScoreList, e);
        }

        //选择框选择改变处理
        private void dgvScoreList_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
         
        }

        //解析组合属性
        private void dgvScoreList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.Value is Student)
            {
                e.Value = (e.Value as Student).StudentId;
            }
            if (e.ColumnIndex == 1 && e.Value is Student)
            {
                e.Value = (e.Value as Student).StudentName;
            }
            if (e.ColumnIndex == 2 && e.Value is Student)
            {
                e.Value = (e.Value as Student).Gender;
            }
            if (e.ColumnIndex == 3 && e.Value is StudentClass)
            {
                e.Value = (e.Value as StudentClass).ClassName;
            }
            if (e.ColumnIndex == 4 && e.Value is ScoreList)
            {
                e.Value = (e.Value as ScoreList).CSharp;
            }
            if (e.ColumnIndex == 5 && e.Value is ScoreList)
            {
                e.Value = (e.Value as ScoreList).SQLServerDB;
            }
        }

        
       
    }
}