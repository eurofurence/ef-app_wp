using System;
using System.Collections.Generic;
using Eurofurence.Companion.Common;
using Eurofurence.Companion.DataModel;

namespace Eurofurence.Companion.ViewModel.Local
{
    public class MenuItemViewModel : BindableBase
    {
        private bool _isActive;

        public string Title { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public RelayCommand NavigationCommand { get; set; }
        public ICollection<Type> ChildTypes { get; set; }

        public MenuItemViewModel()
        {
            InitializeDispatcherFromCurrentThread();
        }

        public bool IsActive
        {
            get { return _isActive; }
            set { SetProperty(ref _isActive, value); }
        }
    }
}