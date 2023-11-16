using FX.Entity;
using FX.ORM.Websharp.ORM.Base;
//-----------------------------------------------------------------------
// <copyright company="工品一号" file="ComboBoxHelper.cs">
//  Copyright (c)  V1.0.0.0  
//  创建作者:   刘少林
//  创建时间:   2018-03-29
//  功能描述:   ComboBox控件帮助类
//  历史版本:
//          2018-03-29 刘少林 创建ComboBoxHelper.cs
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;

namespace FX.MainForms
{
    /// <summary>
    ///  ComboBox控件帮助类
    /// </summary>
    public class HelperComboBox
    {

        /// <summary>
        /// 给ComboBox控件绑定指定的ORM实体基础数组对象
        /// </summary>
        /// <param name="box">ComboBox控件</param>
        /// <param name="enumType">ORM实体基础数组</param>
        /// <param name="displayName">显示名称</param>
        /// <returns>persistenceCapables控件对象，方便链式操作</returns>
        public static ComboBox BindingByPersistenceCapables(ComboBox box, PersistenceCapable[] persistenceCapables, string displayName)
        {
            box.DataSource = persistenceCapables;
            box.DisplayMember = displayName;
            box.ValueMember = displayName;
            //box.Items.Insert(0, string.Empty);
            return box;
        }

        /// <summary>
        /// 给ComboBox控件绑定指定枚举数据
        /// </summary>
        /// <param name="box">ComboBox控件</param>
        /// <param name="enumType">枚举类型</param>
        /// <param name="finalTypes">最终显示的下拉框类型值</param>
        /// <returns>ComboBox控件对象，方便链式操作</returns>
        public static ComboBox BindingByEnum(ComboBox box, Type enumType, params string[] finalTypes)
        {
            HelperEnum.BindDesEnumToComboBox(box, enumType);
            if (finalTypes != null && finalTypes.Length > 0)
            {
                //数组参数为最终显示的下拉框数目
                //有时候一个枚举包含了多个状态配置，但是某些视图只需要显示其中一部分(这个就是最终显示的下拉框数目)
                IList<object> deleted = new List<object>();
                foreach (object item in box.Items)
                {
                    bool existed = false;
                    foreach (string type in finalTypes)
                    {
                        if (type.Equals(item.ToString()))
                        {
                            existed = true;
                            break;
                        }
                    }
                    if (!existed)
                    {
                        deleted.Add(item);
                    }
                }
                if (deleted.Count > 0)
                {
                    foreach (object item in deleted)
                    {
                        box.Items.Remove(item);
                    }
                }
            }
            box.Items.Insert(0, string.Empty);
            return box;
        }

        /// <summary>
        /// 给ComboBox控件绑定指定枚举数据
        /// </summary>
        /// <param name="box">ComboBox控件</param>
        /// <param name="enumType">枚举类型</param>
        /// <param name="finalTypes">最终显示的下拉框类型值</param>
        /// <returns>ComboBox控件对象，方便链式操作</returns>
        public static ComboBox BindingByEnumValue(ComboBox box, Type enumType)
        {
            HelperEnum.BindDesAndValueEnumToComboBox(box, enumType);
            return box;
        }

        /// <summary>
        /// 给ComboBox控件绑定指定自定义数据
        /// </summary>
        /// <param name="box">ComboBox控件</param>
        /// <param name="data">自定义数据数组</param>
        /// <returns>ComboBox控件对象，方便链式操作</returns>
        public static ComboBox BindingByCustomData(ComboBox box, params string[] data)
        {
            if (data != null)
            {
                foreach (string item in data)
                {
                    box.Items.Add(item);
                }
            }
            box.Items.Insert(0, string.Empty);
            return box;
        }

        /// <summary>
        /// 给ComboBox控件绑定指定列数据
        /// </summary>
        /// <param name="box">ComboBox控件</param>
        /// <param name="tableName">数据所属表名称</param>
        /// <param name="displayMember">显示数据</param>
        /// <param name="valueMember">控件对应值</param>
        /// <returns>ComboBox控件对象，方便链式操作</returns>
        public static ComboBox BindingByTable(ComboBox box, string tableName, string displayMember)
        {
            string valueMember = displayMember;
            return BindingByTable(box, tableName, displayMember, valueMember);
        }

        /// <summary>
        /// 给ComboBox控件绑定指定列数据
        /// </summary>
        /// <param name="box">ComboBox控件</param>
        /// <param name="tableName">数据所属表名称</param>
        /// <param name="displayMember">显示数据</param>
        /// <param name="valueMember">控件对应值</param>
        /// <returns>ComboBox控件对象，方便链式操作</returns>
        public static ComboBox BindingByTable(ComboBox box, string tableName, string displayMember, string valueMember)
        {
            string sql = "SELECT " + displayMember + "," + valueMember + " FROM " + tableName + " WITH(NOLOCK) ";
            return BindingBySQL(box, sql, displayMember, valueMember);
        }



        /// <summary>
        /// 给ComboBox控件绑定指定父字典的下级字典列表
        /// </summary>
        /// <param name="box">ComboBox控件</param>
        /// <param name="Id">父字典的主键</param>
        /// <returns>ComboBox控件对象，方便链式操作</returns>
        /// <remarks>本方法是获取字典并使用字典名称作为下拉框使用，
        /// 字典主键参数就是父字典主键</remarks>
        public static ComboBox BindingByDictionaryById(ComboBox box, string Id)
        {
            string sql = "SELECT " + Sys_GlobalDict.SGDName_ColumnName + " FROM " + DBInformation.TableNames.Sys_GlobalDict + " WITH(NOLOCK) WHERE " + Sys_GlobalDict.SGDParentId_ColumnName + " = @" + Sys_GlobalDict.SGDParentId_ColumnName;
            return BindingBySQL(box, sql, Sys_GlobalDict.SGDName_ColumnName, Sys_GlobalDict.SGDName_ColumnName, new SqlParameter("@" + Sys_GlobalDict.SGDParentId_ColumnName, Id));
        }

        /// <summary>
        /// 给ComboBox控件绑定指定父字典的下级字典列表
        /// </summary>
        /// <param name="box">ComboBox控件</param>
        /// <param name="code">父字典的编码</param>
        /// <returns>ComboBox控件对象，方便链式操作</returns>
        /// <remarks>本方法是获取字典并使用字典名称作为下拉框使用，
        /// 字典主键参数就是父字典主键</remarks>
        public static ComboBox BindingByDictionaryByCode(ComboBox box, string code)
        {
            //if (!string.IsNullOrEmpty(code))
            //{
            //    return PersistenceGlobalData.BindingBoxForDictByParentCode(box, code);
            //}
            string sql = "SELECT DISTINCT " + Sys_GlobalDict.SGDName_ColumnName + " FROM " + DBInformation.TableNames.Sys_GlobalDict + " WITH(NOLOCK) WHERE " + Sys_GlobalDict.SGDParentCode_ColumnName + " = @" + Sys_GlobalDict.SGDParentCode_ColumnName;
            return BindingBySQL(box, sql, Sys_GlobalDict.SGDName_ColumnName, Sys_GlobalDict.SGDName_ColumnName, new SqlParameter("@" + Sys_GlobalDict.SGDParentCode_ColumnName, code));
        }

        /// <summary>
        /// 给ComboBox控件绑定指定父字典的下级字典列表
        /// </summary>
        /// <param name="box">ComboBox控件</param>
        /// <param name="code">父字典的编码</param>
        /// <returns>ComboBox控件对象，方便链式操作</returns>
        /// <remarks>本方法是获取字典并使用字典名称作为下拉框使用，
        /// 字典主键参数就是父字典主键</remarks>
        public static ComboBox BindingByDictionaryByCodeWithNormal(ComboBox box, string code)
        {
            //if (!string.IsNullOrEmpty(code))
            //{
            //    return PersistenceGlobalData.BindingBoxForDictByParentCode(box, code);
            //}
            string sql = $"SELECT DISTINCT {Sys_GlobalDict.SGDName_ColumnName} FROM {DBInformation.TableNames.Sys_GlobalDict} WITH(NOLOCK) WHERE {Sys_GlobalDict.SGDParentCode_ColumnName}=@{Sys_GlobalDict.SGDParentCode_ColumnName} AND {Sys_GlobalDict.SGDStatus_ColumnName} ='启用'";
            return BindingBySQL(box, sql, Sys_GlobalDict.SGDName_ColumnName, Sys_GlobalDict.SGDName_ColumnName, new SqlParameter("@" + Sys_GlobalDict.SGDParentCode_ColumnName, code));
        }

        //public static ComboBox BindSingleDistinctColumnBySQL(ComboBox box, string sql, string displayMember, string valueMember, params SqlParameter[] parameters) { }

        /// <summary>
        /// 给ComboBox控件绑定指定列数据
        /// </summary>
        /// <param name="box">ComboBox控件</param>
        /// <param name="tableName">数据所属表名称</param>
        /// <param name="displayMember">显示数据</param>
        /// <param name="valueMember">控件对应值</param>
        /// <param name="parameters">参数化对象</param>
        /// <returns>ComboBox控件对象，方便链式操作</returns>
        public static ComboBox BindingBySQL(ComboBox box, string sql, string displayMember, string valueMember, params SqlParameter[] parameters)
        {
            DataTable table = SQL.GetDataTable(sql, parameters);
            if (table != null)
            {
                if (valueMember == Bas_Warehouses.BWHId_ColumnName)
                {
                    if (table.Rows.Count != 1)
                    {
                        DataRow row = table.NewRow();
                        row[displayMember] = "";
                        table.Rows.InsertAt(row, 0);
                    }
                    else
                    {
                        DataRow row = table.NewRow();
                        row[displayMember] = "";
                        table.Rows.InsertAt(row, 0);
                    }
                }
                else
                {
                    DataRow row = table.NewRow();
                    row[displayMember] = "";
                    table.Rows.InsertAt(row, 0);
                }

                List<string> sList = new List<string>();
                foreach (DataRow drr in table.Rows)
                {
                    sList.Add(drr[displayMember].ToString());
                }
                box.AutoCompleteCustomSource.AddRange(sList.ToArray());
                box.AutoCompleteSource = AutoCompleteSource.ListItems;
                box.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                box.DataSource = table;
                box.DisplayMember = displayMember;
                box.ValueMember = valueMember;
            }
            //20190110 注销肖长春添加的三行代码，此三行代码引起其他问题！ by arison!
            //if (table.Rows.Count == 1)
            //{
            //    box.SelectedIndex = 1;
            //}
            return box;
        }
        /// <summary>
        /// 给ComboBox控件绑定指定列数据
        /// </summary>
        /// <param name="box">ComboBox控件</param>
        /// <param name="tableName">数据所属表名称</param>
        /// <param name="displayMember">显示数据</param>
        /// <param name="valueMember">控件对应值</param>
        /// <param name="parameters">参数化对象</param>
        /// <returns>ComboBox控件对象，方便链式操作</returns>
        public static ComboBox BindingByTable(ComboBox box, DataTable dt, string displayMember, string valueMember)
        {

            DataRow row = dt.NewRow();
            row[displayMember] = "";
            dt.Rows.InsertAt(row, 0);
            box.DataSource = dt;
            box.DisplayMember = displayMember;
            box.ValueMember = valueMember;
            return box;
        }

        /// <summary>
        /// 给ComboBox控件绑定指定列数据
        /// </summary>
        /// <param name="box">ComboBox控件</param>
        /// <param name="tableName">数据所属表名称</param>
        /// <param name="displayMember">显示数据</param>
        /// <param name="valueMember">控件对应值</param>
        /// <param name="parameters">参数化对象</param>
        /// <returns>ComboBox控件对象，方便链式操作</returns>
        public static ComboBox BindingBySQLNum(ComboBox box, string sql, string displayMember, string valueMember, params SqlParameter[] parameters)
        {
            DataTable table = null;
            table = SQL.GetDataTable(sql, parameters);
            if (valueMember == Bas_Warehouses.BWHId_ColumnName)
            {
                if (table.Rows.Count != 1)
                {
                    DataRow row = table.NewRow();
                    //row[displayMember] = "";
                    table.Rows.InsertAt(row, 0);
                }
            }
            else
            {
                DataRow row = table.NewRow();
                //row[displayMember] = "";
                table.Rows.InsertAt(row, 0);
            }
            box.DataSource = table;
            box.DisplayMember = displayMember;
            box.ValueMember = valueMember;

            //20190110 注销肖长春添加的三行代码，此三行代码引起其他问题！ by arison!
            //if (table.Rows.Count == 1)
            //{
            //    box.SelectedIndex = 1;
            //}
            return box;
        }

        /// <summary>
        /// 给ComboBox控件绑定指定列数据
        /// </summary>
        /// <param name="box">ComboBox控件</param>
        /// <param name="tableName">数据所属表名称</param>
        /// <param name="displayMember">显示数据</param>
        /// <param name="valueMember">控件对应值</param>
        /// <param name="parameters">参数化对象</param>
        /// <returns>ComboBox控件对象，方便链式操作</returns>
        public static ComboBox BindingWarehouseBySQL(ComboBox box, string sql, string displayMember, string valueMember, params SqlParameter[] parameters)
        {
            DataTable table = null;
            table = SQL.GetDataTable(sql, parameters);
            DataRow row = table.NewRow();
            row[displayMember] = "";
            table.Rows.InsertAt(row, 0);
            List<string> sList = new List<string>();
            foreach (DataRow drr in table.Rows)
            {
                sList.Add(drr[displayMember].ToString());
            }
            box.AutoCompleteCustomSource.AddRange(sList.ToArray());
            box.AutoCompleteSource = AutoCompleteSource.ListItems;
            box.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            box.DataSource = table;
            box.DisplayMember = displayMember;
            box.ValueMember = valueMember;
            if (table.Rows.Count == 2)
            {
                box.Text = table.Rows[1][displayMember].ToString();
            }
            return box;
        }

    }
}
