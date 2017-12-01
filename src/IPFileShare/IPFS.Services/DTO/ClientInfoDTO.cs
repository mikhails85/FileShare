using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IPFS.Services.DTO
{
    public class ClientInfoDTO
    {
        public string PeerId {get;set;}
        public int PeerCount {get;set;}
        public int FilesStrored {get;set;}
        public long TotalFilesSize {get;set;}
    }
}