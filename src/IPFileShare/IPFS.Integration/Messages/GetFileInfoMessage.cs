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
    public class GetFileInfoMessage: IApiMessage
    {
        public RESTClient Client { get; set;}
        
        private ILogger Log => Logger.Log<GetFileInfoMessage>();
        
        public async Task<Result<IPFSObjectInfo>> SendAsync(string hash, string name)
        {
            var result = new Result<IPFSObjectInfo>();
            
            var url = UrlResolver.GetFileInfoUrl(Client.GatewayUrl, $"arg={hash}");
            Log.WarningMessage(url.ToString());
            
            var response = await this.Client.GetAsync(url);
            
            if(!response.Success)
            {
                result.AddErrors(response.Errors);
                return result;
            }
            
            var json = JObject.Parse(response.Value);
            var jhash = (string)json["Arguments"][hash];
            var jo = (JObject)json["Objects"][jhash];
            
            var info = new IPFSObjectInfo
            {
              Name = name,
              Hash = (string)jo["Hash"],
              Size = (long)jo["Size"],
              IsDirectory = ((string)jo["Type"]).ToLower().Trim() == "directory"
            };
            
            var links = jo["Links"] as JArray;
            if (links != null)
            {
                info.Links = links
                    .Select(l => new IPFSObjectInfo()
                    {
                        Name = (string)l["Name"],
                        Hash = (string)l["Hash"],
                        Size = (long)l["Size"],
                        IsDirectory = ((string)l["Type"]).ToLower().Trim() == "directory",
                    })
                    .ToList();
            }
            
            result.SetValue(info);
            
            return result;
        }
    }
}