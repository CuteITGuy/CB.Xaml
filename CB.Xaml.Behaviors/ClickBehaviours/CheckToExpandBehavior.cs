using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;


namespace CB.Xaml.Behaviors.ClickBehaviours
{
    public class CheckToExpandBehavior: Behavior<ToggleButton>
    {
        #region  Properties & Indexers
        public UIElement Target { get; set; }
        public string TextWhenCollapsing { get; set; }
        public string TextWhenExpanding { get; set; }
        #endregion


        #region Override
        protected override void OnAttached()
        {
            base.OnAttached();
            ExpandOrCollapse();
            AttachHandlers();
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            DetachHandlers();
        }
        #endregion


        #region Event Handlers
        private void AssociatedObject_Checked(object sender, RoutedEventArgs e)
        {
            ExpandOrCollapse();
        }

        private void AssociatedObject_Unchecked(object sender, RoutedEventArgs e)
        {
            ExpandOrCollapse();
        }
        #endregion


        #region Implementation
        private void AttachHandlers()
        {
            AssociatedObject.Checked += AssociatedObject_Checked;
            AssociatedObject.Unchecked += AssociatedObject_Unchecked;
        }

        private void DetachHandlers()
        {
            AssociatedObject.Checked -= AssociatedObject_Checked;
            AssociatedObject.Unchecked -= AssociatedObject_Unchecked;
        }

        private void ExpandOrCollapse()
        {
            if (Target != null)
            {
                Target.Visibility = AssociatedObject.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
                SetText();
            }
        }

        private void SetText()
        {
            if (AssociatedObject.IsChecked == true && !string.IsNullOrEmpty(TextWhenExpanding))
            {
                AssociatedObject.Content = TextWhenExpanding;
            }
            else if (AssociatedObject.IsChecked != true && !string.IsNullOrEmpty(TextWhenCollapsing))
            {
                AssociatedObject.Content = TextWhenCollapsing;
            }
        }
        #endregion
    }
}