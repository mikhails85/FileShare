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
    public class GetLocalObjectListMessage: IApiMessage
    {
        public IIPFSClient Client { get; set;}
        
        private readonly ILogger<GetLocalObjectListMessage> Log;
        
        public GetLocalObjectListMessage(ILogger<GetLocalObjectListMessage> logger)
        {
            this.Log = logger;
        }
        
        public async Task<Result<List<IPFSObjectInfo>>> SendAsync()
        {
            var result = new Result<List<IPFSObjectInfo>>();
            
            var url = UrlResolver.GetLocalObjectListUrl(Client.GatewayUrl, $"type=recursive");
            Log.WarningMessage(url.ToString());
            
            var response = await this.Client.PostAsync(url);
            
            if(!response.Success)
            {
                result.AddErrors(response.Errors);
                return result;
            }
            
            var json = JObject.Parse(response.Value);
            var hashs = json.Value<JObject>("Keys").Properties();
            var hashTasks = hashs.Select(x=> this.Client.Message<GetFileInfoMessage>().SendAsync(x.Name,"")).ToList();
            
            var hashTaskResults = await Task.WhenAll(hashTasks);
            
            var fileInfos = hashTaskResults.Where(r=>r.Success).Select(r=>r.Value).ToList();
            
            result.SetValue(fileInfos);
            
            return result;
        }
    }
}