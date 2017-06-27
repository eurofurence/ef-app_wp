using Eurofurence.Companion.DataModel;
using Eurofurence.Companion.DataModel.Api;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System;
using System.Linq;

namespace Eurofurence.Companion.ViewModel.Local.Entity
{
    public class KnowledgeGroupViewModel : BindableBase
    {
        public KnowledgeGroup Entity { get; }
        public ICollection<KnowledgeEntryViewModel> Entries { get; }

        public string FontAwesomeCharacter
        {
            get
            {
                var unicodeAddress = !string.IsNullOrWhiteSpace(Entity.FontAwesomeIconCharacterUnicodeAddress)
                    ? Entity.FontAwesomeIconCharacterUnicodeAddress : "f129";

                return Regex.Unescape(@"\u" + unicodeAddress);
            }
        }

        public KnowledgeGroupViewModel(KnowledgeGroup entity)
        {
            Entity = entity;
            Entries = new Collection<KnowledgeEntryViewModel>();
        }

        public void Resolve()
        {
            var entries = Entries.OrderBy(a => a.Entity.SortOrderKey).ToList();
            for (int i = 0; i < entries.Count; i++)
            {
                entries[i].Parent = this;
                entries[i].PreviousEntry = i > 0 ? entries[i - 1] : null;
                entries[i].NextEntry = i < entries.Count-1 ? entries[i + 1] : null;
            }
        }
    }
}
