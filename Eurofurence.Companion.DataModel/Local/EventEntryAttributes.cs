using Eurofurence.Companion.DataModel.Abstractions;
using Newtonsoft.Json;

namespace Eurofurence.Companion.DataModel.Local
{
    public class EventEntryAttributes : EntityBase, IEntityExtension
    {

        private bool _isFavorite = false;
        public bool IsFavorite { get { return _isFavorite; } set { SetProperty(ref _isFavorite, value); } }

        public bool GetPersistence()
        {
            return IsFavorite;
        }
    }
}
