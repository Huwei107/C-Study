
//-----------------------------------------------------------------------
// <copyright company="工品一号" file="ImPortAndExprotHelper">
//  Copyright (c)  V1.0.0.0  
//  创建作者:   李 杰
//  创建时间:   2018-05-03 15:40:15
//  功能描述:   【请输入类描述】
//  历史版本:
//          2018-05-03 15:40:15 李 杰 创建ImPortAndExprotHelper类
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
//using NPOI.HSSF.UserModel;
//using NPOI.SS.UserModel;


namespace FX.MainForms
{
    public class HelperImPortAndExprot
    {
        //public static DataTable ExcelRead(string filename)
        //{
        //    //读取xls文件
        //    StringBuilder sbr = new StringBuilder();
        //    DataTable dtmain = new DataTable();
        //    try
        //    {
        //        using (FileStream fs = File.OpenRead(filename))   //打开myxls.xls文件
        //        {
        //            HSSFWorkbook wk = new HSSFWorkbook(fs);   //把xls文件中的数据写入wk中
        //            ISheet sheet = wk.GetSheetAt(0);   //读取当前表数据
        //            IRow row0 = sheet.GetRow(0);  //读取当前行数据
        //            DataTable dtt = new DataTable();
        //            if (row0 != null)//表头
        //            {
        //                for (int k = 0; k < row0.LastCellNum; k++)
        //                {
        //                    ICell cell = row0.GetCell(k);
        //                    string cellName = cell.ToString();
        //                    dtmain.Columns.Add(cellName, typeof(string));
        //                }
        //            }
        //            for (int j = 1; j <= sheet.LastRowNum; j++)  //LastRowNum 是当前表的总行数
        //            {
        //                IRow row = sheet.GetRow(j);  //读取当前行数据
        //                if (row != null)
        //                {
        //                    DataRow row_col = dtmain.NewRow();
        //                    for (int n = 0; n < row.LastCellNum; n++)
        //                    {
        //                        ICell cell = row.GetCell(n);
        //                        if (cell != null)
        //                        {
        //                            row_col[n] = cell.ToString().Trim();
        //                        }
        //                    }
        //                    dtmain.Rows.InsertAt(row_col, dtmain.Rows.Count);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //    return dtmain;
        //}



        ///// <summary>
        ///// 将dataTable导出成Excel
        ///// </summary>
        ///// <param name="dt">所需要导出的数据集合</param>
        ///// <param name="fileName">所需要导出的文件名（例如：XXX.xls）</param>
        ///// param name="isShowExcle">导出后是否打开文件</param>  
        //public static void DataTableToExcel(DataTable dt, string fileName)
        //{

        //    if (dt != null)
        //    {
        //        TableToExcel(dt, fileName);
        //        MessageBoxContentHelper.ShowMessageOK("数据导出成功！");
        //    }
        //    else
        //    {
        //        MessageBoxContentHelper.ShowMessageOK("没有数据可供查出，请核实后在重新导出！");
        //    }
        //}

        /// <summary>  
        /// 导出Excel文件  
        /// </summary>  
        /// /// <param name="dataSet"></param>  
        /// <param name="dataTable">数据集</param>  
        /// <param name="isShowExcle">导出后是否打开文件</param>  
        /// <returns></returns>  
        //public static bool DataTableToExcel(string filePath, System.Data.DataTable dataTable, bool isShowExcle)
        //{
        //    //System.Data.DataTable dataTable = dataSet.Tables[0];  
        //    int rowNumber = dataTable.Rows.Count;
        //    int columnNumber = dataTable.Columns.Count;
        //    int colIndex = 0;

        //    if (rowNumber == 0)
        //    {
        //        return false;
        //    }

        //    Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
        //    Microsoft.Office.Interop.Excel.Workbook workbook = excel.Workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
        //    Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];
        //    excel.Visible = isShowExcle;
        //    Microsoft.Office.Interop.Excel.Range range;


        //    foreach (DataColumn col in dataTable.Columns)
        //    {
        //        colIndex++;
        //        excel.Cells[1, colIndex] = col.ColumnName;
        //    }

        //    object[,] objData = new object[rowNumber, columnNumber];

        //    for (int r = 0; r < rowNumber; r++)
        //    {
        //        for (int c = 0; c < columnNumber; c++)
        //        {
        //            objData[r, c] = dataTable.Rows[r][c];
        //        }
        //    }

        //    range = worksheet.get_Range(excel.Cells[2, 1], excel.Cells[rowNumber + 1, columnNumber]);

        //    range.Value2 = objData;

        //    range.NumberFormatLocal = "@";

        //    worksheet.SaveAs(filePath, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
        //    //excel.Quit();  
        //    return true;
        //}  

        ///// <summary>
        ///// Datable导出成Excel
        ///// </summary>
        ///// <param name="dt"></param>
        ///// <param name="file">导出路径(包括文件名与扩展名)</param>
        //public static void TableToExcel(DataTable dt, string file)
        //{
        //    IWorkbook workbook;
        //    string fileExt = Path.GetExtension(file).ToLower();
        //    if (fileExt == ".xlsx") { workbook = null; } else if (fileExt == ".xls") { workbook = new HSSFWorkbook(); } else { workbook = null; }
        //    if (workbook == null) { return; }
        //    ISheet sheet = string.IsNullOrEmpty(dt.TableName) ? workbook.CreateSheet("Sheet1") : workbook.CreateSheet(dt.TableName);

        //    //表头  
        //    IRow row = sheet.CreateRow(0);
        //    for (int i = 0; i < dt.Columns.Count; i++)
        //    {
        //        ICell cell = row.CreateCell(i);
        //        cell.SetCellValue(dt.Columns[i].ColumnName);
        //    }

        //    //数据  
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        IRow row1 = sheet.CreateRow(i + 1);
        //        for (int j = 0; j < dt.Columns.Count; j++)
        //        {
        //            ICell cell = row1.CreateCell(j);
        //            cell.SetCellValue(dt.Rows[i][j].ToString());
        //        }
        //    }

        //    //转为字节数组  
        //    MemoryStream stream = new MemoryStream();
        //    workbook.Write(stream);
        //    var buf = stream.ToArray();

        //    //保存为Excel文件  
        //    using (FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write))
        //    {
        //        fs.Write(buf, 0, buf.Length);
        //        fs.Flush();
        //    }
        //}
    }
}
