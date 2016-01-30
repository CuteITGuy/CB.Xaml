#region
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using CB.Wpf.Controls;
using CB.Wpf.Controls.Inpl;

#endregion


namespace CB.Xaml.Behaviors.Impl
{
    public abstract class SuggestTextBehaviorBase : Behavior<TextBox>
    {
        #region Fields
        private TextProcessor textProcessor;
        #endregion


        #region Constructors & Destructors
        protected SuggestTextBehaviorBase()
        {
            InitializeSuggestionPopup();
        }
        #endregion


        #region Properties & Indexers
        public IListPopup SuggestionPopup { get; set; }
        #endregion


        #region Abstract
        protected abstract string[] GetSuggestionSource();
        #endregion


        #region Overridden
        protected override void OnAttached()
        {
            base.OnAttached();
            SuggestionPopup.PlacementTarget = AssociatedObject;
            textProcessor = new TextProcessor(AssociatedObject);
            AddHandlers();
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            SuggestionPopup.PlacementTarget = null;
            textProcessor = new TextProcessor(AssociatedObject);
            RemoveHandlers();
        }
        #endregion


        #region Event Handlers
        private void AssociatedObject_LostFocus(object sender, RoutedEventArgs e)
        {
            SuggestionPopup.IsOpen = false;
        }

        private void AssociatedObject_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (SuggestionPopup.IsOpen)
            {
                PreviewKeyDownWhenPopupOpen(e);
            }
            else
            {
                PreviewKeyDownWhenPopupClosed(e);
            }
        }

        private void AssociatedObject_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            SuggestionPopup.IsOpen = false;
        }

        private void AssociatedObject_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textChange = e.Changes.LastOrDefault();
            if (textChange != null)
            {
                TryShowSuggestion(textChange.Offset, false);
            }
        }

        private void SuggestionPopup_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedContent != null)
            {
                textProcessor.InsertSuggestion(e.ClickedContent.ToString());
                HideSuggestion();
            }
        }
        #endregion


        #region Implementation
        protected virtual void AddHandlers()
        {
            AssociatedObject.LostFocus += AssociatedObject_LostFocus;
            AssociatedObject.PreviewMouseDown += AssociatedObject_PreviewMouseDown;
            AssociatedObject.PreviewKeyDown += AssociatedObject_PreviewKeyDown;
            AssociatedObject.TextChanged += AssociatedObject_TextChanged;
        }

        private void ForceShowSuggestion()
        {
            TryShowSuggestion(AssociatedObject.CaretIndex, true);
        }

        private Point GetShowLocation()
        {
            var caretRect = AssociatedObject.GetRectFromCharacterIndex(AssociatedObject.CaretIndex);
            var yDistance = SuggestionPopup.ActualHeight + SuggestionPopup.FontSize;
            return new Point(caretRect.X, caretRect.Y - yDistance);
        }

        private void HideSuggestion()
        {
            SuggestionPopup.IsOpen = false;
        }

        private void InitializeSuggestionPopup()
        {
            SuggestionPopup = new SuggestionPopup();
            SuggestionPopup.ItemClick += SuggestionPopup_ItemClick;
        }

        protected virtual bool IsTextChangeNoticed(TextChange textChange)
        {
            return textChange.AddedLength > 0;
        }

        private void PreviewKeyDownWhenPopupClosed(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Space:
                    if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                    {
                        ForceShowSuggestion();
                        e.Handled = true;
                    }
                    break;
            }
        }

        private void PreviewKeyDownWhenPopupOpen(KeyEventArgs e)
        {
            e.Handled = true;

            switch (e.Key)
            {
                case Key.Down:
                    SuggestionPopup.MoveDownOneItem();
                    break;

                case Key.Up:
                    SuggestionPopup.MoveUpOneItem();
                    break;

                case Key.PageDown:
                    SuggestionPopup.MoveDownOnePage();
                    break;

                case Key.PageUp:
                    SuggestionPopup.MoveUpOnePage();
                    break;

                case Key.Home:
                    SuggestionPopup.MoveToHome();
                    break;

                case Key.End:
                    SuggestionPopup.MoveToEnd();
                    break;

                case Key.Escape:
                    SuggestionPopup.IsOpen = false;
                    break;

                case Key.Tab:
                case Key.Enter:
                    textProcessor.InsertSuggestion(SuggestionPopup.SelectedItem as string);
                    HideSuggestion();
                    break;

                default:
                    e.Handled = false;
                    break;
            }
        }

        protected virtual void RemoveHandlers()
        {
            AssociatedObject.LostFocus -= AssociatedObject_LostFocus;
            AssociatedObject.MouseDown -= AssociatedObject_PreviewMouseDown;
            AssociatedObject.PreviewKeyDown -= AssociatedObject_PreviewKeyDown;
            AssociatedObject.TextChanged -= AssociatedObject_TextChanged;
        }

        protected virtual void ShowSuggestion(IEnumerable<string> suggestion)
        {
            SuggestionPopup.ItemsSource = suggestion;
            SuggestionPopup.SelectedIndex = 0;
            SuggestionPopup.IsOpen = true;
            SuggestionPopup.Location = GetShowLocation();
        }

        private void TryShowSuggestion(int offset, bool useSuggestionSourceIfNotAvail)
        {
            var suggestionSource = GetSuggestionSource();
            if (suggestionSource == null)
            {
                return;
            }

            var suggestion = textProcessor.GetSuggestion(offset, suggestionSource);
            if (suggestion != null)
            {
                ShowSuggestion(suggestion);
            }
            else if (useSuggestionSourceIfNotAvail)
            {
                ShowSuggestion(suggestionSource);
            }
            else
            {
                HideSuggestion();
            }
        }
        #endregion
    }


    /*public abstract class SuggestTextBehaviorBase : Behavior<TextBox>
    {
        #region Fields
        private Word currentWord;
        #endregion


        #region Properties & Indexers
        public IListPopup SuggestionPopup { get; set; }
        #endregion


        #region Constructors & Destructors
        protected SuggestTextBehaviorBase()
        {
            InitializeSuggestionPopup();
        }
        #endregion


        #region Abstract
        protected abstract IEnumerable<string> GetSuggestionSource();
        #endregion


        #region Overridden
        protected override void OnAttached()
        {
            base.OnAttached();
            SuggestionPopup.PlacementTarget = AssociatedObject;
            AddHandlers();
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            SuggestionPopup.PlacementTarget = null;
            RemoveHandlers();
        }
        #endregion


        #region Event Handlers
        private void AssociatedObject_LostFocus(object sender, RoutedEventArgs e)
        {
            SuggestionPopup.IsOpen = false;
        }

        private void AssociatedObject_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (SuggestionPopup.IsOpen)
            {
                PreviewKeyDownWhenPopupOpen(e);
            }
            else
            {
                PreviewKeyDownWhenPopupClosed(e);
            }
        }

        private void AssociatedObject_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            SuggestionPopup.IsOpen = false;
        }

        private void AssociatedObject_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textChange = e.Changes.LastOrDefault();
            if (textChange != null)
            {
                currentWord = GetWordAtOffset(textChange.Offset);
                TryShowSuggestion(false);
            }
        }

        private void SuggestionPopup_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedContent != null)
            {
                AcceptSuggestion(e.ClickedContent.ToString());
            }
        }
        #endregion


        #region Implementation
        private void AcceptSuggestion(string suggestion)
        {
            var text = AssociatedObject.Text;
            var offset = currentWord.StartIndex;
            var newText = text.Remove(offset, currentWord.Content.Length).Insert(offset, suggestion);
            AssociatedObject.Text = newText;
            AssociatedObject.CaretIndex = offset + suggestion.Length;
            SuggestionPopup.IsOpen = false;
        }

        protected virtual void AddHandlers()
        {
            AssociatedObject.LostFocus += AssociatedObject_LostFocus;
            AssociatedObject.PreviewMouseDown += AssociatedObject_PreviewMouseDown;
            AssociatedObject.PreviewKeyDown += AssociatedObject_PreviewKeyDown;
            AssociatedObject.TextChanged += AssociatedObject_TextChanged;
        }

        private static string CreatePatternFromWordContent(string wordContent)
        {
            const string addString = @"\w*";
            var pattern1 = string.Join(addString, wordContent.ToCharArray()) + addString;
            var pattern2 = @"^" + pattern1;
            return pattern2 + "|" + pattern1;
        }

        private static Regex CreateRegexFromWordContent(string wordContent)
        {
            var pattern = CreatePatternFromWordContent(wordContent);

            try
            {
                return new Regex(pattern);
            }
            catch
            {
                return null;
            }
        }

        private void ForceShowSuggestion()
        {
            currentWord = GetWordAtOffset(AssociatedObject.CaretIndex);
            TryShowSuggestion(true);
        }

        private Point GetShowLocation()
        {
            var caretRect = AssociatedObject.GetRectFromCharacterIndex(AssociatedObject.CaretIndex);
            var yDistance = SuggestionPopup.ActualHeight + SuggestionPopup.FontSize;
            return new Point(caretRect.X, caretRect.Y - yDistance);
        }

        protected virtual string[] GetSuggestion(string wordContent, IEnumerable<string> suggestionSource)
        {
            var regex = CreateRegexFromWordContent(wordContent);
            if (regex == null)
            {
                return null;
            }

            var suggestion = suggestionSource.Where(s => regex.IsMatch(s)).ToArray();
            return suggestion.Length < 2 ? null : suggestion;
        }

        protected virtual Word GetWordAtOffset(int offset)
        {
            var text = AssociatedObject.Text;

            / *if (offset >= text.Length || char.IsWhiteSpace(text[offset]))
            {
                return null;
            }* /

            var startIndex = GetWordStartIndex(offset, text);
            var length = GetWordLength(startIndex, text);
            return new Word(startIndex, text.Substring(startIndex, length));
        }

        private static int GetWordLength(int startIndex, string text)
        {
            var endIndex = startIndex;
            while (endIndex < text.Length && !char.IsWhiteSpace(text[endIndex]))
            {
                ++endIndex;
            }
            return endIndex - startIndex;
        }

        private static int GetWordStartIndex(int offset, string text)
        {
            var index = offset >= text.Length ? text.Length - 1 : offset - 1;

            while (index >= 0 && !char.IsWhiteSpace(text[index]))
            {
                --index;
            }

            return index + 1;
        }

        private void HideSuggestion()
        {
            SuggestionPopup.IsOpen = false;
        }

        private void InitializeSuggestionPopup()
        {
            SuggestionPopup = new SuggestionPopup();
            SuggestionPopup.ItemClick += SuggestionPopup_ItemClick;
        }

        protected virtual bool IsTextChangeNoticed(TextChange textChange)
        {
            return textChange.AddedLength > 0;
        }

        protected virtual bool IsValidWord(Word word)
        {
            return word.StartIndex >= 0 && !string.IsNullOrWhiteSpace(word.Content);
        }

        private void PreviewKeyDownWhenPopupClosed(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Space:
                    if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                    {
                        ForceShowSuggestion();
                        e.Handled = true;
                    }
                    break;
            }
        }

        private void PreviewKeyDownWhenPopupOpen(KeyEventArgs e)
        {
            e.Handled = true;

            switch (e.Key)
            {
                case Key.Down:
                    SuggestionPopup.MoveDownOneItem();
                    break;

                case Key.Up:
                    SuggestionPopup.MoveUpOneItem();
                    break;

                case Key.PageDown:
                    SuggestionPopup.MoveDownOnePage();
                    break;

                case Key.PageUp:
                    SuggestionPopup.MoveUpOnePage();
                    break;

                case Key.Home:
                    SuggestionPopup.MoveToHome();
                    break;

                case Key.End:
                    SuggestionPopup.MoveToEnd();
                    break;

                case Key.Escape:
                    SuggestionPopup.IsOpen = false;
                    break;

                case Key.Tab:
                case Key.Enter:
                    AcceptSuggestion(SuggestionPopup.SelectedItem as string);
                    break;

                default:
                    e.Handled = false;
                    break;
            }
        }

        protected virtual void RemoveHandlers()
        {
            AssociatedObject.LostFocus -= AssociatedObject_LostFocus;
            AssociatedObject.MouseDown -= AssociatedObject_PreviewMouseDown;
            AssociatedObject.PreviewKeyDown -= AssociatedObject_PreviewKeyDown;
            AssociatedObject.TextChanged -= AssociatedObject_TextChanged;
        }

        protected virtual void ShowOrHideSuggestion(int offset)
        {
            currentWord = GetWordAtOffset(offset);
            if (currentWord != null && IsValidWord(currentWord))
            {
                var suggestion = GetSuggestion(currentWord.Content, GetSuggestionSource());
                if (suggestion != null)
                {
                    ShowSuggestion(suggestion);
                    return;
                }
            }
            HideSuggestion();
        }

        protected virtual void ShowSuggestion(IEnumerable<string> suggestion)
        {
            SuggestionPopup.ItemsSource = suggestion;
            SuggestionPopup.SelectedIndex = 0;
            ShowSuggestion();
        }

        private void ShowSuggestion()
        {
            SuggestionPopup.IsOpen = true;
            SuggestionPopup.Location = GetShowLocation();
        }

        private void TryShowSuggestion(bool useSuggestionSourceIfNotAvail)
        {
            var suggestionSource = GetSuggestionSource();
            if (suggestionSource == null)
            {
                return;
            }
            var sourceArray = suggestionSource as string[] ?? suggestionSource.ToArray();

            if (currentWord != null && IsValidWord(currentWord))
            {
                var suggestion = GetSuggestion(currentWord.Content, sourceArray);
                if (suggestion != null)
                {
                    ShowSuggestion(suggestion);
                    return;
                }
            }

            if (useSuggestionSourceIfNotAvail)
            {
                ShowSuggestion(sourceArray);
            }
            else
            {
                HideSuggestion();
            }
        }
        #endregion
    }*/
}