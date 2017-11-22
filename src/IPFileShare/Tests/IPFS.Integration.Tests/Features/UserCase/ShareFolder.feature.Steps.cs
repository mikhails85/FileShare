using IPFS.Integration.Messages;
using IPFS.Integration.Models;
using IPFS.Integration.Tests.Helpers;
using IPFS.Integration.Utils.Log;
using IPFS.Results;
using LightBDD.Framework;
using LightBDD.Framework.Commenting;
using LightBDD.XUnit2;
using Xunit;
using Xunit.Abstractions;

namespace IPFS.Integration.Tests.Features.UserCase
{
    public partial class ShareFolder: FeatureFixture
    {
        private RESTClient client;
        
        private Result<IPFSHash> result; 
        
        private void Given_client()
        {
            client = new RESTClient("http://0.0.0.0:7002/");
        }
        
        private async void When_user_share_folder()
        {
            result = await client.Message<AddFolderMessage>().SendAsync("./TestData");
        }
        
        private void Then_client_should_return_folder_id()
        {
            Assert.True(result.Success);
            StepExecution.Current.Comment($"FolderID: '{result.Value.Hash}'");
        }
        
        private async void Then_client_should_get_folder_information()
        {
            var info = await client.Message<GetFileInfoMessage>().SendAsync(result.Value.Hash);
            Assert.True(info.Success);
            Assert.True(info.Value.IsDirectory);
        }
       
        public ShareFolder(ITestOutputHelper output):base(output)
        {
            Logger.SetupLogger(new DummyLogger());
        }
    }
}