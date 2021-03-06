using System;
using System.Collections.Generic;
using System.Linq;
using IPFS.Utils.Logger;
using IPFS.Results;
using System.Threading.Tasks;
using IPFS.Integration.Messages;
using IPFS.Integration.Abstractions;
using IPFS.Services.DTO;
using IPFS.Utils.DI;

namespace IPFS.Services.Handlers
{
    [Injectable]
    public class GetClientInformation
    {
        private readonly IIPFSClient client;
        private readonly ILogger<GetClientInformation> logger;
        
        public GetClientInformation(IIPFSClient client, ILogger<GetClientInformation> logger)
        {
            this.client = client;
            this.logger = logger;
        }
        
        public async Task<Result<ClientInfoDTO>> Get()
        {
            var result = new Result<ClientInfoDTO>();
            
            var peerInfoTask = client.Message<GetPeerInformationMessage>().SendAsync();
            var peerListTask = client.Message<GetPeersMessage>().SendAsync();
            var localStorageTask = client.Message<GetLocalObjectListMessage>().SendAsync();
            
            await Task.WhenAll(peerInfoTask, peerListTask, localStorageTask);
            
            var peerInfoResult = await peerInfoTask;
            var peerListResult = await peerListTask;
            var localStorageResult = await localStorageTask;
            
            if(!peerInfoResult.Success)
            {
                result.AddErrors(peerInfoResult.Errors);
            }
            
            if(!peerListResult.Success)
            {
                result.AddErrors(peerListResult.Errors);
            }
            
            if(!localStorageResult.Success)
            {
                result.AddErrors(localStorageResult.Errors);
            }
            
            if(result.Errors.Any())
                return result;
            
            result.SetValue(new ClientInfoDTO{
                PeerId = peerInfoResult.Value.ID, 
                PeerCount = peerListResult.Value.Count(),
                FilesStrored = localStorageResult.Value.Count(),
                TotalFilesSize = localStorageResult.Value.Sum(x=>x.Size)
            });
            
            return result;
        }
    }
}