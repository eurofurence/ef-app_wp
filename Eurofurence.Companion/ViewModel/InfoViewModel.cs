using Eurofurence.Companion.DataModel.Api;
using Eurofurence.Companion.DependencyResolution;
using System.Collections.ObjectModel;
using Eurofurence.Companion.DataStore.Abstractions;

namespace Eurofurence.Companion.ViewModel
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
