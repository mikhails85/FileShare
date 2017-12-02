using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IPFS.Integration.Models;

namespace IPFS.Services.Contracts
{
    public interface IContentManifestItem
    {
        string Title { get;set; }
        string Thumbnail { get;set; }
        string Description { get;set; }
        ResourceType Type { get;set; }
        string Resource { get;set; } 
    }
}