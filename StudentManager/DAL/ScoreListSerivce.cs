using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Models;

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
        public List<Student> QueryScoreList(string className)
        {
            string sql = string.Format(@"select a.StudentId,a.StudentName,a.Gender,b.ClassName,c.CSharp,c.SQLServerDB
                                        from Students a
                                        inner join StudentClass b on b.ClassId=a.ClassId
                                        inner join ScoreList c on c.StudentId=a.StudentId");
            if (className != null && className.Length != 0)
            {
                sql += string.Format(@" where b.ClassName='{0}'", className);
            }
            SqlDataReader objReader = SQLHelper.GetReader(sql);
            List<Student> list = new List<Student>();
            while (objReader.Read())
            {
                list.Add(new Student()
                {
                    StudentId=Convert.ToInt32(objReader["StudentId"]),
                    StudentName = objReader["StudentName"].ToString(),
                    ClassName=objReader["ClassName"].ToString(),
                    Gender = objReader["Gender"].ToString(),
                    CSharp=Convert.ToInt32(objReader["CSharp"]),
                    SQLServerDB=Convert.ToInt32(objReader["SQLServerDB"])
                });
            }
            objReader.Close();
            return list;
        }
    }
}
