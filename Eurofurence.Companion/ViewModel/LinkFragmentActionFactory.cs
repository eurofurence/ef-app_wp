using Eurofurence.Companion.DataModel.Api;
using Eurofurence.Companion.DependencyResolution;
using Eurofurence.Companion.ViewModel.Local.Entity;
using System;
using System.Linq;
using Windows.System;

namespace Eurofurence.Companion.ViewModel
{
    public static class LinkFragmentActionFactory
    {
        public static LinkFragmentAction ConvertFragment(LinkFragment fragment)
        {
            switch (fragment.FragmentType)
            {
                case LinkFragment.FragmentTypeEnum.DealerDetail:

                    var dealer = ViewModelLocator.Current.DealersViewModel.Dealers.SingleOrDefault(a => a.Entity.Id.ToString() == fragment.Target);
                    if (dealer == null) return null;


                    return new LinkFragmentAction()
                    {
                        TargetTypeName = "Dealer",
                        TargetName = dealer.HasUniqueDisplayName ? $"{dealer.DisplayName} ({dealer.Entity.AttendeeNickname})" : dealer.DisplayName,
                        Execute = () =>
                        {
                            ViewModelLocator.Current.NavigationViewModel.NavigateToDealerDetailPage.Execute(dealer);
                        }
                    };


                case LinkFragment.FragmentTypeEnum.WebExternal:

                    if (!Uri.IsWellFormedUriString(fragment.Target, UriKind.Absolute)) return null;

                    return new LinkFragmentAction()
                    {
                        TargetTypeName = "Website",
                        TargetName = fragment.Name,
                        Execute = async () =>
                        {
                            await Launcher.LaunchUriAsync(new Uri(fragment.Target));
                        }
                    };

            }

            return null;
        }

        public static LinkFragmentAction[] ConvertFragments(LinkFragment[] fragments)
        {
            return fragments
                .Select(fragment => ConvertFragment(fragment))
                .Where(result => result != null)
                .ToArray();
        }

    }
}
