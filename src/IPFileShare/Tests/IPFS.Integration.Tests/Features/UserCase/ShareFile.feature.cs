using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Basic;
using LightBDD.XUnit2;
using System.Threading.Tasks;

namespace IPFS.Integration.Tests.Features.UserCase
{
    [FeatureDescription(
        @"As a user
        I want share my file")]
    public partial class ShareFile
    {
        [Scenario]
        public async Task Get_Peer_Inforation()
        {
            await Runner.RunScenarioActionsAsync(
                Given_client,
                When_user_share_file,
                Then_client_should_return_file_id,
                Then_client_should_get_file_information,
                Then_client_should_download_file);
        }
    }
}