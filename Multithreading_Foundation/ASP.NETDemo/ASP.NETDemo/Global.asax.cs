using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace ASP.NETDemo
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            Application.Lock();
            Application["UserVisit"] = 0;//网站被访问的次数
            Application["CurrentUsers"] = 0;//网站在线的人数
            Application.UnLock();
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            Application.Lock();
            Application["UserVisit"] = (int)Application["UserVisit"] + 1;//网站被访问的次数
            Application["CurrentUsers"] = (int)Application["CurrentUsers"] + 1;//网站在线的人数
            Application.UnLock();
        }

        protected void Session_End(object sender, EventArgs e)
        {
            //在会话结束运行的代码
            //注意：在web.config文件中，把sessionstate模式设置为InPro时才引发此事件
            Application.Lock();
            Application["CurrentUsers"] = (int)Application["CurrentUsers"] - 1;//网站在线的人数
            Application.UnLock();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}