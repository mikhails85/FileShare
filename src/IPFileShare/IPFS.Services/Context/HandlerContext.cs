using System;
using System.Collections.Generic;
using System.Linq;
using IPFS.Utils.DI;
using IPFS.Integration.Abstractions;

namespace IPFS.Services.Context
{
    [Injectable]
    public class HandlerContext
    {
        public IIPFSClient Client {get;}
        
        public HandlerContext(IIPFSClient client)
        {
            Client = client;
        }
    }
}