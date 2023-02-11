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
    public partial class FrmScoreQuery : Form
    {
        private StudentClassService objStudentClassService = new StudentClassService();
        private ScoreListService objScoreListService = new ScoreListService();

        private DataTable dtScoreList = null;
       
        public FrmScoreQuery()
        {
            InitializeComponent();

            DataTable dt = objStudentClassService.GetAllClasses().Tables[0];
            this.cboClass.DataSource = dt;
            this.cboClass.ValueMember = "ClassId";
            this.cboClass.DisplayMember = "ClassName";
            this.cboClass.SelectedIndex = -1;

            //显示考试信息
            dtScoreList = objScoreListService.GetAllScoreList().Tables[0];
            this.dgvScoreList.DataSource = dtScoreList;
        }     

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //根据班级名称动态筛选
        private void cboClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dtScoreList == null)
                return;
            this.dtScoreList.DefaultView.RowFilter = string.Format("ClassName='{0}'", this.cboClass.Text.Trim());
        }
        //显示全部成绩
        private void btnShowAll_Click(object sender, EventArgs e)
        {
            if (dtScoreList == null)
                return;
            this.dtScoreList.DefaultView.RowFilter = string.Format("ClassName like '%%'");
        }
        //根据C#成绩动态筛选
        private void txtScore_TextChanged(object sender, EventArgs e)
        {
            if (dtScoreList == null)
                return;
            if (this.txtScore.Text.Trim().Length == 0)
                return;
            if (!Common.DataValidate.IsInteger(this.txtScore.Text.Trim()))
            {
                this.txtScore.Text = "";
                return;
            }
            this.dtScoreList.DefaultView.RowFilter = string.Format("CSharp > {0}", this.txtScore.Text.Trim());
        } 

        private void FrmScoreQuery_FormClosed(object sender, FormClosedEventArgs e)
        {
            FrmMain.objFrmScoreQuery = null;//当窗体关闭时，将对象窗体清理掉
        }
    }
}

//测试选定的班级名称对应的班级编号
//MessageBox.Show(this.cboClass.SelectedValue.ToString(), "班级ID");