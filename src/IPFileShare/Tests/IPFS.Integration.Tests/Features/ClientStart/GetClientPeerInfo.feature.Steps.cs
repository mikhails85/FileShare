using IPFS.Integration.Messages;
using IPFS.Integration.Models;
using IPFS.Integration.Tests.Helpers;
using IPFS.Results;
using LightBDD.Framework;
using LightBDD.Framework.Commenting;
using LightBDD.XUnit2;
using Xunit;
using Xunit.Abstractions;
using IPFS.Integration.Abstractions;
using IPFS.Integration.Tests.Features;

namespace IPFS.Integration.Tests.Features.ClientStart
{
    public partial class GetClientPeerInfo: BaseFeature
    {
       
        private Result<PeerInfo> result; 
        
        private async void When_user_send_peer_info_request()
        {
            result = await client.Message<GetPeerInformation>().SendAsync();
        }
        
        private void Then_client_should_return_peer_information()
        {
            Assert.True(result.Success);
            StepExecution.Current.Comment($"PeerID: '{result.Value.ID}'");
        }
        
       
        public GetClientPeerInfo(ITestOutputHelper output):base(output)
        {
        }
    }
}