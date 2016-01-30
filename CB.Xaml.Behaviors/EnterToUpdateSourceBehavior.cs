using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interactivity;


namespace CB.Xaml.Behaviors
{
    public class EnterToUpdateSourceBehavior: Behavior<UIElement>
    {
        #region  Properties & Indexers
        public DependencyProperty BoundProperty { get; set; }
        #endregion


        #region Override
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
        private void AssociatedObject_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            BindingExpression expression;
            if (BoundProperty == null ||
                (expression = BindingOperations.GetBindingExpression(AssociatedObject, BoundProperty)) == null) return;

            expression.UpdateSource();
        }
        #endregion


        #region Implementation
        private void AddHandlers()
        {
            AssociatedObject.PreviewKeyDown += AssociatedObject_PreviewKeyDown;
        }

        private void RemoveHandlers()
        {
            AssociatedObject.PreviewKeyDown -= AssociatedObject_PreviewKeyDown;
        }
        #endregion
    }
}