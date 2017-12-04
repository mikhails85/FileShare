using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IPFS.Services.DTO
{
    public class UploadFileDTO
    {
        public string ResourceHash {get;set;}
        public string ThumbnailHash {get;set;}
    }
}