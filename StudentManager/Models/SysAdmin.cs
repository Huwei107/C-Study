using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    /// <summary>
    /// 管理员实体类
    /// </summary>
    public class SysAdmin
    {
        public int LoginId { get; set; }
        public string AdminName { get; set; }
        public string LoginPwd { get; set; }
    }
}
