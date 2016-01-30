using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shell;


namespace CB.Xaml.Behaviors.Impl
{
    public abstract class ResizeElementImpl
    {
        #region Fields & Properties
        protected static readonly double RESIZE_ZONE = 6.0;
        protected static readonly double MINIMUM_DIMENSION = 48.0;
        protected static readonly double SCREEN_HEIGHT;
        protected static readonly double SCREEN_WIDTH;
        protected bool resizing;
        protected double width;
        protected double height;
        protected double left;
        protected double top;
        protected Point mousePosition;
        protected Cursor normalCursor;
        protected ResizeGripDirection resizeDirection = ResizeGripDirection.None;

        protected double maxHeight = SCREEN_HEIGHT;

        public virtual double MaxHeight
        {
            get { return maxHeight; }
            set { maxHeight = Math.Min(value, SCREEN_HEIGHT); }
        }

        protected double maxWidth = SCREEN_WIDTH;

        public virtual double MaxWidth
        {
            get { return maxWidth; }
            set { maxWidth = Math.Min(value, SCREEN_WIDTH); }
        }

        protected double minHeight = MINIMUM_DIMENSION;

        public virtual double MinHeight
        {
            get { return minHeight; }
            set { minHeight = Math.Max(value, MINIMUM_DIMENSION); }
        }

        protected double minWidth = MINIMUM_DIMENSION;

        public virtual double MinWidth
        {
            get { return minWidth; }
            set { minWidth = Math.Max(value, MINIMUM_DIMENSION); }
        }
        #endregion


        #region Constructors
        static ResizeElementImpl()
        {
            SCREEN_HEIGHT = SystemParameters.PrimaryScreenHeight;
            SCREEN_WIDTH = SystemParameters.PrimaryScreenWidth;
        }
        #endregion
    }
}