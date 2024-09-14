using BCVP.Net8.Model;
using SqlSugar;
using System.Linq.Expressions;

namespace BCVP.Net8.Repository
{
    /// <summary>
    /// 泛型接口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        ISqlSugarClient Db { get; }

        /// <summary>
        /// 写入实体数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<long> Add(TEntity entity);
        Task<List<long>> AddSplit(TEntity entity);

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <returns></returns>
        Task<List<TEntity>> Query();
        Task<List<TEntity>> QuerySplit(Expression<Func<TEntity, bool>> whereExpression, string orderByFields = null);
    }
}
