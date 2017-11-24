using IPFS.Integration.Models;
using IPFS.Integration.Abstractions;
using IPFS.Utils.Logger;
using IPFS.Results;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace IPFS.Integration.Messages
{
    public class GetPeerInformation: IApiMessage
    {
        public IIPFSClient Client { get; set;}
        
        private readonly ILogger<GetPeerInformation> Log;
        
        public GetPeerInformation(ILogger<GetPeerInformation> logger)
        {
            this.Log = logger;
        }
        
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