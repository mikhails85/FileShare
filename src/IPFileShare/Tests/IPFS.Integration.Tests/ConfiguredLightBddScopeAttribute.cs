using IPFS.Integration.Tests;
using IPFS.Integration.Tests.Helpers;
using LightBDD.Core.Configuration;
using LightBDD.Framework.Reporting.Configuration;
using LightBDD.Framework.Reporting.Formatters;
using LightBDD.XUnit2;

[assembly: ConfiguredLightBddScope]
namespace IPFS.Integration.Tests
{
    public class ConfiguredLightBddScopeAttribute : LightBddScopeAttribute
    {
        private ServiceRunnerFixture runner;
        protected override void OnConfigure(LightBddConfiguration configuration)
        {
            runner = new ServiceRunnerFixture();
            
            configuration
                .ReportWritersConfiguration()
                .AddFileWriter<PlainTextReportFormatter>("~{TestDateTimeUtc:yyyy-MM-dd-HH_mm_ss}_FeaturesReport.txt");
        }
        
        protected override void OnTearDown()
        {
            runner.Dispose();
        }
    }
}