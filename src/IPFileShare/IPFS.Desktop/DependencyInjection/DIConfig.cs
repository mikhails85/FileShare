using IPFS.Integration;
using IPFS.Integration.Abstractions;
using IPFS.Integration.Messages;
using IPFS.Runner;
using IPFS.Utils.DI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

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
                            .WriteTo.File(@"Logs/app-log.txt", 
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