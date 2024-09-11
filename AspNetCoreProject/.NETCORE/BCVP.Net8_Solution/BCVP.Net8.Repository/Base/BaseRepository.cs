using BCVP.Net8.Model;
using Newtonsoft.Json;
using SqlSugar;

namespace BCVP.Net8.Repository
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, new()
    {
        private readonly ISqlSugarClient _sqlSugarClient;

        public BaseRepository(ISqlSugarClient sqlSugarClient)
        {
            _sqlSugarClient = sqlSugarClient;
        }

        public ISqlSugarClient Db => _sqlSugarClient;

        public async Task<List<TEntity>> Query()
        {
            await Console.Out.WriteLineAsync(Db.GetHashCode().ToString());
            //await Console.Out.WriteLineAsync(_sqlSugarClient.GetHashCode().ToString());
            return await _sqlSugarClient.Queryable<TEntity>().ToListAsync();
        }
    }
}
