using IPFS.Integration;
using IPFS.Integration.Messages;
using IPFS.Integration.Utils.Log;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using IPFS.Runner;
using IPFS.Results;
using Microsoft.Extensions.Configuration;

namespace IPFS.Client
{
    class Program
    {
        [STAThread]
        static async Task Main(string[] args)
        {
            Logger.SetupLogger(new ClientLogger());
            
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
            
            var rest = new RESTClient("http://0.0.0.0:6001/");
        
            var result = await rest.Message<GetPeerInformation>().SendAsync();
        
            Console.WriteLine(result.Success ? "Success" : result.Errors[0].Message);
            
            Console.WriteLine("Ready to Stop");
            Console.ReadKey();
            
            ProcessManager.StopProcess();
            
            Console.WriteLine("Done");
        }
        
        private static ProcessConfig GetConfig()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

            IConfigurationRoot configuration = builder.Build();

            var processConfig = new ProcessConfig();
            configuration.GetSection("ServiceRunner").Bind(processConfig);
            return processConfig;
        }
    }
}
