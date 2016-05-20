namespace Eurofurence.Companion.DataModel.Api
{
    public class Image : EntityBase
    {
        public string Url { get; set; }
        public string Title { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int FileSizeInBytes { get; set; }
        public string MimeType { get; set; }
        public byte[] Content { get; set; }
    }
}