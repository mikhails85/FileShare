using IPFS.Integration.Models;
using IPFS.Integration.Utils.Log;
using IPFS.Results;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace IPFS.Integration.Messages
{
    public class AddFileMessage : IApiMessage
    {
        public RESTClient Client { get; set;}
        
        private ILogger Log => Logger.Log<AddFileMessage>();
        
        public async Task<Result<IPFSHash>> SendAsync(string path)
        {
            var result = new Result<IPFSHash>(); 
            
            var url = UrlResolver.GetAddUrl(this.Client.GatewayUrl);
            Log.WarningMessage(url.ToString());
            
            var response = new Result<string>();
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                response = await this.Client.UploadAsync(url, stream, Path.GetFileName(path));
            }
            
            if(!response.Success)
            {
                result.AddErrors(response.Errors);
                return result;
            }
            
            var hash = JsonConvert.DeserializeObject<IPFSHash>(response.Value);
            hash.Name = Path.GetFileName(path);
            
            result.SetValue(hash);
            
            return result;
        }
    }
}