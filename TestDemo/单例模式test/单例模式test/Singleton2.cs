using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 单例模式test
{
    /// <summary>
    /// 单例模式
    /// </summary>
    public class Singleton2
    {
        //定义一个私有静态变量来保存类的实例
        private static Singleton2? instance;

        //定义私有构造函数，使外界不能创建该类的实例
        private Singleton2()
        {

        }
    }
}
