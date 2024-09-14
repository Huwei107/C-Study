using AutoMapper;
using BCVP.Net8.IService;
using BCVP.Net8.Model;
using BCVP.Net8.Repository;
using Newtonsoft.Json;
using SqlSugar;
using System.Linq.Expressions;

namespace BCVP.Net8.Service
{
    public class BaseService<TEntity, TVo> : IBaseService<TEntity, TVo> where TEntity : class, new()
    {

        private readonly IMapper _mapper;
        private readonly IBaseRepository<TEntity> _baseRepository;

        public ISqlSugarClient Db => _baseRepository.Db;

        public BaseService(IMapper mapper, IBaseRepository<TEntity> baseRepository)
        {
            _mapper = mapper;
            _baseRepository = baseRepository;
        }

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <returns></returns>
        public async Task<List<TVo>> Query()
        {
            //var baseRepo = new BaseRepository<TEntity>();
            //var data = await baseRepo.Query();

            var data = await _baseRepository.Query();
            var bo = _mapper.Map<List<TVo>>(data);
            return bo;
        }

        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<long> Add(TEntity entity)
        {
            var id = await _baseRepository.Add(entity);
            return id;
        }

        public async Task<List<TEntity>> QuerySplit(Expression<Func<TEntity, bool>> whereExpression, string orderByFields = null)
        {
            return await _baseRepository.QuerySplit(whereExpression, orderByFields);
        }

        public async Task<List<long>> AddSplit(TEntity entity)
        {
            return await _baseRepository.AddSplit(entity);
        }
    }
}
