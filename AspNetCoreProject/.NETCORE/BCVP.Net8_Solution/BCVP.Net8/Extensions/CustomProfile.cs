using AutoMapper;
using BCVP.Net8.Model;

namespace BCVP.Net8.Extensions
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
            CreateMap<User, UserVo>()
                .ForMember(x => x.UserName, y => y.MapFrom(z => z.Name));
        }
    }
}
