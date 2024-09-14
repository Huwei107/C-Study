global using BCVP.Net8.Common.Core;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using BCVP.Net8.Common;
using BCVP.Net8.Common.Option;
using BCVP.Net8.Extension;
using BCVP.Net8.Extension.ServiceExtensions;
using BCVP.Net8.Extensions;
using BCVP.Net8.Extensions.ServiceExtensions;
using BCVP.Net8.IService;
using BCVP.Net8.Repository;
using BCVP.Net8.Service;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;


namespace BCVP.Net8
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Autofac依赖注入
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>(builder =>
                {
                    builder.RegisterModule<AutofacModuleRegister>();
                    //属性注入可能会用到api层的类
                    builder.RegisterModule<AutofacPropertityModuleRegister>();
                })
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    hostingContext.Configuration.ConfigureApplication();
                });
            builder.ConfigureApplication();

            //开启控制器
            //Replace是要把IControllerActivator不用DefaultControllerActivator 实现，而是ServiceBasedControllerActivator实现
            builder.Services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());
            //builder.Services.AddControllers().AddControllersAsServices();
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //添加AutoMapper
            builder.Services.AddAutoMapper(typeof(AutoMapperConfig));
            AutoMapperConfig.RegisterMappings();

            //配置Appsettings单例
            builder.Services.AddSingleton(new AppSettings(builder.Configuration));
            //配置IOptions
            builder.Services.AddAllOptionRegister();

            //缓存
            builder.Services.AddCacheSetup();

            //sqlsugar
            builder.Services.AddSqlsugarSetup();

            #region 原生依赖注入
            //Scoped 从请求开始到结束 
            //builder.Services.AddScoped(typeof(IBaseService<,>), typeof(BaseSerivce<,>));
            //builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            #endregion


            var app = builder.Build();
            app.ConfigureApplication();
            app.UseApplicationSetup();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            //处理token
            app.UseAuthorization();


            app.MapControllers();//路由映射

            app.Run();
        }
    }
}
