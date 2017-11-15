using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IPFS.Results;
using System.Threading.Tasks;
using IPFS.Integration.Models;
using IPFS.Integration.Utils.Log;
using Newtonsoft.Json;
using System.IO;

namespace IPFS.Integration.Messages
{
    public class GetContentManifestMessage: IApiMessage
    {
        public RESTClient Client { get; set;}
        
        private ILogger Log => Logger.Log<GetContentManifestMessage>();
        
        public async Task<Result<ContentManifest>> SendAsync(string hash)
        {
            var result = new Result<ContentManifest>(); 
            
            var response = await this.Client.Message<ReadTextMessage>().SendAsync(hash);
            
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