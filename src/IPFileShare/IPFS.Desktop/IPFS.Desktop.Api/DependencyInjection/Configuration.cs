using IPFS.Integration;
using IPFS.Integration.Abstractions;
using IPFS.Integration.Messages;
using IPFS.Utils.DI;
using IPFS.Desktop.Api;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace IPFS.Desktop.Api.DependencyInjection
{
    public static class Configuration
    {
        public static void AddDependencyInjection(this IServiceCollection services, IConfiguration config)
        {
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
                
                return new RESTClient(Program.GetIPFSUrl(), messageProvider);
            })
            .AutoRegisterInstanceOf<IApiMessage>()
            .AutoRegisterInjectable();
        }
    }
}