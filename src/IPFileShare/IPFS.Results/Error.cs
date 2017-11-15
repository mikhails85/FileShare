using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IPFS.Results
{
    public class Error
    {
        public int Code {get;}
        public string Message {get;}
        
        public Error(int code, string message)
        {
            this.Code = code;
            this.Message = message;
        }
    }
}