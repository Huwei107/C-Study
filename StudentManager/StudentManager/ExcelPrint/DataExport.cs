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
            Microsoft.Office.Interop.Excel.Worksheet workSheet = excelApp.Workbooks.Add().Worksheets[1];

            //设置标题样式（从第二行第二列开始）
            workSheet.Cells[2, 2] = "学生成绩表";//设置标题内容
            workSheet.Cells[2, 2].RowHeight = 25;
            Microsoft.Office.Interop.Excel.Range range = workSheet.get_Range("B2", "H2");
            range.Merge(0);//合并表头单元格
            range.Borders.Value = 1;//设置表头边框
            range.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;//设置单元格居中显示
            range.Font.Size = 15;

            //获取总列数和总行数
            int colcumCount = dgv.ColumnCount;
            int rowCount = dgv.RowCount;

            //显示列标题
            for (int i = 0; i < colcumCount; i++)
            {
                //从第三行开始
                workSheet.Cells[3, i + 2] = dgv.Columns[i].HeaderText;
                workSheet.Cells[3, i + 2].Borders.Value = 1;
                workSheet.Cells[3, i + 2].RowHeight = 23;
            }
            //显示数据，从第二列、第四行开始
            for (int j = 0; j < rowCount-1; j++)
            {
                for (int k = 0; k < colcumCount; k++)
                {
                    workSheet.Cells[j + 4, k + 2] = dgv.Rows[j + 1].Cells[k].Value;
                    workSheet.Cells[j + 4, k + 2].Borders.Value = 1;
                    workSheet.Cells[j + 4, k + 2].RowHeight = 23;
                }
            }
            //设置列宽和数据一致
            workSheet.Columns.AutoFit();

            //打印预览
            excelApp.Visible = true;
            excelApp.Sheets.PrintPreview();
            //释放对象
            excelApp.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            excelApp = null;
            return true;
        }
    }
}
