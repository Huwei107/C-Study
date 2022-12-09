using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO_NETDemo.Models
{
    public class StudentExt
    {
        public StudentExt()
        {
            ObjStudent = new Student();
            ObjClass = new StudentClass();
            objScore = new ScoreList();
        }
        public Student ObjStudent { get; set; }
        public StudentClass ObjClass { get; set; }
        public ScoreList objScore { get; set; }
    }
}
