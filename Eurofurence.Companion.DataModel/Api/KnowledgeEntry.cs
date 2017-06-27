using System;
using System.Collections.Generic;
using Eurofurence.Companion.DataModel.Abstractions;
using Newtonsoft.Json;

namespace Eurofurence.Companion.DataModel.Api
{
    public class KnowledgeEntry : EntityBase, ISortOrderKeyProvider
    {
        public Guid KnowledgeGroupId { get; set; }

        public string Title { get; set; }
        public string Text { get; set; }
        public int Order { get; set; }


        public LinkFragment[] Links { get; set; }

        public Guid[] ImageIds { get; set; }

        public KnowledgeEntry()
        {
            Images = new List<Image>();
        }

        [JsonIgnore]
        public virtual List<Image> Images { get; set; }

        [JsonIgnore]
        public virtual KnowledgeGroup Group { get; set; }

        [JsonIgnore]
        public object SortOrderKey => Order;
    }
}