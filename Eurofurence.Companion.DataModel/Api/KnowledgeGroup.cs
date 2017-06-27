using System.Collections.Generic;
using System.Collections.ObjectModel;
using Eurofurence.Companion.DataModel.Abstractions;
using Newtonsoft.Json;

namespace Eurofurence.Companion.DataModel.Api
{
    public class KnowledgeGroup : EntityBase, ISortOrderKeyProvider
    {
        public KnowledgeGroup()
        {
            Entries = new Collection<KnowledgeEntry>();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }

        public bool ShowInHamburgerMenu { get; set; }

        public string FontAwesomeIconCharacterUnicodeAddress { get; set; }

        [JsonIgnore]
        public ICollection<KnowledgeEntry> Entries { get; set; }

        [JsonIgnore]
        public object SortOrderKey => Order;
    }
}