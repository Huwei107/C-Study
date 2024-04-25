using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 单例模式test
{
    /// <summary>
    /// 单例模式
    /// 单线程下是完美的，不能在多线程下
    /// （字段命名规范，首字母大写的是给外部使用，小写的给内部使用。）
    /// </summary>
    public class Singleton1
    {
        //定义一个私有静态变量来保存类的实例
        private static Singleton1? instance;

        //定义私有构造函数，
        private Singleton1()
        {
            Console.WriteLine("============================");
            Console.WriteLine("私有构造函数实例化！");
        }

        /// <summary>
        /// 定义公有方法提供一个全局访问点，同时你也可以定义公有属性来提供全局访问点
        /// </summary>
        /// <returns></returns>
        public static Singleton1 GetInstance()
        {
            if (instance == null)
            {
                instance = new Singleton1();
                Console.WriteLine($"{instance}初始化了！");
            }
            return instance;
        }
    }
}
