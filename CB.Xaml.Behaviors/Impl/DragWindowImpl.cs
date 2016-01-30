using System;
using System.Windows;
using System.Windows.Input;


namespace CB.Xaml.Behaviors.Impl
{
    public class DragWindowImpl<TSource> : DragElementImpl<TSource, Window> where TSource : FrameworkElement
    {
        #region Fields & Properties
        protected ElementMouseEventHelper eventHelper;
        protected bool isDoubleClickChangedWindowState = true;

        public bool IsDoubleClickChangeWindowState
        {
            get { return isDoubleClickChangedWindowState; }
            set { isDoubleClickChangedWindowState = value; }
        }

        public override TSource Source
        {
            get { return base.Source; }
            set
            {
                base.Source = value;
                eventHelper = new ElementMouseEventHelper(Source);
            }
        }
        #endregion


        #region Event Handlers
        protected void eventHelper_PreviewMouseLeftButtonDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (IsDoubleClickChangeWindowState)
            {
                SwitchWindowState();
                e.Handled = true;
            }
        }
        #endregion


        #region Overridden Methods
        public override void AddHandlers()
        {
            base.AddHandlers();
            eventHelper.PreviewMouseLeftButtonDoubleClick += eventHelper_PreviewMouseLeftButtonDoubleClick;
        }

        public override void RemoveHandlers()
        {
            base.RemoveHandlers();
            eventHelper.PreviewMouseLeftButtonDoubleClick -= eventHelper_PreviewMouseLeftButtonDoubleClick;
        }

        protected override double GetLeft()
        {
            return Target.Left;
        }

        protected override double GetTop()
        {
            return Target.Top;
        }

        protected override bool IsValidState()
        {
            return Target.WindowState == WindowState.Normal ||
                   (Target.WindowState == WindowState.Maximized && TryReturnToNormalState());
        }

        protected override void SetLeft(double value)
        {
            Target.Left = value;
        }

        protected override void SetTop(double value)
        {
            Target.Top = value;
        }
        #endregion


        #region Implementation
        protected void ReturnToNormalState()
        {
            double prevWidth = Source.ActualWidth, prevHeight = Source.ActualHeight;
            double widthRatio = mousePosition.X/prevWidth, heightRatio = mousePosition.Y/prevHeight;

            Target.WindowState = WindowState.Normal;

            double currWidth = Source.ActualWidth, currHeight = Source.ActualHeight;
            mousePosition = new Point(currWidth*widthRatio, currHeight*heightRatio);
        }

        private bool TryReturnToNormalState()
        {
            var currentCursor = Mouse.GetPosition(Source);

            var horizontalDistance = Math.Abs(currentCursor.X - mousePosition.X);
            var verticalDistance = Math.Abs(currentCursor.Y - mousePosition.Y);

            if (horizontalDistance < MOUSE_MOVE_SENSITIVITY && verticalDistance < MOUSE_MOVE_SENSITIVITY)
            {
                return false;
            }

            ReturnToNormalState();
            return true;
        }

        protected void SwitchWindowState()
        {
            switch (Target.WindowState)
            {
                case WindowState.Maximized:
                    Target.WindowState = WindowState.Normal;
                    break;
                case WindowState.Normal:
                    Target.WindowState = WindowState.Maximized;
                    break;
            }
        }
        #endregion
    }
}