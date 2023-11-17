using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

using System.Configuration;

namespace DAL
{
    public class SQLHelper
    {
        public static readonly string connString = ConfigurationManager.ConnectionStrings["SqlServer"].ToString();

        /// <summary>
        /// 返回结果集
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>返回SqlDataReader对象</returns>
        public static DataSet GetDataSet(string sql)
        {
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            //创建数据集适配对象
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            try
            {
                conn.Open();
                da.Fill(ds);//使用数据填充器填充数据集
                return ds;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        public static bool UpdateByTran(List<string> sqlList)
        {
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            try
            {
                conn.Open();
                cmd.Transaction = conn.BeginTransaction();//开启事务
                foreach (string itemSql in sqlList)
                {
                    cmd.CommandText = itemSql;
                    cmd.ExecuteNonQuery();
                }
                cmd.Transaction.Commit();//提交事务（保存到数据库）
                return true;
            }
            catch (Exception ex)
            {
                if (cmd.Transaction != null)
                {
                    cmd.Transaction.Rollback();//回滚事务
                }
                throw new Exception("调用事务方法 UpdateByTran 时出错：" + ex.Message);
            }
            finally
            {
                if (cmd.Transaction != null)
                {
                    cmd.Transaction = null;//清空事务
                }
                conn.Close();
            }
        }
    }
}
