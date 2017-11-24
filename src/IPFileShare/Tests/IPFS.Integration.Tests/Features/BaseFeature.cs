using IPFS.Integration.Abstractions;
using IPFS.Integration.Tests.Helpers;
using LightBDD.XUnit2;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace IPFS.Integration.Tests.Features
{
    public class BaseFeature : FeatureFixture
    {
        protected IIPFSClient client;
        
        protected void Given_client()
        {
            client = ServiceRunnerFixture.ServiceProvider.GetService<IIPFSClient>();
        }
        
        public BaseFeature(ITestOutputHelper output):base(output)
        {
            
        }
    }
}