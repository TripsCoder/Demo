using EZNEW.Framework.Drawing;
using EZNEW.Framework.IoC;
using EZNEW.Framework.Serialize;
using EZNEW.Web.Utility;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using EZNEW.Web.DI;

namespace App.IoC
{
    public static class ContainerFactory
    {
        public static IServiceProvider GetServiceProvider()
        {
            return ContainerManager.BuildServiceProvider();
        }

        public static void RegisterServices(IServiceCollection services)
        {
            ContainerManager.Init(services, serviceRegisterAction: RegisterServices);
            RepositoryConfig.Init();
        }

        /// <summary>
        /// 自定义服务注入
        /// </summary>
        /// <param name="container"></param>
        static void RegisterServices(IDIContainer container)
        {
            WebDependencyInjectionManager.RegisterDefaultService();
        }
    }
}
