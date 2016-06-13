using Eurofurence.Companion.DataModel.Api;

namespace Eurofurence.Companion.ViewModel.Local.Entity
{
    public class InfoViewModel
    {
        public Info Entity { get; }

        public InfoViewModel(Info entity)
        {
            Entity = entity;
        }
    }
}
