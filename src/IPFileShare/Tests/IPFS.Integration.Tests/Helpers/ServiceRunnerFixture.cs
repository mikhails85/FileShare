using IPFS.Integration.Abstractions;
using IPFS.Integration.Messages;
using IPFS.Runner;
using IPFS.Utils.DI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.IO;

namespace IPFS.Integration.Tests.Helpers
{
    public class ServiceRunnerFixture: IDisposable
    {
        public static IServiceProvider ServiceProvider;
        public static IConfigurationRoot Configuration;
        private static ProcessConfig ProcessConfig;
        
        public ServiceRunnerFixture()
        {
            SetupConfig();
                
            SetupDI();
            
            Console.WriteLine(ProcessConfig);
            
            var processResult = ProcessManager.StartProcess("TestClient", ProcessConfig).Result;     
        }

        public void Dispose()
        {
            ProcessManager.StopProcess();
        }
        
        private static void SetupConfig()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

            Configuration = builder.Build();
            
            ProcessConfig = new ProcessConfig();
            Configuration.GetSection("ServiceRunner").Bind(ProcessConfig);
        }
        
        private static void SetupDI()
        {
            ServiceProvider = new ServiceCollection()
            .AddSingleton<Serilog.ILogger>((ctx)=>{
                return new Serilog.LoggerConfiguration()
                            .MinimumLevel.Debug()
                            .WriteTo.Console()
                            .CreateLogger();
            })
            .AddTransient(typeof(IPFS.Utils.Logger.ILogger<>), typeof(IPFS.Utils.Logger.Implementation.SeriLogger<>))
            .AddSingleton<IMessageProvider, IPFSMessageProvider>()
            .AddSingleton<IIPFSClient>((ctx) =>
            {
                var messageProvider = ctx.GetService<IMessageProvider>();
                
                return new RESTClient(ProcessConfig.API, messageProvider);
            })
            .AutoRegisterInstanceOf<IApiMessage>()
            .BuildServiceProvider();    
        }
    }
}