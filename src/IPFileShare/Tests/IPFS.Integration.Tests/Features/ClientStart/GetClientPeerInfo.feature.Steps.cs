using System.Linq;
using LightBDD.Framework;
using LightBDD.Framework.Commenting;
using LightBDD.XUnit2;
using Xunit;
using Xunit.Abstractions;
using IPFS.Integration;
using IPFS.Integration.Messages;
using IPFS.Integration.Models;
using IPFS.Results;
using IPFS.Integration.Utils.Log;
using IPFS.Integration.Tests.Helpers;
using System.Threading.Tasks;

namespace IPFS.Integration.Tests.Features.ClientStart
{
    public partial class GetClientPeerInfo: FeatureFixture
    {
        private RESTClient client;
        
        private Result<PeerInfo> result; 
        
        private void Given_client()
        {
            client = new RESTClient("http://0.0.0.0:8081/");
        }
        
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
            Logger.SetupLogger(new DummyLogger());
        }
    }
}