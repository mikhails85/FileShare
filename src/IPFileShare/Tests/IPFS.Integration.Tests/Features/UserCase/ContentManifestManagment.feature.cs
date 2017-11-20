using System.Threading.Tasks;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Basic;
using LightBDD.XUnit2;

namespace IPFS.Integration.Tests.Features.UserCase
{
    [FeatureDescription(
        @"As a user
        I want to update my manifest")]
    //https://github.com/LightBDD/LightBDD/blob/master/examples/LightBDD.Example.AcceptanceTests.XUnit2/Features/Basket_feature.cs
    public partial class ContentManifestManagment
    {
        [Scenario]
        public async Task Get_Peer_Inforation()
        {
            await Runner.RunScenarioActionsAsync(
                Given_client,
                When_user_get_peer_information,
                When_user_get_list_of_local_files,
                Then_client_should_create_manifest,
                Then_client_should_add_new_manifest,
                Then_get_manifest_by_peer_id,
                Then_manifest_should_be_same);
        }
    }
}