using IPFS.Integration.Models;
using IPFS.Integration.Abstractions;
using IPFS.Utils.Logger;
using IPFS.Results;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace IPFS.Integration.Messages
{
    public class PublishContentMessage: IApiMessage
    {
        public IIPFSClient Client { get; set;}
        
        private readonly ILogger<PublishContentMessage> Log;
        
        public PublishContentMessage(ILogger<PublishContentMessage> logger)
        {
            this.Log = logger;
        }
        
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