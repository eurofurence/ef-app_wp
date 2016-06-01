using System.Collections.ObjectModel;
using Eurofurence.Companion.DataModel.Api;
using Eurofurence.Companion.DataStore.Abstractions;
using Eurofurence.Companion.DependencyResolution;

namespace Eurofurence.Companion.ViewModel.Local
{
    [IocBeacon]
    public class InfoViewModel : BindableBase
    {
        private readonly IDataContext _dataContext;

        public ObservableCollection<Info> Infos => _dataContext.Infos;
        public ObservableCollection<InfoGroup> InfoGroups => _dataContext.InfoGroups;


        public InfoViewModel(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }
    }
}
