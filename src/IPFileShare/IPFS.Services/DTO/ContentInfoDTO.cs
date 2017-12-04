using System;
using System.Collections.Generic;
using System.Linq;
using IPFS.Integration.Models;

namespace IPFS.Services.DTO
{
    public class ContentInfoDTO
    {
        public string Hash {get;set;}
        public string Name {get;set;}
        public long Size {get;set;}
        public ResourceType Type {get;set;}
        public string Thumbnail { get;set; }
        public string Description { get;set; }
    }
}