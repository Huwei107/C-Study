using AutoMapper;
using BCVP.Net8.IService;
using BCVP.Net8.Model;
using BCVP.Net8.Repository;
using Newtonsoft.Json;

namespace BCVP.Net8.Service
{
    public class BaseSerivce<TEntity, TVo> : IBaseService<TEntity, TVo> where TEntity : class, new()
    {

        private readonly IMapper _mapper;
        public BaseSerivce(IMapper mapper)
        {
            _mapper = mapper;
        }


        public async Task<List<TVo>> Query()
        {
            var baseRepo = new BaseRepository<TEntity>();
            var data = await baseRepo.Query();
            var bo = _mapper.Map<List<TVo>>(data);
            return bo;
        }
    }
}
