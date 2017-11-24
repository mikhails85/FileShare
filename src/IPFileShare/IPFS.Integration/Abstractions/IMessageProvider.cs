using System;
using System.Collections.Generic;
using System.Linq;
using IPFS.Integration.Messages;

namespace IPFS.Integration.Abstractions
{
    public interface IMessageProvider
    {
        T Message<T>() where T : IApiMessage;
    }
}