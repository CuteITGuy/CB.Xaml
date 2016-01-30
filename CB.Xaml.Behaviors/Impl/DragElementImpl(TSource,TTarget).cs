using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


namespace CB.Xaml.Behaviors.Impl
{
    public abstract class DragElementImpl<TSource, TTarget> : DragElementImpl where TSource : FrameworkElement
        where TTarget : FrameworkElement
    {
        #region Fields & Properties
        public virtual TTarget Target { get; set; }

        protected TSource source;

        public virtual TSource Source
        {
            get { return source; }
            set
            {
                source = value;
                SetObjectBackgroundIfNull();
            }
        }
        #endregion


        #region Methods
        public virtual void AddHandlers()
        {
            Source.MouseLeftButtonDown += Source_MouseLeftButtonDown;
            Source.MouseLeftButtonUp += Source_MouseLeftButtonUp;
            Source.MouseMove += Source_MouseMove;
        }

        public virtual void RemoveHandlers()
        {
            Source.MouseLeftButtonDown -= Source_MouseLeftButtonDown;
            Source.MouseLeftButtonUp -= Source_MouseLeftButtonUp;
            Source.MouseMove -= Source_MouseMove;
        }
        #endregion


        #region Abstract Methods
        protected abstract double GetLeft();
        protected abstract double GetTop();
        protected abstract bool IsValidState();
        protected abstract void SetLeft(double value);
        protected abstract void SetTop(double value);
        #endregion


        #region Event Handlers
        protected void Source_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            UpdateMousePosition();
            dragging = true;
            Source.CaptureMouse();
        }

        protected void Source_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            dragging = false;
            Source.ReleaseMouseCapture();
        }

        protected void Source_MouseMove(object sender, MouseEventArgs e)
        {
            if (!dragging)
            {
                return;
            }

            var currentCursor = e.GetPosition(Source);

            if (IsValidState())
            {
                MoveWindow(currentCursor);
            }
        }
        #endregion


        #region Implementation
        protected void MoveWindow(Point currentCursor)
        {
            var left = GetLeft();
            SetLeft(left + currentCursor.X - mousePosition.X);
            var top = GetTop();

            SetTop(top + currentCursor.Y - mousePosition.Y);
        }

        protected void SetObjectBackgroundIfNull()
        {
            var background = Source.GetValue(Panel.BackgroundProperty) as Brush;
            if (background == null)
            {
                Source.SetValue(Panel.BackgroundProperty, Brushes.Transparent);
            }
        }

        protected void UpdateMousePosition()
        {
            mousePosition = Mouse.GetPosition(Source);
        }
        #endregion
    }
}