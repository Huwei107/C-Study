using BCVP.Net8.Model;
using BCVP.Net8.Model.Vo;

namespace BCVP.Net8.IService
{
    public interface IUserService
    {
        Task<List<UserVo>> Query();

        Task<bool> TestTranPropagation();
    }
}
