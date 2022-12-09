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
                objStudent = new Student()
                {
                    StudentName = objReader["StudentName"].ToString(),
                    Gender = objReader["Gender"].ToString(),
                    Birthday = Convert.ToDateTime(objReader["Birthday"]),
                    StudentIdNo = Convert.ToDecimal(objReader["StudentIdNo"]),
                    StudentAddress = objReader["StudentAddress"].ToString()
                };
            }
            objReader.Close();
            return objStudent;
        }

        public List<Student> GetAllStudents()
        {
            string sql = string.Format(@"select StudentName, Gender, Birthday,StudentIdNo, StudentAddress from Students");
            SqlDataReader objReader = SQLHelper.GetReader(sql);
            List<Student> stuList = new List<Student>();
            //while (objReader.Read())
            //{
            //    Student objStudent = new Student()
            //    {
            //        StudentName = objReader["StudentName"].ToString(),
            //        Gender = objReader["Gender"].ToString(),
            //        Birthday = Convert.ToDateTime(objReader["Birthday"]),
            //        StudentIdNo = Convert.ToDecimal(objReader["StudentIdNo"]),
            //        StudentAddress = objReader["StudentAddress"].ToString()
            //    };
            //    stuList.Add(objStudent);
            //}
            while (objReader.Read())
            {
                stuList.Add(new Student()
                {
                    StudentName = objReader["StudentName"].ToString(),
                    Gender = objReader["Gender"].ToString(),
                    Birthday = Convert.ToDateTime(objReader["Birthday"]),
                    StudentIdNo = Convert.ToDecimal(objReader["StudentIdNo"]),
                    StudentAddress = objReader["StudentAddress"].ToString()
                });
            }
            objReader.Close();
            return stuList;
        }

        public List<StudentExt> GetStudentExt()
        {
            string sql = string.Format(@"select Students.StudentId, StudentName, ClassName, CSharp, SQLServerDB from Students
                                         inner join StudentClass on StudentClass.ClassId = Students.ClassId
                                         inner join ScoreList on ScoreList.StudentId = Student.StudentId");
            SqlDataReader objReader = SQLHelper.GetReader(sql);
            List<StudentExt> exList = new List<StudentExt>();
            while (objReader.Read())
            {
                StudentExt ext = new StudentExt();
                ext.ObjStudent.StudentId = Convert.ToInt32(objReader["StudentId"]);
                ext.ObjStudent.StudentName = objReader["StudentName"].ToString();
                ext.ObjClass.ClassName = objReader["ClassName"].ToString();
                ext.objScore.CSharp = Convert.ToInt32(objReader["CSharp"]);
                ext.objScore.SQLServerDB = Convert.ToInt32(objReader["SQLServerDB"]);
                exList.Add(ext);
            }

        }
    }
}
