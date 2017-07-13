using Eurofurence.Companion.DataModel.Api;

namespace Eurofurence.Companion.ViewModel.Local.Entity
{
    public class KnowledgeEntryViewModel
    {
        public KnowledgeGroupViewModel Parent { get; set; }

        public KnowledgeEntryViewModel PreviousEntry { get; set; }
        public KnowledgeEntryViewModel NextEntry { get; set; }
        public bool HasPreviousEntry => PreviousEntry != null;
        public bool HasNextEntry => NextEntry != null;
        public bool HasPreviousOrNextEntry => HasPreviousEntry || HasNextEntry;

        public LinkFragmentAction[] LinkActions { get; set; }

        public KnowledgeEntry Entity { get; }

        public KnowledgeEntryViewModel(KnowledgeEntry entity)
        {
            Entity = entity;
        }
    }
}
