using Autofac;
using Autofac.Extras.DynamicProxy;
using BCVP.Net8.IService;
using BCVP.Net8.Repository;
using BCVP.Net8.Repository.UnitOfWorks;
using BCVP.Net8.Service;
using System.Reflection;

namespace BCVP.Net8.Extension
{
    public class AutofacModuleRegister : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var basePath = AppContext.BaseDirectory;

            //获取服务层和仓储层的程序集路径
            var serviceDllFile = Path.Combine(basePath, "BCVP.Net8.Service.dll");
            var repositoryDllFile = Path.Combine(basePath, "BCVP.Net8.Repository.dll");

            var aopTypes = new List<Type>() { typeof(ServiceAOP)};
            builder.RegisterType<ServiceAOP>();

            builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IBaseRepository<>)).InstancePerDependency();
            builder.RegisterGeneric(typeof(BaseService<,>)).As(typeof(IBaseService<,>))
                .InstancePerDependency()
                .EnableInterfaceInterceptors()
                .InterceptedBy(aopTypes.ToArray());

            //获取Service.dll程序集服务，并注册
            var assemblyService = Assembly.LoadFrom(serviceDllFile);
            builder.RegisterAssemblyTypes(assemblyService)
                .AsImplementedInterfaces()
                .InstancePerDependency()
                .PropertiesAutowired()
                .EnableInterfaceInterceptors()
                .InterceptedBy(aopTypes.ToArray());

            //Repository.dll程序集服务，并注册
            var assemblyRepositry = Assembly.LoadFrom(repositoryDllFile);
            builder.RegisterAssemblyTypes(assemblyRepositry)
                .AsImplementedInterfaces()
                .InstancePerDependency()
                .PropertiesAutowired();

            builder.RegisterType<UnitOfWorkManage>().As<IUnitOfWorkManage>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();
        }
    }
}
