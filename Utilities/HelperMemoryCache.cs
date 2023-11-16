//-----------------------------------------------------------------------
// <copyright company="工品一号" file="HelperMemoryCache.cs">
//  Copyright (c)  V1.0.0.0  
//  创建作者:   刘少林
//  创建时间:   2019-09-07 8:42:17
//  功能描述:   基于MemoryCache的缓存辅助类
//  历史版本:
//          2019-09-07 8:42:17 刘少林 创建HelperMemoryCache类
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;

namespace FX.MainForms.Public.Utilities
{
    /// <summary>
    /// 基于MemoryCache的缓存辅助类
    /// </summary>
    class HelperMemoryCache
    {
        private static readonly Object _locker = new object();

        /// <summary>
        /// 读取或者插入，方法返回的值
        /// </summary>
        /// <typeparam name="T">结果类型</typeparam>
        /// <param name="key">缓存键值名称</param>
        /// <param name="cachePopulate">方法</param>
        /// <param name="slidingExpiration">多少时间未使用，则缓存清除</param>
        /// <param name="absoluteExpiration">多少时间，则缓存清除</param>
        /// <returns></returns>
        public static T GetCacheItem<T>(String key, Func<T> cachePopulate, TimeSpan? slidingExpiration = null, DateTime? absoluteExpiration = null)
        {
            if (String.IsNullOrWhiteSpace(key)) throw new ArgumentException("Invalid cache key");
            if (cachePopulate == null) throw new ArgumentNullException("cachePopulate");
            if (slidingExpiration == null && absoluteExpiration == null) throw new ArgumentException("Either a sliding expiration or absolute must be provided");

            if (MemoryCache.Default[key] == null)
            {
                lock (_locker)
                {
                    if (MemoryCache.Default[key] == null)
                    {
                        var item = new CacheItem(key, cachePopulate());
                        var policy = CreatePolicy(slidingExpiration, absoluteExpiration);

                        MemoryCache.Default.Add(item, policy);
                    }
                }
            }

            return (T)MemoryCache.Default[key];
        }

        /// <summary>
        /// 创建缓存过期策略
        /// </summary>
        /// <param name="slidingExpiration">多少时间未使用，则缓存清除</param>
        /// <param name="absoluteExpiration">多少时间，则缓存清除</param>
        /// <returns></returns>
        private static CacheItemPolicy CreatePolicy(TimeSpan? slidingExpiration, DateTime? absoluteExpiration)
        {
            var policy = new CacheItemPolicy();

            if (absoluteExpiration.HasValue)
            {
                policy.AbsoluteExpiration = absoluteExpiration.Value;
            }
            else if (slidingExpiration.HasValue)
            {
                policy.SlidingExpiration = slidingExpiration.Value;
            }

            policy.Priority = CacheItemPriority.Default;

            return policy;
        }

        /// <summary>
        /// 缓存使用的键名称集合类
        /// </summary>
        public class Keys
        {
            /// <summary>
            /// 仓库表前缀
            /// </summary>
            public const string Warehouse_Previous = "Warehouse_";

            /// <summary>
            /// 字典前缀
            /// </summary>
            public const string Dict_Key_Previous = "Dict_Key_";

            /// <summary>
            /// 数据库表前缀
            /// </summary>
            public const string Table_Column_Previous = "Table_Column_";

            /// <summary>
            /// 服务器时间
            /// </summary>
            public const string ServerTime = "ServerTime";

            /// <summary>
            /// 我的导航
            /// </summary>
            public const string MyNavigation = "MyNavigation";
        }
    }
}
