using Google.Protobuf;
using IPFS.Integration.Models;
using System.IO;

namespace IPFS.Integration.Utils.Protobuf
{
    public static class ProtobufSerializer
    {
        public static byte[] Serialize(this IPFSObject obj)
        {
            using (var stream = new MemoryStream())
            {
                using (var objStream = new CodedOutputStream(stream, true))
                {
                    foreach (var link in obj.Links)
                    {
                        var msg = link.Serialize();
                        objStream.WriteTag(2, WireFormat.WireType.LengthDelimited);
                        objStream.WriteLength(msg.Length);
                        objStream.WriteSomeBytes(msg);
                    }
                    
                    if (obj.Data.Length > 0)
                    {
                        objStream.WriteTag(1, WireFormat.WireType.LengthDelimited);
                        objStream.WriteLength(obj.Data.Length);
                        objStream.WriteSomeBytes(obj.Data);
                    }
                }
                
                return stream.ToArray();
            }
        }
        
        public static byte[] Serialize(this IPFSObjectLink link)
        {
            using (var stream = new MemoryStream())
            {
                using (var listStream = new CodedOutputStream(stream, true))
                {
                    listStream.WriteTag(1, WireFormat.WireType.LengthDelimited);
                    var mh = new MultiHash(link.Hash);
                    listStream.WriteLength(mh.Algorithm.DigestSize + 2); // + 2 bytes for digest size
                    mh.Write(listStream);
        
                    if (link.Name != null)
                    {
                        listStream.WriteTag(2, WireFormat.WireType.LengthDelimited);
                        listStream.WriteString(link.Name);
                    }
        
                    listStream.WriteTag(3, WireFormat.WireType.Varint);
                    listStream.WriteInt64(link.Size);
                }
                return stream.ToArray();
            }
        }
    }
}