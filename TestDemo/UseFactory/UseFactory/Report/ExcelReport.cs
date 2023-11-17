using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UseFactory.Factory;

namespace UseFactory.Report
{
    class ExcelReport:IReport
    {
        public void StartPrint()
        {
            //在这里编写具体的报表程序
            MessageBox.Show("正在调用Excel报表程序");
        }
    }
}
