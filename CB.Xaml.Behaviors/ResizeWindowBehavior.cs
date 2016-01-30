using System.Windows;
using System.Windows.Interactivity;
using CB.Xaml.Behaviors.Impl;


namespace CB.Xaml.Behaviors
{
    public class ResizeWindowBehavior : Behavior<Window>
    {
        #region Fields & Properties
        private readonly ResizeWindowImpl resizeImpl = new ResizeWindowImpl();

        public double MaxHeight
        {
            get { return resizeImpl.MaxHeight; }
            set { resizeImpl.MaxHeight = value; }
        }

        public double MaxWidth
        {
            get { return resizeImpl.MaxWidth; }
            set { resizeImpl.MinWidth = value; }
        }

        public double MinHeight
        {
            get { return resizeImpl.MinHeight; }
            set { resizeImpl.MinHeight = value; }
        }

        public double MinWidth
        {
            get { return resizeImpl.MinWidth; }
            set { resizeImpl.MinWidth = value; }
        }
        #endregion


        #region Overridden Methods
        protected override void OnAttached()
        {
            base.OnAttached();
            resizeImpl.Element = AssociatedObject;
            resizeImpl.AddHandlers();
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            resizeImpl.RemoveHandlers();
        }
        #endregion
    }


    /*public class ResizeWindowBehavior : Behavior<Window>
    {
        #region Fields & Properties
        private static readonly double RESIZE_ZONE = 6.0;
        private static readonly double MINIMUM_DIMENSION = 48.0;
        private static readonly double SCREEN_HEIGHT;
        private static readonly double SCREEN_WIDTH;
        private bool resizing;
        private double width;
        private double height;
        private double left;
        private double top;
        private Point mousePosition;
        private Cursor normalCursor;
        private ResizeGripDirection resizeDirection = ResizeGripDirection.None;

        private double maxHeight = SCREEN_HEIGHT;

        public double MaxHeight
        {
            get { return maxHeight; }
            set { maxHeight = Math.Min(value, SCREEN_HEIGHT); }
        }

        private double maxWidth = SCREEN_WIDTH;

        public double MaxWidth
        {
            get { return maxWidth; }
            set { maxWidth = Math.Min(value, SCREEN_WIDTH); }
        }

        private double minHeight = MINIMUM_DIMENSION;

        public double MinHeight
        {
            get { return minHeight; }
            set { minHeight = Math.Max(value, MINIMUM_DIMENSION); }
        }

        private double minWidth = MINIMUM_DIMENSION;

        public double MinWidth
        {
            get { return minWidth; }
            set { minWidth = Math.Max(value, MINIMUM_DIMENSION); }
        }
        #endregion


        #region Constructors
        static ResizeWindowBehavior()
        {
            SCREEN_HEIGHT = SystemParameters.PrimaryScreenHeight;
            SCREEN_WIDTH = SystemParameters.PrimaryScreenWidth;
        }
        #endregion


        #region Overridden Methods
        protected override void OnAttached()
        {
            base.OnAttached();
            normalCursor = AssociatedObject.Cursor;
            AddHandlers();
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            RemoveHandlers();
        }
        #endregion


        #region Event Handlers
        private void AssociatedObject_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            switch (e.ChangedButton)
            {
                case MouseButton.Left:
                    ExpandWindow();
                    break;
                case MouseButton.Middle:
                    break;
                case MouseButton.Right:
                    break;
                case MouseButton.XButton1:
                    break;
                case MouseButton.XButton2:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void AssociatedObject_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (resizeDirection != ResizeGripDirection.None)
            {
                resizing = true;
                SaveState();
                e.Handled = true;
                AssociatedObject.CaptureMouse();
            }
        }

        private void AssociatedObject_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            AssociatedObject.ReleaseMouseCapture();
            resizing = false;
        }

        private void AssociatedObject_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (AssociatedObject.WindowState != WindowState.Normal)
            {
                return;
            }

            if (resizing)
            {
                Resize();
                e.Handled = true;
            }
            else
            {
                CheckResizePosition();
            }
        }
        #endregion


        #region Implementation
        private void AddHandlers()
        {
            AssociatedObject.PreviewMouseDoubleClick += AssociatedObject_PreviewMouseDoubleClick;
            AssociatedObject.PreviewMouseLeftButtonDown += AssociatedObject_PreviewMouseLeftButtonDown;
            AssociatedObject.PreviewMouseLeftButtonUp += AssociatedObject_PreviewMouseLeftButtonUp;
            AssociatedObject.PreviewMouseMove += AssociatedObject_PreviewMouseMove;
        }

        private static double CalculateActualDistance(double desiredDistance, double minDistance, double maxDistance)
        {
            return desiredDistance < minDistance
                ? minDistance
                : desiredDistance > maxDistance
                    ? maxDistance
                    : desiredDistance;
        }

        private double CalculateActualHorizontalDistance(double desiredDistance)
        {
            double minDistance = minWidth - width, maxDistance = maxWidth - width;
            return CalculateActualDistance(desiredDistance, minDistance, maxDistance);
        }

        private double CalculateActualVerticalDistance(double desiredDistance)
        {
            double minDistance = minHeight - height, maxDistance = maxHeight - height;
            return CalculateActualDistance(desiredDistance, minDistance, maxDistance);
        }

        private void CheckResizePosition()
        {
            var currentPosition = GetCurrentMousePosition();
            double currentLeft = AssociatedObject.Left, currentTop = AssociatedObject.Top;

            bool leftResize = currentPosition.X - currentLeft < RESIZE_ZONE,
                rightResize = AssociatedObject.Width + currentLeft - currentPosition.X < RESIZE_ZONE,
                topResize = currentPosition.Y - currentTop < RESIZE_ZONE,
                bottomResize = AssociatedObject.Height + currentTop - currentPosition.Y < RESIZE_ZONE;

            if (!(leftResize || rightResize || topResize || bottomResize))
            {
                AssociatedObject.Cursor = normalCursor;
                resizeDirection = ResizeGripDirection.None;
                return;
            }

            if (leftResize && topResize)
            {
                AssociatedObject.Cursor = Cursors.SizeNWSE;
                resizeDirection = ResizeGripDirection.TopLeft;
            }
            else if (rightResize && topResize)
            {
                AssociatedObject.Cursor = Cursors.SizeNESW;
                resizeDirection = ResizeGripDirection.TopRight;
            }
            else if (leftResize && bottomResize)
            {
                AssociatedObject.Cursor = Cursors.SizeNESW;
                resizeDirection = ResizeGripDirection.BottomLeft;
            }
            else if (rightResize && bottomResize)
            {
                AssociatedObject.Cursor = Cursors.SizeNWSE;
                resizeDirection = ResizeGripDirection.BottomRight;
            }
            else if (leftResize)
            {
                AssociatedObject.Cursor = Cursors.SizeWE;
                resizeDirection = ResizeGripDirection.Left;
            }
            else if (rightResize)
            {
                AssociatedObject.Cursor = Cursors.SizeWE;
                resizeDirection = ResizeGripDirection.Right;
            }
            else if (topResize)
            {
                AssociatedObject.Cursor = Cursors.SizeNS;
                resizeDirection = ResizeGripDirection.Top;
            }
            else // bottomResize
            {
                AssociatedObject.Cursor = Cursors.SizeNS;
                resizeDirection = ResizeGripDirection.Bottom;
            }
        }

        private void ExpandBottom()
        {
            ResizeBottom(SCREEN_HEIGHT - AssociatedObject.Height - AssociatedObject.Top);
        }

        private void ExpandLeft()
        {
            ResizeLeft(AssociatedObject.Left);
        }

        private void ExpandRight()
        {
            ResizeRight(SCREEN_WIDTH - AssociatedObject.Width - AssociatedObject.Left);
        }

        private void ExpandTop()
        {
            ResizeTop(AssociatedObject.Top);
        }

        private void ExpandWindow()
        {
            switch (resizeDirection)
            {
                case ResizeGripDirection.None:
                    return;

                case ResizeGripDirection.Top:
                    ExpandTop();
                    break;

                case ResizeGripDirection.Left:
                    ExpandLeft();
                    break;

                case ResizeGripDirection.TopLeft:
                    ExpandTop();
                    ExpandLeft();
                    break;

                case ResizeGripDirection.TopRight:
                    ExpandTop();
                    ExpandRight();
                    break;

                case ResizeGripDirection.Bottom:
                    ExpandBottom();
                    break;

                case ResizeGripDirection.Right:
                    ExpandRight();
                    break;

                case ResizeGripDirection.BottomRight:
                    ExpandBottom();
                    ExpandRight();
                    break;

                case ResizeGripDirection.BottomLeft:
                    ExpandBottom();
                    ExpandLeft();
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static Point GetCurrentMousePosition()
        {
            var currentMouse = Interop.DesktopAppUI.Cursor.GetCursorPos();
            return new Point(currentMouse.X, currentMouse.Y);
        }

        private void RemoveHandlers()
        {
            AssociatedObject.PreviewMouseDoubleClick -= AssociatedObject_PreviewMouseDoubleClick;
            AssociatedObject.PreviewMouseLeftButtonDown -= AssociatedObject_PreviewMouseLeftButtonDown;
            AssociatedObject.PreviewMouseLeftButtonUp -= AssociatedObject_PreviewMouseLeftButtonUp;
            AssociatedObject.PreviewMouseMove -= AssociatedObject_PreviewMouseMove;
        }

        private void Resize()
        {
            var currentMousePos = GetCurrentMousePosition();

            switch (resizeDirection)
            {
                case ResizeGripDirection.None:
                    break;

                case ResizeGripDirection.TopLeft:
                    ResizeTopLeft(currentMousePos);
                    break;

                case ResizeGripDirection.Top:
                    ResizeTop(currentMousePos);
                    break;

                case ResizeGripDirection.TopRight:
                    ResizeTopRight(currentMousePos);
                    break;

                case ResizeGripDirection.Right:
                    ResizeRight(currentMousePos);
                    break;

                case ResizeGripDirection.BottomRight:
                    ResizeBottomRight(currentMousePos);
                    break;

                case ResizeGripDirection.Bottom:
                    ResizeBottom(currentMousePos);
                    break;

                case ResizeGripDirection.BottomLeft:
                    ResizeBottomLeft(currentMousePos);
                    break;

                case ResizeGripDirection.Left:
                    ResizeLeft(currentMousePos);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ResizeBottom(Point newMousePosition)
        {
            var distance = CalculateActualVerticalDistance(newMousePosition.Y - mousePosition.Y);
            ResizeBottom(distance);
        }

        private void ResizeBottom(double distance)
        {
            AssociatedObject.Height = height + distance;
        }

        private void ResizeBottomLeft(Point newMousePosition)
        {
            ResizeLeft(newMousePosition);
            ResizeBottom(newMousePosition);
        }

        private void ResizeBottomRight(Point newMousePosition)
        {
            ResizeRight(newMousePosition);
            ResizeBottom(newMousePosition);
        }

        private void ResizeLeft(Point newMousePosition)
        {
            var distance = CalculateActualHorizontalDistance(mousePosition.X - newMousePosition.X);
            ResizeLeft(distance);
        }

        private void ResizeLeft(double distance)
        {
            AssociatedObject.Width = width + distance;
            AssociatedObject.Left = left - distance;
        }

        private void ResizeRight(Point newMousePosition)
        {
            var distance = CalculateActualHorizontalDistance(newMousePosition.X - mousePosition.X);
            ResizeRight(distance);
        }

        private void ResizeRight(double distance)
        {
            AssociatedObject.Width = width + distance;
        }

        private void ResizeTop(Point newMousePosition)
        {
            var distance = CalculateActualVerticalDistance(mousePosition.Y - newMousePosition.Y);
            ResizeTop(distance);
        }

        private void ResizeTop(double distance)
        {
            AssociatedObject.Height = height + distance;
            AssociatedObject.Top = top - distance;
        }

        private void ResizeTopLeft(Point newMousePosition)
        {
            ResizeLeft(newMousePosition);
            ResizeTop(newMousePosition);
        }

        private void ResizeTopRight(Point newMousePosition)
        {
            ResizeRight(newMousePosition);
            ResizeTop(newMousePosition);
        }

        private void SaveState()
        {
            mousePosition = GetCurrentMousePosition();
            width = AssociatedObject.ActualWidth;
            height = AssociatedObject.ActualHeight;
            left = AssociatedObject.Left;
            top = AssociatedObject.Top;
        }
        #endregion
    }*/
}