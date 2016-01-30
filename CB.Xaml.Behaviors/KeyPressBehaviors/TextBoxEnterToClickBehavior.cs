using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;


namespace CB.Xaml.Behaviors.KeyPressBehaviors
{
    public class TextBoxEnterToClickBehavior: EnterToClickBehaviorBase<TextBox>
    {
        #region  Properties & Indexers
        public string RegularExpression { get; set; }
        #endregion


        #region Override
        protected override void OnAttached()
        {
            base.OnAttached();
            EnableOrDisableTarget();
            AssociatedObject.TextChanged += AssociatedObject_TextChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.TextChanged -= AssociatedObject_TextChanged;
        }
        #endregion


        #region Event Handlers
        private void AssociatedObject_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableOrDisableTarget();
        }

        private void EnableOrDisableTarget()
        {
            var isEnabled = true;

            if (!string.IsNullOrEmpty(RegularExpression))
            {
                try
                {
                    isEnabled = Regex.IsMatch(AssociatedObject.Text, RegularExpression);
                }
                catch
                {
                    // ignored
                }
            }

            Target.SetValue(UIElement.IsEnabledProperty, isEnabled);
            Target.SetValue(ContentElement.IsEnabledProperty, isEnabled);
            #endregion
        }
    }
}