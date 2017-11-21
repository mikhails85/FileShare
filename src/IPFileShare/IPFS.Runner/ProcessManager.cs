using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using IPFS.Results;
using IPFS.Runner.Errors;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
namespace IPFS.Runner
{
    //https://blogs.msdn.microsoft.com/fkaduk/2017/02/22/using-strongly-typed-configuration-in-net-core-console-app/
    //http://www.levibotelho.com/development/async-processes-with-taskcompletionsource/
    public static class ProcessManager
    {
        private static Process process;
        private static Mutex mutex;
        
        public static Action<VoidResult> OnExit{ get; set; }
        
        public static async Task<VoidResult> StartProcess(string appId, string path, params string[] options)
        {   
           var result = new VoidResult();    
           mutex = new Mutex(false, appId);
           if (!mutex.WaitOne(TimeSpan.FromSeconds(0), false))
           {
               result.AddErrors(new ProcessAlreadyRunError());
               return result;
           }
           
           return await Starting(path, options);
        }
        
        public static void StopProcess()
        {
            if(process == null)
            {
                return;
            }
            
            process.Kill ();
		    process.Dispose ();
			process = null;
			
            if(mutex == null)
            {
                return;
            }
            mutex.Dispose();
            mutex = null;
        }
        
        private static Task<VoidResult> Starting(string path, params string[] options)
        {
            var tcs = new TaskCompletionSource<VoidResult>();
            
            process = new Process
            {
                EnableRaisingEvents = true,
                StartInfo = CreateProcessStartInfo(path, options)
            };
            
            var c = Console.Out;
            
            process.Exited += (sender, args) =>
            {
                ExitHandler();
                
                var result = new VoidResult();   
                
                var errorMessage = process.ExitCode != 0 ? process.StandardError.ReadToEnd():"Exit process";
                result.AddErrors(new ProcessExitedWithError(errorMessage));
                
                tcs.SetResult(result);
            };
            
            process.OutputDataReceived += (sender, e) =>
			{
			    Console.SetOut (c);
				c.WriteLine(e.Data);
				
				if(e.Data.Contains("Daemon is ready"))
				{
				    var result = new VoidResult();    
				    tcs.SetResult(result);
				}
			};
			
            process.Start();
            
            process.BeginOutputReadLine();
			process.BeginErrorReadLine();
			
			return tcs.Task;
        }
        
        private static void ExitHandler()
        {
            var exitResult = new VoidResult();
                
            if (process.ExitCode != 0)
            {
                var errorMessage = process.StandardError.ReadToEnd();
                    
                exitResult.AddErrors(new ProcessExitedWithError("The process did not exit correctly. " +
                    "The corresponding error message was: " + errorMessage));    
            }
                
            if(OnExit != null)
            {
                OnExit(exitResult);
            }
           
            process.Dispose();
            process = null;
        }
        
        private static ProcessStartInfo CreateProcessStartInfo (string path, params string[] options)
        {
            var start = new ProcessStartInfo();
            start.FileName = path;
            start.Arguments = string.Join(" ", options);
            start.UseShellExecute = false;
            start.CreateNoWindow = true;
            start.RedirectStandardOutput = true;
            start.RedirectStandardError = true;
            
            return start;
        }
        
        
    }
}