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
    /// 管理员数据访问类
    /// </summary>
    public class SysAdminService
    {
        //修饰符public——>返回值SysAdmin对象——>参数SysAdmin对象
        /// <summary>
        /// 根据账号和密码登录
        /// </summary>
        /// <param name="objAdmin"></param>
        /// <returns></returns>
        public SysAdmin AdminLogin(SysAdmin objAdmin)
        {
            //sql语句编写
            string sql = string.Format(@"select AdminName from Admins where LoginId={0} and LoginPwd='{1}'", objAdmin.LoginId, objAdmin.LoginPwd);
            //调用通用数据访问类
            try
            {
                SqlDataReader objReader = SQLHelper.GetReader(sql);
                if (objReader.Read())
                {
                    objAdmin.AdminName = objReader["AdminName"].ToString();
                }
                else
                {
                    objAdmin = null;
                }
                objReader.Close();
            }
            catch (SqlException ex)
            {

                throw new Exception("应用程序和数据库连接出现问题：" + ex.Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //返回结果
            return objAdmin;
        }
    }
}
