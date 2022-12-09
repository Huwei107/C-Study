using ADO_NETDemo.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
        public int AddStudent(Student objStudent)
        {
            string sql = string.Format(@"insert into Students (StudentName, Gender, Birthday,StudentIdNo, Age, 
                                        PhoneNumber, StudentAddress, ClassId)");
            sql += "values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')";
            sql = string.Format(sql, objStudent.StudentName, objStudent.Gender, objStudent.Birthday, objStudent.StudentIdNo, 
                                objStudent.Age, objStudent.PhoneNumber, objStudent.StudentAddress, objStudent.ClassId);
            return SQLHelper.Update(sql);
        }

        public int GetStuCountByClassId(string classId)
        {
            string sql = string.Format(@"select count(*) from Students where ClassId = '{0}'", classId);
            return Convert.ToInt32(SQLHelper.GetSingleResult(sql));
        }

        public Student GetStudentById(string studentId)
        {
            string sql = string.Format(@"select StudentName, Gender, Birthday,StudentIdNo, 
                                         StudentAddress from Students where StudentId = '{0}'", studentId);
            SqlDataReader objReader = SQLHelper.GetReader(sql);
            Student objStudent = null;
            if (objReader.Read())
            {
                objStudent = new Student();
                objStudent.StudentName = objReader["StudentName"].ToString();
                objStudent.Gender = objReader["Gender"].ToString();
                objStudent.Birthday = Convert.ToDateTime(objReader["Birthday"]);
                objStudent.StudentIdNo = Convert.ToDecimal(objReader["StudentIdNo"]);
                objStudent.StudentAddress = objReader["StudentAddress"].ToString();
            }
            objReader.Close();
            return objStudent;
        }
    }
}
