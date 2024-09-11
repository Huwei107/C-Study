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

        /// <summary>
        /// 查询所有实体
        /// </summary>
        /// <returns></returns>
        public async Task<List<TEntity>> Query()
        {
            await Console.Out.WriteLineAsync(Db.GetHashCode().ToString());
            //await Console.Out.WriteLineAsync(_sqlSugarClient.GetHashCode().ToString());
            return await _sqlSugarClient.Queryable<TEntity>().ToListAsync();
        }

        /// <summary>
        /// 写入实体数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<long> Add(TEntity entity)
        {
            var insert = _sqlSugarClient.Insertable(entity);
            return await insert.ExecuteReturnSnowflakeIdAsync();
        }
    }
}
