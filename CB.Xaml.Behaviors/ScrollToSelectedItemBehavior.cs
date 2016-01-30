using System.Windows.Controls;
using System.Windows.Interactivity;


namespace CB.Xaml.Behaviors
{
    public class ScrollToSelectedItemBehavior : Behavior<ListBox>
    {
        #region Overridden Methods
        protected override void OnAttached()
        {
            base.OnAttached();
            AddHandlers();
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            RemoveHandlers();
        }
        #endregion


        #region Event Handlers
        private void AssociatedObject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = AssociatedObject.SelectedItem;
            AssociatedObject.ScrollIntoView(selectedItem);
            /*if (selectedItem==null)
            {
                AssociatedObject.sc
            }
            else
            {
                
            }*/
        }
        #endregion


        #region Implementation
        private void AddHandlers()
        {
            AssociatedObject.SelectionChanged += AssociatedObject_SelectionChanged;
        }

        private void RemoveHandlers()
        {
            AssociatedObject.SelectionChanged -= AssociatedObject_SelectionChanged;
        }
        #endregion
    }
}