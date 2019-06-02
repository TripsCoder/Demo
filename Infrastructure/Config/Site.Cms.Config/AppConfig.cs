using App.DBConfig;
using App.Mapper;
using EZNEW.Framework.ObjectMap;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Site.Cms.Config
{
    public static class AppConfig
    {
        public static void Init()
        {
            //数据验证
            DataValidationConfig.Init();
            //显示验证
            DisplayConfig.Init();
            //对象转换映射
            ObjectMapManager.ObjectMapper = MapperFactory.ObjectMapper;
            //数据库配置
            DbConfig.Init();
            //对象Id生成初始化
            IdentityKeyConfig.Init();
        }
    }
}
