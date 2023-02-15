using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using DAL;

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
    }
}
