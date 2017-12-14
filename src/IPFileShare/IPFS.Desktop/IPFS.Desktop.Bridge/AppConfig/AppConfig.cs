﻿using IPFS.Runner;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection;

namespace IPFS.Desktop.Bridge.AppConfig
{
    public static class AppConfig
    {
        public static IConfigurationRoot Configuration { get; private set; }
        public static ProcessConfig ProcessConfig { get; private set; }

        public static void SetupConfig()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location))
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

            Configuration = builder.Build();

            ProcessConfig = new ProcessConfig();
            Configuration.GetSection("ServiceRunner").Bind(ProcessConfig);
        }
    }
}
