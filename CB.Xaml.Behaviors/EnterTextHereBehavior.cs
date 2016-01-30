using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;


namespace CB.Xaml.Behaviors
{
    public class EnterTextHereBehavior: Behavior<TextBox>
    {
        #region  Fields
        private const string DEFAULT_STRING = "[Enter text here]";
        private Brush _normalBackground;
        private FontFamily _normalFontFamily;
        private double _normalFontSize;
        private FontStretch _normalFontStretch;
        private FontStyle _normalFontStyle;
        private FontWeight _normalFontWeight;
        private Brush _normalForeground;
        private bool _showing;
        #endregion


        #region  Properties & Indexers
        public Brush Background { get; set; } = Brushes.Bisque;

        public FontFamily FontFamily { get; set; } = new FontFamily("Arial");

        public double FontSize { get; set; } = 13.0;

        public FontStretch FontStretch { get; set; } = FontStretches.Medium;

        public FontStyle FontStyle { get; set; } = FontStyles.Italic;

        public FontWeight FontWeight { get; set; } = FontWeights.Light;

        public Brush Foreground { get; set; } = Brushes.LightSlateGray;

        public string Text { get; set; } = DEFAULT_STRING;
        #endregion


        #region Event Handlers
        private void AssociatedObject_FocusChanged(object sender, RoutedEventArgs e)
        {
            ChooseState();
        }

        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            ChooseState();
        }

        private void AssociatedObject_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChooseState();
        }
        #endregion


        #region Implementation
        private void AddHandlers()
        {
            AssociatedObject.GotFocus += AssociatedObject_FocusChanged;
            AssociatedObject.LostFocus += AssociatedObject_FocusChanged;
            AssociatedObject.Loaded += AssociatedObject_Loaded;
            AssociatedObject.TextChanged += AssociatedObject_TextChanged;
        }

        private void ChooseState()
        {
            if (_showing && (AssociatedObject.IsFocused || AssociatedObject.Text != Text))
            {
                RestoreState();
            }
            else if (!_showing && !AssociatedObject.IsFocused && string.IsNullOrEmpty(AssociatedObject.Text))
            {
                SaveState();
                SetState();
            }
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

        private void RemoveHandlers()
        {
            AssociatedObject.GotFocus -= AssociatedObject_FocusChanged;
            AssociatedObject.LostFocus -= AssociatedObject_FocusChanged;
            AssociatedObject.Loaded -= AssociatedObject_Loaded;
            AssociatedObject.TextChanged -= AssociatedObject_TextChanged;
        }

        private void RestoreState()
        {
            AssociatedObject.FontSize = _normalFontSize;
            AssociatedObject.FontFamily = _normalFontFamily;
            AssociatedObject.FontStretch = _normalFontStretch;
            AssociatedObject.FontStyle = _normalFontStyle;
            AssociatedObject.FontWeight = _normalFontWeight;
            AssociatedObject.Background = _normalBackground;
            AssociatedObject.Foreground = _normalForeground;
            if (AssociatedObject.Text == Text)
            {
                AssociatedObject.Text = "";
            }
            _showing = false;
        }

        private void SaveState()
        {
            _normalFontSize = AssociatedObject.FontSize;
            _normalFontFamily = AssociatedObject.FontFamily;
            _normalFontStretch = AssociatedObject.FontStretch;
            _normalFontStyle = AssociatedObject.FontStyle;
            _normalFontWeight = AssociatedObject.FontWeight;
            _normalBackground = AssociatedObject.Background;
            _normalForeground = AssociatedObject.Foreground;
        }

        private void SetState()
        {
            AssociatedObject.FontSize = FontSize;
            AssociatedObject.FontFamily = FontFamily;
            AssociatedObject.FontStretch = FontStretch;
            AssociatedObject.FontStyle = FontStyle;
            AssociatedObject.FontWeight = FontWeight;
            AssociatedObject.Background = Background;
            AssociatedObject.Foreground = Foreground;
            AssociatedObject.Text = Text;
            _showing = true;
        }
        #endregion
    }
}