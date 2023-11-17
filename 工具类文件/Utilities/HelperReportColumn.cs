//-----------------------------------------------------------------------
// <copyright company="工品一号" file="HelperReportColumn.cs">
//  Copyright (c)  V1.0.0.0  
//  创建作者:   王健
//  创建时间:   2023/10/18 16:34:45 
//  功能描述:   
//  历史版本:
//          2023/10/18 16:34:45 王健 创建HelperReportColumn类
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FX.MainForms
{
    /// <summary>
    /// 打印报表列扩展工具类
    /// </summary>
    public static class HelperReportColumn
    {
        /// <summary>
        /// 获取列
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<ReportColumnDto> ReportColumns(this Type type)
        {
            var list = new List<ReportColumnDto>();
            var props = new List<PropertyInfo>(type.GetProperties(BindingFlags.Public | BindingFlags.Instance));
            for (int i = 0; i < props.Count(); i++)
            {
                var prop = props[i];
                var ignore = prop.GetCustomAttributes<IgnoreCustomerAttribute>(false);
                if (ignore.Count() > 0 && ignore.Any(x => x.IgnoreConvert)) continue;
                var dto = new ReportColumnDto() { Tag = prop.Name,  Property = prop };
                var objAttrs = prop.GetCustomAttributes<ReportColumnAttribute>(false);
                if (objAttrs.Any())
                {
                    var attr = objAttrs.FirstOrDefault();
                    if (attr != null)
                    {
                        dto.Description = string.IsNullOrEmpty(attr.Description) ? prop.Name : attr.Description;
                        dto.Value = string.IsNullOrEmpty(attr.Value) ? prop.Name : attr.Value;
                        dto.Name = string.IsNullOrEmpty(attr.Value) ? prop.Name : attr.Name;
                        list.Add(dto);
                        continue;
                    }
                }
                dto.Description = prop.Name;
                dto.Value = prop.Name;
                dto.Name = prop.Name;
                list.Add(dto);
            }
            return list;
        }

        /// <summary>
        /// 列表获取报表列表格
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static DataTable ListToReportColumnsDataTable<T>(this List<T> list, string tableName = "")
        {
            var props = typeof(T).ReportColumns();
            return ConvertToDataTable(list, props, tableName);
        }

        /// <summary>
        /// 列表获取报表列表格
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static void DataTableToReportColumnsDataTable<T>(this DataTable table)
        {
            var props = typeof(T).ReportColumns();
            DataTableToDataTable(table, props);
        }

        /// <summary>
        /// 数组转表格
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">数据</param>
        /// <param name="tb">表格</param>
        /// <param name="props">属性集合</param>
        /// <returns></returns>
        private static DataTable ConvertToDataTable<T>(List<T> list, List<ReportColumnDto> props, string tableName = "")
        {
            tableName = string.IsNullOrEmpty(tableName) ? typeof(T).Name : tableName;
            var tb = new DataTable(tableName);
            foreach (var prop in props)
            {
                Type colType = prop.Property.PropertyType;
                if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                {
                    colType = colType.GetGenericArguments()[0];
                }
                tb.Columns.Add(prop.Property.Name, colType);
            }
            foreach (T item in list)
            {
                var values = new object[props.Count];
                for (int i = 0; i < props.Count; i++)
                {
                    values[i] = props[i].Property.GetValue(item, null);
                }
                tb.Rows.Add(values);
            }
            foreach (var prop in props)
            {
                var objAttrs = prop.Property.GetCustomAttributes<ReportColumnAttribute>(false).ToList();
                if (objAttrs.Any())
                {
                    var attr = objAttrs.First();

                    if (attr != null)
                    {
                        var columnName = string.IsNullOrEmpty(attr.Value) ? prop.Property.Name : attr.Value;
                        tb.Columns[prop.Property.Name].ColumnName = columnName;
                        break;
                    }

                }
            }
            return tb;
        }

        /// <summary>
        /// 表格转换
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="props"></param>
        private static void DataTableToDataTable(DataTable dataTable, List<ReportColumnDto> props)
        {
            foreach (var prop in props)
            {
                var objAttrs = prop.Property.GetCustomAttributes<ReportColumnAttribute>(false).ToList();
                if (objAttrs.Any())
                {
                    var attr = objAttrs.First();

                    if (attr != null)
                    {
                        var columnName = string.IsNullOrEmpty(attr.Value) ? prop.Property.Name : attr.Value;
                        if (dataTable.Columns.Contains(prop.Property.Name)) dataTable.Columns[prop.Property.Name].ColumnName = columnName;
                        break;
                    }

                }
            }
        }
    }
}
