using IPFS.Integration.Utils.Log;
using IPFS.Results;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

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