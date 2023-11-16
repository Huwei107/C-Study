//-----------------------------------------------------------------------
// <copyright company="工品一号" file="DataTableHelper.cs">
//  Copyright (c)  V1.0.0.0  
//  创建作者:   刘少林
//  创建时间:   2018-03-29
//  功能描述:   DataTable帮助类
//  历史版本:
//          2018-03-29 刘少林 创建DataTableHelper.cs
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FX.MainForms
{
    /// <summary>
    /// DataTable帮助类
    /// </summary>
    public class HelperDataTable
    {
        /// <summary>
        /// 将两个列不同的主子DataTable合并成一个新的DataTable
        /// </summary>
        /// <param name="dt1">主表（1条数据）</param>
        /// <param name="dt2">子表(多条)</param>
        /// <param name="DTName">合并后新的表名</param>
        /// <returns></returns>
        public static DataTable UniteDataTable(DataTable dt1, DataTable dt2, string DTName = "tb")
        {
            if (dt1.Rows.Count != 1)
            {
                return dt1;
            }
            DataTable dt3 = dt1.Clone();

            for (int i = 0; i < dt2.Columns.Count; i++)
            {
                dt3.Columns.Add(dt2.Columns[i].ColumnName);
            }
            object[] obj = new object[dt3.Columns.Count];

            dt1.Rows[0].ItemArray.CopyTo(obj, 0);
            for (int i = 0; i < dt2.Rows.Count; ++i)
            {
                dt3.Rows.Add(obj);
            }

            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                for (int j = 0; j < dt2.Columns.Count; j++)
                {
                    dt3.Rows[i][j + dt1.Columns.Count] = dt2.Rows[i][j].ToString();
                }
            }
            dt3.TableName = DTName; //设置DT的名字
            return dt3;
        }

        /// <summary>
        /// DataTable行转列
        /// </summary>
        /// <param name="dtable">需要转换的表</param>
        /// <param name="head">转换表表头对应旧表字段（小写）</param>
        /// <returns></returns>
        public static DataTable DataTableRowtoCol(DataTable dtable, string head)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("NumberID");
            for (int i = 0; i < dtable.Rows.Count; i++)
            {//设置表头
                dt.Columns.Add(dtable.Rows[i][head].ToString());
            }
            for (int k = 0; k < dtable.Columns.Count; k++)
            {
                string temcol = dtable.Columns[k].ToString();
                if (dtable.Columns[k].ToString().ToLower() != head)//过滤掉设置表头的列
                {
                    DataRow new_dr = dt.NewRow();
                    new_dr[0] = dtable.Columns[k].ToString();
                    for (int j = 0; j < dtable.Rows.Count; j++)
                    {
                        string temp = dtable.Rows[j][k].ToString();
                        new_dr[j + 1] = (Object)temp;
                    }
                    dt.Rows.Add(new_dr);
                }
            }
            return dt;
        }

        /// <summary>
        /// 给DataTable新增一列
        /// </summary>
        /// <param name="dt">据源</param>
        /// <param name="colName">新增列名称</param>
        /// <param name="t">数据类型</param>
        /// <param name="ordinal">指定位置</param>
        /// <param name="dValue">默认值</param>
        public void AddDataTableColumns(DataTable dt, string colName, Type t, int ordinal, object dValue)
        {
            if (!dt.Columns.Contains(colName))
            {
                var dc = new DataColumn(colName, t) { DefaultValue = dValue };
                dt.Columns.Add(dc);
                dt.Columns[colName].SetOrdinal(ordinal);
            }
        }

        /// <summary>
        /// 新增DataTable空行
        /// </summary>
        /// <param name="dt">数据源</param>
        /// <param name="row">默认15行</param>
        /// <param name="row">是否把第2列改成行号true?显示行号:无</param>
        /// <returns>带空行的DataTable</returns>
        public static DataTable AddDataTableNewRow(DataTable dt, int row = 15, bool line = true)
        {
            for (int j = 0; j < row; j++)
            {
                if (dt != null && dt.Columns.Count > 0)
                {
                    int r = dt.Rows.Count;
                    int col = dt.Columns.Count;
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < col; i++)
                    {
                        Type dataType = dt.Columns[i].DataType;
                        if (dataType.Name == "Boolean")
                        {
                            dr[i] = false;
                        }
                        else
                        {
                            dr[i] = DBNull.Value;
                        }
                    }
                    if (line)
                    {
                        dr[1] = r + 1;
                    }
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }

        /// <summary>
        /// 判断DataTable数据有效性
        /// </summary>
        /// <param name="dt">数据源</param>
        /// <returns>true?有效：无效</returns>
        public static bool DataTableValid(DataTable dt)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 判断DataTale中判断某个字段中包含某个数据
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="columnName"></param>
        /// <param name="fieldData"></param>
        /// <returns></returns>
        public static bool IsColumnIncludeData(DataTable dt, String columnName, string fieldData)
        {
            if (dt == null)
            {
                return false;
            }
            else
            {
                DataRow[] dataRows = dt.Select(columnName + "='" + fieldData + "'");
                if (dataRows.Length.Equals(1))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

        #region 根据datatable获得列名
        /// <summary>
        /// 根据datatable获得列名
        /// </summary>
        /// <param name="dt">表对象</param>
        /// <returns>返回结果的数据列数组</returns>
        public static string GetColumnsByDataTable(DataTable dt)
        {
            string strColumns = string.Empty;
            if (dt.Columns.Count > 0)
            {
                int columnNum = 0;
                columnNum = dt.Columns.Count;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    strColumns += "|" + dt.Columns[i].ColumnName;
                }
            }
            return strColumns;
        }
        #endregion

        /// <summary>
        /// 将两个列不同的DataTable合并成一个新的DataTable
        /// </summary>
        /// <param name="dt1">表1</param>
        /// <param name="dt2">表2</param>
        /// <param name="DTName">合并后新的表名</param>
        /// <returns></returns>
        public static DataTable UniteDataTable2(DataTable dt1, DataTable dt2, string DTName = "dt")
        {
            DataTable dt3 = dt1.Clone();
            for (int i = 0; i < dt2.Columns.Count; i++)
            {
                dt3.Columns.Add(dt2.Columns[i].ColumnName);
            }
            object[] obj = new object[dt3.Columns.Count];

            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                dt1.Rows[i].ItemArray.CopyTo(obj, 0);
                dt3.Rows.Add(obj);
            }

            if (dt1.Rows.Count >= dt2.Rows.Count)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    for (int j = 0; j < dt2.Columns.Count; j++)
                    {
                        dt3.Rows[i][j + dt1.Columns.Count] = dt2.Rows[i][j].ToString();
                    }
                }
            }
            else
            {
                DataRow dr3;
                for (int i = 0; i < dt2.Rows.Count - dt1.Rows.Count; i++)
                {
                    dr3 = dt3.NewRow();
                    dt3.Rows.Add(dr3);
                }
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    for (int j = 0; j < dt2.Columns.Count; j++)
                    {
                        dt3.Rows[i][j + dt1.Columns.Count] = dt2.Rows[i][j].ToString();
                    }
                }
            }
            dt3.TableName = DTName; //设置DT的名字
            return dt3;
        }


        /// <summary>
        /// 将两个相同的DataTable合并成一个新的DataTable
        /// </summary>
        /// <param name="dt1">表1</param>
        /// <param name="dt2">表2</param>
        /// <returns></returns>
        public static DataTable MerageDataTable(DataTable dt1, DataTable dt2)
        {
            //拷贝DataTable1的结构和数据
            DataTable newDataTable = dt1.Copy();
            //添加DataTable2的数据
            foreach (DataRow dr in dt2.Rows)
            {
                newDataTable.ImportRow(dr);
            }
            return newDataTable;
        }

        /// <summary>
        /// 数组转datatable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static DataTable ConvertToDataTable<T>(List<T> collection)
        {
            var props = typeof(T).GetProperties();
            var dt = new DataTable();

            var dataColumns = props.Select(p => new DataColumn(p.Name,
                  ((p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) ? p.PropertyType.GetGenericArguments()[0] : p.PropertyType)
                  )).ToArray();
            dt.Columns.AddRange(dataColumns);
            foreach (var i in collection)
            {
                var row = dt.NewRow();
                ArrayList tempList = new ArrayList();
                foreach (PropertyInfo pi in props)
                {
                    object obj = pi.GetValue(i, null) == null ? DBNull.Value : pi.GetValue(i, null); //pi.GetValue(collection.ElementAt(i), null);
                    row[pi.Name] = obj;
                }
                dt.Rows.Add(row);
            }
            return dt;
        }
    }
}
