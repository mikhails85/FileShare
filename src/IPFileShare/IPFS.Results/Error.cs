namespace IPFS.Results
{
    public class Error
    {
        public int Code {get;}
        public string Message {get;}
        
        public Error(int code, string message)
        {
            Code = code;
            Message = message;
        }
    }
}