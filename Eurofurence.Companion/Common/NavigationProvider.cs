using Eurofurence.Companion.DependencyResolution;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Eurofurence.Companion.Common
{

    [IocBeacon(TargetType = typeof(INavigationProvider), Scope = IocBeacon.ScopeEnum.Singleton)]
    public class NavigationProvider : INavigationProvider
    {
        private INavigationMediator _navigationMediator;

        public ObservableCollection<NavigationMenuItem> MainMenu { get; private set; }

        public NavigationProvider(INavigationMediator navigationMediator)
        {
            _navigationMediator = navigationMediator;
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
                        _navigationMediator.Navigate(typeof(Views.InfoPage), forceNewStack: true);
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
                        _navigationMediator.Navigate(typeof(Views.EventsPage), forceNewStack: true);
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
                        _navigationMediator.Navigate(typeof(Views.DealerListPage), forceNewStack: true);
                    })
                }
            };
        }
    }
}
