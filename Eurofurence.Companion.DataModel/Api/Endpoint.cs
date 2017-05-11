using System;
using System.Collections.Generic;

namespace Eurofurence.Companion.DataModel.Api
{
    public class Endpoint
    {
        public DateTime CurrentDateTimeUtc { get; set; }
        public List<EndpointConfiguration> Configuration { get; set; }

        public List<EndpointEntity> Entities { get; set; } 
    }
}