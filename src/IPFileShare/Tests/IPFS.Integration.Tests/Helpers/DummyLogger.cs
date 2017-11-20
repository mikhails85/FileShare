using IPFS.Integration.Utils.Log;
using IPFS.Results;
namespace IPFS.Integration.Tests.Helpers
{
    public class DummyLogger: ILogger
    {
        public string Context { get; set;}
        
        public void Error(Error error, string message ="")
        {
            
        }
        
        public void Result<T> (Result<T> result, string message ="")
        {
            
        }
        
        public void Result (VoidResult result, string message ="")
        {
            
        }
        
        public void ErrorMessage(string message)
        {
            
        }
        
        public void WarningMessage (string message)
        {
            
        }
    }
}