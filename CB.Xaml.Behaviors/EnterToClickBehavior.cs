using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;


namespace CB.Xaml.Behaviors
{
    public class EnterToClickBehavior: Behavior<UIElement>
    {
        #region  Fields
        private RoutedEvent _clickEvent;
        private object _clickObject;
        #endregion


        #region  Constructors & Destructors
        public EnterToClickBehavior() { }

        public EnterToClickBehavior(object clickObject)
        {
            ClickObject = clickObject;
        }
        #endregion


        #region  Properties & Indexers
        public object ClickObject
        {
            get { return _clickObject; }
            set
            {
                _clickObject = value;
                _clickEvent = value == null ? null : GetClickEvent(value);
            }
        }
        #endregion


        #region Event Handlers
        private void AssociatedObject_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                RaiseClickEvent();
            }
        }
        #endregion


        #region Implementation
        private void AddHandlers()
        {
            AssociatedObject.PreviewKeyDown += AssociatedObject_PreviewKeyDown;
        }

        private static RoutedEvent GetClickEvent(object _object)
        {
            RoutedEvent clickEvent = null;
            for (var type = _object.GetType(); type != null && clickEvent == null; type = type.BaseType)
            {
                var routedEvents = EventManager.GetRoutedEventsForOwner(type);
                if (routedEvents != null)
                {
                    clickEvent = routedEvents.FirstOrDefault(re => re.Name == "Click");
                }
            }
            return clickEvent;
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            if (_clickObject == null || _clickEvent == null)
            {
                return;
            }
            AddHandlers();
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            if (_clickObject == null || _clickEvent == null)
            {
                return;
            }
            RemoveHandlers();
        }

        private void RaiseClickEvent()
        {
            var eventArgs = new RoutedEventArgs(_clickEvent, _clickObject);
            var uiElement = _clickObject as UIElement;
            if (uiElement != null)
            {
                uiElement.RaiseEvent(eventArgs);
            }
            else
            {
                (_clickObject as ContentElement)?.RaiseEvent(eventArgs);
            }
        }

        private void RemoveHandlers()
        {
            AssociatedObject.PreviewKeyDown -= AssociatedObject_PreviewKeyDown;
        }
        #endregion
    }
}