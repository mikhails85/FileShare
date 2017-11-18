using System.Threading.Tasks;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Basic;
using LightBDD.XUnit2;

namespace IPFS.Integration.Tests.Features.ClientStart
{
    [FeatureDescription(
        @"In order to run Client
        As a user
        I want to see my peer information")]
    //https://github.com/LightBDD/LightBDD/blob/master/examples/LightBDD.Example.AcceptanceTests.XUnit2/Features/Basket_feature.cs
    public partial class GetClientPeerInfo
    {
        [Scenario]
        public async Task Get_Peer_Inforation()
        {
            await Runner.RunScenarioActionsAsync(
                Given_client,
                When_user_send_peer_info_request,
                Then_client_should_return_peer_information);
        }
    }
}