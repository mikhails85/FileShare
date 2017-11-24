using IPFS.Integration.Messages;
using IPFS.Results;
using System;
using System.IO;
using System.Threading.Tasks;

namespace IPFS.Integration.Abstractions
{
    public interface IIPFSClient
    {
        Uri GatewayUrl {get;}
        
        T Message<T>() where T : IApiMessage;
        
        Task<Result<string>> UploadAsync(Uri url, Stream body, string fileparam="file", string filename="");
        
        Task<Result<Stream>> DownloadAsync(Uri url);
        
        Task<Result<string>> PostAsync(Uri url);
        
        Task<Result<string>> GetAsync(Uri url);
    }
}