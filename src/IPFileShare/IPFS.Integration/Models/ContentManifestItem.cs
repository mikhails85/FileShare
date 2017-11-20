namespace IPFS.Integration.Models
{
    public class ContentManifestItem
    {
        public string Title { get; set; }
        public string ThumbnailHash { get; set; }
        public string Description { get; set; }
        public ResourceType Type  { get; set; }
        public string ResourceHash { get; set; }
    }
}