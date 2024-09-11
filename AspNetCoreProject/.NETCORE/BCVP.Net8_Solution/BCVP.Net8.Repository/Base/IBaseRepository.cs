using BCVP.Net8.Model;
using SqlSugar;

namespace BCVP.Net8.Repository
{
    /// <summary>
    /// 泛型接口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        ISqlSugarClient Db { get; }

        Task<List<TEntity>> Query();
    }
}
