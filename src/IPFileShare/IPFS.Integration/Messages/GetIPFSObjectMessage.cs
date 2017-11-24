using IPFS.Integration.Abstractions;
using IPFS.Integration.Models;
using IPFS.Results;
using IPFS.Utils.Logger;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPFS.Integration.Messages
{
    public class GetIPFSObjectMessage: IApiMessage
    {
        public IIPFSClient Client { get; set;}
        
        private readonly ILogger<GetIPFSObjectMessage> Log;
        
        public GetIPFSObjectMessage(ILogger<GetIPFSObjectMessage> logger)
        {
            this.Log = logger;
        }
        
        public async Task<Result<IPFSObject>> SendAsync(string hash)
        {
            var result = new Result<IPFSObject>();
            
            var url = UrlResolver.GetObjectUrl(Client.GatewayUrl, $"arg={hash}");
            
            Log.WarningMessage(url.ToString());
            
            var response = await this.Client.GetAsync(url);
            
            if(!response.Success)
            {
                result.AddErrors(response.Errors);
                return result;
            }
            
            var ipfsObject = new IPFSObject();
            
            var json = JObject.Parse(response.Value);
            
            var stringData = (string)json["Data"];
            if (stringData != null)
                ipfsObject.Data = Encoding.UTF8.GetBytes(stringData);
            ipfsObject.Links = ((JArray)json["Links"])
                .Select(link => new IPFSObjectLink{
                    Name = (string)link["Name"],
                    Hash = (string)link["Hash"],
                    Size = (long)link["Size"]}).ToList();
            
            result.SetValue(ipfsObject);
            
            return result;
        }
    }
}