using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;


namespace CB.Xaml.Behaviors.KeyPressBehaviors
{
    public class EnterToClickBehaviorBase<TElement>: Behavior<TElement> where TElement: UIElement
    {
        private RoutedEvent _clickEvent;
        private DependencyObject _target;

        // Target may be of type Button (inherits UIElement) or Hyperlink (inherite ContentElement)
        // So, using the base class DependencyObject here
        public DependencyObject Target
        {
            get { return _target; }
            set
            {
                _target = value;
                _clickEvent = value == null ? null : GetClickEvent(value);
            }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            if (Target == null || _clickEvent == null)
            {
                return;
            }
            AddHandlers();
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            if (Target == null || _clickEvent == null)
            {
                return;
            }
            RemoveHandlers();
        }

        private void AssociatedObject_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                RaiseClickEvent();
            }
        }

        private void AddHandlers()
        {
            AssociatedObject.PreviewKeyDown += AssociatedObject_PreviewKeyDown;
        }

        private static RoutedEvent GetClickEvent(DependencyObject element)
        {
            RoutedEvent clickEvent = null;
            for (var type = element.GetType(); type != null && clickEvent == null; type = type.BaseType)
            {
                var routedEvents = EventManager.GetRoutedEventsForOwner(type);
                if (routedEvents != null)
                {
                    clickEvent = routedEvents.FirstOrDefault(re => re.Name == "Click");
                }
            }
            return clickEvent;
        }

        private void RaiseClickEvent()
        {
            var eventArgs = new RoutedEventArgs(_clickEvent, Target);
            var uiElement = Target as UIElement;
            if (uiElement != null)
            {
                if (uiElement.IsEnabled) uiElement.RaiseEvent(eventArgs);
            }
            else
            {
                var contentElement = Target as ContentElement;
                if (contentElement != null && contentElement.IsEnabled) contentElement.RaiseEvent(eventArgs);
            }
        }

        private void RemoveHandlers()
        {
            AssociatedObject.PreviewKeyDown -= AssociatedObject_PreviewKeyDown;
        }
    }
}