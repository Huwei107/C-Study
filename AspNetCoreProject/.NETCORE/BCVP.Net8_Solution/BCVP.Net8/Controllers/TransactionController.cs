using BCVP.Net8.Common;
using BCVP.Net8.IService;
using BCVP.Net8.Model;
using BCVP.Net8.Model.Vo;
using BCVP.Net8.Repository.UnitOfWorks;
using BCVP.Net8.Service;
using Microsoft.AspNetCore.Mvc;

namespace BCVP.Net8.Controllers
{
    public class TransactionController : ApiControllerBase
    {
        private readonly IBaseService<Role, RoleVo> _roleService;
        private readonly IUserService _userService;
        private readonly IUnitOfWorkManage _unitOfWorkManage;

        public TransactionController(IBaseService<Role, RoleVo> roleService, IUserService userService, IUnitOfWorkManage unitOfWorkManage)
        {
            _roleService = roleService;
            _userService = userService;
            _unitOfWorkManage = unitOfWorkManage;
        }

        [HttpGet("[action]")]
        public async Task<object> Get()
        {
            try
            {
                Console.WriteLine($"Begin Transaction");

                //_unitOfWorkManage.BeginTran();
                using var uow = _unitOfWorkManage.CreateUnitOfWork();
                var roles = await _roleService.Query();
                Console.WriteLine($"1 first time : the count of role is :{roles.Count}");

                //什么是事务，什么是工作单元
                //事务的传播形式，


                Console.WriteLine($"insert a data into the table role now.");
                TimeSpan timeSpan = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                var insertPassword = await _roleService.Add(new Role()
                {
                    Id = timeSpan.TotalSeconds.ObjToLong(),
                    IsDeleted = false,
                    Name = "role name",
                });

                var roles2 = await _roleService.Query();
                Console.WriteLine($"2 second time : the count of role is :{roles2.Count}");


                int ex = 0;
                Console.WriteLine($"There's an exception!!");
                Console.WriteLine($" ");
                int throwEx = 1 / ex;

                uow.Commit();
                //_unitOfWorkManage.CommitTran();
            }
            catch (Exception)
            {
                //_unitOfWorkManage.RollbackTran();
                var roles3 = await _roleService.Query();
                Console.WriteLine($"3 third time : the count of role is :{roles3.Count}");
            }

            return "ok";
        }

        [HttpGet("[action]")]
        public async Task<object> TestTranPropagation()
        {
            return await _userService.TestTranPropagation();
        }

    }
}