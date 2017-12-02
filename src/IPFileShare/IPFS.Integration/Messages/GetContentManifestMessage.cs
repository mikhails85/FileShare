using IPFS.Integration.Abstractions;
using IPFS.Integration.Models;
using IPFS.Results;
using IPFS.Utils.Logger;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace IPFS.Integration.Messages
{
    public class GetContentManifestMessage: IApiMessage
    {
        public IIPFSClient Client { get; set;}
        
        private readonly ILogger<GetContentManifestMessage> Log;
        
        public GetContentManifestMessage(ILogger<GetContentManifestMessage> logger)
        {
            this.Log = logger;
        }
        
        public async Task<Result<ContentManifest>> SendAsync(string hash)
        {
            var result = new Result<ContentManifest>(); 
            
            var pathResult = await this.Client.Message<ResolvePublishedObjectMessage>().SendAsync(hash);
            
            if(!pathResult.Success)
            {
                result.AddErrors(pathResult.Errors);
                return result;
            }
            
            var response = await this.Client.Message<ReadTextMessage>().SendAsync(pathResult.Value);
            
            if(!response.Success)
            {
                result.AddErrors(response.Errors);
                return result;
            }
            
            var manifest = JsonConvert.DeserializeObject<ContentManifest>(response.Value);
            
            result.SetValue(manifest);
            
            return result;
        }
    }
}