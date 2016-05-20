using System;
using System.Collections.Generic;

namespace Eurofurence.Companion.Common
{
    public class NavigationMenuItem : BindableBase
    {
        private bool _isActive;

        public string Title { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public RelayCommand NavigationCommand { get; set; }
        public ICollection<Type> ChildTypes { get; set; }

        public bool IsActive
        {
            get { return _isActive; }
            set { SetProperty(ref _isActive, value); }
        }
    }
}