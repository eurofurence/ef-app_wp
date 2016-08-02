using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Eurofurence.Companion.ViewModel.Local;

namespace Eurofurence.Companion.ViewModel.Behaviors
{
    public class MainMenuDataTemplateSelector : DataTemplateSelector
    {

        public DataTemplate MenuItemTemplate { get; set; }
        public DataTemplate MenuSpacerTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item is MenuItemViewModel)
                return MenuItemTemplate;

            return MenuSpacerTemplate;
        }
    }
}
