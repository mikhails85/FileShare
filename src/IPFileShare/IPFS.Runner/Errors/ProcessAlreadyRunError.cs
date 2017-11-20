using IPFS.Results;

namespace IPFS.Runner.Errors
{
    public class ProcessAlreadyRunError : Error
    {
        public ProcessAlreadyRunError()
            :base((int)ErrorCodes.ProcessAlreadyRun, "Process already run")
        { 
        }
    }
}