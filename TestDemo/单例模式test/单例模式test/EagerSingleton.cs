using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 单例模式test
{
    /// <summary>
    /// 饿汉式单例模式
    /// 1-4都是懒汉式单例
    /// </summary>
    public class EagerSingleton
    {
        //先把这个实例创建出来
        private static EagerSingleton instance = new EagerSingleton();

        //私有的构造方法
        private EagerSingleton() { }

        // 如果要使用这个实例，统一通过 Singleton.getInstance() 方式获取
        // 饿汉模式只涉及到读操作，因此是线程安全的
        public static EagerSingleton getInstance()
        {
            return instance;
        }
    }
}
