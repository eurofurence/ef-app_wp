using Eurofurence.Companion.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Eurofurence.Companion.Common
{

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
}
