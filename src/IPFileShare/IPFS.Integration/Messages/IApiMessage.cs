using IPFS.Integration.Abstractions;

namespace IPFS.Integration.Messages
{
    public interface IApiMessage
    {
        IIPFSClient Client {get; set;}
    }
}