using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            #region var关键字
            //var a = 20;
            //var bookName = "开发";
            //var objStudent = new Student() { StudentName = "小张", Age = 22 };
            //Console.WriteLine("共有{0}个人在学习{1}课程，其中{2}，{3}岁", a, bookName, objStudent.StudentName, objStudent.Age);
            #endregion

            #region 匿名类
            //创建匿名对象
            //var objPerson = new
            //{
            //    Name="小王",
            //    Age=25,
            //    ClassName="软件1班"
            //};
            //Console.WriteLine("姓名：{0} 年龄：{1} 班级：{2}", objPerson.Name, objPerson.Age, objPerson.ClassName);
            #endregion

            #region 简单扩展方法
            //string stuName = "小张";
            //int scoreSum = 460;
            //Console.WriteLine("{0} {1}", stuName.StuInfo(), scoreSum.GetAvg());
            #endregion

            #region 简单扩展方法的应用(克服继承的局限性)
            //var objStudent = new Student() { Age = 20, StudentName = "小张" };
            //Console.WriteLine(objStudent.ShowStuInfo());
            #endregion

            #region 简单扩展方法的应用(克服继承的局限性)
            //var objStudent = new Student() { Age = 20, StudentName = "小张" };
            //Console.WriteLine(objStudent.ShowStuInfo(70,80));
            #endregion
            
            #region 委托
            //【3】创建委托对象，关联具体方法
            //CalculatorDelegate objCal = new CalculatorDelegate(Add);
            ////【4】通过委托调用方法，而不是直接使用方法
            //int result = objCal(10, 20);
            //Console.WriteLine(result);
            //objCal -= Add;//断开当前委托所关联的方法(加法)
            //objCal += Sub;//重新关联一个方法（减法）
            //result = objCal(50, 30);
            //Console.WriteLine(result);
            #endregion

            #region 匿名方法
            //CalculatorDelegate objCal = delegate(int a, int b)
            //{
            //    return a + b;
            //};
            //int result = objCal(10, 20);
            //Console.WriteLine(result);
            #endregion

            #region lambda表达式
            //CalculatorDelegate objCal = (int a, int b) => { return a + b; };
            //CalculatorDelegate objCal = (a, b) =>  a + b;
            //int result = objCal(10, 20);
            //Console.WriteLine(result);
            #endregion

            #region ------------------------------------LINQ------------------------------------
            #region 实列1：不使用LINQ查询数组
            //int[] nums = { 1, 7, 2, 6, 5, 4, 9, 27, 20, 13 };
            //List<int> list = new List<int>();
            //foreach (int item in nums)
            //{
            //    if (item % 2 != 0)
            //        list.Add(item);
            //}
            //list.Sort();
            //list.Reverse();//降序
            //foreach (int item in list)
            //{
            //    Console.WriteLine(item);
            //}
            #endregion

            #region 实例2：使用LINQ查询数组
            //int[] nums = { 1, 7, 2, 6, 5, 4, 9, 27, 20, 13 };
            //var list = from num in nums
            //           where num % 2 != 0
            //           orderby num descending   //降序
            //           select num;
            //foreach (int item in list)
            //{
            //    Console.WriteLine(item);
            //}
            #endregion

            #region 实例3：扩展方法Select()应用
            //int[] nums = { 1, 7, 2, 6, 5, 4, 9, 27, 20, 13 };
            //var list = nums.Select(item => item * item);
            //foreach (int item in list)
            //{
            //    Console.WriteLine(item);
            //}
            #endregion

            #region 实例4：扩展方法Where()应用
            //int[] nums = { 1, 7, 2, 6, 5, 4, 9, 27, 20, 13 };
            //var list = nums.Where(item => item % 2 == 0).Select(i => i * i);
            //foreach (int item in list)
            //{
            //    Console.WriteLine(item);
            //}
            #endregion
            
            #region 实例5：扩展方法OrderBy()应用
            //int[] nums = { 1, 7, 2, 6, 5, 4, 9, 27, 20, 13 };
            //var list = nums.Where(i => i % 2 == 0).Select(i => i * i).OrderBy(i => i);
            //foreach (int item in list)
            //{
            //    Console.WriteLine(item);
            //}
            //string[] names = {"张三","李四","王五","赵敏","成龙","那英","古力娜扎"};
            //var listName = names.Where(i => i.Length == 2).Select(i => i).OrderByDescending(i => i.Substring(0, 1));
            //foreach (string item in listName)
            //{
            //    Console.WriteLine(item);
            //}
            #endregion

            #region 实例6：扩展方法GroupBy()应用
            //string[] names = { "张三", "李四", "王五", "赵敏", "成龙", "那英", "古力娜扎","杜丽","杜宇" };
            //var list = names.Where(i => i.Length == 2).Select(i => i).GroupBy(i => i.Substring(0, 1));
            //foreach (var group in list)
            //{
            //    Console.WriteLine("===========");
            //    Console.WriteLine("分组字段：{0}", group.Key);
            //    foreach (var item in group)
            //    {
            //        Console.WriteLine(item);
            //    }
            //}
            #endregion

            #region 实例7：LINQ的立即执行
            //int[] nums = { 1, 7, 2, 6, 5, 4, 9, 27, 20, 13 };
            //var list = nums.Where(i => i % 2 == 0).Select(i => i * i).OrderBy(i => i)
            //    .Count();//立即执行
            //Console.WriteLine(list);
            #endregion

            #region 实例8：复合from子句的使用
            //Student obj1 = new Student()
            //{
            //    StudentId = 1001,
            //    StudentName = "学员1",
            //    ScoreList = new List<int>() { 90, 78, 54 }
            //};
            //Student obj2 = new Student()
            //{
            //    StudentId = 1002,
            //    StudentName = "学员2",
            //    ScoreList = new List<int>() { 70, 88, 74 }
            //};
            //Student obj3 = new Student()
            //{
            //    StudentId = 1003,
            //    StudentName = "学员3",
            //    ScoreList = new List<int>() { 95, 87, 61 }
            //};
            ////将学员封装到集合中
            //List<Student> stuList = new List<Student>() { obj1, obj2, obj3 };
            ////查询成绩大于等于95分的学员
            //var result = from stu in stuList
            //             from score in stu.ScoreList
            //             where score >= 95
            //             select stu;
            ////显示查询结果
            //foreach (var item in result)
            //{
            //    Console.WriteLine(item.StudentName);
            //}
            #endregion

            #region 实例9：多个from子句查询的使用
            //Student obj1 = new Student() { StudentId = 1001, StudentName = "学员1" };
            //Student obj2 = new Student() { StudentId = 1009, StudentName = "学员2" };
            //Student obj3 = new Student() { StudentId = 1011, StudentName = "学员3" };
            //Student obj4 = new Student() { StudentId = 1041, StudentName = "学员4" };
            //Student obj5 = new Student() { StudentId = 1004, StudentName = "学员5" };
            //Student obj6 = new Student() { StudentId = 1021, StudentName = "学员6" };
            //List<Student> stuList1 = new List<Student>() { obj1, obj2, obj3 };
            //List<Student> stuList2 = new List<Student>() { obj4, obj5, obj6 };
            ////查询学号大于1010的学员
            //var result = from stu1 in stuList1
            //             where stu1.StudentId > 1010
            //             from stu2 in stuList2
            //             where stu2.StudentId > 1010
            //             select new { stu1, stu2 };
            //foreach (var item in result)
            //{
            //    Console.WriteLine(item.stu1.StudentName + " " + item.stu1.StudentId);
            //    Console.WriteLine(item.stu2.StudentName + " " + item.stu2.StudentId);
            //}
            #endregion

            #region 实例10：聚合函数Count
            //Student obj1 = new Student() { StudentId = 1001, StudentName = "学员1" };
            //Student obj2 = new Student() { StudentId = 1009, StudentName = "学员2" };
            //Student obj3 = new Student() { StudentId = 1011, StudentName = "学员3" };
            //Student obj4 = new Student() { StudentId = 1041, StudentName = "学员4" };
            //Student obj5 = new Student() { StudentId = 1004, StudentName = "学员5" };
            //Student obj6 = new Student() { StudentId = 1021, StudentName = "学员6" };
            //List<Student> stuList = new List<Student>() { obj1, obj2, obj3, obj4, obj5, obj6 };
            //var count1 = (from c in stuList
            //              where c.StudentId > 1010
            //              select c).Count();
            //var count2 = stuList.Where(c => c.StudentId > 1010).Count();
            //Console.WriteLine("count1:{0}, count2:{1}", count1, count2);
            #endregion

            #region 实例11：聚合函数Max/Min
            //Student obj1 = new Student() { Age = 15, StudentName = "学员1" };
            //Student obj2 = new Student() { Age = 17, StudentName = "学员2" };
            //Student obj3 = new Student() { Age = 16, StudentName = "学员3" };
            //Student obj4 = new Student() { Age = 16, StudentName = "学员4" };
            //Student obj5 = new Student() { Age = 19, StudentName = "学员5" };
            //Student obj6 = new Student() { Age = 15, StudentName = "学员6" };
            //List<Student> stuList = new List<Student>() { obj1, obj2, obj3, obj4, obj5, obj6 };
            //var maxAge = (from stu in stuList
            //              select stu.Age).Max();
            //var minAge = stuList.Select(i => i.Age).Min();
            //var avgAge = (from stu in stuList select stu.Age).Average();
            //var sumAge = stuList.Select(i => i.Age).Sum();
            //Console.WriteLine("最大年龄：{0}，最小年龄：{1}，平均年龄：{2}，年龄总和：{3}", maxAge, minAge, avgAge, sumAge);
            #endregion

            #region 实例12：排序类ThenBy的使用
            //Student obj1 = new Student() { Age = 15, StudentName = "学员1" };
            //Student obj2 = new Student() { Age = 17, StudentName = "学员2" };
            //Student obj3 = new Student() { Age = 16, StudentName = "学员3" };
            //Student obj4 = new Student() { Age = 20, StudentName = "学员4" };
            //Student obj5 = new Student() { Age = 19, StudentName = "学员5" };
            //Student obj6 = new Student() { Age = 14, StudentName = "学员6" };
            //List<Student> stuList = new List<Student>() { obj1, obj2, obj3, obj4, obj5, obj6 };
            //var stus1 = from s in stuList orderby s.StudentName.Substring(0,1), s.Age select s;
            //var stus2 = stuList.OrderBy(s => s.Age).ThenBy(s => s.StudentName).Select(s => s);
            //foreach (var item in stus1)
            //{
            //    Console.WriteLine(item.StudentName);
            //}
            #endregion

            #region 实例13：分区类查询
            //Take 提取指定数量的项
            //Skip 跳过指定数量的项并获取剩余的项
            //TakeWhile 只要满足指定的条件，就会返回序列的元素，然后跳过剩余的元素
            //SkipWhile 只要满足指定的条件，就跳过序列中的元素，然后返回剩余元素
            //int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            //var list1 = nums.Skip(1).Take(1);
            //var list2 = nums.SkipWhile(i => i % 3 != 0).TakeWhile(i => i % 2 != 0);
            //foreach (var item in list1)
            //{
            //    Console.WriteLine(item);
            //}
            //Console.WriteLine("========================");
            //foreach (var item in list2)
            //{
            //    Console.WriteLine(item);
            //}
            #endregion

            #region 实例14：集合类查询Distinct
            //int[] nums = { 1, 2, 2, 6, 5, 6, 7, 8, 9 };
            //var list = nums.Distinct();
            //foreach (var item in list)
            //{
            //    Console.WriteLine(item);
            //}
            #endregion

            #endregion

            Console.ReadLine();
        }
        //【2】根据委托定义具体方法
        static int Add(int a, int b)
        {
            return a + b;
        }
        static int Sub(int a, int b)
        {
            return a - b;
        }
        
    }
    //【1】声明委托（定义一个函数的原型：返回值+参数类型和个数）
    public delegate int CalculatorDelegate(int a, int b);
}
