using IPFS.Integration.Abstractions;
using IPFS.Integration.Messages;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace IPFS.Desktop.Api.DependencyInjection
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