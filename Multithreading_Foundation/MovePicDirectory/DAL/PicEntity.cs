using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class PicEntity
    {

        public DataSet MovePic(string nowDate, string gap, string maxNum)
        {
            string sql = string.Empty;
            DataSet dsReturn = new DataSet();
            try
            {
                sql = string.Format(@"SELECT top {1}
	                                        PATH, TYPE, LOT_NUMBER
                                        FROM
	                                        WIP_IV_PIC 
                                        WHERE 1=1 and IS_MOVED = 0 and
	                                        DateDiff(
	                                        dd,
	                                        CONVERT ( VARCHAR ( 20 ), CREATE_TIME, 23 ),
	                                        CONVERT ( VARCHAR ( 20 ), GETDATE (), 23 ) 
	                                        ) = {0}", Convert.ToInt32(gap), Convert.ToInt32(maxNum));
                dsReturn = SQLHelper.GetDataSet(sql);
            }
            catch (Exception ex)
            {

            }
            return dsReturn;
        }

        public string ModifySourcePic(string targetPic, string lotNumber, string type)
        {
            string sql = string.Empty;
            string msg = string.Empty;
            List<string> sqlList = new List<string>();
            try
            {
                sql = string.Format(@"select * from WIP_IV_PIC where LOT_NUMBER = '{0}' and IS_MOVED = 1 and TYPE = '{1}'", lotNumber, type);
                DataTable dt = SQLHelper.GetDataSet(sql).Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    return string.Format(@"图片：{0}已被移动！", lotNumber);
                }
                sql = string.Format(@"update WIP_IV_PIC set IS_MOVED = 1, MOVE_PATH = '{0}' where LOT_NUMBER = '{1}' and TYPE = '{2}'", targetPic, lotNumber, type);
                sqlList.Add(sql);
                bool result = SQLHelper.UpdateByTran(sqlList);
                if (result == false)
                {
                    return "发生错误。";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return msg;
        }
    }
}
