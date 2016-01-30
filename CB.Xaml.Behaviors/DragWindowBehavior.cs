using System.Windows;
using System.Windows.Interactivity;
using CB.Xaml.Behaviors.Impl;


namespace CB.Xaml.Behaviors
{
    public class DragWindowBehavior : Behavior<FrameworkElement>
    {
        #region Fields & Properties
        private readonly DragWindowImpl<FrameworkElement> dragImpl = new DragWindowImpl<FrameworkElement>();
        private Window ownerWindow;

        public bool IsDoubleClickChangeWindowState
        {
            get { return dragImpl.IsDoubleClickChangeWindowState; }
            set { dragImpl.IsDoubleClickChangeWindowState = value; }
        }
        #endregion


        #region Overridden Methods
        protected override void OnAttached()
        {
            base.OnAttached();
            ownerWindow = Window.GetWindow(AssociatedObject);

            if (ownerWindow == null)
            {
                return;
            }
            dragImpl.Source = AssociatedObject;
            dragImpl.Target = ownerWindow;
            dragImpl.AddHandlers();
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (ownerWindow != null)
            {
                dragImpl.RemoveHandlers();
            }
        }
        #endregion
    }


    /*public class DragWindowBehavior : Behavior<FrameworkElement>
    {
        #region Fields & Properties
        private bool dragging;
        private Point mousePosition;
        private Window ownerWindow;
        private ElementMouseEventHelper eventHelper;
        private readonly double MOUSE_MOVE_SENSITIVITY = 8;

        private bool isDoubleClickChangedWindowState = true;

        public bool IsDoubleClickChangeWindowState
        {
            get { return isDoubleClickChangedWindowState; }
            set { isDoubleClickChangedWindowState = value; }
        }
        #endregion


        #region Overridden Methods
        protected override void OnAttached()
        {
            base.OnAttached();
            ownerWindow = Window.GetWindow(AssociatedObject);

            if (ownerWindow == null)
            {
                return;
            }

            eventHelper = new ElementMouseEventHelper(AssociatedObject);
            SetObjectBackgroundIfNull();
            AddHandlers();
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (ownerWindow != null)
            {
                RemoveHandlers();
            }
        }
        #endregion


        #region Event Handlers
        private void AssociatedObject_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            UpdateMousePosition();
            dragging = true;
            AssociatedObject.CaptureMouse();
        }

        private void AssociatedObject_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            dragging = false;
            AssociatedObject.ReleaseMouseCapture();
        }

        private void AssociatedObject_MouseMove(object sender, MouseEventArgs e)
        {
            if (!dragging)
            {
                return;
            }

            Debug.WriteLine("Dragging");
            var currentCursor = e.GetPosition(AssociatedObject);
            if (ownerWindow.WindowState == WindowState.Normal || IsReturnedToNormalState(currentCursor))
            {
                MoveWindow(currentCursor);
            }
        }

        private void eventHelper_PreviewMouseLeftButtonDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (IsDoubleClickChangeWindowState)
            {
                SwitchWindowState();
                e.Handled = true;
            }
        }
        #endregion


        #region Implementation
        private void AddHandlers()
        {
            AssociatedObject.MouseLeftButtonDown += AssociatedObject_MouseLeftButtonDown;
            AssociatedObject.MouseLeftButtonUp += AssociatedObject_MouseLeftButtonUp;
            AssociatedObject.MouseMove += AssociatedObject_MouseMove;
            eventHelper.PreviewMouseLeftButtonDoubleClick += eventHelper_PreviewMouseLeftButtonDoubleClick;
        }

        private bool IsReturnedToNormalState(Point currentCursor)
        {
            if (ownerWindow.WindowState != WindowState.Maximized)
            {
                return true;
            }

            var horizontalDistance = Math.Abs(currentCursor.X - mousePosition.X);
            var verticalDistance = Math.Abs(currentCursor.Y - mousePosition.Y);

            if (horizontalDistance < MOUSE_MOVE_SENSITIVITY && verticalDistance < MOUSE_MOVE_SENSITIVITY)
            {
                return false;
            }

            ReturnToNormalState();
            Debug.WriteLine("ReturnToNormalState: {0}, {1}", horizontalDistance, verticalDistance);
            return true;
        }

        private void MoveWindow(Point currentCursor)
        {
            ownerWindow.Left += (currentCursor.X - mousePosition.X);
            ownerWindow.Top += (currentCursor.Y - mousePosition.Y);
        }

        private void RemoveHandlers()
        {
            AssociatedObject.MouseLeftButtonDown -= AssociatedObject_MouseLeftButtonDown;
            AssociatedObject.MouseLeftButtonUp -= AssociatedObject_MouseLeftButtonUp;
            AssociatedObject.MouseMove -= AssociatedObject_MouseMove;
            eventHelper.PreviewMouseLeftButtonDoubleClick -= eventHelper_PreviewMouseLeftButtonDoubleClick;
        }

        private void ReturnToNormalState()
        {
            double prevWidth = AssociatedObject.ActualWidth, prevHeight = AssociatedObject.ActualHeight;
            double widthRatio = mousePosition.X/prevWidth, heightRatio = mousePosition.Y/prevHeight;

            ownerWindow.WindowState = WindowState.Normal;

            double currWidth = AssociatedObject.ActualWidth, currHeight = AssociatedObject.ActualHeight;
            mousePosition = new Point(currWidth*widthRatio, currHeight*heightRatio);
        }

        private void SetObjectBackgroundIfNull()
        {
            var background = AssociatedObject.GetValue(Panel.BackgroundProperty) as Brush;
            if (background == null)
            {
                AssociatedObject.SetValue(Panel.BackgroundProperty, Brushes.Transparent);
            }
        }

        private void SwitchWindowState()
        {
            switch (ownerWindow.WindowState)
            {
                case WindowState.Maximized:
                    ownerWindow.WindowState = WindowState.Normal;
                    break;
                case WindowState.Normal:
                    ownerWindow.WindowState = WindowState.Maximized;
                    break;
            }
            Debug.WriteLine("SwitchWindowState: " + ownerWindow.WindowState);
            Debug.WriteLine(ownerWindow.Left + " " + ownerWindow.Top);
        }

        private void UpdateMousePosition()
        {
            mousePosition = Mouse.GetPosition(AssociatedObject);
            Debug.WriteLine("UpdateMousePosition: {0}", mousePosition);
        }
        #endregion
    }*/
}