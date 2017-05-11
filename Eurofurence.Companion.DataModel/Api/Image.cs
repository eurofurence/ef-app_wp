namespace Eurofurence.Companion.DataModel.Api
{
    public class Image : EntityBase
    {
        public string InternalReference { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int SizeInBytes { get; set; }
        public string MimeType { get; set; }
        public string ContentHashSha1 { get; set; }
        public byte[] Content { get; set; }
    }
}