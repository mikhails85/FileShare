using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IPFS.Results;
namespace IPFS.Integration.Errors
{
    public class HttpRequestError : Error
    {
        public HttpRequestError(string message)
            :base((int)ErrorCodes.HttpRequest, message)
        { 
        }
    }
}