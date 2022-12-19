using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO_NETDemo.Models
{
    public class StudentSimpleExt:Student
    {
        public string ClassName { get; set; }
        public int AvgScore { get; set; }
    }
}
