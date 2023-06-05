using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP.NETDemo.DAL
{
    public class AdminService
    {
        public bool AdminLogin(string name, string pwd)
        {
            if (name == "hw" && pwd == "123")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}