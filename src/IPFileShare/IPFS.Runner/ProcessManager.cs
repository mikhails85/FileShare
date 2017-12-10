using IPFS.Results;
using IPFS.Runner.Errors;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace IPFS.Runner
{
    public static class ProcessManager
    {
        private static Process process;
        private static ProcessConfig processConfig;

        public static Action<VoidResult> OnExit{ get; set; }
        
        public static async Task<VoidResult> StartProcess(string appId, ProcessConfig config)
        {
            processConfig = config;
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
           
            if(!process.HasExited)
            {
                process.Kill();
                process.Dispose();
                process = null;
            }

            var lockFile =Path.Combine(processConfig.ConfigPath, "repo.lock");
            if(File.Exists(lockFile))
            {
                File.Delete(lockFile);
            }
        }
        
        private static bool IsAlreadyRunning(string appId)
        {
            var mutex = new Mutex(true, "Global\\" + appId, out bool createdNew);
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
            
            process.Exited += async (sender, args) =>
            {
                ExitHandler();
                
                var result = new VoidResult();   
                
                var errorMessage = process.ExitCode != 0 ? await process.StandardError.ReadToEndAsync():"Exit process";
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
                //var errorMessage = process.StandardError.ReadToEnd();

                exitResult.AddErrors(new ProcessExitedWithError("The process did not exit correctly. "));// +
                 //   "The corresponding error message was: " + errorMessage));    
            }

            OnExit?.Invoke(exitResult);

            StopProcess();
        }
        
        private static ProcessStartInfo CreateProcessStartInfo (ProcessConfig config)
        {
            var start = new ProcessStartInfo
            {
                FileName = config.ExecutorPath,
                Arguments = config.ToString(),
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            if(config.EnvironmentVariables != null)
            {
                foreach(var key in config.EnvironmentVariables.Keys)
                {
                    start.EnvironmentVariables.Add(key, config.EnvironmentVariables[key]);
                }
            }

            return start;
        }
        
        
    }
}