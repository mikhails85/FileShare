using System.Threading.Tasks;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Basic;
using LightBDD.XUnit2;

namespace IPFS.Integration.Tests.Features.ClientStart
{
    [FeatureDescription(
        @"In order to run Client
        As a user
        I want to see available peers")]
    //https://github.com/LightBDD/LightBDD/blob/master/examples/LightBDD.Example.AcceptanceTests.XUnit2/Features/Basket_feature.cs
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