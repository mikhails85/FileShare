using System.Threading.Tasks;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Basic;
using LightBDD.XUnit2;

namespace IPFS.Integration.Tests.Features.UserCase
{
    [FeatureDescription(
        @"As a user
        I want share my file")]
    //https://github.com/LightBDD/LightBDD/blob/master/examples/LightBDD.Example.AcceptanceTests.XUnit2/Features/Basket_feature.cs
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