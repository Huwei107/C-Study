using BCVP.Net8.IService;
using BCVP.Net8.Model;
using BCVP.Net8.Repository;
using Newtonsoft.Json;

namespace BCVP.Net8.Service
{
    public class UserService : IUserService
    {
        public async Task<List<UserVo>> Query()
        {
            var userRepo = new UserRepository();
            var users = await userRepo.Query();
            return users.Select(d => new UserVo()
            {
                UserName = d.Name ?? string.Empty
            })
            .ToList();
        }
    }
}
