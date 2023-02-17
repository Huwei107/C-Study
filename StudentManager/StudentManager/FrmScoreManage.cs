using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DAL;
using Models;

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
        //���ݰ༶��ѯ      
        private void cboClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cboClass.SelectedIndex == -1)
            {
                MessageBox.Show("��ѡ�а༶��", "��ʾ");
                return;
            }
            this.dgvScoreList.DataSource = objScoreListService.QueryScoreList(this.cboClass.Text.Trim());
            new Common.DataGridViewStyle().DgvStyle1(this.dgvScoreList);
        }
        //�ر�
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //ͳ��ȫУ���Գɼ�
        private void btnStat_Click(object sender, EventArgs e)
        {
            this.dgvScoreList.DataSource = objScoreListService.QueryScoreList("");
            new Common.DataGridViewStyle().DgvStyle1(this.dgvScoreList);
        }

        private void dgvScoreList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Common.DataGridViewStyle.DgvRowPostPaint(this.dgvScoreList, e);
        }

    
     
        //ѡ���ѡ��ı䴦��
        private void dgvScoreList_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
         
        }

       
    }
}