using IPFS.Integration.Models;
using IPFS.Integration.Utils.Log;
using IPFS.Results;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IPFS.Integration.Messages
{
    public class AddFolderMessage : IApiMessage
    {
        public RESTClient Client { get; set;}
        
        private ILogger Log => Logger.Log<AddFolderMessage>();
        
        public async Task<Result<IPFSHash>> SendAsync(string path, bool recursive = true)
        {
            var result = new Result<IPFSHash>(); 
            
            path = Path.GetFullPath(path);
            
            var fileTasks = Directory
                .EnumerateFiles(path)
                .Select(p => this.Client.Message<AddFileMessage>().SendAsync(p));
            
            var fileTaskResults = await Task.WhenAll(fileTasks);
            
            var files = fileTaskResults.Where(res => res.Success).Select(res => res.Value).ToList();
            
            var linksTasks = files.Select(x => this.Client.Message<GetFileInfoMessage>().SendAsync(x.Hash, x.Name));
            
            var linksTaskResults = await Task.WhenAll(linksTasks);
            
            var links = linksTaskResults.Where(res => res.Success).Select(res => res.Value).ToList();
            
            if (recursive)
            {
                var folderTasks = Directory
                    .EnumerateDirectories(path)
                    .Select(dir => this.SendAsync(dir, recursive));
                
                var folderTaskResults = await Task.WhenAll(folderTasks);
            
                var folders = folderTaskResults.Where(res => res.Success).Select(res => res.Value).ToList();
                
                files.AddRange(folders);
                
                links.AddRange(folders.Select(dir => new IPFSObjectInfo { 
                    Hash = dir.Hash, 
                    Name = dir.Name, 
                    IsDirectory = true, 
                    Size =0 } ));
            }
             
            var newFolderResult = await this.Client.Message<AddIPFSObjectMessage>().SendAsync();
            if(!newFolderResult.Success)
            {
                result.AddErrors(newFolderResult.Errors);
                return result;
            }
            
            var newFolder = newFolderResult.Value;
            
            newFolder.Links.AddRange(links);
            
            var newFolderHashResult = await this.Client.Message<UpdateIPFSObjectMessage>().SendAsync(newFolder);
            if(!newFolderHashResult.Success)
            {
                result.AddErrors(newFolderHashResult.Errors);
                return result;
            }
            
            var storeFolderResult = await this.Client.Message<StoreFolderMessage>().SendAsync(newFolderHashResult.Value.Hash);
            if(!storeFolderResult.Success)
            {
                result.AddErrors(storeFolderResult.Errors);
                return result;
            }
            
            result.SetValue(new IPFSHash{Hash = newFolderHashResult.Value.Hash, Name = Path.GetFileName(path) });
            
            return result;
        }
    }
}