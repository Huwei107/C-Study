using BCVP.Net8.Model;

namespace BCVP.Net8.IService
{
    public interface IBaseService<TEntity, TVo>
    {
        Task<List<TEntity>> Query();
    }
}
