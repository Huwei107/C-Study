using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP.NETDemo.Handler
{
    /// <summary>
    /// LoginHandler 的摘要说明
    /// </summary>
    public class LoginHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //获取前台网页所提交的数据
            string uname = context.Request.Params["uname"];
            string upwd = context.Request.Params["upwd"];
            //调用数据访问方法判断用户名或密码是否正确
            DAL.AdminService objAdmin = new DAL.AdminService();
            if (objAdmin.AdminLogin(uname, upwd))
            {
                context.Response.Write("登录成功！");
            }
            else
            {
                context.Response.Write("登录失败！");
            }
        }

        public bool IsReusable//是否缓存对象，以供下次使用
        {
            get
            {
                return false;
            }
        }
    }
}