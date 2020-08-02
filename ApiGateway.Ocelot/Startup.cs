using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace ApiGateway.Ocelot
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddAuthentication()
              .AddJwtBearer("Api_Catalog", i =>
              {
                  i.Audience = "Api_Catalog";
                  i.Authority = "http://localhost:5333";
                  i.RequireHttpsMetadata = false;
              }).AddJwtBearer("Api_Ordering", y =>
              {
                  //资源名称，跟认证服务中注册的资源列表名称中的apiResource一致
                  y.Audience = "Api_Ordering";
                  //配置授权认证的地址
                  y.Authority = "http://localhost:5333";
                  //是否启用https
                  y.RequireHttpsMetadata = false;
              });

            services.AddOcelot();//注入Ocelot服务

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseOcelot().Wait();//使用Ocelot中间件

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
