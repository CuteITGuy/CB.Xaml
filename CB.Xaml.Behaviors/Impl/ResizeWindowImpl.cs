using System.Windows;


namespace CB.Xaml.Behaviors.Impl
{
    public class ResizeWindowImpl : ResizeElementImpl<Window>
    {
        #region Overridden Methods
        protected override double GetTop()
        {
            return element.Top;
        }

        protected override double GetLeft()
        {
            return element.Left;
        }

        protected override bool IsValidState()
        {
            return element.WindowState != WindowState.Maximized;
        }

        protected override void SetLeft(double value)
        {
            element.Left = value;
        }

        protected override void SetTop(double value)
        {
            element.Top = value;
        }
        #endregion
    }
}