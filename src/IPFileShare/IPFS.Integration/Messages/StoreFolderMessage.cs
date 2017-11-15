using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IPFS.Results;
using System.Threading.Tasks;
using IPFS.Integration.Models;
using Newtonsoft.Json.Linq;
using System.IO;
using IPFS.Integration.Utils.Log;

namespace IPFS.Integration.Messages
{
    public class StoreFolderMessage: IApiMessage
    {
        public RESTClient Client { get; set;}
        
        private ILogger Log => Logger.Log<StoreFolderMessage>();
        
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
            
            var hashList = ((JArray)json["Pinned"])
                .Select(link => new IPFSHash{
                    Name = (string)link,
                    Hash = (string)link}).ToList();
            
            result.SetValue(hashList);
            
            return result;
        }
    }
}