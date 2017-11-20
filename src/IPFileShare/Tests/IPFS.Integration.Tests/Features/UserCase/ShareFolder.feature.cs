using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Basic;
using LightBDD.XUnit2;
using System.Threading.Tasks;

namespace IPFS.Integration.Tests.Features.UserCase
{
    [FeatureDescription(
        @"As a user
        I want share my folder")]
    public partial class ShareFolder
    {
        [Scenario]
        public async Task Get_Peer_Inforation()
        {
            await Runner.RunScenarioActionsAsync(
                Given_client,
                When_user_share_folder,
                Then_client_should_return_folder_id,
                Then_client_should_get_folder_information);
        }
    }
}