using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.IoC;
using EZNEW.Mvc.CustomModelDisplayName;
using EZNEW.Mvc.DataAnnotationsModelValidatorConfig;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Site.Cms.Config;

namespace Site.Cms
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            ContainerFactory.RegisterServices(services);
            services.AddHttpContextAccessor();
            services.AddMvc().AddMvcOptions(o =>
            {
                o.ModelValidatorProviders.Add(new CustomDataAnnotationsModelValidatorProvider());
                o.ModelMetadataDetailsProviders.Add(new MvcCustomModelDisplayProvider());

            }).AddViewOptions(vo =>
            {
                vo.ClientModelValidatorProviders.Add(new CustomDataAnnotationsClientModelValidatorProvider());
            });
            var serviceProvider = ContainerFactory.GetServiceProvider();
            return serviceProvider;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            AppConfig.Init();
        }
    }
}
