using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace IPFS.Desktop.Api
{
    public class Program
    {
        public static string ApiPort {get;set;}
        
        public static string IPFSPort {get;set;}
        
        public static void Main(string[] args)
        {
            if(!ParseSocketPort(args))
            {
                Console.WriteLine("'Api Port' and 'IPFS Port' required!");
                return;
            }
            
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
                .UseUrls(GetApiUrl())
                .UseSerilog() 
                .Build();
                
        public static string GetApiUrl()
        {
            return $"http://0.0.0.0:{ApiPort}";
        }
        
        public static string GetIPFSUrl()
        {
            return $"http://0.0.0.0:{IPFSPort}";
        }
        
        private static bool ParseSocketPort(string[] args)
        {
            foreach (string argument in args)
            {
                if (argument.ToUpper().Contains("APIPORT"))
                {
                    ApiPort = argument.ToUpper().Replace("/APIPORT=", "");
                    Console.WriteLine("Use API Port: " + ApiPort);
                }
                
                if (argument.ToUpper().Contains("IPFSPORT"))
                {
                    IPFSPort = argument.ToUpper().Replace("/IPFSPORT=", "");
                    Console.WriteLine("IPFS Run On Port: " + IPFSPort);
                }
            }
            
            return !string.IsNullOrWhiteSpace(ApiPort) && !string.IsNullOrWhiteSpace(IPFSPort);
        }
    }
}
