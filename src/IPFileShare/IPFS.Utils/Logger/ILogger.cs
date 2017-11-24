using IPFS.Results;
namespace IPFS.Utils.Logger
{
    public interface ILogger<TContext>
    {
        string Context { get;}
        void Error(Error error, string message);
        void Result<T> (Result<T> result, string message);
        void Result (VoidResult result, string message);
        void ErrorMessage(string message);
        void WarningMessage (string message);
    }
}