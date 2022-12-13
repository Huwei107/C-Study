using ADO_NETDemo.DAL;
using ADO_NETDemo.Models;
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

            //Student objStu = new Student();
            //Console.WriteLine("输入学员姓名：");
            //objStu.StudentName = Console.ReadLine();
            //Console.WriteLine("输入学员性别：");
            //objStu.Gender = Console.ReadLine();
            //Console.WriteLine("输入学员出生日期：");
            //objStu.Birthday = Convert.ToDateTime(Console.ReadLine());
            //Console.WriteLine("输入学员身份证：");
            //objStu.StudentIdNo = Convert.ToDecimal(Console.ReadLine());
            //Console.WriteLine("输入学员年龄：");
            //objStu.Age = Convert.ToInt32(Console.ReadLine());
            //Console.WriteLine("输入学员电话：");
            //objStu.PhoneNumber = Console.ReadLine();
            //Console.WriteLine("输入学员地址：");
            //objStu.StudentAddress = Console.ReadLine();
            //Console.WriteLine("输入学员班级编号：");
            //objStu.ClassId = Convert.ToInt32(Console.ReadLine());
            //StudentService objStudentService = new StudentService();
            //int result = objStudentService.AddStudent(objStu);
            //Console.WriteLine(result);

            //StudentService objStuService = new StudentService();
            //Student objStudent = objStuService.GetStudentById("10004");
            //Console.WriteLine(objStudent.StudentName +'\t'+objStudent.StudentIdNo);

            //StudentService objStudentService = new StudentService();
            //List<Student> list = objStudentService.GetAllStudents();
            //if (list.Count != 0)
            //{
            //    foreach (Student item in list)
            //    {
            //        Console.WriteLine(item.StudentName + "  " + item.Gender + "  " + item.StudentAddress);
            //    }
            //}

            StudentService objStudentService = new StudentService();
            List<StudentExt> list = objStudentService.GetStudentExt();
            foreach (StudentExt item in list)
            {
                Console.WriteLine(item.ObjStudent.StudentName + "\t" + item.ObjClass.ClassName + "\t" + item.objScore.CSharp);
            }
            
            Console.ReadLine();
        }
    }
}
