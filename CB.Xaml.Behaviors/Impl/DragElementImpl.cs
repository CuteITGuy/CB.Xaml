using System.Windows;


namespace CB.Xaml.Behaviors.Impl
{
    public abstract class DragElementImpl
    {
        #region Fields & Properties
        protected bool dragging;
        protected Point mousePosition;
        protected readonly double MOUSE_MOVE_SENSITIVITY = 8;
        #endregion
    }
}