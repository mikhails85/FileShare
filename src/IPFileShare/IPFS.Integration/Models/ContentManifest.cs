using System;
using System.Collections.Generic;
using System.Linq;

namespace IPFS.Integration.Models
{
    public class ContentManifest
    {
        public DateTime LastMidification {get; set;}
        public string PeedID {get; set;}
        public List<ContentManifestItem> Content {get; set;} = new List<ContentManifestItem>();
    }
    
    public class ContentManifestItem
    {
        public string Title { get; set; }
        public string ThumbnailHash { get; set; }
        public string Description { get; set; }
        public ResourceType Type  { get; set; }
        public string ResourceHash { get; set; }
    }
}