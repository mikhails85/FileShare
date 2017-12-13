using IPFS.Desktop.Bridge.AppConfig;
using IPFS.Desktop.Bridge.EventHandlers;
using Microsoft.Extensions.DependencyInjection;
using IPFS.Integration;
using IPFS.Integration.Abstractions;
using IPFS.Integration.Messages;
using IPFS.Runner;
using IPFS.Utils.DI;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;
using IPFS.Utils.Logger;

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
