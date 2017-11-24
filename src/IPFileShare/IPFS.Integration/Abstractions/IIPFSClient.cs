using IPFS.Integration.Errors;
using IPFS.Integration.Messages;
using IPFS.Results;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using IPFS.Integration.Abstractions;

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