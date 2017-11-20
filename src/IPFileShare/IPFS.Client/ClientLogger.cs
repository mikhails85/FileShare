using IPFS.Integration.Utils.Log;
using IPFS.Results;
using Newtonsoft.Json;
using System;

namespace IPFS.Client
{
    public class ClientLogger: ILogger
    {
        public string Context { get; set;}
        
        public void Error(Error error, string message ="")
        {
            var dateTime = DateTime.Now.ToString("dd/MM/yyyy - HH:mm:ss");    
            message = string.IsNullOrWhiteSpace(message) ? message: $" - {message}";
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{Context} - {dateTime}{message} - {error.Code}: {error.Message}"); 
            Console.ResetColor();
        }
        
        public void Result<T> (Result<T> result, string message ="")
        {
            var dateTime = DateTime.Now.ToString("dd/MM/yyyy - HH:mm:ss");    
            message = string.IsNullOrWhiteSpace(message) ? message: $" - {message}";
            
            if(!result.Success)
            {
                foreach(var error in result.Errors)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{Context} - {dateTime}{message} - {error.Code}: {error.Message}"); 
                    Console.ResetColor();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"{Context} - {dateTime}{message} - Result:"); 
                Console.WriteLine($"--------------------------------------------------------------------");
                Console.WriteLine(JsonConvert.SerializeObject(result.Value)); 
                Console.WriteLine($"--------------------------------------------------------------------"); 
                Console.ResetColor();
            }
        }
        
        public void Result (VoidResult result, string message ="")
        {
            var dateTime = DateTime.Now.ToString("dd/MM/yyyy - HH:mm:ss");    
            message = string.IsNullOrWhiteSpace(message) ? message: $" - {message}";
            
            if(!result.Success)
            {
                foreach(var error in result.Errors)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{Context} - {dateTime}{message} - {error.Code}: {error.Message}"); 
                    Console.ResetColor();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"{Context} - {dateTime}{message} - Result:{result.Success}"); 
                Console.ResetColor();
            }
        }
        
        public void ErrorMessage(string message)
        {
            var dateTime = DateTime.Now.ToString("dd/MM/yyyy - HH:mm:ss");    
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{Context} - {dateTime} - {message}"); 
            Console.ResetColor();
        }
        
        public void WarningMessage (string message)
        {
            var dateTime = DateTime.Now.ToString("dd/MM/yyyy - HH:mm:ss");    
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{Context} - {dateTime} - {message}"); 
            Console.ResetColor();
        }
    }
}