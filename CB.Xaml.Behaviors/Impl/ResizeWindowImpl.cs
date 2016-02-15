using System.Windows;


namespace CB.Xaml.Behaviors.Impl
{
    public class ResizeWindowImpl : ResizeElementImpl<Window>
    {
        #region Overridden Methods
        protected override double GetTop()
        {
            return _element.Top;
        }

        protected override double GetLeft()
        {
            return _element.Left;
        }

        protected override bool IsValidState()
        {
            return _element.WindowState != WindowState.Maximized;
        }

        protected override void SetLeft(double value)
        {
            _element.Left = value;
        }

        protected override void SetTop(double value)
        {
            _element.Top = value;
        }
        #endregion
    }
}