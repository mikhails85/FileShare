using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IPFS.Results;
namespace IPFS.Integration.Errors
{
    public class InvalidIPFSCommandError : Error
    {
        public InvalidIPFSCommandError(string message)
            :base((int)ErrorCodes.InvalidIPFSCommand, message)
        { 
        }
    }
}