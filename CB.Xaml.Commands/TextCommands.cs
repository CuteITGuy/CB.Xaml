using System.Windows.Input;


namespace CB.Xaml.Commands
{
    public class TextCommands
    {
        #region  Constructors & Destructors
        static TextCommands()
        {
            Trim = InitializeCommand("Trim", "Trim", ModifierKeys.Control, "Ctrl+T,R", Key.T, Key.R);
            TrimEnd = InitializeCommand("Trim End", "TrimEnd", ModifierKeys.Control, "Ctrl+T,E", Key.T, Key.E);
            TrimStart = InitializeCommand("Trim Start", "TrimStart", ModifierKeys.Control, "Ctrl+T,S", Key.T, Key.S);
        }
        #endregion


        #region  Properties & Indexers
        public static RoutedUICommand Trim { get; }

        public static RoutedUICommand TrimEnd { get; }

        public static RoutedUICommand TrimStart { get; }
        #endregion


        #region Implementation
        private static RoutedUICommand InitializeCommand(
            string text, string name, ModifierKeys modifiers,
            string displayString,
            params Key[] keys)
        {
            var gestures = new InputGestureCollection
            {
                new MultiKeyGesture(modifiers, displayString, keys)
            };
            return new RoutedUICommand(text, name, typeof(TextCommands), gestures);
        }
        #endregion
    }
}