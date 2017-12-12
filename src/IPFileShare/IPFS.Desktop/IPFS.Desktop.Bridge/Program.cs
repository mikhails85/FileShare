using IPFS.Desktop.Bridge.AppConfig;
using IPFS.Desktop.Bridge.EventHandlers;
using Microsoft.Extensions.DependencyInjection;

namespace IPFS.Desktop.Bridge
{
    class Program
    {
        static void Main(string[] args)
        {
            AppConfig.AppConfig.SetupConfig();
            AppDI.SetupDI();
            AppBridge.SetupPort(args);

            BootstrapEventHandlers();
        }

        static void BootstrapEventHandlers()
        {
            AppBridge.RegistrateEventHandler(AppDI.ServiceProvider.GetService<ExampleEventHandler>());
        }
    }
}
