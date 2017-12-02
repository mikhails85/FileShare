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

namespace IPFS.Services.Handlers
{
    [Injectable]
    public class AddContentManifestItem
    {
        private readonly IIPFSClient client;
        private readonly ILogger<AddContentManifestItem> logger;
        
        public AddContentManifestItem(IIPFSClient client, ILogger<AddContentManifestItem> logger)
        {
            this.client = client;
            this.logger = logger;
        }        
        
        public async Task<VoidResult> Add(IContentManifestItem item)
        {
            var result = new VoidResult();
            
            var addContentTask = GetContentTask(item);
            var addThumbnailTask = this.client.Message<AddFileMessage>().SendAsync(item.Resource);
            
            await Task.WhenAll(addContentTask, addThumbnailTask);
            
            var addContentResult = await addContentTask;
            var addThumbnailResult = await addThumbnailTask;
            
            if(!addContentResult.Success)
            {
                result.AddErrors(addContentResult.Errors);
            }
            
            if(!addThumbnailResult.Success)
            {
                result.AddErrors(addThumbnailResult.Errors);
            }
            
            if(result.Errors.Any())
                return result;
            
            var peerResult = await client.Message<GetPeerInformationMessage>().SendAsync();
            
            if(!peerResult.Success)
            {
                result.AddErrors(peerResult.Errors);
                return result;
            }
            
            var resolveResult = await client.Message<ResolvePublishedObjectMessage>().SendAsync(peerResult.Value.ID);
            
            var manifest = new ContentManifest();
            manifest.PeedID = peerResult.Value.ID;
            
            if(!resolveResult.Success && resolveResult.Errors.All(x=>x.Code != (int)ErrorCodes.HttpRequest))
            {
                result.AddErrors(resolveResult.Errors);
                return result;
            }
            
            if(resolveResult.Success)
            {
                var manifestResult = await client.Message<GetContentManifestMessage>().SendAsync(peerResult.Value.ID);
                if(!manifestResult.Success)
                {
                    result.AddErrors(manifestResult.Errors);
                    return result;
                }
                manifest = manifestResult.Value;
            }
            
            manifest.PeedID = peerResult.Value.ID;
            manifest.LastMidification = DateTime.UtcNow;
            manifest.Content.Add(new ContentManifestItem{
                Title = item.Title,
                ThumbnailHash = addThumbnailResult.Value.Hash,
                Description = item.Description,
                Type = item.Type,
                ResourceHash = addContentResult.Value.Hash
            });
            
            var updateManifest = await client.Message<AddContentManifestMessage>().SendAsync(manifest);
            
            if(!updateManifest.Success)
            {
                result.AddErrors(updateManifest.Errors);
            }
            
            return result; 
        }
        
        private Task<Result<IPFSHash>> GetContentTask(IContentManifestItem item)
        {
            switch (item.Type)
            {
                case ResourceType.File:
                case ResourceType.Text:
                case ResourceType.Image:
                case ResourceType.Video:
                case ResourceType.Audio:
                  return this.client.Message<AddFileMessage>().SendAsync(item.Resource);
                case ResourceType.VideoList:
                case ResourceType.AudioList:
                case ResourceType.ImageList:  
                case ResourceType.Folder:
                  return this.client.Message<AddFolderMessage>().SendAsync(item.Resource);
            }
            
            return this.client.Message<AddFileMessage>().SendAsync(item.Resource);
        }
    }
}