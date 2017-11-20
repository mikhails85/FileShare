using System.Collections.Generic;

namespace IPFS.Integration.Models
{
    public class PeerInfo
    {
        public string ID {get;set;}
        public string PublicKey {get;set;}
        public List<string> Addresses {get;set;}
        public string AgentVersion {get;set;}
        public string ProtocolVersion {get;set;}
    }
}