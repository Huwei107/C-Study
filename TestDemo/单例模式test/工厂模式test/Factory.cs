using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 工厂模式test
{
    public class Factory
    {
        public IProduct CreateProduct(string productType)
        {
            switch (productType)
            {
                case "A":
                    return new ConcreteProductA();
                case "B":
                    return new ConcreteProductB();
                default:
                    throw new ArgumentException("无效的产品类型");
            }
        }
    }
}
