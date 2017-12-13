using IPFS.Integration;
using IPFS.Integration.Abstractions;
using IPFS.Integration.Messages;
using IPFS.Utils.DI;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using IPFS.Desktop.Bridge.EventHandlers;

namespace IPFS.Desktop.Bridge.AppConfig
{
    public static class AppDI
    {
        public static ServiceProvider ServiceProvider { get; private set; }

        public static void SetupDI()
        {
            ServiceProvider = new ServiceCollection()
            .AddSingleton<Serilog.ILogger>((ctx) => {
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

                return new RESTClient(AppConfig.ProcessConfig.API, messageProvider);
            })
            .AutoRegisterInstanceOf<IApiMessage>()
            .AutoRegisterInstanceOf<IEventHandler>()
            .AutoRegisterInjectable()
            .BuildServiceProvider();
        }
    }
}
