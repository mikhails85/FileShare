using IPFS.Results;

namespace IPFS.Runner.Errors
{
    public class ProcessExitedWithError : Error
    {
        public ProcessExitedWithError(string message)
            :base((int)ErrorCodes.ProcessExitedWithError, message)
        { 
        }
    }
}