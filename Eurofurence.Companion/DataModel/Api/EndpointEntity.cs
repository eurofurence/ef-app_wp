using System;

namespace Eurofurence.Companion.DataModel.Api
{
    public class EndpointEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime DeltaStartDateTimeUtc { get; set; }
        public DateTime LastChangeDateTimeUtc { get; set; }
    }
}