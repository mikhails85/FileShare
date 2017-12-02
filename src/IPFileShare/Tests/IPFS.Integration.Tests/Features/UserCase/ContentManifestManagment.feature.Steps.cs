using IPFS.Integration.Messages;
using IPFS.Integration.Models;
using IPFS.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace IPFS.Integration.Tests.Features.UserCase
{
    public partial class ContentManifestManagment: BaseFeature
    {
        
        private Result<PeerInfo> peerInfo; 
        
        private Result<List<IPFSObjectInfo>> filesResult; 
        
        private ContentManifest manifest;
        
        private Result<IPFSHash> result; 
        
        private Result<ContentManifest> manifestResult; 
        
        
        private async void When_user_get_peer_information()
        {
            peerInfo = await client.Message<GetPeerInformationMessage>().SendAsync();
            Assert.True(peerInfo.Success);
        }
        
        private async void When_user_get_list_of_local_files()
        {
            filesResult = await this.client.Message<GetLocalObjectListMessage>().SendAsync();
            Assert.True(filesResult.Success);
        }
        
        private void Then_client_should_create_manifest()
        {
            var dirs = filesResult.Value.Where(x=>x.IsDirectory).ToList();
            var filesInDir = dirs.SelectMany(x=>x.Links).Where(x=>!x.IsDirectory).Select(x=>x.Hash).ToList();
            var files = filesResult.Value.Where(x=>!x.IsDirectory && !filesInDir.Contains(x.Hash)).ToList();

            manifest = new ContentManifest
            {
                PeedID = peerInfo.Value.ID,
                LastMidification = DateTime.UtcNow
            };

            foreach (var dir in dirs)
            {
                manifest.Content.Add(new ContentManifestItem{
                    ResourceHash = dir.Hash,
                    Type = ResourceType.Folder
                });
            }
            
            foreach(var file in files)
            {
                manifest.Content.Add(new ContentManifestItem{
                    ResourceHash = file.Hash,
                    Type = ResourceType.File
                });
            }
        }
        
        private async void Then_client_should_add_new_manifest()
        {
            result = await this.client.Message<AddContentManifestMessage>().SendAsync(manifest);
            Assert.True(result.Success);
        }
        
        private async void Then_get_manifest_by_peer_id()
        {
            manifestResult = await this.client.Message<GetContentManifestMessage>().SendAsync(peerInfo.Value.ID);
            Assert.True(manifestResult.Success);
        }
        
        private void Then_manifest_should_be_same()
        {
             Assert.Equal(manifest.PeedID, manifestResult.Value.PeedID);    
             Assert.Equal(manifest.LastMidification, manifestResult.Value.LastMidification);    
             Assert.Equal(manifest.Content.Count(), manifestResult.Value.Content.Count());  
        }
        
        public ContentManifestManagment(ITestOutputHelper output):base(output)
        {
            
        }
    }
}