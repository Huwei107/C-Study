using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 工厂模式test
{
    /// <summary>
    /// 具体产品类A
    /// </summary>
    public class ConcreteProductA : IProduct
    {
        public void Operation()
        {
            Console.WriteLine("具体产品A的操作");
        }
    }
}
