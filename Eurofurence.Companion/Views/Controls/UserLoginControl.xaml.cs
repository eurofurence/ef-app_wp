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

        private void E_PasswordBox_KeyUp(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
                E_Button_Login.Command.Execute(null);
        }

        private void E_TextBox_Nickname_KeyUp(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
                E_PasswordBox.Focus(FocusState.Keyboard);

        }
    }
}
