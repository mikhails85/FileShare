using IPFS.Integration.Utils.Log;
using IPFS.Results;
using System.IO;
using System.Threading.Tasks;

namespace IPFS.Integration.Messages
{
    public class ReadTextMessage : IApiMessage
    {
        public RESTClient Client { get; set;}
        
        private ILogger Log => Logger.Log<ReadTextMessage>();
        
        public async Task<Result<string>> SendAsync(string path)
        {
            var result = new Result<string>();
            
            var response = await Client.Message<DownloadFileMessage>().SendAsync(path);
            
            if(!response.Success)
            {
                result.AddErrors(response.Errors);
                return result;
            }
            
            var content = string.Empty;
            
            using (var data = response.Value)
            using (var text = new StreamReader(data))
            {
                content = await text.ReadToEndAsync();
            }
            
            result.SetValue(content);
            
            return result;
        }
    }
}