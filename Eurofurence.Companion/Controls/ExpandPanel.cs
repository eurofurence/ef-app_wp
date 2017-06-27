using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Eurofurence.Companion.Controls
{
    /// <summary>
    /// A custom control that allows developers to create custom expandable views.
    /// An ExpandPanel is a combination of three contents:
    /// First one is a header, on clicking which the panel expands and shows the content.
    /// When expanded, the header is collapsed, and selected state of the header is displayed instead.
    /// Second one is the selected state of the header. On clicking on this, the panel collapse and hides the content.
    /// When collapsed, the selected state of the header is collapsed and the header is displayed instead.
    /// Third is the main content of the ExpandPanel.
    /// </summary>
    public sealed class ExpandPanel : ContentControl
    {
        #region Constructors
        public ExpandPanel()
        {
            DefaultStyleKey = typeof(ExpandPanel);
            IsExpanded = false;
        }
        #endregion

        #region Local Variables
        /// <summary>
        /// Denotes whether to use transitions(animation effects) while performing UI actions.
        /// </summary>
        private bool _useTransitions = true;

        /// <summary>
        /// The collapsed state of the ExpandPanel.
        /// </summary>
        private VisualState _collapsedState;

        /// <summary>
        /// The content element of the ExpandPanel.
        /// </summary>
        private FrameworkElement contentElement;

        /// <summary>
        /// The normal header element of the ExpandPanel.
        /// </summary>
        private FrameworkElement headerElement;

        /// <summary>
        /// The selected state header element of the ExpandPanel.
        /// </summary>
        private FrameworkElement headerElementSelected;
        #endregion

        #region Dependency Properties
        /// <summary>
        /// The dependency property that registers the value of the the HeaderContent property.
        /// </summary>
        public static readonly DependencyProperty HeaderContentProperty = DependencyProperty.Register("HeaderContent", typeof(object), typeof(ExpandPanel), null);

        /// <summary>
        /// The dependency property that registers the value of the SelectedHeaderContent property.
        /// </summary>
        public static readonly DependencyProperty SelectedHeaderContentProperty = DependencyProperty.Register("SelectedHeaderContent", typeof(object), typeof(ExpandPanel), null);

        /// <summary>
        /// The dependency property that registers the value of the IsExpanded property.
        /// </summary>
        public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register("IsExpanded", typeof(bool), typeof(ExpandPanel), new PropertyMetadata(true, new PropertyChangedCallback(IsExpandedChanged)));

        /// <summary>
        /// The dependency property that registers the value of the IsAnimationEnabled property.
        /// </summary>
        public static readonly DependencyProperty IsAnimationEnabledProperty = DependencyProperty.Register("IsAnimationEnabled", typeof(bool), typeof(ExpandPanel), new PropertyMetadata(true));
        #endregion

        #region Properties
        /// <summary>
        /// Property that holds the normal state(collapsed state) header content of the expand panel.
        /// </summary>
        public object HeaderContent
        {
            get { return GetValue(HeaderContentProperty); }
            set { SetValue(HeaderContentProperty, value); }
        }

        /// <summary>
        /// Property that holds the selected state(expanded state) header content of the expand panel.
        /// </summary>
        public object SelectedHeaderContent
        {
            get { return GetValue(SelectedHeaderContentProperty); }
            set { SetValue(SelectedHeaderContentProperty, value); }
        }

        /// <summary>
        /// The property to determine whether the expand panel is expanded or collapsed.
        /// </summary>
        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set
            {
                SetValue(IsExpandedProperty, value);
                if (headerElement != null && headerElementSelected != null)
                {
                    if (value)
                    {
                        headerElementSelected.Visibility = Visibility.Visible;
                        headerElement.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        headerElementSelected.Visibility = Visibility.Collapsed;
                        headerElement.Visibility = Visibility.Visible;
                    }
                }
                changeVisualState(_useTransitions);
            }
        }

        /// <summary>
        /// The property to determine whether the transitions in the panel should be animated or not.
        /// </summary>
        public bool IsAnimationEnabled
        {
            get { return _useTransitions; }
            set { _useTransitions = value; }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Change the visual state of the ExpandPanel depending on the IsExpanded property.
        /// </summary>
        /// <param name="useTransitions">
        /// Whether the visual state change should be animated or not.
        /// </param>
        private void changeVisualState(bool useTransitions)
        {
            if (IsExpanded)
            {
                if (contentElement != null)
                {
                    contentElement.Visibility = Visibility.Visible;
                }
                VisualStateManager.GoToState(this, "Expanded", useTransitions);
            }
            else
            {
                VisualStateManager.GoToState(this, "Collapsed", useTransitions);
                _collapsedState = (VisualState)GetTemplateChild("Collapsed");
                if (_collapsedState == null)
                {
                    if (contentElement != null)
                    {
                        contentElement.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Called when the template is applied.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            headerElement = GetTemplateChild("HeaderElement") as FrameworkElement;
            if (headerElement != null)
            {
                headerElement.Tapped += HeaderElement_Tapped;
            }
            headerElementSelected = GetTemplateChild("SelectedHeaderElement") as FrameworkElement;
            if (headerElementSelected != null)
            {
                headerElementSelected.Tapped += HeaderElementSelected_Tapped; ;
            }
            if (headerElement != null && headerElementSelected != null)
            {
                if (IsExpanded)
                {
                    headerElementSelected.Visibility = Visibility.Visible;
                    headerElement.Visibility = Visibility.Collapsed;
                }
                else
                {
                    headerElementSelected.Visibility = Visibility.Collapsed;
                    headerElement.Visibility = Visibility.Visible;
                }
            }
            contentElement = GetTemplateChild("Content") as FrameworkElement;
            if (contentElement != null)
            {
                _collapsedState = GetTemplateChild("Collapsed") as VisualState;
                if ((_collapsedState != null) && (_collapsedState.Storyboard != null))
                {
                    _collapsedState.Storyboard.Completed += (object sender, object e) =>
                    {
                        contentElement.Visibility = Visibility.Collapsed;
                    };
                }
            }
            changeVisualState(false);
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Called when the selected state(expanded state) header is tapped.
        /// </summary>
        private void HeaderElementSelected_Tapped(object sender, TappedRoutedEventArgs e)
        {
            IsExpanded = false;
        }

        /// <summary>
        /// Called when the normal state(collapsed state) header is tapped.
        /// </summary>
        private void HeaderElement_Tapped(object sender, TappedRoutedEventArgs e)
        {
            IsExpanded = true;
        }

        /// <summary>
        /// Called when the ISExpanded property changes.
        /// </summary>
        /// <param name="sender">
        /// the element that sent the changed event.
        /// </param>
        /// <param name="e">
        /// The changed event args.
        /// </param>
        private static void IsExpandedChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ExpandPanel source = sender as ExpandPanel;
            source.IsExpanded = (bool)e.NewValue;
        }
        #endregion
    }
}
