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
    public class GetPeersMessage: IApiMessage
    {
        public RESTClient Client { get; set;}
        
        private ILogger Log => Logger.Log<GetPeersMessage>();
        
        public async Task<Result<List<LatencyPeer>>> SendAsync()
        {
            var result = new Result<List<LatencyPeer>>();
            
            var url = UrlResolver.GetPeersUrl(Client.GatewayUrl, "verbose");
            Log.WarningMessage(url.ToString());
            
            var response = await this.Client.GetAsync(url);
            
            if(!response.Success)
            {
                result.AddErrors(response.Errors);
                return result;
            }
            
            var json = JObject.Parse(response.Value);
            
            var peersArr = json["Peers"] as JArray;
            
            var peers = peersArr
                    .Select(l => new LatencyPeer()
                    {
                        Latency = (string)l["Latency"],
                        PeerId = (string)l["Peer"]
                    })
                    .ToList();
            
            result.SetValue(peers);
            
            return result;
        }
    }
}