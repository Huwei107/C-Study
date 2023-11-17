using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Configuration;

namespace StudentManager
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
      
            //显示当前用户
            this.lblCurrentUser.Text = Program.currentAdmin.AdminName + "]";
            //显示主窗体背景
            this.spContainer.Panel2.BackgroundImage = Image.FromFile("mainbg.png");
            this.spContainer.Panel2.BackgroundImageLayout = ImageLayout.Stretch;
            //显示版本号
            this.lblVersion.Text = "版本号：" + ConfigurationManager.AppSettings["pversion"].ToString();
        }

        #region 嵌入窗体显示
        //关闭窗体
        private void CloseForm()
        {
            //判断右侧容器中是否已存在窗体
            foreach (Control item in this.spContainer.Panel2.Controls)
            {
                if (item is Form)
                {
                    Form objControl = (Form)item;
                    objControl.Close();
                }
            }
        }

        //打开窗体
        private void OpenForm(Form objForm)
        {
            CloseForm();
            //嵌入的基本步骤
            objForm.TopLevel = false;//将子窗体设置成非顶级控件
            objForm.FormBorderStyle = FormBorderStyle.None;//去除窗体边框
            objForm.Parent = this.spContainer.Panel2;//指定子窗体显示的容器
            objForm.Dock = DockStyle.Fill;//设置子窗体随着容器大小自动调整窗体大小
            objForm.Show();
        }

        //显示添加新学员窗体       
        private void tsmiAddStudent_Click(object sender, EventArgs e)
        {
            FrmAddStudent objFrmAdd = new FrmAddStudent();
            OpenForm(objFrmAdd);
        }
        private void btnAddStu_Click(object sender, EventArgs e)
        {
            tsmiAddStudent_Click(null, null);
        }
        //批量导入学员信息
        private void tsmi_Import_Click(object sender, EventArgs e)
        {
            FrmImportData objForm = new FrmImportData();
            OpenForm(objForm);
        }
        private void btnImportStu_Click(object sender, EventArgs e)
        {
            tsmi_Import_Click(null, null);
        }
        //考勤打卡      
        private void tsmi_Card_Click(object sender, EventArgs e)
        {
            FrmAttendance objForm = new FrmAttendance();
            OpenForm(objForm);
        }
        private void btnCard_Click(object sender, EventArgs e)
        {
            tsmi_Card_Click(null, null);
        }
        //成绩快速查询【嵌入显示】
        private void tsmiQuery_Click(object sender, EventArgs e)
        {
            FrmScoreQuery objForm = new FrmScoreQuery();
            OpenForm(objForm);
        }
        private void btnScoreQuery_Click(object sender, EventArgs e)
        {
            tsmiQuery_Click(null, null);
        }
        //学员管理【嵌入显示】
        private void tsmiManageStudent_Click(object sender, EventArgs e)
        {
            FrmStudentManage objForm = new FrmStudentManage();
            OpenForm(objForm);
        }
        private void btnStuManage_Click(object sender, EventArgs e)
        {
            tsmiManageStudent_Click(null, null);
        }
        //显示成绩查询与分析窗口    
        private void tsmiQueryAndAnalysis_Click(object sender, EventArgs e)
        {
            FrmScoreManage objForm = new FrmScoreManage();
            OpenForm(objForm);
        }
        private void btnScoreAnalasys_Click(object sender, EventArgs e)
        {
            tsmiQueryAndAnalysis_Click(null, null);
        }
        //考勤查询
        private void tsmi_AQuery_Click(object sender, EventArgs e)
        {
            FrmAttendanceQuery objForm = new FrmAttendanceQuery();
            OpenForm(objForm);
        }
        private void btnAttendanceQuery_Click(object sender, EventArgs e)
        {
            tsmi_AQuery_Click(null, null);
        }

        #endregion

        #region 退出系统确认

        //退出系统
        private void tmiClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("确认退出吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result != DialogResult.OK)
            {
                e.Cancel = true;//告诉窗体事件关闭这个任务取消
            }
        }

        #endregion

        #region 其他

        //密码修改
        private void tmiModifyPwd_Click(object sender, EventArgs e)
        {
            FrmModifyPwd objPwd = new FrmModifyPwd();
            objPwd.ShowDialog();
        }
        private void btnModifyPwd_Click(object sender, EventArgs e)
        {
            tmiModifyPwd_Click(null, null);
        }
        //账号切换
        private void btnChangeAccount_Click(object sender, EventArgs e)
        {
            //创建登录窗体
            FrmUserLogin objFormLogin = new FrmUserLogin();
            objFormLogin.Text = "[账号切换]";
            DialogResult result = objFormLogin.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.lblCurrentUser.Text = Program.currentAdmin.AdminName + "]";
            }
        }
        private void tsbAddStudent_Click(object sender, EventArgs e)
        {
            tsmiAddStudent_Click(null, null);
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            tsmiManageStudent_Click(null, null);
        }
        private void tsbScoreAnalysis_Click(object sender, EventArgs e)
        {
            tsmiQueryAndAnalysis_Click(null, null);
        }
        private void tsbModifyPwd_Click(object sender, EventArgs e)
        {
            tmiModifyPwd_Click(null, null);
        }
        private void tsbExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void tsbQuery_Click(object sender, EventArgs e)
        {
            tsmiQuery_Click(null, null);
        }

        //访问官网
        private void tsmi_linkxkt_Click(object sender, EventArgs e)
        {
         
        }
        private void btnGoXiketang_Click(object sender, EventArgs e)
        {
            tsmi_linkxkt_Click(null, null);
        }
        //系统升级
        private void btnUpdate_Click(object sender, EventArgs e)
        {

        }
        #endregion



    }
}