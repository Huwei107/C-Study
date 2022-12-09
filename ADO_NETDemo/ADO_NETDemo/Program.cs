using ADO_NETDemo.DAL;
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

            //string sql = string.Format(@"update Students set StudentName='刘晶晶' where StudentId=10005");
            //int result = SQLHelper.Update(sql);
            //if (result == 1)
            //{
            //    Console.WriteLine("成功！");
            //}
            //else
            //{
            //    Console.WriteLine("失败！");
            //}

            //string sql = string.Format(@"select StudentName from Students");
            //SqlDataReader objReader = SQLHelper.GetReader(sql);
            //while (objReader.Read())
            //{
            //    Console.WriteLine(objReader["StudentName"]);
            //}
            //objReader.Close();//关闭读取器，同时自动关闭关联的连接

            Console.WriteLine("输入学员姓名：");
            string stuName = Console.ReadLine();
            Console.WriteLine("输入学员性别：");
            string gender = Console.ReadLine();
            Console.WriteLine("输入学员出生日期：");
            DateTime birthday = Convert.ToDateTime(Console.ReadLine());
            Console.WriteLine("输入学员身份证：");
            string stuIdNo = Console.ReadLine();
            Console.WriteLine("输入学员年龄：");
            int age = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("输入学员电话：");
            string phoneNumber = Console.ReadLine();
            Console.WriteLine("输入学员地址：");
            string stuAddress = Console.ReadLine();
            Console.WriteLine("输入学员班级编号：");
            int classId = Convert.ToInt32(Console.ReadLine());

            StudentService objStudentService = new StudentService();
            int result = objStudentService.AddStudent(stuName, gender, birthday, stuIdNo, age, phoneNumber, stuAddress, classId);
            Console.WriteLine(result);
            
            Console.ReadLine();
        }
    }
}
