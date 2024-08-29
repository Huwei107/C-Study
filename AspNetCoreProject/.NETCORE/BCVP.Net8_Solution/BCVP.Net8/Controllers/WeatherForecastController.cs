using AutoMapper;
using BCVP.Net8.IService;
using BCVP.Net8.Model;
using BCVP.Net8.Service;
using Microsoft.AspNetCore.Mvc;

namespace BCVP.Net8.Controllers
{
    public class WeatherForecastController : ApiControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IBaseService<Role, RoleVo> _roleService;

        public IBaseService<Role, RoleVo> _roleServiceObj { get; set; }

        public WeatherForecastController(ILogger<WeatherForecastController> logger, 
            IBaseService<Role,RoleVo> roleService)
        {
            _logger = logger;
            _roleService = roleService;
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

            var roleList = await _roleServiceObj.Query();

            return roleList;
        }
    }
}
