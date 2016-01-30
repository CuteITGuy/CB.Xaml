using System;
using System.Windows;
using System.Windows.Input;


namespace CB.Xaml.Behaviors.Impl
{
    public class ElementMouseEventHelper: IDisposable
    {
        #region Fields
        private readonly UIElement _element;

        /*private static readonly TimeSpan MAX_CLICK_INTERVAL =
            TimeSpan.FromMilliseconds(SystemInformation.DoubleClickTime);

        private DateTime lastClick, lastPreviewClick;*/

        private bool _disposed;
        #endregion


        #region  Constructors & Destructors
        public ElementMouseEventHelper(UIElement element)
        {
            _element = element;
            element.MouseLeftButtonDown += element_MouseLeftButtonDown;
            element.PreviewMouseLeftButtonDown += element_PreviewMouseLeftButtonDown;
        }

        ~ElementMouseEventHelper()
        {
            Dispose();
        }
        #endregion


        #region Events
        public event MouseButtonEventHandler MouseLeftButtonDoubleClick;
        public event MouseButtonEventHandler PreviewMouseLeftButtonDoubleClick;
        #endregion


        #region Methods
        public void Dispose()
        {
            if (!_disposed)
            {
                _element.MouseLeftButtonDown -= element_MouseLeftButtonDown;
                _element.PreviewMouseLeftButtonDown -= element_PreviewMouseLeftButtonDown;
                _disposed = true;
            }
        }
        #endregion


        #region Event Handlers
        private void element_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            /*if (!IsADoubleClick(lastClick))
            {
                lastClick = DateTime.Now;
                return;
            }

            MouseLeftButtonDoubleClick?.Invoke(sender, routedEvent);*/

            HandleDoubleClick(sender, e);
        }

        private void element_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            /*if (!IsADoubleClick(lastPreviewClick))
            {
                lastPreviewClick = DateTime.Now;
                return;
            }

            if (PreviewMouseLeftButtonDoubleClick != null)
            {
                PreviewMouseLeftButtonDoubleClick(sender, routedEvent);
            }*/

            HandleDoubleClick(sender, e);
        }

        private void HandleDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount != 2) return;

            var args = CreateMouseButtonEventArgs(e);
            InvokeEventHandler(sender, e.RoutedEvent, args);
            if (args.Handled)
            {
                e.Handled = true;
            }
        }
        #endregion


        #region Implementation
        private static MouseButtonEventArgs CreateMouseButtonEventArgs(MouseButtonEventArgs e)
        {
            var args = new MouseButtonEventArgs(
                e.MouseDevice, e.Timestamp, e.ChangedButton, e.StylusDevice)
            {
                Source = e.OriginalSource
            };
            return args;
        }

        private void InvokeEventHandler(
            object sender, RoutedEvent routedEvent, MouseButtonEventArgs args)
        {
            if (routedEvent == UIElement.PreviewMouseLeftButtonDownEvent ||
                routedEvent == UIElement.PreviewMouseRightButtonDownEvent)
            {
                PreviewMouseLeftButtonDoubleClick?.Invoke(sender, args);
            }
            else
            {
                MouseLeftButtonDoubleClick?.Invoke(sender, args);
            }
        }
        #endregion


        /*private static bool IsADoubleClick(DateTime lastClickTime)
        {
            return DateTime.Now - lastClickTime < MAX_CLICK_INTERVAL;
        }*/
    }
}