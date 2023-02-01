using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    /// <summary>
    /// 学员信息数据访问类
    /// </summary>
    public class StudentService
    {
        #region 添加学员对象
        /// <summary>
        /// 判断当前身份证号是否已经存在
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public bool IsIdNoExisted(string studentId)
        {
            string sql = string.Format(@"select count(*) from Students where StudentIdNo={0}", studentId);
            int result = Convert.ToInt32(SQLHelper.GetSingleResult(sql));
            if (result == 1)
                return true;
            else return false;
        }

        /// <summary>
        /// 添加学员
        /// </summary>
        /// <param name="objStudent"></param>
        /// <returns></returns>
        public int AddStudent(Student objStudent)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat(@"insert into Students(StudentName,Gender,Birthday,StudentIdNo,Age,PhoneNumber,StudentAddress,CardNo,ClassId)");
            sqlBuilder.AppendFormat(@" values('{0}','{1}','{2}',{3},{4},'{5}','{6}','{7}',{8})", objStudent.StudentName, objStudent.Gender,
                    objStudent.Birthday, objStudent.StudentIdNo, objStudent.Age, objStudent.PhoneNumber, objStudent.StudentAddress, objStudent.CardNo, objStudent.ClassId);
            try
            {
                return SQLHelper.Update(sqlBuilder.ToString());
            }
            catch (SqlException ex)
            {
                throw new Exception("数据库操作出现异常！具体信息：" + ex.Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 查询学员
        /// <summary>
        /// 根据班级查询学员信息
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public List<StudentExt> GetStudentByClass(string className)
        {
            string sql = string.Format(@"select a.StudentId,a.StudentName,a.Gender,a.Birthday,b.ClassName from Students a 
                                        inner join StudentClass b on a.ClassId = b.ClassId
                                        where b.ClassName = '{0}'", className);
            SqlDataReader objReader = SQLHelper.GetReader(sql);
            List<StudentExt> list = new List<StudentExt>();
            while (objReader.Read())
            {
                list.Add(new StudentExt()
                {
                    StudentId =Convert.ToInt32(objReader["StudentId"]),
                    StudentName=objReader["StudentName"].ToString(),
                    Gender=objReader["Gender"].ToString(),
                    Birthday=Convert.ToDateTime(objReader["Birthday"]),
                    ClassName=objReader["ClassName"].ToString()
                });
            }
            objReader.Close();
            return list;
        }

        /// <summary>
        /// 根据学号查询学员对象
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public StudentExt GetStudentById(string studentId)
        {
            string sql = string.Format(@"select StudentId,StudentName,Gender,Birthday,ClassName,StudentIdNo,PhoneNumber,StudentAddress,CardNo from Students
                                         inner join StudentClass on Students.ClassId = StudentClass.ClassId
                                         where StudentId={0}", studentId);
            SqlDataReader objReader = SQLHelper.GetReader(sql);
            StudentExt objStudentExt = null;
            while (objReader.Read())
            {
                objStudentExt = new StudentExt()
                {
                    StudentId = Convert.ToInt32(objReader["StudentId"]),
                    StudentName = objReader["StudentName"].ToString(),
                    Gender = objReader["Gender"].ToString(),
                    Birthday = Convert.ToDateTime(objReader["Birthday"]),
                    ClassName = objReader["ClassName"].ToString(),
                    CardNo=objReader["CardNo"].ToString(),
                    StudentIdNo=objReader["StudentIdNo"].ToString(),
                    PhoneNumber=objReader["PhoneNumber"].ToString(),
                    StudentAddress=objReader["StudentAddress"].ToString()
                };
            }
            objReader.Close();
            return objStudentExt;
        }


        #endregion

        #region 修改学员
        /// <summary>
        /// 修改学员时判断身份证号是否和其他学员重复
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public bool IsIdNoExisted(string studentIdNo, string studentId)
        {
            string sql = string.Format(@"select count(*) from Students where StudentIdNo={0} and StudentId<>{1}", studentIdNo, studentId);
            int result = Convert.ToInt32(SQLHelper.GetSingleResult(sql));
            if (result == 1)
                return true;
            else return false;
        }

        /// <summary>
        /// 修改学员对象
        /// </summary>
        /// <param name="objStudent"></param>
        /// <returns></returns>
        public int ModifyStudent(Student objStudent)
        {
            string sql = string.Format(@"update Students set StudentName='{0}',Gender='{1}',Birthday='{2}',StudentIdNo={3},Age={4},PhoneNumber={5},
                                        StudentAddress='{6}',CardNo='{7}',ClassId={8} where StudentId={8}", objStudent.StudentName, objStudent.Gender,
                                                                                                           objStudent.Birthday, objStudent.StudentIdNo,
                                                                                                           objStudent.Age, objStudent.PhoneNumber,
                                                                                                           objStudent.StudentAddress, objStudent.CardNo,
                                                                                                           objStudent.StudentId);
            try
            {
                return SQLHelper.Update(sql);
            }
            catch (SqlException ex)
            {
                throw new Exception("数据库操作出现异常！具体信息：" + ex.Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
