using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IPFS.Integration.Abstractions;
using IPFS.Integration.Messages;
using Microsoft.Extensions.DependencyInjection;

namespace IPFS.Integration.Tests.Helpers
{
    public class IPFSMessageProvider:IMessageProvider
    {
        private readonly IServiceProvider serviceProvider;
        
        public IPFSMessageProvider(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;    
        }
        
        public T Message<T>() where T : IApiMessage
        {
            return this.serviceProvider.GetService<T>();
        }
    }
}