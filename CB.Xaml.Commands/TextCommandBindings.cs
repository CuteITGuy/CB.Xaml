using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using CB.Data;


namespace CB.Xaml.Commands
{
    public class TextCommandBindings
    {
        #region Fields
        private static readonly Dictionary<Key<TextBox, TextCommandType>, CommandBinding> commandBindingDictionary =
            new Dictionary<Key<TextBox, TextCommandType>, CommandBinding>();
        #endregion


        #region Methods
        public static CommandBinding CreateCommandBinding(TextBox textBox, TextCommandType textCommandType)
        {
            var key = new Key<TextBox, TextCommandType>(textBox, textCommandType);
            if (commandBindingDictionary.ContainsKey(key))
            {
                return commandBindingDictionary[key];
            }

            var commandBinding = new CommandBinding(GetCommand(textCommandType),
                GetExecuteEventHandler(textBox, textCommandType),
                GetCanExecuteEventHandler(textBox, textCommandType));
            commandBindingDictionary[key] = commandBinding;
            return commandBinding;
        }
        #endregion


        #region Implementation
        private static CanExecuteRoutedEventHandler GetCanExecuteEventHandler(TextBox textBox,
            TextCommandType textCommandType)
        {
            switch (textCommandType)
            {
                case TextCommandType.TrimAll:
                case TextCommandType.TrimEnd:
                case TextCommandType.TrimStart:
                    return
                        delegate(object sender, CanExecuteRoutedEventArgs e)
                        {
                            e.CanExecute = !string.IsNullOrEmpty(textBox.Text);
                        };

                default:
                    throw new ArgumentOutOfRangeException("textCommandType", textCommandType, null);
            }
        }

        private static RoutedUICommand GetCommand(TextCommandType textCommandType)
        {
            switch (textCommandType)
            {
                case TextCommandType.TrimAll:
                    return TextCommands.Trim;

                case TextCommandType.TrimEnd:
                    return TextCommands.TrimEnd;

                case TextCommandType.TrimStart:
                    return TextCommands.TrimStart;

                default:
                    throw new ArgumentOutOfRangeException("textCommandType", textCommandType, null);
            }
        }

        private static ExecutedRoutedEventHandler GetExecuteEventHandler(TextBox textBox,
            TextCommandType textCommandType)
        {
            switch (textCommandType)
            {
                case TextCommandType.TrimAll:
                    return
                        delegate { textBox.Text = TextProcessor.TrimAllLines(textBox.Text, TrimmingPosition.TrimAll); };

                case TextCommandType.TrimEnd:
                    return
                        delegate { textBox.Text = TextProcessor.TrimAllLines(textBox.Text, TrimmingPosition.TrimEnd); };

                case TextCommandType.TrimStart:
                    return
                        delegate
                        {
                            textBox.Text = TextProcessor.TrimAllLines(textBox.Text, TrimmingPosition.TrimStart);
                        };
                default:
                    throw new ArgumentOutOfRangeException("textCommandType", textCommandType, null);
            }
        }
        #endregion
    }
}