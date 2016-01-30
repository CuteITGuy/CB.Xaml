using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;


namespace CB.Xaml.Behaviors
{
    public class TextBoxTabSizeBehavior: Behavior<TextBox>
    {
        #region Fields
        private int _tabSize = 4;
        #endregion


        #region  Properties & Indexers
        public int TabSize
        {
            get { return _tabSize; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("TabSize must be greater than 0.");
                }
                _tabSize = value;
            }
        }

        public TabStyle TabStyle { get; set; } = TabStyle.Fixed;
        #endregion


        #region Event Handlers
        private void AssociatedObject_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Tab:
                    OnPreviewTabKeyDown();
                    e.Handled = true;
                    break;
            }
        }
        #endregion


        #region Implementation
        private void AddHandlers()
        {
            AssociatedObject.PreviewKeyDown += AssociatedObject_PreviewKeyDown;
        }

        private int GetTabDifference()
        {
            return Keyboard.Modifiers == ModifierKeys.Shift ? -TabSize : TabSize;
        }

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

        private void OnPreviewTabKeyDown()
        {
            var tabDiff = GetTabDifference();
            switch (TabStyle)
            {
                case TabStyle.Fixed:
                    SetNextFixedTab(tabDiff);
                    break;

                case TabStyle.Flow:
                    SetNextFlowTab(tabDiff);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void RemoveHandlers()
        {
            AssociatedObject.PreviewKeyDown -= AssociatedObject_PreviewKeyDown;
        }

        private void SetNextFixedTab(int tabDiff)
        {
            if (Keyboard.Modifiers == ModifierKeys.Shift)
            {
                throw new NotImplementedException();
            }
            else
            {
                var indexDiff = TabSize - AssociatedObject.CaretIndex % TabSize;
                var nextCaretIndex = AssociatedObject.CaretIndex + indexDiff;
                AssociatedObject.Text = AssociatedObject.Text.Insert(
                    AssociatedObject.CaretIndex, new string(' ', indexDiff));
                AssociatedObject.CaretIndex = nextCaretIndex;
            }
        }

        private void SetNextFlowTab(int tabDiff)
        {
            if (Keyboard.Modifiers == ModifierKeys.Shift)
            {
                throw new NotImplementedException();
            }
            else
            {
                var nextCaretIndex = AssociatedObject.CaretIndex + TabSize;
                AssociatedObject.Text = AssociatedObject.Text.Insert(
                    AssociatedObject.CaretIndex, new string(' ', TabSize));
                AssociatedObject.CaretIndex = nextCaretIndex;
            }
        }
        #endregion
    }
}