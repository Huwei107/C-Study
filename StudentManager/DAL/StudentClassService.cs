using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Models;

namespace DAL
{
    /// <summary>
    /// 班级数据访问类
    /// </summary>
    public class StudentClassService
    {
        /// <summary>
        /// 获取所有班级对象
        /// </summary>
        /// <returns></returns>
        public List<StudentClass> GetAllClass()
        {
            string sql = string.Format(@"select ClassName,ClassId from StudentClass");
            SqlDataReader objReader = SQLHelper.GetReader(sql);
            List<StudentClass> list = new List<StudentClass>();
            while (objReader.Read())
            {
                list.Add(new StudentClass()
                {
                    ClassId = Convert.ToInt32(objReader["ClassId"]),
                    ClassName = Convert.ToString(objReader["ClassName"])
                });
            }
            objReader.Close();
            return list;
        }

        /// <summary>
        /// 获取所有的班级存放在数据集里
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllClasses()
        {
            string sql = string.Format(@"select ClassName,ClassId from StudentClass");
            return SQLHelper.GetDataSet(sql);
        }
    }
}
