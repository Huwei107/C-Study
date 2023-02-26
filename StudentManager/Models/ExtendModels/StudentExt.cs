using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.ExtendModels
{
    /// <summary>
    /// 学员实体（扩展）
    /// </summary>
    public class StudentExt
    {
        public Student StudentObj { get; set; }
        public StudentClass ClassObj { get; set; }
        public ScoreList ScoreObj { get; set; }

    }
}
