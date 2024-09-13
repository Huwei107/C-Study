﻿using BCVP.Net8.Model;
using SqlSugar;

namespace BCVP.Net8.IService
{
    public interface IBaseService<TEntity, TVo>
    {
        ISqlSugarClient Db { get; }

        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<long> Add(TEntity entity);

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <returns></returns>
        Task<List<TVo>> Query();
    }
}
