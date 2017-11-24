using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IPFS.Utils.Logger;
using IPFS.Results;
using Newtonsoft.Json;
namespace IPFS.Utils.Logger.Implementation
{
    public class SeriLogger<TContext>: ILogger<TContext>
    {
        private readonly Serilog.ILogger logger; 
        
        public SeriLogger(Serilog.ILogger logger )
        {
            this.logger = logger;
        }
        
        public string Context => typeof(TContext).FullName;
        
        public void Error(Error error, string message ="")
        {
            var dateTime = DateTime.Now.ToString("dd/MM/yyyy - HH:mm:ss");    
            message = string.IsNullOrWhiteSpace(message) ? message: $" - {message}";
            
            this.logger.Error($"{Context} - {dateTime}{message} - {error.Code}: {error.Message}");
            
        }
        
        public void Result<T> (Result<T> result, string message ="")
        {
            message = string.IsNullOrWhiteSpace(message) ? message: $" - {message}";
            
            if(!result.Success)
            {
                foreach(var error in result.Errors)
                {
                    this.logger.Error($"{Context}{message} - {error.Code}: {error.Message}");
                }
            }
            else
            {
                this.logger.Debug($"{Context}{message} - Result:");
                this.logger.Debug($"--------------------------------------------------------------------");
                this.logger.Debug(JsonConvert.SerializeObject(result.Value)); 
                this.logger.Debug($"--------------------------------------------------------------------");
            }
        }
        
        public void Result (VoidResult result, string message ="")
        {
            message = string.IsNullOrWhiteSpace(message) ? message: $" - {message}";
            
            if(!result.Success)
            {
                foreach(var error in result.Errors)
                {
                    this.logger.Error($"{Context}{message} - {error.Code}: {error.Message}");
                }
            }
            else
            {
                this.logger.Debug($"{Context}{message} - Result:{result.Success}");
            }
        }
        
        public void ErrorMessage(string message)
        {
            this.logger.Error($"{Context} - {message}");
        }
        
        public void WarningMessage (string message)
        {
            this.logger.Warning($"{Context} - {message}");
        }
    }
}