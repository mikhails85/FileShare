using System;
using System.Collections.Generic;
using System.Linq;
using IPFS.Results;
using IPFS.Runner.Errors;
using System.Threading;
using System.Diagnostics;

namespace IPFS.Runner
{
    public class ProcessContext : IDisposable
    {
        private readonly Mutex mutex;
        
        private Process context;
        
        private bool disposed = false;
        
        public Action<VoidResult> OnExit{ get; set; }
        
        public ProcessContext(string appId)
        {
            mutex = new Mutex(false, appId);
        }
        
        public VoidResult CanRun()
        {
            var result = new VoidResult();
            
            if (!mutex.WaitOne(TimeSpan.FromSeconds(0), false))
            {
                result.AddErrors(new ProcessAlreadyRunError());
            }
            
            return result;
        }
        
        public VoidResult Run(string path, params string[] options)
        {
            var result = new VoidResult();
            
            result = CanRun();
            
            if(!result.Success)
            {
                return result;
            }
            
            context = new Process
            {
                EnableRaisingEvents = true,
                StartInfo = CreateProcessStartInfo(path, options)
            };
            
            context.Exited += (sender, args) =>
            {
                ExitHandler();
            };
            
            context.Start();
            
            return result;
        }
        
        public void Stop()
        {
            if(context == null)
            {
                return;
            }
            
            context.StandardInput.Flush();
            context.StandardInput.Close();
            context.WaitForExit();
            context.Close();
        }
        
        private void ExitHandler()
        {
            var exitResult = new VoidResult();
                
            if (context.ExitCode != 0)
            {
                var errorMessage = context.StandardError.ReadToEnd();
                    
                exitResult.AddErrors(new ProcessExitedWithError("The process did not exit correctly. " +
                    "The corresponding error message was: " + errorMessage));    
            }
                
            if(OnExit != null)
            {
                OnExit(exitResult);
            }
           
            context.Dispose();
            context = null;
        }
        
        private ProcessStartInfo CreateProcessStartInfo (string path, params string[] options)
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
        
        public void Dispose()
        { 
           Dispose(true);
           GC.SuppressFinalize(this);           
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return; 
    
            if (disposing) {
                mutex.Dispose();
            }
    
            disposed = true;
        }
    }
}