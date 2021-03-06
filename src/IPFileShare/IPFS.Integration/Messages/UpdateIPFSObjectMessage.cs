using IPFS.Integration.Abstractions;
using IPFS.Integration.Models;
using IPFS.Integration.Utils.Protobuf;
using IPFS.Results;
using IPFS.Utils.Logger;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Threading.Tasks;

namespace IPFS.Integration.Messages
{
    public class UpdateIPFSObjectMessage: IApiMessage
    {
        public IIPFSClient Client { get; set;}
        
        private readonly ILogger<UpdateIPFSObjectMessage> Log;
        
        public UpdateIPFSObjectMessage(ILogger<UpdateIPFSObjectMessage> logger)
        {
            this.Log = logger;
        }
        
        public async Task<Result<IPFSHash>> SendAsync(IPFSObject obj)
        {
            var result = new Result<IPFSHash>(); 
            
            var body =  new MemoryStream(obj.Serialize(),false);
            var url = UrlResolver.GetUpdateObjectUrl(this.Client.GatewayUrl, "inputenc=protobuf");
            
            Log.WarningMessage(url.ToString());
            
            var response = await this.Client.UploadAsync(url, body);
            if(!response.Success)
            {
                result.AddErrors(response.Errors);
                return result;
            }
            
            Log.WarningMessage(response.Value);
            
            var json = JObject.Parse(response.Value);
            
            var hash = new IPFSHash{Hash=(string) json["Hash"]};
            
            result.SetValue(hash);
            
            return result;
        }
    }
}