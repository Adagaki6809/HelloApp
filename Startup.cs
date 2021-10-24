using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloApp
{
    public class Startup
    {
        IWebHostEnvironment _env;
        public Startup(IWebHostEnvironment env)
        {
            _env = env;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        { 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseMiddleware<AuthenticationMiddleware>();
            app.UseMiddleware<RoutingMiddleware>();
            
            app.UseRouting();
            //int x = 2;
            
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("<p>Hello World!</p>");
                });
                endpoints.MapGet("/test", async context =>
                {
                    await context.Response.WriteAsync($"<p>Application name: {_env.ApplicationName}.</p>");
                });
            });
            
            int x = 5;
            int y = 8;
            int z = 0;
            app.Use(async (context, next) =>
            {
                
                z = x * y;
                await context.Response.WriteAsync($"<p>Message:</p>");
                await next.Invoke();
            });
            app.Run(async (context) =>
            {
                await Task.Delay(10000);
                await context.Response.WriteAsync($"<p> x*y = {z}</p>");

            });

        }
    }
}
