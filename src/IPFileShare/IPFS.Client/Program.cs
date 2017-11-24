using IPFS.Integration;
using IPFS.Integration.Abstractions;
using IPFS.Integration.Messages;
using IPFS.Runner;
using IPFS.Utils.DI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;

namespace IPFS.Client
{
    class Program
    {
        private static IServiceProvider serviceProvider;
        private static IConfigurationRoot configuration;
        private static ProcessConfig processConfig;
        
        [STAThread]
        static async Task Main(string[] args)
        {
            
            SetupConfig();
                
            SetupDI();
            
            Console.WriteLine(processConfig);
            
            var processResult = await ProcessManager.StartProcess("MyClient", processConfig);     
            
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
        
        private static void SetupConfig()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

            configuration = builder.Build();
            
            processConfig = new ProcessConfig();
            configuration.GetSection("ServiceRunner").Bind(processConfig);
        }
        
        private static void SetupDI()
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
                
                return new RESTClient(processConfig.API, messageProvider);
            })
            .AutoRegisterInstanceOf<IApiMessage>()
            .BuildServiceProvider();    
        }
    }
}
