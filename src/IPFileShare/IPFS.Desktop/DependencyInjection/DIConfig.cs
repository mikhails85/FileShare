using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Serilog;
using IPFS.Integration;
using IPFS.Integration.Messages;
using IPFS.Integration.Abstractions;
using IPFS.Runner;
using IPFS.Utils.DI;

namespace IPFS.Desktop.DependencyInjection
{
    public static class DIConfig
    {
        public static void AddDependencyInjection(this IServiceCollection services, IConfiguration config)
        {
            var processConfig = new ProcessConfig();
            config.GetSection("ServiceRunner").Bind(processConfig);
            
            services.AddSingleton<Serilog.ILogger>((ctx)=>{
                return new Serilog.LoggerConfiguration()
                            .MinimumLevel.Debug()
                            .WriteTo.File("app-log.txt", 
                                  rollingInterval: RollingInterval.Day, 
                                  outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{u3}] {Message:lj}{NewLine}{Exception}")
                            .CreateLogger();
            })
            .AddTransient(typeof(IPFS.Utils.Logger.ILogger<>), typeof(IPFS.Utils.Logger.Implementation.SeriLogger<>))
            .AddSingleton<IMessageProvider, IPFSMessageProvider>()
            .AddSingleton<IIPFSClient>((ctx) =>
            {
                var messageProvider = ctx.GetService<IMessageProvider>();
                
                return new RESTClient(processConfig.API, messageProvider);
            })
            .AutoRegisterInstanceOf<IApiMessage>();
        }
    }
}