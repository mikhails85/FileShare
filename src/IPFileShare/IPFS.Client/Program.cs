using IPFS.Integration;
using IPFS.Integration.Messages;
using IPFS.Integration.Utils.Log;
using System;
using System.Threading.Tasks;
using IPFS.Runner;
using IPFS.Results;

namespace IPFS.Client
{
    class Program
    {
        [STAThread]
        static async Task Main(string[] args)
        {
            Logger.SetupLogger(new ClientLogger());
            
            var processResult = ProcessManager.CreateProcess("MyClient");     
            
            if(!processResult.Success)
            {
                Console.WriteLine(processResult.Success ? "Success" : processResult.Errors[0].Message);
                return;
            }
            using(var process = processResult.Value)
            {
                process.OnExit = (exitResult) => { 
                    Console.WriteLine(exitResult.Success ? "Success" : exitResult.Errors[0].Message);
                };
                process.Run( "./IPFS/ipfs.exe", "daemon");   
               
                var rest = new RESTClient("http://0.0.0.0:8081/");
            
                var result = await rest.Message<DownloadFileMessage>().SendAsync("QmSwaM2teG1vXyhcmKGjAwkWw6vcYF948Yu2TSzFGEAjUZ");
            
                Console.WriteLine(result.Success ? "Success" : result.Errors[0].Message);
            
                process.Stop();
            }
            Console.WriteLine("Done");
        }
    }
}
