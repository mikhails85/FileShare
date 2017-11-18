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

namespace IPFS.Integration.Tests.Features.UserCase
{
    public partial class ContentManifestManagment: FeatureFixture
    {
        private RESTClient client;
        
        private Result<IPFSHash> result; 
        
        private void Given_client()
        {
            client = new RESTClient("http://0.0.0.0:8081/");
        }
        
        private void When_user_get_list_of_local_files()
        {
            
        }
        
        private void Then_client_should_update_manifest()
        {
            
        }
        
        private void Then_client_should_add_new_manifest()
        {
            
        }
        
        private void Then_get_manifest_by_peer_id()
        {
            
        }
        
        private void Then_manifest_should_be_same()
        {
            
        }
        
       
        public ContentManifestManagment(ITestOutputHelper output):base(output)
        {
            Logger.SetupLogger(new DummyLogger());
        }
    }
}