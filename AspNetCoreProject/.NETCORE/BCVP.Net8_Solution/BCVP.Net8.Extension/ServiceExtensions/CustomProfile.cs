using AutoMapper;
using BCVP.Net8.Model;
using BCVP.Net8.Model.Vo;

namespace BCVP.Net8.Extension
{
    public class CustomProfile : Profile
    {
        public CustomProfile()
        {
            User();
        }

        private void User()
        {
            CreateMap<Role, RoleVo>()
                .ForMember(x => x.RoleName, y => y.MapFrom(z => z.Name));

            CreateMap<SysUserInfo, UserVo>()
                .ForMember(x => x.UserName, y => y.MapFrom(z => z.Name));
            CreateMap<UserVo, SysUserInfo>()
                .ForMember(x => x.Name, y => y.MapFrom(z => z.UserName));

            CreateMap<Department, DepartmentVo>();
            CreateMap<DepartmentVo, Department>();
        }
    }
}
