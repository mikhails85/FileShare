using IPFS.Results;

namespace IPFS.Integration.Utils.Log
{
    public interface ILogger
    {
        string Context { get; set;}
        void Error(Error error, string message);
        void Result<T> (Result<T> result, string message);
        void Result (VoidResult result, string message);
        void ErrorMessage(string message);
        void WarningMessage (string message);
    }
}