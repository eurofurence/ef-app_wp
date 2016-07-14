using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Eurofurence.Companion.DependencyResolution;

namespace Eurofurence.Companion.ViewModel.Behaviors
{
    public static class AsyncImageLoaderProperties
    {

        public static readonly DependencyProperty ImageIdProperty =
            DependencyProperty.RegisterAttached(
              "ImageId",
              typeof(object),
              typeof(AsyncImageLoaderProperties),
              new PropertyMetadata(null, OnImageIdChanged)
            );
        public static void SetImageId(UIElement element, object value)
        {
            element.SetValue(ImageIdProperty, value);
        }
        public static object GetImageId(UIElement element)
        {
            return element.GetValue(ImageIdProperty);
        }

        private static void OnImageIdChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs e)
        {
            if (!(e.NewValue is Guid) || !(depObj is Image)) return;
            ServiceLocator.Current.AsyncImageLoaderService.EnqueueAsyncImageLoadTask((Guid)e.NewValue, (Image)depObj);
        }
    }
}