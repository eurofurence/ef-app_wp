using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Eurofurence.Companion.Common
{
    public static class ThemeManager
    {
        private static readonly string[] brushKeys = new[]
        {
            //wp
            "PhoneHighContrastSelectedForegroundThemeBrush",
            "JumpListDefaultEnabledBackground","ListPickerFlyoutPresenterSelectedItemForegroundThemeBrush",
            "ProgressBarBackgroundThemeBrush",
            "PhoneAccentBrush",
            "PhoneRadioCheckBoxPressedBrush",
            "TextSelectionHighlightColorThemeBrush",
            "ButtonPressedBackgroundThemeBrush",
            "CheckBoxPressedBackgroundThemeBrush",
            "ComboBoxHighlightedBorderThemeBrush",
            "ComboBoxItemSelectedForegroundThemeBrush",
            "ComboBoxPressedBackgroundThemeBrush",
            "HyperlinkPressedForegroundThemeBrush",
            "ListBoxItemSelectedBackgroundThemeBrush",
            "ListBoxItemSelectedPointerOverBackgroundThemeBrush",
            "ListViewItemCheckHintThemeBrush",
            "ListViewItemCheckSelectingThemeBrush",
            "ListViewItemDragBackgroundThemeBrush",
            "ListViewItemSelectedBackgroundThemeBrush",
            "ListViewItemSelectedPointerOverBackgroundThemeBrush",
            "ListViewItemSelectedPointerOverBorderThemeBrush",
            "ProgressBarForegroundThemeBrush",
            "ProgressBarIndeterminateForegroundThemeBrush",
            "SliderTrackDecreaseBackgroundThemeBrush",
            "SliderTrackDecreasePointerOverBackgroundThemeBrush",
            "SliderTrackDecreasePressedBackgroundThemeBrush",
            "ToggleSwitchCurtainBackgroundThemeBrush",
            "ToggleSwitchCurtainPointerOverBackgroundThemeBrush",
            "ToggleSwitchCurtainPressedBackgroundThemeBrush",
            "LoopingSelectorSelectionBackgroundThemeBrush",
 
            // windows
            "ComboBoxItemSelectedBackgroundThemeBrush",
            "ComboBoxSelectedBackgroundThemeBrush",
            "IMECandidateSelectedBackgroundThemeBrush",
            "ListBoxItemSelectedBackgroundThemeBrush",
            "ListViewItemSelectedBackgroundThemeBrush",
            "SearchBoxButtonBackgroundThemeBrush",
            "SearchBoxHitHighlightForegroundThemeBrush",
            "TextSelectionHighlightColorThemeBrush",

        };


        public static void SetThemeColor(Color color)
        {
            foreach (var brushKey in brushKeys)
            {
                if (Application.Current.Resources.ContainsKey(brushKey))
                {
                    var solidColorBrush = Application.Current.Resources[brushKey] as SolidColorBrush;
                    if (solidColorBrush != null)
                        solidColorBrush.Color = color;
                }
            }

#if WINDOWS_PHONE_APP
            var statusBar = StatusBar.GetForCurrentView();
            statusBar.ForegroundColor = color;
#endif
        }
    }
}
