using IPFS.Integration.Models;
using IPFS.Integration.Abstractions;
using IPFS.Utils.Logger;
using IPFS.Results;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace IPFS.Integration.Messages
{
    public class AddIPFSObjectMessage: IApiMessage
    {
        public IIPFSClient Client { get; set;}
        
        private readonly ILogger<AddIPFSObjectMessage> Log;
        
        public AddIPFSObjectMessage(ILogger<AddIPFSObjectMessage> logger)
        {
            this.Log = logger;
        }
        
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