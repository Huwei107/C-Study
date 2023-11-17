
//-----------------------------------------------------------------------
// <copyright company="工品一号" file="PrintHelper">
//  Copyright (c)  V1.0.0.0  
//  创建作者:   唐亮
//  创建时间:   2018-04-11 16:57:57
//  功能描述:   【请输入类描述】
//  历史版本:
//          2018-04-11 16:57:57 唐亮 创建PrintHelper类
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using C1.Win.C1Report;
using DevExpress.XtraPrinting.BarCode;
using DevExpress.XtraPrinting.BarCode.Native;
using DevExpress.XtraReports.UI;
using FX.Entity;
using XtraReportsDesign;

namespace FX.MainForms
{
    /// <summary>
    /// 【C1打印帮助类】
    /// </summary>
    public class HelperPrint
    {
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("winspool.drv")]
        public static extern bool SetDefaultPrinter(String Name);


        private const string PrintFilePath = "print.ini";

        /// <summary>
        /// c1打印
        /// </summary>
        public static void c1ModelPrint(string txtname, DataTable dt, bool Isprintview = true)
        {
            string path = string.Empty;
            string printname = string.Empty;
            string Paper = string.Empty;
            string name = string.Empty;
            int width = 0;
            int height = 0;
            bool landscape = false;
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add(nameof(Sys_PrintConfiguration.SPCConfigFileName), txtname);
                param.Add(nameof(Sys_PrintConfiguration.SPCUserLoginID), PersistenceGlobalData.CurrentUser.Id);
                var entity = SQL.GetEntityByDictionary(typeof(Sys_PrintConfiguration), param) as Sys_PrintConfiguration;
                if (entity != null)
                {
                    path = entity.SPCConfigFilePath;
                    printname = entity.SPCPrintName;
                    Paper = entity.SPCPrintPaperType;
                    name = entity.SPCPaperName;
                    width = entity.SPCPaperWidth.ToString().ToInt();
                    height = entity.SPCPaperHeight.ToString().ToInt();
                    landscape = entity.SPCIsLandscape;
                }
                else
                {
                    StringBuilder str_temp = new StringBuilder(500);
                    string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PrintFilePath);
                    GetPrivateProfileString(txtname, "xmlname", "", str_temp, 200, filePath);
                    path = str_temp.ToString();

                    GetPrivateProfileString(txtname, "printname", "", str_temp, 100, filePath);
                    printname = str_temp.ToString();

                    GetPrivateProfileString(txtname, "paper", "", str_temp, 100, filePath);
                    Paper = str_temp.ToString();

                    GetPrivateProfileString(txtname, "width", "", str_temp, 100, filePath);
                    width = Convert.ToInt16(str_temp.ToString() == "" ? "0" : str_temp.ToString());

                    GetPrivateProfileString(txtname, "height", "", str_temp, 100, filePath);
                    height = Convert.ToInt16(str_temp.ToString() == "" ? "0" : str_temp.ToString());

                    GetPrivateProfileString(txtname, "landscape", "", str_temp, 100, filePath);
                    landscape = str_temp.ToString().ToBoolean();

                    GetPrivateProfileString(txtname, "pagename", "", str_temp, 100, filePath);
                    name = str_temp.ToString();
                }
            }
            catch (Exception)
            {
                HelperMessageBoxContent.ShowMessageOK("配置信息有错误2");
                return;
            }
            try
            {
                C1Report c1Report1 = new C1Report();
                c1Report1.Load(path, name);
                System.Drawing.Printing.PageSettings ps = new PageSettings();
                ps.PaperSize = new System.Drawing.Printing.PaperSize(Paper, width, height);
                ps.Landscape = landscape;
                if (Isprintview)
                {
                    PrintDocument fPrintDocument = new PrintDocument();
                    if (fPrintDocument.PrinterSettings.PrinterName != printname)
                        SetDefaultPrinter(printname);
                    PrinterSettings pt = new PrinterSettings();
                    c1Report1.DataSource.Recordset = dt;
                    if (printname == "0")
                    {
                        frmPrintView frm = new frmPrintView(c1Report1.Document, ps, pt);
                        frm.Show();
                    }
                    else
                    {
                        pt.PrinterName = printname;
                        frmPrintView frm = new frmPrintView(c1Report1.Document, ps, pt);
                        frm.Show();
                        //c1Report1.Render();
                    }
                }
                else
                {
                    c1Report1.DataSource.Recordset = dt;
                    if (printname != "0")
                        c1Report1.Document.PrinterSettings.PrinterName = printname;
                    c1Report1.Document.DefaultPageSettings = ps;
                    c1Report1.Document.Print();
                }
            }
            catch (Exception) //找不到打印机就再找一遍
            {
                try
                {
                    C1Report c1Report1 = new C1Report();
                    c1Report1.Load(path, name);
                    System.Drawing.Printing.PageSettings ps = new PageSettings();
                    ps.PaperSize = new System.Drawing.Printing.PaperSize(Paper, width, height);
                    ps.Landscape = landscape;
                    if (Isprintview)
                    {
                        PrinterSettings pt = new PrinterSettings();
                        c1Report1.DataSource.Recordset = dt;
                        if (printname == "0")
                        {
                            frmPrintView frm = new frmPrintView(c1Report1.Document, ps, pt);
                            frm.Show();
                        }
                        else
                        {
                            pt.PrinterName = printname;
                            frmPrintView frm = new frmPrintView(c1Report1.Document, ps, pt);
                            frm.Show();
                        }
                    }
                    else
                    {
                        c1Report1.DataSource.Recordset = dt;
                        if (printname != "0")
                            c1Report1.Document.PrinterSettings.PrinterName = printname;
                        c1Report1.Document.DefaultPageSettings = ps;
                        c1Report1.Document.Print();
                    }
                    return;
                }
                catch (Exception err)
                {
                    HelperMessageBoxContent.ShowMessageOK(err.Message);
                    return;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="txtname"></param>
        /// <param name="dt"></param>
        /// <param name="pre">是否显示打印按钮</param>
        /// <param name="Isprintview">是否预览</param>
        public static void c1ModelPrint(string txtname, DataTable dt, bool pre, bool Isprintview = true)
        {
            string path = string.Empty;
            string printname = string.Empty;
            string Paper = string.Empty;
            string name = string.Empty;
            int width = 0;
            int height = 0;
            bool landscape = false;
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add(nameof(Sys_PrintConfiguration.SPCConfigFileName), txtname);
                param.Add(nameof(Sys_PrintConfiguration.SPCUserLoginID), PersistenceGlobalData.CurrentUser.Id);
                var entity = SQL.GetEntityByDictionary(typeof(Sys_PrintConfiguration), param) as Sys_PrintConfiguration;
                if (entity != null)
                {
                    path = entity.SPCConfigFilePath;
                    printname = entity.SPCPrintName;
                    Paper = entity.SPCPrintPaperType;
                    name = entity.SPCPaperName;
                    width = entity.SPCPaperWidth.ToString().ToInt();
                    height = entity.SPCPaperHeight.ToString().ToInt();
                    landscape = entity.SPCIsLandscape;
                }
                else
                {
                    StringBuilder str_temp = new StringBuilder(500);
                    string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PrintFilePath);
                    GetPrivateProfileString(txtname, "xmlname", "", str_temp, 200, filePath);
                    path = str_temp.ToString();

                    GetPrivateProfileString(txtname, "printname", "", str_temp, 100, filePath);
                    printname = str_temp.ToString();

                    GetPrivateProfileString(txtname, "paper", "", str_temp, 100, filePath);
                    Paper = str_temp.ToString();

                    GetPrivateProfileString(txtname, "width", "", str_temp, 100, filePath);
                    width = Convert.ToInt16(str_temp.ToString());

                    GetPrivateProfileString(txtname, "height", "", str_temp, 100, filePath);
                    height = Convert.ToInt16(str_temp.ToString());

                    GetPrivateProfileString(txtname, "landscape", "", str_temp, 100, filePath);
                    landscape = Convert.ToBoolean(str_temp.ToString());

                    GetPrivateProfileString(txtname, "pagename", "", str_temp, 100, filePath);
                    name = str_temp.ToString();
                }
            }
            catch (Exception)
            {
                HelperMessageBoxContent.ShowMessageOK("配置信息有错误2");
                return;
            }
            try
            {
                path = Application.StartupPath + path.Substring(1, path.Length - 1);

                C1Report c1Report1 = new C1Report();

                if (txtname == "工令单打印" && dt != null && dt.Rows.Count > 0)
                {
                    var imgPath = dt.Rows[0]["img"].ToString();
                    AddPictureByWorkOrder(path, imgPath);
                }

                c1Report1.Load(path, name);
                System.Drawing.Printing.PageSettings ps = new PageSettings();
                ps.PaperSize = new System.Drawing.Printing.PaperSize(Paper, width, height);
                ps.Landscape = landscape;

                if (Isprintview)
                {
                    PrintDocument fPrintDocument = new PrintDocument();
                    if (fPrintDocument.PrinterSettings.PrinterName != printname)
                        SetDefaultPrinter(printname);
                    PrinterSettings pt = new PrinterSettings();
                    c1Report1.DataSource.Recordset = dt;
                    if (printname == "0")
                    {
                        frmPrintView frm = new frmPrintView(c1Report1.Document, ps, pt);
                        frm.Show();
                    }
                    else
                    {
                        pt.PrinterName = printname;
                        frmPrintView frm = new frmPrintView(c1Report1.Document, ps, pt);
                        frm.Show();
                    }
                }
                else
                {
                    c1Report1.DataSource.Recordset = dt;
                    if (printname != "0")
                        c1Report1.Document.PrinterSettings.PrinterName = printname;
                    c1Report1.Document.DefaultPageSettings = ps;
                    c1Report1.Document.Print();
                }
            }
            catch (Exception) //找不到打印机就再找一遍
            {
                try
                {

                    C1Report c1Report1 = new C1Report();
                    if (txtname == "工令单打印" && dt != null && dt.Rows.Count > 0)
                    {
                        var imgPath = dt.Rows[0]["img"].ToString();
                        AddPictureByWorkOrder(path, imgPath);
                    }
                    c1Report1.Load(path, name);
                    System.Drawing.Printing.PageSettings ps = new PageSettings();
                    ps.PaperSize = new System.Drawing.Printing.PaperSize(Paper, width, height);
                    ps.Landscape = landscape;
                    if (Isprintview)
                    {
                        PrinterSettings pt = new PrinterSettings();
                        c1Report1.DataSource.Recordset = dt;
                        if (printname == "0")
                        {
                            frmPrintView frm = new frmPrintView(c1Report1.Document, ps, pt);
                            frm.Show();
                        }
                        else
                        {
                            pt.PrinterName = printname;
                            frmPrintView frm = new frmPrintView(c1Report1.Document, ps, pt);
                            frm.Show();
                        }
                    }
                    else
                    {
                        c1Report1.DataSource.Recordset = dt;
                        if (printname != "0")
                            c1Report1.Document.PrinterSettings.PrinterName = printname;
                        c1Report1.Document.DefaultPageSettings = ps;
                        c1Report1.Document.Print();
                    }
                }
                catch (Exception err)
                {
                    HelperMessageBoxContent.ShowMessageOK(err.Message);
                    return;
                }
            }
        }

        /// <summary>
        /// 判断XML中的节点是否存在（根据节点名称）
        /// </summary>
        /// <param name="pic"></param>
        /// <param name="xnl"></param>
        /// <returns></returns>
        private static bool CheckPicName(string picName, XmlNodeList xnl)
        {
            for (int i = 0; i < xnl.Count; i++)
            {
                if (xnl[i].Name.ToLower().Equals(picName))
                    return true;
            }
            return false;
        }
        /// <summary>
        /// 根据模板路径以及图纸路径打印工令单
        /// </summary>
        /// <param name="path"></param>
        /// <param name="dt"></param>
        private static void AddPictureByWorkOrder(string path, string pathurl)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            XmlNode root = doc.SelectSingleNode("/Reports");
            XmlNodeList childlist = root.ChildNodes;
            XmlNodeList nodelist = doc.SelectNodes("/Reports/Report/Fields/Field");
            for (int i = 0; i < nodelist.Count; i++)
            {
                XmlNode cnode = nodelist[i];
                if (cnode.HasChildNodes)
                {
                    XmlNodeList clist = cnode.ChildNodes;
                    for (int j = 0; j < clist.Count; j++)
                    {
                        XmlNode ccnode = clist[j];

                        if (ccnode.Name.ToLower().Equals("name"))
                        {
                            if (ccnode.InnerText == "img")
                            {
                                if (CheckPicName("picture", clist))
                                {

                                    XmlElement xe = (XmlElement)cnode.SelectSingleNode("Picture");
                                    xe.InnerText = pathurl;
                                    // btnPrint.Tag = xe.InnerText;
                                    break;
                                }
                                else
                                {
                                    XmlElement xe = doc.CreateElement("Picture");
                                    xe.InnerText = pathurl;
                                    //  xe.InnerText = GetPictureData(pathurl).ToString();
                                    // btnPrint.Tag = xe.InnerText;
                                    cnode.AppendChild(xe);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            doc.Save(path);
        }


        #region 工令单打印或预览模块
#warning 此处必须改进，已经冗余了大量代码！
        /// <summary>
        /// 
        /// </summary>
        /// <param name="txtname"></param>
        /// <param name="dt"></param>
        /// <param name="pre">是否显示打印按钮</param>
        ///  <param name="landscape">是否反转</param>
        /// <param name="isPrintSeal">是否打印公章</param>
        /// <param name="Isprintview">是否预览</param>
        public static void PrintOrPreviewWorkOrderDev2(string txtname, DataTable dt, bool pre, bool landscape, bool isPrintSeal, bool Isprintview = true, bool isWorkorderImage = false, bool isPrintLogo = false, Sys_ReportManager reportInfo = null)
        {
            string path = string.Empty;
            string printname = string.Empty;
            string Paper = string.Empty;
            string name = string.Empty;
            int width = 0;
            int height = 0;
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add(nameof(Sys_PrintConfiguration.SPCConfigFileName), txtname);
                param.Add(nameof(Sys_PrintConfiguration.SPCUserLoginID), PersistenceGlobalData.CurrentUser.Id);
                var entity = SQL.GetEntityByDictionary(typeof(Sys_PrintConfiguration), param) as Sys_PrintConfiguration;
                if (entity != null)
                {
                    path = entity.SPCConfigFilePath;
                    printname = entity.SPCPrintName;
                    Paper = entity.SPCPrintPaperType;
                    name = entity.SPCPaperName;
                    width = entity.SPCPaperWidth.ToString().ToInt();
                    height = entity.SPCPaperHeight.ToString().ToInt();
                    landscape = entity.SPCIsLandscape;
                }
                else
                {
                    StringBuilder str_temp = new StringBuilder(500);
                    string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PrintFilePath);
                    GetPrivateProfileString(txtname, "xmlname", string.Empty, str_temp, 200, filePath);
                    path = str_temp.ToString();


                    GetPrivateProfileString(txtname, "printname", string.Empty, str_temp, 100, filePath);
                    printname = str_temp.ToString();

                    GetPrivateProfileString(txtname, "paper", string.Empty, str_temp, 100, filePath);
                    Paper = str_temp.ToString();

                    GetPrivateProfileString(txtname, "width", string.Empty, str_temp, 100, filePath);
                    width = Convert.ToInt16(str_temp.ToString());

                    GetPrivateProfileString(txtname, "height", string.Empty, str_temp, 100, filePath);
                    height = Convert.ToInt16(str_temp.ToString());

                    GetPrivateProfileString(txtname, "landscape", string.Empty, str_temp, 100, filePath);
                    landscape = Convert.ToBoolean(str_temp.ToString());

                    GetPrivateProfileString(txtname, "pagename", string.Empty, str_temp, 100, filePath);
                    name = str_temp.ToString();
                }
            }
            catch (Exception ex)
            {
                HelperLog.Write(ex);
                HelperMessageBoxContent.ShowMessageOK("配置信息有错误");
                return;
            }
            DataTable dtInfo = HelperPrint.GetPrintCount(txtname, dt);
            var resultStr = InvokePrintPic(dtInfo, Isprintview, path,
                   txtname,
                   printname, txtname,
                   height, width,
                    Paper, name, landscape, reportInfo);
            if (!Isprintview)//判断是否是打印
            {
                bool b = RecordPrintCount(txtname, dt);
                if (!b)
                {
                    HelperMessageBoxContent.ShowMessageOK("添加打印次数记录失败！");
                }
            }
            if (resultStr != "success")
            {
                HelperMessageBoxContent.ShowMessageOK(resultStr);
            }
        }
        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="txtname">配置字段</param>
        /// <param name="dt">打印数据</param>
        /// <param name="pre">是否显示打印按钮</param>
        /// <param name="landscape">是否横向打印</param>
        /// <param name="isPrintSeal">是否打印印章</param>
        /// <param name="Isprintview">是否预览</param>
        /// <param name="isWorkorderImage">是否打印工令图纸</param>
        /// <param name="isPrintLogo">是否打印logo</param>
        /// <param name="reportInfo">模板</param>
        public static void PrintOrPreviewWorkOrderDev(string txtname, DataTable dt, bool pre, bool landscape, bool isPrintSeal, bool Isprintview = true, bool isWorkorderImage = false, bool isPrintLogo = false, Sys_ReportManager reportInfo = null)
        {
            string path = string.Empty;
            string printname = string.Empty;
            string Paper = string.Empty;
            string name = string.Empty;
            int width = 0;
            int height = 0;
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add(nameof(Sys_PrintConfiguration.SPCConfigFileName), txtname);
                param.Add(nameof(Sys_PrintConfiguration.SPCUserLoginID), PersistenceGlobalData.CurrentUser.Id);
                var entity = SQL.GetEntityByDictionary(typeof(Sys_PrintConfiguration), param) as Sys_PrintConfiguration;
                if (entity != null)
                {
                    path = entity.SPCConfigFilePath;
                    printname = entity.SPCPrintName;
                    Paper = entity.SPCPrintPaperType;
                    name = entity.SPCPaperName;
                    width = entity.SPCPaperWidth.ToString().ToInt();
                    height = entity.SPCPaperHeight.ToString().ToInt();
                    landscape = entity.SPCIsLandscape;
                }
                else
                {
                    StringBuilder str_temp = new StringBuilder(500);
                    string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PrintFilePath);
                    GetPrivateProfileString(txtname, "xmlname", string.Empty, str_temp, 200, filePath);
                    path = str_temp.ToString();

                    GetPrivateProfileString(txtname, "printname", string.Empty, str_temp, 100, filePath);
                    printname = str_temp.ToString();

                    GetPrivateProfileString(txtname, "paper", string.Empty, str_temp, 100, filePath);
                    Paper = str_temp.ToString();

                    GetPrivateProfileString(txtname, "width", string.Empty, str_temp, 100, filePath);
                    width = Convert.ToInt16(str_temp.ToString());

                    GetPrivateProfileString(txtname, "height", string.Empty, str_temp, 100, filePath);
                    height = Convert.ToInt16(str_temp.ToString());

                    GetPrivateProfileString(txtname, "landscape", string.Empty, str_temp, 100, filePath);
                    landscape = Convert.ToBoolean(str_temp.ToString());

                    GetPrivateProfileString(txtname, "pagename", string.Empty, str_temp, 100, filePath);
                    name = str_temp.ToString();
                }
                var list = PrinterSettings.InstalledPrinters.Cast<string>().ToList();
                var p = list.FirstOrDefault(x => x == printname);
                if (p == null)
                {
                    HelperMessageBoxContent.ShowMessageOK($"打印机未找到:{txtname}-{printname},请先设置打印机");
                    HelperLog.Write($"打印机未找到：{txtname}-{printname}");
                    return;
                }
            }
            catch (Exception ex)
            {
                HelperMessageBoxContent.ShowMessageOK("配置信息有错误");
                HelperLog.Write(ex);
                return;
            }

            DataTable dtInfo = HelperPrint.GetPrintCount(txtname, dt);
            var resultStr = InvokePrint(dtInfo, Isprintview, path,
                  txtname,
                  printname, txtname,
                  height, width,
                  Paper, name, landscape, isPrintSeal, isWorkorderImage, isPrintLogo, reportInfo);
            if (!Isprintview)//判断是否是打印
            {
                bool b = RecordPrintCount(txtname, dt);
                if (!b)
                {
                    HelperMessageBoxContent.ShowMessageOK("添加打印次数记录失败！");
                }
            }
            if (resultStr != "success")
            {
                HelperMessageBoxContent.ShowMessageOK(resultStr);
            }
        }

        /// <summary>
        /// 带图片打印
        /// </summary>
        /// <param name="txtname">配置字段</param>
        /// <param name="dt">打印数据</param>
        /// <param name="landscape">是否横向打印</param>
        /// <param name="isPrintSeal">是否打印印章</param>
        /// <param name="Isprintview">是否预览</param>
        /// <param name="isWorkorderImage">是否打印工令图纸</param>
        /// <param name="isPrintLogo">是否打印logo</param>
        /// <param name="reportInfo">模板</param>
        /// <param name="bitmaps">图片</param>
        public static void PrintOrPreviewWithPicture(string txtname, DataTable dt, bool landscape,
            bool isPrintSeal, bool Isprintview = true, bool isWorkorderImage = false, bool isPrintLogo = false, Sys_ReportManager reportInfo = null, List<Bitmap> bitmaps = null)
        {
            string path = string.Empty;
            string printname = string.Empty;
            string Paper = string.Empty;
            string name = string.Empty;
            int width = 0;
            int height = 0;
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add(nameof(Sys_PrintConfiguration.SPCConfigFileName), txtname);
                param.Add(nameof(Sys_PrintConfiguration.SPCUserLoginID), PersistenceGlobalData.CurrentUser.Id);
                var entity = SQL.GetEntityByDictionary(typeof(Sys_PrintConfiguration), param) as Sys_PrintConfiguration;
                if (entity != null)
                {
                    path = entity.SPCConfigFilePath;
                    printname = entity.SPCPrintName;
                    Paper = entity.SPCPrintPaperType;
                    name = entity.SPCPaperName;
                    width = entity.SPCPaperWidth.ToString().ToInt();
                    height = entity.SPCPaperHeight.ToString().ToInt();
                    landscape = entity.SPCIsLandscape;
                }
                else
                {
                    StringBuilder str_temp = new StringBuilder(500);
                    string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PrintFilePath);
                    GetPrivateProfileString(txtname, "xmlname", string.Empty, str_temp, 200, filePath);
                    path = str_temp.ToString();

                    GetPrivateProfileString(txtname, "printname", string.Empty, str_temp, 100, filePath);
                    printname = str_temp.ToString();

                    GetPrivateProfileString(txtname, "paper", string.Empty, str_temp, 100, filePath);
                    Paper = str_temp.ToString();

                    GetPrivateProfileString(txtname, "width", string.Empty, str_temp, 100, filePath);
                    width = Convert.ToInt16(str_temp.ToString());

                    GetPrivateProfileString(txtname, "height", string.Empty, str_temp, 100, filePath);
                    height = Convert.ToInt16(str_temp.ToString());

                    GetPrivateProfileString(txtname, "landscape", string.Empty, str_temp, 100, filePath);
                    landscape = Convert.ToBoolean(str_temp.ToString());

                    GetPrivateProfileString(txtname, "pagename", string.Empty, str_temp, 100, filePath);
                    name = str_temp.ToString();
                }
            }
            catch (Exception ex)
            {
                HelperMessageBoxContent.ShowMessageOK("配置信息有错误");
                HelperLog.Write(ex);
                return;
            }
            DataTable dtInfo = HelperPrint.GetPrintCount(txtname, dt);
            var resultStr = InvokePrint(dtInfo, Isprintview,
                  txtname,
                  printname, txtname,
                  height, width,
                  Paper, name, landscape, isPrintSeal, isWorkorderImage, isPrintLogo, reportInfo, bitmaps);
            if (!Isprintview)//判断是否是打印
            {
                bool b = RecordPrintCount(txtname, dt);
                if (!b)
                {
                    HelperMessageBoxContent.ShowMessageOK("添加打印次数记录失败！");
                }
            }
            if (resultStr != "success")
            {
                HelperMessageBoxContent.ShowMessageOK(resultStr);
            }
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="txtname"></param>
        /// <param name="dt"></param>
        /// <param name="landscape"></param>
        /// <param name="isPrintSeal"></param>
        /// <param name="Isprintview"></param>
        /// <param name="isWorkorderImage"></param>
        /// <param name="isPrintLogo"></param>
        /// <param name="reportInfo"></param>
        public static void PrintOrPreviewWorkOrderDev(string txtname, string paperName, DataTable dt, bool? landscape, bool isPrintSeal,
            bool Isprintview = true, bool isWorkorderImage = false, bool isPrintLogo = false, Sys_ReportManager reportInfo = null)
        {
            string path = string.Empty;
            string printname = string.Empty;
            string Paper = string.Empty;
            string name = string.Empty;
            int width = 0;
            int height = 0;
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add(nameof(Sys_PrintConfiguration.SPCConfigFileName), txtname);
                param.Add(nameof(Sys_PrintConfiguration.SPCUserLoginID), PersistenceGlobalData.CurrentUser.Id);
                var entity = SQL.GetEntityByDictionary(typeof(Sys_PrintConfiguration), param) as Sys_PrintConfiguration;
                if (entity != null)
                {
                    path = entity.SPCConfigFilePath;
                    printname = entity.SPCPrintName;
                    Paper = entity.SPCPrintPaperType;
                    name = string.IsNullOrEmpty(entity.SPCPaperName)? paperName: entity.SPCPaperName;
                    width = entity.SPCPaperWidth.ToString().ToInt();
                    height = entity.SPCPaperHeight.ToString().ToInt();
                    landscape = landscape ?? entity.SPCIsLandscape.ToString().ToBoolean();
                }
                else
                {
                    StringBuilder str_temp = new StringBuilder(500);
                    string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PrintFilePath);
                    GetPrivateProfileString(txtname, "xmlname", string.Empty, str_temp, 200, filePath);
                    path = str_temp.ToString();

                    GetPrivateProfileString(txtname, "printname", string.Empty, str_temp, 100, filePath);
                    printname = str_temp.ToString();

                    GetPrivateProfileString(txtname, "paper", string.Empty, str_temp, 100, filePath);
                    Paper = str_temp.ToString();

                    GetPrivateProfileString(txtname, "width", string.Empty, str_temp, 100, filePath);
                    width = Convert.ToInt16(str_temp.ToString());

                    GetPrivateProfileString(txtname, "height", string.Empty, str_temp, 100, filePath);
                    height = Convert.ToInt16(str_temp.ToString());

                    GetPrivateProfileString(txtname, "landscape", string.Empty, str_temp, 100, filePath);
                    landscape = landscape ?? Convert.ToBoolean(str_temp.ToString());

                    GetPrivateProfileString(txtname, "pagename", string.Empty, str_temp, 100, filePath);
                    name = string.IsNullOrEmpty(paperName) ? str_temp.ToString() : paperName;
                }
            }
            catch (Exception ex)
            {
                HelperMessageBoxContent.ShowMessageOK("配置信息有错误");
                HelperLog.Write(ex);
                return;
            }
            DataTable dtInfo = HelperPrint.GetPrintCount(txtname, dt);
            var resultStr = InvokePrint(dtInfo, Isprintview, path,
                  name,
                  printname, name,
                  height, width,
                  Paper, name, landscape ?? false, isPrintSeal, isWorkorderImage, isPrintLogo, reportInfo);
            if (!Isprintview)//判断是否是打印
            {
                bool b = RecordPrintCount(txtname, dt);
                if (!b)
                {
                    HelperMessageBoxContent.ShowMessageOK("添加打印次数记录失败！");
                }
            }
            if (resultStr != "success")
            {
                HelperMessageBoxContent.ShowMessageOK(resultStr);
            }
        }

        /// <summary>
        /// 打印标签
        /// </summary>
        /// <param name="name">printini节点名称</param>
        /// <param name="dt">数据源</param>
        /// <param name="image">产品图片</param>
        /// <param name="preview">是否预览</param>
        public static void printLable(string name, DataTable dt, Image image, bool preview)
        {
            try
            {
                StringBuilder str_temp = new StringBuilder(500);
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PrintFilePath);
                GetPrivateProfileString(name, "printname", string.Empty, str_temp, 100, filePath);
                string printname = str_temp.ToString();

                GetPrivateProfileString(name, "paper", string.Empty, str_temp, 100, filePath);
                string paper = str_temp.ToString();

                GetPrivateProfileString(name, "width", string.Empty, str_temp, 100, filePath);
                int width = Convert.ToInt32(str_temp.ToString());

                GetPrivateProfileString(name, "height", string.Empty, str_temp, 100, filePath);
                int height = Convert.ToInt32(str_temp.ToString());

                GetPrivateProfileString(name, "landscape", string.Empty, str_temp, 100, filePath);
                bool landscape = Convert.ToBoolean(str_temp.ToString());

                GetPrivateProfileString(name, "pagename", string.Empty, str_temp, 100, filePath);
                string pagename = str_temp.ToString();
                string prefixPath = AppDomain.CurrentDomain.BaseDirectory + @"PrintModel\ReportS_304.repx";
                XtraReport newReport = XtraReport.FromFile(prefixPath, true);
                if (newReport == null)
                {
                    return;
                }
                newReport.Name = name;
                newReport.PageHeight = height;
                newReport.PageWidth = width;
                newReport.PaperKind =
                    (System.Drawing.Printing.PaperKind)
                        Enum.Parse(typeof(System.Drawing.Printing.PaperKind), paper);
                newReport.PaperName = pagename;
                newReport.PrinterName = printname;
                newReport.Landscape = landscape;
                newReport.DataSource = dt;
                if (newReport.Bands != null && newReport.Bands.Count > 0)
                {
                    if (newReport.Bands[0] is DetailBand)
                    {
                        DetailBand detail = newReport.Bands[0] as DetailBand;
                        foreach (var box in detail.Band)
                        {
                            if (box is XRPictureBox)
                            {
                                XRPictureBox picture = box as XRPictureBox;
                                picture.Image = image;
                                break;
                            }
                        }
                    }
                }
                var tool = new ReportPrintTool(newReport);

                if (preview)
                {
                    tool.ShowPreview();
                }
                else
                {
                    #region 记录打印日志
                    HelperLog.AddPrintLog(prefixPath,
                        PersistenceGlobalData.CurrentUser.Name, string.Empty, string.Empty, dt);
                    #endregion
                    tool.Print();
                }
            }
            catch (Exception error)
            {
                HelperLog.Write(error);
            }
        }
        /// <summary>
        /// 打印工令图纸
        /// </summary>
        /// <param name="txtname"></param>
        /// <param name="dt"></param>
        /// <param name="pre"></param>
        /// <param name="landscape"></param>
        /// <param name="isPrintSeal"></param>
        /// <param name="Isprintview"></param>
        /// <param name="isWorkorderImage"></param>
        /// <param name="isPrintLogo"></param>
        public static void PrintOrPrevie(string txtname, DataTable dt, bool pre, bool landscape, bool isPrintSeal, bool Isprintview = true, bool isWorkorderImage = false, bool isPrintLogo = false)
        {
            string path = string.Empty;
            string printname = string.Empty;
            string Paper = string.Empty;
            string name = string.Empty;
            int width = 0;
            int height = 0;
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add(nameof(Sys_PrintConfiguration.SPCConfigFileName), txtname);
                param.Add(nameof(Sys_PrintConfiguration.SPCUserLoginID), PersistenceGlobalData.CurrentUser.Id);
                var entity = SQL.GetEntityByDictionary(typeof(Sys_PrintConfiguration), param) as Sys_PrintConfiguration;
                if (entity != null)
                {
                    path = entity.SPCConfigFilePath;
                    printname = entity.SPCPrintName;
                    Paper = entity.SPCPrintPaperType;
                    name = entity.SPCPaperName;
                    width = entity.SPCPaperWidth.ToString().ToInt();
                    height = entity.SPCPaperHeight.ToString().ToInt();
                    landscape = entity.SPCIsLandscape;
                }
                else
                {
                    StringBuilder str_temp = new StringBuilder(500);
                    string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PrintFilePath);
                    GetPrivateProfileString(txtname, "xmlname", string.Empty, str_temp, 200, filePath);
                    path = str_temp.ToString();

                    GetPrivateProfileString(txtname, "printname", string.Empty, str_temp, 100, filePath);
                    printname = str_temp.ToString();

                    GetPrivateProfileString(txtname, "paper", string.Empty, str_temp, 100, filePath);
                    Paper = str_temp.ToString();

                    GetPrivateProfileString(txtname, "width", string.Empty, str_temp, 100, filePath);
                    width = Convert.ToInt16(str_temp.ToString());

                    GetPrivateProfileString(txtname, "height", string.Empty, str_temp, 100, filePath);
                    height = Convert.ToInt16(str_temp.ToString());

                    GetPrivateProfileString(txtname, "landscape", string.Empty, str_temp, 100, filePath);
                    landscape = Convert.ToBoolean(str_temp.ToString());

                    GetPrivateProfileString(txtname, "pagename", string.Empty, str_temp, 100, filePath);
                    name = str_temp.ToString();
                }
            }
            catch (Exception)
            {
                HelperMessageBoxContent.ShowMessageOK("配置信息有错误2");
                return;
            }
            DataTable dtInfo = HelperPrint.GetPrintCount(txtname, dt);
            var resultStr = InvokePrintPic(dtInfo, Isprintview, path,
                  txtname,
                  printname, txtname,
                  height, width,
                   Paper, name, landscape);
            if (!Isprintview)//判断是否是打印
            {
                bool b = RecordPrintCount(txtname, dt);
                if (!b)
                {
                    HelperMessageBoxContent.ShowMessageOK("添加打印次数记录失败！");
                }
            }

            if (resultStr != "success")
            {
                HelperMessageBoxContent.ShowMessageOK(resultStr);
            }
        }

        /// <summary>
        /// 成品采购打印图纸使用方法
        /// </summary>
        /// <param name="txtname"></param>
        /// <param name="filePath"></param>
        /// <param name="landscape"></param>
        /// <param name="Isprintview"></param>
        public static void PrintOrPrevie(string txtname, string printName, string filePath, bool landscape, bool Isprintview = true)
        {
            string path = string.Empty;
            string printname = string.Empty;
            string Paper = string.Empty;
            string name = string.Empty;
            int width = 0;
            int height = 0;
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add(nameof(Sys_PrintConfiguration.SPCConfigFileName), txtname);
                param.Add(nameof(Sys_PrintConfiguration.SPCUserLoginID), PersistenceGlobalData.CurrentUser.Id);
                var entity = SQL.GetEntityByDictionary(typeof(Sys_PrintConfiguration), param) as Sys_PrintConfiguration;
                if (entity != null)
                {
                    path = entity.SPCConfigFilePath;
                    printname = entity.SPCPrintName;
                    Paper = entity.SPCPrintPaperType;
                    name = entity.SPCPaperName;
                    width = entity.SPCPaperWidth.ToString().ToInt();
                    height = entity.SPCPaperHeight.ToString().ToInt();
                    landscape = entity.SPCIsLandscape;
                }
                else
                {
                    StringBuilder str_temp = new StringBuilder(500);
                    string file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PrintFilePath);
                    GetPrivateProfileString(txtname, "xmlname", string.Empty, str_temp, 200, file);
                    path = str_temp.ToString();

                    GetPrivateProfileString(printName, "printname", string.Empty, str_temp, 100, file);//打印机名称
                    printname = str_temp.ToString();

                    GetPrivateProfileString(txtname, "paper", string.Empty, str_temp, 100, file);
                    Paper = str_temp.ToString();

                    GetPrivateProfileString(txtname, "width", string.Empty, str_temp, 100, file);
                    width = Convert.ToInt16(str_temp.ToString());

                    GetPrivateProfileString(txtname, "height", string.Empty, str_temp, 100, file);
                    height = Convert.ToInt16(str_temp.ToString());

                    GetPrivateProfileString(txtname, "landscape", string.Empty, str_temp, 100, file);
                    landscape = Convert.ToBoolean(str_temp.ToString());

                    GetPrivateProfileString(txtname, "pagename", string.Empty, str_temp, 100, file);
                    name = str_temp.ToString();
                }
            }
            catch (Exception)
            {
                HelperMessageBoxContent.ShowMessageOK("配置信息有错误2");
                return;
            }
            var resultStr = PrintProductDrawings(filePath, Isprintview, path,
                  txtname,
                  printname, txtname,
                  height, width,
                   Paper, name, landscape);
            if (resultStr != "success")
            {
                HelperMessageBoxContent.ShowMessageOK(resultStr);
            }
        }

        /// <summary>
        /// 
        /// 调用打印
        /// </summary> 
        /// <param name="dt">数据集合</param>
        /// <param name="type">是否预览true?预览:直接打印</param> 
        /// <param name="filePath">文件保存路径</param>
        /// <param name="configFileName">打印配置名称</param>
        /// <param name="printName">打印机名称</param>
        /// <param name="paperName">纸张名称</param>
        /// <param name="paperHeight">纸张高度</param>
        /// <param name="paperWidth">纸张宽度</param>
        /// <param name="printPaperType">纸张类型</param>
        /// <param name="printPaperUser">纸张使用者</param>
        /// <param name="isWorkOrderImage">是否来自工令图</param>
        public static string InvokePrint(DataTable dt, bool type, string filePath, string configFileName, string printName, string paperName, float paperHeight, float paperWidth, string printPaperType, string printPaperUser, bool flag, bool isPrintSeal, bool isWorkOrderImage = false, bool isPrintLogo = false, Sys_ReportManager report = null)
        {
            if (report == null)
            {
                //var fileInfo = new FileInfo(filePath);
                if (!File.Exists(filePath))
                {
                    return "模板文件不存在";
                }
            }
            //不去检测打印机是否可用，此方法比较耗性能 by arison 20190907
            //if (!CheckPrintMachineActive(printName))
            //{
            //    //将"该打印机无法正常打印，请选择其他打印机进行打印"改为"配置文件错误！" 2019.5.21
            //    return "配置文件错误！";
            //}
            var modelSrc = string.Empty;
            var csid = string.Empty;
            var cstype = string.Empty;
            var newReport = new XtraReport();
            if (report != null)
            {
                modelSrc = report.SRGName;
                csid = report.SRGId;
                cstype = report.SRGCsType;
                byte[] sqlfile = (Byte[])report.SRGSqlfile;
                System.IO.MemoryStream ms = new MemoryStream(sqlfile);
                newReport.LoadLayout(ms);
            }
            else
            {
                modelSrc = filePath;
                newReport = XtraReport.FromFile(filePath, true);
            }
            try
            {
                /*值从数据库获取*/
                newReport.Name = configFileName;
                newReport.PageHeight = Convert.ToInt32(paperHeight.ToString());
                newReport.PageWidth = Convert.ToInt32(paperWidth.ToString());
                newReport.PaperKind =
                    (System.Drawing.Printing.PaperKind)
                        Enum.Parse(typeof(System.Drawing.Printing.PaperKind), printPaperType);
                newReport.PaperName = printPaperUser;
                newReport.PrinterName = printName;
                newReport.Landscape = flag;
                newReport.DataSource = dt;
                if (isWorkOrderImage)
                {
                    if (newReport.Bands?.Count > 0)
                    {
                        //工令图纸打印的背景图设置
                        if (newReport.Bands[0] is TopMarginBand)
                        {
                            TopMarginBand top = newReport.Bands[0] as TopMarginBand;
                            if (top?.Controls?.Count > 0)
                            {
                                foreach (var box in top.Band)
                                {
                                    if (box is XRTable)
                                    {
                                        XRTable table = box as XRTable;
                                        foreach (XRTableRow row in table.Rows)
                                        {
                                            foreach (XRTableCell cell in row.Cells)
                                            {
                                                foreach (var final in cell)
                                                {
                                                    if (final is XRPictureBox)
                                                    {
                                                        XRPictureBox picture = final as XRPictureBox;

                                                        if (!string.IsNullOrEmpty(dt.Rows[0]["图纸路径"].ToString()))
                                                        {
                                                            Image pic =
                                                                Image.FromStream(
                                                                    WebRequest.Create(dt.Rows[0]["图纸路径"].ToString())
                                                                        .GetResponse()
                                                                        .GetResponseStream());
                                                            picture.Image = new Bitmap(pic, table.Width, table.Height);
                                                        }
                                                        else
                                                        {
                                                            var globalInfo = SQL.GetEntity<Sys_GlobalDict>(Sys_GlobalDict.SGDCode_ColumnName, DataDictionaryModuleIndex.ImgUploadPath);
                                                            //HelperLog.Write("globalInfo=globalInfo" + (globalInfo == null).ToString());
                                                            if (globalInfo.SGDBinaryContent != null && globalInfo.SGDBinaryContent.Length > 0)
                                                            {
                                                                using (MemoryStream buf = new MemoryStream(globalInfo.SGDBinaryContent))
                                                                {
                                                                    Image pic = Image.FromStream(buf, true);
                                                                    picture.Image = new Bitmap(pic, table.Width, table.Height);
                                                                }
                                                            }
                                                        }
                                                        goto skip;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    skip:;
                    }

                }
                if (isPrintLogo)
                {
                    HelperLog.Write("isPrintLogo" + isPrintLogo.ToString());
                    TopMarginBand top = newReport.Bands[BandKind.TopMargin] as TopMarginBand;
                    GroupFooterBand gfooter = newReport.Bands[BandKind.GroupFooter] as GroupFooterBand;
                    BottomMarginBand bottom = newReport.Bands[BandKind.BottomMargin] as BottomMarginBand;
                    ReportFooterBand reportbottom = newReport.Bands[BandKind.ReportFooter] as ReportFooterBand;
                    PageHeaderBand page = newReport.Bands[BandKind.PageHeader] as PageHeaderBand;

                    bool exists = false;
                    if (top?.Controls?.Count > 0)
                    {
                        foreach (var control in top.Controls)
                        {
                            if (control is XRPictureBox)
                            {
                                XRPictureBox picture = control as XRPictureBox;
                                var picSgdCode = string.Empty;
                                if (picture.Name == "picCompany")
                                {
                                    picSgdCode = DataDictionaryModuleIndex.CompanyPic;
                                }
                                if (picSgdCode != string.Empty)
                                {
                                    var companySeal = PersistenceGlobalData.GetSystemDictByCode(DataDictionaryModuleIndex.CompanyPic);
                                    //HelperLog.Write("companySeal1=companySeal1" + (companySeal == null).ToString());
                                    //公司印章
                                    if (companySeal.SGDBinaryContent != null)
                                    {
                                        byte[] bytes = (byte[])companySeal.SGDBinaryContent;
                                        if (bytes != null && bytes.Length > 0)
                                        {
                                            using (MemoryStream buf = new MemoryStream(companySeal.SGDBinaryContent))
                                            {
                                                exists = true;
                                                Image pic = Image.FromStream(buf, true);
                                                picture.Image = new Bitmap(pic);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (gfooter?.Controls?.Count > 0)
                    {
                        foreach (var control in gfooter.Controls)
                        {
                            if (control is XRPictureBox)
                            {
                                XRPictureBox picture = control as XRPictureBox;
                                var picSgdCode = string.Empty;
                                if (picture.Name == "pictureBox1")
                                {
                                    //如果已审核展示盖章图片
                                    if (dt?.Rows.Count > 0 && dt.Columns.Contains("SOMStatus"))
                                    {
                                        picture.Visible = (AuditStatusTypes.已审核.ToString().Equals(dt.Rows[0]["SOMStatus"].ToString()) || AuditStatusTypes.已结案.ToString().Equals(dt.Rows[0]["SOMStatus"].ToString()));
                                    }
                                    if (dt?.Rows.Count > 0 && dt.Columns.Contains("SQMStatus"))
                                    {
                                        picture.Visible = (AuditStatusTypes.已审核.ToString().Equals(dt.Rows[0]["SQMStatus"].ToString()) || AuditStatusTypes.已结案.ToString().Equals(dt.Rows[0]["SQMStatus"].ToString()));
                                    }
                                }
                            }
                        }
                    }
                    if (bottom?.Controls?.Count > 0)
                    {
                        foreach (var control in bottom.Controls)
                        {
                            if (control is XRPictureBox)
                            {
                                XRPictureBox picture = control as XRPictureBox;
                                var picSgdCode = string.Empty;
                                if (picture.Name == "pictureBox1")
                                {
                                    //如果已审核展示盖章图片
                                    if (dt?.Rows.Count > 0 && dt.Columns.Contains("SOMStatus"))
                                    {
                                        picture.Visible = (AuditStatusTypes.已审核.ToString().Equals(dt.Rows[0]["SOMStatus"].ToString()) || AuditStatusTypes.已结案.ToString().Equals(dt.Rows[0]["SOMStatus"].ToString()));
                                    }
                                    if (dt?.Rows.Count > 0 && dt.Columns.Contains("SQMStatus"))
                                    {
                                        picture.Visible = (AuditStatusTypes.已审核.ToString().Equals(dt.Rows[0]["SQMStatus"].ToString()) || AuditStatusTypes.已结案.ToString().Equals(dt.Rows[0]["SQMStatus"].ToString()));
                                    }
                                }
                            }
                        }
                    }
                    if (reportbottom?.Controls?.Count > 0)
                    {
                        foreach (var control in reportbottom.Controls)
                        {
                            if (control is XRPictureBox)
                            {
                                XRPictureBox picture = control as XRPictureBox;
                                var picSgdCode = string.Empty;
                                if (picture.Name == "pictureBox1")
                                {
                                    //如果已审核展示盖章图片
                                    if (dt?.Rows.Count > 0 && dt.Columns.Contains("SOMStatus"))
                                    {
                                        picture.Visible = (AuditStatusTypes.已审核.ToString().Equals(dt.Rows[0]["SOMStatus"].ToString())|| AuditStatusTypes.已结案.ToString().Equals(dt.Rows[0]["SOMStatus"].ToString()));
                                    }
                                    if (dt?.Rows.Count > 0 && dt.Columns.Contains("SQMStatus"))
                                    {
                                        picture.Visible = (AuditStatusTypes.已审核.ToString().Equals(dt.Rows[0]["SQMStatus"].ToString())|| AuditStatusTypes.已结案.ToString().Equals(dt.Rows[0]["SQMStatus"].ToString()));
                                    }
                                }
                            }
                        }
                    }

                    if (!exists)
                    {
                        HelperLog.Write("!exists" + exists.ToString());
                        if (page?.Controls?.Count > 0)
                        {
                            foreach (var control in page.Controls)
                            {
                                if (control is XRPictureBox)
                                {
                                    XRPictureBox picture = control as XRPictureBox;
                                    var picSgdCode = string.Empty;
                                    if (picture.Name == "picCompany")
                                    {
                                        picSgdCode = DataDictionaryModuleIndex.CompanyPic;
                                    }
                                    if (picSgdCode != string.Empty)
                                    {
                                        var companySeal = PersistenceGlobalData.GetSystemDictByCode(DataDictionaryModuleIndex.CompanyPic);
                                        //HelperLog.Write("companySeal2=companySeal2" + (companySeal == null).ToString());
                                        //公司印章
                                        if (companySeal.SGDBinaryContent != null)
                                        {
                                            byte[] bytes = (byte[])companySeal.SGDBinaryContent;
                                            if (bytes != null && bytes.Length > 0)
                                            {
                                                using (MemoryStream buf = new MemoryStream(companySeal.SGDBinaryContent))
                                                {
                                                    Image pic = Image.FromStream(buf, true);
                                                    picture.Image = new Bitmap(pic);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (isPrintSeal)
                {
                    //HelperLog.Write("isPrintSeal" + isPrintSeal.ToString());
                    PageFooterBand footer = newReport.Bands[BandKind.PageFooter] as PageFooterBand;
                    if (footer?.Controls?.Count > 0)
                    {
                        foreach (var control in footer.Controls)
                        {
                            if (control is XRPictureBox)
                            {
                                XRPictureBox picture = control as XRPictureBox;
                                if (picture.Name == "pictureBox2")
                                {
                                    var companySeal = SQL.GetEntity<Sys_GlobalDict>(Sys_GlobalDict.SGDCode_ColumnName, DataDictionaryModuleIndex.CompanySeal);
                                    //公司印章
                                    if (companySeal.SGDBinaryContent != null)
                                    {
                                        byte[] bytes = (byte[])companySeal.SGDBinaryContent;
                                        if (bytes != null && bytes.Length > 0)
                                        {
                                            using (MemoryStream buf = new MemoryStream(companySeal.SGDBinaryContent))
                                            {
                                                Image pic = Image.FromStream(buf, true);
                                                picture.Image = new Bitmap(pic);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                ChangeLabelMark(newReport);
            }
            catch (Exception ex)
            {
                HelperLog.Write(ex.ToString() + @"\r\n" + ex.StackTrace);
            }


            newReport.RequestParameters = false;
            var tool = new ReportPrintTool(newReport);
            try
            {
                if (type)
                {
                    tool.AutoShowParametersPanel = false;
                    tool.ShowPreview();
                }
                else
                {
                    #region 记录打印日志
                    HelperLog.AddPrintLog(modelSrc,
                        PersistenceGlobalData.CurrentUser.Name, csid, cstype, dt);
                    #endregion
                    tool.Print();
                }
            }
            catch (Exception error)
            {
                //有的电脑，如果不选择打印输入对象，将可能报错，因此此处如果报错，则只记录，不弹出异常阻止用户操作 by arison!
                HelperLog.Write(error);
            }
            return "success";

        }

        /// <summary>
        /// 
        /// 调用打印
        /// </summary> 
        /// <param name="dt">数据集合</param>
        /// <param name="type">是否预览true?预览:直接打印</param> 
        /// <param name="filePath">文件保存路径</param>
        /// <param name="configFileName">打印配置名称</param>
        /// <param name="printName">打印机名称</param>
        /// <param name="paperName">纸张名称</param>
        /// <param name="paperHeight">纸张高度</param>
        /// <param name="paperWidth">纸张宽度</param>
        /// <param name="printPaperType">纸张类型</param>
        /// <param name="printPaperUser">纸张使用者</param>
        /// <param name="isWorkOrderImage">是否来自工令图</param>
        /// <param name="isPrintLogo">是否打印logo</param>
        /// <param name="isPrintSeal">是否打印印章</param>
        /// <param name="landscape">是否横向打印</param>
        /// <param name="report">模板配置</param>
        /// <param name="bitmaps">图片</param>
        public static string InvokePrint(DataTable dt, bool type, string configFileName,
            string printName, string paperName, float paperHeight, float paperWidth,
            string printPaperType, string printPaperUser, bool landscape, bool isPrintSeal, bool isWorkOrderImage = false, bool isPrintLogo = false, Sys_ReportManager report = null, List<Bitmap> bitmaps = null)
        {
            if (report == null)
            {
                return "模板文件不存在";

            }

            var newReport = new XtraReport();

            var modelSrc = report.SRGName;
            var csid = report.SRGId;
            var cstype = report.SRGCsType;
            byte[] sqlfile = (Byte[])report.SRGSqlfile;
            using (System.IO.MemoryStream ms = new MemoryStream(sqlfile))
            {
                newReport.LoadLayout(ms);
            }

            XtraReport picNewReport = GetPictureReport(report, paperName, printName,landscape, bitmaps);

            try
            {
                /*值从数据库获取*/
                newReport.Name = configFileName;
                newReport.PageHeight = Convert.ToInt32(paperHeight.ToString());
                newReport.PageWidth = Convert.ToInt32(paperWidth.ToString());
                newReport.PaperKind =
                    (System.Drawing.Printing.PaperKind)
                        Enum.Parse(typeof(System.Drawing.Printing.PaperKind), printPaperType);
                newReport.PaperName = printPaperUser;
                newReport.PrinterName = printName;
                newReport.Landscape = landscape;
                newReport.DataSource = dt;
                if (isWorkOrderImage)
                {
                    if (newReport.Bands?.Count > 0)
                    {
                        //工令图纸打印的背景图设置
                        if (newReport.Bands[0] is TopMarginBand)
                        {
                            TopMarginBand top = newReport.Bands[0] as TopMarginBand;
                            if (top?.Controls?.Count > 0)
                            {
                                foreach (var box in top.Band)
                                {
                                    if (box is XRTable)
                                    {
                                        XRTable table = box as XRTable;
                                        foreach (XRTableRow row in table.Rows)
                                        {
                                            foreach (XRTableCell cell in row.Cells)
                                            {
                                                foreach (var final in cell)
                                                {
                                                    if (final is XRPictureBox)
                                                    {
                                                        XRPictureBox picture = final as XRPictureBox;

                                                        if (!string.IsNullOrEmpty(dt.Rows[0]["图纸路径"].ToString()))
                                                        {
                                                            Image pic =
                                                                Image.FromStream(
                                                                    WebRequest.Create(dt.Rows[0]["图纸路径"].ToString())
                                                                        .GetResponse()
                                                                        .GetResponseStream());
                                                            picture.Image = new Bitmap(pic, table.Width, table.Height);
                                                        }
                                                        else
                                                        {
                                                            var globalInfo = SQL.GetEntity<Sys_GlobalDict>(Sys_GlobalDict.SGDCode_ColumnName, DataDictionaryModuleIndex.ImgUploadPath);
                                                            //HelperLog.Write("globalInfo=globalInfo" + (globalInfo == null).ToString());
                                                            if (globalInfo.SGDBinaryContent != null && globalInfo.SGDBinaryContent.Length > 0)
                                                            {
                                                                using (MemoryStream buf = new MemoryStream(globalInfo.SGDBinaryContent))
                                                                {
                                                                    Image pic = Image.FromStream(buf, true);
                                                                    picture.Image = new Bitmap(pic, table.Width, table.Height);
                                                                }
                                                            }
                                                        }
                                                        goto skip;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    skip:;
                    }

                }
                if (isPrintLogo)
                {
                    HelperLog.Write("isPrintLogo" + isPrintLogo.ToString());
                    TopMarginBand top = newReport.Bands[BandKind.TopMargin] as TopMarginBand;
                    GroupFooterBand gfooter = newReport.Bands[BandKind.GroupFooter] as GroupFooterBand;
                    BottomMarginBand bottom = newReport.Bands[BandKind.BottomMargin] as BottomMarginBand;
                    ReportFooterBand reportbottom = newReport.Bands[BandKind.ReportFooter] as ReportFooterBand;
                    PageHeaderBand page = newReport.Bands[BandKind.PageHeader] as PageHeaderBand;

                    bool exists = false;
                    if (top?.Controls?.Count > 0)
                    {
                        foreach (var control in top.Controls)
                        {
                            if (control is XRPictureBox)
                            {
                                XRPictureBox picture = control as XRPictureBox;
                                var picSgdCode = string.Empty;
                                if (picture.Name == "picCompany")
                                {
                                    picSgdCode = DataDictionaryModuleIndex.CompanyPic;
                                }
                                if (picSgdCode != string.Empty)
                                {
                                    var companySeal = PersistenceGlobalData.GetSystemDictByCode(DataDictionaryModuleIndex.CompanyPic);
                                    //HelperLog.Write("companySeal1=companySeal1" + (companySeal == null).ToString());
                                    //公司印章
                                    if (companySeal.SGDBinaryContent != null)
                                    {
                                        byte[] bytes = (byte[])companySeal.SGDBinaryContent;
                                        if (bytes != null && bytes.Length > 0)
                                        {
                                            using (MemoryStream buf = new MemoryStream(companySeal.SGDBinaryContent))
                                            {
                                                exists = true;
                                                Image pic = Image.FromStream(buf, true);
                                                picture.Image = new Bitmap(pic);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (gfooter?.Controls?.Count > 0)
                    {
                        foreach (var control in gfooter.Controls)
                        {
                            if (control is XRPictureBox)
                            {
                                XRPictureBox picture = control as XRPictureBox;
                                var picSgdCode = string.Empty;
                                if (picture.Name == "pictureBox1")
                                {
                                    //如果已审核展示盖章图片
                                    if (dt?.Rows.Count > 0 && dt.Columns.Contains("SOMStatus"))
                                    {
                                        picture.Visible = (AuditStatusTypes.已审核.ToString().Equals(dt.Rows[0]["SOMStatus"].ToString()) || AuditStatusTypes.已结案.ToString().Equals(dt.Rows[0]["SOMStatus"].ToString()));
                                    }
                                    if (dt?.Rows.Count > 0 && dt.Columns.Contains("SQMStatus"))
                                    {
                                        picture.Visible = (AuditStatusTypes.已审核.ToString().Equals(dt.Rows[0]["SQMStatus"].ToString()) || AuditStatusTypes.已结案.ToString().Equals(dt.Rows[0]["SQMStatus"].ToString()));
                                    }
                                }
                            }
                        }
                    }
                    if (bottom?.Controls?.Count > 0)
                    {
                        foreach (var control in bottom.Controls)
                        {
                            if (control is XRPictureBox)
                            {
                                XRPictureBox picture = control as XRPictureBox;
                                var picSgdCode = string.Empty;
                                if (picture.Name == "pictureBox1")
                                {
                                    //如果已审核展示盖章图片
                                    if (dt?.Rows.Count > 0 && dt.Columns.Contains("SOMStatus"))
                                    {
                                        picture.Visible = (AuditStatusTypes.已审核.ToString().Equals(dt.Rows[0]["SOMStatus"].ToString()) || AuditStatusTypes.已结案.ToString().Equals(dt.Rows[0]["SOMStatus"].ToString()));
                                    }
                                    if (dt?.Rows.Count > 0 && dt.Columns.Contains("SQMStatus"))
                                    {
                                        picture.Visible = (AuditStatusTypes.已审核.ToString().Equals(dt.Rows[0]["SQMStatus"].ToString()) || AuditStatusTypes.已结案.ToString().Equals(dt.Rows[0]["SQMStatus"].ToString()));
                                    }
                                }
                            }
                        }
                    }
                    if (reportbottom?.Controls?.Count > 0)
                    {
                        foreach (var control in reportbottom.Controls)
                        {
                            if (control is XRPictureBox)
                            {
                                XRPictureBox picture = control as XRPictureBox;
                                var picSgdCode = string.Empty;
                                if (picture.Name == "pictureBox1")
                                {
                                    //如果已审核展示盖章图片
                                    if (dt?.Rows.Count > 0 && dt.Columns.Contains("SOMStatus"))
                                    {
                                        picture.Visible = (AuditStatusTypes.已审核.ToString().Equals(dt.Rows[0]["SOMStatus"].ToString()) || AuditStatusTypes.已结案.ToString().Equals(dt.Rows[0]["SOMStatus"].ToString()));
                                    }
                                    if (dt?.Rows.Count > 0 && dt.Columns.Contains("SQMStatus"))
                                    {
                                        picture.Visible = (AuditStatusTypes.已审核.ToString().Equals(dt.Rows[0]["SQMStatus"].ToString()) || AuditStatusTypes.已结案.ToString().Equals(dt.Rows[0]["SQMStatus"].ToString()));
                                    }
                                }
                            }
                        }
                    }

                    if (!exists)
                    {
                        HelperLog.Write("!exists" + exists.ToString());
                        if (page?.Controls?.Count > 0)
                        {
                            foreach (var control in page.Controls)
                            {
                                if (control is XRPictureBox)
                                {
                                    XRPictureBox picture = control as XRPictureBox;
                                    var picSgdCode = string.Empty;
                                    if (picture.Name == "picCompany")
                                    {
                                        picSgdCode = DataDictionaryModuleIndex.CompanyPic;
                                    }
                                    if (picSgdCode != string.Empty)
                                    {
                                        var companySeal = PersistenceGlobalData.GetSystemDictByCode(DataDictionaryModuleIndex.CompanyPic);
                                        //HelperLog.Write("companySeal2=companySeal2" + (companySeal == null).ToString());
                                        //公司印章
                                        if (companySeal.SGDBinaryContent != null)
                                        {
                                            byte[] bytes = (byte[])companySeal.SGDBinaryContent;
                                            if (bytes != null && bytes.Length > 0)
                                            {
                                                using (MemoryStream buf = new MemoryStream(companySeal.SGDBinaryContent))
                                                {
                                                    Image pic = Image.FromStream(buf, true);
                                                    picture.Image = new Bitmap(pic);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (isPrintSeal)
                {
                    //HelperLog.Write("isPrintSeal" + isPrintSeal.ToString());
                    PageFooterBand footer = newReport.Bands[BandKind.PageFooter] as PageFooterBand;
                    if (footer?.Controls?.Count > 0)
                    {
                        foreach (var control in footer.Controls)
                        {
                            if (control is XRPictureBox)
                            {
                                XRPictureBox picture = control as XRPictureBox;
                                if (picture.Name == "pictureBox2")
                                {
                                    var companySeal = SQL.GetEntity<Sys_GlobalDict>(Sys_GlobalDict.SGDCode_ColumnName, DataDictionaryModuleIndex.CompanySeal);
                                    //公司印章
                                    if (companySeal.SGDBinaryContent != null)
                                    {
                                        byte[] bytes = (byte[])companySeal.SGDBinaryContent;
                                        if (bytes != null && bytes.Length > 0)
                                        {
                                            using (MemoryStream buf = new MemoryStream(companySeal.SGDBinaryContent))
                                            {
                                                Image pic = Image.FromStream(buf, true);
                                                picture.Image = new Bitmap(pic);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                ChangeLabelMark(newReport);
            }
            catch (Exception ex)
            {
                HelperLog.Write(ex.ToString() + @"\r\n" + ex.StackTrace);
            }
            if (picNewReport != null)
            {
                newReport.CreateDocument();
                picNewReport.CreateDocument();
                foreach (DevExpress.XtraPrinting.Page page in picNewReport.Pages)
                {
                    newReport.Pages.Add(page);
                }
                newReport.PrintingSystem.ContinuousPageNumbering = true;
            }



            newReport.RequestParameters = false;
            var tool = new ReportPrintTool(newReport);
            try
            {
                if (type)
                {
                    tool.AutoShowParametersPanel = false;
                    tool.ShowPreview();
                }
                else
                {
                    #region 记录打印日志
                    HelperLog.AddPrintLog(modelSrc,
                        PersistenceGlobalData.CurrentUser.Name, csid, cstype, dt);
                    #endregion
                    tool.Print();
                }
            }
            catch (Exception error)
            {
                //有的电脑，如果不选择打印输入对象，将可能报错，因此此处如果报错，则只记录，不弹出异常阻止用户操作 by arison!
                HelperLog.Write(error);
            }
            return "success";

        }

        /// <summary>
        /// 获取图片模板
        /// </summary>
        /// <param name="report">打印模板</param>
        /// <param name="pagename">名称</param>
        /// <param name="printname">打印机名称</param>
        /// <param name="landscape">是否横向打印</param>
        /// <param name="bitmaps">图片列表</param>
        /// <returns></returns>
        private static XtraReport GetPictureReport(Sys_ReportManager report, string pagename, string printname, bool landscape, List<Bitmap> bitmaps)
        {
            if (bitmaps == null || !bitmaps.Any()) return null;
            var picNewReport = new XtraReport();
            //byte[] picFile = (Byte[])picReport.SRGSqlfile;
            //using (System.IO.MemoryStream ms = new MemoryStream(picFile))
            //{
            //    picNewReport.LoadLayout(ms);
            //}

            //if (picNewReport == null)
            //{
            //    return null;
            //}
            picNewReport.Name = report.SRGName;
            picNewReport.PageHeight = (int)report.SRGHigh.ToDecimal();
            picNewReport.PageWidth = (int)report.SRGWidth.ToDecimal();
            picNewReport.PaperKind =
                (System.Drawing.Printing.PaperKind)
                    Enum.Parse(typeof(System.Drawing.Printing.PaperKind), report.SRGPaperType);
            picNewReport.PaperName = pagename;
            picNewReport.PrinterName = printname;
            picNewReport.Landscape = landscape;
            picNewReport.Margins = new Margins(0, 0, 0, 0); 
            var details = new DetailBand();
            var y = 0;
            foreach(var image in bitmaps)
            {
                XRPictureBox box = new XRPictureBox();
                box.Image = image;
                box.Sizing = DevExpress.XtraPrinting.ImageSizeMode.Squeeze;
                box.SizeF = new System.Drawing.Size(picNewReport.PageWidth, picNewReport.PageHeight);
                box.LocationF = new PointF(0, y);
                y += picNewReport.PageHeight;
                details.Controls.Add(box);
            }
            picNewReport.Bands.Add(details);


            return picNewReport;
        }

        #endregion

        /// <summary>
        /// 打印工令图纸
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="type"></param>
        /// <param name="filePath"></param>
        /// <param name="configFileName"></param>
        /// <param name="printName"></param>
        /// <param name="paperName"></param>
        /// <param name="paperHeight"></param>
        /// <param name="paperWidth"></param>
        /// <param name="printPaperType"></param>
        /// <param name="printPaperUser"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static string InvokePrintPic(DataTable dt, bool type, string filePath, string configFileName,
            string printName, string paperName, float paperHeight, float paperWidth, string printPaperType
            , string printPaperUser, bool flag, Sys_ReportManager report = null)
        {
            if (report == null)
            {
                if (!File.Exists(filePath))
                {
                    return "模板文件不存在";
                }
            }
            var newReport = new XtraReport();
            var modelSrc = string.Empty;
            var csid = string.Empty;
            var cstype = string.Empty;
            if (report != null)
            {
                modelSrc = report.SRGName;
                csid = report.SRGId;
                cstype = report.SRGCsType;
                byte[] sqlfile = (Byte[])report.SRGSqlfile;
                System.IO.MemoryStream ms = new MemoryStream(sqlfile);
                newReport.LoadLayout(ms);
            }
            else
            {
                modelSrc = filePath;
                newReport = XtraReport.FromFile(filePath, true);
            }
            /*值从数据库获取*/
            newReport.Name = configFileName;
            newReport.PageHeight = Convert.ToInt32(paperHeight.ToString());
            newReport.PageWidth = Convert.ToInt32(paperWidth.ToString());
            newReport.PaperKind =
                (System.Drawing.Printing.PaperKind)
                    Enum.Parse(typeof(System.Drawing.Printing.PaperKind), printPaperType);
            newReport.PaperName = printPaperUser;
            newReport.PrinterName = printName;
            newReport.Landscape = flag;
            newReport.DataSource = dt;
            ReportHeaderBand footer = newReport.Bands[BandKind.ReportHeader] as ReportHeaderBand;
            if (footer?.Controls?.Count > 0)
            {
                foreach (var control in footer.Controls)
                {
                    if (control is XRPictureBox)
                    {
                        XRPictureBox picture = control as XRPictureBox;
                        if (picture.Name == "pictureBox1")
                        {
                            if (dt.Columns.Contains("图纸路径") && !string.IsNullOrEmpty(dt.Rows[0]["图纸路径"].ToString()))
                            {
                                MemoryStream ms = HelperFTP.DownloadFtp(dt.Rows[0]["图纸路径"].ToString());
                                if (ms != null)
                                {
                                    Image pic = Image.FromStream(ms);
                                    picture.Image = new Bitmap(pic);
                                }

                            }
                            else if (dt.Columns.Contains("产品图纸") && !string.IsNullOrEmpty(dt.Rows[0]["产品图纸"].ToString()))
                            {
                                MemoryStream ms = HelperFTP.DownloadFtp(dt.Rows[0]["产品图纸"].ToString());
                                if (ms != null)
                                {
                                    Image pic = Image.FromStream(ms);
                                    picture.Image = new Bitmap(pic);
                                }

                            }
                        }
                        var picSgdCode = string.Empty;
                        if (picture.Name == "picCompany")
                        {
                            picSgdCode = DataDictionaryModuleIndex.CompanyPic;
                        }
                        if (picSgdCode != string.Empty)
                        {
                            var companySeal = SQL.GetEntity<Sys_GlobalDict>(Sys_GlobalDict.SGDCode_ColumnName, picSgdCode);
                            //公司印章
                            if (companySeal.SGDBinaryContent != null)
                            {
                                byte[] bytes = (byte[])companySeal.SGDBinaryContent;
                                if (bytes != null && bytes.Length > 0)
                                {
                                    using (MemoryStream buf = new MemoryStream(companySeal.SGDBinaryContent))
                                    {
                                        Image pic = Image.FromStream(buf, true);
                                        picture.Image = new Bitmap(pic);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            ChangeLabelMark(newReport);
            newReport.RequestParameters = false;
            var tool = new ReportPrintTool(newReport);

            if (type)
            {
                tool.AutoShowParametersPanel = false;
                tool.ShowPreview();
            }
            else
            {
                #region 记录打印日志
                HelperLog.AddPrintLog(modelSrc + "-" + printName,
                    PersistenceGlobalData.CurrentUser.Name, csid, cstype, dt);
                #endregion
                tool.Print();
            }
            return "success";

        }

        /// <summary>
        /// 打印工令图纸
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="type"></param>
        /// <param name="filePath"></param>
        /// <param name="configFileName"></param>
        /// <param name="printName"></param>
        /// <param name="paperName"></param>
        /// <param name="paperHeight"></param>
        /// <param name="paperWidth"></param>
        /// <param name="printPaperType"></param>
        /// <param name="printPaperUser"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static string PrintProductDrawings(string imgfilePath, bool type, string filePath, string configFileName,
            string printName, string paperName, float paperHeight, float paperWidth,
            string printPaperType, string printPaperUser, bool flag)
        {
            var fileInfo = new FileInfo(filePath);
            if (!fileInfo.Exists)
            {
                return "模板文件不存在";
            }
            var newReport = XtraReport.FromFile(filePath, true);
            /*值从数据库获取*/
            newReport.Name = configFileName;
            newReport.PageHeight = Convert.ToInt32(paperHeight.ToString());
            newReport.PageWidth = Convert.ToInt32(paperWidth.ToString());
            newReport.PaperKind =
                (System.Drawing.Printing.PaperKind)
                    Enum.Parse(typeof(System.Drawing.Printing.PaperKind), printPaperType);
            newReport.PaperName = printPaperUser;
            newReport.PrinterName = printName;
            newReport.Landscape = flag;
            ReportHeaderBand footer = newReport.Bands[BandKind.ReportHeader] as ReportHeaderBand;
            if (footer?.Controls?.Count > 0)
            {
                foreach (var control in footer.Controls)
                {
                    if (control is XRPictureBox)
                    {
                        XRPictureBox picture = control as XRPictureBox;
                        if (picture.Name == "pictureBox1")
                        {
                            MemoryStream ms = HelperFTP.DownloadFtp(imgfilePath);
                            if (ms != null)
                            {
                                Image pic = Image.FromStream(ms);
                                picture.Image = new Bitmap(pic);
                            }
                        }
                    }
                }
            }
            ChangeLabelMark(newReport);
            var tool = new ReportPrintTool(newReport);
            if (type)
            {
                tool.ShowPreview();
            }
            else
            {
                #region 记录打印日志
                HelperLog.AddPrintLog(filePath,
                    PersistenceGlobalData.CurrentUser.Name, "", "", null);
                #endregion
                tool.Print();
            }
            return "success";

        }


        /// <summary>
        /// 打印机是否存在是否可打印
        /// </summary>
        /// <param name="printMachine"></param>
        /// <returns></returns>
        public static bool CheckPrintMachineActive(string printMachine)
        {
            try
            {
                var scope = new ManagementScope(@"\root\cimv2");
                scope.Connect();

                // Select Printers from WMI Object Collections
                var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Printer");

                var printerName = "";
                foreach (var o in searcher.Get())
                {
                    var printer = (ManagementObject)o;
                    printerName = printer["Name"].ToString().ToLower();
                    if (printerName.IndexOf(printMachine.ToLower(), System.StringComparison.Ordinal) > -1)
                    {

                        if (printer["WorkOffline"].ToString().ToLower().Equals("true"))
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HelperLog.Write(ex);
            }

            return false;
        }

        /// <summary>
        /// 记录打印次数
        /// </summary>
        /// <param name="sourceName">打印模板名称</param>
        /// <param name="dt">打印模板所需填充的DataTable</param>
        /// <returns>true添加成功，false添加失败</returns>
        public static bool RecordPrintCount(string sourceName, DataTable dt)
        {
            string sourceSerial = string.Empty;
            switch (sourceName)
            {
                case "客户对账单":
                    {
                        break;
                    }
                case "客户发票回执单":
                    {
                        break;
                    }
                case "供应商对账单":
                    {
                        break;
                    }
                case "供应商发票回执单":
                    {
                        break;
                    }
                case "流程卡打印":
                    {
                        sourceSerial = dt.Rows[0]["订单编号"].ToString();
                        break;
                    }
                case "外加工出库明细打印":
                    {
                        sourceSerial = dt.Rows[0]["外加工出库流水号"].ToString();
                        break;
                    }
                case "成品采购单":
                    {
                        sourceSerial = dt.Rows[0]["biID"].ToString();
                        break;
                    }
                case "成品采购退单":
                    {
                        sourceSerial = dt.Rows[0]["biID"].ToString();
                        break;
                    }
                case "采购询价单":
                    {
                        sourceSerial = dt.Rows[0]["biID"].ToString();
                        break;
                    }
                case "原料采购单":
                    {
                        sourceSerial = dt.Rows[0]["biID"].ToString();
                        break;
                    }
                case "原料采购退单":
                    {
                        sourceSerial = dt.Rows[0]["biID"].ToString();
                        break;
                    }
                case "模具采购单":
                    {
                        sourceSerial = dt.Rows[0]["biID"].ToString();
                        break;
                    }
                case "模具采购退单":
                    {
                        sourceSerial = dt.Rows[0]["biID"].ToString();
                        break;
                    }
                case "质检单打印":
                    {
                        sourceSerial = dt.Rows[0]["PITSerial"].ToString();
                        break;
                    }
                case "质保书打印":
                    {
                        sourceSerial = dt.Rows[0]["检验单号"].ToString();
                        break;
                    }
                case "订单合同打印":
                    {
                        sourceSerial = dt.Rows[0]["订单主表流水号"].ToString();
                        break;
                    }
                case "销售报价单":
                    {
                        sourceSerial = dt.Rows[0]["SQMSerial"].ToString();
                        break;
                    }
                case "销售退货打印":
                    {
                        sourceSerial = dt.Rows[0]["SPTSerial"].ToString();
                        break;
                    }
                case "出货通知单打印":
                    {
                        sourceSerial = dt.Rows[0]["SPTSerial"].ToString();
                        break;
                    }
                case "送货通知单打印含单价":
                    {
                        sourceSerial = dt.Rows[0]["SPTSerial"].ToString();
                        break;
                    }
                case "送货通知单打印不含单价":
                    {
                        sourceSerial = dt.Rows[0]["SPTSerial"].ToString();
                        break;
                    }
                case "印记工令单打印":
                    {
                        sourceSerial = dt.Rows[0]["工令编号"].ToString();
                        break;
                    }
                case "工令单打印":
                    {
                        sourceSerial = dt.Rows[0]["工令编号"].ToString();
                        break;
                    }
                case "工令图纸打印":
                    {
                        sourceSerial = dt.Rows[0]["工令流水号"].ToString();
                        break;

                    }
                case "外加工明细产品图纸打印":
                    {
                        sourceSerial = dt.Rows[0]["外加工流水号"].ToString();
                        break;
                    }
                case "备货单打印":
                    {
                        sourceSerial = dt.Rows[0]["SOMSerial"].ToString();
                        break;
                    }
                case "成品盘点打印":
                    {
                        sourceSerial = dt.Rows[0]["WPCSerial"].ToString();
                        break;
                    }
                case "送货通知单":
                    {
                        sourceSerial = dt.Rows[0]["SPTSerial"].ToString();
                        break;
                    }
                case "包装单":
                    {
                        sourceSerial = dt.Rows[0]["PackingSerial"].ToString();
                        break;
                    }
            }

            string sql = "insert into Bas_PrintCountRecord (PCRId,PCRSourceName,PCRSourceSerial,PCRPrintName,PCRPrintTime) values ('" + HelperGUID.GetGuid().ToString() + "','" + sourceName + "','" + sourceSerial + "','" + PersistenceGlobalData.CurrentUser.Name + "','" + SQL.GetTime() + "')";
            return SQL.ExecuteNonQueryReturnBool(sql);
        }

        /// <summary>
        /// 根据打印模板名称获取打印次数
        /// </summary>
        /// <param name="sourceName">模板名称</param>
        /// <param name="sourceName">打印模板所需填充的DataTable</param>
        /// <returns>返回带有打印次数的DataTable</returns>
        public static DataTable GetPrintCount(string sourceName, DataTable dt)
        {
            string sourceSerial = string.Empty;
            switch (sourceName)
            {
                case "客户对账单":
                    {
                        break;
                    }
                case "客户发票回执单":
                    {
                        break;
                    }
                case "供应商对账单":
                    {
                        break;
                    }
                case "供应商发票回执单":
                    {
                        break;
                    }
                case "库位标签打印":
                    {
                        break;
                    }
                case "流程卡打印":
                    {
                        sourceSerial = dt.Rows[0]["订单编号"].ToString();
                        break;
                    }
                case "外加工出库明细打印":
                    {
                        sourceSerial = dt.Rows[0]["EPSSerial"].ToString();
                        break;
                    }
                case "成品采购单":
                    {
                        sourceSerial = dt.Rows[0]["biID"].ToString();
                        break;
                    }
                case "成品采购退单":
                    {
                        sourceSerial = dt.Rows[0]["biID"].ToString();
                        break;
                    }
                case "采购询价单":
                    {
                        sourceSerial = dt.Rows[0]["biID"].ToString();
                        break;
                    }
                case "原料采购单":
                    {
                        sourceSerial = dt.Rows[0]["BBMSerial"].ToString();
                        break;
                    }
                case "原料采购退单":
                    {
                        sourceSerial = dt.Rows[0]["BBMSerial"].ToString();
                        break;
                    }
                case "模具采购单":
                    {
                        sourceSerial = dt.Rows[0]["biID"].ToString();
                        break;
                    }
                case "模具采购退单":
                    {
                        sourceSerial = dt.Rows[0]["biID"].ToString();
                        break;
                    }
                case "质检单打印":
                    {
                        sourceSerial = dt.Rows[0]["PITSerial"].ToString();
                        break;
                    }
                case "质保书打印":
                    {
                        sourceSerial = dt.Rows[0]["检验单号"].ToString();
                        break;
                    }
                case "订单合同打印":
                    {
                        sourceSerial = dt.Rows[0]["订单主表流水号"].ToString();
                        break;
                    }
                case "销售报价单":
                    {
                        sourceSerial = dt.Rows[0]["SQMSerial"].ToString();
                        break;
                    }
                case "销售退货打印":
                    {
                        sourceSerial = dt.Rows[0]["SPTSerial"].ToString();
                        break;
                    }
                case "出货通知单打印":
                    {
                        sourceSerial = dt.Rows[0]["SPTSerial"].ToString();
                        break;
                    }
                case "送货通知单打印含单价":
                    {
                        sourceSerial = dt.Rows[0]["SPTSerial"].ToString();
                        break;
                    }
                case "送货通知单打印不含单价":
                    {
                        sourceSerial = dt.Rows[0]["SPTSerial"].ToString();
                        break;
                    }
                case "印记工令单打印":
                    {
                        sourceSerial = dt.Rows[0]["工令编号"].ToString();
                        break;
                    }
                case "工令单打印":
                    {
                        sourceSerial = dt.Rows[0]["工令编号"].ToString();
                        break;
                    }
                case "工令图纸打印":
                    {
                        sourceSerial = dt.Rows[0]["工令流水号"].ToString();
                        break;

                    }
                case "外加工明细产品图纸打印":
                    {
                        sourceSerial = dt.Rows[0]["外加工流水号"].ToString();
                        break;
                    }
                case "备货单打印":
                    {
                        sourceSerial = dt.Rows[0]["SOMSerial"].ToString();
                        break;
                    }
                case "成品盘点单":
                    {
                        sourceSerial = dt.Rows[0]["WPCSerial"].ToString();
                        break;
                    }
                case "质检包装单打印":
                    {
                        break;
                    }
                case "包装单":
                    {
                        sourceSerial = dt.Rows[0]["PackingSerial"].ToString();
                        break;
                    }
            }

            DataTable dtInfo = new DataTable();
            dtInfo.Columns.Add("打印次数");
            string sql = "select count(*) from Bas_PrintCountRecord where PCRSourceName='" + sourceName + "' and PCRSourceSerial='" + sourceSerial + "' ";
            int printCont = SQL.ExecuteScalar(sql).ToString().ToInt();
            DataRow dr = dtInfo.NewRow();
            dr["打印次数"] = printCont + 1;
            dtInfo.Rows.Add(dr);
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    DataRow dr = dtInfo.NewRow();
            //    dr["打印次数"] = printCont;
            //    dtInfo.Rows.Add(dr);
            //}
            return HelperDataTable.UniteDataTable(dtInfo, dt);
        }



        /// <summary>
        /// 改变标签条码格式
        /// </summary>
        /// <param name="newReport">传入整个打印对象</param>
        public static void ChangeLabelMark(XtraReport newReport)
        {

            if (!string.IsNullOrEmpty(DataDictionaryModuleIndex.IsShowCode128))
            {
                var isShowCode128 = SQL.GetEntity<Sys_GlobalDict>(Sys_GlobalDict.SGDCode_ColumnName, DataDictionaryModuleIndex.IsShowCode128);

                if (isShowCode128 != null)
                {
                    var dicValue = isShowCode128.SGDDictValue;
                    if (!string.IsNullOrEmpty(dicValue) && dicValue == "1")
                    {
                        TopMarginBand topMargin = newReport.Bands[BandKind.TopMargin] as TopMarginBand;
                        searchChangeBarCode(topMargin);
                        PageHeaderBand pageHeader = newReport.Bands[BandKind.PageHeader] as PageHeaderBand;
                        searchChangeBarCode(pageHeader);
                        ReportHeaderBand reportHeader = newReport.Bands[BandKind.ReportHeader] as ReportHeaderBand;
                        searchChangeBarCode(reportHeader);
                        GroupHeaderBand groupHeader = newReport.Bands[BandKind.GroupHeader] as GroupHeaderBand;
                        searchChangeBarCode(groupHeader);
                        DetailBand detail = newReport.Bands[BandKind.Detail] as DetailBand;
                        searchChangeBarCode(detail);
                        DetailReportBand detailReport = newReport.Bands[BandKind.DetailReport] as DetailReportBand;
                        searchChangeBarCode(detailReport);
                        GroupFooterBand groupFooter = newReport.Bands[BandKind.GroupFooter] as GroupFooterBand;
                        searchChangeBarCode(groupFooter);
                        ReportFooterBand reportFooter = newReport.Bands[BandKind.ReportFooter] as ReportFooterBand;
                        searchChangeBarCode(reportFooter);
                        PageFooterBand pageFooter = newReport.Bands[BandKind.PageFooter] as PageFooterBand;
                        searchChangeBarCode(pageFooter);
                        BottomMarginBand bottomMargin = newReport.Bands[BandKind.BottomMargin] as BottomMarginBand;
                        searchChangeBarCode(bottomMargin);
                    }
                }

            }
        }

        /// <summary>
        /// 遍历页面控件下的子控件，找到条码控件改成一维码格式
        /// </summary>
        /// <param name="tempControl"></param>
        public static void searchChangeBarCode(XRControl tempControl)
        {
            if (tempControl != null && tempControl.Controls != null && tempControl.Controls.Count > 0)
            {
                foreach (var control in tempControl.Controls)
                {
                    if (control is XRBarCode)
                    {
                        /// <summary>
                        /// 条形码格式
                        /// </summary>
                        DevExpress.XtraPrinting.BarCode.Code128Generator code128Generator1 = new DevExpress.XtraPrinting.BarCode.Code128Generator();
                        XRBarCode barcode = control as XRBarCode;
                        barcode.Symbology = code128Generator1;

                    }
                }
            }
        }

        /// <summary>
        /// 标签模板打印
        /// </summary>
        ///  <param name="printName"></param>打印机名称
        /// <param name="printnodeName"></param>标签名称
        /// <param name="type"></param>是否预览标签
        /// <param name="customerOrsupplierid"></param>客户或供应商ID
        /// <param name="modelclass"></param>标签大类以维护在字典中跟据模块大类对应
        /// <param name="dtSourse"></param>标签中的数据源
        public static void printReport(string printName, string printnodeName, bool type, string customerOrsupplierid, string modelclass, DataTable dtSourse)
        {
            DataTable dtid; string sql;
            //判断标签名称 客户或供应商标签大类是否为空，若果没有直接带出该大类下的默认模板
            if (!string.IsNullOrEmpty(printnodeName) && !string.IsNullOrEmpty(customerOrsupplierid) && !string.IsNullOrEmpty(modelclass))
            {
                sql = "select SRGSqlfile,SRGName,SRGCsId,SRGCsType from Sys_ReportManager  where SRGName= '" + printnodeName + "' and SRGCsId='" + customerOrsupplierid + "' and SRGClass='" + modelclass + "' and SRGState='启用' ";
                //string sql = "select " + Sys_ReportManager.SRGSqlfile_ColumnName + " from Sys_ReportManager where "
                //+ Sys_ReportManager.SRGName_ColumnName + "=" + printnodeName + "'" + " and "
                //+ Sys_ReportManager.SRGId_ColumnName + "=" + customerOrsupplierid + " and "
                //+ Sys_ReportManager.SRGClass_ColumnName + "=" + modelclass + " and "
                //+ Sys_ReportManager.SRGState_ColumnName + "='启用'";

            }
            else
            {
                sql = "select SRGSqlfile,SRGName,SRGCsId,SRGCsType from Sys_ReportManager  where SRGClass='" + modelclass + "' and SRGDefaultReport=1 and SRGState='启用' ";
            }
            dtid = SQL.GetDataTable(sql);
            if (dtid.Rows.Count > 0)
            {
                var srgname = dtid.Rows[0]["SRGName"].ToString();//模板名称
                var cstype = dtid.Rows[0]["SRGCsType"].ToString(); //客户/供应商类型
                var csid = dtid.Rows[0]["SRGCsId"].ToString(); //客户/供应商编号
                //二进制流读取转换   
                //XtraReport sqlrerport = new XtraReport();
                byte[] sf = (Byte[])dtid.Rows[0]["SRGSqlfile"];
                System.IO.MemoryStream ms = new MemoryStream(sf);
                //sqlrerport.LoadLayout(ms);
                //ReportMainForm form = new ReportMainForm();
                XtraReport newReport = XtraReport.FromStream(ms, true);
                //var printnodeName = e.Item.Caption;
                try
                {
                    //避免操作系统环境中出现问题，系统自动退出异常问题 20190226 by arison!
                    if (!string.IsNullOrEmpty(printnodeName))
                    {
                        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PrintFilePath);
                        printName = HelperIniFile.GetPrivateProfileString(printnodeName, "printname", filePath); //读取配置文件
                    }
                }
                catch (Exception err)
                {
                    HelperLog.Write(err);
                    return;
                }
                if (string.IsNullOrEmpty(printName))
                {
                    HelperMessageBoxContent.ShowMessageOK("打印配置节点不存在，请先配置打印信息！");
                }
                else
                {
                    newReport.PrinterName = printName;
                    //2019.5.22修改打印配置中选择的打印机无法正常打印，请选择其他打印机进行打印 改为 配置文件错误！
                    if (!HelperPrint.CheckPrintMachineActive(printName))
                    {
                        HelperMessageBoxContent.ShowMessageOK("配置文件错误！");
                    }
                    else
                    {
                        //var dtTable = GetDataSource();
                        newReport.DataSource = dtSourse;
                        ReportPrintTool reportPrintTool = new ReportPrintTool(newReport);
                        if (type)
                        {
                            reportPrintTool.ShowPreview();
                        }
                        else
                        {
                            #region 记录打印日志
                            HelperLog.AddPrintLog(srgname,
                                PersistenceGlobalData.CurrentUser.Name, csid, cstype, dtSourse);
                            #endregion
                            reportPrintTool.Print();
                        }
                    }
                }
            }
            else
            {
                HelperMessageBoxContent.ShowMessageOK("请先维护模板!");
            }
        }
    }
}
