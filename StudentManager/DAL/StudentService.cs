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
    /// 学员管理数据访问类
    /// </summary>
    public class StudentService
    {
        /// <summary>
        /// 判断身份证号是否已存在
        /// </summary>
        /// <param name="studentIdNo"></param>
        /// <returns></returns>
        public bool IsIdNoExisted(string studentIdNo)
        {
            string sql = string.Format(@"select count(*) from Students where StudentIdNo={0}", studentIdNo);
            int result = Convert.ToInt32(SQLHelper.GetSingleResult(sql));
            if (result == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 判断考勤卡号是否存在
        /// </summary>
        /// <param name="cardNo"></param>
        /// <returns></returns>
        public bool IsCardNoExisted(string cardNo)
        {
            string sql = string.Format(@"select count(*) from Students where CardNo ='{0}'", cardNo);
            int result = Convert.ToInt32(SQLHelper.GetSingleResult(sql));
            if (result == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 将学员对象保存到数据库
        /// </summary>
        /// <param name="objStudent"></param>
        /// <returns></returns>
        public int AddStudent(Student objStudent)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(string.Format("insert into Students (StudentName,Gender,Birthday,Age,StudentIdNo,CardNo,PhoneNumber,StudentAddress,ClassId,StuImage)"));
            sqlBuilder.Append(string.Format(" values('{0}','{1}','{2}',{3},{4},'{5}','{6}','{7}',{8},'{9}');select @@identity", objStudent.StudentName, objStudent.Gender,
                                                objStudent.Birthday.ToString("yyyy-MM-dd"),objStudent.Age,objStudent.StudentIdNo,objStudent.CardNo,objStudent.PhoneNumber,
                                                objStudent.StudentAddress,objStudent.ClassId,objStudent.StuImage));
            try
            {
                return Convert.ToInt32(SQLHelper.GetSingleResult(sqlBuilder.ToString()));
            }
            catch (Exception ex)
            {
                
                throw new Exception("发生数据访问异常："+ex.Message);
            }
        }

        /// <summary>
        /// 根据班级查询学生信息
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public List<Student> GetStudentByClass(string className)
        {
            string sql = string.Format(@"select StudentId,StudentName,Gender,Birthday,StudentIdNo,CardNo,PhoneNumber,ClassName from Students
                                         inner join StudentClass on Students.ClassId = StudentClass.ClassId
                                         where ClassName='{0}'", className);
            SqlDataReader objReader = SQLHelper.GetReader(sql);
            List<Student> stuList = new List<Student>();
            while (objReader.Read())
            {
                stuList.Add(new Student()
                {
                    StudentId=Convert.ToInt32(objReader["StudentId"]),
                    StudentName=objReader["StudentName"].ToString(),
                    Gender=objReader["Gender"].ToString(),
                    PhoneNumber=objReader["PhoneNumber"].ToString(),
                    Birthday=Convert.ToDateTime(objReader["Birthday"].ToString()),
                    StudentIdNo=objReader["StudentIdNo"].ToString(),
                    ClassName=objReader["ClassName"].ToString()
                });
            }
            objReader.Close();
            return stuList;
        }

        /// <summary>
        /// 根据学号查询学员对象
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public Student GetStudentById(string studentId)
        {
            string sql = string.Format(@"select StudentId,StudentName,Gender,Birthday,StudentIdNo,CardNo,PhoneNumber,
                                                StuImage,StudentAddress,ClassName
                                         from Students
                                         inner join StudentClass on Students.ClassId = StudentClass.ClassId
                                         where StudentId='{0}'", studentId);
            SqlDataReader objReader = SQLHelper.GetReader(sql);
            Student objStudent = null;
            if (objReader.Read())
            {
                objStudent = new Student()
                {
                    StudentId = Convert.ToInt32(objReader["StudentId"]),
                    StudentName = objReader["StudentName"].ToString(),
                    Gender = objReader["Gender"].ToString(),
                    PhoneNumber = objReader["PhoneNumber"].ToString(),
                    Birthday = Convert.ToDateTime(objReader["Birthday"].ToString()),
                    StudentIdNo = objReader["StudentIdNo"].ToString(),
                    ClassName = objReader["ClassName"].ToString(),
                    StudentAddress=objReader["StudentAddress"].ToString(),
                    CardNo =objReader["CardNo"].ToString(),
                    StuImage = objReader["StuImage"]==null?"":objReader["StuImage"].ToString()
                };
            }
            objReader.Close();
            return objStudent;
        }
    }
}
