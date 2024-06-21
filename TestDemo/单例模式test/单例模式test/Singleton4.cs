using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 单例模式test
{
    /// <summary>
    /// 泛型单例模式
    /// </summary>
    public class Singleton4<TEntity> where TEntity : class, new()
    {
        // 定义一个静态变量来保存类的实例
        private static TEntity _instance;

        // 定义一个标识确保线程同步
        private static readonly object syslock = new object();

        public static TEntity getInstance()
        {
            if (_instance == null)
            {
                lock (syslock)
                {
                    if (_instance == null)
                    {
                        _instance = new TEntity();
                    }
                }
            }
            return _instance;
        }
    }
}
