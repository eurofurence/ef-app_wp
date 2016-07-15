using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Animation;
using Eurofurence.Companion.Common;
using Eurofurence.Companion.Common.Abstractions;
using Eurofurence.Companion.DependencyResolution;

namespace Eurofurence.Companion.ViewModel.Local
{
    [IocBeacon(Scope = IocBeacon.ScopeEnum.Singleton)]
    public class MenuViewModel : BindableBase
    {
        private readonly INavigationMediator _navigationMediator;
        private bool _isMenuVisible = false;
        public bool IsMenuVisible {  get { return _isMenuVisible; } set { SetProperty(ref _isMenuVisible, value); } }



        public RelayCommand ToggleMenuCommand => new RelayCommand(p => IsMenuVisible = !IsMenuVisible);
        public RelayCommand OpenMenuCommand => new RelayCommand(p => IsMenuVisible = true);
        public RelayCommand CloseMenuCommand => new RelayCommand(p => IsMenuVisible = false);


        public MenuViewModel(INavigationMediator navigationMediator)
        {
            InitializeDispatcherFromCurrentThread();

            _navigationMediator = navigationMediator;
            _navigationMediator.OnPageLoaded += (s, e) => UpdateActiveMenuItem(e);

            BuildMainMenu();
        }

        private void UpdateActiveMenuItem(Type currentPageType)
        {
            foreach (var item in Items)
            {
                item.IsActive = item.ChildTypes?.Contains(currentPageType) ?? false;
            }
        }

        private void BuildMainMenu()
        {
            Items = new ObservableCollection<MenuItemViewModel>()
            {
                new MenuItemViewModel {
                    Title = Translations.Info_Title,
                    Icon = Views.InfoPage.PAGE_ICON,
                    Description = "Information across all areas & departments",
                    NavigationCommand = new RelayCommand(p => {
                        _navigationMediator.NavigateAsync(typeof(Views.InfoPage), forceNewStack: true);
                    }),
                    ChildTypes = new List<Type>() {
                        typeof(Views.InfoPage),
                        typeof(Views.InfoGroupDetailPage)
                    }
                },
                new MenuItemViewModel {
                    Title = Translations.EventSchedule_Title,
                    Icon = Views.EventsPage.PAGE_ICON,
                    Description = "What's happening, when & where",
                    NavigationCommand = new RelayCommand(p => {
                        _navigationMediator.NavigateAsync(typeof(Views.EventsPage), forceNewStack: true);
                    }),
                    ChildTypes = new List<Type>() {
                        typeof(Views.EventsPage),
                        typeof(Views.EventsByDayPage),
                        typeof(Views.EventDetailPage)
                    }
                },
                new MenuItemViewModel{
                    Title = Translations.Dealers_Title,
                    Icon = Views.DealerListPage.PAGE_ICON,
                    Description = "List of dealers and their merchandise",
                    NavigationCommand = new RelayCommand(p => {
                        _navigationMediator.NavigateAsync(typeof(Views.DealerListPage), forceNewStack: true);
                    }),
                    ChildTypes = new List<Type>() {
                        typeof(Views.DealerListPage),
                        typeof(Views.DealerDetailPage)
                    }
                },
                new MenuItemViewModel{
                    Title = "Maps",
                    Icon = "\uE128",
                    Description = "Convention space maps to help navigating",
                    NavigationCommand = new RelayCommand(p => {
                        _navigationMediator.NavigateAsync(typeof(Views.MapsPage), forceNewStack: true);
                    }),
                    ChildTypes = new List<Type>() {
                        typeof(Views.MapsPage),
                        typeof(Views.MapDetailPage)
                    }

                }
            };
        }

        public ObservableCollection<MenuItemViewModel> Items { get; set; }
    }

}