using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Events;
using System;

namespace IPFS_Desktop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.File(@"Logs/sys-log.txt", 
                                  rollingInterval: RollingInterval.Day, 
                                  outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            try
            {
                Log.Information("Starting web host");
                BuildWebHost(args).Run();
               
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls("http://0.0.0.0:8080")
                .UseSerilog() // <-- Add this line
                .Build();
    }
}
