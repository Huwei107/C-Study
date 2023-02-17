using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Models;

namespace DAL
{
    /// <summary>
    /// 成绩表数据访问类
    /// </summary>
    public class ScoreListSerive
    {
        /// <summary>
        /// 获取所有成绩
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllScoreList()
        {
            string sql = string.Format(@"select distinct(a.StudentId), a.StudentName,a.Gender,a.PhoneNumber,b.ClassName,c.CSharp,c.SQLServerDB
                                         from Students a
                                         inner join StudentClass b on a.ClassId = b.ClassId
                                         inner join ScoreList c on c.StudentId = a.StudentId");
            return SQLHelper.GetDataSet(sql);
        }
    }
}
