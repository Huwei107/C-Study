using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Xml;
using System.IO;
using System.Collections;
using System.Data.SQLite;
using System.Runtime.InteropServices;
using System.Text;
namespace FX.MainForms
{
    /// <summary>
    /// 对SQLite操作的类
    /// </summary>
    public class SQLiteHelper
    {
        //获取连接字符串
        private static readonly string str = HelperIniFile.GetPrivateProfileString("LabelPrintConfig", "conStr", @".\public.ini");

        /// <summary>
        /// 做增删改的功能
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="ps">SQL语句中的参数</param>
        /// <returns>受影响的行数</returns>
        public static int ExecuteNonQuery(string sql, params SQLiteParameter[] ps)
        {
            //连接数据库
            using (SQLiteConnection con = new SQLiteConnection(str))
            {
                using (SQLiteCommand cmd = new SQLiteCommand(sql, con))
                {
                    con.Open();//打开数据库
                    if (ps != null)
                    {
                        cmd.Parameters.AddRange(ps);
                    }
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// 查询首行首列
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="ps">SQL语句的参数</param>
        /// <returns>返回首行首列object</returns>
        public static object ExecuteScalar(string sql, params SQLiteParameter[] ps)
        {
            using (SQLiteConnection con = new SQLiteConnection(str))
            {
                using (SQLiteCommand cmd = new SQLiteCommand(sql, con))
                {
                    con.Open();
                    if (ps != null)
                    {
                        cmd.Parameters.AddRange(ps);
                    }
                    return cmd.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// 查询多行
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="ps">SQL语句的参数</param>
        /// <returns>返回多行SQLiteDataReader</returns>
        public static SQLiteDataReader ExecuteReader(string sql, params SQLiteParameter[] ps)
        {
            SQLiteConnection con = new SQLiteConnection(str);
            using (SQLiteCommand cmd = new SQLiteCommand(sql, con))
            {
                if (ps != null)
                {
                    cmd.Parameters.AddRange(ps);
                }
                try
                {
                    con.Open();
                    return cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                }
                catch (Exception ex)
                {
                    con.Close();
                    con.Dispose();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 查询数据表
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="ps">SQL语句中的参数</param>
        /// <returns>返回表DataTable</returns>
        public static DataTable ExecuteTable(string sql, params SQLiteParameter[] ps)
        {
            DataTable dt = new DataTable();
            using (SQLiteDataAdapter sda = new SQLiteDataAdapter(sql, str))
            {
                if (ps != null)
                {
                    sda.SelectCommand.Parameters.AddRange(ps);
                }
                sda.Fill(dt);
                return dt;
            }
        }
    }
}