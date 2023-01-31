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
    /// 管理员数据访问类
    /// </summary>
    public class SysAdminService
    {
        /// <summary>
        /// 根据登录账号和密码登录
        /// </summary>
        /// <param name="objAdmin"></param>
        /// <returns></returns>
        public SysAdmin AdminLogin(SysAdmin objAdmin)
        {
            string sql = string.Format(@"select AdminName from Admins where LoginId={0} and LoginPwd='{1}'", objAdmin.LoginId, objAdmin.LoginPwd);
            SqlDataReader objReader = SQLHelper.GetReader(sql);
            if (objReader.Read())
            {
                objAdmin.AdminName = objReader["AdminName"].ToString();
            }
            else
            {
                objAdmin = null;//如果登录不成功，则将对象清空
            }
            objReader.Close();

            return objAdmin;
        }
    }
}
