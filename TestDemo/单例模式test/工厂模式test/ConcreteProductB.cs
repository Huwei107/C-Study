using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 工厂模式test
{
    /// <summary>
    /// 具体产品类B
    /// </summary>
    public class ConcreteProductB : IProduct
    {
        public void Operation()
        {
            Console.WriteLine("具体产品B的操作");
        }
    }
}
