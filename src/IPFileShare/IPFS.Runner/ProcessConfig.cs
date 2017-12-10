using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;

namespace IPFS.Runner
{
    public class ProcessConfig
    {
        public string API {get; set;}
        
        public string ExecutorPath {get; set;}
        
        public string ReadyAfter {get; set;}
        
        public string ConfigPath {get; set;}
        
        public string RunServiceCommand {get; set;}
        
        public Dictionary<string,string> EnvironmentVariables { get; set; }

        public override string ToString() 
        {
             var path = string.IsNullOrWhiteSpace(ConfigPath) ? string.Empty : Path.GetFullPath(ConfigPath);
             var command = string.Format(RunServiceCommand, path);
             
             return command;
        }
    }
}