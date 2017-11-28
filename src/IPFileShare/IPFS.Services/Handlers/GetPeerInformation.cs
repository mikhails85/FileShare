using System;
using System.Collections.Generic;
using System.Linq;
using IPFS.Utils.Logger;
using IPFS.Services.Context;
using IPFS.Results;
using System.Threading.Tasks;
using IPFS.Integration.Messages;
using IPFS.Integration.Abstractions;
using IPFS.Services.DTO;

namespace IPFS.Services.Handlers
{
    public class GetClientInformation
    {
        private readonly IIPFSClient client;
        private readonly ILogger<GetPeerInformation> logger;
        
        public GetClientInformation(IIPFSClient client, ILogger<GetPeerInformation> logger)
        {
            this.client = client;
            this.logger = logger;
        }
        
        public async Task<Result<ClientInfoDTO>> Get()
        {
            var result = new Result<ClientInfoDTO>();
            
            var messageResult = await client.Message<GetPeerInformation>().SendAsync();
            
            logger.Result(messageResult, "Get peer info response");
            
            if(!messageResult.Success)
            {
                
                result.AddErrors(messageResult.Errors);
                return result;
            }
            
            return result;
        }
    }
}