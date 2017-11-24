using IPFS.Integration.Abstractions;
using IPFS.Integration.Models;
using IPFS.Results;
using IPFS.Utils.Logger;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPFS.Integration.Messages
{
    public class StoreFolderMessage: IApiMessage
    {
        public IIPFSClient Client { get; set;}
        
        private readonly ILogger<StoreFolderMessage> Log;
        
        public StoreFolderMessage(ILogger<StoreFolderMessage> logger)
        {
            this.Log = logger;
        }
        
        public async Task<Result<List<IPFSHash>>> SendAsync(string hash)
        {
            var result = new Result<List<IPFSHash>>();
            
            var url = UrlResolver.GetStoreFolderUrl(Client.GatewayUrl, $"arg={hash}");
            
            Log.WarningMessage(url.ToString());
            
            var response = await this.Client.GetAsync(url);
            
            if(!response.Success)
            {
                result.AddErrors(response.Errors);
                return result;
            }
            
            var json = JObject.Parse(response.Value);
            
            var hashList = ((JArray)json["Pins"])
                .Select(link => new IPFSHash{
                    Name = (string)link,
                    Hash = (string)link}).ToList();
            
            result.SetValue(hashList);
            
            return result;
        }
    }
}