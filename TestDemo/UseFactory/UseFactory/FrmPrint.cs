using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UseFactory.Factory;
using UseFactory.Report;

namespace UseFactory
{
    public partial class FrmPrint : Form
    {
        public FrmPrint()
        {
            InitializeComponent();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            //IReport objReport = new ExcelReport();
            
            //根据工厂提供的具体产品来使用
            IReport objReport = UseFactory.Factory.Factory.ChooseReportType();
            //调用接口打印方法
            objReport.StartPrint();
        }
    }
}
