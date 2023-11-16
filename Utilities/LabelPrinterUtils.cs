//-----------------------------------------------------------------------
// <copyright company="工品一号" file="LabelPrinterUtils.cs">
//  Copyright (c)  V1.0.0.0  
//  创建作者:   王健
//  创建时间:   2023/5/16 13:09:10 
//  功能描述:   
//  历史版本:
//          2023/5/16 13:09:10 王健 创建LabelPrinterUtils类
// </copyright>
//-----------------------------------------------------------------------
using DevExpress.XtraReports.UI;
using FX.Entity;
using FX.MainForms.Public.Daos;
using FX.MainForms.Public.Models.ApiDtos;
using ReportUtil.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using XtraReportsDesign;

namespace FX.MainForms
{
    public class LabelPrinterUtils
    {
        /// <summary>
        /// 获取自定义标签字段
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        public static List<string> GetLabelCustomFields(string reportId)
        {
            var report = SQL.GetEntity<Sys_ReportManager>(reportId);
            if (report == null || report.SRGSqlfile == null)
            {
                return new List<string>();
            }
            var list = new List<string>();
            try
            {
                using (MemoryStream ms = new MemoryStream(report.SRGSqlfile))
                {
                    XtraReport newReport = XtraReport.FromStream(ms, true);
                    list = GetLabelCustomFields(newReport);
                }
            }
            catch (Exception e)
            {

            }
            return list;
        }

        /// <summary>
        /// 获取模板自定义字段
        /// </summary>
        /// <param name="control"></param>
        /// <param name="list"></param>
        public static List<string> GetLabelCustomFields(XtraReport report)
        {
            var list = new List<string>();
            foreach (Band band in report.Bands)
            {
                foreach (XRControl item in band.Controls)
                {
                    GetLabelCustomFields(item, list);
                }
            }
            list = list.Distinct().ToList();
            return list;
        }

        /// <summary>
        /// 获取自定义字段
        /// </summary>
        /// <param name="control"></param>
        /// <param name="list"></param>
        private static void GetLabelCustomFields(XRControl control, List<string> list)
        {
            foreach (Match mch in Regex.Matches(control.Text, "\\[CustomField\\d*\\]"))
            {
                list.Add(mch.Value.Replace("[", "").Replace("]", ""));
            }
            foreach (XRControl item in control.Controls)
            {
                GetLabelCustomFields(item, list);
            }
        }

        /// <summary>
        /// 打印标签
        /// </summary>
        /// <param name="printInfo"></param>
        public static void PrintLabels(PushPrintLableDto printInfo, Image image = null)
        {
            var report = SQL.GetEntity<Sys_ReportManager>(printInfo.ReportId);
            if (report == null) return;
            string printName = string.Empty;
            try
            {
                //避免操作系统环境中出现问题，系统自动退出异常问题 20190226 by arison!
                if (!string.IsNullOrEmpty(report.SRGName))
                {
                    var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "print.ini");
                    printName = HelperIniFile.GetPrivateProfileString(report.SRGName, "printname", filePath); //读取配置文件
                }
            }
            catch (Exception err)
            {
                HelperLog.Write(err);
                throw new Exception("读取打印机配置异常！");
            }
            if (string.IsNullOrEmpty(printName))
            {
                throw new Exception("请先配置打印机！");
            }

            //二进制流读取转换   
            //XtraReport sqlrerport = new XtraReport();
            byte[] sf = (Byte[])report.SRGSqlfile;
            if (sf == null)
            {
                throw new Exception("标签模板数据不存在！");
            }
            var srgname = report.SRGName;//模板名称
            var cstype = report.SRGCsType; //客户/供应商类型
            var csid = report.SRGCsId; //客户/供应商编号
            System.IO.MemoryStream ms = new MemoryStream(sf);
            //sqlrerport.LoadLayout(ms);
            //ReportMainForm form = new ReportMainForm();
            XtraReport newReport = XtraReport.FromStream(ms, true);

            newReport.PrinterName = printName;
            if (!HelperPrint.CheckPrintMachineActive(printName))
            {
                throw new Exception("打印机未启动！");
            }
            else
            {
                printInfo.Data.ForEach(x =>
                {
                    x.ProductCNDescription = string.IsNullOrEmpty(x.CPSProductName) ? x.CNDescription : x.CPSProductName;
                    x.ProductName = string.IsNullOrEmpty(x.CPSStandardCode) ? x.Name : x.CPSStandardCode;
                    x.ProductMaterial = string.IsNullOrEmpty(x.CPSMaterial) ? x.Material : x.CPSMaterial;
                    x.ProductSpecification = string.IsNullOrEmpty(x.CPSSpecification) ? x.Specification : x.CPSSpecification;
                    x.ProductStrengthLevel = string.IsNullOrEmpty(x.CPSStrengthLevel) ? x.StrengthLevel : x.CPSStrengthLevel;
                    x.ProductSurfaceMethod = string.IsNullOrEmpty(x.CPSSurfaceMethod) ? x.SurfaceMethod : x.CPSSurfaceMethod;
                });
                var dtTable = printInfo.Data.ListToReportColumnsDataTable();
                if (dtTable == null || dtTable.Rows.Count == 0)
                {
                    throw new Exception("数据未加载请勿尝试打印标签！");
                }
                if (image != null)
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
                                    if (picture.Name == "pictureBox1" || picture.Name == "pictureBox3")
                                    {
                                        picture.Image = image;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                if (printInfo.CustomFields != null && printInfo.CustomFields.Count > 0)
                {
                    foreach (var item in printInfo.CustomFields)
                    {
                        if (!dtTable.Columns.Contains(item.Key)) dtTable.Columns.Add(item.Key);
                    }
                    foreach (System.Data.DataRow row in dtTable.Rows)
                    {
                        foreach (var item in printInfo.CustomFields)
                        {
                            if (dtTable.Columns.Contains(item.Key)) row[item.Key] = item.Value;
                        }
                    }
                }
                newReport.RequestParameters = false;
                newReport.DataSource = dtTable;
                #region 记录打印日志
                HelperLog.AddPrintLog(srgname,
                    PersistenceGlobalData.CurrentUser.Name, csid, cstype, dtTable);
                #endregion
                ReportPrintTool reportPrintTool = new ReportPrintTool(newReport);
                reportPrintTool.Print();
            }
        }
        /// <summary>
        /// 打印标签根据送货单主键
        /// </summary>
        /// <param name="shipmentId"></param>
        public static void PrintLabelsByShipmentId(string shipmentId)
        {
            var entity = SQL.GetEntity<Sel_Shipment>(shipmentId);
            if (entity == null || "已作废".Equals(entity.SPTStatus) || "已删除".Equals(entity.SPTStatus)) return;
            var sql = $@"SELECT SPDId Id,SPDProductId,SPDRequestNumber,SPDOrderSerial SOMSerial,SODOrderNumber,SODCustomerPN
                         FROM Sel_ShipmentDetails SPD WITH(NOLOCK) 
                         INNER JOIN Sel_OrderDetails SOD WITH(NOLOCK) ON SPD.SPDOrderSerial=SODSerial AND SPD.SPDProductID=SODProductId
                         WHERE SPD.SPDSerial = '{entity.SPTSerial}' AND SPDRequestNumber>0";
            var dt = SQL.GetDataTable(sql);
            if (dt == null || dt.Rows.Count <= 0) return;

            PrintLabelInfoDto dto = new PrintLabelInfoDto()
            {
                CustomerCode = entity.SPTCustomerCode,
                SOMCustomerOrderSerial = entity.SPTCustomerOrderSerial,
                SOMSerial = dt.Rows[0]["SOMSerial"].ToString()
            };
            var detail = new List<PrintLabelDetailDto>();
            foreach (DataRow item in dt.Rows)
            {
                detail.Add(new PrintLabelDetailDto()
                {
                    ProductId = item["SPDProductId"].ToString(),
                    CustomerPN = item["SODCustomerPN"].ToString(),
                    Number = item["SPDRequestNumber"].ToString().ToDecimal(),
                    OrderNumber = item["SODOrderNumber"].ToString().ToDecimal(),
                });
            }
            dto.Details = detail;

            if (entity != null)
            {
                try
                {
                    LabelPrinterUtils.PrintLabels(dto);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        /// <summary>
        /// 根据送货单号打印散装标签
        /// </summary>
        /// <param name="strSPTId"></param>
        public static void PrintBulkLabelByShipmentId(string strSPTId)
        {
            var entity = SQL.GetEntity<Sel_Shipment>(strSPTId);
            if (entity == null || "已作废".Equals(entity.SPTStatus) || "已删除".Equals(entity.SPTStatus)) return;
            var customer = SQL.GetEntity<Bas_Customers>(Bas_Customers.BCTCode_ColumnName, entity.SPTCustomerCode);
            if (customer == null) return;
            var sql = $@"SELECT SPDId Id,BPTBarCode BarCode,BPTCode Code,BPTMaterial Material,BPTStrengthLevel StrengthLevel,BPTName Name,BPTCNDescription CNDescription,BPTSurfaceMethod SurfaceMethod,BPTSpecification Specification,BPTToothType ToothType,BPTMainUnit MainUnit,BPTStardandSinglet StardandSinglet,BPTSinglet Singlet,BPTMinPackageUnit1,BPTMinPackageQty1,BPTMinPackageUnit2,BPTMinPackageQty2,BPTMinPackageUnit3,BPTMinPackageQty3,
                         BPTMark Mark,BPTBrandName BrandName,
                         CPSId,CPSCode,CPSProjectCode,CPSProductName,CPSStandardCode,CPSMaterial,CPSSpecification,CPSMinPackageUnit1,CPSMinPackageQty1,CPSMinPackageUnit2,CPSMinPackageQty2,CPSMinPackageUnit3,CPSMinPackageQty3,CPSSurfaceMethod,CPSStrengthLevel,
                         SDLTakeNumber SPDRequestNumber,SPDOrderSerial SOMSerial,SODOrderNumber
                         FROM Sel_ShipmentDetails SPD WITH(NOLOCK) 
                         INNER JOIN Sel_OrderDetails SOD WITH(NOLOCK) ON SPD.SPDOrderSerial=SODSerial AND SPD.SPDProductID=SODProductId
                         INNER JOIN Bas_Products BPT WITH(NOLOCK) ON SPD.SPDProductId =BPT.BPTId 
                         INNER JOIN Sel_DeliveryStockLocation SDL WITH(NOLOCK) ON SPD.SPDId = SDL.SDLDeliveryDetailId
                         LEFT JOIN Bas_CustomerProducts CPS WITH(NOLOCK) ON BPT.BPTId =CPS.CPSProductId AND CPS.CPSCustomerId ='{customer.BCTId}' AND CPS.CPSStatus ='启用'
                         WHERE SPD.SPDSerial = '{entity.SPTSerial}' AND SPDRequestNumber>0;;
                         ";
            sql += $@"IF EXISTS (SELECT TOP 1 SRGId  from Sys_ReportManager  where SRGClass = '标签模板类' and SRGState ='启用' and SRGCsId='{customer.BCTId}' ORDER  BY ISNULL(SRGDefaultReport,0) DESC)
                        SELECT TOP 1 SRGId,SRGName  from Sys_ReportManager  where SRGClass = '标签模板类' and SRGState ='启用' and SRGCsId='{customer.BCTId}' ORDER  BY ISNULL(SRGDefaultReport,0) DESC
                      ELSE 
                        IF EXISTS (SELECT TOP 1 SRGId  from Sys_ReportManager  where SRGClass = '标签模板类' and SRGState ='启用' and SRGName like '%外部通用%' ORDER  BY ISNULL(SRGDefaultReport,0) DESC)
                            SELECT TOP 1 SRGId,SRGName  from Sys_ReportManager  where SRGClass = '标签模板类' and SRGState ='启用' and SRGName like '%外部通用%'  ORDER  BY ISNULL(SRGDefaultReport,0) DESC
                        ELSE
                            SELECT TOP 1 SRGId,SRGName  from Sys_ReportManager  where SRGClass = '标签模板类' and SRGState ='启用'  ORDER  BY ISNULL(SRGDefaultReport,0) DESC
                    ;";
            var ds = SQL.GetDataSet(sql);
            if (ds == null || ds.Tables.Count < 2) return;
            if (ds.Tables[0] == null || ds.Tables[0].Rows.Count <= 0)
            {
                throw new Exception("数据获取异常");
            }
            if (ds.Tables[1] == null || ds.Tables[1].Rows.Count < 0)
            {
                throw new Exception("请维护模板");
            }
            var report = new PushPrintLableDto()
            {
                ReportId = ds.Tables[1].Rows[0]["SRGId"].ToString()
            };
            var time = SQL.GetTime();
            List<PrintLabelColumnDto> list = new List<PrintLabelColumnDto>();
            var order = SQL.GetEntity<Sel_Orders>(Sel_Orders.SOMSerial_ColumnName, ds.Tables[0].Rows[0]["SOMSerial"].ToString());
            DataTable table = ExtendDictionary.GetDictionariesByParentCode(DataDictionaryModuleIndex.PrintFactoryInfo);
            string globalFlagCode = string.Empty;
            var sgdInfo = SQL.GetEntity<Sys_GlobalDict>(Sys_GlobalDict.SGDCode_ColumnName, DataDictionaryModuleIndex.GlobalFlagCode);
            if (sgdInfo != null)
            {
                globalFlagCode = sgdInfo.SGDDictValue;
            }

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                var bPTMinPackageQty1 = row["BPTMinPackageQty1"].ToString().ToDecimal();
                string bPTMinPackageUnit1 = row["BPTMinPackageUnit1"].ToString();

                decimal bPTMinPackageQty2 = row["BPTMinPackageQty2"].ToString().ToDecimal();
                string bPTMinPackageUnit2 = row["BPTMinPackageUnit2"].ToString();
                decimal bPTMinPackageQty3 = row["BPTMinPackageQty3"].ToString().ToDecimal();
                string bPTMinPackageUnit3 = row["BPTMinPackageUnit3"].ToString();
                decimal cPSMinPackageQty1 = row["CPSMinPackageQty1"].ToString().ToDecimal();
                string cPSMinPackageUnit1 = row["CPSMinPackageUnit1"].ToString();
                decimal cPSMinPackageQty2 = row["CPSMinPackageQty2"].ToString().ToDecimal();
                string cPSMinPackageUnit2 = row["CPSMinPackageUnit2"].ToString();
                decimal cPSMinPackageQty3 = row["CPSMinPackageQty3"].ToString().ToDecimal();
                string cPSMinPackageUnit3 = row["CPSMinPackageUnit3"].ToString();
                var sPDRequestNumber = row["SPDRequestNumber"].ToString().ToDecimal();
                var packages = ExtendProducts.GetPackages(bPTMinPackageQty1, bPTMinPackageUnit1, bPTMinPackageQty2, bPTMinPackageUnit2, bPTMinPackageQty3, bPTMinPackageUnit3,
                cPSMinPackageQty1, cPSMinPackageUnit1, cPSMinPackageQty2, cPSMinPackageUnit2, cPSMinPackageQty3, cPSMinPackageUnit3);
                var bulkNumber = sPDRequestNumber;
                if (packages != null && packages.Count > 0)
                {
                    bulkNumber = ExtendProducts.GetBulkPackageNumber(packages, sPDRequestNumber);
                }
                bulkNumber = bulkNumber.ToString("0.###").ToDecimal();
                if (bulkNumber <= 0) continue;
                int labelIndex = 0;
                var obj = new PrintLabelColumnDto()
                {
                    Id = row["Id"].ToString(),
                    BarCode = row["BarCode"].ToString(),
                    Code = row["Code"].ToString(),
                    Material = row["Material"].ToString(),
                    StrengthLevel = row["StrengthLevel"].ToString(),
                    Name = row["Name"].ToString(),
                    Specification = row["Specification"].ToString(),
                    ToothType = row["ToothType"].ToString(),
                    MainUnit = row["MainUnit"].ToString(),
                    StardandSinglet = row["StardandSinglet"].ToString().ToDecimal(),
                    Singlet = row["Singlet"].ToString().ToDecimal(),
                    CNDescription = row["CNDescription"].ToString(),
                    MinPackageUnit1 = (packages != null && packages.Count > 0) ? packages.First().Key : string.Empty,
                    MinPackageQty1 = ((packages != null && packages.Count > 0) ? packages.First().Value : 0),
                    MinPackageUnit2 = (packages != null && packages.Count > 1) ? packages.Skip(1).Take(1).First().Key : string.Empty,
                    MinPackageQty2 = ((packages != null && packages.Count > 1) ? packages.Skip(1).Take(1).First().Value : 0),
                    MinPackageUnit3 = (packages != null && packages.Count > 2) ? packages.Skip(2).Take(1).First().Key : string.Empty,
                    MinPackageQty3 = ((packages != null && packages.Count > 2) ? packages.Skip(2).Take(1).First().Value : 0),
                    Mark = row["Mark"].ToString(),
                    //CPSProductName = string.IsNullOrEmpty(row["CPSProductName"].ToString()) ? row["BrandName"].ToString() : row["CPSProductName"].ToString(),
                    CPSProductName = row["CPSProductName"].ToString(),
                    CPSCode = row["CPSCode"].ToString(),
                    CPSProjectCode = row["CPSProjectCode"].ToString(),
                    SOMCustomerOrderSerial = order?.SOMCustomerOrderSerial,
                    CPSId = row["CPSId"].ToString(),
                    CPSMaterial = row["CPSMaterial"].ToString(),
                    CPSSpecification = row["CPSSpecification"].ToString(),
                    CPSStrengthLevel = row["CPSStrengthLevel"].ToString(),
                    CPSSurfaceMethod = row["CPSSurfaceMethod"].ToString(),
                    //默认情况，批次需要小于ShipDate一天，常识情况 by arison 20230919
                    Batch = time.AddDays(-1).ToString("yyMMdd"),
                    Date = time.ToShortDateString(),

                    SOMSerial = row["SOMSerial"].ToString(),
                    OrderSerial = row["SOMSerial"].ToString(),
                    CustomerOrderSerial = order?.SOMCustomerOrderSerial,
                    PackageNum1 = 0,
                    PackageNum2 = 0,
                    PackageNum3 = 0,
                    ShipDate = time.ToString("yyyy.MM.dd"),
                    SSinglet = row["Singlet"].ToString().ToDecimal(),
                    CompanyName = (table == null ? "" : (table.Select("SGDCode='FactoryName'").FirstOrDefault()?["SGDDictValue"].ToString() ?? string.Empty)),
                    CompanyFullName = (table == null ? "" : (table.Select("SGDCode='CompanyFullName'").FirstOrDefault()?["SGDDictValue"].ToString() ?? string.Empty)),
                    BPTBarCode = row["BarCode"].ToString(),
                    Level = row["StrengthLevel"].ToString(),
                    Surface = row["SurfaceMethod"].ToString() + "/" + row["ToothType"].ToString(),
                    SurfaceMethod = row["SurfaceMethod"].ToString(),
                    NameDesc = row["CNDescription"].ToString(),
                    Brand = row["BrandName"].ToString(),
                    BCTFullName = customer?.BCTFullName,
                    ShipmentSerial = entity.SPTSerial,
                    SODOrderNumber = row["SODOrderNumber"].ToString().ToDecimal(),
                    MinPackageQty = bulkNumber,
                    MinPackageUnit = "散",
                    Number = bulkNumber + "",
                    PackageNum = string.Empty,
                    Flag = globalFlagCode,
                    CPSStandardCode = row["CPSStandardCode"].ToString(),
                    LabelSequenceIndex = (++labelIndex).ToString("0000"),
                };
                var unit = (obj.MainUnit.StartsWith("千") || obj.MainUnit.ToLower().StartsWith("k")) ? "KPCS" : "PCS";
                obj.barCode = $"S{globalFlagCode + row["BarCode"]}";
                obj.Qrcode = $"{obj.barCode},{bulkNumber},{unit},{obj.Batch}";
                obj.GPQrcode = $"{globalFlagCode + obj.BarCode},{bulkNumber},{unit},{obj.Batch},S";
                obj.PackageNumber = bulkNumber + $"{unit}/散";
                obj.PackageNumberDesc = bulkNumber + unit;
                list.Add(obj);
            }
            if (!list.Any())
            {
                throw new Exception("没有需要打印的散装标签");
            }
            report.Data = list;
            var customFields = LabelPrinterUtils.GetLabelCustomFields(report.ReportId);
            if (customFields.Any())
            {
                var frm = new frmCustomPrintFieldsInfo(customFields, (dic) =>
                {
                    report.CustomFields = dic;
                });
                frm.ShowDialog();
            }
            PrintLabels(report);
        }

        /// <summary>
        /// 打印出货后剩余散装标签
        /// </summary>
        /// <param name="strSPTId"></param>
        /// <exception cref="Exception"></exception>
        public static void PrintLeaveBulkLabelsAfterShip(string strSPTId)
        {
            var entity = SQL.GetEntity<Sel_Shipment>(strSPTId);
            if (entity == null || "已作废".Equals(entity.SPTStatus) || "已删除".Equals(entity.SPTStatus)) return;

            var sql = $@"SELECT  BPTId Id,BPTBarCode BarCode,BPTCode Code,BPTMaterial Material,BPTStrengthLevel StrengthLevel,BPTName Name,BPTCNDescription CNDescription,
                         BPTSurfaceMethod SurfaceMethod,BPTSpecification Specification,BPTToothType ToothType,BPTMainUnit MainUnit,BPTStardandSinglet StardandSinglet,
                         BPTSinglet Singlet,BPTMinPackageUnit1,BPTMinPackageQty1,BPTMinPackageUnit2,BPTMinPackageQty2,BPTMinPackageUnit3,BPTMinPackageQty3,BPTMark Mark,BPTBrandName BrandName,ISNULL(B.WPLStock,0) -ISNULL(A.SDLTakeNumber,0) RequestNumber
                         FROM Sel_DeliveryStockLocation A WITH(NOLOCK) 
                         INNER JOIN Whs_ProductLocationStock B WITH(NOLOCK) ON A.SDLProductId =B.WPLProductId AND A.SDLLocationId =B.WPLLocationId 
                         INNER JOIN Bas_Products C WITH(NOLOCK) ON A.SDLProductId =C.BPTId 
                         WHERE ISNULL(B.WPLStock,0) -ISNULL(A.SDLTakeNumber,0) >0 AND  A.SDLDeliverySerial = '{entity.SPTSerial}'
                         ";
            sql += $@"SELECT TOP 1 SRGId,SRGName  from Sys_ReportManager  where SRGClass = '标签模板类' and SRGState ='启用'  ORDER  BY ISNULL(SRGDefaultReport,0) DESC;";
            var ds = SQL.GetDataSet(sql);
            if (ds == null || ds.Tables.Count < 2) return;
            if (ds.Tables[0] == null || ds.Tables[0].Rows.Count <= 0)
            {
                throw new Exception("数据获取异常");
            }
            if (ds.Tables[1] == null || ds.Tables[1].Rows.Count < 0)
            {
                throw new Exception("请维护默认标签模板");
            }
            var report = new PushPrintLableDto()
            {
                ReportId = ds.Tables[1].Rows[0]["SRGId"].ToString()
            };
            var time = SQL.GetTime();
            List<PrintLabelColumnDto> list = new List<PrintLabelColumnDto>();
            DataTable table = ExtendDictionary.GetDictionariesByParentCode(DataDictionaryModuleIndex.PrintFactoryInfo);
            string globalFlagCode = string.Empty;
            var sgdInfo = SQL.GetEntity<Sys_GlobalDict>(Sys_GlobalDict.SGDCode_ColumnName, DataDictionaryModuleIndex.GlobalFlagCode);
            if (sgdInfo != null)
            {
                globalFlagCode = sgdInfo.SGDDictValue;
            }
            int labelIndex = 0;
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                var bPTMinPackageQty1 = row["BPTMinPackageQty1"].ToString().ToDecimal();
                string bPTMinPackageUnit1 = row["BPTMinPackageUnit1"].ToString();

                decimal bPTMinPackageQty2 = row["BPTMinPackageQty2"].ToString().ToDecimal();
                string bPTMinPackageUnit2 = row["BPTMinPackageUnit2"].ToString();
                decimal bPTMinPackageQty3 = row["BPTMinPackageQty3"].ToString().ToDecimal();
                string bPTMinPackageUnit3 = row["BPTMinPackageUnit3"].ToString();
                decimal cPSMinPackageQty1 = 0;
                string cPSMinPackageUnit1 = string.Empty;
                decimal cPSMinPackageQty2 = 0;
                string cPSMinPackageUnit2 = string.Empty;
                decimal cPSMinPackageQty3 = 0;
                string cPSMinPackageUnit3 = string.Empty;
                var requestNumber = row["RequestNumber"].ToString().ToDecimal();
                var packages = ExtendProducts.GetPackages(bPTMinPackageQty1, bPTMinPackageUnit1, bPTMinPackageQty2, bPTMinPackageUnit2, bPTMinPackageQty3, bPTMinPackageUnit3,
                cPSMinPackageQty1, cPSMinPackageUnit1, cPSMinPackageQty2, cPSMinPackageUnit2, cPSMinPackageQty3, cPSMinPackageUnit3);
                var bulkNumber = requestNumber;
                if (packages != null && packages.Count > 0)
                {
                    bulkNumber = ExtendProducts.GetBulkPackageNumber(packages, requestNumber);
                }
                bulkNumber = bulkNumber.ToString("0.###").ToDecimal();
                if (bulkNumber <= 0) continue;
                var obj = new PrintLabelColumnDto()
                {
                    Id = row["Id"].ToString(),
                    BarCode = row["BarCode"].ToString(),
                    Code = row["Code"].ToString(),
                    Material = row["Material"].ToString(),
                    StrengthLevel = row["StrengthLevel"].ToString(),
                    Name = row["Name"].ToString(),
                    Specification = row["Specification"].ToString(),
                    ToothType = row["ToothType"].ToString(),
                    MainUnit = row["MainUnit"].ToString(),
                    StardandSinglet = row["StardandSinglet"].ToString().ToDecimal(),
                    Singlet = row["Singlet"].ToString().ToDecimal(),
                    CNDescription = row["CNDescription"].ToString(),
                    MinPackageUnit1 = (packages != null && packages.Count > 0) ? packages.First().Key : string.Empty,
                    MinPackageQty1 = ((packages != null && packages.Count > 0) ? packages.First().Value : 0),
                    MinPackageUnit2 = (packages != null && packages.Count > 1) ? packages.Skip(1).Take(1).First().Key : string.Empty,
                    MinPackageQty2 = ((packages != null && packages.Count > 1) ? packages.Skip(1).Take(1).First().Value : 0),
                    MinPackageUnit3 = (packages != null && packages.Count > 2) ? packages.Skip(2).Take(1).First().Key : string.Empty,
                    MinPackageQty3 = ((packages != null && packages.Count > 2) ? packages.Skip(2).Take(1).First().Value : 0),
                    Mark = row["Mark"].ToString(),
                    //CPSProductName = row["BrandName"].ToString(),
                    //默认情况，批次需要小于ShipDate一天，常识情况 by arison 20230919
                    Batch = time.AddDays(-1).ToString("yyMMdd"),
                    Date = time.ToShortDateString(),
                    PackageNum1 = 0,
                    PackageNum2 = 0,
                    PackageNum3 = 0,
                    ShipDate = time.ToString("yyyy.MM.dd"),
                    SSinglet = row["Singlet"].ToString().ToDecimal(),
                    CompanyName = (table == null ? "" : (table.Select("SGDCode='FactoryName'").FirstOrDefault()?["SGDDictValue"].ToString() ?? string.Empty)),
                    CompanyFullName = (table == null ? "" : (table.Select("SGDCode='CompanyFullName'").FirstOrDefault()?["SGDDictValue"].ToString() ?? string.Empty)),
                    BPTBarCode = row["BarCode"].ToString(),
                    Level = row["StrengthLevel"].ToString(),
                    Surface = row["SurfaceMethod"].ToString() + "/" + row["ToothType"].ToString(),
                    SurfaceMethod = row["SurfaceMethod"].ToString(),
                    NameDesc = row["CNDescription"].ToString(),
                    Brand = row["BrandName"].ToString(),
                    MinPackageQty = bulkNumber,
                    MinPackageUnit = "散",
                    Number = bulkNumber + "",
                    PackageNum = string.Empty,
                    Flag = globalFlagCode,
                    LabelSequenceIndex = (++labelIndex).ToString("0000"),
                };
                var unit = (obj.MainUnit.StartsWith("千") || obj.MainUnit.ToLower().StartsWith("k")) ? "KPCS" : "PCS";
                obj.barCode = $"S{globalFlagCode + row["BarCode"]}";
                obj.Qrcode = $"{obj.barCode},{bulkNumber},{unit},{obj.Batch}";
                obj.GPQrcode = $"{globalFlagCode + obj.BarCode},{bulkNumber},{unit},{obj.Batch},S";
                obj.PackageNumber = bulkNumber + $"{unit}/散";
                obj.PackageNumberDesc = bulkNumber + unit;
                list.Add(obj);
            }
            if (!list.Any())
            {
                return;
            }
            report.Data = list;
            var customFields = LabelPrinterUtils.GetLabelCustomFields(report.ReportId);
            if (customFields.Any())
            {
                var frm = new frmCustomPrintFieldsInfo(customFields, (dic) =>
                {
                    report.CustomFields = dic;
                });
                frm.ShowDialog();
            }
            PrintLabels(report);
        }

        /// <summary>
        /// 打印标签
        /// </summary>
        /// <param name="dto"></param>
        /// <exception cref="Exception"></exception>
        public static void PrintLabels(PrintLabelInfoDto dto)
        {
            var time = SQL.GetTime();
            var packageFlags = ProductPackageDao.GetPackageFlags();
            DataTable table = ExtendDictionary.GetDictionariesByParentCode(DataDictionaryModuleIndex.PrintFactoryInfo);
            string globalFlagCode = string.Empty;
            var sgdInfo = SQL.GetEntity<Sys_GlobalDict>(Sys_GlobalDict.SGDCode_ColumnName, DataDictionaryModuleIndex.GlobalFlagCode);
            if (sgdInfo != null)
            {
                globalFlagCode = sgdInfo.SGDDictValue;
            }

            var customFileds = new Dictionary<string, Dictionary<string, string>>();

            Bas_CustomerProducts customerProduct = null;
            Bas_Customers customer = null;
            if (!string.IsNullOrEmpty(dto.CustomerCode))
            {
                customer = SQL.GetEntity<Bas_Customers>(Bas_Customers.BCTCode_ColumnName, dto.CustomerCode);
                if (customer == null) return;
            }
            List<PrintLabelColumnDto> labels = new List<PrintLabelColumnDto>();
            var reportManager = ReportManagerDao.GetLabelReport(customer?.BCTId ?? string.Empty);
            if (reportManager == null)
            {
                throw new Exception("未找到打印模板，请先维护模板");
            }
            foreach (var item in dto.Details)
            {
                var product = SQL.GetEntity<Bas_Products>(Bas_Products.BPTId_ColumnName, item.ProductId);
                if (product == null) return;
                if (!string.IsNullOrEmpty(dto.CustomerCode))
                    customerProduct = CustomerProductDao.GetCustomerProduct(item.ProductId, customer.BCTId, item.CustomerPN);


                var packages = ExtendProducts.GetPackages((decimal)product.BPTMinPackageQty1, product.BPTMinPackageUnit1, (decimal)product.BPTMinPackageQty2, product.BPTMinPackageUnit2, (decimal)product.BPTMinPackageQty3, product.BPTMinPackageUnit3,
               (decimal)(customerProduct?.CPSMinPackageQty1 ?? 0), customerProduct?.CPSMinPackageUnit1, (decimal)(customerProduct?.CPSMinPackageQty2 ?? 0), customerProduct?.CPSMinPackageUnit2, (decimal)(customerProduct?.CPSMinPackageQty3 ?? 0), customerProduct?.CPSMinPackageUnit3);

                var number = item.Number;
                Dictionary<string, int> printLabels = new Dictionary<string, int>();
                packages.Select(x => new Public.Models.PackageInfo() { packageQty = x.Value, packageUnit = x.Key }).ToList()
               .ForEach(z =>
               {
                   number = item.Number;
                   var num = (int)Math.Floor(number / z.packageQty);
                   if (num > 0) printLabels.Add(z.packageUnit, num);
                   number %= z.packageQty;
               });
                int count = 0;
                int totalCount = printLabels.Sum(x => x.Value) + (number > 0 ? 1 : 0);
                var entity = new PrintLabelColumnDto()
                {
                    Id = product.BPTId,
                    BarCode = product.BPTBarCode,
                    Code = product.BPTCode,
                    Material = product.BPTMaterial,
                    StrengthLevel = product.BPTStrengthLevel,
                    Name = product.BPTName,
                    Specification = product.BPTSpecification,
                    ToothType = product.BPTToothType,
                    MainUnit = product.BPTMainUnit,
                    StardandSinglet = (decimal?)product.BPTStardandSinglet,
                    Singlet = (decimal?)product.BPTSinglet,
                    CNDescription = product.BPTCNDescription,
                    MinPackageUnit1 = (packages != null && packages.Count > 0) ? packages.First().Key : string.Empty,
                    MinPackageQty1 = ((packages != null && packages.Count > 0) ? packages.First().Value : 0),
                    MinPackageUnit2 = (packages != null && packages.Count > 1) ? packages.Skip(1).Take(1).First().Key : string.Empty,
                    MinPackageQty2 = ((packages != null && packages.Count > 1) ? packages.Skip(1).Take(1).First().Value : 0),
                    MinPackageUnit3 = (packages != null && packages.Count > 2) ? packages.Skip(2).Take(1).First().Key : string.Empty,
                    MinPackageQty3 = ((packages != null && packages.Count > 2) ? packages.Skip(2).Take(1).First().Value : 0),
                    Mark = product.BPTMark,
                    //CPSProductName = customerProduct?.CPSProductName ?? product.BPTBrandName,
                    CPSProductName = customerProduct?.CPSProductName,
                    CPSCode = customerProduct?.CPSCode,
                    CPSProjectCode = customerProduct?.CPSProjectCode,
                    SOMCustomerOrderSerial = dto.SOMCustomerOrderSerial,
                    CPSId = customerProduct?.CPSId,
                    CPSMaterial = customerProduct?.CPSMaterial,
                    CPSStandardCode = customerProduct?.CPSStandardCode,
                    CPSSpecification = customerProduct?.CPSSpecification,
                    CPSStrengthLevel = customerProduct?.CPSStrengthLevel,
                    CPSSurfaceMethod = customerProduct?.CPSSurfaceMethod,
                    //默认情况，批次需要小于ShipDate一天，常识情况 by arison 20230919
                    Batch = time.AddDays(-1).ToString("yyMMdd"),
                    Date = time.ToShortDateString(),
                    SOMSerial = dto.SOMSerial,
                    OrderSerial = dto.SOMSerial,
                    CustomerOrderSerial = dto.SOMCustomerOrderSerial,
                    PackageNum1 = ((printLabels != null && printLabels.Count > 0) ? printLabels.First().Value : 0),
                    PackageNum2 = ((printLabels != null && printLabels.Count > 1) ? printLabels.Skip(1).Take(1).First().Value : 0),
                    PackageNum3 = ((printLabels != null && printLabels.Count > 2) ? printLabels.Skip(2).Take(1).First().Value : 0),
                    ShipDate = time.ToString("yyyyMMdd"),
                    SSinglet = product.BPTSinglet.ToString().ToDecimal(),
                    CompanyName = (table == null ? "" : (table.Select("SGDCode='FactoryName'").FirstOrDefault()?["SGDDictValue"].ToString() ?? string.Empty)),
                    CompanyFullName = (table == null ? "" : (table.Select("SGDCode='CompanyFullName'").FirstOrDefault()?["SGDDictValue"].ToString() ?? string.Empty)),
                    BPTBarCode = product.BPTBarCode,
                    Level = product.BPTStrengthLevel,
                    Surface = product.BPTSurfaceMethod + "/" + product.BPTToothType,
                    SurfaceMethod = product.BPTSurfaceMethod,
                    NameDesc = product.BPTCNDescription,
                    Brand = customerProduct?.CPSProductName ?? product.BPTBrandName,
                    BCTFullName = customer?.BCTFullName,
                    ShipmentSerial = string.Empty,
                    SODOrderNumber = item.OrderNumber,
                    PackageNum = string.Empty,
                    Flag = globalFlagCode,
                };
                int labelIndex = 0;
                foreach (var printLabel in printLabels)
                {
                    decimal num = packages[printLabel.Key].ToString("0.###").ToDecimal();
                    var obj = entity.Clone() as PrintLabelColumnDto;
                    var unit = (obj.MainUnit.StartsWith("千") || obj.MainUnit.ToLower().StartsWith("k")) ? "KPCS" : "PCS";
                    obj.MinPackageQty = num;
                    obj.MinPackageUnit = printLabel.Key;
                    obj.Number = packages[printLabel.Key].ToString();
                    obj.barCode = (packageFlags.ContainsKey(printLabel.Key) ? packageFlags[printLabel.Key] : "S") + globalFlagCode + obj.BarCode;
                    obj.Qrcode = $"{obj.barCode},{packages[printLabel.Key]},KPCS,{obj.Batch}";
                    obj.GPQrcode = $"{globalFlagCode + obj.BarCode},{packages[printLabel.Key]},{unit},{obj.Batch},{(packageFlags.ContainsKey(printLabel.Key) ? packageFlags[printLabel.Key] : "S")}";
                    obj.PackageNumber = packages[printLabel.Key] + $"{unit}/{printLabel.Key}";
                    obj.PackageNumberDesc = packages[printLabel.Key] + unit;
                    for (int i = 0; i < printLabel.Value; i++)
                    {
                        var objcopy = obj.Clone() as PrintLabelColumnDto;
                        objcopy.PackageNum = $"{count}/{totalCount}";
                        //标签序号（打印多少张的顺序序列值）
                        objcopy.LabelSequenceIndex = (++labelIndex).ToString("0000");
                        labels.Add(objcopy);
                    }
                }

                if (number > 0)
                {
                    number = number.ToString("0.###").ToDecimal();
                    var obj = entity.Clone() as PrintLabelColumnDto;
                    var unit = (obj.MainUnit.StartsWith("千") || obj.MainUnit.ToLower().StartsWith("k")) ? "KPCS" : "PCS";
                    obj.MinPackageQty = number;
                    obj.MinPackageUnit = "散";
                    obj.Number = number.ToString();
                    obj.barCode = "S" + globalFlagCode + obj.BarCode;
                    obj.Qrcode = $"{obj.barCode},{number},{unit},{obj.Batch}";
                    obj.GPQrcode = $"{globalFlagCode + obj.BarCode},{number},{unit},{obj.Batch},S";
                    obj.PackageNumber = number + $"{unit}/散";
                    obj.PackageNumberDesc = number + unit;
                    obj.PackageNum = $"{count}/{totalCount}";
                    //标签序号（打印多少张的顺序序列值）
                    obj.LabelSequenceIndex = (++labelIndex).ToString("0000");
                    labels.Add(obj);
                }

            }

            var report = new PushPrintLableDto()
            {
                ReportId = reportManager.SRGId,
                Data = labels
            };
            if (customFileds.ContainsKey(report.ReportId))
            {
                report.CustomFields = customFileds[report.ReportId];
            }
            else
            {
                var customFields = LabelPrinterUtils.GetLabelCustomFields(report.ReportId);
                if (customFields.Any())
                {
                    var frm = new frmCustomPrintFieldsInfo(customFields, (dic) =>
                    {
                        report.CustomFields = dic;
                        customFileds.Add(report.ReportId, dic);
                    });
                    frm.ShowDialog();
                }
            }
            LabelPrinterUtils.PrintLabels(report);

        }
    }

    public class PrintLabelInfoDto
    {
        /// <summary>
        /// 客户编号
        /// </summary>
        public string CustomerCode { get; set; }

        public string SOMCustomerOrderSerial { get; set; }
        public string SOMSerial { get; set; }
        public string Unit { get; set; } = "KPCS";

        public List<PrintLabelDetailDto> Details { get; set; }
    }

    public class PrintLabelDetailDto
    {
        public string ProductId { get; set; }

        /// <summary>
        /// 料号
        /// </summary>
        public string CustomerPN { get; set; }
        public decimal Number { get; set; }
        public decimal OrderNumber { get; set; }
    }
}
