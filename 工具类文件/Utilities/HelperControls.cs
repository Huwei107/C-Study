//-----------------------------------------------------------------------
// <copyright company="工品一号" file="ControlsHelper.cs">
//  Copyright (c)  V1.0.0.0  
//  创建作者:   刘少林
//  创建时间:   2018-03-29
//  功能描述:   Control控件帮助类
//  历史版本:
//          2018-03-29 刘少林 创建ControlsHelper.cs
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Windows.Forms;
using System.Linq;

namespace FX.MainForms
{
    /// <summary>
    ///  Control控件帮助类
    ///  以及根据业务情况，对控件的一些操作类也集中在这里
    /// </summary>
    public class HelperControls
    {
        /// <summary>
        /// 将所有控件设置只读
        /// </summary>
        /// <param name="controls">控件集合</param>
        /// <param name="status">布尔值状态</param>
        public static void SetReadOnly(Control.ControlCollection controls, bool status)
        {
            for (int i = 0; i < controls.Count; i++)
            {
                Control txt = controls[i];
                if (txt.GetType().Name == "TextBox")
                {
                    TextBox t = txt as TextBox;
                    t.ReadOnly = status;
                }
                if (txt.GetType().Name == "NumericUpDown")
                {
                    NumericUpDown t = txt as NumericUpDown;
                    t.ReadOnly = status;
                }
                if (txt.GetType().Name == "DateTimePicker")
                {
                    DateTimePicker t = txt as DateTimePicker;
                    t.Enabled = status;
                }

                if (txt.GetType().Name == "CheckBox" || txt.GetType().Name == "Button" || txt.GetType().Name == "DateTimePicker")
                {
                    txt.Enabled = !status;
                }
            }
        }


        /// 绑定审核状态的图片并使其显示在控件上方，不覆盖控件信息
        /// </summary>
        /// <param name="pictureBox1">图片对象</param>
        /// <param name="auditStatusDesc">审核状态描述</param>
        public static void AuditStatusImageBindInit(FXPictureBox pictureBox1, string auditStatusDesc)
        {
            ResourceManager rm = new ResourceManager("FX.MainForms.Properties.Resources", Assembly.GetExecutingAssembly());
            #region 暂时判断枚举来决定中文名称
            Image imageModel = null;
            if (auditStatusDesc == HelperEnum.GetEnumDesc(AuditStatusTypes.未审核))
            {
                imageModel = global::FX.MainForms.Properties.Resources.Auditing;
            }
            else if (auditStatusDesc == HelperEnum.GetEnumDesc(AuditStatusTypes.未通过))
            {
                imageModel = global::FX.MainForms.Properties.Resources.AuditFail;
            }
            else if (auditStatusDesc == HelperEnum.GetEnumDesc(AuditStatusTypes.已删除))
            {
                imageModel = global::FX.MainForms.Properties.Resources.Delete;
            }
            else if (auditStatusDesc == HelperEnum.GetEnumDesc(AuditStatusTypes.已审核))
            {
                imageModel = global::FX.MainForms.Properties.Resources.AuditSuccess;
            }
            else if (auditStatusDesc == HelperEnum.GetEnumDesc(AuditStatusTypes.已作废))
            {
                imageModel = global::FX.MainForms.Properties.Resources.Cancel;
            }
            else if (auditStatusDesc == HelperEnum.GetEnumDesc(AuditStatusTypes.已结案))
            {
                imageModel = global::FX.MainForms.Properties.Resources.EndCase;
            }
            else if (auditStatusDesc == HelperEnum.GetEnumDesc(ShipmentStatus.申请中))
            {
                imageModel = global::FX.MainForms.Properties.Resources.申请中;
            }
            else if (auditStatusDesc == HelperEnum.GetEnumDesc(ShipmentStatus.申请中))
            {
                imageModel = global::FX.MainForms.Properties.Resources.申请中;
            }
            else if (auditStatusDesc == HelperEnum.GetEnumDesc(ShipmentStatus.已取货))
            {
                imageModel = global::FX.MainForms.Properties.Resources.已取货;
            }
            else if (auditStatusDesc == HelperEnum.GetEnumDesc(ShipmentStatus.已送货))
            {
                imageModel = global::FX.MainForms.Properties.Resources.已发货;
            }
            else if (auditStatusDesc == HelperEnum.GetEnumDesc(ShipmentStatus.已收货))
            {
                imageModel = global::FX.MainForms.Properties.Resources.已收货;
            }
            else if (auditStatusDesc == HelperEnum.GetEnumDesc(AuditStatusTypes.启用))
            {
                imageModel = global::FX.MainForms.Properties.Resources.Enable;
            }
            else if (auditStatusDesc == HelperEnum.GetEnumDesc(AuditStatusTypes.禁用))
            {
                imageModel = global::FX.MainForms.Properties.Resources.Disabled;
            }
            else if (auditStatusDesc == HelperEnum.GetEnumDesc(AuditStatusTypes.已排产))
            {
                imageModel = global::FX.MainForms.Properties.Resources.已排产;
            }
            else if (auditStatusDesc == "对账中")
            {
                imageModel = global::FX.MainForms.Properties.Resources.对账中;
            }

            #endregion
            if (imageModel == null)
            {
                return;
            }
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Image = imageModel;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.BringToFront();
        }

        /// <summary>
        /// 隐藏删除和启用禁用按钮对业务详情窗体
        /// </summary>
        /// <param name="info">详情窗体对象</param>
        /// <remarks>由于权限先执行，次方法不要执行将Enabled设置为true,避免权限冲突</remarks>
        public static void HiddenDelAndEnableBtnForBillForm(frmBase info)
        {
            if (info is frmBaseInfo)
            {
                Dictionary<ButtonEnumList, int> exists = GetButtonDictionary(new[] { ButtonEnumList.删除, ButtonEnumList.禁用, ButtonEnumList.启用 });
                IList<FXDevButtonItem> buttons = GetFiltersButtonItem(info.HeaderButtons, exists);
                foreach (var btn in buttons)
                {
                    btn.Visible = false;
                }
            }
            else if (info is frmBaseList)
            {
                Dictionary<ButtonEnumList, int> exists = GetButtonDictionary(new[] { ButtonEnumList.删除, ButtonEnumList.禁用, ButtonEnumList.启用 });
                IList<FXDevButtonItem> buttons = GetFiltersButtonItem(info.HeaderButtons, exists);
                foreach (var btn in buttons)
                {
                    btn.Visible = false;
                }
            }
        }

        /// <summary>
        /// 作废和结案的业务单据详情窗体，禁用所有按钮
        /// </summary>
        /// <param name="info">详情窗体对象</param>
        /// <param name="auditDesc">审核状态描述</param>
        /// <remarks>由于权限先执行，次方法不要执行将Enabled设置为true,避免权限冲突</remarks>
        public static void DisableAllButtonForAuditStatus(frmBase info, string auditDesc)
        {
            if (HelperEnum.GetEnumDesc(AuditStatusTypes.已结案).Equals(auditDesc) || HelperEnum.GetEnumDesc(AuditStatusTypes.已排产).Equals(auditDesc))
            {
                SetEnableButton(info, false);
                Dictionary<ButtonEnumList, int> exists = GetButtonDictionary(new[] { ButtonEnumList.新增, ButtonEnumList.关闭, ButtonEnumList.打印, ButtonEnumList.打印图纸, ButtonEnumList.打印配置, ButtonEnumList.预览, ButtonEnumList.图纸打印配置, ButtonEnumList.图纸预览, ButtonEnumList.导出,ButtonEnumList.列配置 });
                IList<FXDevButtonItem> buttons = GetFiltersButtonItem(info.HeaderButtons, exists);
                foreach (var btn in buttons)
                {
                    btn.Enabled = true;
                }
            }
            else if (HelperEnum.GetEnumDesc(AuditStatusTypes.已审核).Equals(auditDesc))
            {
                info.SetEnableButton(true);
                Dictionary<ButtonEnumList, int> exists = GetButtonDictionary(new[] { ButtonEnumList.保存 });
                IList<FXDevButtonItem> buttons = GetFiltersButtonItem(info.HeaderButtons, exists);
                foreach (var btn in buttons)
                {
                    btn.Enabled = false;
                }
            }
            else if (string.IsNullOrEmpty(auditDesc))
            {
                //特殊控制,只显示保存按钮可操作
                info.SetEnableButton(false);
                Dictionary<ButtonEnumList, int> exists = GetButtonDictionary(new[] { ButtonEnumList.新增, ButtonEnumList.保存, ButtonEnumList.关闭, ButtonEnumList.列配置 });
                IList<FXDevButtonItem> buttons = GetFiltersButtonItem(info.HeaderButtons, exists);
                foreach (var btn in buttons)
                {
                    btn.Enabled = true;
                }
            }
            else if (HelperEnum.GetEnumDesc(AuditStatusTypes.已作废).Equals(auditDesc))
            {
                SetEnableButton(info, false);
                Dictionary<ButtonEnumList, int> exists = GetButtonDictionary(new[] { ButtonEnumList.新增, ButtonEnumList.关闭, ButtonEnumList.列配置 });
                IList<FXDevButtonItem> buttons = GetFiltersButtonItem(info.HeaderButtons, exists);
                foreach (var btn in buttons)
                {
                    btn.Enabled = true;
                }
            }
            else
            {
                info.SetEnableButton(true);
            }
        }

        /// <summary>
        /// 获取需要过滤后的按钮集合
        /// </summary>
        /// <param name="source">原始按钮集合</param>
        /// <param name="filters">过滤规则</param>
        /// <returns>按钮集合</returns>
        private static IList<FXDevButtonItem> GetFiltersButtonItem(IList<FXDevButtonItem> source, Dictionary<ButtonEnumList, int> filters)
        {
            if (filters == null || filters.Count == 0)
            {
                return source;
            }
            else
            {
                IList<FXDevButtonItem> items = new List<FXDevButtonItem>();
                if (source == null)
                {
                    return items;
                }
                //找出要过滤的按钮集合
                foreach (ButtonEnumList key in filters.Keys)
                {
                    string action = HelperEnum.GetEnumDesc(key);
                    if (!string.IsNullOrEmpty(action))
                    {
                        var results = source.Where(item => action.Equals(item.FXAuthActionFlag, StringComparison.CurrentCultureIgnoreCase));
                        if (results != null && results.Count() > 0)
                        {
                            items.Add(results.First());
                        }
                    }

                }
                return items;
            }
        }

        /// <summary>
        /// 设置窗体指定按钮可见
        /// </summary>
        /// <param name="rootForm">根窗体</param>
        /// <param name="isShow">可见布尔值,true:可见，false:不可见</param>
        /// <param name="filters">按钮集合</param>
        /// <remarks>权限不允许按钮可见，则按钮无法可见，无论是否设置可见</remarks>
        public static void SetVisualButton(frmBase rootForm, bool isShow, params ButtonEnumList[] filters)
        {
            Dictionary<ButtonEnumList, int> exists = GetButtonDictionary(filters);
            IList<FXDevButtonItem> buttons = GetFiltersButtonItem(rootForm.HeaderButtons, exists);
            foreach (var btn in buttons)
            {
                btn.Visible = isShow;
            }
        }

        /// <summary>
        /// 获取按钮字典
        /// </summary>
        /// <param name="buttons">按钮集合</param>
        /// <returns>按钮字典</returns>
        private static Dictionary<ButtonEnumList, int> GetButtonDictionary(ButtonEnumList[] buttons)
        {
            if (buttons != null && buttons.Length > 0)
            {
                Dictionary<ButtonEnumList, int> exists = new Dictionary<ButtonEnumList, int>(buttons.Length);
                foreach (ButtonEnumList btn in buttons)
                {
                    if (!exists.ContainsKey(btn))
                    {
                        exists.Add(btn, 1);
                    }
                }
                return exists;
            }
            else
            {
                return null;
            }

        }
        /// <summary>
        /// 设置窗体指定按钮禁用
        /// </summary>
        /// <param name="rootForm">根窗体</param>
        /// <param name="buttons">按钮集合,不传递则设置所有按钮禁用</param>
        /// <remarks>权限不允许按钮可用，则按钮无法可用，无论是否设置可用</remarks>
        public static void SetEnableButton(frmBase rootForm, bool isEnable, params ButtonEnumList[] buttons)
        {
            SetAuthtEnableButton(rootForm, isEnable, false, buttons);
        }


        /// <summary>
        /// 设置窗体指定按钮禁用
        /// </summary>
        /// <param name="rootForm">根窗体</param>
        /// <param name="buttons">按钮集合,不传递则设置所有按钮禁用</param>
        /// <param name="authMode">权限模式</param>
        /// <remarks>权限不允许按钮可用，则按钮无法可用，无论是否设置可用</remarks>
        public static void SetAuthtEnableButton(frmBase rootForm, bool isEnable, bool authMode, params ButtonEnumList[] filters)
        {
            Dictionary<ButtonEnumList, int> exists = GetButtonDictionary(filters);
            IList<FXDevButtonItem> buttons = GetFiltersButtonItem(rootForm.HeaderButtons, exists);
            foreach (var btn in buttons)
            {
                btn.Enabled = isEnable;
                if (authMode)
                {
                    btn.FXAuthIsDisabled = isEnable;
                }
            }
        }

        /// <summary>
        /// 单据按钮事件行为判断
        /// </summary>
        /// <param name="entityStatus">单据状态</param>
        /// <param name="status">操作的状态</param>
        /// <returns>true?有下级业务:不可操作</returns>
        public static bool ButtonPermissions(string entityStatus, AuditStatusTypes status, int type = 1)
        {
            bool result = true;
            if (entityStatus == HelperEnum.GetEnumDesc(AuditStatusTypes.未审核) && status == AuditStatusTypes.已结案)
            {
                HelperMessageBoxContent.ShowMessageOK("单据未审核，不可以结案!");
                result = false;
            }
            else if (entityStatus == HelperEnum.GetEnumDesc(AuditStatusTypes.已审核))
            {
                if (status == AuditStatusTypes.已审核)
                {
                    HelperMessageBoxContent.ShowMessageOK("单据已审核，不可重复操作!");
                    result = false;
                }
                else
                {
                    //下次业务单据
                    result = true;
                }
            }
            else if (entityStatus == HelperEnum.GetEnumDesc(AuditStatusTypes.未通过))
            {
                if (status == AuditStatusTypes.已审核)
                {
                    HelperMessageBoxContent.ShowMessageOK("单据未通过，不可以审核!");
                    result = false;
                }
                else if (status == AuditStatusTypes.已结案)
                {
                    HelperMessageBoxContent.ShowMessageOK("单据未通过，不可以结案!");
                    result = false;
                }
                else if (status == AuditStatusTypes.未通过)
                {
                    HelperMessageBoxContent.ShowMessageOK("单据未通过，不可重复操作!");
                    result = false;
                }
            }
            else if (entityStatus == HelperEnum.GetEnumDesc(AuditStatusTypes.已作废))
            {
                HelperMessageBoxContent.ShowMessageOK("单据已作废，不可操作!");
                result = false;
            }
            else if (entityStatus == HelperEnum.GetEnumDesc(AuditStatusTypes.已结案))
            {
                if (type == 1)
                {
                    HelperMessageBoxContent.ShowMessageOK("单据已结案，不可操作!");
                    result = false;
                }
            }
            return result;
        }

        /// <summary>
        /// 根据获取用户拥有的权限行为配置权限按钮
        /// </summary>
        /// <param name="rootForm">根窗体</param>
        /// <param name="table">权限行为数据表</param>
        public static void SetAuthtEnableButton(frmBase rootForm, DataTable table)
        {
            if (table != null && table.Rows.Count > 0 &&
                rootForm.HeaderButtons != null
                )
            {

                foreach (DataRow row in table.Rows)
                {
                    var results = rootForm.HeaderButtons.Where(item => row[0].ToString().Equals(item.FXAuthActionFlag, StringComparison.OrdinalIgnoreCase));
                    if (results != null && results.Count() > 0)
                    {
                        var btn = results.First();
                        btn.Enabled = true;
                        btn.FXAuthIsDisabled = true;
                    }
                }
            }
        }

    }
}