using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Collections;
using DevExpress.XtraReports.UI;
using DevExpress.XtraEditors;
using System.Security.Cryptography;
using System.Drawing;
using System.Net;

namespace FX.MainForms
{
    public abstract class PublicClass
    {
        public static string warehouselist, defaultw;//仓库集合，默认仓库

        /// <summary>
        /// 设置gridview样式
        /// </summary>
        /// <param name="gridView1"></param>
        /// <param name="editcol">可编辑列名称</param>
        public static void SetGridDefaultStyle(DevExpress.XtraGrid.Views.Grid.GridView gridView1, bool showFindPanel = true)
        {
            if (showFindPanel)
                gridView1.ShowFindPanel();
            gridView1.OptionsView.ShowFooter = true;        //显示footer行
            gridView1.OptionsView.ColumnAutoWidth = false;  //列宽不自动，如果超过则出现滚动条
            gridView1.OptionsBehavior.Editable = true;     //设置单元格不可编辑
            gridView1.OptionsView.ShowGroupPanel = false;   //去掉表格上方“Drag a column header ……”
            //gridView1.IndicatorWidth = 45;                //设置显示行号的列宽
            gridView1.Columns[0].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count;
            gridView1.OptionsView.ShowAutoFilterRow = false;
        }

        #region 与快递对接
        public static string UID = string.Empty, UName = string.Empty, LoginName = string.Empty, UFLAG = string.Empty;// UType = "", UTel = "", UJob = "";//用户ID,Name和用户类型，电话

        public static string Tel = "0532-88080518"; //电话
        public static string Fax = string.Empty; //传真
        public static string WareHouseAddress = "山东省青岛市即墨区通济街道邢家岭"; //仓库地址
        public static string ApiUrl = string.Empty; //快递鸟url接口服务地址
        public static string ZTO = string.Empty;    //中通打印机
        public static string UC = string.Empty;     //优速打印机
        public static string YTO = string.Empty;    //圆通打印机
        public static string HTKY = string.Empty;    //百世打印机

        //中通账号密码
        public static string ztoCustomerName = "1000400870";
        public static string ztoCustomerPwd = "4AEBE7XZ2Y";

        //优速账号密码
        public static string uceCustomerName = "80730178";
        public static string uceCustomerPwd = "8c2c97f1-9781-4181-a596-6d792c938fe7";

        //圆通账号密码
        public static string ytoCustomerName = "K512138891";
        public static string ytoCustomerPwd = "5TeTZQ4J";

        //百世账号密码
        public static string bestCustomerName = "2152490010";
        public static string bestCustomerPwd = "B5bTv2HlWk7o";

        //顺丰账号密码
        public static string sfCustomerName = "15726227877"; //这里账号密码 暂时不知道
        public static string sfCustomerPwd = "Zpd08880888";//这里账号密码 暂时不知道
        public static string monthCode = "5322124799"; //顺丰月结账号

        //德邦
        public static string dbCustomerName = "15726227877";
        public static string dbCustomerPwd = " zpd0888";
        #endregion


        /// <summary>
        /// 调用打印
        /// </summary>
        /// <param name="rpt">对应的Report模版</param>
        /// <param name="dt">数据集合</param>
        /// <param name="type">是否预览true?预览:直接打印</param>
        /// <param name="filename">文件名</param>
        public static void InvokePrint(DataTable dt, bool type, string filename, string printnodeName, Image img = null)
        {
            XtraReport newReport = XtraReport.FromFile(filename, true);
            //  newReport.PrinterName = HelperIniFile.GetPrivateProfileString("LabelPrintConfig", "PrintName", @".\public.ini"); //读取配置文件
            var printName = string.Empty;
            try
            {
                //避免操作系统环境中出现问题，系统自动退出异常问题 20190226 by arison!
                if (!string.IsNullOrEmpty(printnodeName))
                {
                    var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "print.ini");
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
                XtraMessageBox.Show("打印配置节点不存在，请先配置打印信息！");
            }
            else
            {
                newReport.PrinterName = printName;

                if (!HelperPrint.CheckPrintMachineActive(printName))
                {
                    //2019.5.22 打印配置中选择的打印机无法正常打印，请选择其他打印机进行打印 改为 配置文件错误！
                    XtraMessageBox.Show("配置文件错误！");
                }
                else
                {
                    newReport.DataSource = dt;
                    if (img != null)
                    {
                        if (newReport.Bands != null && newReport.Bands.Count > 0)
                        {
                            //工令图纸打印的背景图设置
                            if (newReport.Bands[0] is DetailBand)
                            {
                                DetailBand detail = newReport.Bands[0] as DetailBand;
                                foreach (var box in detail.Band)
                                {
                                    if (box is XRPictureBox)
                                    {
                                        XRPictureBox picture = box as XRPictureBox;
                                        picture.Image = img;
                                        goto skip;
                                    }
                                }
                            }
                        skip: ;
                        }
                    }
                    ReportPrintTool reportPrintTool = new ReportPrintTool(newReport);
                    if (type)
                    {
                        reportPrintTool.ShowPreview();
                    }
                    else
                    {
                        #region 记录打印日志
                        HelperLog.AddPrintLog(filename,
                            PersistenceGlobalData.CurrentUser.Name, "", "", dt);
                        #endregion
                        reportPrintTool.Print();
                    }
                }
            }

        }


        /// <summary>
        /// 修改App.Config的值
        /// </summary>
        /// <param name="path">全路径</param>
        /// <param name="key">config key值</param>
        /// <param name="value">修改后的value</param>
        public static void SetConfigValue(string path, string key, string value)
        {
            XmlDocument xDoc = new XmlDocument();
            XmlNode xNode; XmlElement xElem1; XmlElement xElem2;
            xDoc.Load(path);
            xNode = xDoc.SelectSingleNode("//appSettings");
            xElem1 = (XmlElement)xNode.SelectSingleNode("//add[@key='" + key + "']");
            if (xElem1 != null) xElem1.SetAttribute("value", value);
            else
            {
                xElem2 = xDoc.CreateElement("add");
                xElem2.SetAttribute("key", key);
                xElem2.SetAttribute("value", value);
                xNode.AppendChild(xElem2);
            }
            xDoc.Save(path);
        }

        /// <summary>
        /// 根据key获取value值
        /// </summary>
        /// <param name="path">config全路径</param>
        /// <param name="appKey">config key值</param>
        /// <returns>value</returns>
        public static string GetConfigValue(string path, string appKey)
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlElement xmlElement = null;
            try
            {
                xmlDocument.Load(path);
                XmlNode xmlNode = xmlDocument.SelectSingleNode("//appSettings");
                xmlElement = (XmlElement)xmlNode.SelectSingleNode("//add[@key=\"" + appKey + "\"]");
            }
            catch (XmlException ex)
            {
                XtraMessageBox.Show(ex.Message, "提示");
            }
            string result;
            if (xmlElement != null) result = xmlElement.GetAttribute("value");
            else result = "";
            return result;
        }

        /// <summary>
        /// list转换DataTable
        /// </summary>
        /// <param name="_list"></param>
        /// <returns></returns>
        public static DataTable TransDatable(IList _list)
        {
            DataTable dt = new DataTable();
            if (_list != null && _list.Count > 0)
            {
                //通过反射获取list中的字段 
                System.Reflection.PropertyInfo[] p = _list[0].GetType().GetProperties();
                foreach (System.Reflection.PropertyInfo pi in p)
                {
                    dt.Columns.Add(pi.Name, System.Type.GetType(pi.PropertyType.ToString()));
                }
                for (int i = 0; i < _list.Count; i++)
                {
                    IList TempList = new ArrayList();
                    //将IList中的一条记录写入ArrayList
                    foreach (System.Reflection.PropertyInfo pi in p)
                    {
                        object oo = pi.GetValue(_list[i], null);
                        TempList.Add(oo);
                    }
                    object[] itm = new object[p.Length];
                    for (int j = 0; j < TempList.Count; j++)
                    {
                        itm.SetValue(TempList[j], j);
                    }
                    dt.LoadDataRow(itm, true);
                }
            }
            return dt;
        }

        /// <summary>
        /// 图片转换为字节数组
        /// </summary>
        /// <param name="image">图片</param>
        /// <returns>字节数组</returns>
        public static byte[] ImageToBytes(Image image)
        {
            try
            {
                if (image == null) return null;
                using (Bitmap bitmap = new Bitmap(image))
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        bitmap.Save(stream, image.RawFormat);
                        return stream.GetBuffer();
                    }
                }
            }
            finally
            {
                if (image != null)
                {
                    image.Dispose();
                    image = null;
                }
            }
        }

        #region ========加密========
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string Encrypt(string Text)
        {
            return Encrypt(Text, "gpyh_label");
        }
        /// <summary> 
        /// 加密数据 
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string Encrypt(string Text, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray;
            inputByteArray = Encoding.Default.GetBytes(Text);
            des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
                ret.AppendFormat("{0:X2}", b);
            return ret.ToString();
        }
        #endregion

        #region ========解密========
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string Decrypt(string Text)
        {
            return Decrypt(Text, "gpyh_label");
        }
        /// <summary> 
        /// 解密数据 
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string Decrypt(string Text, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            int len;
            len = Text.Length / 2;
            byte[] inputByteArray = new byte[len];
            int x, i;
            for (x = 0; x < len; x++)
            {
                i = Convert.ToInt32(Text.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }
            des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Encoding.Default.GetString(ms.ToArray());
        }
        #endregion
    }
}
