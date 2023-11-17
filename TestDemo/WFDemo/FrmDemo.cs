using ADO_NETDemo.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFDemo
{
    public partial class FrmDemo : Form
    {
        public FrmDemo()
        {
            InitializeComponent();
        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            StudentService objStudentService = new StudentService();
            int class1 = objStudentService.GetStuCountByClassId("1");
            int class2 = objStudentService.GetStuCountByClassId("2");
            this.lblInfo.Text = string.Format("1班的学生总数：{0}，2班的学生总数：{1}", class1, class2);
        }
    }
}
