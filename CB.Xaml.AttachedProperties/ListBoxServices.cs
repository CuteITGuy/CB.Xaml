using System.Windows;
using CB.Xaml.AttachedProperties.Impl;


namespace CB.Xaml.AttachedProperties
{
    public class ListBoxServices
    {
        #region Dependency Properties
        public static readonly DependencyProperty BrushesProperty = DependencyProperty
            .RegisterAttached(
                "Brushes", typeof(ListBoxBrushes), typeof(ListBoxServices),
                new PropertyMetadata(default(ListBoxBrushes), OnBrushesChanged));

        private static void OnBrushesChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var targetElement = d as FrameworkElement;
            var brushes = e.NewValue as ListBoxBrushes;
            if (targetElement != null)
            {
                brushes?.ApplyStyle(targetElement);
            }
        }

        public static ListBoxBrushes GetBrushes(DependencyObject element)
        {
            return (ListBoxBrushes)element.GetValue(BrushesProperty);
        }

        public static void SetBrushes(DependencyObject element, ListBoxBrushes value)
        {
            element.SetValue(BrushesProperty, value);
        }
        #endregion
    }
}