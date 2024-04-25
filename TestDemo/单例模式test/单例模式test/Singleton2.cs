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

        //定义一个标识确保线程同步
        private static readonly object locker = new object();

        //定义私有构造函数，使外界不能创建该类的实例
        private Singleton2()
        {

        }

        /// <summary>
        /// 定义公有方法提供一个全局访问点,同时你也可以定义公有属性来提供全局访问点
        /// </summary>
        /// <returns></returns>
        public static Singleton2 GetInstance()
        {
            //当第一个线程运行到这里时，此时会对locker对象 "加锁"，
            //当第二个线程运行该方法时，首先检测到locker对象为"加锁"状态，该线程就会挂起等待第一个线程解锁
            //lock语句运行完之后（即线程运行完之后）会对该对象"解锁"
            lock (locker)
            {
                if (instance == null)
                {
                    instance = new Singleton2();
                }
            }
            //因为当第一个线程创建了该类的实例之后，
            //后面的线程此时只需要直接判断（uniqueInstance==null）为假，
            //此时完全没必要对线程辅助对象加锁之后再去判断，
            //所以上面的实现方式增加了额外的开销，损失了性能
            return instance;
        }
    }
}
