using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Models;

namespace DAL
{
    public class AdminService
    {
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="objAdmin">用户对象</param>
        /// <returns></returns>
        public SysAdmin AdminLogin(SysAdmin objAdmin)
        {
            string sql = string.Format(@"select AdminName from Admins where LoginId = @LoginId and LoginPwd = @LoginPwd");
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@LoginId",objAdmin.LoginId),
                new SqlParameter("@LoginPwd",objAdmin.LoginPwd)
            };
            try
            {
                SqlDataReader objReader = SQLHelper.GetReader(sql, param, false);
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
            catch (Exception ex)
            {

                throw new Exception("用户登录数据访问出现异常" + ex.Message); 
            }
            return objAdmin;
        }

        /// <summary>
        /// 修改登录密码
        /// </summary>
        /// <param name="objAdmin">参数数组</param>
        /// <returns></returns>
        public int ModifyPwd(SysAdmin objAdmin)
        {
            string sql = string.Format(@"update Admins set LoginPwd=@LoginPwd where LoginId = @LoginId");
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@LoginPwd",objAdmin.LoginPwd),
                new SqlParameter("@LoginId",objAdmin.LoginId)
            };
            try
            {
                return SQLHelper.Update(sql, param, false);
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }
    }
}
