using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shell;
using Cursor = CB.Interop.DesktopAppUI.Cursor;


namespace CB.Xaml.Behaviors.Impl
{
    public abstract class ResizeElementImpl<TElement> : ResizeElementImpl where TElement : FrameworkElement
    {
        #region Fields & Properties
        private ElementMouseEventHelper mouseEventHelper;

        protected TElement element;

        public virtual TElement Element
        {
            get { return element; }
            set
            {
                element = value;
                normalCursor = element.Cursor;
                mouseEventHelper = new ElementMouseEventHelper(element);
            }
        }
        #endregion


        #region Methods
        public void AddHandlers()
        {
            Element.PreviewMouseLeftButtonDown += AssociatedObject_PreviewMouseLeftButtonDown;
            Element.PreviewMouseLeftButtonUp += AssociatedObject_PreviewMouseLeftButtonUp;
            Element.PreviewMouseMove += AssociatedObject_PreviewMouseMove;
            mouseEventHelper.PreviewMouseLeftButtonDoubleClick += mouseEventHelper_PreviewMouseLeftButtonDoubleClick;
        }

        public void RemoveHandlers()
        {
            Element.PreviewMouseLeftButtonDown -= AssociatedObject_PreviewMouseLeftButtonDown;
            Element.PreviewMouseLeftButtonUp -= AssociatedObject_PreviewMouseLeftButtonUp;
            Element.PreviewMouseMove -= AssociatedObject_PreviewMouseMove;
            mouseEventHelper.PreviewMouseLeftButtonDoubleClick -= mouseEventHelper_PreviewMouseLeftButtonDoubleClick;
        }
        #endregion


        #region Abstract Methods
        protected abstract double GetTop();

        protected abstract double GetLeft();

        protected abstract bool IsValidState();

        protected abstract void SetLeft(double value);

        protected abstract void SetTop(double value);
        #endregion


        #region Event Handlers
        protected void AssociatedObject_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (resizeDirection != ResizeGripDirection.None)
            {
                resizing = true;
                SaveState();
                e.Handled = true;
                Element.CaptureMouse();
            }
        }

        protected void AssociatedObject_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Element.ReleaseMouseCapture();
            resizing = false;
        }

        protected void AssociatedObject_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (!IsValidState())
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

        private void mouseEventHelper_PreviewMouseLeftButtonDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ExpandWindow();
        }
        #endregion


        #region Implementation
        protected static double CalculateActualDistance(double desiredDistance, double minDistance, double maxDistance)
        {
            return desiredDistance < minDistance
                ? minDistance
                : desiredDistance > maxDistance
                    ? maxDistance
                    : desiredDistance;
        }

        protected double CalculateActualHorizontalDistance(double desiredDistance)
        {
            double minDistance = minWidth - width, maxDistance = maxWidth - width;
            return CalculateActualDistance(desiredDistance, minDistance, maxDistance);
        }

        protected double CalculateActualVerticalDistance(double desiredDistance)
        {
            double minDistance = minHeight - height, maxDistance = maxHeight - height;
            return CalculateActualDistance(desiredDistance, minDistance, maxDistance);
        }

        protected void CheckResizePosition()
        {
            var currentPosition = GetCurrentMousePosition();
            double currentLeft = GetLeft(), currentTop = GetTop();

            bool leftResize = currentPosition.X - currentLeft < RESIZE_ZONE,
                rightResize = Element.Width + currentLeft - currentPosition.X < RESIZE_ZONE,
                topResize = currentPosition.Y - currentTop < RESIZE_ZONE,
                bottomResize = Element.Height + currentTop - currentPosition.Y < RESIZE_ZONE;

            if (!(leftResize || rightResize || topResize || bottomResize))
            {
                Element.Cursor = normalCursor;
                resizeDirection = ResizeGripDirection.None;
                return;
            }

            if (leftResize && topResize)
            {
                Element.Cursor = Cursors.SizeNWSE;
                resizeDirection = ResizeGripDirection.TopLeft;
            }
            else if (rightResize && topResize)
            {
                Element.Cursor = Cursors.SizeNESW;
                resizeDirection = ResizeGripDirection.TopRight;
            }
            else if (leftResize && bottomResize)
            {
                Element.Cursor = Cursors.SizeNESW;
                resizeDirection = ResizeGripDirection.BottomLeft;
            }
            else if (rightResize && bottomResize)
            {
                Element.Cursor = Cursors.SizeNWSE;
                resizeDirection = ResizeGripDirection.BottomRight;
            }
            else if (leftResize)
            {
                Element.Cursor = Cursors.SizeWE;
                resizeDirection = ResizeGripDirection.Left;
            }
            else if (rightResize)
            {
                Element.Cursor = Cursors.SizeWE;
                resizeDirection = ResizeGripDirection.Right;
            }
            else if (topResize)
            {
                Element.Cursor = Cursors.SizeNS;
                resizeDirection = ResizeGripDirection.Top;
            }
            else // bottomResize
            {
                Element.Cursor = Cursors.SizeNS;
                resizeDirection = ResizeGripDirection.Bottom;
            }
        }

        protected void ExpandBottom()
        {
            ResizeBottom(SCREEN_HEIGHT - Element.Height - GetTop());
        }

        protected void ExpandLeft()
        {
            ResizeLeft(GetLeft());
        }

        protected void ExpandRight()
        {
            ResizeRight(SCREEN_WIDTH - Element.Width - GetLeft());
        }

        protected void ExpandTop()
        {
            ResizeTop(GetTop());
        }

        protected void ExpandWindow()
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

        protected static Point GetCurrentMousePosition()
        {
            var currentMouse = Cursor.GetCursorPos();
            return new Point(currentMouse.X, currentMouse.Y);
        }

        protected void Resize()
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

        protected void ResizeBottom(Point newMousePosition)
        {
            var distance = CalculateActualVerticalDistance(newMousePosition.Y - mousePosition.Y);
            ResizeBottom(distance);
        }

        protected void ResizeBottom(double distance)
        {
            Element.Height = height + distance;
        }

        protected void ResizeBottomLeft(Point newMousePosition)
        {
            ResizeLeft(newMousePosition);
            ResizeBottom(newMousePosition);
        }

        protected void ResizeBottomRight(Point newMousePosition)
        {
            ResizeRight(newMousePosition);
            ResizeBottom(newMousePosition);
        }

        protected void ResizeLeft(Point newMousePosition)
        {
            var distance = CalculateActualHorizontalDistance(mousePosition.X - newMousePosition.X);
            ResizeLeft(distance);
        }

        protected void ResizeLeft(double distance)
        {
            Element.Width = width + distance;
            SetLeft(left - distance);
        }

        protected void ResizeRight(Point newMousePosition)
        {
            var distance = CalculateActualHorizontalDistance(newMousePosition.X - mousePosition.X);
            ResizeRight(distance);
        }

        protected void ResizeRight(double distance)
        {
            Element.Width = width + distance;
        }

        protected void ResizeTop(Point newMousePosition)
        {
            var distance = CalculateActualVerticalDistance(mousePosition.Y - newMousePosition.Y);
            ResizeTop(distance);
        }

        protected void ResizeTop(double distance)
        {
            Element.Height = height + distance;
            SetTop(top - distance);
        }

        protected void ResizeTopLeft(Point newMousePosition)
        {
            ResizeLeft(newMousePosition);
            ResizeTop(newMousePosition);
        }

        protected void ResizeTopRight(Point newMousePosition)
        {
            ResizeRight(newMousePosition);
            ResizeTop(newMousePosition);
        }

        protected void SaveState()
        {
            mousePosition = GetCurrentMousePosition();
            width = Element.ActualWidth;
            height = Element.ActualHeight;
            left = GetLeft();
            top = GetTop();

            Debug.WriteLine("mousePosition: {0}, width: {1}, height: {2}, left: {3}, top: {4}", mousePosition, width,
                height, left, top);
        }
        #endregion
    }
}