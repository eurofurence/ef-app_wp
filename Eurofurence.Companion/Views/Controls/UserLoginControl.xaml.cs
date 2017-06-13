using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Eurofurence.Companion.Views.Controls
{
    public sealed partial class UserLoginControl : UserControl
    {
        public UserLoginControl()
        {
            this.InitializeComponent();

            this.Loaded += (e, a) =>
            {
                E_Button_Login.Focus(FocusState.Keyboard);
            };
        }
    }
}
