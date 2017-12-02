using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IPFS.Integration.Models;
using IPFS.Services.Contracts;

namespace IPFS.Desktop.Models
{
    public class ContentManifestItem: IContentManifestItem
    {
        public string Title { get;set; }
        public string Thumbnail { get;set; }
        public string Description { get;set; }
        public ResourceType Type { get;set; }
        public string Resource { get;set; } 
    }
}