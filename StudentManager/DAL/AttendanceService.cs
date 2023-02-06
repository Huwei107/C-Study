using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    /// <summary>
    /// 打卡信息数据访问类
    /// </summary>
    public class AttendanceService
    {
        public string AddRecord(string cardNo)
        {
            string sql = string.Format(@"insert into Attendance (CardNo) values ('{0}')", cardNo);
            try
            {
                SQLHelper.Update(sql);
                return "success";
            }
            catch (Exception ex)
            {
                return "打卡失败！请联系管理员！" + ex.Message;
            }
        }
    }
}
