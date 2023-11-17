//-----------------------------------------------------------------------
// <copyright company="工品一号" file="AuthHelper.cs">
//  Copyright (c)  V1.0.0.0  
//  创建作者:   刘少林
//  创建时间:   2018-03-29
//  功能描述:   AuthHelper主要用于系统权限同一代码处理
//  历史版本:
//          2018-03-29 刘少林 创建AuthHelper.cs
// </copyright>
//-----------------------------------------------------------------------
using FX.Entity;
using FX.ORM.Websharp.ORM.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;

namespace FX.MainForms
{
    /// <summary>
    ///  AuthHelper主要用于系统权限同一代码处理
    /// </summary>
    public class HelperAuth
    {
        /// <summary>
        /// 权限映射按钮
        /// </summary>
        /// <remarks>根据权限配置来启用或者禁用按钮</remarks>
        public static void AuthMapButtons(frmBase root)
        {
            if (PersistenceGlobalData.CurrentUser == null)
            {
                //没有登录账号，则所有按钮无权限操作
                HelperControls.SetAuthtEnableButton(root, false, true);
                return;
            }
            //超级管理员，所有按钮可点击
            if (PersistenceGlobalData.CurrentUser.IsManager)
            {
                HelperControls.SetAuthtEnableButton(root, true, true);
                return;
            }
            string frmSrc = root.ToString();
            string authSrc = frmSrc.Substring(0, frmSrc.IndexOf(',')).ToLower();
            if (PersistenceGlobalData.CurrentUser.Actions != null && !PersistenceGlobalData.CurrentUser.Actions.ContainsKey(authSrc))
            {
                PersistenceGlobalData.CurrentUser.Actions.Add(authSrc, GetFormAuthActions(authSrc, PersistenceGlobalData.CurrentUser.Id));
            }
            if (PersistenceGlobalData.CurrentUser.Actions.ContainsKey(authSrc))
            {
                //获取特定用户指定窗体的权限按钮
                DataTable table = PersistenceGlobalData.CurrentUser.Actions[authSrc];
                //默认禁用所有按钮,然后按权限开启对应
                root.SetEnableButton(false);
                if (table != null && table.Rows.Count > 0)
                {
                    HelperControls.SetAuthtEnableButton(root, table);
                }
            }
            else
            {
                //没有执行权限，则所有按钮无权限操作
                HelperControls.SetAuthtEnableButton(root, false, true);
            }
        }

        private void SetAuthButton(frmBase root)
        {

        }

        /// <summary>
        /// 获取特定用户指定窗体的权限行为列表
        /// </summary>
        /// <param name="authSrc">需要判断的权限所属窗体逻辑路径</param>
        /// <param name="userId">账户主键</param>
        public static DataTable GetFormAuthActions(string authSrc, string userId = "")
        {
            DataTable actions = new DataTable();
            if (string.IsNullOrEmpty(authSrc) || authSrc.Trim().Length == 0)
            {
                return actions;
            }
            if (string.IsNullOrEmpty(userId) && PersistenceGlobalData.CurrentUser != null)
            {
                //获取当前登录用户的主键
                userId = PersistenceGlobalData.CurrentUser.Id;
            }
            if (!string.IsNullOrEmpty(userId))
            {
                List<SqlParameter> list = new List<SqlParameter>();
                list.Add(new SqlParameter("@SGUEmployeeId", userId));
                list.Add(new SqlParameter("@SEMSrc", authSrc));
                list.Add(new SqlParameter("@SystemId", PersistenceGlobalData.SystemId));
                string sql = @"
                            --获取指定用户包含的模块行为（并指定了窗体逻辑路径，仅查询一个窗体的所有权限行为）
                            SELECT MACTION.SMAActionFlag FROM Sys_ModuleAction MACTION
                            INNER JOIN 
                            (SELECT SONActionId FROM Sys_AuthGroupActions WITH(NOLOCK) WHERE SONSystemId=@SystemId AND
                            SONGroupId IN (
                            SELECT SGUAuthGroupId FROM Sys_AuthGroupUser WITH(NOLOCK) WHERE SGUEmployeeId=@SGUEmployeeId AND SGUEmployeeId=@SystemId 
                            )) USERActions
                            ON MACTION.SMAId=USERActions.SONActionId
                            INNER JOIN Sys_Modules MODU
                            ON MODU.SEMId=MACTION.SMAModuleId
                            WHERE MODU.SEMSrc=@SEMSrc AND MODU.SEMSystemId=@SystemId
                            UNION
                            --获取全启用的模块行为(不受权限管控)
                            SELECT MACTION.SMAActionFlag FROM Sys_ModuleAction MACTION
                            INNER JOIN Sys_Modules MODU
                            ON MODU.SEMId=MACTION.SMAModuleId
                            WHERE MODU.SEMSrc=@SEMSrc AND MODU.SEMSystemId=@SystemId  AND MACTION.SMAStatus='" + AuditStatusTypes.启用 + "'";
                return SQL.GetDataTable(sql, list.ToArray());
            }
            return actions;
        }
    }
}
