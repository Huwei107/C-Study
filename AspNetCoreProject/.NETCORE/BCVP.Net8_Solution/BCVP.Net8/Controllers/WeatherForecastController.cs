using AutoMapper;
using BCVP.Net8.Common;
using BCVP.Net8.Common.Option;
using BCVP.Net8.IService;
using BCVP.Net8.Model;
using BCVP.Net8.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BCVP.Net8.Controllers
{
    public class WeatherForecastController : ApiControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IBaseService<Role, RoleVo> _roleService;
        private readonly IOptions<RedisOptions> _redisOptions;

        public IBaseService<Role, RoleVo> _roleServiceObj { get; set; }

        public WeatherForecastController(ILogger<WeatherForecastController> logger,
            IBaseService<Role, RoleVo> roleService,
            IOptions<RedisOptions> redisOptions)
        {
            _logger = logger;
            _roleService = roleService;
            _redisOptions = redisOptions;
        }

        [HttpGet(Name = "UserList")]
        public async Task<object> GetUserList()
        {
            //var userSerivce = new UserService();
            //var userList = await userSerivce.Query();
            //return userList;

            //var roleService = new BaseSerivce<Role, RoleVo>(_mapper);
            //var roleList = await roleService.Query();

            //var roleList = await _roleService.Query();

            //var roleList = await _roleServiceObj.Query();
            //var redisEnable = AppSettings.app(new string[] { "Redis", "Enable" });
            //var redisConnectionString = AppSettings.app(new string[] { "Redis", "ConnectionString" });
            //var redisOptons = _redisOptions.Value;

            var roleServiceObjNew = App.GetService<IBaseService<Role, RoleVo>>(false);
            var roleList = await roleServiceObjNew.Query();
            var redisOptons = App.GetOptions<RedisOptions>();

            return roleList;
        }
    }
}
