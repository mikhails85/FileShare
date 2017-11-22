using System;
using System.IO;
using System.Diagnostics;
using Xunit;
using Xunit.Sdk;
using Microsoft.Extensions.Configuration;
using IPFS.Runner;
using IPFS.Results;

namespace IPFS.Integration.Tests.Helpers
{
    public class ServiceRunnerFixture: IDisposable
    {
        public ServiceRunnerFixture()
        {
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