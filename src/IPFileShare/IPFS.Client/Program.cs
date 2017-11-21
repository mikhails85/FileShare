using IPFS.Integration;
using IPFS.Integration.Messages;
using IPFS.Integration.Utils.Log;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
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
            
            var path = Path.GetFullPath("./.ipfs");
                
            Console.WriteLine(path);
            
            var processResult = await ProcessManager.StartProcess("MyClient", 
                                    "/bin/bash","-c", $"\"IPFS_PATH={path} ipfs daemon\"" );     
            
            if(!processResult.Success)
            {
                Console.WriteLine(processResult.Success ? "Success" : processResult.Errors[0].Message);
                return;
            }
            
            ProcessManager.OnExit = (exitResult) => { 
                Console.WriteLine(exitResult.Success ? "Success" : exitResult.Errors[0].Message);
            };
            
            var rest = new RESTClient("http://0.0.0.0:6001/");
        
            var result = await rest.Message<GetPeerInformation>().SendAsync();
        
            Console.WriteLine(result.Success ? "Success" : result.Errors[0].Message);
            
            ProcessManager.StopProcess();
            
            Console.WriteLine("Done");
        }
    }
}
