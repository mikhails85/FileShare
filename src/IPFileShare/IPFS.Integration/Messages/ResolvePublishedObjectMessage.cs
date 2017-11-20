using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IPFS.Results;
using System.Threading.Tasks;
using IPFS.Integration.Models;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using IPFS.Integration.Utils.Log;

namespace IPFS.Integration.Messages
{
    public class ResolvePublishedObjectMessage: IApiMessage
    {
        public RESTClient Client { get; set;}
        
        private ILogger Log => Logger.Log<ResolvePublishedObjectMessage>();
        
        public async Task<Result<string>> SendAsync(string hash = "")
        {
            var result = new Result<string>();
            
            var url = UrlResolver.GetResolveUrl(Client.GatewayUrl);
            
            if(!string.IsNullOrWhiteSpace(hash))
            {
                url = UrlResolver.GetResolveUrl(Client.GatewayUrl, $"arg={hash}");
            }
            
            Log.WarningMessage(url.ToString());
            
            var response = await this.Client.GetAsync(url);
            
            if(!response.Success)
            {
                result.AddErrors(response.Errors);
                return result;
            }
            
            var json = JObject.Parse(response.Value);
            
            var path = (string)json["Path"];
            
            result.SetValue(path);
            
            return result;
        }
    }
}