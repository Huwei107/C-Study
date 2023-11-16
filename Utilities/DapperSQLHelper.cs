//-----------------------------------------------------------------------
// <copyright company="螺丝钉" file="DapperSQLHelper.cs">
//  Copyright (c)  V1.0.0.0  
//  创建作者:   刘少林
//  创建时间:   2019-06-28 16:54:46
//  功能描述:   Dapper SQL 帮助类
//  历史版本:
//          2019-06-28 16:54:46 刘少林 创建DapperSQLHelper类
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using Dapper;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using FX.ORM.Websharp.ORM.Service;

namespace FX.MainForms.Public.Utilities
{
    /// <summary>
    /// Dapper SQL 帮助类
    /// </summary>
    public class DapperSQLHelper
    {
        /// <summary>
        /// 获取数据集合对应实体集合
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="sql">执行SQL语句</param>
        /// <param name="connStr">数据库连接字符串</param>
        /// <param name="parameters">参数化对象</param>
        /// <returns>实体集合</returns>
        public static List<T> GetEntities<T>(string sql, string connStr = "", params SqlParameter[] parameters)
        {
            if (string.IsNullOrEmpty(connStr))
            {
                connStr = ApplicationConfiguration.DefaultDatabaseProperty.ConnectionString;
            }
            using (var conn = new System.Data.SqlClient.SqlConnection(connStr))
            {
                conn.Open();
                var dp = new DynamicParameters();
                if (parameters != null && parameters.Length > 0)
                {
                    foreach (SqlParameter pt in parameters)
                    {
                        dp.Add(pt.ParameterName, pt.Value, pt.DbType);
                    }
                }
                var result = conn.Query<T>(sql, dp);
                conn.Close();
                return result.ToList();
            }
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="connStr">数据库连接字符串</param>
        /// <returns></returns>
        public static bool Execute(string sql, string connStr = "", params SqlParameter[] parameters)
        {
            if (string.IsNullOrEmpty(connStr))
            {
                connStr = ApplicationConfiguration.DefaultDatabaseProperty.ConnectionString;
            }
            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                IDbTransaction trans = conn.BeginTransaction();
                try
                {
                    var dp = new DynamicParameters();
                    if (parameters != null && parameters.Length > 0)
                    {
                        foreach (SqlParameter pt in parameters)
                        {
                            dp.Add(pt.ParameterName, pt.Value, pt.DbType);
                        }
                    }
                    conn.Execute(sql, dp, trans, null, null);
                    trans.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    HelperLog.Write(ex);
                    return false;
                }
            }
        }

        /// <summary>
        /// 插入实体集合
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="sql">对应插入SQL</param>
        /// <param name="entities">实体集合</param>
        /// <param name="connStr">数据库连接字符串</param>
        /// <returns></returns>
        public static int InsertMultiple<T>(string sql, IEnumerable<T> entities, string connStr = "") where T : class, new()
        {
            if (string.IsNullOrEmpty(connStr))
            {
                connStr = ApplicationConfiguration.DefaultDatabaseProperty.ConnectionString;
            }
            using (SqlConnection cnn = new SqlConnection(connStr))
            {
                cnn.Open();
                int records = 0;
                using (var trans = cnn.BeginTransaction())
                {
                    try
                    {
                        cnn.Execute(sql, entities, trans, 30, CommandType.Text);
                    }
                    catch (DataException ex)
                    {
                        trans.Rollback();
                        throw ex;
                    }
                    trans.Commit();
                }
                return records;
            }
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="connStr">数据库字符串链接</param>
        /// <param name="parameters">sqlparameter参数集</param>
        public static void RunStoreProc(string procName, params SqlParameter[] parameters)
        {
            var dp = new DynamicParameters();
            foreach (SqlParameter pt in parameters)
            {
                dp.Add(pt.ParameterName, pt.Value, pt.DbType, pt.Direction, pt.Size);
            }
            RunStoreProc(procName, dp);
        }


        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="connStr">数据库字符串链接</param>
        /// <param name="parameters">sqlparameter参数集</param>
        public static void RunStoreProc(string procName, string connStr, params SqlParameter[] parameters)
        {
            var dp = new DynamicParameters();
            foreach (SqlParameter pt in parameters)
            {
                dp.Add(pt.ParameterName, pt.Value, pt.DbType, pt.Direction, pt.Size);
            }
            RunStoreProc(procName, dp, connStr);
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="parameters">所使用参数</param>
        /// <param name="connStr">数据库链接字符串</param>
        public static void RunStoreProc(string procName, DynamicParameters parameters)
        {
            var connStr = ApplicationConfiguration.DefaultDatabaseProperty.ConnectionString;
            RunStoreProc(procName, parameters);
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="parameters">所使用参数</param>
        /// <param name="connStr">数据库链接字符串</param>
        public static void RunStoreProc(string procName, DynamicParameters parameters, string connStr)
        {
            if (string.IsNullOrEmpty(connStr))
            {
                connStr = ApplicationConfiguration.DefaultDatabaseProperty.ConnectionString;
            }
            using (SqlConnection cnn = new SqlConnection(connStr))
            {
                cnn.Open();
                using (var trans = cnn.BeginTransaction())
                {
                    try
                    {
                        cnn.Execute(procName, parameters, trans, null, CommandType.StoredProcedure);
                    }
                    catch (DataException ex)
                    {
                        trans.Rollback();
                        throw ex;
                    }
                    trans.Commit();
                }
            }
        }

        /// <summary>
        /// describe:支持 DataSet
        /// </summary>
        /// <param name="cnn"></param>
        /// <param name="adapter"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="buffered"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        //public static DataSet ExecuteDataSet(this IDbConnection cnn, IDbDataAdapter adapter, string sql, object param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        //{
        //    var ds = new DataSet();
        //    var command = new CommandDefinition(sql, (object)param, null, commandTimeout, commandType, buffered ? CommandFlags.Buffered : CommandFlags.None);
        //    var identity = new Identity(command.CommandText, command.CommandType, cnn, null, param == null ? null : param.GetType(), null);
        //    var info = GetCacheInfo(identity, param, command.AddToCache);
        //    bool wasClosed = cnn.State == ConnectionState.Closed;
        //    if (wasClosed) cnn.Open();
        //    adapter.SelectCommand = command.SetupCommand(cnn, info.ParamReader);
        //    adapter.Fill(ds);
        //    if (wasClosed) cnn.Close();
        //    return ds;
        //}

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="connStr">数据库连接字符串</param>
        /// <returns></returns>
        public static DataTable GetTable(string sql, params SqlParameter[] parameters)
        {
            var connStr = ApplicationConfiguration.DefaultDatabaseProperty.ConnectionString;
            return GetTable(sql, connStr, parameters);
        }

        /// <summary>
        /// 获取单个表格
        /// </summary>
        /// <param name="sql">执行SQL语句</param>
        /// <param name="connStr">数据库链接字符串</param>
        /// <param name="parameters">参数集</param>
        /// <returns></returns>
        public static DataTable GetTable(string sql, string connStr, params SqlParameter[] parameters)
        {
            if (string.IsNullOrEmpty(connStr))
            {
                connStr = ApplicationConfiguration.DefaultDatabaseProperty.ConnectionString;
            }
            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    var dp = new DynamicParameters();
                    if (parameters != null && parameters.Length > 0)
                    {
                        foreach (SqlParameter pt in parameters)
                        {
                            dp.Add(pt.ParameterName, pt.Value, pt.DbType);
                        }
                    }
                    var ireader = conn.ExecuteReader(sql, dp, null, null, null);
                    var table = new DataTable();
                    table.Load(ireader);
                    foreach (DataColumn column in table.Columns)
                    {
                        column.AllowDBNull = true;
                    }
                    return table;
                }
                catch (Exception ex)
                {
                    HelperLog.Write(ex);
                    return null;
                }
            }
        }

        /// <summary>
        /// 获取表格DataSet集合
        /// </summary>
        /// <param name="sql">执行SQL语句</param>
        /// <param name="connStr">数据库链接字符串</param>
        /// <param name="parameters">参数集</param>
        /// <returns></returns>
        public static DataSet GetDataSet(string sql, string connStr = "", params SqlParameter[] parameters)
        {
            if (string.IsNullOrEmpty(connStr))
            {
                connStr = ApplicationConfiguration.DefaultDatabaseProperty.ConnectionString;
            }
            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    var dp = new DynamicParameters();
                    if (parameters != null && parameters.Length > 0)
                    {
                        foreach (SqlParameter pt in parameters)
                        {
                            dp.Add(pt.ParameterName, pt.Value, pt.DbType);
                        }
                    }
                    var ireader = conn.ExecuteReader(sql, dp, null, null, null);
                    DataSet ds = new DataSet();
                    //ds.Load(ireader, LoadOption.PreserveChanges);
                    do
                    {
                        var table = new DataTable();
                        table.Load(ireader);
                        if (table == null || table.Columns == null || table.Columns.Count == 0)
                        {
                            //20191109 经过测试对于使用union查询的SQL语句执行的时候，会出现异常问题，循环无法退出 by arison
                            //20191109 同时SQL里面，也有IF的判断，不知道是否是两者引起，还是其他方式引起          by arison
                            break;
                        }
                        else
                        {
                            foreach (DataColumn column in table.Columns)
                            {
                                column.AllowDBNull = true;
                            }
                        }
                        ds.Tables.Add(table);
                    }
                    while (ireader != null || ireader.NextResult());
                    return ds;
                }
                catch (Exception ex)
                {
                    HelperLog.Write(ex);
                    return null;
                }
            }
        }

    }
}