using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqDemo
{
    sealed public class Student
    {
        public int Age { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }

        public List<int> ScoreList { get; set; }
    }
}
