using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO_NETDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //string sql = string.Format(@"select count(*) from Students");
            //object result = SQLHelper.GetSingleResult(sql);
            //Console.WriteLine(result);
            //Console.ReadLine();

            string sql = string.Format(@"update Students set StudentName='刘晶晶' where StudentId=10005");
            int result = SQLHelper.Update(sql);
            if (result == 1)
            {
                Console.WriteLine("成功！");
            }
            else
            {
                Console.WriteLine("失败！");
            }
            
            Console.ReadLine();
        }
    }
}
