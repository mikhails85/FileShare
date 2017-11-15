using System;
using System.Collections.Generic;
using System.Linq;

namespace IPFS.Integration.Models
{
    public class IPFSObject
    {
        public byte[] Data{ get; set; }
        public List<IPFSObjectLink> Links { get; set; }
    }
}