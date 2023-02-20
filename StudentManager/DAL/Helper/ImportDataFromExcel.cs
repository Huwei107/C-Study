using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using DAL;
using System.Data;

namespace DAL
{
    /// <summary>
    /// 从Excel中导入数据
    /// </summary>
    public class ImportDataFromExcel
    {
        /// <summary>
        /// 将选择的Excel数据表查询后封装成对象集合
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public List<Student> GetStudentByExcel(string path)
        {
            List<Student> list = new List<Student>();
            string sql = string.Format(@"select * from [Student$]");
            DataSet ds = OleDbHelper.GetDataSet(sql, path);
            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(new Student()
                {
                    StudentName = dr["姓名"].ToString(),
                    Gender = dr["性别"].ToString(),
                    PhoneNumber = dr["电话号码"].ToString(),
                    Birthday = Convert.ToDateTime(dr["出生日期"].ToString()),
                    StudentIdNo = dr["身份证号"].ToString(),
                    Age = DateTime.Now.Year - Convert.ToDateTime(dr["出生日期"].ToString()).Year,
                    CardNo = dr["考勤卡号"].ToString(),
                    StudentAddress = dr["家庭住址"].ToString(),
                    ClassId = Convert.ToInt32(dr["班级编号"])
                });
            }
            return list;
        }

        public bool Import(List<Student> list)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            List<string> sqlList = new List<string>();
            sqlBuilder.Append(string.Format("insert into Students (StudentName,Gender,Birthday,Age,StudentIdNo,CardNo,PhoneNumber,StudentAddress,ClassId,StuImage)"));
            sqlBuilder.Append(string.Format(" values('{0}','{1}','{2}',{3},{4},'{5}','{6}','{7}',{8})"));
            foreach (Student objStudent in list)
            {
                string sql =string.Format(sqlBuilder.ToString(), objStudent.StudentName, objStudent.Gender,
                                                objStudent.Birthday.ToString("yyyy-MM-dd"), objStudent.Age, objStudent.StudentIdNo, objStudent.CardNo, objStudent.PhoneNumber,objStudent.StudentAddress, objStudent.ClassId);
                sqlList.Add(sql);
            }


        } 
    }
}
