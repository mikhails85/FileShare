using IPFS.Integration;
using IPFS.Integration.Messages;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using IPFS.Runner;
using IPFS.Results;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IPFS.Integration.Abstractions;
using Serilog;
using IPFS.Utils.DI;

namespace IPFS.Client
{
    class Program
    {
        private static IServiceProvider serviceProvider;
        private static IConfigurationRoot configuration;
        
        [STAThread]
        static async Task Main(string[] args)
        {
            SetupConfiguration();
            
            var config = GetConfig();
                
            Console.WriteLine(config);
            
            var processResult = await ProcessManager.StartProcess("MyClient", config);     
            
            if(!processResult.Success)
            {
                Console.WriteLine(processResult.Success ? "Success" : processResult.Errors[0].Message);
                return;
            }
            
            ProcessManager.OnExit = (exitResult) => { 
                Console.WriteLine(exitResult.Success ? "Success" : exitResult.Errors[0].Message);
            };
            
            var rest = serviceProvider.GetService<IIPFSClient>();
        
            var result = await rest.Message<GetPeerInformation>().SendAsync();
        
            Console.WriteLine(result.Success ? "Success" : result.Errors[0].Message);
            
            Console.WriteLine("Ready to Stop");
            Console.ReadKey();
            
            ProcessManager.StopProcess();
            
            Console.WriteLine("Done");
        }
        
        private static ProcessConfig GetConfig()
        {
            var processConfig = new ProcessConfig();
            configuration.GetSection("ServiceRunner").Bind(processConfig);
            return processConfig;
        }
        
        private static void SetupConfiguration()
        {
            serviceProvider = new ServiceCollection()
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
                
                return new RESTClient("http://0.0.0.0:6001/", messageProvider);
            })
            .AutoRegisterInstanceOf<IApiMessage>()
            .BuildServiceProvider();    
            
             var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

            configuration = builder.Build();
        }
    }
}
