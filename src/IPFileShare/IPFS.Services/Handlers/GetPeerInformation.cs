using System;
using System.Collections.Generic;
using System.Linq;
using IPFS.Utils.Logger;
using IPFS.Services.Context;
using IPFS.Results;
using System.Threading.Tasks;
using IPFS.Integration.Message;

namespace IPFS.Services.Handlers
{
    public class GetPeerInformation
    {
        private readonly HandlerContext context;
        private readonly ILogger<GetPeerInformation> logger;
        
        public GetPeerInformation(HandlerContext context, ILogger<GetPeerInformation> logger)
        {
            this.context = context;
            this.logger = logger;
        }
        
        public async Task<Result<ClientInfoDTO>> GetClientInfo()
        {
            var result = new Result<ClientInfoDTO>();
            
            var messageResult = await context.Client.Message<GetPeerInformation>().SendAsync();
            
            logger.Result(messageResult, "Get peer info response");
            
            if(!messageResult.Success)
            {
                
                result.AddErrors(messageResult.Errors);
                return result;
            }
            
            
        }
    }
}