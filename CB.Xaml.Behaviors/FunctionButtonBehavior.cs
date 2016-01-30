using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;
using CB.Xaml.Behaviors.Impl;


namespace CB.Xaml.Behaviors
{
    public class FunctionButtonBehavior : Behavior<ButtonBase>
    {
        #region Fields & Properties
        private Window ownerWindow;

        private ButtonFunction function = ButtonFunction.Close;

        public ButtonFunction Function
        {
            get { return function; }
            set { function = value; }
        }
        #endregion


        #region Overridden Methods
        protected override void OnAttached()
        {
            base.OnAttached();
            ownerWindow = Window.GetWindow(AssociatedObject);

            if (ownerWindow != null)
            {
                AssociatedObject.Click += AssociatedObject_Click;
                AddOwnerWindowHandlers();
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (ownerWindow != null)
            {
                AssociatedObject.Click -= AssociatedObject_Click;
                RemoveOwnerWindowHandlers();
            }
        }
        #endregion


        #region Event Handlers
        private void AssociatedObject_Click(object sender, RoutedEventArgs e)
        {
            switch (function)
            {
                case ButtonFunction.Close:
                    ownerWindow.Close();
                    break;

                case ButtonFunction.Minimize:
                    ownerWindow.WindowState = WindowState.Minimized;
                    break;

                case ButtonFunction.Maximize:
                    ownerWindow.WindowState = WindowState.Maximized;
                    break;

                case ButtonFunction.Restore:
                    ownerWindow.WindowState = WindowState.Normal;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OwnerWindowRestor_StateChanged(object sender, EventArgs eventArgs)
        {
            SetRestoreButtonVisibility();
        }

        private void OwnerWindowMaximize_StateChanged(object sender, EventArgs eventArgs)
        {
            SetMaximizeButtonVisibility();
        }
        #endregion


        #region Implementation
        private void AddOwnerWindowHandlers()
        {
            switch (function)
            {
                case ButtonFunction.Maximize:
                    SetMaximizeButtonVisibility();
                    ownerWindow.StateChanged += OwnerWindowMaximize_StateChanged;
                    break;

                case ButtonFunction.Restore:
                    SetRestoreButtonVisibility();
                    ownerWindow.StateChanged += OwnerWindowRestor_StateChanged;
                    break;
            }
        }

        private void RemoveOwnerWindowHandlers()
        {
            switch (function)
            {
                case ButtonFunction.Maximize:
                    ownerWindow.StateChanged -= OwnerWindowMaximize_StateChanged;
                    break;

                case ButtonFunction.Restore:
                    ownerWindow.StateChanged -= OwnerWindowRestor_StateChanged;
                    break;
            }
        }

        private void SetMaximizeButtonVisibility()
        {
            AssociatedObject.Visibility = ownerWindow.WindowState == WindowState.Maximized
                ? Visibility.Collapsed
                : Visibility.Visible;
        }

        private void SetRestoreButtonVisibility()
        {
            AssociatedObject.Visibility = ownerWindow.WindowState == WindowState.Normal
                ? Visibility.Collapsed
                : Visibility.Visible;
        }
        #endregion
    }
}