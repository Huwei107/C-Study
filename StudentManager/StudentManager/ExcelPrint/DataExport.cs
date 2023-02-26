using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using System.Windows.Forms;

namespace StudentManager.ExcelPrint
{
    /// <summary>
    /// 
    /// </summary>
    public class DataExport
    {
        public bool Export(DataGridView dgv)
        {
            //定义Excel工作簿
            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
            //定义Excel工作表
            Microsoft.Office.Interop.Excel.Worksheet = new Microsoft.Office.Interop.Excel.Worksheet();

        }
    }
}
