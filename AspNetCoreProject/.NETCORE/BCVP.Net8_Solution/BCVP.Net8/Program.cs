
using Autofac;
using Autofac.Extensions.DependencyInjection;
using BCVP.Net8.Common;
using BCVP.Net8.Common.Option;
using BCVP.Net8.Extension;
using BCVP.Net8.Extensions;
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

            //Autofac����ע��
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>(builder =>
                {
                    builder.RegisterModule<AutofacModuleRegister>();
                    //����ע����ܻ��õ�api�����
                    builder.RegisterModule<AutofacPropertityModuleRegister>();
                })
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    hostingContext.Configuration.ConfigureApplication();
                })
                ;
            //����������
            builder.Services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //���AutoMapper
            builder.Services.AddAutoMapper(typeof(AutoMapperConfig));
            AutoMapperConfig.RegisterMappings();

            //����Appsettings����
            builder.Services.AddSingleton(new AppSettings(builder.Configuration));
            //����IOptions
            builder.Services.AddAllOptionRegister();

            #region ԭ������ע��
            //Scoped ������ʼ������ 
            //builder.Services.AddScoped(typeof(IBaseService<,>), typeof(BaseSerivce<,>));
            //builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            #endregion

            //Autofac����ע��


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();//·��ӳ��

            app.Run();
        }
    }
}
