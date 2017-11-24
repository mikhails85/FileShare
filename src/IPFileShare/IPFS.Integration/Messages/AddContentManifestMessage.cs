using IPFS.Integration.Models;
using IPFS.Integration.Abstractions;
using IPFS.Utils.Logger;
using IPFS.Results;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace IPFS.Integration.Messages
{
    public class AddContentManifestMessage : IApiMessage
    {
        public IIPFSClient Client { get; set;}
        
        private readonly ILogger<AddContentManifestMessage> Log;
        
        public AddContentManifestMessage(ILogger<AddContentManifestMessage> logger)
        {
            this.Log = logger;
        }
        
        public async Task<Result<IPFSHash>> SendAsync(ContentManifest manifest)
        {
            var result = new Result<IPFSHash>(); 
            
            var request = JsonConvert.SerializeObject(manifest);
            var body = new MemoryStream(Encoding.UTF8.GetBytes(request), false);
            var url = UrlResolver.GetAddUrl(this.Client.GatewayUrl);
            
            Log.WarningMessage(url.ToString());
            
            var response = await this.Client.UploadAsync(url, body);
            if(!response.Success)
            {
                result.AddErrors(response.Errors);
                return result;
            }
            
            var hash = JsonConvert.DeserializeObject<IPFSHash>(response.Value);
            
            var publishResult = await this.Client.Message<PublishContentMessage>().SendAsync(hash.Hash);
            
            if(!publishResult.Success)
            {
                result.AddErrors(publishResult.Errors);
                return result;
            }
            
            result.SetValue(hash);
            
            return result;
        }
    }
}