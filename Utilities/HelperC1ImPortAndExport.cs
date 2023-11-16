
//-----------------------------------------------------------------------
// <copyright company="工品一号" file="C1ImPortAndExportHelper">
//  Copyright (c)  V1.0.0.0  
//  创建作者:   唐亮
//  创建时间:   2018-05-28 09:51:15
//  功能描述:   【请输入类描述】
//  历史版本:
//          2018-05-28 09:51:15 唐亮 创建C1ImPortAndExportHelper类
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using C1.C1Excel;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Formula.Eval;
using NPOI.XSSF.UserModel;

namespace FX.MainForms
{
    /// <summary>
    /// 【请输入类描述】
    /// </summary>
    public class HelperC1ImPortAndExport
    {
        /// <summary>
        /// C1Excel读取不除去空行        
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static DataTable ExcelRead(string filename)
        {
            #region NPOI文件解析
            ///读取xls文件
            StringBuilder sbr = new StringBuilder();
            DataTable dtmain = new DataTable();
            try
            {
                using (FileStream fs = File.OpenRead(filename))   //打开myxls.xls文件
                {
                    HSSFWorkbook wk = new HSSFWorkbook(fs);   //把xls文件中的数据写入wk中
                    ISheet sheet = wk.GetSheetAt(0);   //读取当前表数据
                    IRow row0 = sheet.GetRow(0);  //读取当前行数据
                    DataTable dtt = new DataTable();
                    if (row0 != null)//表头
                    {
                        for (int k = 0; k < row0.LastCellNum; k++)
                        {
                            ICell cell = row0.GetCell(k);
                            string cellName = cell.ToString();
                            dtmain.Columns.Add(cellName, typeof(string));
                        }
                    }
                    for (int j = 1; j <= sheet.LastRowNum; j++)  //LastRowNum 是当前表的总行数
                    {
                        IRow row = sheet.GetRow(j);  //读取当前行数据
                        if (row != null)
                        {
                            DataRow row_col = dtmain.NewRow();
                            for (int n = 0; n < row.LastCellNum; n++)
                            {
                                ICell cell = row.GetCell(n);
                                if (cell != null)
                                {
                                    row_col[n] = cell.ToString().Trim();
                                }
                            }
                            dtmain.Rows.InsertAt(row_col, dtmain.Rows.Count);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HelperLog.Write(ex);
                throw;
            }
            return dtmain;
            #endregion

            #region c1导入解析模板Load会有问题所以启用npoi方法
            //#region 暂时注销
            //DataTable dtmain = new DataTable();
            //try
            //{

            //    C1XLBook c1XLBook1 = new C1XLBook();
            //    c1XLBook1.Load(filename);
            //    C1.C1Excel.XLSheet sheet = c1XLBook1.Sheets[0];
            //    XLRow row0 = sheet.Rows[0];  //读取当前行数据
            //    if (row0 != null)//表头
            //    {
            //        for (int k = 0; k < row0.Sheet.Columns.Count; k++)
            //        {
            //            XLCell cell = row0.Sheet.GetCell(0, k);
            //            string cellName = cell.Text;
            //            dtmain.Columns.Add(cellName, typeof(string));
            //        }
            //    }
            //    for (int r = 1; r < sheet.Rows.Count; r++)
            //    {
            //        XLRow row = sheet.Rows[r];  //读取当前行数据
            //        if (row != null)
            //        {
            //            DataRow row_col = dtmain.NewRow();
            //            for (int n = 0; n < row.Sheet.Columns.Count; n++)
            //            {
            //                XLCell cell = row.Sheet.GetCell(r, n);
            //                if (cell != null)
            //                {
            //                    row_col[n] = cell.Text.Trim();
            //                }
            //            }
            //            dtmain.Rows.InsertAt(row_col, dtmain.Rows.Count);
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    HelperLog.Write(ex);
            //    throw;
            //}
            //return dtmain;
            //#endregion 
            #endregion
        }


        public static DataTable ExcelToDataTable(string filename)
        {
            #region NPOI文件解析
            DataTable dtmain = new DataTable();
            try
            {
                using (FileStream fs = new FileStream(filename, FileMode.Open))   //打开myxls.xls文件
                {
                    HSSFWorkbook wk = new HSSFWorkbook(fs);   //把xls文件中的数据写入wk中
                    ISheet sheet = wk.GetSheetAt(0);   //读取当前表数据
                    IRow row0 = sheet.GetRow(0);  //读取当前行数据
                    DataTable dtt = new DataTable();
                    if (row0 != null)//表头
                    {
                        for (int k = 0; k < row0.LastCellNum; k++)
                        {
                            ICell cell = row0.GetCell(k);
                            string cellName = cell.ToString();
                            dtmain.Columns.Add(cellName);
                        }
                    }
                    for (int j = 1; j <= sheet.LastRowNum; j++)  //LastRowNum 是当前表的总行数
                    {
                        IRow row = sheet.GetRow(j);  //读取当前行数据
                        if (row != null)
                        {
                            DataRow dataRow = dtmain.NewRow();
                            for (int n = 0; n < row.LastCellNum; n++)
                            {
                                ICell cell = row.GetCell(n);
                                if (cell != null)
                                {
                                    switch (cell.CellType)
                                    {
                                        case CellType.String:
                                            string str = cell.StringCellValue;
                                            if (str != null && str.Length > 0)
                                            {
                                                dataRow[n] = str.ToString();
                                            }
                                            else
                                            {
                                                dataRow[n] = null;
                                            }
                                            break;
                                        case CellType.Numeric:
                                            if (DateUtil.IsCellDateFormatted(cell))
                                            {
                                                dataRow[n] = DateTime.FromOADate(cell.NumericCellValue);
                                            }
                                            else
                                            {
                                                dataRow[n] = Convert.ToDouble(cell.NumericCellValue);
                                            }
                                            break;
                                        case CellType.Boolean:
                                            dataRow[n] = Convert.ToString(cell.BooleanCellValue);
                                            break;
                                        case CellType.Error:
                                            dataRow[n] = ErrorEval.GetText(cell.ErrorCellValue);
                                            break;
                                        case CellType.Formula:
                                            switch (cell.CachedFormulaResultType)
                                            {
                                                case CellType.String:
                                                    string strFORMULA = cell.StringCellValue;
                                                    if (strFORMULA != null && strFORMULA.Length > 0)
                                                    {
                                                        dataRow[n] = strFORMULA.ToString();
                                                    }
                                                    else
                                                    {
                                                        dataRow[n] = null;
                                                    }
                                                    break;
                                                case CellType.Numeric:
                                                    dataRow[n] = Convert.ToString(cell.NumericCellValue);
                                                    break;
                                                case CellType.Boolean:
                                                    dataRow[n] = Convert.ToString(cell.BooleanCellValue);
                                                    break;
                                                case CellType.Error:
                                                    dataRow[n] = ErrorEval.GetText(cell.ErrorCellValue);
                                                    break;
                                                default:
                                                    dataRow[n] = "";
                                                    break;
                                            }
                                            break;
                                        default:
                                            dataRow[n] = "";
                                            break;
                                    }
                                }
                            }
                            dtmain.Rows.Add(dataRow);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HelperLog.Write(ex);
                return null;
            }
            return dtmain;
            #endregion
        }

        /// <summary>
        /// 导入返回数据表
        /// </summary>
        /// <param name="filename">文件名称</param>
        /// <param name="errorCode">错误代码</param>
        /// <param name="headRow">列头行索引</param>
        /// <param name="endRow">列尾行索引</param>
        /// <param name="isAgreeFirstRowSingleColumn">允许首行单列吗（true：允许,fase：不允许）</param>
        /// <returns></returns>
        public static DataTable ExcelToDataTable(string filename, out int errorCode, int headRow = 0, int endRow = -1, bool isAgreeFirstRowSingleColumn = true)
        {
            #region NPOI文件解析
            DataTable dtmain = new DataTable();
            IWorkbook workbook;
            string fileExt = Path.GetExtension(filename).ToLower();
            errorCode = -1;//错误代码默认-1（无错误信息）
            if (endRow != -1 && endRow <= headRow) return null;
            try
            {
                using (FileStream fs = File.OpenRead(filename))   //打开myxls.xls文件
                {
                    //XSSFWorkbook 适用XLSX格式，HSSFWorkbook 适用XLS格式
                    if (fileExt == ".xlsx")
                    {
                        //有时候客户不经意将xlsx后缀修改为xls导致此处会BUG  by arison 20230912
                        try
                        {
                            workbook = new XSSFWorkbook(fs);
                        }
                        catch (Exception ex)
                        {
                            HelperLog.Write(ex);
                            using (FileStream fs2 = File.OpenRead(filename))
                            {
                                workbook = new HSSFWorkbook(fs2);
                            }
                        }
                    }
                    else if (fileExt == ".xls")
                    {
                        //有时候客户不经意将xlsx后缀修改为xls导致此处会BUG  by arison 20230912
                        try
                        {
                            workbook = new HSSFWorkbook(fs);
                        }
                        catch (Exception ex)
                        {
                            HelperLog.Write(ex);
                            using (FileStream fs2 = File.OpenRead(filename))
                            {
                                workbook = new XSSFWorkbook(fs2);
                            }
                        }
                    }
                    else { workbook = null; }
                    if (workbook == null) { return null; }
                    ISheet sheet = workbook.GetSheetAt(0);   //读取当前表数据
                    IRow row0 = sheet.GetRow(headRow);  //读取当前行数据
                    DataTable dtt = new DataTable();
                    if (row0 != null)//表头
                    {
                        Dictionary<string, int> dict = new Dictionary<string, int>();
                        for (int k = 0; k < row0.LastCellNum; k++)
                        {

                            ICell cell = row0.GetCell(k);
                            string cellName = cell.ToString().Trim();
                            if (cellName.Length == 0)
                            {
                                //不允许出现空列名称
                                errorCode = 111;
                                return null;
                            }
                            if (!dict.ContainsKey(cellName))
                            {
                                dict.Add(cellName, 0);
                                dtmain.Columns.Add(cellName);
                            }
                            else
                            {
                                //已经存在重复列
                                errorCode = 110;
                                return null;
                            }

                        }
                        if (dict.Keys.Count == 1 && !isAgreeFirstRowSingleColumn)
                        {
                            //不允许首行单列出现
                            errorCode = 112;
                            return null;
                        }
                    }
                    var lastRow = endRow == -1 ? sheet.LastRowNum : endRow;
                    for (int j = headRow + 1; j <= lastRow; j++)  //LastRowNum 是当前表的总行数
                    {
                        IRow row = sheet.GetRow(j);  //读取当前行数据
                        if (row != null)
                        {
                            DataRow dataRow = dtmain.NewRow();
                            for (int n = 0; n < row.LastCellNum; n++)
                            {
                                ICell cell = row.GetCell(n);
                                if (cell != null)
                                {
                                    switch (cell.CellType)
                                    {
                                        case CellType.String:
                                            string str = cell.StringCellValue;
                                            if (str != null && str.Length > 0)
                                            {
                                                dataRow[n] = str.ToString();
                                            }
                                            else
                                            {
                                                dataRow[n] = null;
                                            }
                                            break;
                                        case CellType.Numeric:
                                            if (DateUtil.IsCellDateFormatted(cell))
                                            {
                                                dataRow[n] = DateTime.FromOADate(cell.NumericCellValue);
                                            }
                                            else
                                            {
                                                dataRow[n] = Convert.ToDouble(cell.NumericCellValue);
                                            }
                                            break;
                                        case CellType.Boolean:
                                            dataRow[n] = Convert.ToString(cell.BooleanCellValue);
                                            break;
                                        case CellType.Error:
                                            dataRow[n] = ErrorEval.GetText(cell.ErrorCellValue);
                                            break;
                                        case CellType.Formula:
                                            switch (cell.CachedFormulaResultType)
                                            {
                                                case CellType.String:
                                                    string strFORMULA = cell.StringCellValue;
                                                    if (strFORMULA != null && strFORMULA.Length > 0)
                                                    {
                                                        dataRow[n] = strFORMULA.ToString();
                                                    }
                                                    else
                                                    {
                                                        dataRow[n] = null;
                                                    }
                                                    break;
                                                case CellType.Numeric:
                                                    dataRow[n] = Convert.ToString(cell.NumericCellValue);
                                                    break;
                                                case CellType.Boolean:
                                                    dataRow[n] = Convert.ToString(cell.BooleanCellValue);
                                                    break;
                                                case CellType.Error:
                                                    dataRow[n] = ErrorEval.GetText(cell.ErrorCellValue);
                                                    break;
                                                default:
                                                    dataRow[n] = "";
                                                    break;
                                            }
                                            break;
                                        default:
                                            dataRow[n] = "";
                                            break;
                                    }
                                }
                            }
                            dtmain.Rows.Add(dataRow);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HelperLog.Write(ex);
                if (ex.Source == "mscorlib")
                    errorCode = 103;//Excel正被占用
                return null;
            }
            return dtmain;
            #endregion
        }

        /// <summary>
        /// NPOI读取excel去除空行
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>返回解析的dt</returns>
        public static DataTable ExcelReadRemoveEmptyRow(string filename)
        {
            #region NPOI文件解析
            ///读取xls文件
            StringBuilder sbr = new StringBuilder();
            DataTable dtmain = new DataTable();
            int rowIndex = 0;//用来判断一行中是否有空白列
            try
            {
                using (FileStream fs = File.OpenRead(filename))   //打开myxls.xls文件
                {
                    HSSFWorkbook wk = new HSSFWorkbook(fs);   //把xls文件中的数据写入wk中
                    ISheet sheet = wk.GetSheetAt(0);   //读取当前表数据
                    IRow row0 = sheet.GetRow(0);  //读取当前行数据
                    DataTable dtt = new DataTable();
                    if (row0 != null)//表头
                    {
                        for (int k = 0; k < row0.LastCellNum; k++)
                        {
                            ICell cell = row0.GetCell(k);
                            string cellName = cell.ToString();
                            dtmain.Columns.Add(cellName, typeof(string));
                        }
                    }
                    for (int j = 1; j <= sheet.LastRowNum; j++)  //LastRowNum 是当前表的总行数
                    {
                        IRow row = sheet.GetRow(j);  //读取当前行数据
                        if (row != null)
                        {
                            DataRow row_col = dtmain.NewRow();
                            for (int n = 0; n < row.LastCellNum; n++)
                            {
                                rowIndex = n;
                                ICell cell = row.GetCell(n);
                                if (cell != null)
                                {
                                    if (cell.ToString().Trim() != string.Empty)
                                    {
                                        row_col[n] = cell.ToString().Trim();
                                    }
                                }
                            }
                            //去除空行
                            //if (row_col[rowIndex].ToString() != string.Empty)
                            //{                                
                            dtmain.Rows.InsertAt(row_col, dtmain.Rows.Count);
                            //}
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HelperLog.Write(ex);
                return null;
            }
            return dtmain;
            #endregion
        }

        /// <summary>
        /// NPOI读取excel去除空行
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>返回解析的dt</returns>
        public static DataTable ProductExcelTrunDataTable(string filename)
        {
            #region NPOI文件解析
            ///读取xls文件
            StringBuilder sbr = new StringBuilder();
            DataTable dtmain = new DataTable();
            int rowIndex = 0;//用来判断一行中是否有空白列

            try
            {
                using (FileStream fs = File.OpenRead(filename))   //打开myxls.xls文件
                {
                    HSSFWorkbook wk = new HSSFWorkbook(fs);   //把xls文件中的数据写入wk中
                    ISheet sheet = wk.GetSheetAt(0);   //读取当前表数据
                    IRow row0 = sheet.GetRow(0);  //读取当前行数据
                    DataTable dtt = new DataTable();
                    if (row0 != null)//表头
                    {
                        int cellCount = 0;//列名总计
                        int colnameCount = row0.LastCellNum;
                        for (int k = 0; k < colnameCount; k++)
                        {
                            ICell cell = row0.GetCell(k);
                            string cellName = cell.ToString();
                            if (cellName != string.Empty)
                            {
                                //220518 黄璐要求增加外加工价格列（张佳陆）
                                if ("产品大类|产品小类|品牌*|条码|编码*|材质*|级别|标准*|类似标准|品名|表色*|规格*|牙别|直径|公称直径|长度|公称长度|单位|头标|单重|成本吨价|是否常备|是否定制|所用线径|毛坯单重|成本价|牌价|牌价来源|一级包装单位|一级包装数量|二级包装单位|二级包装数量|三级包装单位|三级包装数量|一级包装类型|二级包装类型|三级包装类型|库存上限|库存下限|状态|加工价".Contains(cellName))
                                {
                                    dtmain.Columns.Add(cellName, typeof(string));
                                    cellCount++;
                                }
                                else
                                {

                                }
                            }
                        }
                        if (cellCount != colnameCount)
                        {
                            HelperMessageBoxContent.ShowMessageOK("请确认导入的模板是否正确!");
                            return null;
                        }
                    }
                    for (int j = 1; j <= sheet.LastRowNum; j++)  //LastRowNum 是当前表的总行数
                    {
                        rowIndex = 0;
                        IRow row = sheet.GetRow(j);  //读取当前行数据

                        if (row != null)
                        {
                            int lasatCell = row.LastCellNum;
                            DataRow row_col = dtmain.NewRow();
                            for (int n = 0; n < lasatCell; n++)
                            {
                                string cell_Value = string.Empty;
                                ICell cell = row.GetCell(n);
                                string col_Name = dtmain.Columns[n].ColumnName;
                                if (cell != null)
                                {
                                    cell_Value = cell.ToString().Trim();

                                    if (cell_Value == string.Empty)
                                    {
                                        rowIndex++;
                                        string msg = string.Empty;
                                        switch (col_Name)
                                        {
                                            case "条码":
                                                //cell_Value = j.ToString();
                                                break;
                                            //case "编码*":
                                            //    msg = "第" + (j + 1) + "行第" + (n + 1) + "列编码不能为空";
                                            //    break;
                                            //case "材质*":
                                            //    msg = "第" + (j + 1) + "行第" + (n + 1) + "列材质不能为空";
                                            //    break;
                                            //case "标准*":
                                            //    msg = "第" + (j + 1) + "行第" + (n + 1) + "列标准不能为空";
                                            //    break;
                                            //case "表色*":
                                            //    msg = "第" + (j + 1) + "行第" + (n + 1) + "列表色不能为空";
                                            //    break;
                                            //case "规格*":
                                            //    msg = "第" + (j + 1) + "行第" + (n + 1) + "列规格不能为空";
                                            //    break;
                                            //case "品牌*":
                                            //    msg = "第" + (j + 1) + "行第" + (n + 1) + "列品牌不能为空";
                                            //    break;
                                            case "级别":
                                                cell_Value = string.Empty;
                                                break;
                                            case "头标":
                                                cell_Value = string.Empty;
                                                break;
                                            case "公称直径":
                                                cell_Value = cell_Value.ToDecimal().ToString().Trim();
                                                break;
                                            case "公称长度":
                                                cell_Value = cell_Value.ToDecimal().ToString().Trim();
                                                break;
                                            case "单重":
                                                cell_Value = cell_Value.ToDecimal().ToString().Trim();
                                                break;
                                            case "成本吨价":
                                                cell_Value = cell_Value.ToDecimal().ToString().Trim();
                                                break;
                                            case "所用线径":
                                                cell_Value = cell_Value.ToDecimal().ToString().Trim();
                                                break;
                                            case "毛坯单重":
                                                cell_Value = cell_Value.ToDecimal().ToString().Trim();
                                                break;
                                            case "牌价":
                                                cell_Value = cell_Value.ToDecimal().ToString().Trim();
                                                break;
                                            case "一级包装数量":
                                                cell_Value = cell_Value.ToDecimal().ToString().Trim();
                                                break;
                                            case "二级包装数量":
                                                cell_Value = cell_Value.ToDecimal().ToString().Trim();
                                                break;
                                            case "三级包装数量":
                                                cell_Value = cell_Value.ToDecimal().ToString().Trim();
                                                break;
                                            case "库存上限":
                                                cell_Value = cell_Value.ToDecimal().ToString().Trim();
                                                break;
                                            case "库存下限":
                                                cell_Value = cell_Value.ToDecimal().ToString().Trim();
                                                break;
                                            case "是否常备":
                                                if (cell_Value == "是")
                                                {
                                                    cell_Value = "TRUE";
                                                }
                                                else
                                                {
                                                    cell_Value = "FALSE";
                                                }
                                                break;
                                            case "是否定制":
                                                if (cell_Value == "是")
                                                {
                                                    cell_Value = "TRUE";
                                                }
                                                else
                                                {
                                                    cell_Value = "FALSE";
                                                }
                                                break;
                                        }
                                        if (msg != string.Empty)
                                        {
                                            HelperMessageBoxContent.ShowMessageOK(msg);
                                            return null;
                                        }
                                    }
                                    else
                                    {
                                        switch (col_Name)
                                        {
                                            case "条码":
                                                //cell_Value = j.ToString();
                                                break;
                                            case "公称直径":
                                                cell_Value = cell_Value.ToDecimal().ToString().Trim();
                                                break;
                                            case "公称长度":
                                                cell_Value = cell_Value.ToDecimal().ToString().Trim();
                                                break;
                                            case "单重":
                                                cell_Value = cell_Value.ToDecimal().ToString().Trim();
                                                break;
                                            case "成本吨价":
                                                cell_Value = cell_Value.ToDecimal().ToString().Trim();
                                                break;
                                            case "所用线径":
                                                cell_Value = cell_Value.ToDecimal().ToString().Trim();
                                                break;
                                            case "毛坯单重":
                                                cell_Value = cell_Value.ToDecimal().ToString().Trim();
                                                break;
                                            case "成本价":
                                                cell_Value = cell_Value.ToDecimal().ToString().Trim();
                                                break;
                                            case "牌价":
                                                cell_Value = cell_Value.ToDecimal().ToString().Trim();
                                                break;
                                            case "一级包装数量":
                                                cell_Value = cell_Value.ToDecimal().ToString().Trim();
                                                break;
                                            case "二级包装数量":
                                                cell_Value = cell_Value.ToDecimal().ToString().Trim();
                                                break;
                                            case "三级包装数量":
                                                cell_Value = cell_Value.ToDecimal().ToString().Trim();
                                                break;
                                            case "库存上限":
                                                cell_Value = cell_Value.ToDecimal().ToString().Trim();
                                                break;
                                            case "库存下限":
                                                cell_Value = cell_Value.ToDecimal().ToString().Trim();
                                                break;
                                            case "是否常备":
                                                if (cell_Value == "是")
                                                {
                                                    cell_Value = "TRUE";
                                                }
                                                else
                                                {
                                                    cell_Value = "FALSE";
                                                }
                                                break;
                                            case "是否定制":
                                                if (cell_Value == "是")
                                                {
                                                    cell_Value = "TRUE";
                                                }
                                                else
                                                {
                                                    cell_Value = "FALSE";
                                                }
                                                break;
                                        }
                                    }
                                    row_col[n] = cell_Value;
                                }
                            }
                            if (rowIndex < lasatCell)
                            {
                                dtmain.Rows.InsertAt(row_col, dtmain.Rows.Count);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HelperMessageBoxContent.ShowMessageOK("EXCEL解析失败,请关闭EXCEL重试！请检查excel版本，推荐使用Excel 97-2003 文件");
                HelperLog.Write(ex);
                return null;
            }
            return dtmain;
            #endregion
        }

        /// <summary>
        /// 将dataTable导出成Excel
        /// </summary>
        /// <param name="dt">所需要导出的数据集合</param>
        /// <param name="fileName">所需要导出的文件名（例如：XXX.xls）</param>
        /// param name="isShowExcle">导出后是否打开文件</param>  
        public static void DataTableToExcel(DataTable dt, string fileName)
        {
            if (dt != null)
            {
                TableToExcel(dt, fileName);
                HelperMessageBoxContent.ShowMessageOK("数据导出成功！");
            }
            else
            {
                HelperMessageBoxContent.ShowMessageOK("没有数据可供查出，请核实后在重新导出！");
            }
        }



        /// <summary>
        /// Datable导出成Excel
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="file">导出路径(包括文件名与扩展名)</param>
        public static string TableToExcel(DataTable dt, string file, params string[] arg)
        {
            C1.C1Excel.C1XLBook c1XLBook1 = new C1XLBook();
            XLSheet sheet = c1XLBook1.Sheets[0];
            if (arg.Length > 0)
            {
                for (int i = 0; i < arg.Length; i++)
                {
                    dt.Columns.Add(arg[i]);
                }
            }


            //添加标题行
            int c = 0;
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                //if (c1TrueDBGrid1.Splits[0].DisplayColumns[i].Visible == true)
                //{
                sheet[0, c].Value = dt.Columns[i];
                c++;
                //  }
            }
            //内容
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int show = 0;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    //if (c1TrueDBGrid1.Splits[0].DisplayColumns[j].Visible == true)
                    //{
                    if (dt.Columns[j].DataType == typeof(DateTime))
                    {
                        if (dt.Rows[i][j].ToString() != "")
                        {
                            sheet[i + 1, show].Value = DateTime.Parse(dt.Rows[i][j].ToString()).ToString("yyyy-MM-dd");
                        }
                    }
                    else
                    {
                        sheet[i + 1, show].Value = dt.Rows[i][j].ToString();
                    }
                    show++;
                    //}
                }
            }
            SaveFileDialog dlg1 = new SaveFileDialog();//导出地址
            dlg1.DefaultExt = "xls";
            dlg1.FileName = file + DateTime.Now.ToString("yyyyMMdd") + ".xls";
            dlg1.RestoreDirectory = true;
            if (dlg1.ShowDialog() == DialogResult.OK)
            {
                string dialogPath = dlg1.FileName.Substring(0, dlg1.FileName.LastIndexOf("\\"));
                c1XLBook1.Save(dlg1.FileName);
                return dialogPath;
            }
            return string.Empty;
        }


    }

}
