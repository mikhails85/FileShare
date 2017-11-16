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
    public class GetPeerInformation: IApiMessage
    {
        public RESTClient Client { get; set;}
        
        private ILogger Log => Logger.Log<GetPeerInformation>();
        
        public async Task<Result<PeerInfo>> SendAsync(string hash = "")
        {
            var result = new Result<PeerInfo>();
            
            var url = UrlResolver.GetPeerInfoUrl(Client.GatewayUrl);
            if(!string.IsNullOrWhiteSpace(hash))
            {
                url = UrlResolver.GetPeerInfoUrl(Client.GatewayUrl, $"arg={hash}");
            }
            Log.WarningMessage(url.ToString());
            
            var response = await this.Client.PostAsync(url);
            
            if(!response.Success)
            {
                result.AddErrors(response.Errors);
                return result;
            }
            
            var info = JsonConvert.DeserializeObject<PeerInfo>(response.Value);
            
            result.SetValue(info);
            
            return result;
        }
    }
}