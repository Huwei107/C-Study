using BCVP.Net8.Model;

namespace BCVP.Net8.Repository
{
    /// <summary>
    /// 泛型接口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<List<TEntity>> Query();
    }
}
