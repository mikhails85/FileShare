using IPFS.Integration.Models;
using IPFS.Integration.Utils.Log;
using IPFS.Results;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace IPFS.Integration.Messages
{
    public class AddIPFSObjectMessage: IApiMessage
    {
        public RESTClient Client { get; set;}
        
        private ILogger Log => Logger.Log<AddIPFSObjectMessage>();
        
        public async Task<Result<IPFSObject>> SendAsync()
        {
            var result = new Result<IPFSObject>();
            
            var url = UrlResolver.GetNewObjectUrl(Client.GatewayUrl, "arg=unixfs-dir");
            Log.WarningMessage(url.ToString());
            
            var response = await this.Client.GetAsync(url);
            
            if(!response.Success)
            {
                result.AddErrors(response.Errors);
                return result;
            }
            
            var hash = (string) (JObject.Parse(response.Value)["Hash"]);
            
            var ipfsObjectResult = await this.Client.Message<GetIPFSObjectMessage>().SendAsync(hash);
            
            if(!ipfsObjectResult.Success)
            {
                result.AddErrors(ipfsObjectResult.Errors);
                return result;
            }
            
            result.SetValue(ipfsObjectResult.Value);
            return result;
        }
    }
}