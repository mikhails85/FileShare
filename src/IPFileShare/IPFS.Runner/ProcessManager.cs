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
    public static class ProcessManager
    {
        private static Process process;
        
        public static Action<VoidResult> OnExit{ get; set; }
        
        public static async Task<VoidResult> StartProcess(string appId, ProcessConfig config)
        {   
           var result = new VoidResult();    
          
           if (IsAlreadyRunning(appId))
           {
               result.AddErrors(new ProcessAlreadyRunError());
               return result;
           }
           
           return await Starting(config);
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
        }
        
        private static bool IsAlreadyRunning(string appId)
        {
            bool createdNew;

            var mutex = new Mutex(true, "Global\\"+appId, out createdNew);
            if (createdNew)
                mutex.ReleaseMutex();

            return !createdNew;
        }
        
        private static Task<VoidResult> Starting(ProcessConfig config)
        {
            var tcs = new TaskCompletionSource<VoidResult>();
            
            process = new Process
            {
                EnableRaisingEvents = true,
                StartInfo = CreateProcessStartInfo(config)
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
				
				if(!string.IsNullOrWhiteSpace(config.ReadyAfter) && !string.IsNullOrWhiteSpace(e.Data) && e.Data.Contains(config.ReadyAfter))
				{
				    var result = new VoidResult();    
				    tcs.SetResult(result);
				}
			};
			
            process.Start();
            
            process.BeginOutputReadLine();
			process.BeginErrorReadLine();
			
			if(string.IsNullOrWhiteSpace(config.ReadyAfter))
			{
			    var result = new VoidResult();    
			    tcs.SetResult(result);
			}
			
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
            
            StopProcess();
        }
        
        private static ProcessStartInfo CreateProcessStartInfo (ProcessConfig config)
        {
            var start = new ProcessStartInfo();
            start.FileName = config.ExecutorPath;
            start.Arguments = config.ToString();
            start.UseShellExecute = false;
            start.CreateNoWindow = true;
            start.RedirectStandardOutput = true;
            start.RedirectStandardError = true;
            
            return start;
        }
        
        
    }
}