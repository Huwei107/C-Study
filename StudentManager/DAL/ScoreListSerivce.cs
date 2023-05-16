using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Models;
using Models.ExtendModels;

namespace DAL
{
    /// <summary>
    /// 成绩表数据访问类
    /// </summary>
    public class ScoreListSerivce
    {
        /// <summary>
        /// 获取所有成绩
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllScoreList()
        {
            string sql = string.Format(@"select distinct(a.StudentId), a.StudentName,a.Gender,a.PhoneNumber,b.ClassName,c.CSharp,c.SQLServerDB
                                         from Students a
                                         inner join StudentClass b on a.ClassId = b.ClassId
                                         inner join ScoreList c on c.StudentId = a.StudentId");
            return SQLHelper.GetDataSet(sql);
        }

        /// <summary>
        /// 根据班级查询成绩
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
//        public List<Student> QueryScoreList(string className)
//        {
//            string sql = string.Format(@"select distinct(a.StudentId),a.StudentName,a.Gender,b.ClassName,c.CSharp,c.SQLServerDB
//                                        from Students a
//                                        inner join StudentClass b on b.ClassId=a.ClassId
//                                        inner join ScoreList c on c.StudentId=a.StudentId");
//            if (className != null && className.Length != 0)
//            {
//                sql += string.Format(@" where b.ClassName='{0}'", className);
//            }
//            SqlDataReader objReader = SQLHelper.GetReader(sql);
//            List<Student> list = new List<Student>();
//            while (objReader.Read())
//            {
//                list.Add(new Student()
//                {
//                    StudentId=Convert.ToInt32(objReader["StudentId"]),
//                    StudentName = objReader["StudentName"].ToString(),
//                    ClassName=objReader["ClassName"].ToString(),
//                    Gender = objReader["Gender"].ToString(),
//                    CSharp=Convert.ToInt32(objReader["CSharp"]),
//                    SQLServerDB=Convert.ToInt32(objReader["SQLServerDB"])
//                });
//            }
//            objReader.Close();
//            return list;
//        }
        public List<StudentExt> QueryScoreList(string className)
        {
            string sql = string.Format(@"select distinct(a.StudentId),a.StudentName,a.Gender,b.ClassName,c.CSharp,c.SQLServerDB
                                        from Students a
                                        inner join StudentClass b on b.ClassId=a.ClassId
                                        inner join ScoreList c on c.StudentId=a.StudentId");
            if (className != null && className.Length != 0)
            {
                sql += string.Format(@" where b.ClassName='{0}'", className);
            }
            SqlDataReader objReader = SQLHelper.GetReader(sql);
            List<StudentExt> list = new List<StudentExt>();
            while (objReader.Read())
            {
                list.Add(new StudentExt()
                {
                    StudentObj=new Student()
                    {
                        StudentId = Convert.ToInt32(objReader["StudentId"]),
                        StudentName = objReader["StudentName"].ToString(),
                        Gender = objReader["Gender"].ToString()
                    },
                    ClassObj = new StudentClass()
                    {
                        ClassName = objReader["ClassName"].ToString()
                    },
                    ScoreObj = new ScoreList()
                    {
                        CSharp = Convert.ToInt32(objReader["CSharp"]),
                        SQLServerDB = Convert.ToInt32(objReader["SQLServerDB"])
                    }
                });
            }
            objReader.Close();
            return list;
        }

        /// <summary>
        /// 查询考试总人数，缺考人数，平均成绩
        /// </summary>
        /// <param name="classId"></param>
        /// <returns></returns>
        public Dictionary<string, string> QueryScoreInfo(string classId)
        {
            string sql = string.Format(@"select count(*) AS stuCount, avg(CSharp) AS avgCSharp, avg(SQLServerDB) AS avgDB from ScoreList
                                        inner join Students on Students.StudentId = ScoreList.StudentId");
            if (classId != null && classId.Length != 0)
            {
                sql += string.Format(@" where ClassId={0}", classId);
            }
            sql += string.Format(@";select count(*) AS absentCount from Students where StudentId not in (
                                  select StudentId from ScoreList)");
            if (classId != null && classId.Length != 0)
            {
                sql += string.Format(@" and ClassId={0}", classId);
            }
            SqlDataReader objReader = SQLHelper.GetReader(sql);
            Dictionary<string, string> scoreInfo = null;
            while (objReader.Read())//读取考试统计结果
            {
                scoreInfo = new Dictionary<string, string>();
                scoreInfo.Add("stuCount", objReader["stuCount"].ToString());
                scoreInfo.Add("avgCSharp", objReader["avgCSharp"].ToString());
                scoreInfo.Add("avgDB", objReader["avgDB"].ToString());
            }
            //读取缺考人员
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

        /// <summary>
        /// (根据班级)查询全校缺考人员的姓名
        /// </summary>
        /// <param name="classId"></param>
        /// <returns></returns>
        public List<string> QueryAbsentList(string classId)
        {
            string sql = string.Format(@"select StudentName from Students where StudentId not in (select StudentId from ScoreList)");
            if (classId != null && classId.Length != 0)
            {
                sql += string.Format(@" and ClassId={0}", classId);
            }
            SqlDataReader objReader = SQLHelper.GetReader(sql);
            List<string> list = new List<string>();
            while (objReader.Read())
            {
                list.Add(objReader["StudentName"].ToString());
            }
            objReader.Close();
            return list;
        }

        #region 调用存储过程统计考试信息
        public List<StudentExt> GetScoreInfo(string className, out Dictionary<string, string> dicParam, out List<string> absentList)
        {
            //定义参数
            SqlParameter inputClassName = new SqlParameter("@className", className);
            inputClassName.Direction = ParameterDirection.Input;//设置参数为输入类型

            SqlParameter outStuCount = new SqlParameter("@stuCount", SqlDbType.Int);
            outStuCount.Direction = ParameterDirection.Output;

            SqlParameter outAbsentCount = new SqlParameter("@absentCount", SqlDbType.Int);
            outAbsentCount.Direction = ParameterDirection.Output;

            SqlParameter outAvgDB = new SqlParameter("@avgDB", SqlDbType.Int);
            outAvgDB.Direction = ParameterDirection.Output;

            SqlParameter outAvgCSharp = new SqlParameter("@avgCSharp", SqlDbType.Int);
            outAvgCSharp.Direction = ParameterDirection.Output;

            //执行查询
            SqlParameter[] param = new SqlParameter[] { inputClassName, outStuCount, outAbsentCount, outAvgDB, outAvgCSharp };
            SqlDataReader objReader = SQLHelper.GetReaderProce("usp_ScoreQuery", param);
            //读取考试成绩列表
            List<StudentExt> scoreList = new List<StudentExt>();
            while (objReader.Read())
            {
                scoreList.Add(new StudentExt()
                {
                    StudentObj = new Student()
                    {
                        StudentId = Convert.ToInt32(objReader["StudentId"]),
                        StudentName = Convert.ToString(objReader["StudentName"]),
                        ClassName = Convert.ToString(objReader["ClassName"]),
                        CSharp = Convert.ToInt32(objReader["CSharp"]),
                        SQLServerDB = Convert.ToInt32(objReader["SQLServerDB"])
                    }
                });
            }
            //读取缺考人员列表
            List<string> absentName = new List<string>();
            if (objReader.NextResult())
            {
                while (objReader.Read())
                {
                    absentName.Add(objReader["StudentName"].ToString());
                }
            }
            //关闭读取器
            objReader.Close();
            //获取输出参数
            Dictionary<string, string> outDicParam = new Dictionary<string, string>();
            outDicParam["stuCount"] = outStuCount.Value.ToString();
            outDicParam["absentCount"] = outAbsentCount.Value.ToString();
            outDicParam["avgDB"] = outAvgDB.Value.ToString();
            outDicParam["avgCSharp"] = outAvgCSharp.Value.ToString();
            //返回输出参数和结果
            dicParam = outDicParam;
            absentList = absentName;
            return scoreList;
        }
        #endregion 
    }
}
