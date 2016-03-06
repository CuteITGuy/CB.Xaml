using System.ComponentModel;
using System.Windows;
using CB.Xaml.AttachedProperties.Impl;


namespace CB.Xaml.AttachedProperties
{
    public class CommandServices: DependencyObject
    {
        #region Dependency Properties
        public static readonly DependencyProperty AttachedCommandProperty = DependencyProperty.RegisterAttached(
            "AttachedCommand", typeof(AttachedCommand), typeof(CommandServices),
            new PropertyMetadata(default(AttachedCommand), OnAttachedCommandChanged));

        [Category("CommandServices")]
        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static AttachedCommand GetAttachedCommand(DependencyObject d)
        {
            return (AttachedCommand)d.GetValue(AttachedCommandProperty);
        }

        [Category("CommandServices")]
        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static void SetAttachedCommand(DependencyObject d, AttachedCommand value)
        {
            d.SetValue(AttachedCommandProperty, value);
        }
        #endregion


        #region Implementation
        private static void OnAttachedCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as FrameworkElement;
            if (element == null) return;
            (e.OldValue as AttachedCommand)?.Detach(element);
            (e.NewValue as AttachedCommand)?.Attach(element);
        }
        #endregion
    }
}