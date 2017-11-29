using IPFS.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using IPFS.Desktop.DependencyInjection;
using IPFS.Desktop.ControllerExtensions;

namespace IPFS_Desktop
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            StartProcess();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDependencyInjection(this.Configuration);
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime applicationLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseApiExtension();
            
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
            
            applicationLifetime.ApplicationStopping.Register(StopProcess);
            
        }
        
        private void StartProcess()
        {
            var processConfig = new ProcessConfig();
            Configuration.GetSection("ServiceRunner").Bind(processConfig);
            var processResult = ProcessManager.StartProcess("DesktopClient", processConfig).Result;     
            if(!processResult.Success)
            {
                throw new Exception(processResult.Errors[0].Message);
            }
        }
        
        private void StopProcess()
        {
            ProcessManager.StopProcess();
        }
    }
}
