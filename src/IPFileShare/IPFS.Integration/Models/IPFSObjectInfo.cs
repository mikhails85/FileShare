using System;
using System.Collections.Generic;
using System.Linq;

namespace IPFS.Integration.Models
{
    public class IPFSObjectInfo : IPFSObjectLink
    {
        public bool IsDirectory { get; set; }
        public List<IPFSObjectInfo> Links { get; set; } = new List<IPFSObjectInfo>();
    }
}