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
    public partial class ShareFile: FeatureFixture
    {
        private RESTClient client;
        
        private Result<IPFSHash> result; 
        
        private void Given_client()
        {
            client = new RESTClient("http://0.0.0.0:8081/");
        }
        
        private async void When_user_share_file()
        {
            result = await client.Message<AddFileMessage>().SendAsync("./TestData/01. Patron Saint O Thieves.mp3");
        }
        
        private void Then_client_should_return_file_id()
        {
            Assert.True(result.Success);
            StepExecution.Current.Comment($"FileID: '{result.Value.Hash}'");
        }
        
        private async void Then_client_should_get_file_information()
        {
            var info = await client.Message<GetFileInfoMessage>().SendAsync(result.Value.Hash);
            Assert.True(info.Success);
            Assert.False(info.Value.IsDirectory);
        }
       
        private async void Then_client_should_download_file()
        {
            var file = await client.Message<DownloadFileMessage>().SendAsync(result.Value.Hash);
            Assert.True(file.Success);
            file.Value.Dispose();
        }
        
        public ShareFile(ITestOutputHelper output):base(output)
        {
            Logger.SetupLogger(new DummyLogger());
        }
    }
}