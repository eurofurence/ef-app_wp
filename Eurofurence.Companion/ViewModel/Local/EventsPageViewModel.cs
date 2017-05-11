using Eurofurence.Companion.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eurofurence.Companion.ViewModel.Local
{
    public class EventsPageViewModel: BindableBase
    {
        public class EventGroupViewModel : BindableBase
        {
            private bool _isSelected;
            public bool IsSelected { get { return _isSelected; } set { SetProperty(ref _isSelected, value); } }

            public string Icon { get; set; }
            public string Name { get; set; }

            public EventGroupViewModel()
            {
                InitializeDispatcherFromCurrentThread();
            }
        }


        public ObservableCollection<EventGroupViewModel> Groups { get; private set; }

        public EventsPageViewModel()
        {
            InitializeDispatcherFromCurrentThread();
            Groups = new ObservableCollection<EventGroupViewModel>()
            {
                new EventGroupViewModel { Name = "Day", Icon="\uE163", IsSelected = true },
                new EventGroupViewModel { Name = "Track", Icon="\uE8EC" },
                new EventGroupViewModel { Name = "Room" , Icon="\uE1D2" },
                new EventGroupViewModel { Name = "Search" , Icon="\uE71E" }
            };
        }

    }


    
}
