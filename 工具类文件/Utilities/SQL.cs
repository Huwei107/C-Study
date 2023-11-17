//-----------------------------------------------------------------------
// <copyright company="工品一号" file="SQL.cs">
//  Copyright (c)  V1.0.0.0  
//  创建作者:   刘少林
//  创建时间:   2018-03-29
//  功能描述:   对原FX.ORM框架操作进行二次封装类，方便界面调用和处理类
//  历史版本:
//          2018-03-29 刘少林 创建SQL.cs
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using FX.ORM.Websharp.ORM.Base;
using FX.ORM.Websharp.ORM.Service;
using FX.ORM.Websharp.Data;
using System.Data;
using System.Data.SqlClient;
using FX.Entity;
using System.Reflection;
using System.Linq;
using System.Diagnostics;

namespace FX.MainForms
{
    /// <summary>
    /// 对原FX.ORM框架操作进行二次封装类，方便界面调用和处理类
    /// </summary>
    public class SQL
    {
        /// <summary>
        /// 操作数据库的接口
        /// </summary>
        private static PersistenceManager pm = PersistenceManagerFactory.Instance().CreatePersistenceManager();

        /// <summary>
        ///  添加一个实体
        /// </summary>
        /// <param name="entity">需要添加的实体</param>
        public static void Add(PersistenceCapable entity)
        {
            pm.PersistNewObject(entity);
            pm.Close();
        }

        /// <summary>
        /// 是否添加成功
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public static bool AddNew(PersistenceCapable entity)
        {
            try
            {
                pm.PersistNewObject(entity);
                pm.Close();
                return true;
            }
            catch (Exception ex)
            {
                HelperLog.Write(ex);
                return false;
            }
            finally
            {
                pm.Close();
            }
        }

        /// <summary>
        /// 封装DataTable数据有效性
        /// </summary>
        /// <param name="dt">数据</param>
        /// <returns>true?有效:无效</returns>
        public static bool DtIsNullOrEmpty(DataTable dt)
        {
            if (dt != null && dt.Rows.Count > 0)
                return true;
            return false;
        }


        /// <summary>
        /// 按主键修改一个实体
        /// </summary>
        /// <param name="entity">需要修改的实体，修改了全部字段</param>
        public static void Update(PersistenceCapable entity)
        {
            pm.UpdateObject(entity);
            pm.Close();
        }

        /// <summary>
        /// 修改整个实体
        /// </summary>
        /// <param name="entity"></param>
        public static bool UpdateReturnBool(PersistenceCapable entity)
        {
            try
            {
                pm.UpdateObject(entity);
                pm.Close();
                return true;
            }
            catch (Exception ex)
            {
                HelperLog.Write(ex);
                return false;
            }
        }



        /// <summary>
        /// 按SQL修改
        /// </summary>
        /// <param name="sql"></param>
        public static bool Update(string sql)
        {
            int count = 0;
            try
            {
                count = pm.ExecuteNonQuery(sql);
                if (count > 0)
                    return true;
                else return false;
            }
            catch (Exception ex)
            {
                HelperLog.Write(ex);
                return false;
            }
        }

        /// <summary>
        /// 按主键修改一个实体
        /// </summary>
        /// <param name="entity">需要修改的实体</param>
        /// <param name="col">输入需要修改的字段名称，其它字段不更改，以逗号隔开。例:Update(en,"ID","Name")</param>
        public static void Update(PersistenceCapable entity, params string[] col)
        {
            pm.UpdateObject(entity, col);
            pm.Close();
        }

        /// <summary>
        /// 按主键删除一个实体
        /// </summary>
        /// <param name="entity">要删除的实体，只需给主键一赋值就可以</param>
        public static void Delete(PersistenceCapable entity)
        {
            pm.DeleteObject(entity);
            pm.Close();
        }

        /// <summary>
        /// 通过SQL语句返回一个DataTable
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="qpc">SQL参数,写法QueryParameterCollection qpc = new QueryParameterCollection();  qpc.Add("@ID",1);</param>
        /// <returns>查询结果(表对象)</returns>
        public static DataTable GetDataTable(string sql, QueryParameterCollection qpc = null)
        {
            if (qpc != null && qpc.Count > 0)
            {
                DataSet ds = pm.ExecuteDataSet(CommandType.Text, sql, qpc);
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
                return null;
            }
            else
            {
                return GetDataTable(sql);
            }
        }

        /// <summary>
        /// 通过SQL语句返回一个DataTable
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="para">原生态SQL参数化对象</param>
        /// <returns>查询结果(表对象)</returns>
        public static DataTable GetDataTable(string sql, SqlParameter[] para = null)
        {
            if (para != null && para.Length > 0)
            {
                //return pm.ExecuteDataSet(CommandType.Text, sql, para).Tables[0];
                DataSet ds = pm.ExecuteDataSet(CommandType.Text, sql, para);
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
                return null;
            }
            else
            {
                return GetDataTable(sql);
            }
        }

        /// <summary>
        /// 通过SQL语句返回一个DataTable
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>查询结果(表对象)</returns>
        public static DataTable GetDataTable(string sql)
        {
            DataSet ds = GetDataSet(sql);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 通过SQL语句返回一个DataSet
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>查询结果(集合对象)</returns>
        public static DataSet GetDataSet(string sql)
        {
            return pm.ExecuteDataSet(sql);
        }

        /// <summary>
        /// 通过SQL语句返回一个DataSet
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="qpc">SQL参数,写法QueryParameterCollection qpc = new QueryParameterCollection();  qpc.Add("@ID",1);</param>
        /// <returns>查询结果(集合对象)</returns>
        public static DataSet GetDataSet(string sql, QueryParameterCollection qpc)
        {
            return pm.ExecuteDataSet(CommandType.Text, sql, qpc);
        }

        /// <summary>
        /// 根据实体类型和实体主键得到这实体,在修改页的绑定时常用
        /// </summary>
        /// <param name="entity">实体类型 写法typeof(EntityClass) EntityClass表示实体类</param>
        /// <param name="InnerID">主键</param>
        /// <returns>返回一个实体 用一个实体接收此返回值，需转换一下 as EntityClass</returns>
        public static PersistenceCapable GetEntity(Type entity, object InnerID)
        {
            return pm.FindObjectByPrimaryKey(InnerID, entity);
        }

        /// <summary>
        /// 根据实体类型和实体主键得到这实体,
        /// </summary>
        /// <param name="entity">实体类型 写法typeof(EntityClass) EntityClass表示实体类</param>
        /// <param name="objectId">主键</param>
        /// <returns>返回一个实体 用一个实体接收此返回值，需转换一下 as EntityClass</returns>
        public static T GetEntity<T>(string objectId)
            where T : PersistenceCapable
        {
            return pm.FindObjectByPrimaryKey(objectId, typeof(T)) as T;
        }

        /// <summary>
        /// 获取无数据的实体类
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <returns>无数据实体类对象</returns>
        public static T GetEntity<T>()
            where T : PersistenceCapable
        {
            Type type = typeof(T);
            PersistenceCapable entity;
            if (type.IsInterface || type.IsAbstract)
            {
                entity = EntityManager.CreateObject(type) as PersistenceCapable;
            }
            else
            {
                entity = Activator.CreateInstance(type) as PersistenceCapable;
            }
            return entity as T;
        }


        /// <summary>
        /// 根据列名称获取相对SystemId唯一实体对象
        /// </summary>
        /// <param name="entity">实体类型 写法typeof(EntityClass) EntityClass表示实体类</param>
        /// <param name="columnName">列名称</param>
        /// <param name="columnValue">列值</param>
        /// <returns>返回一个实体 用一个实体接收此返回值，需转换一下 as EntityClass</returns>
        public static T GetEntity<T>(string columnName, object columnValue)
            where T : PersistenceCapable
        {
            IDictionary<string, object> dict = new Dictionary<string, object>(1);
            dict.Add(columnName, columnValue);
            return GetEntityByDictionary(typeof(T), dict) as T;
        }




        /// <summary>
        /// 根据键值对获取对应实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="dict">键值对</param>
        /// <returns>PersistenceCapable对象</returns>
        public static PersistenceCapable GetEntityByDictionary(Type type, IDictionary<string, object> dict)
        {
            //Type type = entity.GetType();

            PersistenceCapable entity;
            if (type.IsInterface || type.IsAbstract)
            {
                entity = EntityManager.CreateObject(type) as PersistenceCapable;
            }
            else
            {
                entity = Activator.CreateInstance(type) as PersistenceCapable;
            }

            string tableName = entity.EntityData.MainTableName;
            QueryParameterCollection collection = new QueryParameterCollection();
            string condition = string.Empty;
            if (dict != null && dict.Count > 0)
            {
                foreach (string key in dict.Keys)
                {
                    collection.Add(new QueryParameter("@" + key, dict[key]));
                    condition += $" AND {key}=@{key} ";
                }
            }
            PropertyInfo info = type.GetProperties()[0];
            string prefix = info.Name.Substring(0, 3);
            //20180930 注销systemId 此值不再使用! by arison!
            //collection.Add(new QueryParameter("@" + prefix + "SystemId", PersistenceGlobalData.SystemId));
            string sql = "SELECT * FROM " + tableName + " WITH(NOLOCK) WHERE 1=1 " + condition;
            return pm.FindEntityDataByCondition(sql, collection, type);
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="storedProcName">存储过程名称</param>
        /// <param name="p">原生态SQL参数化对象</param>
        public static void RunProcedure(string storedProcName, SqlParameter[] p)
        {
            pm.RunProcedure(storedProcName, p);
        }

        /// <summary>
        /// 获取单据编码
        /// </summary>
        /// <param name="tablename">表名称</param>
        /// <param name="systemId">系统账户</param>
        /// <param name="codeName">编码名称</param>
        /// <returns>单据编码</returns>
        /// <remarks>主要用于流水号获取</remarks>
        public static string GetCode(string tablename, string systemId = "0", string codeName = "0")
        {
            string code = string.Empty;
            SqlParameter[] p = new SqlParameter[4];
            p[0] = new SqlParameter("@tablename", tablename);
            p[1] = new SqlParameter("@CodeName", codeName);
            p[2] = new SqlParameter("@SystemId ", systemId);
            p[3] = new SqlParameter("@Res", SqlDbType.NVarChar, 50);
            p[3].Direction = ParameterDirection.Output;
            SQL.RunProcedure("dbo.GetCode", p);
            code = p[3].Value.ToString();
            return code;
        }
        /// <summary>
        /// 获取系统时间
        /// </summary>
        /// <returns>当前数据库时间</returns>
        public static DateTime GetTime()
        {
            DataTable timeTable = null;
            try
            {
                timeTable = SQL.GetDataTable(" SELECT CONVERT(VARCHAR(100),GETDATE(),120)");
                if (timeTable != null && timeTable.Rows.Count > 0)
                {
                    DateTime time;
                    if (DateTime.TryParse(timeTable.Rows[0][0].ToString(), out time))
                    {
                        return time;
                    }
                }
            }
            catch (Exception ex)
            {
                HelperLog.Write(ex);
                return DateTime.Now;
            }
            return DateTime.Now;
        }



        /// <summary>
        /// 执行SQL组
        /// </summary>
        /// <param name="list">SQL执行清单</param>
        public static bool ExecuteNonQuery(List<string> list)
        {
            bool result = false;
            Transaction tm = pm.CurrentTransaction;
            tm.Begin();
            try
            {
                for (int i = 0; i < list.Count; ++i)
                {
                    pm.ExecuteNonQuery(list[i]);
                }
                tm.Commit();
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                tm.Rollback();
                HelperLog.Write(ex);
                throw;
            }
            finally
            {
                pm.Close();
            }
            return result;
        }

        /// <summary>
        /// 执行SQL组(自带事务逻辑)
        /// </summary>
        /// <param name="sql">SQL执行清单数组</param>
        public static bool ExecuteNonQueryByTrans(params string[] sql)
        {
            if (sql != null && sql.Length > 0)
            {
                return ExecuteNonQuery(sql.ToList());
            }
            else
            {
                return false;
            }
        }

        private class tableInfo
        {
            public string tablename { get; set; }
            public string primary_key { get; set; }
            public string primary_Value { get; set; }
        }

        private static tableInfo GetEntityTableInfo(EntityData entity)
        {
            tableInfo info = new tableInfo();
            if (entity.Tables.Count < 1)
                return null;
            info.tablename = entity.CurrentTableName;
            DataColumn[] columns = entity.Tables[0].PrimaryKey;
            info.primary_key = columns[0].ColumnName;
            info.primary_Value = entity.GetProperty(info.primary_key).ToString();
            return info;

        }

        private static tableInfo GetEntityTableInfo(PersistenceCapable pc)
        {
            return GetEntityTableInfo(pc.EntityData);
        }

        /// <summary>
        /// 保存主子表单据(只支持主表单一主键)
        /// </summary>
        /// <param name="EntitysMain">主实体</param>
        /// <param name="EntitysItem">子实体数组</param>
        /// <param name="type">对应this.FormState</param>
        /// <returns></returns>
        public static void SaveMaster(PersistenceCapable EntitysMain, PersistenceCapable[] EntitysItem, int type)
        {
            GetEntityTableInfo(EntitysMain);
            Transaction tm = pm.CurrentTransaction;
            tm.Begin();
            try
            {
                if (type == 2)
                {
                    tableInfo infomain = GetEntityTableInfo(EntitysMain);
                    tableInfo infoitem = GetEntityTableInfo(EntitysItem[0]);
                    string sqldel = string.Format("delete {0} where {1}='{2}'    delete {3} where {1}='{2}'", infomain.tablename, infomain.primary_key, infomain.primary_Value, infoitem.tablename);
                    pm.ExecuteNonQuery(sqldel);//执行SQL 
                }

                pm.PersistNewObject(EntitysMain);
                for (int i = 0; i < EntitysItem.Length; i++)
                {
                    pm.PersistNewObject(EntitysItem[i]);
                }
                tm.Commit();
            }
            catch (Exception ex)
            {
                tm.Rollback();
                HelperLog.Write(ex);
                throw ex;
            }
            finally
            {
                pm.Close();
            }
        }

        /// <summary>
        /// 保存主子表单据，实体只执行添加
        /// </summary>
        /// <param name="EntitysMain">主表实体</param>
        /// <param name="EntitysItem">子表实体数组</param>
        /// <param name="dic_start">添加实体前执行SQL数组</param>
        /// <param name="dic_end">添加实体后执行SQL数组</param>
        public static void SaveMaster(PersistenceCapable EntitysMain, PersistenceCapable[] EntitysItem, Dictionary<string, SqlParameter[]> dic_start, Dictionary<string, SqlParameter[]> dic_end = null)
        {
            GetEntityTableInfo(EntitysMain);
            Transaction tm = pm.CurrentTransaction;
            tm.Begin();
            try
            {
                foreach (string key in dic_start.Keys)
                {
                    pm.ExecuteNonQuery(key, (SqlParameter[])dic_start[key]);
                }

                pm.PersistNewObject(EntitysMain);
                for (int i = 0; i < EntitysItem.Length; i++)
                {
                    pm.PersistNewObject(EntitysItem[i]);
                }
                if (dic_end != null)
                {
                    foreach (string key in dic_end.Keys)
                    {
                        pm.ExecuteNonQuery(key, (SqlParameter[])dic_end[key]);
                    }
                }
                tm.Commit();
            }
            catch (Exception ex)
            {
                tm.Rollback();
                HelperLog.Write(ex);
                throw ex;
            }
            finally
            {
                pm.Close();
            }
        }

        /// <summary>
        /// 保存多个实体然后执行多个SQL语句
        /// </summary>
        /// <param name="entityItems">多个实体数组</param>
        /// <param name="multiSQL">多个SQL组合字典</param>
        public static void SaveEntitiesThenMutiSQL(PersistenceCapable[] entityItems, Dictionary<string, SqlParameter[]> multiSQL)
        {
            Transaction tm = pm.CurrentTransaction;
            tm.Begin();
            try
            {
                for (int i = 0; i < entityItems.Length; i++)
                {
                    pm.PersistNewObject(entityItems[i]);
                }
                foreach (string key in multiSQL.Keys)
                {
                    if (multiSQL[key] == null || multiSQL[key].Length == 0)
                    {
                        pm.ExecuteNonQuery(key);
                    }
                    else
                    {
                        pm.ExecuteNonQuery(key, (SqlParameter[])multiSQL[key]);
                    }
                }
                tm.Commit();
            }
            catch (Exception ex)
            {
                tm.Rollback();
                HelperLog.Write(ex);
                throw ex;
            }
            finally
            {
                pm.Close();
            }
        }

        /// <summary>
        /// 执行多个SQL语句然后保存多个实体
        /// </summary>
        /// <param name="multiSQL">多个SQL组合字典</param>
        /// <param name="entityItems">多个实体数组</param>
        public static void SaveMutiSQLThenEntities(Dictionary<string, SqlParameter[]> multiSQL, PersistenceCapable[] entityItems)
        {
            Transaction tm = pm.CurrentTransaction;
            tm.Begin();
            try
            {
                foreach (string key in multiSQL.Keys)
                {
                    if (multiSQL[key] == null || multiSQL[key].Length == 0)
                    {
                        pm.ExecuteNonQuery(key);
                    }
                    else
                    {
                        pm.ExecuteNonQuery(key, (SqlParameter[])multiSQL[key]);
                    }
                }
                for (int i = 0; i < entityItems.Length; i++)
                {
                    pm.PersistNewObject(entityItems[i]);
                }
                tm.Commit();
            }
            catch (Exception ex)
            {
                tm.Rollback();
                HelperLog.Write(ex);
                throw ex;
            }
            finally
            {
                pm.Close();
            }
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="dic">SQL语句和SQL参数字典</param>
        /// <returns>布尔值</returns>
        public static bool ExecuteNonQuery(Dictionary<string, SqlParameter[]> dic)
        {
            Transaction tm = pm.CurrentTransaction;
            tm.Begin();
            try
            {
                foreach (string key in dic.Keys)
                {
                    pm.ExecuteNonQuery(key, (SqlParameter[])dic[key]);
                }
                tm.Commit();
                return true;
            }
            catch (Exception ex)
            {
                tm.Rollback();
                HelperLog.Write(ex);
                return false;
            }
            finally
            {
                pm.Close();
            }
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>受影响行数</returns>
        public static int ExecuteNonQuery(string sql)
        {
            return pm.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>布尔值:true执行成功，受影响的行数大于1,false:受影响的行数小于1</returns>
        public static bool ExecuteNonQueryReturnBool(string sql)
        {
            return ExecuteNonQuery(sql) > 0;
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="para">SQL原生参数数组</param>
        /// <returns>布尔值:true执行成功，受影响的行数大于1,false:受影响的行数小于1</returns>
        public static bool ExecuteNonQueryReturnBool(string sql, params SqlParameter[] para)
        {
            return ExecuteNonQuery(sql, para) > 0;
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="para">SQL原生参数数组</param>
        /// <returns>受影响行数</returns>
        /// <remarks>lsl:将第二个参数SqlParameter[]类型修改为 params SqlParameter[]模式 by arison</remarks>
        public static int ExecuteNonQuery(string sql, params SqlParameter[] para)
        {
            return pm.ExecuteNonQuery(sql, para);
        }

        /// <summary>
        /// 返回单列单行值
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="para">SQL原生参数数组</param>
        /// <returns>单列单行值</returns>
        public static object ExecuteScalar(string sql, params SqlParameter[] para)
        {
            DataTable table = GetDataTable(sql, para);
            if (table == null || table.Rows.Count == 0 || table.Columns.Count != 1)
            {
                return null;
            }
            else
            {
                return table.Rows[0][0];
            }
        }

        /// <summary>
        /// 根据主单查询上下单数据主键
        /// </summary>
        /// <param name="table">查询表</param>
        /// <param name="requireId">表主键</param>
        /// <param name="serialColumnName">流水号列名</param>
        /// <param name="serial">流水号值</param>
        /// <param name="pnType">上下单枚举</param>
        /// <returns>上下单主键</returns>
        /// <remarks>20180418 wang建议不提供此功能!</remarks>
        public string GetMainSerialByPreOrNext(string table, string requireId, string serialColumnName, string serial, PreAndNextTypes pnType)
        {
            var resultId = string.Empty;
            var strsql = " SELECT TOP 1 " + requireId + " FROM " + table + " WITH(NOLOCK) ";
            if (pnType == PreAndNextTypes.Previous)
            {
                strsql += " WHERE " + serialColumnName + "<" + "'" + serial + "' ORDER BY " + serialColumnName + " DESC";
            }
            else if (pnType == PreAndNextTypes.Next)
            {
                strsql += " WHERE " + serialColumnName + ">" + "'" + serial + "' ORDER BY " + serialColumnName + " ASC";
            }
            var dt = SQL.GetDataTable(strsql);
            if (dt != null && dt.Rows.Count > 0)
            {
                resultId = dt.Rows[0][requireId].ToString();
            }
            return resultId;
        }


        /// <summary>
        /// 执行SQL脚本，返回是否执行成功
        /// </summary>
        /// <param name="script">脚本</param>
        /// <returns>True:成功,False失败</returns>
        public static bool RunSQLScript(string script)
        {
            try
            {
                SQL.ExecuteNonQuery(script);
            }
            catch (Exception ex)
            {
                HelperLog.Write(ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 启用原始的单独数据库链接操作数据库
        /// </summary>
        /// <param name="sql">普通的操作SQL语句</param>
        public static void ExecOriginalSql(string sql)
        {
            using (SqlConnection conn = new SqlConnection(ApplicationConfiguration.DefaultDatabaseProperty.ConnectionString))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand();
                comm.Connection = conn;
                comm.CommandText = sql;
                comm.CommandType = CommandType.Text;
                comm.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 启用原始的单独数据库链接操作数据库
        /// </summary>
        /// <param name="sql">读取单个表格的SQL</param>
        /// <returns></returns>
        public static DataTable GetTableByOriginalSql(string sql)
        {
            using (SqlConnection conn = new SqlConnection(ApplicationConfiguration.DefaultDatabaseProperty.ConnectionString))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand();
                comm.Connection = conn;
                comm.CommandText = sql;
                comm.CommandType = CommandType.Text;
                SqlDataAdapter adap = new SqlDataAdapter();
                adap.SelectCommand = comm;
                DataSet ds = new DataSet();
                adap.Fill(ds);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
                return null;
            }
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="dt">表格</param>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public static bool BatchInsertBySqlBulkCopy(DataTable dt, string tableName)
        {
            try
            {
                using (SqlConnection destinationConnection = new SqlConnection(ApplicationConfiguration.DefaultDatabaseProperty.ConnectionString))
                {
                    destinationConnection.Open();

                    using (SqlTransaction transaction = destinationConnection.BeginTransaction())
                    {
                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(destinationConnection, SqlBulkCopyOptions.Default,
                                   transaction))
                        {
                            bulkCopy.BatchSize = dt.Rows.Count;
                            bulkCopy.DestinationTableName = tableName;

                            try
                            {
                                bulkCopy.WriteToServer(dt);
                                transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                HelperLog.Write(ex);
                                transaction.Rollback();
                                return false;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                HelperLog.Write(e);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="dt">数据表格</param>
        /// <param name="tableName">表名</param>
        /// <param name="pm">数据操作</param>
        /// <returns></returns>
        public static bool BatchInsertBySqlBulkCopy(DataTable dt, string tableName, PersistenceManager pm)
        {
            try
            {
                if (!(pm.GetConnection() is SqlConnection)) return false;
                var destinationConnection = pm.GetConnection() as SqlConnection;
                if (destinationConnection.State.Equals(ConnectionState.Closed))
                    destinationConnection.Open();
                var transaction = pm.CurrentTransaction.GetTransaction as SqlTransaction;

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(destinationConnection, SqlBulkCopyOptions.Default,
                           transaction))
                {
                    bulkCopy.BatchSize = dt.Rows.Count;
                    bulkCopy.DestinationTableName = tableName;

                    try
                    {
                        bulkCopy.WriteToServer(dt);
                    }
                    catch (Exception ex)
                    {
                        HelperLog.Write(ex);
                        return false;
                    }
                }


            }
            catch (Exception e)
            {
                HelperLog.Write(e);
                return false;
            }
            return true;
        }
    }
}
