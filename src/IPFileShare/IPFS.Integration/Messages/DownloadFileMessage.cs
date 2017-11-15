using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IPFS.Results;
using System.Threading.Tasks;
using IPFS.Integration.Models;
using Newtonsoft.Json;
using System.IO;
using IPFS.Integration.Utils.Log;

namespace IPFS.Integration.Messages
{
    public class DownloadFileMessage : IApiMessage
    {
        public RESTClient Client { get; set;}
        
        private ILogger Log => Logger.Log<DownloadFileMessage>();
        
        public async Task<Result<Stream>> SendAsync(string path)
        {
            var result = new Result<Stream>();
            
            var url = UrlResolver.GetDownloadUrl(this.Client.GatewayUrl, "arg={path}");
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