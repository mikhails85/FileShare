using System;
using System.Collections.Generic;
using System.Linq;
using IPFS.Results;

namespace IPFS.Runner
{
    //http://www.levibotelho.com/development/async-processes-with-taskcompletionsource/
    //https://medium.com/@dpursanov/running-python-script-from-c-and-working-with-the-results-843e68d230e5
    //https://code.msdn.microsoft.com/windowsapps/Come-evitare-di-eseguire-3dfb6872
    //https://ehikioya.com/multiple-instances-of-same-application/
    //
    public static class ProcessManager
    {
        public static Result<ProcessContext> CreateProcess(string appId)
        {
           var result = new Result<ProcessContext>();
           var process = new ProcessContext(appId);
           var canRunResult = process.CanRun();
           if(!canRunResult.Success)
           {
               result.AddErrors(canRunResult.Errors);
               return result;
           }
           result.SetValue(process);
           return result;
        }
    }
}