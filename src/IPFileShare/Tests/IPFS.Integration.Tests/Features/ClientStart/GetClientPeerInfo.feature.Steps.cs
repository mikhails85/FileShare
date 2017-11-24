using IPFS.Integration.Messages;
using IPFS.Integration.Models;
using IPFS.Results;
using LightBDD.Framework;
using LightBDD.Framework.Commenting;
using Xunit;
using Xunit.Abstractions;

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