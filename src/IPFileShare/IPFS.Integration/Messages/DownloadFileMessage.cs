using IPFS.Integration.Models;
using IPFS.Integration.Abstractions;
using IPFS.Utils.Logger;
using IPFS.Results;
using System.IO;
using System.Threading.Tasks;

namespace IPFS.Integration.Messages
{
    public class DownloadFileMessage : IApiMessage
    {
        public IIPFSClient Client { get; set;}
        
        private readonly ILogger<DownloadFileMessage> Log;
        
        public DownloadFileMessage(ILogger<DownloadFileMessage> logger)
        {
            this.Log = logger;
        }
        
        public async Task<Result<Stream>> SendAsync(string path)
        {
            var result = new Result<Stream>();
            
            var url = UrlResolver.GetDownloadUrl(this.Client.GatewayUrl, $"arg={path}");
            Log.WarningMessage(url.ToString());
            
            var response = new Result<Stream>();
            
            response = await this.Client.DownloadAsync(url);
            
            if(!response.Success)
            {
                result.AddErrors(response.Errors);
                return result;
            }
            
            result.SetValue(response.Value);
            
            return result;
        }
    }
}