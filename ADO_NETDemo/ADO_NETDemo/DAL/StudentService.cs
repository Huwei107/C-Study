using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO_NETDemo.DAL
{
    /// <summary>
    /// 学员信息数据访问类
    /// </summary>
    public class StudentService
    {
        /// <summary>
        /// 添加学员对象
        /// </summary>
        /// <param name="stuName"></param>
        /// <param name="gender"></param>
        /// <param name="birthday"></param>
        /// <param name="stuIdNo"></param>
        /// <param name="age"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="stuAddress"></param>
        /// <param name="classId"></param>
        /// <returns></returns>
        public int AddStudent(string stuName, string gender, DateTime birthday, string stuIdNo,
            int age, string phoneNumber, string stuAddress, int classId)
        {
            string sql = string.Format(@"insert into Students (StudentName, Gender, Birthday,StudentIdNo, Age, 
                                        PhoneNumber, StudentAddress, ClassId)");
            sql += "values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')";
            sql = string.Format(sql, stuName, gender, birthday, stuIdNo, age, phoneNumber, stuAddress, classId);
            return SQLHelper.Update(sql);
        }

        public int GetStuCountByClassId(string classId)
        {
            string sql = string.Format(@"select count(*) from Students where ClassId = '{0}'", classId);
            return Convert.ToInt32(SQLHelper.GetSingleResult(sql));
        }
    }
}
