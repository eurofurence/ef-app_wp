using Eurofurence.Companion.Common;
using Eurofurence.Companion.DataModel.Api;
using Eurofurence.Companion.DataStore;
using Eurofurence.Companion.DependencyResolution;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Appointments;

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
