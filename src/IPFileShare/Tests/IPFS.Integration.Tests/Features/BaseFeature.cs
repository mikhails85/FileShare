using System;
using System.Collections.Generic;
using System.Linq;
using IPFS.Integration.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using IPFS.Integration.Tests.Helpers;
using LightBDD.Framework;
using LightBDD.Framework.Commenting;
using LightBDD.XUnit2;
using Xunit;
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