using IPFS.Integration.Messages;
using IPFS.Integration.Models;
using IPFS.Integration.Tests.Helpers;
using IPFS.Results;
using LightBDD.Framework;
using LightBDD.Framework.Commenting;
using LightBDD.XUnit2;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using IPFS.Integration.Abstractions;
using IPFS.Integration.Tests.Features;

namespace IPFS.Integration.Tests.Features.ClientStart
{
    public partial class GetPeerList: BaseFeature
    {
        
        private Result<List<LatencyPeer>> result; 
        
        private async void When_user_send_peer_list_request()
        {
            result = await client.Message<GetPeersMessage>().SendAsync();
        }
        
        private void Then_client_should_return_peer_list()
        {
            Assert.True(result.Success);
            StepExecution.Current.Comment($"Peers: '{result.Value.Count()}'");
        }
        
       
        public GetPeerList(ITestOutputHelper output):base(output)
        {
            
        }
    }
}