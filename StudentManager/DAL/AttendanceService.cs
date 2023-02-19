using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

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

        
    }
}
