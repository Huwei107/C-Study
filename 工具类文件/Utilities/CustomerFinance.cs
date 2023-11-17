using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace FX.MainForms.Public.Utilities
{
    public class CustomerFinance
    {
        /// <summary>
        /// 获取该客户的财务核验
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string GetFinanceInfo(string code)
        {
            string sql = @" SELECT A.BCTId,A.BCTCode+CASE A.BCTIsInvoice WHEN 1 THEN '-Y' WHEN 0 THEN '-N' END BCTCode,
                        A.BCTRememberCode,A.BCTShortName,A.BCTSettlementMethod,
                       ISNULL((SELECT SUM(CONVERT(DECIMAL(18,2),SPDCheckNumber*SPDCheckPrice)) FROM dbo.Sel_Shipment A1,dbo.Sel_ShipmentDetails B1 
                        WHERE A1.SPTSerial = B1.SPDSerial AND B1.SPDCheckStatus ='未对账'  AND (A1.SPTStatus = '已送货' or A1.SPTStatus = '已收货') 
                        AND A1.SPTCustomerCode = A.BCTCode),0)+
                        ISNULL((SELECT SUM(FCZCheckBalance) FROM dbo.Fin_CustomerCheck A1,dbo.Fin_CustomerCheckDetails B1 
                        WHERE A1.FCDSerial = B1.FCZSerial AND A1.FCDCustomerId = A.BCTCode AND A1.FCDStatus = '已审核'),0) DJYS,
                        ISNULL((SELECT SUM(CONVERT(DECIMAL(18,2),SPDCheckNumber*B1.SPDCheckPrice)) FROM dbo.Sel_Shipment A1,dbo.Sel_ShipmentDetails B1 
                        WHERE A1.SPTSerial = B1.SPDSerial AND B1.SPDCheckStatus ='未对账' AND (A1.SPTStatus = '已送货' or A1.SPTStatus = '已收货') 
                        AND A1.SPTCustomerCode = A.BCTCode),0) WDZ,                                                             --未对账
                        ISNULL((SELECT SUM(FCZCheckBalance) FROM dbo.Fin_CustomerCheck A1,dbo.Fin_CustomerCheckDetails B1 
                        WHERE A1.FCDSerial = B1.FCZSerial AND A1.FCDCustomerId = A.BCTCode AND A1.FCDStatus = '未审核'),0) DZZ, --对账中
                        ISNULL((SELECT SUM(FCZCheckBalance) FROM dbo.Fin_CustomerCheck A1,dbo.Fin_CustomerCheckDetails B1 
                        WHERE A1.FCDSerial = B1.FCZSerial AND A1.FCDCustomerId = A.BCTCode AND A1.FCDStatus = '已审核'),0) YDZ, --已对账

                       ISNULL((SELECT SUM(FCDCheckMoney-FCDInvoiceBalance) FROM dbo.Fin_CustomerCheck A1 
                        WHERE A1.FCDCustomerId = A.BCTCode AND A1.FCDStatus = '已审核'),0) YKP,     --已开票=立账金额-发票余额
                        
                        ISNULL((SELECT SUM(FCDInvoiceBalance) FROM dbo.Fin_CustomerCheck A1 
                        WHERE A1.FCDCustomerId = A.BCTCode AND A1.FCDStatus = '已审核'),0) DKP,     --未开票
                        
                        ISNULL((SELECT SUM(FCDCheckMoney-FCDProceedsBalance) FROM dbo.Fin_CustomerCheck A1 
                        WHERE A1.FCDCustomerId = A.BCTCode AND A1.FCDStatus = '已审核'),0) SJYS,     --已收款=立账金额-收款余额
                                                 
                        ISNULL((SELECT SUM(FCDProceedsBalance) FROM dbo.Fin_CustomerCheck A1 
                        WHERE A1.FCDCustomerId = A.BCTCode AND A1.FCDStatus = '已审核'),0) WSK,     --未收款=收款的余额

                        ISNULL((SELECT SUM(FCDProceedsBalance) FROM dbo.Fin_CustomerCheck A1 
                        WHERE A1.FCDCustomerId = A.BCTCode AND A1.FCDStatus = '已审核' 
                        AND GETDATE() >= FCDFinalDate AND GETDATE() < FCDOverdueDate),0) YDQ,       --已到期

                        ISNULL((SELECT SUM(FCDProceedsBalance) FROM dbo.Fin_CustomerCheck A1 
                        WHERE A1.FCDCustomerId = A.BCTCode AND A1.FCDStatus = '已审核' 
                        AND GETDATE() >= FCDOverdueDate),0) YYQ                                     --已逾期      
                        FROM dbo.Bas_Customers A 
                        WHERE A.BCTCode = '" + code + "' " +
                      @" AND
                        (EXISTS(SELECT 1 FROM dbo.Sel_Shipment B WHERE B.SPTCustomerCode = A.BCTCode)
                        OR EXISTS(SELECT * FROM dbo.Fin_CustomerCheck C,dbo.Fin_CustomerCheckDetails D WHERE A.BCTCode = C.FCDCustomerId AND C.FCDSerial = D.FCZSerial AND D.FCZSourceType='初期应收款单'))";
            //已对账|未对账|对账中|已开票|未开票|已收款|未收款|已到期|逾期|总欠款
            DataTable dt = SQL.GetDataTable(sql);
            string totalMsg = " 未对账:" + 0.00 + ",已对账:" + 0.00 + ",待开票:" + 0.00 + ",实际已收:" + 0.00 + ",已到期:" + 0.00 + ",已逾期:" + 0.00 + ",总欠款:" + 0.00;
            if (dt != null && dt.Rows.Count > 0)
            {
                decimal totalmoney = dt.Rows[0]["WSK"].ToString().ToDecimal() + dt.Rows[0]["DZZ"].ToString().ToDecimal() + dt.Rows[0]["WDZ"].ToString().ToDecimal();
                totalMsg = " 未对账:" + dt.Rows[0]["WDZ"].ToString() + ",已对账:" + dt.Rows[0]["YDZ"].ToString() + ",待开票:" + dt.Rows[0]["DKP"].ToString() + ",实际已收:" + dt.Rows[0]["SJYS"].ToString() + ",已到期:" + dt.Rows[0]["YDQ"].ToString() + ",已逾期:" + dt.Rows[0]["YYQ"].ToString() + ",总欠款:" + totalmoney;
            }
            return totalMsg;
        }
    }
}
