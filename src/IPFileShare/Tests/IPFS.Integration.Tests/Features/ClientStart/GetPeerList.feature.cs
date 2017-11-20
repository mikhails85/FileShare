using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Basic;
using LightBDD.XUnit2;
using System.Threading.Tasks;

namespace IPFS.Integration.Tests.Features.ClientStart
{
    [FeatureDescription(
        @"In order to run Client
        As a user
        I want to see available peers")]
    public partial class GetPeerList
    {
        [Scenario]
        public async Task Get_Peer_List()
        {
            await Runner.RunScenarioActionsAsync(
                Given_client,
                When_user_send_peer_list_request,
                Then_client_should_return_peer_list);
        }
    }
}