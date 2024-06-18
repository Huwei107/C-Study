using BCVP.Net8.IService;
using BCVP.Net8.Model;
using BCVP.Net8.Service;
using Microsoft.AspNetCore.Mvc;

namespace BCVP.Net8.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet(Name = "GetUserList")]
        public async Task<List<UserVo>> GetUserList()
        {
            var userSerivce = new UserService();
            var userList = await userSerivce.Query();
            return userList;
        }
    }
}
