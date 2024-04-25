using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 单例模式test
{
    public class ICache
    {
        private readonly IDistributedCache _cache;

        public ICache(IDistributedCache cache)
        {
            _cache = cache;
        }


    }
}
