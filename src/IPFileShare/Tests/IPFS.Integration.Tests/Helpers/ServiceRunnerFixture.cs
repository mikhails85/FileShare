using System;
using System.IO;
using System.Diagnostics;
using Xunit;
using Xunit.Sdk;
using Microsoft.Extensions.Configuration;
using IPFS.Runner;
using IPFS.Results;
using Microsoft.Extensions.DependencyInjection;
using IPFS.Integration.Abstractions;
using Serilog;
using IPFS.Utils.DI;
using IPFS.Integration.Messages;

namespace IPFS.Integration.Tests.Helpers
{
    public class ServiceRunnerFixture: IDisposable
    {
        public static IServiceProvider ServiceProvider;
        public static IConfigurationRoot Configuration;
        
        public ServiceRunnerFixture()
        {
            SetupConfiguration();
            
            var config = GetConfig();
                
            Console.WriteLine(config);
            
            var processResult = ProcessManager.StartProcess("TestClient", config).Result;     
        }

        public void Dispose()
        {
            ProcessManager.StopProcess();
        }
        
        private ProcessConfig GetConfig()
        {
            var processConfig = new ProcessConfig();
            Configuration.GetSection("ServiceRunner").Bind(processConfig);
            return processConfig;
        }
        
        private void SetupConfiguration()
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
                
                return new RESTClient("http://0.0.0.0:7002/", messageProvider);
            })
            .AutoRegisterInstanceOf<IApiMessage>()
            .BuildServiceProvider();    
            
             var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

            Configuration = builder.Build();
        }
    }
}