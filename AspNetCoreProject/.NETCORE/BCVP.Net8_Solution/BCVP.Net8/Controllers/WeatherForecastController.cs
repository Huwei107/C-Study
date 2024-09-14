using AutoMapper;
using BCVP.Net8.Common;
using BCVP.Net8.Common.Caches;
using BCVP.Net8.Common.Option;
using BCVP.Net8.IService;
using BCVP.Net8.Model;
using BCVP.Net8.Model.Vo;
using BCVP.Net8.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace BCVP.Net8.Controllers
{
    public class WeatherForecastController : ApiControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IBaseService<Role, RoleVo> _roleService;
        private readonly IBaseService<AuditSqlLog, AuditSqlLogVo> _aduitSqlLogService;
        private readonly IOptions<RedisOptions> _redisOptions;
        private readonly ICaching _caching;

        public IBaseService<Role, RoleVo>? _roleServiceObj { get; set; }

        public WeatherForecastController(ILogger<WeatherForecastController> logger,
            IBaseService<Role, RoleVo> roleService,
            IBaseService<AuditSqlLog, AuditSqlLogVo> aduitSqlLogService,
            IOptions<RedisOptions> redisOptions,
            ICaching caching)
        {
            _logger = logger;
            _roleService = roleService;
            _aduitSqlLogService = aduitSqlLogService;
            _redisOptions = redisOptions;
            _caching = caching;
        }

        [HttpGet("[action]")]
        public async Task<object> GetUserList()
        {
            //var userSerivce = new UserService();
            //var userList = await userSerivce.Query();
            //return userList;

            //var roleService = new BaseSerivce<Role, RoleVo>(_mapper);
            //var roleList = await roleService.Query();

            var roleList = await _roleService.Query();

            //var roleList = await _roleServiceObj.Query();
            //var redisEnable = AppSettings.app(new string[] { "Redis", "Enable" });
            //var redisConnectionString = AppSettings.app(new string[] { "Redis", "ConnectionString" });
            //var redisOptons = _redisOptions.Value;

            //var roleServiceObjNew = App.GetService<IBaseService<Role, RoleVo>>(false);
            //var roleList = await roleServiceObjNew.Query();
            //var redisOptons = App.GetOptions<RedisOptions>();

            //var cachKey = "cach-key";
            //List<string> cachKeys = await _caching.GetAllCacheKeysAsync();
            //await Console.Out.WriteLineAsync("全部keys --> "+ JsonConvert.SerializeObject(cachKeys));

            //await Console.Out.WriteLineAsync("添加一个缓存");
            //await _caching.SetStringAsync(cachKey, "你好！！！！");
            //await Console.Out.WriteLineAsync("全部keys --> " + JsonConvert.SerializeObject(await _caching.GetAllCacheKeysAsync()));
            //await Console.Out.WriteLineAsync("当前key内容 -->" + JsonConvert.SerializeObject(await _caching.GetStringAsync(cachKey)));

            //await Console.Out.WriteLineAsync("删除一个缓存");
            //await _caching.RemoveAsync(cachKey);
            //await Console.Out.WriteLineAsync("全部keys --> " + JsonConvert.SerializeObject(await _caching.GetAllCacheKeysAsync()));

            return roleList;
        }

        [HttpGet("[action]")]
        public async Task<object> GetAuditSqlLogList()
        {
            TimeSpan timeSpan = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var id = timeSpan.TotalSeconds.ObjToLong();
            await _aduitSqlLogService.AddSplit(new AuditSqlLog()
            {
                Id = id,
                DateTime = Convert.ToDateTime("2023-12-23"),
            });

            var resultList = await _aduitSqlLogService.QuerySplit(t => t.DateTime <= Convert.ToDateTime("2023-12-24"));

            Console.WriteLine("api request end...");
            return resultList;
        }
    }
}
