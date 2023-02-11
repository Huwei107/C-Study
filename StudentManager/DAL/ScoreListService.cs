using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using DAL;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    /// <summary>
    /// 成绩信息数据访问类
    /// </summary>
    public class ScoreListService
    {
        #region 成绩查询
        /// <summary>
        /// 根据班级查询考试成绩
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public List<StudentExt> GetScoreList(string className)
        {
            string sql = string.Format(@"select a.StudentId,a.StudentName,b.ClassName,c.CSharp,c.SQLServerDB from Students a
                                        inner join StudentClass b on a.ClassId = b.ClassId
                                        inner join ScoreList c on c.StudentId = a.StudentId");
            if (className != null && className.Length != 0)
            {
                sql += string.Format(@" where b.ClassName = '{0}'", className);
            }
            SqlDataReader objReader = SQLHelper.GetReader(sql);
            List<StudentExt> list = new List<StudentExt>();
            while (objReader.Read())
            {
                list.Add(new StudentExt()
                {
                    StudentId = Convert.ToInt32(objReader["StudentId"]),
                    StudentName = objReader["StudentName"].ToString(),
                    ClassName = objReader["ClassName"].ToString(),
                    CSharp = Convert.ToInt32(objReader["CSharp"]),
                    SQLServerDB = Convert.ToInt32(objReader["SQLServerDB"])
                });
            }
            objReader.Close();
            return list;
        }

        /// <summary>
        /// 获取全部考试统计信息
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetScoreInfo()
        {
            string sql = string.Format(@"select count(*) as stuCount, avg(CSharp) as avgCsharp, avg(SQLServerDB) as avgDB from ScoreList;");
            sql += string.Format(@"select count(*) as absentCount from Students where StudentId not in (select StudentId from ScoreList)");
            Dictionary<string, string> scoreInfo = null;
            SqlDataReader objReader = SQLHelper.GetReader(sql);
            if (objReader.Read())
            {
                scoreInfo = new Dictionary<string, string>();
                scoreInfo.Add("stuCount", objReader["stuCount"].ToString());
                scoreInfo.Add("avgCsharp", objReader["avgCsharp"].ToString());
                scoreInfo.Add("avgDB", objReader["avgDB"].ToString());
            }
            if (objReader.NextResult())
            {
                if (objReader.Read())
                {
                    scoreInfo.Add("absentCount", objReader["absentCount"].ToString());
                }
            }
            objReader.Close();
            return scoreInfo;
        }

        public List<string> GetAbsentList()
        {
            string sql = string.Format(@"select StudentName from Students where StudentId not in (select StudentId from ScoreList)");
            SqlDataReader objReader = SQLHelper.GetReader(sql);
            List<string> list = new List<string>();
            while (objReader.Read())
            {
                list.Add(objReader["StudentName"].ToString());
            }
            objReader.Close();
            return list;
        }

        #endregion

        #region 基于DataSet的数据查询
        /// <summary>
        /// 获取所有考试信息 
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllScoreList()
        {
            string sql = string.Format(@"select a.StudentId, a.StudentName, b.ClassName, c.CSharp, c.SQLServerDB
                                         from Students a
                                         inner join StudentClass b on b.ClassId = a.ClassId
                                         inner join ScoreList c on c.StudentId = a.StudentId");
            return SQLHelper.GetDataSet(sql);
        }
        #endregion
    }
}
