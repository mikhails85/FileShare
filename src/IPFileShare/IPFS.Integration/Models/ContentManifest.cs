using System;
using System.Collections.Generic;

namespace IPFS.Integration.Models
{
    public class ContentManifest
    {
        public DateTime LastMidification {get; set;}
        public string PeedID {get; set;}
        public List<ContentManifestItem> Content {get; set;} = new List<ContentManifestItem>();
    }
}