using AutoMapper;
using BCVP.Net8.IService;
using BCVP.Net8.Model;
using BCVP.Net8.Repository;
using Newtonsoft.Json;

namespace BCVP.Net8.Service
{
    public class BaseService<TEntity, TVo> : IBaseService<TEntity, TVo> where TEntity : class, new()
    {

        private readonly IMapper _mapper;
        private readonly IBaseRepository<TEntity> _baseRepository;

        public BaseService(IMapper mapper, IBaseRepository<TEntity> baseRepository)
        {
            _mapper = mapper;
            _baseRepository = baseRepository;
        }


        public async Task<List<TVo>> Query()
        {
            //var baseRepo = new BaseRepository<TEntity>();
            //var data = await baseRepo.Query();

            var data = await _baseRepository.Query();
            var bo = _mapper.Map<List<TVo>>(data);
            return bo;
        }
    }
}
