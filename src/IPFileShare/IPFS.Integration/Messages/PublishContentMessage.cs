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
    public class PublishContentMessage: IApiMessage
    {
        public RESTClient Client { get; set;}
        
        private ILogger Log => Logger.Log<PublishContentMessage>();
        
        public async Task<Result<IPFSHash>> SendAsync(string hash)
        {
            var result = new Result<IPFSHash>();
            
            var url = UrlResolver.GetPublishUrl(Client.GatewayUrl, $"arg={hash}");
            
            Log.WarningMessage(url.ToString());
            
            var response = await this.Client.PostAsync(url);
            
            if(!response.Success)
            {
                result.AddErrors(response.Errors);
                return result;
            }
            
            var json = JObject.Parse(response.Value);
            
            var name = (string)json["Name"];
            var value = (string)json["Value"];
            
            result.SetValue(new IPFSHash {Hash = name, Name = value});
            
            return result;
        }
    }
}