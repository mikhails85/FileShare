using System;
using System.Collections.Generic;
using System.Linq;
using IPFS.Utils.Logger;
using IPFS.Results;
using System.Threading.Tasks;
using IPFS.Integration.Messages;
using IPFS.Integration.Models;
using IPFS.Integration.Errors;
using IPFS.Integration.Abstractions;
using IPFS.Services.Contracts;
using IPFS.Utils.DI;
using IPFS.Services.DTO;

namespace IPFS.Services.Handlers
{
    [Injectable]
    public class GetLocalManifestItems
    {
        private readonly IIPFSClient client;
        private readonly ILogger<GetLocalManifestItems> logger;
        
        public GetLocalManifestItems(IIPFSClient client, ILogger<GetLocalManifestItems> logger)
        {
            this.client = client;
            this.logger = logger;
        }
        
        public async Task<Result<List<ContentInfoDTO>>> Get()
        {
            var result = new Result<List<ContentInfoDTO>>();
            
            var peerResult = await client.Message<GetPeerInformationMessage>().SendAsync();
            
            if(!peerResult.Success)
            {
                result.AddErrors(peerResult.Errors);
                return result;
            }
            
            var manifestResult = await client.Message<GetContentManifestMessage>().SendAsync(peerResult.Value.ID);
            if(!manifestResult.Success)
            {
                //result.AddErrors(manifestResult.Errors);
                logger.Result(manifestResult, "Get local manifest error");
                
                result.SetValue(new List<ContentInfoDTO>());
                return result;
            }
            
            var manifestItems = manifestResult.Value.Content;
            
            var fileInfoTasks = manifestItems.Select(x=>client.Message<GetFileInfoMessage>().SendAsync(x.ResourceHash)).ToList();
            
            var fileInfoResults = await Task.WhenAll(fileInfoTasks);
            
            var files = fileInfoResults.Where(res => res.Success).Select(res => res.Value).ToList();
            
            var contentInfoList = new List<ContentInfoDTO>();
            
            foreach(var file in files)
            {
                var info = manifestItems.First(x=>x.ResourceHash == file.Hash);
                
                contentInfoList.Add(new ContentInfoDTO
                {
                    Hash = info.ResourceHash,
                    Name = info.Title,
                    Size = file.Size,
                    Type = info.Type,
                    Thumbnail = info.ThumbnailHash,
                    Description = info.Description
                });
            }
            
            result.SetValue(contentInfoList);
            
            return result;
        }
        
    }
}