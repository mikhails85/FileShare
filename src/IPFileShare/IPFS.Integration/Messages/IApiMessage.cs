using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IPFS.Integration.Messages
{
    public interface IApiMessage
    {
        RESTClient Client {get; set;}
    }
}