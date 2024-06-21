﻿using AutoMapper;

namespace BCVP.Net8
{
    public class AutoMapperConfig
    {
        /// <summary>
        /// 静态全局 AutoMapper 配置文件
        /// </summary>
        /// <returns></returns>
        public static MapperConfiguration RegisterMappings()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new CustomProfile());
            });
        }
    }
}