using Eurofurence.Companion.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Eurofurence.Companion.Common
{
    public class MockNavigationProvider: INavigationProvider
    {
        public ObservableCollection<NavigationMenuItem> MainMenu { get; private set; }

        public MockNavigationProvider()
        {
            MainMenu = new ObservableCollection<NavigationMenuItem>()
            {
                new NavigationMenuItem {
                    Title = "Item 1",
                    Icon = "\uEC42"
                },
                new NavigationMenuItem {
                    Title = "Item 2",
                    Icon = "\uE163"
                },
                new NavigationMenuItem {
                    Title = "Item 3",
                    Icon = "\uE13D"
                }
            };
        }

    }

    public class NavigationProvider : INavigationProvider
    {
        private ILayoutPage _layoutPage;

        public ObservableCollection<NavigationMenuItem> MainMenu { get; private set; }

        public NavigationProvider(ILayoutPage layoutPage)
        {
            _layoutPage = layoutPage;
            BuildMainMenu();
        }

        private void BuildMainMenu()
        {
            MainMenu = new ObservableCollection<NavigationMenuItem>()
            {
                new NavigationMenuItem {
                    Title = Translations.Info_Title,
                    Icon = Views.InfoPage.PAGE_ICON,
                    Description = "Helpful information across all areas & departments",
                    NavigationCommand = new RelayCommand(p => {
                        _layoutPage.Navigate(typeof(Views.InfoPage), forceNewStack: true);
                    }),
                    ChildTypes = new List<Type>() {
                        typeof(Views.InfoPage),
                        typeof(Views.InfoGroupDetailPage)
                    }
                },
                new NavigationMenuItem {
                    Title = Translations.EventSchedule_Title,
                    Icon = Views.EventsPage.PAGE_ICON,
                    Description = "What's happening, when & where",
                    NavigationCommand = new RelayCommand(p => {
                        _layoutPage.Navigate(typeof(Views.EventsPage), forceNewStack: true);
                    }),
                    ChildTypes = new List<Type>() {
                        typeof(Views.EventsPage),
                        typeof(Views.EventsByDayPage),
                        typeof(Views.EventDetailPage)
                    }
                },
                new NavigationMenuItem{
                    Title = Translations.Dealers_Title,
                    Icon = Views.DealerListPage.PAGE_ICON,
                    Description = "What's happening, when & where",
                    NavigationCommand = new RelayCommand(p => {
                        _layoutPage.Navigate(typeof(Views.DealerListPage), forceNewStack: true);
                    })
                }
            };
        }
    }

    public class NavigationMenuItem : BindableBase
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public RelayCommand NavigationCommand { get; set; }
        public ICollection<Type> ChildTypes { get; set; }

        private bool _isActive = false;
        public bool IsActive { get { return _isActive; } set { SetProperty(ref _isActive, value); } }
    }
}
