using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqDemo
{
    /// <summary>
    /// 扩展方法类
    /// </summary>
    static class ExtendMethod
    {
        public static int GetAvg(this int sum)
        {
            return sum / 5;
        }
        public static string StuInfo(this string name)
        {
            return string.Format(@"{0}，你的5门平均成绩为：",name);
        }

        //为密封类Student添加扩展方法
        public static string ShowStuInfo(this Student objStudent)
        {
            return "欢迎您：" + objStudent.StudentName;
        }
        public static string ShowStuInfo(this Student objStudent, int csharp, int database)
        {
            return string.Format(@"{0}两门课的平均成绩为：{1}", objStudent.StudentName, (csharp+database)/2);
        }
    }
}
