using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IPFS.Runner.Errors
{
    public enum ErrorCodes : int
    {
        ProcessAlreadyRun = 4000,
        ProcessExitedWithError = 4001
    }
}