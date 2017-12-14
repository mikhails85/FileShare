using IPFS.Desktop.Bridge.AppConfig;
using IPFS.Desktop.Bridge.EventHandlers;
using IPFS.Runner;
using IPFS.Utils.Logger;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;

namespace IPFS.Desktop.Bridge
{
    class Program
    {
        
        static async Task Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(ProcessExit);
            
            AppConfig.AppConfig.SetupConfig();
            AppDI.SetupDI();
            
            var processResult = await ProcessManager.StartProcess("IPFSClient", AppConfig.AppConfig.ProcessConfig);     
            
            if(!processResult.Success)
            {
                AppDI.ServiceProvider.GetService<ILogger<Program>>().Result(processResult,"Run IPFS process");
                return;
            }
            
            AppBridge.SetupPort(args);
            BootstrapEventHandlers();

            Console.ReadKey();
        }

        static void BootstrapEventHandlers()
        {
            AppBridge.RegistrateEventHandler(AppDI.ServiceProvider.GetService<ExampleEventHandler>());
        }
        
        static void ProcessExit(object sender, EventArgs e)
        {
            ProcessManager.StopProcess();
        }
    }
}
