using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Models;

namespace DAL
{
    public class StudentService
    {
        /// <summary>
        /// 判断身份证号是否已存在
        /// </summary>
        /// <param name="studentIdNo"></param>
        /// <returns></returns>
        public bool IsIdNoExisted(string studentIdNo)
        {
            string sql = string.Format(@"select count(*) from Students where StudentIdNo=@StudentIdNo");
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@StudentIdNo",studentIdNo)
            };
            int result = Convert.ToInt32(SQLHelper.GetSingleResult(sql, param, false));
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
        /// 调用存储过程添加学员对象
        /// </summary>
        /// <param name="objStudent">对象</param>
        /// <returns>返回当前学员对象的标识列</returns>
        public int AddStudent(Student objStudent)
        {
            //【1】封装参数
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@StudentIdNo",objStudent.StudentIdNo),
                new SqlParameter("@Gender",objStudent.Gender),
                new SqlParameter("@Birthday",objStudent.Birthday),
                new SqlParameter("@StudentName",objStudent.StudentName),
                new SqlParameter("@StudentAddress",objStudent.StudentAddress),
                new SqlParameter("@PhoneNumber",objStudent.PhoneNumber),
                new SqlParameter("@ClassId",objStudent.ClassId)
            };
            //【2】调用存储过程
            try
            {
                return Convert.ToInt32(SQLHelper.GetSingleResult("usp_AddStudent", param, true));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
