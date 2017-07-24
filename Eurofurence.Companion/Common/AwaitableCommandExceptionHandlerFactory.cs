using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Eurofurence.Companion.DependencyResolution;

namespace Eurofurence.Companion.Common
{
    public static class AwaitableCommandExceptionHandlerFactory
    {
        public static Task Retry(Exception e, Func<Task> t, Action onCancel)
        {
            return RetryTask(e, t, onCancel);
        }

        public static Task RetryOrReturnToMainPage(Exception e, Func<Task> t)
        {
            return RetryTask(e, t, () => ViewModelLocator.Current.NavigationViewModel.NavigateToMainPage.Execute(null));
                
        }

        private static async Task RetryTask(Exception e, Func<Task> t, Action onCancel)
        {
            await new MessageDialog($"There was a problem contacting the server.\n\nWould you like to retry?\n\nError:\n{e.Message}", "Unable to retrieve data")
                {
                    CancelCommandIndex = 0,
                    DefaultCommandIndex = 1,
                    Commands =
                    {
                        new UICommand("Cancel", _ => onCancel.Invoke()),
                        new UICommand("Retry", async _=> await t())
                    }
                }
                .ShowAsync();
        }

    }
}
