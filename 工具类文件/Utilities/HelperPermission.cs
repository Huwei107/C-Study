//-----------------------------------------------------------------------
// <copyright company="工品一号" file="HelperPermission.cs">
//  Copyright (c)  V1.0.0.0  
//  创建作者:   王延雄
//  创建时间:   2018-9-5
//  功能描述:   数字操作集合(常用操作数字方法)
//  历史版本:
//          2018-9-5 13:11:36 王延雄 创建HelperPermission类
//          2018-9-18         刘少林 针对GetModuleAction方法添加上级模块名称查询，优化SQL执行性能,新增登录模块合并查询操作!  @@20180918001
// </copyright>
//-----------------------------------------------------------------------
using DevComponents.DotNetBar;
using FX.ORM.Websharp.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.IO;
using DevExpress.XtraBars.Docking2010;

namespace FX.MainForms
{
    public class HelperPermission
    {
        /// <summary>
        /// 所有的模块行为
        /// </summary>
        /// <remarks>系统所有的权限模块和行为</remarks>
        public static DataView dvAllAction;

        /// <summary>
        /// 账号信息表
        /// </summary>
        public static DataTable dtAccount;

        /// <summary>
        /// 无权操作模块时显示的文本信息
        /// </summary>
        public static string showTextNoPermission = "无权操作此模块!";

        /// <summary>
        /// 账号权限行为
        /// </summary>
        /// <remarks>只包含当前登录用户拥有的权限和行为</remarks>
        public static DataTable dtAuthAction;

        public static void Login()
        {

        }

        /// <summary>
        /// 根据moduleTag判断是否有权限访问
        /// </summary>
        /// <param name="moduleTag"></param>
        /// <returns></returns>
        public static bool CheckModulePermission(string moduleTag)
        {
            bool sign = false;
            if (dvAllAction != null && dvAllAction.Table != null)
            {
                DataRow[] drs = dvAllAction.Table.Select("MTag='" + moduleTag + "'");
                if (drs != null && drs.Length > 0)
                {
                    sign = true;
                }
            }

            return sign;
        }

        /// <summary>
        /// 查询所有的模块和行为
        /// </summary>
        /// <param name="uid">用户编号</param>
        /// <param name="pwd">密码</param>
        /// <param name="initLoginer">是否初始化登录账号实体,false:不实例化,true:实例化登录实体</param>
        /// <remarks>本模块可以验证登录和获取登录成功后的权限模块行为!</remarks>
        /// <returns>模块数据视图对象</returns>
        public static DataView GetModuleAction(string uid, string pwd, bool initLoginer = false)
        {
            //20180918 添加上级模块名称查询，优化SQL执行性能,新增登录模块合并查询操作! @@20180918001
            string sql = @" SELECT * FROM Sys_User WITH(NOLOCK) WHERE UAccount=@UAccount AND UPwd=@UPwd AND UStatus=1
                            IF @@ROWCOUNT=1
                            --读取权限行为表（包含了所有的权限行为，和对应含有权限的配置组合表,操作的时候需要过滤读取所有模块，和过滤只有权限的数据!）
                            select m.MID,m.MName,m.MParentID,m.MTag,m.MISMenu,m.MSrc,m.MSort,m.MStatus,m2.MName as MParentName,a.AID,A.AName,A.ATag,A.ASort,A.AType,u.UAccount,u.UName,UPIS=1,ma.MAStatus,ma.MASort from Sys_Module m with(nolock)
                            inner join Sys_ModuleAction ma with(nolock) on m.MID=ma.MAModuleID
                            inner join Sys_Action a with(nolock) on ma.MAActionID=a.AID
                            left join Sys_RolePermission rp with(nolock) on rp.RPModuleID=ma.MAModuleID and rp.RPActionID=ma.MAActionID
                            left join Sys_UserRole ur with(nolock) on ur.URRoleID=rp.RPRoleID
                            left join Sys_User u with(nolock) on u.UID=ur.URUserID and u.UAccount=@UAccount and u.UStatus=1 
                            inner join Sys_Module m2 with(nolock) on m2.MID=m.MParentID
							left join Sys_Role r2  ON ur.URRoleID=r2.RID
                            where ma.MAStatus<>-1   AND m.MStatus=1  AND u.UAccount=@UAccount and r2.RStatus=1 
                            union
                            --用户特殊权限读取
                            select m.MID,m.MName,m.MParentID,m.MTag,m.MISMenu,m.MSrc,m.MSort,m.MStatus,m2.MName as MParentName,a.AID,A.AName,A.ATag,A.ASort,A.AType,u.UAccount,u.UName,UPIS,ma.MAStatus,ma.MASort from Sys_Module m with(nolock)
                            inner join Sys_ModuleAction ma with(nolock) on m.MID=ma.MAModuleID
                            inner join Sys_Action a with(nolock) on ma.MAActionID=a.AID
                            inner join Sys_UserPermission up on up.UPModuleID=ma.MAModuleID and up.UPActionID=ma.MAActionID
                            inner join Sys_User u on u.UID=up.UpUserID 
                            inner join Sys_Module m2 with(nolock) on m2.MID=m.MParentID
                            where m.MID=ma.MAModuleID and a.AID=ma.MAActionID and m.MStatus=1 and ma.MAStatus<>-1 
                            and u.UAccount=@UAccount AND m.MStatus=1 
                            union 
                            --读取没有权限行为模块 SELECT MID FROM Sys_Module WHERE MParentID=0 AND MStatus=1
                            select m.MID,m.MName,m.MParentID,m.MTag,m.MISMenu,m.MSrc,m.MSort,m.MStatus,MParentName='',AID=0,AName='',ATag='',ASort='',AType='',UAccount='',
                            UName='',UPIS=1 ,MAStatus=0,MASort=0  from Sys_Module m where m.MID IN(
                            select distinct MParentID from Sys_Module
								where MID in 
								(
									select RPModuleID 
									from Sys_User a, Sys_UserRole b, Sys_RolePermission c
									where a.UID=b.URUserID and b.URRoleID=c.RPRoleID and a.UAccount=@UAccount
								)
                            ) AND m.MStatus=1 
                    ";
            QueryParameterCollection qpc = new QueryParameterCollection();
            qpc.Add("@UAccount", uid);
            qpc.Add("@UPwd", pwd);
            //包含登录人员信息表（集合的第一个表记录）和权限行为模块表（集合的第二个表记录）
            DataSet ds = SQL.GetDataSet(sql, qpc);
            if (ds != null && ds.Tables.Count > 0)
            {
                dtAccount = ds.Tables[0];
                PersistenceGlobalData.SetLoginAccount(dtAccount);
            }
            if (ds != null && ds.Tables.Count > 1)
            {
                dvAllAction = ds.Tables[1].DefaultView;
                if (PersistenceGlobalData.CurrentUser != null && !PersistenceGlobalData.CurrentUser.IsManager)
                {
                    //非超级管理员才需要做行为限制过滤!
                    //读取指定账户权限
                    ds.Tables[1].DefaultView.RowFilter = " (UAccount='" + uid + "') OR (MParentName='' AND AID=0 AND UAccount='' ) ";
                }
                dtAuthAction = ds.Tables[1].DefaultView.ToTable();
                //移除过滤条件，避免其他地方有调用整体模块
                ds.Tables[1].DefaultView.RowFilter = string.Empty;
                //dvAllAction.RowFilter = " MParentID = 100 ";
            }
            return dvAllAction;
        }

        /// <summary>
        /// 不管是否有权限，加载所用行为按钮
        /// </summary>
        /// <param name="ModuleTag">模块标识</param>
        /// <param name="Type">类型INFO/LIST</param>
        /// <returns></returns>
        public static void GetModuleButtons(string ModuleTag, string Type)
        {
            if (dvAllAction != null)
            {
                //过滤指定模块标志和行为类型
                //MTag模块标志
                //AType行为类型，分别为详情，列表等
                dvAllAction.RowFilter = "MTag='" + ModuleTag + "'  and AType='" + Type + "'";
                //安装行为排序值升序排序!
                dvAllAction.Sort = " MASort ";
            }
        }


        /// <summary>
        /// 加载所有窗体按钮
        /// </summary>
        /// <param name="barButtons"></param>
        /// <param name="barButtons"></param>
        /// <param name="handler"></param>
        /// <param name="frmPath"></param>
        public static void LoadButton(frmBase frm, Bar barButtons, EventHandler handler, string frmPath)
        {
            if (barButtons == null || handler == null)
            {
                return;
            }
            if (HelperPermission.dvAllAction == null)
            {
                return;
            }
            //船体按钮列表
            List<FXDevButtonItem> btns = new List<FXDevButtonItem>();
            //临时权限按钮字典
            IDictionary<string, FXDevButtonItem> set = new Dictionary<string, FXDevButtonItem>();
            //读取行为标志（用于读取行为图标）
            string iconTags = string.Empty;
            IList<string> tags = new List<string>();
            foreach (DataRow row in HelperPermission.dvAllAction.Table.Rows)
            {
                string aTag = row["ATag"].ToString();
                if (!tags.Contains(aTag) && aTag.Length > 0)
                {
                    tags.Add(aTag);
                }
            }
            //行为ICON和行为之间关系字典
            IDictionary<string, Image> imgDictionary = new Dictionary<string, Image>();
            //移除多余tag到 IN条件中去! by arison!
            if (tags != null && tags.Count > 0)
            {
                foreach (string tag in tags)
                {
                    if (!PersistenceGlobalData.GlobalCacheSet.ContainsKey(tag))
                    {
                        iconTags += ",'" + tag + "'";
                    }
                    else
                    {
                        if (!imgDictionary.ContainsKey(tag))
                        {
                            imgDictionary.Add(tag, (Image)PersistenceGlobalData.GlobalCacheSet[tag]);
                        }
                    }
                }
            }
            if (iconTags.Length > 0)
            {
                //读取按钮对应图标
                string readIconImgsSql = "SELECT ATag,AIcon FROM Sys_Action WITH(NOLOCK) WHERE ATag IN (" + iconTags.Trim(',') + ") ";
                DataTable iconTable = iconTable = SQL.GetDataTable(readIconImgsSql);
                if (iconTable != null)
                {
                    foreach (DataRow row in iconTable.Rows)
                    {
                        string aTag = row["ATag"].ToString();
                        if (!imgDictionary.ContainsKey(aTag))
                        {
                            MemoryStream buf = new MemoryStream((byte[])row["AIcon"]);
                            Image image = Image.FromStream(buf, true);
                            imgDictionary.Add(aTag, image);
                            if (!PersistenceGlobalData.GlobalCacheSet.ContainsKey(aTag))
                            {
                                PersistenceGlobalData.GlobalCacheSet.Add(aTag, image);
                            }
                        }
                    }
                }
            }
            //读取所有视图按钮
            //此处查询有重复按钮问题，所以使用了字典结构去重，后期需要优化by arison 20180927
            for (int i = 0; i < HelperPermission.dvAllAction.Count; i++)
            {
                DataRow dr = HelperPermission.dvAllAction[i].Row;
                FXDevButtonItem btn = new FXDevButtonItem();
                //btn.Enabled = false;
                btn.Text = dr["AName"].ToString();
                btn.Name = dr["ATag"].ToString();
                if (imgDictionary.ContainsKey(dr["ATag"].ToString()))
                {
                    btn.ButtonStyle = eButtonStyle.ImageAndText;
                    btn.Image = imgDictionary[dr["ATag"].ToString()];
                }
                btn.Tag = dr["ATag"].ToString();
                btn.FXAuthActionFlag = dr["ATag"].ToString();
                btn.Click += handler;
                if (!set.ContainsKey(btn.Text))
                {
                    set.Add(btn.Text, btn);
                }

            }
            foreach (string key in set.Keys)
            {
                btns.Add(set[key]);
            }
            FXDevButtonItem closeBtn = new FXDevButtonItem();
            //btn.Enabled = false;
            closeBtn.Text = "关闭";
            closeBtn.Name = "Close";
            closeBtn.Tag = "Close";
            closeBtn.FXAuthActionFlag = "Close";
            closeBtn.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            closeBtn.Image = global::FX.MainForms.Properties.Resources.guanbi_b;
            closeBtn.Click += delegate(object sender, EventArgs e) { frm.Close(); };
            btns.Add(closeBtn);

            //权限适配(过滤权限)
            FilterAuthModes(frm, btns);

            foreach (FXDevButtonItem item in btns)
            {
                barButtons.Items.Add(item);
                //保存当前窗体按钮集合
                frm.HeaderButtons.Add(item);
            }
            //缓存权限
            //if (PersistenceGlobalData.GlobalCacheSet.ContainsKey(frmPath))
            //{
            //    List<ButtonItem> items = ((List<ButtonItem>)PersistenceGlobalData.GlobalCacheSet[frmPath]);
            //    foreach (ButtonItem item in btns)
            //    {
            //        items.Add(item);
            //    }
            //}
            //else
            //{
            //    PersistenceGlobalData.GlobalCacheSet.Add(frmPath, btns);
            //}
            ////按钮加入视图
            //if (((List<ButtonItem>)PersistenceGlobalData.GlobalCacheSet[frmPath]).Count > 0)
            //{
            //    barButtons.Items.AddRange(((List<ButtonItem>)PersistenceGlobalData.GlobalCacheSet[frmPath]).ToArray());
            //}
            //}

        }

        /// <summary>
        /// 过滤权限
        /// </summary>
        /// <param name="frm">窗体对象</param>
        /// <param name="items">视图功能按钮</param>
        /// <remarks>有权限的就启用，否则enabled = false;</remarks>
        private static void FilterAuthModes(frmBase frm, IList<FXDevButtonItem> items)
        {
            //获取指定窗体路径
            HelperPermission.dtAuthAction.DefaultView.RowFilter = " MTag='" + frm.ModuleTag + "' AND AType='" + frm.FormType + "' ";
            //无权限按钮将被禁用 
            foreach (var btn in items)
            {
                if (HelperPermission.dtAuthAction.DefaultView.ToTable().Rows.Cast<DataRow>().Any(item => item["ATag"].ToString().Equals(btn.FXAuthActionFlag, StringComparison.CurrentCultureIgnoreCase)) ||
                   ActionValues.Close.ToString().Equals(btn.FXAuthActionFlag, StringComparison.CurrentCultureIgnoreCase))
                {
                    //默认关闭按钮等公共按钮不受权限管控!
                    btn.FXAuthIsDisabled = false;
                    btn.Enabled = true;
                }
                else
                {
                    btn.FXAuthIsDisabled = true;
                    btn.Enabled = false;
                }
            }
            //移除过滤条件，方便其他窗体读取权限配置
            HelperPermission.dtAuthAction.DefaultView.RowFilter = string.Empty;
        }

    }
}
