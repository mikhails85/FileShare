using System;
using System.Collections.Generic;
using System.Linq;

namespace IPFS.Integration.Errors
{
    public enum ErrorCodes:int
    {
        HttpRequest = 1001,
        
        InvalidIPFSCommand = 2001
    }
}