using System;
using System.Collections.Generic;
using System.Linq;

namespace IPFS.Integration.Models
{
    public class ContentManifest
    {
        public string Title { get; set; }
        public string ThumbnailHash { get; set; }
        public string Description { get; set; }
        public ResourceType Type  { get; set; }
        public string ResourceHash { get; set; }
    }
}