using IPFS.Integration.Models;
using IPFS.Integration.Abstractions;
using IPFS.Utils.Logger;
using IPFS.Results;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPFS.Integration.Messages
{
    public class GetPeersMessage: IApiMessage
    {
        public IIPFSClient Client { get; set;}
        
        private readonly ILogger<GetPeersMessage> Log;
        
        public GetPeersMessage(ILogger<GetPeersMessage> logger)
        {
            this.Log = logger;
        }
        
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