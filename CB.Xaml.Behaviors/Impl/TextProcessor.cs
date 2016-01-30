using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Controls;


namespace CB.Xaml.Behaviors.Impl
{
    public class TextProcessor
    {
        #region Fields
        private readonly TextBox targetTextBox;
        private string content;
        private int startIndex;
        #endregion


        #region Constructors & Destructors
        public TextProcessor(TextBox targetTextBox)
        {
            this.targetTextBox = targetTextBox;
        }
        #endregion


        #region Methods
        public string[] GetSuggestion(int offset, IEnumerable<string> suggestionSource)
        {
            SetOffset(offset);

            var regex = CreateRegex();
            if (regex == null)
            {
                return null;
            }

            var suggestion = suggestionSource.Where(s => regex.IsMatch(s)).ToArray();
            return suggestion.Length < 2 ? null : suggestion;
        }

        public void InsertSuggestion(string suggestion)
        {
            var offset = startIndex;
            var newText = targetTextBox.Text.Remove(offset, content.Length).Insert(offset, suggestion);
            targetTextBox.Text = newText;
            targetTextBox.CaretIndex = offset + suggestion.Length;
        }
        #endregion


        #region Implementation
        private Regex CreateRegex()
        {
            var pattern = CreateRegexPattern();

            try
            {
                return new Regex(pattern);
            }
            catch
            {
                return null;
            }
        }

        private string CreateRegexPattern()
        {
            const string addString = @"\w*";
            var pattern1 = string.Join(addString, content.ToCharArray()) + addString;
            var pattern2 = @"^" + pattern1;
            return pattern2 + "|" + pattern1;
        }

        private int GetWordLength()
        {
            var text = targetTextBox.Text;
            var endIndex = startIndex;

            while (endIndex < text.Length && !char.IsWhiteSpace(text[endIndex]))
            {
                ++endIndex;
            }
            return endIndex - startIndex;
        }

        private int GetWordStartIndex(int offset)
        {
            var text = targetTextBox.Text;
            var index = offset >= text.Length ? text.Length - 1 : offset - 1;

            while (index >= 0 && !char.IsWhiteSpace(text[index]))
            {
                --index;
            }
            return index + 1;
        }

        private void SetOffset(int offset)
        {
            var text = targetTextBox.Text;
            startIndex = GetWordStartIndex(offset);
            var length = GetWordLength();
            content = text.Substring(startIndex, length);
        }
        #endregion
    }
}