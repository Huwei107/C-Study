using C1.C1Excel;
//-----------------------------------------------------------------------
// <copyright company="工品一号" file="ExtensionMethodHelper.cs">
//  Copyright (c)  V1.0.0.0  
//  创建作者:   刘少林
//  创建时间:   2017-2-13 16:57:03
//  功能描述:   扩展方法类
//  历史版本:
//          2017-2-13 16:57:03 刘少林 创建ExtensionMethodHelper类
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Drawing;
using System.Management;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows.Forms;
using FX.ORM.Websharp.ORM.Base;
using System.Collections.Generic;
using C1.Win.C1TrueDBGrid;
using System.Data;
using System.Text;
using DevExpress.XtraGrid.Views.Grid;
using System.Xml;
using System.IO;
using ReportUtil.Attibutes;
using System.Linq;
using System.Reflection;

namespace FX.MainForms
{
    /// <summary>
    /// 扩展方法类
    /// </summary>
    public static class HelperExtensionMethod
    {
        /// <summary>
        /// 扩展字符串转换整型方法
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>转换后的整型值</returns>
        public static int ToInt(this string str, int defaultValue = 0)
        {
            return HelperConvert.StrToInt(str, defaultValue);
        }


        public static decimal ToDecimal(this string str, decimal defaultValue = 0)
        {
            return HelperConvert.StrToDecimal(str, defaultValue);
        }

        public static double ToDouble(this string str, double defaultValue = 0)
        {
            return HelperConvert.StrToDouble(str, defaultValue);
        }

        public static float ToFloat(this string str, float defaultValue = 0)
        {
            return (float)HelperConvert.StrToDouble(str, (double)defaultValue);
        }

        public static long ToLong(this string str, long defaultValue = 0)
        {
            return HelperConvert.StrToLong(str, defaultValue);
        }
        public static decimal ToKeepNum(this string str, int keep)
        {
            if (keep == 0)
            {
                return Convert.ToDecimal(str.Split('.')[0]);
            }
            else
            {
                return Convert.ToDecimal(str.Split('.')[0] + "." + str.Split('.')[1].Substring(0, keep));
            }
        }

        /// <summary>
        /// 设置GridView指定列可见
        /// </summary>
        /// <param name="rootForm">根窗体</param>
        /// <param name="isShow">可见布尔值,true:可见，false:不可见</param>
        /// <param name="columns">列集合</param>
        public static void SetColumnsVisual(this GridView gridView, bool isShow, params string[] columns)
        {
            if (columns != null)
            {
                foreach (string column in columns)
                {
                    gridView.Columns[column].Visible = isShow;
                }
            }
        }

        /// <summary>
        /// 设置窗体指定按钮可见
        /// </summary>
        /// <param name="rootForm">根窗体</param>
        /// <param name="isShow">可见布尔值,true:可见，false:不可见</param>
        /// <param name="buttons">按钮集合</param>
        /// <remarks>权限不允许按钮可见，则按钮无法可见，无论是否设置可见</remarks>
        public static void SetVisualButton(this frmBase rootForm, bool isShow, params ButtonEnumList[] buttons)
        {
            HelperControls.SetVisualButton(rootForm, isShow, buttons);
        }

        /// <summary>
        /// 初始化弹出窗体监听配置
        /// </summary>
        /// <typeparam name="T">数据对象控件类型</typeparam>
        /// <param name="rootForm">容器窗体</param>
        /// <param name="control">绑定控件对象</param>
        /// <param name="handler">回调容器触发委托</param>
        /// <param name="columns">绑定数据列表容器列配置</param>
        /// <param name="showX">展示内容的X轴</param>
        /// <param name="showY">展示内容的Y轴</param>
        public static void InitFormPopupListener<T>(this frmBase rootForm,
            Control control, BasicDelegateHandler handler, int showX = 0, int showY = 0, params string[] columns)
             where T : UserControl, ICustomQuickSearchControl, new()
        {
            var extend = new MultiControlSearchObjectFilterExtend();
            extend.InitControlopupListener<T>(rootForm, control, handler, showX, showY, false, columns);
        }

        /// <summary>
        /// 初始化弹出窗体监听配置
        /// </summary>
        /// <typeparam name="T">数据对象控件类型</typeparam>
        /// <param name="rootForm">容器窗体</param>
        /// <param name="control">绑定控件对象</param>
        /// <param name="handler">回调容器触发委托</param>
        /// <param name="columns">绑定数据列表容器列配置</param>
        /// <param name="showX">展示内容的X轴</param>
        /// <param name="showY">展示内容的Y轴</param>
        public static void InitFormPopupListener<T>(this frmBase rootForm,
            Control control, BasicDelegateHandler handler,bool above, int showX = 0, int showY = 0, params string[] columns)
             where T : UserControl, ICustomQuickSearchControl, new()
        {
            var extend = new MultiControlSearchObjectFilterExtend();
            extend.InitControlopupListener<T>(rootForm, control, handler, showX, showY, above, columns);
        }

        /// <summary>
        /// 初始化弹出窗体监听配置
        /// </summary>
        /// <typeparam name="T">数据对象控件类型</typeparam>
        /// <param name="rootForm">容器窗体</param>
        /// <param name="control">绑定控件对象</param>
        /// <param name="handler">回调容器触发委托</param>
        /// <param name="columns">绑定数据列表容器列配置</param>
        /// <param name="showX">展示内容的X轴</param>
        /// <param name="showY">展示内容的Y轴</param>
        /// <param name="param">控件构造参数</param>
        public static void InitFormPopupListenerWithParam<T>(this frmBase rootForm,
            Control control, BasicDelegateHandler handler, int showX = 0, int showY = 0, string[] columns = null ,params object[] param)
             where T : UserControl, ICustomQuickSearchControl, new()
        {
            var extend = new MultiControlSearchObjectFilterExtend();
            extend.InitControlopupListener<T>(rootForm, control, handler, showX, showY, false, columns, param);
        }

        /// <summary>
        /// 初始化弹出窗体监听配置
        /// </summary>
        /// <typeparam name="T">数据对象控件类型</typeparam>
        /// <param name="rootForm">容器窗体</param>
        /// <param name="control">绑定控件对象</param>
        /// <param name="handler">回调容器触发委托</param>
        /// <param name="columns">绑定数据列表容器列配置</param>
        /// <param name="showX">展示内容的X轴</param>
        /// <param name="showY">展示内容的Y轴</param>
        /// <param name="param">控件构造参数</param>
        public static void InitFormPopupListenerWithParam<T>(this frmBase rootForm,
            Control control, BasicDelegateHandler handler, bool above,bool cover, int showX = 0, int showY = 0, string[] columns = null, params object[] param)
             where T : UserControl, ICustomQuickSearchControl, new()
        {
            var extend = new MultiControlSearchObjectFilterExtend();
            extend.InitControlopupListener<T>(rootForm, control, handler, showX, showY, above, cover, columns, param);
        }

        /// <summary>
        /// 根据单列值获取单个实体(是否保证唯一，需要开发人员自行判断，主要用于相对(同一SystemId)唯一键来获取实体)
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="columnName">列名称</param>
        /// <param name="columnValue">列值</param>
        /// <returns>实体</returns>
        /// <remarks>是否保证唯一，需要开发人员自行判断，主要用于相对(同一SystemId)唯一键来获取实体</remarks>
        public static PersistenceCapable NewEntity(this PersistenceCapable entity, string columnName, object columnValue)
        {
            IDictionary<string, object> dict = new Dictionary<string, object>(1);
            dict.Add(columnName, columnValue);
            return SQL.GetEntityByDictionary(entity.GetType(), dict);
        }

        /// <summary>
        /// 按钮事件行为判断
        /// </summary>
        /// <param name="rootForm">顶级窗体</param>
        /// <param name="entityStatus">单据状态</param>
        /// <param name="status">操作的状态</param>
        /// <param name="callback">回到方法检测是否有子集业务方法</param>
        /// <returns>true?有下级业务:不可操作</returns>
        public static bool ButtonPermissions(this frmBase rootForm, string entityStatus, AuditStatusTypes status)
        {
            return HelperControls.ButtonPermissions(entityStatus, status);
        }

        /// <summary>
        /// 设置窗体指定按钮禁用
        /// </summary>
        /// <param name="rootForm">根窗体</param>
        /// <param name="buttons">按钮集合,不传递则设置所有按钮禁用</param>
        /// <remarks>权限不允许按钮可用，则按钮无法可用，无论是否设置可用</remarks>
        public static void SetEnableButton(this frmBase rootForm, bool isEnable, params ButtonEnumList[] buttons)
        {
            HelperControls.SetEnableButton(rootForm, isEnable, buttons);
        }

        public static bool ToBoolean(this string str)
        {
            bool result = false;
            if (bool.TryParse(str, out result))
            {
                return result;
            }
            return result;
        }

        /// <summary>
        /// 扩展字符串解密方法
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>解密后的字符串</returns>
        /// <remarks>默认的秘钥不能随意更改，很多地方配置已使用这个默认秘钥</remarks>
        public static string DecryptDES(this string str)
        {
            return HelperSecurity.DecryptDES(str, "0x120300");
        }

        /// <summary>
        /// 扩展字符串加密方法
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>加密后的字符串</returns>
        /// <remarks>默认的秘钥不能随意更改，很多地方配置已使用这个默认秘钥</remarks>
        public static string EncryptDES(this string str)
        {
            return HelperSecurity.EncryptDES(str, "0x120300");
        }


        /// <summary>
        /// 扩展字符串转换日期方法
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>转换后的日期值</returns>
        public static DateTime ToDate(this string str)
        {
            return HelperConvert.StrToDate(str);
        }

        /// <summary>
        /// 设置不可见列
        /// </summary>
        /// <param name="grid">C1TrueDBGrid控件对象</param>
        /// <param name="caption">列名称列表</param>
        /// <remarks>caption必须是|链接而成的字符串 例如 A|B|C</remarks>
        public static void SetUnVisible(this C1.Win.C1TrueDBGrid.C1TrueDBGrid grid, string caption)
        {
            string captions = GetColumnNameString(grid);
            string[] cap = caption.Trim().Split('|');
            ushort len = (ushort)cap.Length;
            for (ushort i = 0; i < len; i++)
            {
                if (captions.IndexOf("," + cap[i].Trim() + ",") >= 0)
                {
                    try
                    {
                        grid.Splits[0].DisplayColumns[cap[i].Trim()].Visible = false;
                    }
                    catch (Exception ex)
                    {
                        HelperLog.Write(ex);
                        HelperMessageBoxContent.ShowMessageOK(cap[i].Trim() + "不存在，列配置错误，请勿继续操作,(请重置列后还不行)联系程序员手工调整 IN SetUnVisible", 0);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 获取C1TrueDBGrid列字符串集合字符串
        /// </summary>
        /// <param name="grid">C1TrueDBGrid</param>
        /// <returns></returns>
        private static string GetColumnNameString(C1.Win.C1TrueDBGrid.C1TrueDBGrid grid)
        {
            StringBuilder builder = new StringBuilder(64);
            if (grid != null && grid.Columns.Count > 0)
            {
                builder.Append(",");
                short len = (short)grid.Columns.Count;
                for (short index = 0; index < len; index++)
                {
                    //经过测试当两部分相同的时候，是没必要都放入到builder里面的
                    //引起性能降低 by arison 20221021
                    if (grid.Columns[index].Caption == grid.Columns[index].DataField)
                    {
                        builder.Append(grid.Columns[index].Caption).Append(",");
                    }
                    else
                    {
                        builder.Append(grid.Columns[index].Caption).Append(",");
                        builder.Append(grid.Columns[index].DataField).Append(",");
                    }

                }
            }
            return builder.ToString();
        }

        /// <summary>
        /// 设置列为几位小数
        /// </summary>
        /// <param name="grid">C1TrueDBGrid控件对象</param>
        /// <param name="caption">列名称列表</param>
        /// <param name="severaldecimal">显示几位小数,severaldecimal>0的正整数</param>
        /// <param name="severaldecimal">显示几位小数,severaldecimal>0的正整数</param>
        /// <remarks>caption必须是|链接而成的字符串 例如 A|B|C</remarks> 
        public static void SetAccurate(this C1.Win.C1TrueDBGrid.C1TrueDBGrid grid, string caption, int severalDecimal, string mode = "#")
        {
            if (string.IsNullOrEmpty(mode))
            {
                mode = "#";
            }
            if (mode != "#" && mode != "0")
            {
                mode = "#";
            }
            //保留3位小数的时候(用1.000这种形式表示)
            if (severalDecimal == 3)
            {
                mode = "0";
            }
            if (caption.Trim() != string.Empty)
            {
                string formatText = "0.";
                for (int i = 0; i < severalDecimal; i++)
                {
                    formatText += mode;
                }

                string[] cap = caption.Trim().Split('|');
                var len = cap.Length;
                string captions = GetColumnNameString(grid);
                for (var i = 0; i < len; i++)
                {
                    if (captions.IndexOf("," + cap[i].Trim() + ",") >= 0)
                    {
                        try
                        {
                            grid.Columns[cap[i]].NumberFormat = formatText;
                        }
                        catch (Exception ex)
                        {
                            HelperLog.Write(ex);
                            HelperMessageBoxContent.ShowMessageOK(cap[i].Trim() + "不存在，列配置错误，请勿继续操作,(请重置列后还不行)联系程序员手工调整 IN SetAccurate", 0);
                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 点击单元格的时候，显示这列单元格精确之前的小数位数
        /// </summary>
        /// <param name="grid">C1TrueDBGrid</param>
        /// <param name="caption">列名</param>
        /// <param name="beforeAccurate">点击其他列时当前列显示的小数位数,例如：2位小数</param>
        /// <param name="afterAccurate">点击之后当前列显示的小数位数，例如：10位小数</param>
        public static void ClickDisplay(this C1.Win.C1TrueDBGrid.C1TrueDBGrid grid, string caption, int beforeAccurate, int afterAccurate)
        {
            string formatBeforeText = "0.";
            for (int i = 0; i < beforeAccurate; i++)
            {
                formatBeforeText += "#";
            }
            string formatAfterText = "0.";
            for (int i = 0; i < afterAccurate; i++)
            {
                formatAfterText += "#";
            }
            //当前选择列
            var columnIndex = grid.Col;
            string[] cap = caption.Trim().Split('|');
            var len = cap.Length;
            var nowColumnName = grid.Columns[columnIndex].ToString();
            string captions = GetColumnNameString(grid);
            for (var i = 0; i < len; i++)
            {
                if (captions.IndexOf("," + cap[i].Trim() + ",") >= 0)
                {
                    try
                    {
                        if (nowColumnName == grid.Columns[cap[i]].ToString())
                        {
                            grid.Columns[cap[i]].NumberFormat = formatAfterText;
                        }
                        else
                        {
                            grid.Columns[cap[i]].NumberFormat = formatBeforeText;
                        }
                    }
                    catch (Exception ex)
                    {
                        HelperLog.Write(ex);
                        HelperMessageBoxContent.ShowMessageOK(cap[i].Trim() + "不存在，列配置错误，请勿继续操作,(请重置列后还不行)联系程序员手工调整 IN ClickDisplay", 0);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 点击单元格的时候，显示这列单元格精确之前的小数位数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="grid">表格</param>
        /// <param name="caption"></param>
        /// <param name="afterAccurate"></param>
        /// <param name="groupId"></param>
        public static void ClickDisplay<T>(this C1.Win.C1TrueDBGrid.C1TrueDBGrid grid, string caption, int afterAccurate, bool padzero = false,int groupId=0) where T : class, new()
        {
            var arrs = typeof(T).GetCustomAttributes<CustomTableAttribute>(false).ToArray();
            if (arrs == null || !arrs.Any()) return;
            var arr = arrs.FirstOrDefault(x => x.GroupId == groupId);
            if (arr == null) return;

            List<TableColumnDto> columns = new List<TableColumnDto>();
            if (RuntimeContext.Instance.Columns.ContainsKey($"{arr.ModuleTag}.{arr.TableName}.{arr.Type}.{typeof(T).FullName}"))
            {
                columns = RuntimeContext.Instance.Columns[$"{arr.ModuleTag}.{arr.TableName}.{arr.Type}.{typeof(T).FullName}"];
            }
            columns = HelperC1TrueDBGridTableExtension.GetColumnInfos<T>(arr.TableName, arr.ModuleTag, columns);

            var dic = columns.ToDictionary(x => x.Tag, x => x);
            string formatAfterText = padzero ? "0.".PadRight(afterAccurate + 2, '0').Trim('.') : "0.".PadRight(afterAccurate + 2, '#').Trim('.');
            //string formatAfterText = "0.";
            //for (int i = 0; i < afterAccurate; i++)
            //{
            //    formatAfterText += "#";
            //}
            //当前选择列
            var columnIndex = grid.Col;
            string[] cap = caption.Trim().Split('|');
            var len = cap.Length;
            var nowFields = grid.Columns[columnIndex].DataField;

           
            foreach (var item in cap)
            {
                try
                {
                    if (!dic.ContainsKey(item)) continue;
                    if (nowFields == item)
                    {
                        grid.Columns[item].NumberFormat = formatAfterText;
                    }
                    else
                    {
                        grid.Columns[item].NumberFormat = dic[item].NumberFormat;
                    }
                }
                catch (Exception ex)
                {
                    HelperLog.Write(ex);
                    HelperMessageBoxContent.ShowMessageOK(item + "不存在，列配置错误，请勿继续操作,(请重置列后还不行)联系程序员手工调整 IN ClickDisplay", 0);
                    return;
                }
                
            }
        }

        /// <summary>
        /// 设置列可编辑
        /// </summary>
        /// <param name="grid">C1TrueDBGrid控件对象</param>
        /// <param name="caption">列名称列表</param>
        /// <remarks>caption必须是|链接而成的字符串 例如 A|B|C</remarks> 
        public static void SetUnLock(this C1.Win.C1TrueDBGrid.C1TrueDBGrid grid, string caption, bool locked = false)
        {
            string captions = GetColumnNameString(grid);
            if (caption.Trim() != string.Empty)
            {
                string[] cap = caption.Trim().Split('|');
                var len = cap.Length;
                for (var i = 0; i < len; i++)
                {
                    if (captions.IndexOf("," + cap[i].Trim() + ",") >= 0)
                    {
                        try
                        {
                            //var tempStr = grid.Splits[0].DisplayColumns[cap[row]].ToString();
                            grid.Splits[0].DisplayColumns[cap[i]].Locked = locked;
                            grid.Splits[0].DisplayColumns[cap[i]].Style.BackColor = Color.Azure;
                        }
                        catch (Exception ex)
                        {
                            HelperLog.Write(ex);
                            HelperMessageBoxContent.ShowMessageOK(cap[i].Trim() + "不存在，列配置错误，请勿继续操作,(请重置列后还不行)联系程序员手工调整 IN SetUnLock", 0);
                            return;
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 设置不可编辑
        /// </summary>
        /// <param name="grid">C1TrueDBGrid控件对象</param>
        /// <param name="caption">列名称列表</param>
        /// <remarks>caption必须是|链接而成的字符串 例如 A|B|C</remarks> 
        public static void SetLock(this C1.Win.C1TrueDBGrid.C1TrueDBGrid grid, string caption, bool locked = true)
        {
            if (caption.Trim() != string.Empty)
            {
                string captions = GetColumnNameString(grid);
                string[] cap = caption.Trim().Split('|');
                var len = cap.Length;
                for (var i = 0; i < len; i++)
                {
                    if (captions.IndexOf("," + cap[i].Trim() + ",") >= 0)
                    {
                        try
                        {
                            grid.Splits[0].DisplayColumns[cap[i]].Locked = locked;
                        }
                        catch (Exception ex)
                        {
                            HelperLog.Write(ex);
                            HelperMessageBoxContent.ShowMessageOK(cap[i].Trim() + "不存在，列配置错误，请勿继续操作,(请重置列后还不行)联系程序员手工调整 IN SetLock", 0);
                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 设置所有不可编辑
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="locked"></param>
        public static void SetAllLock(this C1.Win.C1TrueDBGrid.C1TrueDBGrid grid, bool locked = true)
        {
            foreach (C1DisplayColumn item in grid.Splits[0].DisplayColumns)
            {
                item.Locked = locked;
            }
        }

        /// <summary>
        /// 设置指定列自动列宽
        /// </summary>
        /// <param name="grid">C1TrueDBGrid控件对象</param>
        /// <param name="caption">列名称列表</param>
        /// <remarks>设置列自动列宽</remarks>
        public static void SetColAutoSize(this C1.Win.C1TrueDBGrid.C1TrueDBGrid grid, string caption)
        {
            if (caption.Trim() != string.Empty)
            {
                string captions = GetColumnNameString(grid);
                string[] cap = caption.Trim().Split('|');
                ushort len = (ushort)cap.Length;
                for (ushort i = 0; i < len; i++)
                {
                    if (captions.IndexOf("," + cap[i].Trim() + ",") >= 0)
                    {
                        try
                        {
                            grid.Splits[0].DisplayColumns[cap[i]].AutoSize();
                        }
                        catch (Exception ex)
                        {
                            HelperLog.Write(ex);
                            HelperMessageBoxContent.ShowMessageOK(cap[i].Trim() + "不存在，列配置错误，请勿继续操作,(请重置列后还不行)联系程序员手工调整 IN SetColAutoSize", 0);
                            return;
                        }
                    }
                }
            }
        }

        ///<summary>
        /// 截前后字符串
        ///</summary>
        ///<param name="val">原字符串</param>
        ///<param name="str">要截掉的字符串</param>
        ///<param name="bAllStr">是否对整个字符串进行截取
        ///如果为true则对整个字符串中匹配的进行截取
        ///如果为false则只截取前缀或者后缀</param>
        ///<returns></returns>
        public static string GetString(string val, string str, bool bAllStr)
        {
            return Regex.Replace(val, @"(^(" + str + ")" + (bAllStr ? "*" : "") + "|(" + str + ")" + (bAllStr ? "*" : "") + "$)", "");
        }

        /// <summary>
        /// 截前后字符
        /// </summary>
        /// <param name="val">原字符串</param>
        /// <param name="c">要截取的字符</param>
        /// <returns></returns>
        public static string GetString(string val, char c)
        {
            return val.TrimStart(c).TrimEnd(c);
        }
        /// <summary>
        /// 设置列宽度
        /// </summary>
        /// <param name="grid">C1TrueDBGrid控件对象</param>
        /// <param name="caption">列名称列表</param>
        /// <remarks>设置类的宽度</remarks>
        public static void SetColWidth(this C1.Win.C1TrueDBGrid.C1TrueDBGrid grid, Dictionary<string, int> dic)
        {
            if (dic == null)
            {
                return;
            }
            if (dic.Count > 0)
            {
                foreach (KeyValuePair<string, int> kvp in dic)
                {
                    grid.Splits[0].DisplayColumns[kvp.Key].Width = kvp.Value;
                }
            }
        }

        /// <summary>
        /// C1TrueDBGrid初始化加载列头
        /// </summary>
        /// <param name="grid">C1TrueDBGrid</param>
        /// <param name="caption">所有列头</param>
        /// <param name="unVisible">隐藏列</param>
        public static void SetCaptionInit(this C1.Win.C1TrueDBGrid.C1TrueDBGrid grid, string caption, string unVisible = "")
        {
            grid.HeadingStyle.BackColor = Color.FromArgb(224, 224, 224, 224);
            grid.BackColor = Color.White;

            grid.Style.Font = new Font(new FontFamily("宋体"), 10);
            grid.FilterBar = true;
            //grid.ExtendRightColumn = true;

            string[] cap = caption.Split('|');
            var dt = new DataTable();
            for (int i = 0; i < cap.Length; i++)
            {
                dt.Columns.Add(cap[i].Trim());
            }
            //20190115 经过测试有的时候DataSource包含DataTable对象，如果中间有NULL列值，这个时候dt为只包含列头的空DataTable对象，赋值上去就会报错
            //因此将DataSource默认先置空(Null),因为此处赋值dt肯定会将之前的数据清空，因此添加此赋值，不影响代码执行流程! by arison!
            if (grid.DataSource != null)
            {
                grid.DataSource = null;
            }
            grid.DataSource = dt;
            if (!string.IsNullOrEmpty(unVisible))
            {
                string[] uv = unVisible.Split('|');
                string captions = GetColumnNameString(grid);
                for (var i = 0; i < uv.Length; i++)
                {
                    if (captions.IndexOf("," + uv[i].Trim() + ",") >= 0)
                    {
                        try
                        {
                            grid.Splits[0].DisplayColumns[uv[i].Trim()].Visible = false;

                        }
                        catch (Exception ex)
                        {
                            HelperLog.Write(ex);
                            HelperMessageBoxContent.ShowMessageOK(uv[i] + "不存在，列配置错误，请勿继续操作,(请重置列后还不行)联系程序员手工调整 IN SetCaptionInit", 0);
                            return;
                        }
                    }
                }
            }
            for (int i = 0; i < cap.Length; i++)
            {
                grid.Splits[0].DisplayColumns[i].Style.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Near;
                grid.Splits[0].DisplayColumns[i].HeadingStyle.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center;
                grid.Splits[0].DisplayColumns[i].AutoSize();
            }
        }

        public static int row; //c1TrueDBGrid设置行索引

        /// <summary>
        /// 设定C1TrueDBGrid的表头，自动 列宽
        /// </summary>
        /// <param name="grid">C1TrueDBGrid控件对象</param>
        /// <param name="caption">表头"a|b|c|d"</param>
        /// <param name="res">true?列自动伸展:默认列宽</param>
        /// <param name="IsLike">是否模糊搜索，数字列会失效、针对文本列</param>
        /// <param name="filter">默认显示过滤航</param>
        /// <param name="rowHeight">行高</param>
        /// <param name="fontSize">字体大小</param>
        public static void SetCaption(this C1.Win.C1TrueDBGrid.C1TrueDBGrid grid, string caption, bool res = true, bool IsLike = true, bool filter = true, int rowHeight = 22, int fontSize = 10)
        {
            try
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
                //grid.ExtendRightColumn = true;  //延长右列
                string[] cap = caption.Split('|');
                int n = cap.Length < grid.Splits[0].DisplayColumns.Count ? cap.Length : grid.Splits[0].DisplayColumns.Count;
                for (int i = 0; i < n; i++)
                {
                    grid.Columns[i].Caption = cap[i].Trim();
                    grid.Splits[0].DisplayColumns[i].Style.HorizontalAlignment = cap[i].Trim() == "选择" ? C1.Win.C1TrueDBGrid.AlignHorzEnum.Center : C1.Win.C1TrueDBGrid.AlignHorzEnum.Near;
                    if (grid.Columns[i].DataType == typeof(decimal))
                    {
                        grid.Splits[0].DisplayColumns[i].Style.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Far;
                    }
                    grid.Splits[0].DisplayColumns[i].HeadingStyle.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center;
                    grid.Splits[0].DisplayColumns[i].Locked = true;
                    if (res)
                    {
                        grid.Splits[0].DisplayColumns[i].AutoSize();
                        if (grid.Splits[0].DisplayColumns[i].Width < 70 && grid.Columns[i].Caption != string.Empty)
                        {
                            grid.Splits[0].DisplayColumns[i].Width = 70;
                        }
                    }
                }
                #region 设置列左侧索引(序号)
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
                        e.Value = e.Row == row ? "1" : (e.Row + 1).ToString();
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
            catch (Exception err)
            {
                HelperLog.Write(err);
            }
            #endregion

            #region 设置列模糊查询
            if (IsLike)
            {
                grid.FilterChange -= (sender, e) => { FilterChange(grid); };
                grid.FilterChange += (sender, e) => { FilterChange(grid); };
            }
            #endregion
        }


        /// <summary>
        /// 改变后的列名与字段名相对应
        /// </summary>
        /// <param name="grid">C1TrueDBGrid控件对象</param>
        /// <param name="columns">表头"a|b|c|d"</param>       
        /// <param name="dt">Datatable表</param>
        public static DataTable SetColumns(this C1.Win.C1TrueDBGrid.C1TrueDBGrid grid, string columns, DataTable dt)
        {
            if (dt.Rows.Count > 0 && dt != null)
            {
                string[] col = columns.Split('|');
                int n = col.Length;//< grid.Splits[0].DisplayColumns.Count ? col.Length : grid.Splits[0].DisplayColumns.Count
                string captions = GetColumnNameString(grid);
                for (int i = 0; i < n; i++)
                {
                    string str = col[i].Trim().ToString();
                    string[] strings = str.Split(',');
                    if (strings.Length > 1)
                    {
                        string pt1 = strings[0];
                        string pt2 = strings[1];
                        if (pt1.Length > 0)
                        {
                            if (captions.IndexOf("," + pt1 + ",") >= 0)
                            {
                                try
                                {
                                    int ord = dt.Columns[pt1].Ordinal;
                                    if (i != ord)
                                    {
                                        throw new Exception("列配置异常，请重置列!(如问题依旧存在请联系开发人员!)");
                                    }
                                    dt.Columns[pt1].SetOrdinal(i);
                                }
                                catch (Exception ex)
                                {
                                    HelperLog.Write(ex);
                                    HelperMessageBoxContent.ShowMessageOK(pt1 + "不存在，列配置错误，请勿继续操作,(请重置列后还不行)联系程序员手工调整 IN SetColumns", 0);
                                    return dt;
                                }
                            }
                            else if (captions.IndexOf("," + pt2 + ",") >= 0)
                            {
                                //pt2是指字段列对应的字段文字描述，而实际操作的就是Columns对应的名称 by arison 20220401
                                try
                                {
                                    dt.Columns[pt1].SetOrdinal(i);
                                }
                                catch (Exception ex)
                                {
                                    HelperLog.Write(ex);
                                    HelperMessageBoxContent.ShowMessageOK(pt1 + "不存在，列配置错误，请勿继续操作,(请重置列后还不行)联系程序员手工调整 IN SetColumns", 0);
                                    return dt;
                                }
                            }
                            else
                            {
                                try
                                {
                                    //系统测试在某些情况下，会爆出异常，如果本次修改
                                    //导致了不可的流程，至少确保之前的流程还是可以正常走
                                    //除非之前的流程本身存在问题 by arison 20220401
                                    dt.Columns[pt1].SetOrdinal(i);
                                }
                                catch (Exception ex)
                                {
                                    HelperLog.Write(ex);
                                    HelperMessageBoxContent.ShowMessageOK(pt1 + "不存在，列配置错误，请勿继续操作,(请重置列后还不行)联系程序员手工调整 IN SetColumns ", 0);
                                    return dt;
                                }
                            }
                        }
                    }
                }
            }

            return dt;
        }

        /// <summary>
        /// 获取所有列的数目
        /// </summary>
        /// <param name="grid">C1TrueDBGrid控件对象</param>
        /// <param name="caption">表头"a|b|c|d"</param>       
        public static int GetColumnsCount(this C1.Win.C1TrueDBGrid.C1TrueDBGrid grid, string caption)
        {
            int count = 0;
            try
            {
                string[] col = caption.Split('|');
                count = col.Length;
            }
            catch
            {

            }
            return count;
        }

        /// <summary>
        /// 根据列的数据重置列
        /// </summary>
        /// <param name="grid">C1TrueDBGrid控件对象</param>
        /// <param name="caption">表头"a|b|c|d"</param>       
        public static void ResetColumns(this C1.Win.C1TrueDBGrid.C1TrueDBGrid grid, string caption, string formKey)
        {
            try
            {
                int beforeCount = GetColumnsCount(grid, HelperConfig.GetConfigValue(@".\columnsList.xml", formKey + "_Sort"));
                int afterCount = GetColumnsCount(grid, caption);
                if (beforeCount != afterCount && beforeCount > 1)
                {
                    //保存显示的节点顺序
                    HelperConfig.SetConfigValue(@".\columnsList.xml", formKey + "_Show", string.Empty);
                    //保存隐藏节点
                    HelperConfig.SetConfigValue(@".\columnsList.xml", formKey + "_Hide", string.Empty);
                    //保存的列名顺序
                    HelperConfig.SetConfigValue(@".\columnsList.xml", formKey + "_Sort", string.Empty);
                }
            }
            catch (Exception ex)
            {
                HelperLog.Write(ex);
                //HelperMessageBoxContent.ShowMessageOK("初始化成功,请重新打开界面!");  //注释不用提示框 2019-8-24
                //这里由于合并代码冲突,导致列配置文件错误,出现这种情况时,程序里面是删除这个冲突的列配置xml,新增一个空的列配置,同时将之前冲突的备份下来.
                string path = System.Environment.CurrentDirectory + "\\columnsList.xml";
                //删除xml文件
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                //新建xml文件
                JudgmentFile(path);
            }
        }

        /// <summary>
        /// 判断是否存在新增列 ,假如存在的话,将新增的列放在列表的最后面
        /// </summary>
        /// <param name="grid">列表</param>
        /// <param name="dt">datatable</param>
        /// <param name="caption">所有列名</param>
        /// <param name="unVisuableCaption">不显示列名</param>
        /// <param name="formKey"></param>
        public static void ResetColumns(this C1.Win.C1TrueDBGrid.C1TrueDBGrid grid, DataTable dt, string caption, string unVisuableCaption, string formKey)
        {
            try
            {
                int beforeCount = GetColumnsCount(grid, HelperConfig.GetConfigValue(@".\columnsList.xml", formKey + "_Sort"));
                if (beforeCount > 1)
                {
                    //代码中的集合
                    string[] captionArray = caption.Trim().Split('|');
                    //配置文件中的集合
                    string[] showArray = HelperConfig.GetConfigValue(@".\columnsList.xml", formKey + "_Show").Split('|');
                    string[] sortArray = HelperConfig.GetConfigValue(@".\columnsList.xml", formKey + "_Sort").Split('|');
                    string[] hideArray = HelperConfig.GetConfigValue(@".\columnsList.xml", formKey + "_Hide").Split('|');

                    List<string> sortsLastArray = new List<string>(); //重组后的数组

                    //获取最新的列名的集合与原先配置文件列名的集合的差集(全部的列)
                    string[] intersectioncaptionArray = StringScreen(captionArray, showArray, false);
                    //出现差异列再进行调整
                    if (intersectioncaptionArray.Length > 0)
                    {
                        //与列表里caption列的交集
                        string[] showIntersect = StringScreen(captionArray, intersectioncaptionArray, true);
                        //与配置文件里show的交集
                        string[] showConfigIntersect = StringScreen(showArray, intersectioncaptionArray, true);

                        Dictionary<string, string> dicAdd = new Dictionary<string, string>(); //列增加的集合
                        Dictionary<string, string> dicRedure = new Dictionary<string, string>();//列减少的集合

                        //根据caption列名的索引获取dataTable 对应的字段名的集合 
                        //列增加的集合
                        for (int j = 0; j < showIntersect.Length; j++)
                        {
                            for (int i = 0; i < captionArray.Length; i++)
                            {
                                if (showIntersect[j] == captionArray[i])
                                {
                                    dicAdd.Add(dt.Columns[i].ColumnName, captionArray[i].ToString().Trim());
                                }
                            }
                        }
                        //列减少的集合
                        for (int j = 0; j < showConfigIntersect.Length; j++)
                        {
                            for (int i = 0; i < sortArray.Length; i++)
                            {
                                if (sortArray[i].Split(',')[1] == (showConfigIntersect[j]))
                                {
                                    dicRedure.Add(sortArray[i].Split(',')[0], sortArray[i].Split(',')[1].Trim());
                                }
                            }
                        }

                        //原先的排序数字集合
                        List<string> sortsArray = new List<string>(sortArray);
                        List<string> captionsArray = new List<string>(showArray);

                        string sort = string.Empty;
                        string show = string.Empty;
                        string hide = string.Empty;
                        //将需要添加的字段和中文名称集合拼装成新的sortArray集合中                 
                        foreach (KeyValuePair<string, string> kvp in dicAdd)
                        {
                            sortsArray.Add(kvp.Key + "," + kvp.Value);
                            captionsArray.Add(kvp.Value);
                        }
                        //将需要减少的字段和中文名称集合拼装成新的sortArray集合中                 
                        foreach (KeyValuePair<string, string> kvp in dicRedure)
                        {
                            sortsArray.Remove(kvp.Key + "," + kvp.Value);
                            captionsArray.Remove(kvp.Value);
                        }
                        //新的集合
                        sortArray = sortsArray.ToArray();
                        showArray = captionsArray.ToArray();

                        //重组为了 ,保证之前本地列配置文件错误的话 ,可以重新排列
                        for (int j = 0; j < sortArray.Length; j++)
                        {
                            for (int i = 0; i < captionArray.Length; i++)
                            {
                                if (sortArray[j].Split(',')[1] == captionArray[i])
                                {
                                    sortsLastArray.Add(dt.Columns[i].ColumnName + "," + captionArray[i]);
                                }
                            }
                        }
                        sortArray = sortsLastArray.ToArray();

                        //合并后的信息的数组 
                        for (int i = 0; i < sortArray.Length; i++)
                        {
                            sort += sortArray[i] + "|";  //.Trim()
                            show += showArray[i] + "|";  //.Trim()
                            for (int j = 0; j < hideArray.Length; j++)
                            {
                                if (hideArray[j] == showArray[i])
                                {
                                    hide += hideArray[j] + "|";  //.Trim()
                                }
                            }
                        }
                        sort = sort.TrimEnd('|');
                        show = show.TrimEnd('|');
                        hide = hide.TrimEnd('|');
                        //保存显示的节点顺序
                        HelperConfig.SetConfigValue(@".\columnsList.xml", formKey + "_Show", show);
                        //保存隐藏节点
                        HelperConfig.SetConfigValue(@".\columnsList.xml", formKey + "_Hide", hide);
                        //保存的列名顺序
                        HelperConfig.SetConfigValue(@".\columnsList.xml", formKey + "_Sort", sort);

                    }
                    //else
                    //{
                    //    string sort = string.Empty;
                    //    string show = string.Empty;
                    //    //重组为了 ,保证之前列配置文件错误
                    //    for (int show = 0; show < sortArray.Length; show++)
                    //    {
                    //        for (int row = 0; row < captionArray.Length; row++)
                    //        {
                    //            if (sortArray[show].Split(',')[1] == captionArray[row])
                    //            {
                    //                sortsLastArray.Add(dt.Columns[row].ColumnName + "," + captionArray[row].ToString());
                    //            }
                    //        }
                    //    }
                    //    sortArray = sortsLastArray.ToArray();
                    //    //合并后的信息的数组 
                    //    for (int row = 0; row < sortArray.Length; row++)
                    //    {
                    //        sort += sortArray[row] + "|";
                    //        show += showArray[row] + "|";
                    //    }
                    //    sort = sort.TrimEnd('|');
                    //    show = show.TrimEnd('|');
                    //    //保存显示的节点顺序
                    //    HelperConfig.SetConfigValue(@".\columnsList.xml", formKey + "_Show", show);
                    //    //保存隐藏节点
                    //    HelperConfig.SetConfigValue(@".\columnsList.xml", formKey + "_Hide", unVisuableCaption);
                    //    //保存的列名顺序
                    //    HelperConfig.SetConfigValue(@".\columnsList.xml", formKey + "_Sort", sort);

                    //}
                }
            }
            catch (Exception ex)
            {
                HelperLog.Write(ex.ToString());
            }
        }

        /// <summary>
        /// 两个string[]数组 取 交集 或 差集
        /// </summary>
        /// <param name="arr1"></param>
        /// <param name="arr2"></param>
        /// <param name="isIntersected">true取交集 false取差集</param>
        /// <returns></returns>
        public static string[] StringScreen(string[] arr1, string[] arr2, bool isIntersected)
        {

            int len = arr1.Length > arr2.Length ? arr1.Length : arr2.Length;
            int o = 0;

            string[] result = new string[len];
            Dictionary<string, Boolean> map = new Dictionary<string, Boolean>();

            foreach (string str1 in arr1)
            {
                if (!map.ContainsKey(str1))
                {
                    map[str1] = false;
                }
            }

            foreach (string str2 in arr2)
            {
                if (str2 != null && !str2.Equals(""))
                {
                    if (map.ContainsKey(str2))
                    {
                        map[str2] = true;
                    }
                    else
                    {
                        map[str2] = false;
                    }
                }
            }

            foreach (string entry in map.Keys)
            {
                if (map[entry] == isIntersected)
                {
                    result[o] = entry;
                    o++;
                }
            }


            string[] res = new string[o];
            for (int r = 0; r < o; r++)
            {
                res[r] = result[r];
            }

            return res;
        }

        /// <summary>
        /// 判断columnsList.xml是否存在，不存在则创建
        /// </summary>
        /// <param name="path"></param>
        public static void JudgmentFile(string path)
        {
            if (!File.Exists(path))
            {
                File.Create(path).Close();
                StreamWriter sw = new StreamWriter(path, false);
                sw.Write("<?xml version='1.0' encoding='utf-8'?>" + "\r\n" + "<appSettings>" + "\r\n" + "</appSettings>");
                sw.Close();
            }
        }

        /// <summary>
        /// 清空xml appSettings 节点的内容,为了合并代码时候节点冲突报异常
        /// </summary>
        public static void ClearXml()
        {
            bool bSuccess = true;
            while (bSuccess)
            {
                string strTaskListPath = @".\columnsList.xml";
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(strTaskListPath);
                XmlNodeList xnl = xmlDoc.SelectSingleNode("appSettings").ChildNodes;
                int nCount = xnl.Count;
                if (0 == nCount)
                {
                    bSuccess = false;
                }
                for (int i = 0; i < xnl.Count; i++)
                {
                    xnl[i].ParentNode.RemoveChild(xnl[i]);
                }
                xmlDoc.Save(strTaskListPath);
            }
        }
        /// <summary>
        /// 设定C1TrueDBGrid的表头，自动 列宽
        /// </summary>
        /// <param name="grid">C1TrueDBGrid控件对象</param>
        /// <param name="caption">表头"a|b|c|d"</param>
        /// <param name="res">true?列自动伸展:默认列宽</param>
        /// <param name="IsLike">是否模糊搜索，数字列会失效、针对文本列</param>
        /// <param name="filter">默认显示过滤航</param>
        /// <param name="rowHeight">行高</param>
        /// <param name="fontSize">字体大小</param>
        public static void SetCaptionNoLineNum(this C1.Win.C1TrueDBGrid.C1TrueDBGrid grid, string caption, bool res = true, bool IsLike = true, bool filter = true, int rowHeight = 22, int fontSize = 10)
        {
            try
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
                //grid.ExtendRightColumn = true;  //延长右列
                string[] cap = caption.Split('|');
                int n = cap.Length < grid.Splits[0].DisplayColumns.Count ? cap.Length : grid.Splits[0].DisplayColumns.Count;
                for (int i = 0; i < n; i++)
                {
                    grid.Columns[i].Caption = cap[i].Trim();
                    grid.Splits[0].DisplayColumns[i].Style.HorizontalAlignment = cap[i].Trim() == "选择" ? C1.Win.C1TrueDBGrid.AlignHorzEnum.Center : C1.Win.C1TrueDBGrid.AlignHorzEnum.Near;
                    if (grid.Columns[i].DataType == typeof(decimal))
                    {
                        grid.Splits[0].DisplayColumns[i].Style.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Far;
                    }
                    grid.Splits[0].DisplayColumns[i].HeadingStyle.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center;
                    grid.Splits[0].DisplayColumns[i].Locked = true;
                    if (res)
                    {
                        grid.Splits[0].DisplayColumns[i].AutoSize();
                        if (grid.Splits[0].DisplayColumns[i].Width < 70 && grid.Columns[i].Caption != string.Empty)
                        {
                            grid.Splits[0].DisplayColumns[i].Width = 70;
                        }
                    }
                }
                #region 设置列模糊查询
                //if (IsLike)
                //{
                //    grid.FilterChange -= (sender, e) => { FilterChange(grid); };
                //    grid.FilterChange += (sender, e) => { FilterChange(grid); };
                //}
                #endregion
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 列模糊查询
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        public static DataTable FilterChange(C1.Win.C1TrueDBGrid.C1TrueDBGrid grid)
        {
            DataTable dt = grid.DataSource as DataTable;
            try
            {
                StringBuilder sb = new StringBuilder();
                grid.AllowFilter = false;
                for (int i = 0; i < grid.Columns.Count; i++)
                {
                    if (grid.Columns[i].FilterText.Length > 0)
                    {
                        if (sb.Length > 0)
                        {
                            sb.Append(" AND ");
                        }
                        string filtertext = string.Empty;
                        if (grid.Columns[i].DataType == typeof(string))
                        {
                            filtertext = grid.Columns[i].FilterText.Contains("*") ?
                                grid.Columns[i].FilterText.Replace("*", "[*]") : grid.Columns[i].FilterText;
                            sb.Append(grid.Columns[i].DataField + " like " + "'*" + filtertext + "*'");
                        }
                        else if (grid.Columns[i].DataType == typeof(decimal) || grid.Columns[i].DataType == typeof(double) ||
                                grid.Columns[i].DataType == typeof(int) || grid.Columns[i].DataType == typeof(float))
                        {
                            filtertext = grid.Columns[i].FilterText;
                            if (HelperNumber.IsNumberFormat(filtertext))
                            {
                                sb.Append(grid.Columns[i].DataField + " = '" + filtertext + "'");
                            }
                            else
                            {
                                sb.Append("1=2");
                            }
                        }
                        else if (grid.Columns[i].DataType == typeof(DateTime))
                        {
                            filtertext = grid.Columns[i].FilterText;
                            if (HelperDateTime.IsTimeFormat(filtertext))
                            {
                                sb.Append(grid.Columns[i].DataField + " >= '" + DateTime.Parse(filtertext).Date.ToString() + "' AND " +
                                        grid.Columns[i].DataField + " <= '" + DateTime.Parse(filtertext).Date.AddDays(1).AddMilliseconds(-1).ToString() + "' ");
                            }
                            else
                            {
                                sb.Append("1=2");
                            }
                        }
                        else if (grid.Columns[i].DataType == typeof(bool))
                        {
                            filtertext = grid.Columns[i].FilterText;
                            if (bool.TryParse(filtertext,out bool result))
                            {
                                sb.Append(grid.Columns[i].DataField + " = '" + filtertext + "'");
                            }
                            else
                            {
                                sb.Append("1=2");
                            }
                        }
                    }
                }
                dt.DefaultView.RowFilter = sb.ToString();
            }
            catch (Exception err) { MessageBox.Show(err.Message); };
            return dt;
        }

        /// <summary>
        /// 定C1TrueDBGrid的自动列宽
        /// </summary>
        /// <param name="grid">C1TrueDBGrid控件对象</param>
        /// <param name="res">true?列自动伸展:默认列宽</param>
        public static void AutoSize(this C1.Win.C1TrueDBGrid.C1TrueDBGrid grid, bool res = true)
        {
            int n = grid.Splits[0].DisplayColumns.Count;
            for (int i = 0; i < n; i++)
            {
                grid.Splits[0].DisplayColumns[i].Style.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Near;
                if (grid.Columns[i].DataType == typeof(decimal))
                {
                    grid.Splits[0].DisplayColumns[i].Style.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Far;
                }
                grid.Splits[0].DisplayColumns[i].HeadingStyle.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center;
                if (res)
                    grid.Splits[0].DisplayColumns[i].AutoSize();
            }
        }

        /// <summary>
        /// 设定C1FlexGrid的表头，自动 列宽
        /// </summary>
        /// <param name="grid">C1TrueDBGrid控件对象</param>
        /// <param name="caption">表头"a|b|c|d"</param>
        /// <param name="res">true?列自动伸展:默认列宽</param>
        public static void SetCaption(this C1.Win.C1FlexGrid.C1FlexGrid grid, string caption, bool res = true)
        {
            string[] cap = caption.Split('|');
            System.Data.DataTable dt = grid.DataSource as System.Data.DataTable;
            int n = dt == null || cap.Length < dt.Columns.Count ? cap.Length : dt.Columns.Count;
            for (int i = 0; i < n; i++)
            {
                grid.Cols[i + 1].Caption = cap[i].Trim();
                if (dt != null && dt.Columns[i].DataType.Name == "DateTime")
                {
                    grid.Cols[i + 1].Format = "yyyy-MM-dd HH:mm:ss";
                }
            }
            if (res == true)
                grid.AutoSizeCols();
            grid.AutoResize = true;
        }

        /// <summary>
        /// 利用C1Excel将C1TrueDBGrid导出到Excel,根据可见列和Caption确定要导出的表头
        /// </summary>
        /// <param name="c1TrueDBGrid1">要导出的带数据的Grid</param>
        ///  /// <param name="c1TrueDBGrid1">文件名的前辍</param>
        public static string C1GridToExcel(this C1.Win.C1TrueDBGrid.C1TrueDBGrid c1TrueDBGrid1, string fileName)
        {
            try
            {
                C1.C1Excel.C1XLBook c1XLBook1 = new C1XLBook();
                XLSheet sheet = c1XLBook1.Sheets[0];
                //添加标题行
                int c = 0;
                for (int i = 0; i < c1TrueDBGrid1.Columns.Count; i++)
                {
                    if (c1TrueDBGrid1.Splits[0].DisplayColumns[i].Visible == true && c1TrueDBGrid1.Splits[0].DisplayColumns[i].ToString() != "选择")
                    {
                        if (i == 0 && c1TrueDBGrid1.Columns[i].Caption == string.Empty)
                        {
                            sheet[0, c].Value = "序号";
                        }
                        else
                        {
                            sheet[0, c].Value = c1TrueDBGrid1.Columns[i].Caption;
                        }

                        c++;
                    }
                }
                //内容
                for (int i = 0; i < c1TrueDBGrid1.Splits[0].Rows.Count; i++)
                {
                    int show = 0;
                    for (int j = 0; j < c1TrueDBGrid1.Columns.Count; j++)
                    {
                        if (c1TrueDBGrid1.Splits[0].DisplayColumns[j].Visible == true && c1TrueDBGrid1.Splits[0].DisplayColumns[j].ToString() != "选择")
                        {
                            if (c1TrueDBGrid1.Columns[j].DataType == typeof(DateTime))
                            {
                                if (c1TrueDBGrid1[i, j].ToString() != "")
                                { sheet[i + 1, show].Value = DateTime.Parse(c1TrueDBGrid1[i, j].ToString()).ToString("yyyy-MM-dd"); }
                            }
                            else if (c1TrueDBGrid1.Columns[j].DataType == typeof(Decimal))
                            {
                                if (c1TrueDBGrid1[i, j].ToString() != "")
                                { sheet[i + 1, show].Value = Decimal.Parse(c1TrueDBGrid1[i, j].ToString()); }
                            }
                            else
                            {
                                if (c1TrueDBGrid1[i, j].ToString() == "False")
                                {
                                    sheet[i + 1, show].Value = "否";
                                }
                                else if (c1TrueDBGrid1[i, j].ToString() == "True")
                                {
                                    sheet[i + 1, show].Value = "是";
                                }
                                else
                                {
                                    sheet[i + 1, show].Value = c1TrueDBGrid1[i, j];
                                }
                            }
                            show++;
                        }
                    }
                }
                SaveFileDialog dlg1 = new SaveFileDialog();//导出地址
                dlg1.DefaultExt = "xls";
                dlg1.FileName = fileName + DateTime.Now.ToString("yyyyMMddss") + ".xls";  //命名加了秒，表面出现重复导出
                dlg1.RestoreDirectory = true;
                if (dlg1.ShowDialog() == DialogResult.OK)
                {
                    string dialogPath = dlg1.FileName.Substring(0, dlg1.FileName.LastIndexOf("\\"));
                    c1XLBook1.Save(dlg1.FileName);
                    return dialogPath;
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                HelperLog.Write(ex);
                MessageBox.Show("文件已打开,请关闭后重试!");
                return string.Empty;
            }
        }

        /// <summary>
        /// 带合并单元格导出excel
        /// </summary>
        /// <param name="c1TrueDBGrid1">表格</param>
        /// <param name="fileName">文件名</param>
        /// <param name="key">合并起始列名</param>
        /// <returns></returns>
        public static string C1GridToExcelWithMerge(this C1.Win.C1TrueDBGrid.C1TrueDBGrid c1TrueDBGrid1, string fileName,string key)
        {
            try
            {
                C1.C1Excel.C1XLBook c1XLBook1 = new C1XLBook();
                XLSheet sheet = c1XLBook1.Sheets[0];
                //添加标题行
                int c = 0;
                for (int i = 0; i < c1TrueDBGrid1.Columns.Count; i++)
                {
                    if (c1TrueDBGrid1.Splits[0].DisplayColumns[i].Visible == true && c1TrueDBGrid1.Splits[0].DisplayColumns[i].ToString() != "选择")
                    {
                        if (i == 0 && c1TrueDBGrid1.Columns[i].Caption == string.Empty)
                        {
                            sheet[0, c].Value = "序号";
                        }
                        else
                        {
                            sheet[0, c].Value = c1TrueDBGrid1.Columns[i].Caption;
                        }

                        c++;
                    }
                }
                //内容
                for (int i = 0; i < c1TrueDBGrid1.Splits[0].Rows.Count; i++)
                {
                    if (i == c1TrueDBGrid1.Splits[0].Rows.Count - 1)
                    {
                        ExportOneRow(c1TrueDBGrid1, sheet, i);
                        break;
                    }
                    int row = i + 1;
                    bool marge = false;
                    while (c1TrueDBGrid1[i,key].ToString()== c1TrueDBGrid1[row, key].ToString())
                    {
                        marge = true;
                        row += 1;
                        if (row == c1TrueDBGrid1.Splits[0].Rows.Count) break;
                    }
                    if (!marge)
                    {
                        ExportOneRow(c1TrueDBGrid1, sheet, i);
                        continue;
                    }
                    else
                    {
                        ExportOneMergeRow(c1TrueDBGrid1, c1XLBook1, sheet, i, row);
                        i = row - 1;
                    }
                }
                SaveFileDialog dlg1 = new SaveFileDialog();//导出地址
                dlg1.DefaultExt = "xls";
                dlg1.FileName = fileName + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"; 
                dlg1.RestoreDirectory = true;
                if (dlg1.ShowDialog() == DialogResult.OK)
                {
                    string dialogPath = dlg1.FileName.Substring(0, dlg1.FileName.LastIndexOf("\\"));
                    c1XLBook1.Save(dlg1.FileName);
                    return dialogPath;
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                HelperLog.Write(ex);
                MessageBox.Show("文件已打开,请关闭后重试!");
                return string.Empty;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c1TrueDBGrid1"></param>
        /// <param name="sheet"></param>
        /// <param name="i"></param>
        private static void ExportOneRow(C1TrueDBGrid c1TrueDBGrid1,XLSheet sheet, int i)
        {
            int show = 0;
            for (int j = 0; j < c1TrueDBGrid1.Columns.Count; j++)
            {
                if (c1TrueDBGrid1.Splits[0].DisplayColumns[j].Visible == true && c1TrueDBGrid1.Splits[0].DisplayColumns[j].ToString() != "选择")
                {
                    DrawOneCell(c1TrueDBGrid1, sheet, i, j, show);
                    show++;
                }
            }
        }

        /// <summary>
        /// 导出一行合并列
        /// </summary>
        /// <param name="c1TrueDBGrid1">表格</param>
        /// <param name="book">excel</param>
        /// <param name="sheet">sheet页</param>
        /// <param name="initRow">起始行号</param>
        /// <param name="row">终止行号</param>
        private static void ExportOneMergeRow(C1TrueDBGrid c1TrueDBGrid1, C1XLBook book, XLSheet sheet, int initRow,int row)
        {
            int show = 0;
            bool merge = true;
            XLStyle _Colstyle = new C1.C1Excel.XLStyle(book);
            _Colstyle.AlignVert = XLAlignVertEnum.Center;
            _Colstyle.AlignHorz = XLAlignHorzEnum.Left;
            for (int i = 0; i < c1TrueDBGrid1.Columns.Count; i++)
            {
                if (c1TrueDBGrid1.Splits[0].DisplayColumns[i].Visible == true && c1TrueDBGrid1.Splits[0].DisplayColumns[i].ToString() != "选择")
                {
                    int start = initRow;
                    int count = 0;
                    for (int k = initRow; k <= row; k++)
                    {
                        if ((i == 0 && c1TrueDBGrid1.Columns[i].Caption == string.Empty) || !merge ) 
                        {
                            sheet[k + 1, show].Value = c1TrueDBGrid1[k, i];
                            sheet[k + 1, show].Style = _Colstyle;
                            continue;
                        }
                        if (c1TrueDBGrid1[k, i].ToString() == c1TrueDBGrid1[start, i].ToString() && k != row) 
                        {
                            continue;
                        }
                        else
                        {
                            XLCellRange _ColRange = new C1.C1Excel.XLCellRange(start + 1, k , show, show);
                            DrawOneMergeCell(c1TrueDBGrid1, sheet, start + 1, i, show, c1TrueDBGrid1[start, i]);
                            sheet.MergedCells.Add(_ColRange);
                            sheet[start + 1, show].Style = _Colstyle;
                            start = k;
                            count += 1;
                            if (count == row - initRow) 
                                merge = false;
                        }
                    }
                    show++;
                }
            }
        }

        /// <summary>
        /// 绘制一个合并单元格
        /// </summary>
        /// <param name="c1TrueDBGrid1">表格</param>
        /// <param name="sheet">sheet页</param>
        /// <param name="row">行号</param>
        /// <param name="col">列号</param>
        /// <param name="show">显示列号</param>
        /// <param name="value">值</param>
        private static void DrawOneMergeCell(C1TrueDBGrid c1TrueDBGrid1, XLSheet sheet, int row,int col, int show,object value)
        {
            if (c1TrueDBGrid1.Columns[col].DataType == typeof(DateTime))
            {
                if (value.ToString() != "")
                    sheet[row, show].Value = DateTime.Parse(value.ToString()).ToString("yyyy-MM-dd");
            }
            else if (c1TrueDBGrid1.Columns[col].DataType == typeof(Decimal))
            {
                if (value.ToString() != "")
                    sheet[row, show].Value = Decimal.Parse(value.ToString()); 
            }
            else
            {
                if (value.ToString().ToLower() == "false")
                {
                    sheet[row, show].Value = "否";
                }
                else if (value.ToString().ToLower() == "true")
                {
                    sheet[row,show].Value = "是";
                }
                else
                {
                    sheet[row, show].Value = value.ToString();
                }
            }
        }

        /// <summary>
        /// 绘制一个单元格
        /// </summary>
        /// <param name="c1TrueDBGrid1">表格</param>
        /// <param name="sheet">sheet页</param>
        /// <param name="row">行号</param>
        /// <param name="col">列号</param>
        /// <param name="show">显示列号</param>
        private static void DrawOneCell(C1TrueDBGrid c1TrueDBGrid1, XLSheet sheet, int row, int col, int show)
        {
            if (c1TrueDBGrid1.Columns[col].DataType == typeof(DateTime))
            {
                if (c1TrueDBGrid1[row, col].ToString() != "")
                { sheet[row + 1, show].Value = DateTime.Parse(c1TrueDBGrid1[row, col].ToString()).ToString("yyyy-MM-dd"); }
            }
            else if (c1TrueDBGrid1.Columns[col].DataType == typeof(Decimal))
            {
                if (c1TrueDBGrid1[row, col].ToString() != "")
                { sheet[row + 1, show].Value = Decimal.Parse(c1TrueDBGrid1[row, col].ToString()); }
            }
            else
            {
                if (c1TrueDBGrid1[row, col].ToString().ToLower() == "false")
                {
                    sheet[row + 1, show].Value = "否";
                }
                else if (c1TrueDBGrid1[row, col].ToString().ToLower() == "true")
                {
                    sheet[row + 1, show].Value = "是";
                }
                else
                {
                    sheet[row + 1, show].Value = c1TrueDBGrid1[row, col];
                }
            }
        }

        /// <summary>
        /// 导出表格
        /// </summary>
        /// <param name="table">表格</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public static string DateTableToExcel(this DataTable table, string fileName)
        {
            try
            {
                C1.C1Excel.C1XLBook c1XLBook1 = new C1XLBook();
                XLSheet sheet = c1XLBook1.Sheets[0];
                //添加标题行
                
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    sheet[0, i].Value = table.Columns[i].ColumnName;

                }
                //内容
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        if(table.Rows[i][j] is decimal|| table.Rows[i][j] is int)
                            sheet[i + 1, j].Value = table.Rows[i][j];
                        else 
                            sheet[i + 1, j].Value = table.Rows[i][j].ToString();
                    }
                }
                SaveFileDialog dlg1 = new SaveFileDialog();//导出地址
                dlg1.DefaultExt = "xls";
                dlg1.FileName = fileName + DateTime.Now.ToString("yyyyMMddss") + ".xls";  //命名加了秒，表面出现重复导出
                dlg1.RestoreDirectory = true;
                if (dlg1.ShowDialog() == DialogResult.OK)
                {
                    string dialogPath = dlg1.FileName.Substring(0, dlg1.FileName.LastIndexOf("\\"));
                    c1XLBook1.Save(dlg1.FileName);
                    return dialogPath;
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                HelperLog.Write(ex);
                MessageBox.Show("文件已打开,请关闭后重试!");
                return string.Empty;
            }
        }

        /// <summary>
        /// 利用DtToExcel将DtToExcel导出到Excel,根据可见列和Caption确定要导出的表头
        /// </summary>
        /// <param name="c1TrueDBGrid1">要导出的带数据的Grid</param>
        ///  /// <param name="c1TrueDBGrid1">文件名的前辍</param>
        public static string DtToExcel(this C1.Win.C1TrueDBGrid.C1TrueDBGrid c1TrueDBGrid1, string fileName, DataTable dt)
        {
            C1.C1Excel.C1XLBook c1XLBook1 = new C1XLBook();
            XLSheet sheet = c1XLBook1.Sheets[0];
            //添加标题行
            int c = 0;
            for (int i = 0; i < c1TrueDBGrid1.Columns.Count; i++)
            {
                if (c1TrueDBGrid1.Splits[0].DisplayColumns[i].Visible == true)
                {
                    if (i == 0 && c1TrueDBGrid1.Columns[i].Caption == string.Empty)
                    {
                        sheet[0, c].Value = "序号";
                    }
                    else
                    {
                        sheet[0, c].Value = c1TrueDBGrid1.Columns[i].Caption;
                    }
                    c++;
                }
            }
            //内容
            int sort = 1; //设置列的序号重1开始
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int show = 0;
                for (int j = 0; j < dt.Columns.Count + 1; j++)
                {
                    if (c1TrueDBGrid1.Splits[0].DisplayColumns[j].Visible == true)
                    {
                        if (j == 0)
                        {
                            sheet[i + 1, show].Value = sort;
                        }
                        else
                        {
                            if (dt.Columns[j - 1].DataType == typeof(DateTime))
                            {
                                if (dt.Rows[i][j - 1].ToString() != "")
                                { sheet[i + 1, show].Value = DateTime.Parse(dt.Rows[i][j - 1].ToString()).ToString("yyyy-MM-dd"); }
                            }
                            else
                            {
                                if (dt.Rows[i][j - 1].ToString() == "False")
                                {
                                    sheet[i + 1, show].Value = "否";
                                }
                                else if (dt.Rows[i][j - 1].ToString() == "True")
                                {
                                    sheet[i + 1, show].Value = "是";
                                }
                                else
                                {
                                    sheet[i + 1, show].Value = dt.Rows[i][j - 1];
                                }
                            }
                        }
                        show++;
                    }
                }
                sort++;
            }
            SaveFileDialog dlg1 = new SaveFileDialog();//导出地址
            dlg1.DefaultExt = "xls";
            dlg1.FileName = fileName + DateTime.Now.ToString("yyyyMMddss") + ".xls";  //命名加了秒，表面出现重复导出
            dlg1.RestoreDirectory = true;
            if (dlg1.ShowDialog() == DialogResult.OK)
            {
                string dialogPath = dlg1.FileName.Substring(0, dlg1.FileName.LastIndexOf("\\"));
                c1XLBook1.Save(dlg1.FileName);
                return dialogPath;
            }
            return null;
        }
        /// <summary>
        /// 查询字典，返回字典分类名称中字段的最大长度，改变下拉框的宽度
        /// </summary>
        /// <param name="parentCode">上级字典编码</param>
        /// <returns>下拉框宽度长度</returns>
        public static int CaculateComboBoxLength(string parentCode)
        {
            int length = 120;
            string sql = "select top 1 SGDName from dbo.Sys_GlobalDict where SGDParentCode = '" + parentCode + "' order by len(SGDName) desc";
            DataTable dt = SQL.GetDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                length = dt.Rows[0][0].ToString().Length;
                length = length * 14;
                if (length < 120)
                {
                    length = 120;
                }
                else if (length > 240)
                {
                    length = 240;
                }
            }
            return length;
        }

        /// <summary>
        /// 导出模板
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="head">表头(例如：全称|简称|主助记码)</param>
        /// <param name="changeColumn">需改变的列，输入第几列 (例如：1|2|3，代表需要改变第1列，第2列，第3列列宽)</param>
        /// <param name="columnWidth">需改变的列宽大小， (例如：100|200|300，与列相对应)</param>
        /// <returns></returns>
        public static string ImportTemplate(string fileName, string head, string changeColumn, string columnWidth)
        {
            C1.C1Excel.C1XLBook c1XLBook1 = new C1XLBook();
            XLSheet sheet = c1XLBook1.Sheets[0];
            //添加标题行
            int c = 0;
            //sheet[0, c].Value = "序号";
            for (int i = 0; i < head.Split('|').Length; i++)
            {
                sheet[0, c].Value = head.Split('|')[i];
                c++;
            }
            for (int j = 0; j < changeColumn.Split('|').Length; j++)
            {
                sheet.Columns[changeColumn.Split('|')[j].ToInt()].Width = C1XLBook.PixelsToTwips(columnWidth.Split('|')[j].ToInt());
            }
            SaveFileDialog dlg1 = new SaveFileDialog();//导出地址
            dlg1.DefaultExt = "xls";
            dlg1.FileName = fileName + DateTime.Now.ToString("yyyyMMdd") + ".xls";
            dlg1.RestoreDirectory = true;
            if (dlg1.ShowDialog() == DialogResult.OK)
            {
                string dialogPath = dlg1.FileName.Substring(0, dlg1.FileName.LastIndexOf("\\"));
                c1XLBook1.Save(dlg1.FileName);
                return dialogPath;
            }
            return null;
        }

        /// <summary>
        /// 导出模板并导出某列所有信息
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="head">表头(例如：全称|简称|主助记码)</param>
        /// <param name="changeColumn">需改变的列，输入第几列 (例如：1|2|3，代表需要改变第1列，第2列，第3列列宽)</param>
        /// <param name="columnWidth">需改变的列宽大小， (例如：100|200|300，与列相对应)</param>
        /// <param name="dt">导出列的表</param>>
        /// <returns></returns>
        public static string ImportTemplate(string fileName, string head, string changeColumn, string columnWidth, DataTable dt, bool isContiesNum = true)
        {
            C1.C1Excel.C1XLBook c1XLBook1 = new C1XLBook();
            XLSheet sheet = c1XLBook1.Sheets[0];
            //添加标题行
            int c = 0;
            if (isContiesNum)
            {
                sheet[0, c].Value = "序号";
                for (int i = 0; i < head.Split('|').Length; i++)
                {
                    c++;
                    sheet[0, c].Value = head.Split('|')[i];
                }
            }
            else
            {
                for (int i = 0; i < head.Split('|').Length; i++)
                {
                    sheet[0, i].Value = head.Split('|')[i];
                }
            }
            for (int j = 0; j < changeColumn.Split('|').Length; j++)
            {
                sheet.Columns[changeColumn.Split('|')[j].ToInt()].Width = C1XLBook.PixelsToTwips(columnWidth.Split('|')[j].ToInt());
            }

            //内容
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int show = 1;
                if (isContiesNum)
                {
                    show = 1;
                }
                else
                {
                    show = 0;
                }
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (dt.Columns[j].DataType == typeof(Decimal))
                    {
                        if (dt.Rows[i][j].ToString() != "")
                        {
                            sheet[i + 1, show].Value = Decimal.Parse(dt.Rows[i][j].ToString());
                        }
                    }
                    else if (dt.Columns[j].DataType == typeof(Int32))
                    {
                        if (dt.Rows[i][j].ToString() != "")
                        {
                            sheet[i + 1, show].Value = Int32.Parse(dt.Rows[i][j].ToString());
                        }
                    }
                    else if (dt.Columns[j].DataType == typeof(Double))
                    {
                        if (dt.Rows[i][j].ToString() != "")
                        {
                            sheet[i + 1, show].Value = Double.Parse(dt.Rows[i][j].ToString());
                        }
                    }
                    else if (dt.Columns[j].DataType == typeof(float))
                    {
                        if (dt.Rows[i][j].ToString() != "")
                        {
                            sheet[i + 1, show].Value = float.Parse(dt.Rows[i][j].ToString());
                        }
                    }

                    else if (dt.Columns[j].DataType == typeof(String))
                    {
                        if (dt.Rows[i][j].ToString() == "False")
                        {
                            sheet[i + 1, show].Value = "否";
                        }
                        else if (dt.Rows[i][j].ToString() == "True")
                        {
                            sheet[i + 1, show].Value = "是";
                        }
                        else
                        {
                            sheet[i + 1, show].Value = dt.Rows[i][j].ToString();
                        }
                    }
                    else
                    {
                        if (dt.Rows[i][j].ToString() != "")
                        {
                            sheet[i + 1, show].Value = dt.Rows[i][j].ToString();
                        }
                    }
                    show++;
                }
            }

            SaveFileDialog dlg1 = new SaveFileDialog();//导出地址
            dlg1.DefaultExt = "xls";
            dlg1.FileName = fileName + DateTime.Now.ToString("yyyyMMdd") + ".xls";
            dlg1.RestoreDirectory = true;
            if (dlg1.ShowDialog() == DialogResult.OK)
            {
                string dialogPath = dlg1.FileName.Substring(0, dlg1.FileName.LastIndexOf("\\"));
                c1XLBook1.Save(dlg1.FileName);
                return dlg1.FileName;
            }
            return null;
        }

        /// <summary>
        /// 清空选中产品
        /// </summary>
        /// <param name="c1TrueDBGrid1"></param>
        /// <param name="procode">产品字段</param>
        /// <param name="status">实体类状态</param>
        /// <param name="objectid">主键</param>
        public static void ClearProduct(this C1.Win.C1TrueDBGrid.C1TrueDBGrid c1TrueDBGrid1, string procode, string status, string objectid)
        {
            if (c1TrueDBGrid1.RowCount > 0)
            {
                var strsql = string.Empty;
                for (int i = c1TrueDBGrid1.RowCount; i >= 0; i--)
                {
                    var productcode = c1TrueDBGrid1[i, procode].ToString();
                    if (productcode != string.Empty)
                    {
                        var flag = c1TrueDBGrid1[i, "选择"].ToString() == string.Empty ? false : c1TrueDBGrid1[i, "选择"].ToString().ToBoolean();
                        if (flag == true)
                        {
                            if (!string.IsNullOrEmpty(objectid))
                            {
                                if (status != HelperEnum.GetEnumDesc(AuditStatusTypes.未审核) && status != HelperEnum.GetEnumDesc(AuditStatusTypes.未通过))
                                {
                                    MessageBox.Show("单据" + status + "，无法删除明细!");
                                    return;
                                }
                                c1TrueDBGrid1.Delete(i);
                                c1TrueDBGrid1.UpdateData();

                                //新增一行
                                DataTable dt = c1TrueDBGrid1.DataSource as DataTable;
                                dt.Rows.Add(dt.NewRow());
                                c1TrueDBGrid1.DataSource = dt;
                                c1TrueDBGrid1.Row = c1TrueDBGrid1.Row + 1;
                            }
                            else
                            {
                                c1TrueDBGrid1.Delete(i);
                                c1TrueDBGrid1.UpdateData();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 新增行
        /// </summary>
        /// <param name="table"></param>
        /// <param name="rows"></param>
        public static void AddRows(this DataTable table ,int rows=15)
        {
            for (int i = 0; i < rows; i++)
                table.Rows.Add(table.NewRow());
        }

    }//end class
}//end namespace
