using System;
using IPFS.Integration;
using IPFS.Integration.Messages;
using IPFS.Integration.Models;
using IPFS.Integration.Utils.Log;
using System.Threading.Tasks;
namespace IPFS.Client
{
    class Program
    {
        [STAThread]
        static async Task Main(string[] args)
        {
            Logger.SetupLogger(new ClientLogger());
            
            var rest = new RESTClient("http://0.0.0.0:8081/");
            
            var result = await rest.Message<DownloadFileMessage>().SendAsync("QmSwaM2teG1vXyhcmKGjAwkWw6vcYF948Yu2TSzFGEAjUZ");
            
            Console.WriteLine(result.Success ? "Success" : result.Errors[0].Message);
            
            Console.WriteLine("Done");
        }
    }
}
