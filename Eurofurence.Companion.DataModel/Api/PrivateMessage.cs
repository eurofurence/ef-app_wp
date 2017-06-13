using System;

namespace Eurofurence.Companion.DataModel.Api
{
    public class PrivateMessage
    {
        public Guid Id { get; set; }

        public DateTime CreatedDateTimeUtc { get; set; }
        public DateTime? ReceivedDateTimeUtc { get; set; }
        public DateTime? ReadDateTimeUtc { get; set; }

        public string AuthorName { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }
    }
}
