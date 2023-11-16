//-----------------------------------------------------------------------
// <copyright company="工品一号" file="HelperC1TrueDBGridTableExtension.cs">
//  Copyright (c)  V1.0.0.0  
//  创建作者:   王健
//  创建时间:   2023/2/24 15:24:30 
//  功能描述:   
//  历史版本:
//          2023/2/24 15:24:30 王健 创建HelperC1TrueDBGridTableExtension类
// </copyright>
//-----------------------------------------------------------------------
using C1.Win.C1TrueDBGrid;
using ReportUtil.Attibutes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FX.MainForms
{
    /// <summary>
    /// C1TrueDBGrid表格拓展类
    /// </summary>
    public static class HelperC1TrueDBGridTableExtension
    {
        /// <summary>
        /// 加载表格数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="grid">表格</param>
        /// <param name="data">数据表格</param>
        /// <param name="columns">列配置</param>
        /// <param name="addOrderNumber">增加序号</param>
        /// <param name="hasChoose">是否增加选择</param>
        /// <param name="IsLike">模糊查询</param>
        /// <param name="filter">筛选列</param>
        /// <param name="rowHeight">行高</param>
        /// <param name="fontSize">文字大小</param>
        public static void LoadData<T>(this C1.Win.C1TrueDBGrid.C1TrueDBGrid grid, string moduleTag, DataTable data,List<TableColumnDto> columns = null,bool addOrderNumber = true, bool hasChoose = false,  bool IsLike = true, bool filter = true, int rowHeight = 22, int fontSize = 10) where T : class, new()
        {
            List<TableColumnDto> list = GetColumnInfos<T>(grid.Name, moduleTag, columns);
            InitGrid(grid, IsLike, filter, rowHeight, fontSize);
            grid.LoadData(list, data, hasChoose);

            if(addOrderNumber) CreateNumber(grid);
        }

        /// <summary>
        /// 根据实体类加载表格数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="grid">表格</param>
        /// <param name="data">数据表格</param>
        /// <param name="columns">列配置</param>
        /// <param name="addOrderNumber">增加序号</param>
        /// <param name="IsLike">模糊查询</param>
        /// <param name="filter">筛选列</param>
        /// <param name="rowHeight">行高</param>
        /// <param name="fontSize">文字大小</param>
        public static void LoadDataThroughList<T>(this C1.Win.C1TrueDBGrid.C1TrueDBGrid grid, string moduleTag, DataTable data, List<TableColumnDto> columns = null, bool addOrderNumber = true,  
            bool IsLike = true, bool filter = true, int rowHeight = 22, int fontSize = 10) where T : class, new()
        {
            List<TableColumnDto> list = GetColumnInfos<T>(grid.Name, moduleTag, columns);
            data = ConvertToDataTable(list, ConvertToList<T>(data));
            InitGrid(grid, IsLike, filter, rowHeight, fontSize);
            grid.LoadDataThroughList(list, data);

            if (addOrderNumber) CreateNumber(grid);
        }

        /// <summary>
        /// 根据实体类加载表格数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="grid">表格</param>
        /// <param name="data">数据表格</param>
        /// <param name="columns">列配置</param>
        /// <param name="addOrderNumber">增加序号</param>
        /// <param name="IsLike">模糊查询</param>
        /// <param name="filter">筛选列</param>
        /// <param name="rowHeight">行高</param>
        /// <param name="fontSize">文字大小</param>
        public static void LoadDataThroughList<T>(this C1.Win.C1TrueDBGrid.C1TrueDBGrid grid, string moduleTag, List<T> data, List<TableColumnDto> columns = null, bool addOrderNumber = true,
           bool IsLike = true, bool filter = true, int rowHeight = 22, int fontSize = 10) where T : class, new()
        {
            List<TableColumnDto> list = GetColumnInfos<T>(grid.Name, moduleTag, columns);
            var dt = ConvertToDataTable(list, data);
            InitGrid(grid, IsLike, filter, rowHeight, fontSize);
            grid.LoadDataThroughList(list, dt);

            if (addOrderNumber) CreateNumber(grid);
        }

        /// <summary>
        /// 刷新表格
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="grid">表格</param>
        /// <param name="moduleTag"></param>
        /// <param name="columns"></param>
        public static void RefreshGridView<T>(this C1.Win.C1TrueDBGrid.C1TrueDBGrid grid, string moduleTag, List<TableColumnDto> columns = null) where T : class, new()
        {
            List<TableColumnDto> list = GetColumnInfos<T>(grid.Name, moduleTag, columns);
            grid.RefreshGrid(list);
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="grid">表格</param>
        /// <param name="tableCulomns">列配置</param>
        /// <param name="data">数据表格</param>
        /// <param name="hasChoose">是否增加选择</param>
        private static void LoadData(this C1.Win.C1TrueDBGrid.C1TrueDBGrid grid, List<TableColumnDto> tableCulomns,DataTable data, bool hasChoose)
        {
            if (hasChoose)
            {
                if (!data.Columns.Contains("Choose")) data.Columns.Add("Choose", typeof(bool));
                data.Columns["Choose"].SetOrdinal(0);
                foreach (DataRow item in data.Rows)
                {
                    item["Choose"] = false;
                }
            }
            for (int i = 0; i < tableCulomns.Count; i++)
            {
                var dto = tableCulomns[i];
                if (!data.Columns.Contains(dto.Tag))
                {
                    data.Columns.Add(dto.Tag);
                }
                data.Columns[dto.Tag].SetOrdinal(hasChoose ? i + 1 : i);
            }
            grid.DataSource = data;
            for (int i = 0; i < tableCulomns.Count; i++)
            {
                var dto = tableCulomns[i];
                if (!data.Columns.Contains(dto.Tag)) continue;
                grid.Columns[dto.Tag].Caption = dto.Description;
                if (dto.IsAutoSize) grid.Splits[0].DisplayColumns[dto.Tag].AutoSize();
                grid.Splits[0].DisplayColumns[dto.Tag].Merge = (ColumnMergeEnum)dto.MergeType;
                grid.Splits[0].DisplayColumns[dto.Tag].Locked = !dto.IsEdit;
                grid.Splits[0].DisplayColumns[dto.Tag].Visible = !dto.IsHide;
            }
            if (hasChoose)
            {
                grid.Columns["Choose"].Caption = "选择";
                grid.Columns["选择"].ValueItems.Presentation = C1.Win.C1TrueDBGrid.PresentationEnum.CheckBox;
                grid.Splits[0].DisplayColumns["选择"].Width = 40;
            }
            foreach (C1DataColumn column in grid.Columns)
            {
                grid.Splits[0].DisplayColumns[column.Caption].HeadingStyle.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center;
                if (column.DataType == typeof(decimal))
                    grid.Splits[0].DisplayColumns[column.Caption].Style.HorizontalAlignment = AlignHorzEnum.Far;
                else if (column.DataType == typeof(bool))
                    grid.Splits[0].DisplayColumns[column.Caption].Style.HorizontalAlignment = AlignHorzEnum.Center;
                else
                    grid.Splits[0].DisplayColumns[column.Caption].Style.HorizontalAlignment = AlignHorzEnum.Near;              
            }

        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="grid">表格</param>
        /// <param name="tableCulomns">列配置</param>
        /// <param name="data">数据表格</param>
        private static void LoadDataThroughList(this C1.Win.C1TrueDBGrid.C1TrueDBGrid grid, List<TableColumnDto> tableCulomns, DataTable data)
        {
            grid.DataSource = data;
            for (int i = 0; i < tableCulomns.Count; i++)
            {
                var dto = tableCulomns[i];
                if (!data.Columns.Contains(dto.Tag)) continue;
                grid.Columns[dto.Tag].Caption = dto.Description;
                grid.Splits[0].DisplayColumns[dto.Tag].HeadingStyle.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center;
                if (dto.IsAutoSize) 
                { 
                    grid.Splits[0].DisplayColumns[dto.Tag].AutoSize();
                }
                else { grid.Splits[0].DisplayColumns[dto.Tag].Width = dto.Width; }
                grid.Splits[0].DisplayColumns[dto.Tag].Merge = (ColumnMergeEnum)dto.MergeType;
                grid.Splits[0].DisplayColumns[dto.Tag].Locked = !dto.IsEdit;
                grid.Splits[0].DisplayColumns[dto.Tag].Visible = !dto.IsHide;
                if (dto.IsEdit) grid.Splits[0].DisplayColumns[dto.Tag].Style.BackColor = Color.Azure;
                if (!string.IsNullOrEmpty(dto.NumberFormat)) grid.Columns[dto.Tag].NumberFormat = dto.NumberFormat;
                else
                {

                    if (dto.Property.PropertyType == typeof(decimal?) || dto.Property.PropertyType == typeof(decimal)
                         || dto.Property.PropertyType == typeof(double?) || dto.Property.PropertyType == typeof(double)
                         || dto.Property.PropertyType == typeof(float?) || dto.Property.PropertyType == typeof(float))
                    {
                        if (dto.NumberScale != null && dto.NumberScale.Value >= 0)
                        {
                            grid.Columns[dto.Tag].NumberFormat = dto.NumberPadRightZero ? "0.".PadRight(dto.NumberScale.Value + 2, '0') : "0.".PadRight(dto.NumberScale.Value + 2, '#');
                        }
                    }
                }
            }
            foreach(C1DataColumn column in grid.Columns)
            {
                //grid.Splits[0].DisplayColumns[column.Caption].HeadingStyle.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center;
                grid.Splits[0].DisplayColumns[column.Caption].Style.HorizontalAlignment = AlignHorzEnum.Near;

                if (column.DataType != typeof(bool))
                {
                    if (column.DataType == typeof(decimal)|| column.DataType == typeof(double)|| column.DataType == typeof(float)) grid.Splits[0].DisplayColumns[column.Caption].Style.HorizontalAlignment = AlignHorzEnum.Far;
                    continue;
                }
                grid.Splits[0].DisplayColumns[column.Caption].Style.HorizontalAlignment = AlignHorzEnum.Center;
                column.ValueItems.Presentation = C1.Win.C1TrueDBGrid.PresentationEnum.CheckBox;
                if (grid.Splits[0].DisplayColumns[column.Caption].Width < 50) grid.Splits[0].DisplayColumns[column.Caption].Width = 50;
            }

        }

        /// <summary>
        /// 刷新表格
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="tableCulomns"></param>
        private static void RefreshGrid(this C1.Win.C1TrueDBGrid.C1TrueDBGrid grid, List<TableColumnDto> tableCulomns)
        {
            var data = grid.DataSource as DataTable;
            for (int i = 0; i < tableCulomns.Count; i++)
            {
                var dto = tableCulomns[i];
                if (!(data?.Columns?.Contains(dto.Tag)??false)) continue;
              
                if (dto.IsAutoSize)
                {
                    grid.Splits[0].DisplayColumns[dto.Tag].AutoSize();
                }
                else { grid.Splits[0].DisplayColumns[dto.Tag].Width = dto.Width; }
               
            }
           
        }

        /// <summary>
        /// 获取列信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<TableColumnDto> GetColumnInfos<T>(string dbName, string moduleTag, List<TableColumnDto> columns = null) where T : class, new()
        {
            List<TableColumnDto> list = new List<TableColumnDto>();

            int group = 0;
            var attributes =  typeof(T).GetCustomAttributes<CustomTableAttribute>(false);
            group = attributes.FirstOrDefault(x =>x.ModuleTag== moduleTag&& x.TableName == dbName)?.GroupId ?? 0;
            var props = new List<PropertyInfo>(typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance));
            for (int i = 0; i < props.Count(); i++)
            {
                var prop = props[i];
                var ignore = prop.GetCustomAttributes(typeof(IgnoreCustomerAttribute), false);
                if (ignore.Length > 0 && (ignore.Select(x => x as IgnoreCustomerAttribute).FirstOrDefault(x => x.GroupId == group)?.IgnoreConvert ?? false)) continue;
                var dto = new TableColumnDto() { Tag = prop.Name, Sort = i, Property = prop };
                var objAttrs = prop.GetCustomAttributes<TableConvertAttribute>(true).ToList();
                var attr = objAttrs.FirstOrDefault(x => x.GroupId == group);

                if (attr != null)
                {
                    dto.Description = string.IsNullOrEmpty(attr.Description) ? prop.Name : attr.Description;
                    dto.IsHide = attr.IsHide;
                    dto.IsAutoSize = attr.IsAutoSize;
                    dto.MergeType = attr.MergeType;
                    dto.IsEdit = attr.IsEdit;
                    dto.Width = attr.Width;
                    dto.NumberFormat = attr.NumberFormat;
                    if (attr.SortValue != null) dto.Sort = attr.SortValue.Value;
                    dto.NumberScale = attr.NumberScaleValue;
                    dto.NumberPadRightZero = attr.NumberPadRightZero;

                    list.Add(dto);
                    continue;
                }

                dto.Description = prop.Name;
                dto.IsHide = true;
                dto.IsAutoSize = true;
                dto.MergeType = 0;
                list.Add(dto);
            }
            columns?.ForEach(x =>
            {
                var k = list.FirstOrDefault(z => z.Tag == x.Tag);
                if (k != null)
                {
                    k.Sort = x.Sort;
                    k.IsHide = x.IsHide;
                    k.Description = x.Description;
                    k.IsAutoSize = x.IsAutoSize;
                    k.IsEdit = x.IsEdit;
                    k.Width = x.Width;
                    k.NumberScale = x.NumberScale;
                    k.NumberPadRightZero = x.NumberPadRightZero;
                }
            });
            list.ForEach(k =>
            {
                if (string.IsNullOrEmpty(k.NumberFormat))
                {
                    if (k.Property.PropertyType == typeof(decimal?) || k.Property.PropertyType == typeof(decimal)
                     || k.Property.PropertyType == typeof(double?) || k.Property.PropertyType == typeof(double)
                     || k.Property.PropertyType == typeof(float?) || k.Property.PropertyType == typeof(float))
                    {
                        if (k.NumberScale != null && k.NumberScale.Value >= 0)
                        {
                            k.NumberFormat = k.NumberPadRightZero ? "0.".PadRight(k.NumberScale.Value + 2, '0') : "0.".PadRight(k.NumberScale.Value + 2, '#');
                        }
                    }
                }
            });
            list = list.OrderBy(x => x.Sort).ToList();
            return list;
        }

        /// <summary>
        /// 初始化表格
        /// </summary>
        /// <param name="grid">表格</param>
        /// <param name="IsLike">模糊查询</param>
        /// <param name="filter">是否筛选</param>
        /// <param name="rowHeight"></param>
        /// <param name="fontSize"></param>
        private static void InitGrid(C1TrueDBGrid grid, bool IsLike = true, bool filter = true, int rowHeight = 22, int fontSize = 10)
        {
            grid.HeadingStyle.BackColor = Color.FromArgb(224, 224, 224, 224);
            grid.BackColor = Color.White;
            grid.Style.VerticalAlignment = AlignVertEnum.Center;//内容垂直居中
            grid.Style.Font = new Font(new FontFamily("宋体"), fontSize);
            grid.FilterBar = filter;                            //是否显示过滤行
            grid.AlternatingRows = true;                        //交替行
            grid.OddRowStyle.BackColor = Color.WhiteSmoke;
            grid.EvenRowStyle.BackColor = Color.LightGray;
            grid.MarqueeStyle = C1.Win.C1TrueDBGrid.MarqueeEnum.HighlightRowRaiseCell;
            grid.RowHeight = rowHeight;
            if (IsLike)
            {
                grid.FilterChange -= (sender, e) => { HelperExtensionMethod.FilterChange(grid); };
                grid.FilterChange += (sender, e) => { HelperExtensionMethod.FilterChange(grid); };
            }
        }


        /// <summary>
        /// DataTable 转数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        private static List<T> ConvertToList<T>(DataTable dt) where T :class,new()
        {
            // 定义集合   
            List<T> ts = new List<T>();
            // 获得此模型的类型   
            Type type = typeof(T);
            //定义一个临时变量   
            string tempName = string.Empty;
            //遍历DataTable中所有的数据行   
            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                // 获得此模型的公共属性   
                PropertyInfo[] propertys = t.GetType().GetProperties();
                //遍历该对象的所有属性   
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;//将属性名称赋值给临时变量   
                    //检查DataTable是否包含此列（列名==对象的属性名）     
                    if (dt.Columns.Contains(tempName))
                    {
                        // 判断此属性是否有Setter   
                        if (!pi.CanWrite) continue;//该属性不可写，直接跳出   
                        //取值   
                        object value = dr[tempName];
                        //如果非空，则赋给对象的属性   
                        if (value != DBNull.Value)
                        {
                            var tt = value.GetType();
                            pi.SetValue(t, value, null);
                        }
                    }
                }
                //对象添加到泛型集合中   
                ts.Add(t);
            }
            return ts;
        }

        /// <summary>
        /// 数组转datatable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static DataTable ConvertToDataTable<T>(List<TableColumnDto> props,  List<T> collection)
        {
            var dt = new DataTable();

            var dataColumns = props.Select(p => new DataColumn(p.Property.Name,
                  ((p.Property.PropertyType.IsGenericType && p.Property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) ? p.Property.PropertyType.GetGenericArguments()[0] : p.Property.PropertyType)
                  )).ToArray();
            dt.Columns.AddRange(dataColumns);
            foreach (var i in collection)
            {
                var row = dt.NewRow();
                ArrayList tempList = new ArrayList();
                foreach (var pi in props)
                {
                    object obj = pi.Property.GetValue(i, null) == null ? DBNull.Value : pi.Property.GetValue(i, null); //pi.GetValue(collection.ElementAt(i), null);
                    row[pi.Property.Name] = obj;
                }
                dt.Rows.Add(row);
            }
            return dt;
        }


        private static void CreateNumber(C1.Win.C1TrueDBGrid.C1TrueDBGrid grid)
        {
            if (grid.Splits[0].DisplayColumns.Count > 0)
            {
                grid.Splits[0].DisplayColumns[0].OwnerDraw = true;
                grid.RecordSelectors = false;
                C1.Win.C1TrueDBGrid.C1DataColumn Col = new C1.Win.C1TrueDBGrid.C1DataColumn();
                C1.Win.C1TrueDBGrid.C1DisplayColumn dc;
                grid.Columns.Insert(0, Col);
                Col.Caption = string.Empty;
                dc = grid.Splits[0].DisplayColumns[string.Empty];
                grid.Splits[0].DisplayColumns.RemoveAt(grid.Splits[0].DisplayColumns.IndexOf(dc));
                grid.Splits[0].DisplayColumns.Insert(0, dc);
                dc.Visible = true;
                dc.Width = 30;
                dc.Style.BackColor = grid.RecordSelectorStyle.BackColor;
                dc.Locked = true;
                dc.AllowFocus = false;
                grid.Rebind(true);
                //设置grid行号
                grid.UnboundColumnFetch += (ss, ee) =>
                {
                    var e = (C1.Win.C1TrueDBGrid.UnboundColumnFetchEventArgs)ee;
                    e.Value = e.Row + 1 + "";
                };
                //设置右击定位到行
                grid.MouseDown += (ss, ee) =>
                {
                    if (ee.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        grid.SetActiveCell(grid.RowContaining(ee.Y), 2);
                    }
                };
            }
        }
    }
}
