using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using System.Data.SqlClient;
using System.Data;

namespace DAL
{
    /// <summary>
    /// 考勤数据访问类
    /// </summary>
    public class AttendanceService
    {
        /// <summary>
        /// 添加考勤记录
        /// </summary>
        /// <param name="cardNo"></param>
        /// <returns></returns>
        public string AddRecord(string cardNo)
        {
            string sql = string.Format("Insert into Attendance (CardNo) values ('{0}')", cardNo);
            try
            {
                SQLHelper.Update(sql);
                return "success";
            }
            catch (Exception ex)
            {
                return "打卡失败！系统出现问题，请联系管理员！";
            }
        }

        /// <summary>
        /// 获取学员总数（应到人数）
        /// </summary>
        /// <returns></returns>
        public int GetStudentCount()
        {
            string sql = string.Format(@"select count(*) from Students");
            return Convert.ToInt32(SQLHelper.GetSingleResult(sql));
        }

        /// <summary>
        /// 获取当天已签到人数
        /// </summary>
        /// <returns></returns>
        public int GetSignStudents()
        {
            DateTime dTime = Convert.ToDateTime(SQLHelper.GetServerTime().ToShortDateString());
            string sql = string.Format(@"select count(distinct(CardNo)) from Attendance where DTime between '{0}' and '{1}'", dTime, dTime.AddDays(1.0));
            return Convert.ToInt32(SQLHelper.GetSingleResult(sql));
        }

        public int GetSignStudents(DateTime beginTime, DateTime endTime)
        {
            string sql = string.Format(@"select count(distinct(CardNo)) from Attendance where DTime between '{0}' and '{1}'", beginTime, endTime);
            return Convert.ToInt32(SQLHelper.GetSingleResult(sql));
        }

        /// <summary>
        /// 根据日期和姓名查询考勤信息
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Student> GetStudentBySignDate(DateTime beginTime, DateTime endTime, string name)
        {
            string sql = string.Format(@"select Distinct(StudentId),DTime,StudentName,Gender,ClassName,Attendance.CardNo from Students
                                         inner join StudentClass on StudentClass.ClassId = Students.ClassId
                                         inner join Attendance on Students.CardNo = Attendance.CardNo");
            sql += string.Format(@" where DTime between '{0}' and '{1}'", beginTime, endTime);
            if (name != null && name.Length != 0)
            {
                sql += string.Format(@" and StudentName like '%{0}%'", name);
            }
            sql += " Order by DTime ASC";
            SqlDataReader objReader = SQLHelper.GetReader(sql);
            List<Student> list = new List<Student>();
            while (objReader.Read())
            {
                list.Add(new Student()
                {
                    StudentId = Convert.ToInt32(objReader["StudentId"]),
                    StudentName = objReader["StudentName"].ToString(),
                    ClassName = objReader["ClassName"].ToString(),
                    Gender = objReader["Gender"].ToString(),
                    SignTime = Convert.ToDateTime(objReader["DTime"]),
                    CardNo = objReader["CardNo"].ToString()
                });
            }
            objReader.Close();
            return list;
        }
    }
}
